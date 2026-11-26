# مصالحة تقرير الفحص العميق الأرشيفي مع المصدر الحي — 2026-11-26

**المصدر التاريخي:** `References/تقرير_فحص_عميق_CostOfStaying.md` داخل حزمة `CostOfStaying.zip`.  
**تاريخ التقرير:** 2026-11-19.  
**رأس المصدر الحي لهذه المراجعة:** `main` بعد تحديثات 2026-11-26؛ ملفات المصدر واختباراته قُرئت مباشرة من GitHub.  
**الغرض:** منع إعادة فتح عيوب سبق إصلاحها، وعدم نقل نتائج أو موافقات قديمة دون مطابقة.

## 1. مصفوفة نتائج التقرير التاريخي

| # | ادعاء التقرير القديم | الحالة في المصدر الحي | الدليل/المسار | القرار |
|---|---|---|---|---|
| 3.1 | مؤقت الانهيار الأولي يعتمد على ترتيب اللاعبين | **عولج**؛ تهيئة roster كاملة ثم حساب interval مشترك | `Domain/Match.cs` و`Domain.Tests/CollapseTests.cs` — اختبار `Start_Assigns_The_Same_Initial_Collapse_Timer_To_Every_Player` | `FIXED_IN_LIVE` |
| 3.2 | الإجماع لا ينهي المباراة فوراً | **عولج**؛ آخر Yes يستدعي إغلاق التصويت فوراً | `Match.CastVote` و`VoteAndEndgameTests` — اختبار الإجماع الفوري | `FIXED_IN_LIVE` |
| 3.3 | الانقطاع أثناء التصويت لا يغيّر Yes سابقاً إلى No | لا توجد آلية disconnect/transport في Domain؛ سياسة موت/انقطاع المصوّت لم تُحسم | `docs/quality/DOMAIN_DEFECT_REVIEW_2026-11-25.md`، بند D-05 | `OWNER_DECISION / INTEGRATION_GAP` |
| 3.4 | قبول تصويت بعد deadline أو enum غير معروف | **عولج**؛ رفض enum غير معرف، وإغلاق التصويت عند وصول deadline حتى دون Tick سابق | `Match.CastVote` و`Vote_Rejects_Unknown_Choice_And_Vote_After_Deadline_Without_Tick` | `FIXED_IN_LIVE` |
| 3.5 | Time Limit يتقدم على Last Survivor | **عولج**؛ Last Survivor يُقيّم قبل Time Limit | `Match.EvaluateEndConditions` و`Endgame_LastSurvivor_Takes_Precedence_At_TimeLimit` | `FIXED_IN_LIVE` |
| 3.6 | أوامر اللعب تقبل بعد اكتمال المباراة | **عولج**؛ أوامر النقل/Echo/Hint تتحقق من نشاط المباراة | `Match.IsGameplayActive` و`Gameplay_Commands_Are_Rejected_After_Match_Completion` | `FIXED_IN_LIVE` |
| 3.7 | Echo IDs لا ترتبط بسجل موارد خادمي | لا يوجد registry للعالم/الموارد؛ تحقق Domain الحالي يمنع تكرار ID لكل لاعب فقط | `Match.CollectEcho` | `OPEN_PRE_INTEGRATION` — لا توصيل RPC مباشر |
| 3.8 | التلميح يقبل نصاً حراً | ما زال `TrySendHint` يقبل `hintText`؛ لا قائمة IDs/نصوص خادمية | `Match.TrySendHint` | `OPEN_PRE_INTEGRATION` — يلزم عقد خادمي قبل الشبكة |
| 3.9 | مجموعات Domain قابلة للتعديل مباشرة | ما زال قائماً في `Players`, `Events`, `ProcessedRequestIds`, `Votes`, `Player.Debuffs`, `CollectedEchoIds` | `Match.cs`, `Player.cs`; مراجعة D-03 | `OPEN_D-03` |
| 3.10 | تحقق المدخلات غير مكتمل | **عولج جزئياً وبشكل مهم**: finite/range في Tick والمسافة، config snapshot validation، وDebuff validation. لا يوجد حد عام لطول requestId/hintText، كما أن سجل الأحداث وIDs غير محدودين | `Match.Tick`, `MatchConfig.CreateValidatedSnapshot`, `Debuff`, `SecurityTests`; D-04 | `PARTIALLY_FIXED` |

## 2. نتائج معمارية وتكامل

