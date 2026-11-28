# ملكية الحالة والاستعادة — حدود Domain/Core

**الحالة: INVENTORY / NO PERSISTENCE OR RECOVERY CONTRACT**  
**الفرع:** security/core-trust-boundaries-phase0  
**الغرض:** تحديد من يملك الحالة، وأين تعيش، وما الذي يمكن استعادته بعد انتهاء العملية، قبل اختيار قاعدة بيانات أو إضافة طبقة persistence.

## 1. النتيجة التنفيذية

- `CostOfStaying.Domain` يملك gameplay aggregate للمباراة واللاعبين، لكنه يعمل داخل الذاكرة ولا يقدم عقداً عاماً لـsnapshot/restore أو repository أو serialization.
- `CostOfStaying.Core` يملك primitives للجلسات والقبول والتسلسل والحسابات والسجل، لكنها أيضاً in-memory. `CoreComposition` يربط الموارد بنطاق MatchId ولا يحولها إلى durable storage.
- لا يوجد حالياً adapter موثق يربط Domain بـCore، ولا host/network runtime أو persistence boundary يحدد transaction أو recovery behavior.
- إسقاط العملية يعني فقدان حالة Domain/Core الموجودة في الذاكرة. لا يوجد في المصدر الحالي ضمان استعادة المباراة أو الجلسة أو replay state بعد restart.
- لا نوصي بإضافة EF Core/SQLite/Redis أو مزود backend في هذه المرحلة؛ اختيار التقنية قبل تعريف recovery, consistency, retention, privacy, and ownership requirements سيخلق عقداً غير معتمد.

## 2. خريطة ملكية الحالة

| الحالة | المالك الحالي | نطاقها وعمرها | ما هو مثبت | ما لا يضمنه التنفيذ |
|---|---|---|---|---|
| Match lifecycle, Outcome, WinnerId, StartedAt, vote state | Domain `Match` | كائن Match داخل العملية | methods المجال تنفذ انتقالات الحالة | لا snapshot/restore ولا استعادة بعد restart |
| Player vitality, lifecycle, debuffs, Spirit counters | Domain `Player` المملوك من Match المقصود | عمر كائن اللاعب/المباراة | scalar setters داخلية ومسارات انتقال Domain | لا persistence ولا concurrency isolation؛ collections العامة ما زالت قابلة للتعديل كما في D-03 |
| Domain Events | `Match.Events` | عمر Match في الذاكرة | ترتيب الإضافة داخل القائمة | ليست durable audit log؛ لا replay بعد restart ولا retention policy |
| Transfer dedupe | `Match.ProcessedRequestIds` | عمر Match في الذاكرة | منع تكرار الطلبات الناجحة داخل المجموعة الحالية | لا idempotency عبر restart أو reconnect؛ لا نافذة احتفاظ معتمدة |
| Echo dedupe | `Player.CollectedEchoIds` | عمر Player في الذاكرة | منع التكرار داخل المجموعة الحالية | لا استعادة أو retention policy |
| Session registry and connection mappings | Core registries | عمر registry instance | capacities وعمليات detach/cleanup محلية | لا durable session store ولا reconnect recovery |
| Command queue and sequence tracker | Core queue/tracker | عمر queue/session tracking | bounded queue and per-session sequence checks | لا durable queue أو sequence continuity بعد restart |
| Match player registry / VitalityAccount / lifecycle | Core composition-scoped | composition واحدة لكل MatchId | MatchId isolation ورفض خلط الموارد بين المباريات | لا durable player state أو mapping إلى Domain PlayerId |
| Betrayal ledger events/scores | Core `BetrayalLedger` | عمر ledger/composition | local event sequence and exact replay checks | لا persistence أو history retention؛ F-002 writer handler غير مكتمل |

## 3. حدود التنفيذ الفعلية

### Domain
- `Match` يحتفظ بقوائم/مجموعات الحالة مباشرة، ويضيف DomainEvent إلى `Events` من خلال `Emit`.
- `Player` يحتفظ بـDebuffs وCollectedEchoIds داخل الذاكرة.
- `MatchConfig` يأخذ snapshot validated عند إنشاء Match، لكن ذلك لا يمثل snapshot لحالة المباراة.
- لا توجد في مشروع Domain واجهة persistence أو serialization/restore ضمن الملفات التي تمت مراجعتها.
- لا توجد آلية lock أو actor scheduler داخل Match؛ لا نستنتج thread-safety من وجود aggregate. يجب أن يحدد host single-writer/serialization قبل تعريضه للتزامن.

### Core
- `CoreComposition.Create(MatchId)` ينشئ composition scoped؛ هذا يعزل النطاق ولا يحفظه.
- `SessionRegistry`, `ConnectionSessionRegistry`, `MatchPlayerRegistry`, `CommandAdmissionQueue`, `CommandSequenceTracker` و`BetrayalLedger` موارد in-memory.
- توثيق CoreComposition يحدد mutation على أنها single-writer/server-serialized؛ host مسؤول عن فرض ذلك.
- لا يوجد repository/provider interface أو durable transaction boundary في Core الحالي.

### التكامل
- `CostOfStaying.Domain.csproj` و`CostOfStaying.Core.csproj` يستهدفان netstandard2.1؛ لا توجد ProjectReference تربط المشروعين.
- لا يوجد adapter يربط Domain `Guid` identities مع Core `MatchId`/`ActorId`، ولا adapter للساعة أو Vitality أو event sequencing.
- لا يوجد host أو transport أو Unity/FishNet integration منشور يحدد نقطة كتابة durable أو ترتيب commit بين state وevents.

