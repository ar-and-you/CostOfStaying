# المرحلة 18 — مطابقة Git blob لملفات الاختبارات مع الأرشيف (2026-11-27)

## النطاق والرأس المرجعي

- HEAD قبل المرحلة: `9dd54f45915cff331d52197849ef19d658f6d5d2`.
- أُعيد جلب شجرة `main` عبر GitHub recursive tree؛ الشجرة غير مقتطعة، وإجماليها 211 tracked blobs.
- نطاق المقارنة: ملفات `tests/` في main، ومقابلاتها المعيارية في الأرشيفات السبعة داخل `ملفات لعبة .zip`.
- استُخدمت بصمات Git blob SHA1 المحسوبة من bytes الخام وفق ترويسة Git blob، لا SHA-256 أو الحجم وحدهما.

## النتائج

| التصنيف | العدد |
|---|---:|
| ملفات اختبارات حية | 33 |
| مسارات أرشيفية ضمن نطاق tests | 38 |
| تطابق بايتّي حي/أرشيف | 27 |
| المسارات المشتركة ذات اختلاف محتوى | 5 |
| LIVE_ONLY (حي بلا مقابل أرشيفي مطابق للمسار) | 1 |
| ARCHIVE_ONLY (مسار أرشيفي غير موجود في main) | 6 |
| مسارات لها أكثر من نسخة أرشيفية مختلفة | 3 |

### التطابقات البايتية

تطابق 27 ملفاً حياً مع نسخة أرشيفية واحدة على الأقل. تشمل اختبارات قبول الأوامر، تسلسل الأوامر، سجل الاتصال والجلسات، تركيب Core، دورة حياة Match/Session، الحسابات والنتائج، وملفات إعداد مشاريع الاختبارات. لا حاجة لإعادة استيراد هذه النسخ.

### اختلافات المحتوى — KEEP_LIVE

المسارات الخمسة المشتركة المختلفة:
- `tests/CostOfStaying.Domain.Tests/CollapseTests.cs`
- `tests/CostOfStaying.Domain.Tests/SecurityTests.cs`
- `tests/CostOfStaying.Domain.Tests/SpiritTests.cs`
- `tests/CostOfStaying.Domain.Tests/VitalityTests.cs`
- `tests/CostOfStaying.Domain.Tests/VoteAndEndgameTests.cs`

**القرار:** إبقاء النسخ الحية. اختلاف Git blob يثبت عدم التطابق فقط، ولا يثبت أن نسخة الأرشيف أحدث أو أكمل. لا استبدال ولا دمج قبل مراجعة فروق الاختبارات والسلوك الذي تغطيه.

### LIVE_ONLY

- `tests/CostOfStaying.Domain.Tests/DebuffTests.cs`

لا يوجد له مسار معياري مقابل في الجرد الأرشيفي السباعي. هذا لا يعني أن الاختبار زائد؛ بل يعني أن النسخة الحية لا تظهر في هذه المجموعة الأرشيفية.

### ARCHIVE_ONLY

ستة مسارات ظهرت في الأرشيف ولم تعد موجودة بالمسار نفسه في main:
- `tests/CostOfStaying.Core.Tests/CollapseTests.cs`
- `tests/CostOfStaying.Core.Tests/SecurityTests.cs`
- `tests/CostOfStaying.Core.Tests/SpiritTests.cs`
- `tests/CostOfStaying.Core.Tests/TransferTests.cs`
- `tests/CostOfStaying.Core.Tests/VitalityTests.cs`
- `tests/CostOfStaying.Core.Tests/VoteAndEndgameTests.cs`

هذه مسارات تاريخية محتملة من تقسيم/إعادة تنظيم سابق للاختبارات. لم يُحكم بأنها محذوفة وظيفياً أو مكررة؛ يلزم تتبع تغطيتها الحالية ومراجعة أسماء/حالات الاختبار قبل أي قرار إزالة أو استعادة.

### تعدد النسخ الأرشيفية

ثلاثة مسارات لها أكثر من Git blob SHA أرشيفي:
- `tests/CostOfStaying.Core.Tests/CostOfStaying.Core.Tests.csproj`: نسختان؛ النسخة الحية تطابق إحداهما.
- `tests/CostOfStaying.Core.Tests/MatchCommandAuthorizationServiceTests.cs`: نسختان؛ النسخة الحية تطابق إحداهما.
- `tests/CostOfStaying.Domain.Tests/CollapseTests.cs`: نسختان؛ لا تطابق أي منهما النسخة الحية.

هذا يثبت وجود تباين تاريخي بين الأرشيفات، وليس سبباً لاختيار النسخة الأحدث زمنياً دون مراجعة.

## القيود والقرارات

- المقارنة تغطي ملفات الاختبارات الـ33 في main والمسارات الأرشيفية الـ38 ضمن `tests/` فقط؛ لا تشمل بقية `src/` أو الوثائق أو الإعدادات أو الأدوات أو Workflows.
- لا استنتاج بأن ARCHIVE_ONLY ملفات آمنة للحذف، ولا بأن LIVE_ONLY لم تكن موجودة في أي مصدر آخر.
- لم تُشغّل الاختبارات في هذه المرحلة؛ كما أن بيئة العمل لا تحتوي .NET SDK محلياً، فلا يُدّعى نجاح تشغيل.
- لا حذف، لا استبدال، لا نقل، ولا استيراد. تبقى main المصدر الوحيد الموثوق.

**الحالة:** `STAGE 18 COMPLETE — TESTS PATH/BLOB CROSSWALK; GLOBAL ARCHIVE/LIVE RECONCILIATION OPEN.`
