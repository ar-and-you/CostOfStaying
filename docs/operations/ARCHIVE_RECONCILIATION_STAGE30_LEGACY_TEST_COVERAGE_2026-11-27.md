# المرحلة 30 — مراجعة دلالية لاختبارات Core القديمة الأرشيفية (2026-11-27)

## الرأس والنطاق

- HEAD قبل المرحلة: db8fc163e9fe159284680f70a98e8df66a0e7575.
- النطاق: ستة ملفات اختبارات ARCHIVE_ONLY كانت تحت tests/CostOfStaying.Core.Tests/ في الأرشيف:
  CollapseTests.cs وSecurityTests.cs وSpiritTests.cs وTransferTests.cs وVitalityTests.cs وVoteAndEndgameTests.cs.
- تمت مقارنة أسماء حالات الاختبار مع ملفات Domain.Tests الحية المناظرة، دون إعادة حساب بصمات المرحلة 18.

## نتيجة المقارنة على مستوى أسماء الاختبارات

- الملفات الستة الأرشيفية تضم 37 اسماً لاختبار.
- ظهرت أسماء الحالات الـ37 كلها في ملفات Domain.Tests الحية المناظرة.
- ملفات Domain الحية تضيف تغطية غير موجودة في المجموعة القديمة، ومنها:
  - توحيد مؤقت Collapse الأولي لجميع اللاعبين.
  - رفض القيم غير finite ورفض إعداد Debuff غير الصالح.
  - منع أوامر اللعب بعد اكتمال المباراة.
  - تطبيق Debuff فقط خلال الجزء الفعلي من Tick الذي تكون فيه المدة نشطة.
  - رفض خيار تصويت غير معروف والتصويت بعد deadline دون Tick سابق.
  - أولوية LastSurvivor عند حد TimeLimit وحالة المباراة ذات اللاعب الواحد.

هذا يثبت تكرار أسماء السيناريوهات، لا التطابق الكامل لهيكل Arrange/Act/Assert أو جميع assertions أو fixtures. لا نعلن تكافؤاً وظيفياً كاملاً قبل مقارنة bodies والاعتماديات.

## تفسير اختلاف المسار

الاختبارات الأرشيفية تنتمي إلى تصميم Core قديم كان يحتوي نموذج اللعب. الشجرة الحالية تفصل المسؤوليات: اختبارات قواعد اللعب في CostOfStaying.Domain.Tests، بينما CostOfStaying.Core.Tests الحالية تركز على command admission/sequence/authorization، registry/session lifecycle، cleanup، composition، وtyped payload validation.

لذلك فإن غياب الملفات الستة من Core.Tests الحالي لا يعني فقدان تغطية قواعد اللعب؛ توجد ملفات Domain.Tests المناظرة وتضم حالات الاختبار ذات الأسماء نفسها. إعادة إحياء الملفات القديمة داخل Core.Tests ستعيد خلط حدود المشروعين وقد تكرر التغطية في طبقة غير مالكة لها.

## القرار

- تصنيف الملفات الستة ARCHIVE_ONLY / LEGACY DOMAIN TESTS.
- لا استعادة داخل Core.Tests ولا حذف من الأرشيف.
- KEEP_LIVE لملفات Domain.Tests وCore.Tests الحالية كل ضمن مسؤوليته.
- لا تعديل اختبارات أو source، ولا تشغيل .NET أو CI.
- الخطوة التقنية اللاحقة عند الحاجة: مقارنة assertions والfixtures فقط للحالات التي يظهر فيها فرق سلوكي حقيقي أو تغطية ناقصة، لا نسخ الملفات كاملة.

**الحالة:** STAGE 30 COMPLETE — 37/37 LEGACY TEST NAMES MAPPED TO LIVE DOMAIN TESTS; ASSERTION-LEVEL EQUIVALENCE NOT CLAIMED.
