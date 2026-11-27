# المرحلة 45 — مراجعة أجسام اختبارات Core القديمة مقابل Domain الحي (2026-11-27)

## الرأس والنطاق

- HEAD قبل المرحلة: f117a9575847e4dadb3dac2e3e0786449fd7766d.
- النطاق: ستة ملفات ARCHIVE_ONLY من المسار القديم `tests/CostOfStaying.Core.Tests/`: CollapseTests، SecurityTests، SpiritTests، TransferTests، VitalityTests، VoteAndEndgameTests.
- تمت قراءة النسخة الأرشيفية الفعلية من حزمة `CostOfStaying-v3.0-clean(1).zip` داخل `ملفات لعبة .zip` ومقارنتها بملفات Domain.Tests الحية المناظرة.
- لا إعادة حساب SHA ولا إعادة فحص ملفات Core.Tests الحية الخاصة بالأوامر والجلسات.

## القياس

| الملف | Methods أرشيفية Core | Assertions أرشيفية | Methods حية Domain | Assertions حية |
|---|---:|---:|---:|---:|
| CollapseTests.cs | 3 | 6 | 4 | 8 |
| SecurityTests.cs | 4 | 6 | 9 | 23 |
| SpiritTests.cs | 5 | 12 | 5 | 12 |
| TransferTests.cs | 10 | 27 | 11 | 37 |
| VitalityTests.cs | 6 | 13 | 7 | 15 |
| VoteAndEndgameTests.cs | 9 | 19 | 12 | 31 |
| **الإجمالي** | **37** | **83** | **48** | **126** |

تعني Assertions هنا استدعاءات NUnit من نوع `Assert.That` و`Assert.Throws` معاً. المجموعة الأرشيفية تحتوي 83 استدعاء Assert.That ولا Assert.Throws؛ الحية تحتوي 124 Assert.That و2 Assert.Throws. أضيفت 11 methods و43 استدعاء assertion إجمالاً.

## نتائج المراجعة

1. **كل السيناريوهات الأرشيفية الـ37 باقية في Domain.Tests الحية.** عبارات assertions الأرشيفية المقابلة موجودة؛ لم يظهر سيناريو Core قديم مفقود يستلزم استعادة الملف.
2. **Collapse:** توقع الموعد 17.525 ±1e-9 في نسخة Core الأرشيفية يطابق اختبار Domain الحي؛ لا توجد هنا نسخة 17.6 القديمة التي فُحصت في المرحلة 44.
3. **Security:** الحالات الأربع القديمة باقية. اختبار المسافة أعيدت تسميته كي يوضح أن Domain يرفض القيمة التي يتلقاها، ولا يقيس موقع اللاعب أو يثبت موثوقية المستدعي. أضيفت تغطية config snapshot والقيم غير الصالحة ومنع أوامر ما بعد المباراة.
4. **Spirit:** حالات Echo وHint ورفض النقل للاعب Spirit وبقاء Vitality صفراً محفوظة. بقاء قيمة صفر بعد Tick لا يثبت وحده كل خصائص عدم drain؛ هو تحقق محدود للسلوك المحدد.
5. **Transfer:** الحالات العشر القديمة باقية. أضافت النسخة الحية assertions لحدث `TransferRejected` عند فشل out-of-range/cooldown، وأضيف اختبار RequestId null/empty/Tab. الرفض لا يغير gameplay state لكنه قد يسجل حدث تدقيق.
6. **Vitality:** أضيف اختبار يثبت عدم احتساب bonus Debuff بعد انتهاء المدة داخل Tick طويل؛ بقية السيناريوهات القديمة محفوظة.
7. **Vote/Endgame:** أضيفت حالات enum غير معروف/deadline، أولوية LastSurvivor، وإكمال مباراة لاعب واحد. اختبار الإجماع الحي يتحقق من الإغلاق الفوري واستقرار النتيجة بعد Tick لاحق.
8. **حد الطبقات:** الملفات القديمة تستورد `CostOfStaying.Core` وتضع قواعد اللعب في Core.Tests؛ الملفات الحية تستورد `CostOfStaying.Domain`. استعادتها إلى Core.Tests ستكرر اختبارات Domain وتعيد خلط المسؤوليات الحالية. Core.Tests الحية تبقى مركزة على command admission/sequence/authorization وsession/registry/cleanup والـtyped payload.

## القرار

- تصنيف الملفات الستة: `ARCHIVE_ONLY / LEGACY DOMAIN TESTS`.
- KEEP_LIVE لملفات Domain.Tests الحالية، وKEEP_LIVE لمجموعة Core.Tests الحالية ضمن حدودها.
- لا استعادة أو نسخ أو حذف لملفات Core القديمة.
- لا تعديل source/tests في هذه المرحلة ولا تشغيل جديد؛ CI السابق موثق في المرحلة 43.
- تم إغلاق body/assertion crosswalk لملفات Domain القديمة ذات الستة ملفات هنا؛ لا ندعي تكافؤ كل تفاصيل runtime أو integration.

**الحالة:** STAGE 45 COMPLETE — 37/37 LEGACY CORE TEST SCENARIOS AND 83 ASSERTIONS MAPPED TO LIVE DOMAIN; NO RESTORATION REQUIRED.