1. **Domain/Core lifecycle:** ما زال الاختلاف قائماً. Domain يتضمن `Dead` و`Disconnected` و`Spectating`، بينما Core يستخدم `Alive/Spirit/Eliminated`. لا توجد سجلات ADR مقبولة في `docs/adr/` الحي تسمح بتوحيد أو ترحيل النموذجين. القرار `OWNER_DECISION`.
2. **Single-writer:** ما زال قيداً معمارياً مطلوباً؛ لا يوجد transport runtime يثبت تطبيقه على callbacks الشبكة.
3. **Core والمصادقة:** Core مكتبة خدمات/عقود؛ وجود خدمة قبول أو مصادقة محلية لا يثبت وجود هوية أو مصادقة إنتاجية.
4. **Unity/Android/server:** لا يوجد مشروع Unity أو Android build أو Dedicated Server/transport runtime في شجرة `main` الحالية.

## 3. نتائج المحاكي وأدوات الاختبار

- **Parity:** ملف `tools/balance_simulator/test_parity.py` يصرح حالياً صراحةً أنه يختبر محاكي Python فقط ولا ينفذ أو يقارن C#؛ لذلك تسمية الملف تاريخية/متوافقة ولا تثبت cross-language parity.
- **حقل سجل الجلسة:** تغير الحقل إلى `vote_opened` في `tools/playtest/run_sessions.py`؛ لم يعد الحقل القديم `voted` موجوداً في المقطع الحالي.
- **الوثيقة المستقبلية زمنياً:** ملاحظة التقرير القديم حول تاريخ `VERIFICATION_2026-11-20.md` كانت مرتبطة بكون تاريخ الفحص 19 نوفمبر. لم تعد مسألة تاريخ مستقبلي في 26 نوفمبر؛ لكنها تظل دليلاً تاريخياً محدوداً ولا تستبدل نتائج CI الحديثة المرتبطة بالـSHA.
- **نتائج Python القديمة:** أرقام التشغيل في تقرير 19 نوفمبر تخص بيئته وعينته؛ لا تُنسب إلى الرأس الحالي دون تشغيل/CI مرتبط به.

## 4. نقاط إصلاح إضافية تأكدت في المصدر الحي

1. `MatchConfig` ينشئ snapshot validated داخل Match؛ قراءة `Match.Config` تعيد snapshot ولا تمنح المستهلك مرجع الإعداد الداخلي.
2. `Debuff` يرفض معرفاً فارغاً وقيم decay غير finite/سالبة ومدة غير finite/غير موجبة.
3. `Match.Tick` يرفض delta غير موجب أو NaN أو Infinity.
4. `TryTransfer` يرفض مسافة غير finite أو سالبة.
5. اختبار ما بعد اكتمال المباراة يثبت عدم تغير الأحداث أو موارد اللاعبين عند رفض الأوامر.

هذه إصلاحات موجودة في المصدر الحي، لكن قراءة الملفات لا تعادل تشغيل NUnit محلياً. نتائج CI يجب أن ترتبط بالرأس النهائي المحدد.

## 5. البنود المفتوحة التي تستحق الاستمرار

- D-03: تغليف مجموعات الحالة القابلة للتعديل بخطة توافق API واختبارات.
- D-04: سياسة retention/limits لسجل الأحداث وProcessedRequestIds وحدود أطوال المدخلات.
- D-05: سياسة تصويت الموت/الانقطاع أثناء التصويت، مع عقد transport عند توفره.
- D-06: سياسة collapse catch-up عند تأخر Tick (skip أو bounded catch-up أو replay) وقرار مالك.
- Echo registry خادمي وقائمة Hint IDs معتمدة قبل أي RPC/network binding.
- عقد تحويل/توحيد Lifecycle بين Domain/Core بعد قرار معماري مقبول.
- اختبارات integration للـsingle-writer والاتصال وإعادة الاتصال عند تنفيذ transport.

## 6. قرار الاستيراد

التقرير العميق الأرشيفي مفيد كسجل تاريخي لتفسير أسباب بعض الإصلاحات، لكنه لا يُنسخ فوق التقارير الحية ولا يُعامل كقائمة عيوب حالية. تم تحويل كل نتيجة إلى حالة مقارنة: `FIXED_IN_LIVE`, `OPEN`, `PARTIALLY_FIXED`, أو `OWNER_DECISION`. لا تعديل في كود اللعبة نتيجة هذه القراءة وحدها.
