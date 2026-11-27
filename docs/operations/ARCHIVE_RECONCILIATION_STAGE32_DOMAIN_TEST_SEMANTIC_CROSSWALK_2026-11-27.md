# المرحلة 32 — مراجعة فروق محتوى اختبارات Domain الأرشيفية (2026-11-27)

## الرأس والنطاق

- HEAD قبل المرحلة: 87d5b4ec44cc9deb20f98f37e84efd28b88ee74b.
- النطاق: خمسة ملفات Domain.Tests مشتركة بالاسم ومختلفة المحتوى في المرحلة 18:
  CollapseTests.cs وSecurityTests.cs وSpiritTests.cs وVitalityTests.cs وVoteAndEndgameTests.cs.
- لا يعاد حساب SHA أو مراجعة الملفات الـ27 المطابقة في المرحلة 18.

## مقارنة أسماء حالات الاختبار

أسماء الحالات الموجودة في النسخ الأرشيفية الخمس تظهر كلها في ملفات main الحية المناظرة. كما تضيف النسخ الحية 10 أسماء حالات جديدة موزعة كالآتي:

| الملف | حالات إضافية في main |
|---|---:|
| CollapseTests.cs | 1 |
| SecurityTests.cs | 5 |
| SpiritTests.cs | 0 |
| VitalityTests.cs | 1 |
| VoteAndEndgameTests.cs | 3 |
| **المجموع** | **10** |

أمثلة التغطية الإضافية الحية:
- Security: validated config snapshot، رفض non-finite config/tick، رفض مدة Debuff غير صالحة، رفض server distance غير finite/سالبة، ومنع أوامر gameplay بعد نهاية المباراة.
- Vitality: احتساب Debuff فقط خلال مدة نشاطه داخل Tick طويل.
- Collapse: توزيع مؤقت الانهيار الأولي نفسه على جميع اللاعبين.
- Vote/Endgame: رفض enum غير معروف والتصويت بعد deadline دون Tick سابق، أولوية LastSurvivor عند TimeLimit، وإكمال مباراة اللاعب الواحد.

## تفسير الفروق

- الاختلاف البايتّي لا يدل على فقدان التغطية؛ الأسماء القديمة باقية في الاختبارات الحية مع حالات إضافية.
- CollapseTests الأرشيفي له نسختان مختلفتان، ولا تطابق أي منهما النسخة الحية؛ مع ذلك بقيت أسماء السيناريوهات القديمة في الحي.
- لا نعلن تطابقاً كاملاً للـassertions أو fixtures أو ترتيب التنفيذ؛ المقارنة هنا على أسماء حالات الاختبار ومجالات التغطية المعلنة. تم تنفيذ مراجعة body/fixture/assertion للملفات الخمسة في المرحلة 44؛ راجع [تقرير المرحلة 44](ARCHIVE_RECONCILIATION_STAGE44_DOMAIN_TEST_BODY_ASSERTION_CROSSWALK_2026-11-27.md).
- ملف DebuffTests الحي الذي صنفته المرحلة 18 LIVE_ONLY يمثل إضافة اختبارية حية ولا يُعاد فتحه هنا.

## القرار

- KEEP_LIVE للملفات الخمسة الحية.
- لا استيراد أو استبدال من الأرشيف.
- لا حذف للنسخ الأرشيفية؛ تُحفظ كمرجع تاريخي.
- لا تعديل اختبارات أو source، ولا تشغيل .NET/CI.

**الحالة:** STAGE 32 COMPLETE — LEGACY TEST SCENARIO NAMES RETAINED; 10 LIVE ADDITIONAL TEST NAMES IDENTIFIED; ASSERTION PARITY NOT CLAIMED.
