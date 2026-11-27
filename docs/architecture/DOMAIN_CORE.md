# Domain وCore — الحالة الفعلية وحدود المسؤولية

**الحالة:** وصف للفرع الحي، وليس ADR أو قراراً بأن هذا الحد معماري نهائي.

## 1. ما يوجد في `main`

| المسار | المسؤولية | الاستهداف |
|---|---|---|
| `src/CostOfStaying.Domain/` | مجمّع المباراة واللاعب وقواعد Vitality/Collapse/Transfer/Vote/Spirit والنتائج | `netstandard2.1` |
| `src/CostOfStaying.Core/` | عقود وخدمات admission/authorization/validation، هوية وتسلسل الأوامر، سجل اللاعبين والجلسات وتنظيفها | `netstandard2.1` |
| `tests/CostOfStaying.Domain.Tests/` | اختبارات Domain | `.NET 8` |
| `tests/CostOfStaying.Core.Tests/` | اختبارات Core | `.NET 10` عبر `global.json` محلي |

المشروعان منفصلان. Core لا يحتوي حالياً `Match` gameplay aggregate من Domain، ولا توجد هجرة كاملة. لا تنفذ النقل أو توحيد المشروعين اعتماداً على وثيقة أرشيفية.

## 2. ملاحظات تنفيذ موثقة

- Domain يستخدم `IMatchClock` وساعة قابلة للتحكم في الاختبارات.
- `Match` يحمل `Guid` للمباراة واللاعبين، وحالة `MatchState` و`PlayerState`.
- `PlayerState` enum يحتوي Connecting وAlive وDead وSpirit وDisconnected وSpectating.
- عندما تصل Vitality إلى صفر، `TransitionToSpirit` يغيّر الحالة مباشرة من Alive إلى Spirit. لا يوجد مسار active يضع PlayerState.Dead؛ لكن قيمة Dead ما زالت موجودة وتدخل في شرط `hasDeath` في نهاية المباراة.
- لا توجد قيمة `Eliminated` ضمن enum الحالي على `main`، رغم ظهورها في بعض سجلات الأرشيف ومقترحات القواعد.
- `Core` يملك بنية أوامر وجلسات مستقلة، ولا يساوي خادم شبكي أو تطبيقاً كاملاً.
- يختلف تمثيل Vitality بين النموذجين: يستخدم Domain النوع `double` في `Player.Vitality`، بينما يستخدم Core النوع `uint` في `IVitalityAccount` و`VitalityAccount`. لا يوجد عقد تحويل أو تكافؤ موثق بينهما؛ هذا وصف للاختلاف الحالي وليس قراراً بتوحيد النوع أو ترحيل النموذج.

### مصفوفة توافق API قبل أي ترحيل

| البعد | Domain | Core | فجوة الدمج |
|---|---|---|---|
| هوية المباراة | Match.Id: Guid | MatchId: ulong typed؛ الصفر invalid sentinel | يلزم mapping/registry ثابت؛ لا cast أو تحويل ضمني |
| هوية اللاعب | Player.Id: Guid | ActorId: ulong typed؛ الصفر invalid sentinel | يلزم mapping يحافظ على الهوية عبر الجلسات والاستعادة |
| Vitality | double مع decay per-second وDebuff contributions | uint Current/Maximum مع Lose/Restore | يلزم تحديد precision/rounding/units/max؛ لا تكافؤ تلقائي |
| Lifecycle | Connecting/Alive/Dead/Spirit/Disconnected/Spectating | Alive/Spirit/Eliminated | لا تطابق كامل؛ Dead enum في Domain غير مستخدم كمسار active، وEliminated Core terminal |
| Resurrection/Elimination | لا يوجد Domain operation موحد | TryResurrect وTryEliminate في VitalityAccount | قرار منتج مطلوب قبل إدخال هذه الانتقالات في Domain |
| Time | IMatchClock.NowSeconds: double وTick(deltaSeconds) | ServerTick: ulong وStateRevision: ulong | يلزم تعريف tick frequency والتحويل والـmonotonic source |
| Replay/sequence | RequestId: string لنقل Debuff؛ مجموعة per-Match في الذاكرة | CommandSequenceId per-session وsequence tracker | مختلفان في scope والهوية والاحتفاظ؛ لا يستبدل أحدهما الآخر |
| Events | DomainEvent داخل Match event list | EventSequenceId typed في Core contracts | يلزم عقد ترتيب وربط/تخزين، لا يوجد adapter حالياً |
| مسؤولية المشروع | gameplay aggregate وقواعد المباراة | admission/authorization/session/registry primitives | لا توجد ProjectReference أو composition تربطهما |

Core VitalityAccount ليس Match gameplay aggregate: منشؤه الداخلي يتطلب ActorId، ولا يحتوي Collapse أو Debuff أو Vote أو Spirit economy. كما أن Core لا يملك Domain MatchId/Guid mapping أو clock adapter.

### متطلبات قرار الترحيل

