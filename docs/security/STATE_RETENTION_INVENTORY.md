# جرد حدود الذاكرة والاحتفاظ — Domain/Core

**الحالة: INVENTORY / NO RETENTION POLICY APPROVED**  
**النطاق:** مخازن الحالة والـcollections في `src/CostOfStaying.Domain` و`src/CostOfStaying.Core`.  
**الهدف:** تحديد ما هو bounded فعلياً، وما يعتمد على عمر المباراة أو دلالات replay، دون فرض eviction أو تغيير سلوك اللعبة.

## 1. ملخص تنفيذي

- توجد مخازن محددة السعة في Core، لكن سعتها تأتي من المضيف كقيم موجبة وقت الإنشاء؛ لا يوجد global active-match cap أو حد تشغيلي أعلى موحد لهذه القيم.
- توجد مخازن Domain وCore غير محددة السعة. أهمها Domain event log وtransfer request dedupe وecho dedupe وCore BetrayalLedger event history.
- بعض collections مكشوفة كـpublic mutable collections؛ لذلك حدود API مثل `AddPlayer` و`HasDebuffCapacity` لا تضمن السعة إذا عدّل مستهلك خارجي الـcollection مباشرة. هذه أيضاً فجوة encapsulation مسجلة ضمن D-03.
- لا توجد سياسة persistence/snapshot/restore أو retention/TTL موحدة.
- **لا تنفذ تقليماً أو eviction أو cap اعتباطياً الآن.** بعض المخازن جزء من exact replay أو deduplication؛ حذف عناصرها يغير دلالات السلوك.

## 2. مخازن Core

| المخزن | scope / lifetime | الحد الحالي | دلالات الاحتفاظ والمخاطر |
|---|---|---|---|
| `BetrayalLedger._events` | instance واحد من CoreComposition للمباراة | غير محدود | يحتفظ بالأحداث حسب EventSequenceId لتمييز exact replay من إعادة استخدام sequence ببيانات مختلفة، ويفرض sequence high-water mark. حذف السجلات القديمة قد يغير replay semantics. |
| `BetrayalLedger._scores` | ledger instance | غير محدود بعدد ActorIds | لا يوجد roster membership check داخل ledger؛ أول تسجيل لممثل جديد ينشئ entry. الكتابة internal حالياً ولا يوجد Trusted Gameplay Handler إنتاجي. |
| `SessionRegistry._sessions` | registry instance | `capacity` موجب يمرره المضيف | bounded لكل instance، لكن لا يوجد upper bound موحد أو سياسة per-Actor multi-session في Core. |
| `ConnectionSessionRegistry` maps | registry instance | `capacity` موجب يمرره المضيف | map باتجاهين؛ bounded بالسعة المقدمة، ولا يمثل transport adapter أو persistence. |
| `MatchPlayerRegistry._accounts/_orderedIds` | Match-scoped | `capacity` موجب يمرره المضيف | bounded لكل مباراة، مع رفض الحساب ذي MatchId المختلف. |
| `CommandAdmissionQueue._pending` | Match-scoped queue | `capacity` موجب يمرره المضيف | bounded؛ full queue يرفض دون استهلاك sequence وفق الاختبار. |
| `CommandSequenceTracker._lastAccepted` | داخل queue | `maximumTrackedSessions` موجب يمرره المضيف | bounded بعدد الجلسات المتتبعة؛ لا persistence أو replay عبر restart. |
| `MatchRosterSnapshot` | snapshot | حجم نسخة roster المقدمة | لا يحتفظ بتاريخ أحداث؛ حجمها يتبع roster builder/capacity. |

**ملاحظة:** الحدود الموجبة تمنع النمو غير المحدود داخل instance بعد بلوغ القيمة الممررة، لكنها لا تضمن قيمة تشغيلية آمنة إذا أنشأ المضيف instance بسعة مفرطة. كما لا توجد global active-match cap تمنع إنشاء عدد غير محدود من compositions.

