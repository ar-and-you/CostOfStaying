# المرحلة 46 — استكمال Assertions للنقل واختبار أهلية تصويت Spirit وتنظيف التوثيق (2026-11-27)

## الرأس والنطاق

- HEAD عند بدء المرحلة: `8f1763a4c49067deb4e12d42c0176bef10a609bf`.
- هذه متابعة لتقرير المرحلة 45؛ لا تعيد جرد ZIP أو إعادة مطابقة البصمات أو مراجعة الملفات الـ37 القديمة من البداية.
- فُحصت تغييرات الاختبارات التي دخلت بعد توثيق المرحلة 45، ثم روجعت نتائج CI وسجلات فشل حوكمة الوثائق.

## 1. فجوة أهلية تصويت Spirit

كان الاختبار القديم `Spirit_CannotTransferOrVote` يختبر `TryTransfer` فقط ولا يستدعي `CastVote`. لذلك كان اسم الاختبار أوسع من التغطية الفعلية.

قراءة `Match.CastVote` تؤكد رفض اللاعب إذا لم يكن ضمن سجل `Votes` أو لم تكن حالته `Alive`. أُصلح الاختبار الحي في `tests/CostOfStaying.Domain.Tests/SpiritTests.cs`:
- تغيير الاسم إلى `Spirit_Cannot_Transfer` ليطابق السلوك الذي يختبره.
- إضافة `Spirit_Cannot_Cast_Vote` في مباراة من أربعة لاعبين؛ يُحوّل أحدهم إلى Spirit، ويُترك ثلاثة أحياء لفتح التصويت.
- التحقق من أن Spirit غير موجود في سجل المصوتين، وأن `CastVote` يرفضه، ولا يصدر حدثاً ولا يضيفه إلى السجل.

هذا اختبار انحدار لسلوك موجود؛ لم يتغير كود الإنتاج أو قاعدة اللعب.

## 2. تقوية Assertions للنقل

أضيفت assertions إلى `TransferTests.cs`:
- اختبار حفظ المدة المتبقية يتحقق الآن من نتيجة Success، وإزالة Debuff من المصدر، ووجود العنصر الصحيح لدى الهدف قبل فحص المدة.
- رفض TargetFull يتحقق من بقاء Debuff المصدر وكل عناصر الهدف كما هي، وثبات Betrayal وProcessedRequestIds، مع تسجيل حدث `TransferRejected`.
- فشل النقل إلى لاعب Spirit يتحقق من هوية Debuff المصدر وثبات Betrayal وProcessedRequestIds، ومن تسجيل حدث الرفض دون mutation في حالة اللعب.

هذه التغييرات توسع التحقق من atomicity وحدود gameplay state مقابل audit events؛ لا تغير منطق الإنتاج.

## 3. قياس اختبارات الملفات الستة بعد التحديث

| الملف | اختبارات الأرشيف | Assertions الأرشيفية | اختبارات Domain الحالية | Assertions الحالية |
|---|---:|---:|---:|---:|
| CollapseTests.cs | 3 | 6 | 4 | 8 |
| SecurityTests.cs | 4 | 6 | 9 | 23 |
| SpiritTests.cs | 5 | 12 | 6 | 18 |
| TransferTests.cs | 10 | 27 | 11 | 55 |
| VitalityTests.cs | 6 | 13 | 7 | 15 |
| VoteAndEndgameTests.cs | 9 | 19 | 12 | 31 |
| **الإجمالي** | **37** | **83** | **49** | **150** |

العداد يشمل `Assert.That` و`Assert.Throws`. تغيرت تسمية سيناريو Spirit المركب إلى اختبارين أدق، وأضيف اختبار تصويت صريح؛ لا يُشترط تطابق أسماء الاختبارات حرفياً.

## 4. تنظيف تقرير المرحلة 45 المكرر

كان هناك تقريران متنافسان للموضوع نفسه:
- `ARCHIVE_RECONCILIATION_STAGE45_LEGACY_CORE_TEST_BODY_CROSSWALK_2026-11-27.md` هو التقرير المعتمد والمفهرس في `docs/README.md`.
- `ARCHIVE_RECONCILIATION_STAGE45_CORE_TEST_BODY_ASSERTION_CROSSWALK_2026-11-27.md` نسخة مكررة غير مفهرسة.

فشل Documentation Governance على HEAD `1c81cbaaea8190f2689f09fd24f64bb2ff736e68` لأن `tools/docs_audit.sh` وجد الملف الثاني orphan document. حُذف الملف المكرر فقط، وأُبقي التقرير المعتمد وفهرسه. لا حذف لملفات مصدر أو اختبارات أو أرشيفات أصلية.

## 5. حالة CI المرصودة قبل تنظيف التوثيق

- Domain and Python CI run `45916260487` على SHA `1c81cba...`: انتهى `cancelled`؛ لا يُحسب نجاحاً نهائياً.
- Core CI run `45916260370` على SHA نفسه: `cancelled`.
- Documentation Governance run `45916260374` فشل بسبب تقرير المرحلة 45 المكرر أعلاه.
- يلزم انتظار تشغيلات جديدة على HEAD بعد حذف الملف اليتيم وتحديث الفهرس وسجل التقدم. لا يتوفر .NET SDK محلياً، ولا أدعي تشغيل NUnit محلياً.

## القرارات

- `KEEP_LIVE` لاختبارات Domain؛ لا استعادة لملفات Core القديمة.
- إصلاح فجوة Spirit vote واختبارات atomicity فقط؛ لا تغيير إنتاجي.
- حذف التقرير الوثائقي المكرر غير المفهرس، مع الاحتفاظ بالتقرير المعتمد.
- الحالة: `STAGE 46 IMPLEMENTED — SPIRIT VOTE COVERAGE AND TRANSFER ASSERTIONS STRENGTHENED; DUPLICATE DOC CLEANUP; CI RECHECK REQUIRED.`