1. اختيار canonical identity strategy وmapping persistence لـGuid/ulong.
2. تحديد Vitality unit/precision/rounding/max والحدود المقبولة.
3. تحديد mapping lifecycle بما يشمل Dead/Eliminated/Spirit وقرار resurrection.
4. تعريف clock adapter من monotonic seconds إلى ServerTick، وتحديد من يملك الوقت.
5. تعريف العلاقة بين RequestId وCommandSequenceId، ونطاق idempotency بعد restart.
6. تحديد event ordering/persistence/replay، ثم بناء adapter واختبارات contract بين المشروعين.

لا يمكن اختيار هذه التحويلات من الأرشيف أو من اختلاف الأنواع وحده؛ يلزم قرار معماري مقبول قبل الدمج.
- لا تحتوي المكتبتان مراجع Unity أو FishNet في المشروعين الحاليين.

## 3. ما لا يوجد

لا Unity project، ولا FishNet adapter، ولا Dedicated Server، ولا نقل Domain إلى Core، ولا Android/iOS build. وجود العقود وخدمات Core لا يثبت تكامل الشبكة أو المصادقة الإنتاجية.

## 4. قرار التغيير

ابقِ الحد الحالي مستقراً مؤقتاً. قبل الترحيل يلزم:
1. سجل ADR مقبول يحدد مسؤولية كل مشروع والواجهات المستهدفة.
2. تحليل الاعتماديات والـAPI العامة والاختبارات والـserialization والـcompatibility.
3. خطة ترحيل قابلة للتراجع، مع عدم حذف واجهات متوافقة قبل دورة إهمال موثقة.
4. اختبارات انحدار تغطي قواعد Domain وخدمات Core معاً.
5. تشغيل CI على SHA الترحيل وتحديث الوثائق والفهارس في الدفعة نفسها.

لا يوجد ADR مقبول على `main` حالياً؛ السجلات المتنافسة محفوظة في `docs/archive/` فقط.


## حدود الذاكرة في Core

| المخزن | الحد الحالي | تحرير السعة |
|---|---|---|
| SessionRegistry | capacity صريح وقت الإنشاء | إزالة Closed بعد drain/reset |
| MatchPlayerRegistry | capacity صريح وقت الإنشاء | Remove(actorId) |
| ConnectionSessionRegistry | capacity صريح وقت الإنشاء | TryDetach(connectionId) |
| CommandAdmissionQueue | queue capacity صريح | TryDequeue |
| CommandSequenceTracker | maximumTrackedSessions وmaximumSequenceJump صريحان | ResetSession بعد cleanup |
| BetrayalLedger._events | غير محدود داخل عمر CoreComposition | لا clear/retention API؛ يُحرر عند إسقاط composition |
| BetrayalLedger._scores | لا حد مستقل؛ يعتمد على ActorIds المقبولة في TryRecord | لا remove/reset API |
| VitalityAccount | لاعب واحد ضمن MatchId immutable | يرفض MatchPlayerRegistry تسجيل حساب ذي MatchId مختلف |
| CommandAdmissionQueue | FIFO + sequence tracker لمباراة واحدة | يحمل MatchId immutable؛ composition يرفض تمريره لخدمة من MatchId آخر |

كل registries المحدودة تتطلب capacities من host عند الإنشاء؛ لا توجد defaults تشغيلية أو workload-derived values. على فرع hardening الحالي أصبح `CoreComposition.Create(MatchId)` إلزامياً؛ MatchLifecycle وMatchPlayerRegistry وVitalityAccount وCommandAdmissionQueue وmatch services مرتبطة بالمعرف نفسه. لا يوجد global active-match cap؛ إدارة عدد compositions/المباريات النشطة مسؤولية host. كما أن TCommand generic لا يضع حد حجم؛ raw frame/serializer/decompression quotas مسؤولية transport قبل بناء envelope.

BetrayalLedger يستقبل BetrayalEvent بعد التحقق من non-zero ActorId/VictimId وعدم self-betrayal وتسلسل EventSequenceId، لكنه لا يتحقق من عضوية Actor/Victim في MatchPlayerRegistry. Host يجب أن يتحقق من roster والسلطة قبل TryRecord. يخزن ledger كل sequence/event فريد للتمييز بين exact replay وsequence reuse المختلف؛ لذلك لا يمكن تقليم السجل دون قرار يغير idempotency semantics.

**القرار:** لا cap رقمي أو eviction policy من دون workload ومدة match وقرار الاحتفاظ. D-04 يشمل الآن Core ledger إضافة إلى Domain event/request/echo collections.


### Scope isolation — enforced factory boundary (branch under review)

`CoreComposition.Create(MatchId)` now requires a valid MatchId. MatchLifecycle, MatchPlayerRegistry, VitalityAccount and CommandAdmissionQueue carry the same immutable scope; registration rejects cross-match vitality accounts, and composition factories reject foreign match dependencies/queues. The public API no longer permits one composition instance, account or queue to manufacture/share match state across MatchIds. Core CI passed 256/256 tests on review head `289affbd14970b7c374fce3e9a861bd06e7c78fa`. The host still owns global match concurrency and must create/dispose one composition per active match. This supersedes the historical stage-65 KEEP_CURRENT_API note on the review branch only; it is not yet merged into main.
