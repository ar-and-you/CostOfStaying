# المرحلة 51 — مصالحة ميثاق وتصميم وقواعد اللعبة والمسرد (2026-11-27)

## الرأس والنطاق

- HEAD قبل المرحلة: 72f75447882a776cf74e4dcf086726099182b8ed.
- تمت مراجعة خمسة ملفات مشتركة بالاسم: 00_PROJECT_CHARTER، 01_GAME_DESIGN_DOCUMENT، 02_GAME_RULES_SPECIFICATION، 03_PLAYER_EXPERIENCE_AND_CORE_LOOP، GLOSSARY.
- لكل مسار 7 ظهورات أرشيفية؛ 35 ظهوراً إجمالاً و11 نسخة محتوى فريدة (3 charter، 2 GDD، 2 rules، 1 player experience، 3 glossary).
- تمت مقارنة MASTER UNIFIED وv3.0 clean ونسخ FINAL/backup والنسخة canonical مع main. لا إعادة crosswalk للـblob الذي حُسم في المرحلة 21.

## النتائج حسب الوثيقة

### ميثاق المشروع
النسخ الأرشيفية تؤكد رؤية اللعبة وركائزها ونطاق MVP (4 لاعبين/خريطة واحدة/Android/10–16 دقيقة و20 دقيقة أولية)، وهذه التفاصيل ممثلة بالفعل في الميثاق الحي. لكنها تتضمن ادعاءات تاريخية بأن ADR-001/002/005/021/023 معتمدة أو أن Domain انتقل إلى Unified Core، كما تضيف iOS QA ضمن نطاق سابق. هذه الادعاءات لا تتوافق مع حالة الحوكمة الحالية؛ لا استيرادها ولا تغيير حالة المنتج.

### Game Design Document
النسخ الأرشيفية لا تضيف ميكانيكاً ناضجاً غير ممثل في GDD الحي: الأعمدة، الحلقة، النتائج الخمس، وSpirit موجودة. المنظور وCombat والعتبات ما زالت TBD/غير ملزمة. لا استبدال ولا توسيع نطاق.

### مواصفة القواعد
ظهرت نسخ أرشيفية متعارضة:
- نسخة قديمة تعتبر Vitality=0 حالة Dead وتضع سياسة disconnect/timeout = No.
- نسخة v2.1 تقول Alive→Spirit لكنها تستند إلى ADR-007/023 كقرارات معتمدة، وتضيف مصفوفة Eliminated/Resurrect/Eliminate مع تعطيلها في MVP.
- نسخ أخرى تصف Domain بأنه انتقل إلى Core أو أن الخادم يقيس المسافة.

الوثيقة الحية أدق: قواعد RULE-01..05 PROPOSED FOR PLAYTEST؛ Domain ينتقل حالياً Alive→Spirit؛ Core يملك PlayerLifecycleState منفصلاً؛ سلوك disconnect وTimeLimit edge cases تحتاج قراراً؛ وDomain يستقبل distance من caller ولا يقيسها عبر adapter شبكي. لا اعتماد لنسخ ADR الأرشيفية ولا استيراد سياساتها.

### تجربة اللاعب والحلقة
المحتوى الأرشيفي يكرر الحلقة الأساسية ومقاييس Playtest الموجودة في الوثيقة الحية. لا تفاصيل كاميرا أو تحكم أو UX قابلة للتنفيذ دون prototype؛ لا استيراد.

### المسرد — salvage محدود
أضيفت إلى GLOSSARY.md تعريفات تقنية ناقصة لكنها مدعومة بالمصدر الحي:
- Intent: مفهوم طلب العميل غير الموثوق؛ لا adapter حالي.
- MatchId: Core typed ulong، والصفر غير صالح؛ منفصل عن Domain Match.Id من نوع Guid.
- Move: نقل Debuff الكامل مع القيمة والمدة المتبقية.
- RequestId: مفتاح idempotency غير فارغ؛ deduplication داخل عمر Match في الذاكرة فقط.
- Resurrect/Eliminate: عمليات VitalityAccount في Core فقط؛ لا تمثلان lifecycle موحداً في Domain أو قواعد MVP معتمدة.
- PSS: مقياس ذاكرة Android مذكور في PERFORMANCE_BUDGET؛ لا ميزانية رقمية معتمدة.

لم تُضف عبارة CI Gate الأرشيفية؛ فهي تزعم أن ci.yml يتضمن secret scan، بينما لا يثبت الـworkflow الحي هذا الادعاء. لم تُستورد تسمية Unified Core.

## التغييرات والقرار

- تعديل GLOSSARY.md فقط لإضافة التعريفات التقنية السبعة أعلاه.
- لم تتغير وثائق Charter/GDD/Rules/Player Experience أو قواعد اللعب أو الكود.
- لا حذف أو نقل أو استبدال أو استيراد ملفات أرشيفية.
- لا تشغيل اختبارات في هذه المرحلة؛ التغيير توثيقي فقط.

**الحالة:** STAGE 51 COMPLETE — PRODUCT/DESIGN DOCS RETAIN LIVE; SEVEN TECHNICAL GLOSSARY TERMS SALVAGED WITHOUT IMPORTING ARCHIVE DECISIONS.