## 3. مخازن Domain

| المخزن | موضعه | الحد الحالي | دلالات الاحتفاظ والمخاطر |
|---|---|---|---|
| `Match.Events` | Match | غير محدود | سجل مرتب لكل Domain events، ويشمل أحداث اللعب والرفض والتصويت وEcho/Hint. `Emit` يضيف دائماً؛ لا archive أو pruning أو persistence. |
| `Match.ProcessedRequestIds` | Match | غير محدود | HashSet لمعرفات transfer الناجحة لمنع تكرار mutation. لا TTL أو max length؛ تقليمه يغير نافذة idempotency. |
| `Player.CollectedEchoIds` | Player | غير محدود | dedupe لمعرفات Echo التي جمعها اللاعب؛ لا حد أو سياسة انتهاء. |
| `Player.Debuffs` | Player | `MaxActiveDebuffs` في مساري collapse وtransfer | `HasDebuffCapacity` يفحص الحد عند إضافة debuff في `ApplyCollapse` وعند النقل إلى الهدف، لكن List عامة قابلة للتعديل مباشرة. قيمة config غير سالبة ولا يوجد hard upper bound. |
| `Match.Players` | Match | `PlayerCount` عبر `AddPlayer` فقط | Dictionary عامة قابلة للتعديل مباشرة؛ يمكن تجاوز فحص `PlayerCount` خارج API المقصودة. config يشترط قيمة موجبة فقط ولا يفرض سقفاً عالمياً. |
| `Match.Votes` | Match / vote lifecycle | roster snapshot عند `OpenVote` عبر المسار المقصود | يتم ملؤها من اللاعبين Alive وتُمسح عند فشل التصويت؛ لكنها Dictionary عامة قابلة للتعديل مباشرة. |
| `DomainEvent.Detail` | كل event | لا يوجد حد طول | يمكن أن يحمل requestId أو echoId أو hintText أو تفاصيل مشتقة من caller. لا يوجد max length داخل Domain. |

### مصادر نمو متكررة

- `TryTransfer` يضيف rejection event عند تكرار requestId؛ الطلب المكرر لا يكرر mutation، لكنه قد يضيف حدث رفض في كل استدعاء.
- `TryTransfer` يضيف requestId إلى `ProcessedRequestIds` بعد نجاح النقل فقط.
- `CollectEcho` يضيف echoId إلى `CollectedEchoIds` وإلى `Events`.
- `TrySendHint` يضيف `HintSent` event مع النص؛ لا يوجد حد طول للنص أو rate limit عام داخل Domain، عدا cooldown الخاص باللاعب.
- `CastVote` يضيف `VoteCast` event عند كل تصويت مقبول.
- `Tick` وعمليات collapse/end-state قد تضيف أحداثاً إضافية بحسب الزمن والمدخلات.

هذه الملاحظات تصف Domain library الحالية فقط. لا يوجد حالياً transport/host منشور يثبت أن هذه methods مكشوفة مباشرة لمستخدم غير موثوق؛ لذلك لا نصنفها كاستغلال شبكي قائم. إذا أضيف host لاحقاً، يجب أن يفرض raw payload size، string length، rate limits، monotonic timestamps، ومدة/عدد استدعاءات قبل وصول البيانات إلى Domain.

## 4. قرارات لازمة قبل تصميم retention

القيم التالية ما زالت `UNKNOWN / OPEN_OWNER_DECISION`:

