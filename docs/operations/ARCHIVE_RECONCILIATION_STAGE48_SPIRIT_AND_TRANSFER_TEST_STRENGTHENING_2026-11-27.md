# المرحلة 48 — إغلاق فجوة تصويت Spirit وتقوية اختبارات النقل بعد مراجعة Core القديمة (2026-11-27)

## الرأس والنطاق

- HEAD قبل التوثيق: `6a31df45ea049789037b9a582c701f4c317c63d6`.
- متابعة لمراجعة أجسام الاختبارات الأرشيفية في المرحلة 45، ولتدقيق الأدوات وإعدادات GitHub في المرحلتين 46 و47.
- لا إعادة جرد أو إعادة مطابقة SHA للملفات الأرشيفية؛ التغيير يخص assertions في اختبارات Domain الحية.

## 1. فجوة Spirit vote — تم إصلاحها

الاختبار المركب القديم `Spirit_CannotTransferOrVote` كان ينفذ `TryTransfer` فقط ولا يستدعي `CastVote`. لذلك لم يكن يثبت الجزء الخاص بالتصويت رغم الاسم.

تم في `tests/CostOfStaying.Domain.Tests/SpiritTests.cs`:
- إعادة تسمية الاختبار إلى `Spirit_Cannot_Transfer`.
- إضافة `Spirit_Cannot_Cast_Vote` بتهيئة أربعة لاعبين، وتحويل أحدهم إلى Spirit، ثم إبقاء ثلاثة أحياء لفتح التصويت.
- التحقق من استبعاد Spirit من `Votes`، ورفض `CastVote` له، وعدم إضافة حدث أو إدخاله في سجل المصوتين.

قراءة `Match.CastVote` تؤكد أن هذا السلوك موجود بالفعل في Domain؛ لم يتغير كود الإنتاج أو قاعدة اللعب.

## 2. تقوية اختبارات Transfer — تم استكمالها

في `tests/CostOfStaying.Domain.Tests/TransferTests.cs`:
- اختبار حفظ المدة يتحقق من Success، وإزالة Debuff من المصدر، وهوية Debuff الهدف، ثم المدة المتبقية.
- اختبار TargetFull يتحقق من ثبات Debuff المصدر والهدف وهويات العناصر، وBetrayal، وProcessedRequestIds، مع حدث `TransferRejected`.
- اختبار Atomic failure يتحقق من هوية Debuff المصدر، وثبات Debuffs وBetrayal وProcessedRequestIds، مع تسجيل حدث رفض دون gameplay mutation.

هذه تحسينات في دقة الاختبارات وحدود الآثار الجانبية، لا تغييرات في منطق النقل.

## 3. القياس المحدث لملفات الاختبارات الستة

| الملف | اختبارات الأرشيف | Assertions الأرشيفية | اختبارات Domain الحالية | Assertions الحالية |
|---|---:|---:|---:|---:|
| CollapseTests.cs | 3 | 6 | 4 | 8 |
| SecurityTests.cs | 4 | 6 | 9 | 23 |
| SpiritTests.cs | 5 | 12 | 6 | 18 |
| TransferTests.cs | 10 | 27 | 11 | 55 |
| VitalityTests.cs | 6 | 13 | 7 | 15 |
| VoteAndEndgameTests.cs | 9 | 19 | 12 | 31 |
| **الإجمالي** | **37** | **83** | **49** | **150** |

العداد يشمل `Assert.That` و`Assert.Throws`. كل السيناريوهات الأرشيفية الـ37 ممثلة دلالياً؛ سيناريو Spirit المركب أصبح اختبار نقل مستقل واختبار تصويت صريح. أضيفت 12 method و67 assertion إلى مجموعة Domain الحالية مقارنة بمجموعة Core القديمة. هذه الأعداد لا تقيس وحدها جودة الاختبارات أو تغطية التكامل.

## 4. تحسين انتهاء Debuff ودقة اختبار Tick — تم استكمالهما

في `tests/CostOfStaying.Domain.Tests/VitalityTests.cs`:
- يلتقط اختبار `Debuff_Expires_After_Duration` معرف Debuff الأول ثم يثبت أن العنصر المتبقي بعد Tick لاحق يحمل معرفاً مختلفاً؛ لم يعد يعتمد على العدد فقط.
- صحح التعليق الزمني: Collapse المستحق أثناء Tick طويل يُعالج عند نهاية Tick وفق تنفيذ Match، ولا يُدّعى أنه عولج عند 40s داخل الفترة.
- أعيدت تسمية `Server_Clock_Is_Authority_Delta_Not_Client` إلى `Tick_Uses_The_Delta_Provided_By_Its_Caller`، مع تعليق صريح أن الاختبار لا يثبت حد ثقة شبكياً أو وجود server adapter.
- أزيل متغير محلي غير مستخدم من `Spirit_Has_No_Vitality_Drain` في SpiritTests.

## 4. تنظيف تقارير المرحلة 45

كان هناك تقريران للموضوع نفسه. أُبقي التقرير المفهرس:
`ARCHIVE_RECONCILIATION_STAGE45_LEGACY_CORE_TEST_BODY_CROSSWALK_2026-11-27.md`

وحُذف التقرير المكرر غير المفهرس:
`ARCHIVE_RECONCILIATION_STAGE45_CORE_TEST_BODY_ASSERTION_CROSSWALK_2026-11-27.md`

سبب التنظيف: Documentation Governance run `45916260374` على SHA `1c81cba...` أبلغ عن الملف الثاني بوصفه orphan document. بقي تقرير المرحلة 45 المعتمد وفهرسه. لا حذف لملفات المصدر أو الاختبارات أو ZIP الأصلية.

## 6. قرارات المرحلة

- `KEEP_LIVE` لاختبارات Domain؛ لا استعادة لملفات Core القديمة.
- فجوة Spirit vote مغلقة باختبار انحدار صريح.
- تغطية Transfer failure/duration/atomicity أصبحت أوضح.
- التقرير المكرر أزيل؛ تقارير المرحلتين 46 و47 تغطيان على التوالي أدوات الأرشيف وإعدادات GitHub والقوالب، ولا يوجد اعتماد تلقائي للملفات القديمة.
- لا تعديل في production code، ولا تغيير في قواعد اللعب أو إعدادات GitHub.
- الحالة: `STAGE 48 IMPLEMENTED — TEST ASSERTIONS STRENGTHENED; DUPLICATE REPORT REMOVED; CI VERIFICATION PENDING.`

## التحقق

يلزم اعتماد تشغيلات Domain/Core/Documentation Governance التي تستهدف HEAD النهائي بعد هذه التغييرات. لا يُحسب أي تشغيل `cancelled` نجاحاً، ولا أدعي تشغيل .NET محلياً.