## 4. مخاطر الاستعادة التي يجب حسمها

1. **Crash consistency:** هل يجب أن تكون آخر عملية gameplay ناجحة durable قبل إرسال acknowledgement؟ ما RPO المقبول؟
2. **Atomicity:** كيف ترتبط mutation في Match مع DomainEvent وdedupe ID وCore ledger event في معاملة واحدة؟ لا يوجد transaction مشترك حالياً.
3. **Replay:** هل المطلوب event sourcing كامل، أم snapshots دورية مع event tail، أم حفظ نتيجة المباراة فقط؟
4. **Reconnect:** هل reconnect يعيد نفس Actor/Match state، أم ينشئ session جديدة مع استمرار المباراة؟ ما نافذة الاستئناف؟
5. **Idempotency:** هل يجب أن يظل RequestId فعالاً بعد restart؟ وما نطاقه: match أو account أو lifetime؟
6. **Concurrency:** هل لكل Match writer واحد؟ كيف تمنع أوامر متزامنة من تعديل aggregate أو stores في الوقت نفسه؟
7. **Retention/privacy:** ما الذي يحتفظ به، ولمدة كم، ومن يستطيع قراءته أو حذفه؟ لا تسجل hint text أو معرفات حساسة في telemetry بلا سياسة.
8. **Schema evolution:** كيف تهاجر snapshots/events بين إصدارات Domain/Core من دون كسر replay أو restore؟

هذه ليست متطلبات مفترضة؛ إنها أسئلة قرار قبل تصميم الاستعادة. لا يمكن استنتاج إجاباتها من نوع البيانات أو الاختبارات الحالية.

## 5. تصميم مرحلي مقترح بعد اعتماد المتطلبات

### المرحلة A — تعريف العقد
- اعتماد canonical MatchId/ActorId mapping وDomain identity strategy.
- تحديد match lifecycle عند process crash، reconnect, restart, and completion.
- اختيار consistency model وRPO/RTO، وتحديد ما إذا كانت النتيجة النهائية وحدها كافية.
- تعريف atomic boundary بين state transition وevent/dedupe/acknowledgement.

### المرحلة B — اختيار نمط persistence
- **Snapshot + event tail** إذا كانت استعادة الحالة مطلوبة مع سجل أحداث محدود؛ يجب أن يتضمن snapshot version وsequence watermark.
- **Append-only event log** إذا كان replay والتدقيق الكامل مطلوبين؛ يتطلب event schema/versioning وidempotent projection.
- **Final-result persistence** إذا لم تكن استعادة المباراة الحية مطلوبة؛ أبسط وأقل كلفة لكنه لا يدعم resume.
- لا تختار نمطاً هجيناً قبل تحديد مصدر الحقيقة وtransaction boundary.

### المرحلة C — تنفيذ واختبار
- إضافة adapter/ports في composition root بدلاً من ربط Domain مباشرة بمزود تخزين.
- اختبار crash بين mutation وevent write وack، وduplicate after restart، وrestore from snapshot + tail، وترقية schema، وmulti-match isolation.
- قياس حجم state/events وlatency وwrite amplification قبل اختيار قاعدة البيانات أو cache.
- ربط أي retention/pruning بالـsequence watermark وreplay window المعتمدة، لا بمؤقت حذف عام.

## 6. قرارات تمنع التنفيذ الآن

| القرار | الحالة | سبب الحجب |
|---|---|---|
| Canonical identity + mapping | D-07 / OPEN_OWNER_DECISION | Domain Guid وCore typed ulong منفصلان ولا يوجد resolver معتمد |
| Match restart/resume lifecycle | OPEN_OWNER_DECISION | لا توجد دلالات reconnect/restart معتمدة |
| RPO/RTO and durability | OPEN_OWNER_DECISION | لا يوجد هدف استعادة أو ضمان commit محدد |
| Event replay and retention | D-04 / OPEN_OWNER_DECISION | حذف/تقليص الأحداث قد يغير replay/idempotency |
| Privacy and deletion lifecycle | D-7 / OPEN_OWNER_DECISION | لا توجد سياسة احتفاظ/وصول/حذف معتمدة |
| Single-writer host contract | HOST RESPONSIBILITY / NOT IMPLEMENTED | Core docs تفترض server-serialized mutation، لكن host runtime غير موجود |

## 7. مصادر الكود

- `src/CostOfStaying.Domain/Match.cs`
- `src/CostOfStaying.Domain/Player.cs`
- `src/CostOfStaying.Domain/MatchConfig.cs`
- `src/CostOfStaying.Domain/CostOfStaying.Domain.csproj`
- `src/CostOfStaying.Core/CoreComposition.cs`
- `src/CostOfStaying.Core/CostOfStaying.Core.csproj`
- `docs/architecture/DOMAIN_CORE.md`
- `docs/security/STATE_RETENTION_INVENTORY.md`
- `docs/governance/OPEN_DECISION_REGISTER.md`

**الخلاصة:** فصل Domain/Core الحالي مفيد كحدود مسؤولية، لكنه ليس نظام استعادة. Core match scoping يمنع خلط الموارد داخل العملية؛ ولا يساوي persistence. يجب تثبيت الهوية ودورة الحياة والـconsistency/retention أولاً، ثم اختيار adapter ونمط التخزين واختباره.