1. الحد الأدنى والأقصى للاعبين في المباراة، والحد العالمي للمباريات النشطة.
2. مدة المباراة القصوى التشغيلية، وما يحدث للموارد بعد Completed.
3. سياسة disconnect/reconnect ونافذة استئناف المباراة.
4. هل تستمر المباراة بعد restart؟ وإذا نعم، ما RPO/RTO وsnapshot consistency المطلوبة؟
5. معدل/عدد أحداث Betrayal وDomain Events المتوقع في مباراة.
6. هل يحتاج المنتج إلى event history كاملة بعد نهاية المباراة، أم نتيجة نهائية/ملخص فقط؟
7. مدة الاحتفاظ بنتائج المباريات والأحداث والـsecurity telemetry، ومن يحق له الوصول والحذف.
8. هل يجب أن تمنع dedupe stores إعادة الطلب بعد انتهاء المباراة أو بعد restart، وما نافذة idempotency المطلوبة؟

## 5. مراحل معالجة آمنة

### المرحلة A — قبل أي retention policy
- إبقاء المخازن كما هي دون pruning.
- تحديد limits للـhost ingress مستقبلاً: payload bytes، string lengths، request rate، command rate، والـconcurrency.
- قياس counts/estimated bytes لكل store مع مراقبة high-water marks؛ لا تُسجل محتوى hints أو معرفات حساسة في telemetry بلا سياسة خصوصية.
- إضافة اختبارات حدود بعد اعتماد قيم المنتج؛ لا تستنتج الحدود من defaults الاختبارية.

### المرحلة B — بعد قرارات المالك
- تحديد lifetime لكل store: command/session/match/account/operational.
- تحديد ما إذا كانت exact replay semantics مطلوبة عبر المباراة فقط أو عبر reconnect/restart.
- اختيار بين in-memory bounded window، durable event log، snapshots، أو summary-only وفق احتياجات الاستئناف والتدقيق.
- وضع migration/versioning وخطة failure recovery قبل حذف أي تاريخ قديم.

### المرحلة C — التنفيذ والتحقق
- تنفيذ retention policy لكل store على حدة، لا عبر generic cleanup timer.
- اختبار duplicate قديم، sequence reuse، restart/resume، match completion، memory high-water، وmulti-match isolation.
- تحديث threat model وprivacy/data lifecycle والـADR والسجلات التشغيلية بأدلة مرتبطة بـSHA.

## 6. القرارات المؤقتة

- لا cap/eviction/snapshot في هذه المرحلة.
- لا تحويل `Events` إلى ring buffer قبل حسم الحاجة إلى history كاملة.
- لا حذف `ProcessedRequestIds` أو `BetrayalLedger._events` قبل تعريف replay window.
- لا اعتبار config defaults مثل `PlayerCount = 4` أو `MatchTimeLimitSeconds = 20 minutes` حدود تشغيلية معتمدة؛ الوثائق والكود تصفها كـMVP/proposed/test values.
- D-03 وD-04 وSEC-05 تبقى مفتوحة حتى قرار المنتج وخطة توافق API.

## 7. مصادر التنفيذ

- `src/CostOfStaying.Domain/Match.cs`
- `src/CostOfStaying.Domain/Player.cs`
- `src/CostOfStaying.Domain/MatchConfig.cs`
- `src/CostOfStaying.Domain/DomainEvent.cs`
- `src/CostOfStaying.Core/Domain/BetrayalLedger.cs`
- `src/CostOfStaying.Core/Domain/SessionRegistry.cs`
- `src/CostOfStaying.Core/Domain/ConnectionSessionRegistry.cs`
- `src/CostOfStaying.Core/Domain/MatchPlayerRegistry.cs`
- `src/CostOfStaying.Core/Domain/CommandAdmissionQueue.cs`
- `src/CostOfStaying.Core/Domain/CommandSequenceTracker.cs`
- `docs/governance/OPEN_DECISION_REGISTER.md`

**الخلاصة:** Core registries والـqueue bounded لكل instance وفق capacities يحددها host. أما event/dedupe stores في Domain وBetrayalLedger فغير bounded، وبعض Domain collections قابلة للتعديل المباشر. العلاج الصحيح يعتمد على دورة حياة المباراة ودلالات replay والخصوصية، وليس على حذف عشوائي أو cap غير معتمد.
