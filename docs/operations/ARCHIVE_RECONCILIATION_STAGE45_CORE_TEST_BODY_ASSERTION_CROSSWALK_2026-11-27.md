# المرحلة 45 — مطابقة أجسام اختبارات Core القديمة مع اختبارات Domain الحية (2026-11-27)

## الرأس والنطاق
- HEAD قبل المرحلة: `a6f003d3c6e6b0e65ddc479fd2fca199992e2fb5`.
- النطاق: ملفات Core.Tests الستة ARCHIVE_ONLY التي حددتها المراحل السابقة: Collapse, Security, Spirit, Transfer, Vitality, VoteAndEndgame.
- المصدر الأرشيفي: `CostOfStaying-v3.0-clean(1).zip` داخل `ملفات لعبة .zip`.
- قورنت أجسام الاختبارات والـfixtures والـassertions مع ملفات Domain.Tests الحية المناظرة، دون إعادة مطابقة أسماء/بصمات الملفات المحسومة سابقاً.

## القياس

| الملف | اختبارات الأرشيف | Assertions الأرشيفية | اختبارات Domain الحية | Assertions الحية |
|---|---:|---:|---:|---:|
| CollapseTests.cs | 3 | 6 | 4 | 8 |
| SecurityTests.cs | 4 | 6 | 9 | 23 |
| SpiritTests.cs | 5 | 12 | 5 | 12 |
| TransferTests.cs | 10 | 27 | 11 | 37 |
| VitalityTests.cs | 6 | 13 | 7 | 15 |
| VoteAndEndgameTests.cs | 9 | 19 | 12 | 31 |
| **الإجمالي** | **37** | **83** | **48** | **126** |

كل السيناريوهات الأرشيفية الـ37 ممثلة في Domain.Tests الحية. أضيفت 11 حالة حية و43 استدعاء Assertion. أعيدت تسمية بعض الحالات لتصف حدود Domain بدقة؛ لا يلزم تطابق أسماء الاختبارات حرفياً.

## النتائج الدلالية

### Collapse
- حالات الحفاظ على نسبة مؤقت الانهيار، وعدم القتل الفوري، والحد الأقصى للـDebuffs موجودة.
- أضيف اختبار توحيد مؤقت الانهيار الأولي لجميع اللاعبين.
- لا حاجة لاستعادة ملف Core القديم.

### Security
- حالات منع التعديل المباشر، ورفض المسافة المقدمة إلى Domain، ورفض replay، واحتساب Betrayal عند النجاح فقط موجودة.
- أعيدت تسمية اختبار المسافة إلى `Transfer_Rejects_OutOfRange_Distance_Supplied_To_Domain`؛ فالاختبار لا يثبت قياس الخادم للمسافة ولا يتتبع تجاهل ادعاء العميل.
- أضيفت اختبارات snapshot الإعدادات، والقيم غير finite، ومدة Debuff غير الصالحة، والمسافات غير الصالحة، ورفض الأوامر بعد اكتمال المباراة.
- اختبار منع التعديل المباشر يثبت مسار Tick runtime؛ لا يثبت compile-time عبر assembly خارجي، وهذا موضح في التعليق الحي.

### Spirit
- حالات Echo الفريد، ومنع Echo للاعب الحي، وHint cooldown، ومنع النقل للاعب Spirit، وعدم استنزاف Vitality موجودة.
- **فجوة:** اسم `Spirit_CannotTransferOrVote` يوحي باختبار النقل والتصويت، لكن الجسم لا ينفذ إلا Transfer. لا يوجد assertion يثبت منع Spirit من التصويت. لا تستعاد النسخة القديمة؛ يضاف اختبار تصويت فقط إذا حدد عقد قواعد التصويت أهلية Spirit بوضوح.
- المتغير المحلي Vitality في `Spirit_Has_No_Vitality_Drain` غير مستخدم؛ تحسين صيانة غير وظيفي.

### Transfer
- السيناريوهات العشرة القديمة موجودة، مع تغطية إضافية لرفض RequestId null/empty/whitespace دون آثار جانبية.
- اختبارات فشل المسافة والـcooldown الحية تتحقق أيضاً من تسجيل `TransferRejected` مع عدم تغيير حالة اللعب.
- اختبار Atomic يقيس أعداد Debuffs فقط؛ لا يثبت وحده عدم تغير Betrayal أو الأحداث أو سجل الطلبات لكل مسارات الفشل.
- اختبار الحفاظ على المدة لا يفحص نتيجة Transfer صراحة قبل قراءة Debuff الهدف؛ إضافة assertion للنتيجة تحسن التشخيص.

### Vitality
- حالات decay الأساسي، وتأثير Collapse، والانتقال إلى Spirit مرة واحدة، وعدم الإحياء، وانتهاء Debuff، وdelta الممرر إلى Tick موجودة.
- أضيف اختبار يثبت أن Debuff قصير المدة لا يمنح bonus بعد انقضاء مدته أثناء Tick طويل.
- **فجوة:** `Debuff_Expires_After_Duration` يتحقق من العدد النهائي فقط؛ لا يثبت هوية Debuff الذي انتهى (سبق تسجيل ذلك في المرحلة 44).
- **حد ادعاء:** `Server_Clock_Is_Authority_Delta_Not_Client` يختبر استهلاك Domain للـdelta الممرر إلى Tick؛ لا يثبت وجود server adapter أو منع عميل غير موثوق من تمرير delta.

### Vote and Endgame
- السيناريوهات التسعة القديمة موجودة.
- اختبار الإجماع الحي أقوى: يثبت الإكمال فور آخر Yes وبقاء النتيجة بعد Tick لاحق.
- أضيفت حالات enum غير معروف/التصويت بعد deadline دون Tick، وأولوية LastSurvivor عند TimeLimit، وإكمال مباراة لاعب واحد عند TimeLimit.

## القرار
- تصنيف ملفات Core.Tests الستة: `ARCHIVE_ONLY / LEGACY DOMAIN TESTS — FUNCTIONAL SCENARIOS MAPPED`.
- `KEEP_LIVE` لملفات Domain.Tests الحالية؛ لا استعادة ولا نسخ موازٍ داخل Core.Tests.
- لا تعامل عناوين الاختبارات أو تعليقاتها كدليل على server authority أو قياس المسافة أو أهلية التصويت دون assertions فعلية.
- فجوات Spirit vote، وهوية Debuff المنتهي، وحدود server clock، واتساع اختبار atomicity تسجل كمرشحات تحسين، لا تغييرات قواعد تلقائية.
- لا تعديل source/tests، ولا حذف أو نقل أو استيراد. لم تُشغّل NUnit أو .NET أو CI في هذه المرحلة.

## القيود
القياس يحصي 37 اختباراً و83 Assertion أرشيفياً مقابل 48 اختباراً و126 Assertion حياً. عدد Assertions لا يساوي جودة التغطية، ومطابقة السيناريوهات لا تثبت تكامل transport أو persistence أو الأمن التشغيلي.

**الحالة:** `STAGE 45 COMPLETE — SIX LEGACY CORE TEST FILES BODY/ASSERTION-CROSSWALKED; TARGETED COVERAGE GAPS REGISTERED.`
