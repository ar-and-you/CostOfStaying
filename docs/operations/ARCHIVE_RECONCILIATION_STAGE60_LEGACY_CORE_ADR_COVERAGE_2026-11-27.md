# المرحلة 60 — مطابقة عقود ADR الأرشيفية 008–020 مع Core الحي (2026-11-27)

## الرأس والنطاق

- HEAD قبل المرحلة: 460c3da42181f3c27a6e12cfd7ad437539c63357.
- النطاق: 13 ملف ADR تقني من الأرشيف (ADR-008 إلى ADR-020) مقابل شجرة Core الحالية واختبارات NUnit.
- المصدر الأرشيفي: MASTER_UNIFIED؛ الحالات APPROVED/PROPOSED المذكورة أدناه هي نصوص تاريخية داخل الأرشيف فقط.
- لم تُستورد ADRs ولم يُنشأ أي سجل قبول في docs/adr.

## مصفوفة التغطية

| ADR | الموضوع | تطبيق Core الحالي | حالة التغطية/الفجوة |
|---|---|---|---|
| 008 | Command envelope and sequence admission | CommandEnvelope, CommandSequenceTracker, CommandAdmissionService | Implemented + tests؛ replay per-session in-memory |
| 009 | Match-scoped player registry | MatchPlayerRegistry | Implemented + tests؛ membership فقط |
| 010 | Server session lifecycle | SessionLifecycle | Implemented + tests؛ reconnect policy خارج Core |
| 011 | Bounded session registry | SessionRegistry | Implemented + tests؛ capacity محددة وقت composition، multi-session policy مفتوحة |
| 012 | Authenticated command admission | CommandAdmissionService | Implemented + tests؛ يعتمد على ActorId موثوق مرتبط بالجلسة |
| 013 | Safe session cleanup | SessionCleanupService, ConnectionAwareSessionCleanupService | Implemented + tests؛ queue/replay cleanup، لا persistence |
| 014 | Victim scope | VictimId typed identifier | type only؛ categories/player-vs-NPC غير محسومة |
| 015 | Command identity/idempotency | CommandSequenceTracker | Partial؛ لا CommandId مستقل ولا result store ولا idempotency عبر reconnect/restart؛ التسلسل مرتبط بـSessionId |
| 016 | Transport connection/session binding | ConnectionSessionRegistry | Implemented + tests؛ mapping in-memory، لا transport adapter |
| 017 | Credential validation/actor binding | SessionLifecycle.TryAuthenticate(ActorId) boundary | Boundary only؛ لا credential validator أو identity provider أو account-to-ActorId mapping |
| 018 | Match roster command admission | MatchCommandAdmissionService | Implemented + tests؛ membership قبل enqueue وبعد dequeue |
| 019 | Match command authorization | MatchCommandAuthorizationService | Generic fail-closed gate + tests؛ policy صريحة مطلوبة ولا توجد gameplay policy معتمدة |
| 020 | Typed command payload validation | MatchCommandPayloadValidationService | Generic validator gate + tests؛ لا concrete gameplay DTOs ولا raw frame/serializer quotas |

## قراءة حالات ADR الأرشيفية

- نسخ ADR-008..014 تسجل APPROVED (Core contract only أو deferral disposition) بموجب ADR-027 في 2026-11-17، وتذكر أن المراجعة كانت static وأن الاختبارات لم تُعد.
- ADR-015..020 تبقى PROPOSED في الأرشيف، وبعضها يذكر implementation/tests على review branches تاريخية.
- لا توجد سجلات ADR مقبولة في docs/adr على main؛ لذلك لا تنقل حالة الأرشيف إلى الحوكمة الحالية.
- وجود تطبيق في Core لا يثبت أن السياسة المعمارية مقبولة، ولا يثبت وجود transport/authentication/server runtime.

## حدود تقنية مهمة

1. CommandSequenceId ليس CommandId مستقلاً؛ نطاقه SessionId، ويُمسح replay state عند إزالة session.
2. SessionLifecycle لا يتحقق من credentials؛ TryAuthenticate يقبل ActorId يقدمه trusted application بعد التحقق الخارجي.
3. ConnectionSessionRegistry يربط IDs فقط؛ لا يخزن socket أو token أو transport object ولا ينشئ اتصالاً.
4. MatchCommandAuthorizationService يتطلب policy صريحة fail-closed؛ لا يوجد default allow-all ولا policy gameplay معتمدة.
5. MatchCommandPayloadValidationService يعمل على TCommand بعد deserialization؛ حدود wire frame/serializer/decompression تخص transport adapter الغائب.
6. VictimId type لا يحدد فئة victim ولا يثبت دعم NPC.
7. كل الخدمات موصوفة single-writer/server-serialized؛ لا تدعي thread-safety أو distributed consistency.

## الاختبارات والأدلة

- Core test suite يشمل CommandEnvelope/SequenceTracker/AdmissionQueue/AdmissionService، SessionLifecycle/Registry/Cleanup، ConnectionSessionRegistry، MatchPlayerRegistry/Admission/Authorization/PayloadValidation، وCoreComposition.
- آخر تشغيل موثق على SHA bfbed74ae24e8e5e49baff84e7a18b0e99b55807: Core CI run 45916990210 نجح 246/246.
- هذه نتيجة SHA تاريخية بالنسبة إلى HEAD المرحلة 60؛ لم يُشغّل اختبار جديد في هذه المرحلة.

## القرار

- KEEP_LIVE لتطبيقات Core الحالية والاختبارات الموجودة.
- ARCHIVE_ONLY لحالة ADR الأرشيفية؛ لا اعتماد ولا استيراد.
- إبقاء D1 وقرارات credential provider/reconnect/multi-session/persistent idempotency/victim scope/concrete DTOs مفتوحة.
- لا تغيير source أو workflows أو dependencies.

**الحالة:** STAGE 60 COMPLETE — 13 LEGACY CORE CONTRACTS MAPPED; IMPLEMENTATION COVERAGE SEPARATED FROM ADR ACCEPTANCE.
