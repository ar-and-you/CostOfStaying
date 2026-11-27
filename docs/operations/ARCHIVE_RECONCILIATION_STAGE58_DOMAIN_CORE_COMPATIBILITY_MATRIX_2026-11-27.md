# المرحلة 58 — مصفوفة توافق Domain/Core قبل أي ترحيل (2026-11-27)

## الرأس والنطاق

- HEAD قبل المرحلة: 46b41c845197575cb753fb48f8892986f73434f5.
- تمت مقارنة Domain gameplay aggregate وPlayer/Clock/Events مع Core VitalityAccount وtyped identifiers وTimeValues وcontracts.
- الهدف: تحديد فجوات التحويل قبل أي قرار دمج أو نقل؛ لا تغيير source أو dependencies.

## مصفوفة الفروق

| البعد | Domain | Core | النتيجة |
|---|---|---|---|
| Match identity | Guid | MatchId ulong typed، zero invalid | mapping مطلوب |
| Player identity | Guid | ActorId ulong typed، zero invalid | mapping مطلوب |
| Vitality | double مع decay/debuff | uint Current/Maximum وLose/Restore | precision/units/max غير متفق عليها |
| Lifecycle | Connecting/Alive/Dead/Spirit/Disconnected/Spectating | Alive/Spirit/Eliminated | لا mapping واحد إلى واحد |
| Resurrection/Elimination | لا عمليات موحدة | TryResurrect/TryEliminate | primitives Core-only، لا قواعد منتج معتمدة |
| Time | monotonic seconds double + Tick(delta) | ServerTick/StateRevision ulong | tick frequency/adapter غير موجود |
| Replay | string RequestId per-Match in-memory | CommandSequenceId per-session | scopes مختلفة |
| Events | DomainEvent list | EventSequenceId typed contract | ordering/persistence adapter غير موجود |
| Ownership | gameplay aggregate | admission/authorization/session/registry | المشاريع منفصلة ولا ProjectReference |

## ملاحظات حاسمة

- Core VitalityAccount ليس بديلاً عن Domain Match؛ لا Collapse ولا Debuff ولا Vote ولا Spirit economy.
- Domain Match.Id Guid لا يتحول مباشرة إلى Core MatchId ulong.
- Domain Player.Id Guid لا يتحول مباشرة إلى Core ActorId ulong.
- Core TryResurrect وTryEliminate لا يعنيان أن هذه قواعد MVP معتمدة.
- RequestId ليس بديلاً عن CommandSequenceId؛ الأول مفتاح نقل داخل Match، والثاني تسلسل أوامر داخل Session.
- ServerTick لا يساوي seconds؛ أي adapter يحتاج frequency/monotonic contract.
- لا يوجد runtime consumer أو Unity/transport adapter يثبت كيف ستتم هذه التحويلات.

## متطلبات ما قبل الترحيل

1. قرار canonical IDs وmapping persistence.
2. قرار Vitality precision/unit/rounding/max.
3. قرار lifecycle mapping، خصوصاً Dead/Eliminated/Spirit/resurrection.
4. clock adapter من monotonic seconds إلى ServerTick.
5. عقد RequestId مقابل CommandSequenceId ونطاق idempotency.
6. عقد event ordering/persistence/replay.
7. adapter وcontract tests بين Domain/Core قبل نقل أي منطق.

## القرار

- KEEP_SEPARATE للمشروعين كما هما.
- D-07 يبقى OPEN_OWNER_DECISION؛ لا merge ولا project reference ولا type conversion.
- أضيفت مصفوفة توافق إلى DOMAIN_CORE.md لتسهيل قرار مستقبلي.
- لا تغيير source أو dependencies أو gameplay rules.
- لم تُشغّل اختبارات جديدة؛ هذه مرحلة تحليل وتوثيق.

**الحالة:** STAGE 58 COMPLETE — DOMAIN/CORE CONTRACT GAPS MAPPED; NO MIGRATION AUTHORIZED.
