# المرحلة 34 — استخراج ضوابط CI الناضجة من متطلبات الأرشيف ومقارنتها بالـWorkflows الحية (2026-11-27)

## الرأس والمصادر

- HEAD قبل المرحلة: 9e1885660905206e3d7a7cba1e67b3d25c24711e.
- المصدر الأرشيفي: docs/governance/REQUIREMENTS_TRACEABILITY.md وdocs/audit/CI_SUPPLY_CHAIN_AND_LANGVERSION_REVIEW_2026-11-14.md وتقارير pipeline التاريخية.
- المصدر الحي: .github/workflows/ci.yml وcore-ci.yml وdocs-governance.yml، إضافة إلى ROADMAP.md وCONTRIBUTING.md.
- هذه مراجعة إعدادات ساكنة؛ لم تُشغّل workflows أو scanners.

## ضوابط ناضجة موجودة بالفعل في main

| الضابط الأرشيفي | دليل main الحالي | النتيجة |
|---|---|---|
| تثبيت Actions على commit SHA مع تعليق إصدار | checkout/setup-dotnet/setup-python في workflows الثلاثة مثبتة على SHA كامل مع version comment | PRESENT |
| صلاحيات GitHub دنيا | كل workflow يحدد permissions: contents: read | PRESENT |
| مهلة job | 20 دقيقة لDomain/Python وCore، و5 دقائق للتوثيق | PRESENT |
| تعطيل persisted checkout credentials | كل خطوة checkout تستخدم persist-credentials: false | PRESENT |
| عدم استخدام pull_request_target | لا يظهر في workflows الحية الثلاثة | PRESENT |
| تجنب حقن قيم حدث غير موثوقة مباشرة في run | docs workflow يمرر base.ref عبر env ويستخدم متغيراً مقتبساً في shell؛ لا يظهر interpolation مباشر من head_ref داخل run | NO DIRECT INJECTION OBSERVED |
| فصل خطوط SDK | Domain/Python يقرأ global.json الجذري، وCore يقرأ tests/CostOfStaying.Core.Tests/global.json | PRESENT |

هذه الضوابط لا تحتاج استيراد الملفات الأرشيفية؛ هي مطبقة في workflows الحية.

## ضوابط أرشيفية لا يقابلها تنفيذ حي حالياً

1. **Gitleaks/secret scan workflow:** لا يوجد security.yml أو Gitleaks workflow متتبع في main، ولا يوجد gitleaks config حي. إعدادات Secret Scanning/Push Protection على GitHub موضوع منفصل عن وجود scanner داخل CI، ويحتاج إعادة قراءة مباشرة قبل تقرير الحالة الحالية.
2. **repo-quality workflow واختبارات إعداد GitHub:** لا توجد repo-quality.yml أو tools/github_config_tests في main؛ اختبارات الأرشيف تتوقع إعدادات كثيرة غير موجودة (Dependabot، CODEOWNERS، rulesets، security/mobile workflows). لا تُنقل كما هي.
3. **Unity mobile workflow:** غير موجود؛ وهذا متسق مع عدم وجود Unity project أو build pipeline أو ترخيص/أسرار موثقة. لا يُنشأ workflow شكلي.
4. **فحوص actionlint/zizmor أو SARIF:** تظهر في وثائق وتقارير تاريخية، لكن لا توجد ملفات workflow حية تثبت تشغيلها حالياً. لا يُدّعى امتثالها دون تنفيذ موثق.
5. **CODEOWNERS/ruleset:** القوالب والأداة الأرشيفية تتطلب مالكاً فعلياً وقراراً بتغيير متطلبات الدمج. لا يُنشأ ملف placeholder ولا يُطبق ruleset من ZIP.

## القرار

- الاحتفاظ بضوابط SHA pinning وleast privilege وjob timeouts وpersist-credentials:false كما هي؛ هذه ضوابط ناضجة وموجودة بالفعل.
- عدم استيراد workflows الأمن/Unity أو اختبارات إعداد GitHub الأرشيفية مباشرة.
- فتح مراجعة مستقلة لإضافة secret scanning داخل CI إذا قرر المالك نطاقها والأداة ومخرجاتها، مع فحص إعدادات GitHub الحية أولاً.
- لا تعديل في workflows أو إعدادات المستودع.
- لم يُشغّل CI أو security scanners.

**الحالة:** STAGE 34 COMPLETE — MATURE CI GUARDRAILS CONFIRMED LIVE; OPTIONAL SECURITY/CONFIG SCANNING ADOPTION REMAINS A SEPARATE DECISION.
