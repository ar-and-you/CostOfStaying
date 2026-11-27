# المرحلة 43 — تصحيح اختبار RequestId وتدقيق حدود الثقة للمسافة (2026-11-27)

## الرأس والنطاق

- HEAD قبل توثيق المرحلة: `77be997231cd93b6437d95b8ba3c9a2018ef861d`.
- أصل الفشل: تشغيل Domain CI رقم `45915454182` على SHA `33fa0fd158c894a3d4e4b554cd4bc171ee8452d4`.
- التشغيل الأول فشل في اختبار واحد من 61؛ لم يكن الفشل في شرط `string.IsNullOrWhiteSpace`، بل في بيانات الاختبار.

## السبب الجذري

كان الاختبار يمرر النص `" \\t "` (مسافة ثم backslash ثم الحرف t ثم مسافة)، وليس محرف Tab. هذه سلسلة غير فارغة وغير whitespace بالكامل، ولذلك كان قبولها كـRequestId صالحاً سلوكاً صحيحاً. لم يكن الاختبار يختبر الحالة التي يدعيها.

## التصحيح

1. استبدال قيمة الاختبار بـ`"\t"`، أي محرف Tab فعلي، وتحديث اسم الاختبار إلى `Transfer_Rejects_Null_Empty_And_Tab_RequestIds_Without_SideEffects`.
2. إبقاء شرط الإنتاج `string.IsNullOrWhiteSpace(requestId)` كما هو؛ لم يلزم تغيير سلوكي إضافي.
3. تصحيح XML documentation في `Match.TryTransfer`: Domain يستقبل المسافة من المستدعي ويتحقق من القيمة، لكنه لا يقيس مواضع اللاعبين ولا يثبت هوية/موثوقية المستدعي.
4. إعادة تسمية اختبار المسافة إلى `Transfer_Rejects_OutOfRange_Distance_Supplied_To_Domain` وتعديل التعليقات لتوضيح أن الاختبار لا يثبت وجود قياس خادمي.
5. تعديل وثيقة Suffering Transfer للفصل بين عدم تغيير حالة اللعب وبين إمكانية تسجيل حدث رفض لأغراض التدقيق.

## الملفات المعدلة

- `tests/CostOfStaying.Domain.Tests/TransferTests.cs`
- `src/CostOfStaying.Domain/Match.cs`
- `tests/CostOfStaying.Domain.Tests/SecurityTests.cs`
- `docs/systems/SUFFERING_TRANSFER_SYSTEM.md`

## التحقق على SHA

SHA المختبر: `77be997231cd93b6437d95b8ba3c9a2018ef861d`.

| الفحص | Run ID | النتيجة |
|---|---:|---|
| Domain and Python CI | 45915594682 | `completed / success` |
| Core CI (.NET 10) | 45915594692 | `completed / success` |
| Documentation Governance | 45915594633 | `completed / success` |

- Domain NUnit: **61 passed, 0 failed, 0 skipped**.
- Python syntax، simulator regression، وsimulated-playtest CLI: نجحت جميع الخطوات.
- Core NUnit: نجح.
- تدقيق روابط Markdown وإمكانية الوصول من فهرس الوثائق: نجح.
- فشل التشغيل السابق على `33fa0fd` محفوظ في التاريخ؛ لا نخفيه، لكن إصلاح fixture تحقق بنجاح على الرأس الحالي.

## الحدود المتبقية

- لا يوجد Network Adapter أو server runtime؛ قياس المسافة الخادمي لم يُنفذ.
- `ProcessedRequestIds` ما زال داخل الذاكرة وعمر Match واحد؛ لا persistence أو deduplication بعد restart.
- لا يثبت هذا التشغيل التزامن المتوازي أو تكامل النقل عبر الشبكة.
- لا توجد اختبارات Unity/Android أو جلسات لعب بشرية في هذا النطاق.

**الحالة:** `STAGE 43 COMPLETE — INVALID WHITESPACE FIXTURE CORRECTED; CURRENT SHA CI VERIFIED.`
