# المرحلة 25 — فرز خطط التنفيذ والمعمارية وحزم الرفع التاريخية (2026-11-27)

## الرأس والنطاق

- HEAD قبل المرحلة: 4b82146038ae2e7a5c0f8a93e4158b34538005fb.
- تمت مراجعة 13 مساراً أرشيفياً غير موجودة في main: 5 ملفات roadmap، ملفا Phase 1 architecture، 4 ملفات جذر/تنظيم/رفع، ci-baseline، وBUILD_AND_RELEASE.
- المقارنة الحية شملت ROADMAP.md وdocs/plans/ADVISORY_PLAN_2026-11-26.md وdocs/architecture/TECHNICAL_ARCHITECTURE.md.

## النتائج حسب المجموعة

1. **خطط roadmap الخمس:** ACCEPTANCE_CRITERIA وDELIVERY_PLAN وMVP_SCOPE وWORK_PLAN_2026-11-14 وWORK_PLAN_V2_DETAILED تصف بوابات ومتطلبات مفيدة تاريخياً، لكنها PROPOSED ومربوطة بفروع وتصورات سابقة. تكرر كثيراً ما تغطيه ROADMAP.md وADVISORY_PLAN الحالية، وتحتوي على افتراضات لم تعد نافذة (مثل توحيد Domain/Core كأمر مقرر). لا تُنشأ نسخ موازية منها.
2. **وثيقتا Phase 1 architecture:** PHASE1_CORE_IMPLEMENTATION وPHASE1_TECHNICAL_MASTER تصفان baseline Core أحادي/موحد وحالات lifecycle ونتائج CI على فروع تاريخية. هذا يتعارض مع الوصف الحالي في DOMAIN_CORE.md: Domain وCore منفصلان، ولا يوجد ADR مقبول على main يسمح بالترحيل. تحفظان كمرجع تاريخي ولا يُعاد تطبيق خطواتهما.
3. **CONSOLIDATION_NOTES وUPLOAD_GUIDE:** تسجلان حزمة v3.0 بتاريخ 2026-11-17، وتوجهان إلى توحيد Domain→Core ورفع حزمة أحادية الفرع. هذه التعليمات مرتبطة بحالة/حزمة تاريخية؛ لا تنفذ على main الحالي ولا تعيد دفع ZIP أو bundle.
4. **PROJECT_STATUS_AND_EXECUTION_REGISTER:** يسجل حالة فروع وPRs تاريخية، ويصرح بأن بعض Core لم يكن مدمجاً في main عند إعداد السجل. لا يُستخدم لتقرير الحالة الحالية أو حالة PRs.
5. **REPOSITORY_STRUCTURE:** مرجع تنظيمي PROPOSED يعود إلى 2026-11-17؛ الشجرة الفعلية وفهرس main هما المرجع.
6. **ci-baseline:** يصرح بأنه نتائج محلية جزئية من ZIP بلا SHA أو GitHub Actions run؛ لا يرقى إلى VERIFIED ولا يُستخدم كدليل CI.
7. **BUILD_AND_RELEASE:** وثيقة PROPOSED تشير إلى CI للنواة الموحدة وUnity/Android مستقبلية؛ مسارها غير موجود في main. لا تستورد Workflow أو إعداد بناء من هذه الوثيقة.

## قيمة قابلة للاحتفاظ

يمكن إعادة استخدام أفكار معايير القبول، بوابات الاعتماد، فصل نتائج المحاكاة عن اللعب البشري، متطلبات artifact/SHA، وتبعية بناء Unity/Android لوجود مشروع فعلي. هذه المبادئ ممثلة بالفعل في ROADMAP.md وADVISORY_PLAN وPLAYTEST_PLAN وSTATUS_AND_EVIDENCE؛ لا توجد فجوة مثبتة تبرر نسخ الملفات القديمة كما هي.

## القرار

- تصنيف المسارات الـ13: ARCHIVE_ONLY / HISTORICAL PLAN OR PACKAGE INSTRUCTIONS.
- KEEP_LIVE لخط سير main الحالي؛ لا تُستبدل الخطط الحية ولا تدمج التعليمات القديمة.
- تعليمات الرفع والتوحيد القديمة DO_NOT_EXECUTE على main.
- لا حذف أو نقل أو استيراد أو تعديل كود/إعدادات.
- لم تُشغّل اختبارات أو CI.

**الحالة:** STAGE 25 COMPLETE — HISTORICAL ROADMAP/ARCHITECTURE/UPLOAD DOCS TRIAGED; CURRENT ROADMAP REMAINS AUTHORITATIVE.
