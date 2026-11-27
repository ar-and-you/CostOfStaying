# المرحلة 48 — مراجعة ملفات الجذر والتقارير التاريخية الأرشيفية (2026-11-27)

## الرأس والنطاق
- HEAD قبل المرحلة: b6de73169004d30ef6910e94555cfa6e2662e4d5.
- النطاق: 10 مسارات جذرية أرشيفية فقط لا تظهر في main: أربعة ملفات إعداد/تنسيق وستة تقارير جرد/تدقيق تاريخية.
- الملفات المشتركة أو الحية مثل .editorconfig و.gitignore وglobal.json وCostOfStaying.sln وREADME وLICENSE لم تُعد مراجعتها؛ لها crosswalk سابق.
- لم تُنقل التقارير القديمة إلى main.

## الملفات الإعدادية الأربعة

| الملف | ما يقدمه | الملاحظة | القرار |
|---|---|---|---|
| .gitattributes | LF normalization وتصنيف binary وdiff drivers | يضع *.lock binary، ما يخفي فروق ملفات lock النصية؛ كما أن .editorconfig الحي يحدد كثيراً من قواعد النصوص | DO_NOT_IMPORT AS-IS |
| .codespellrc | إعداد codespell مع ignore list | يستثني كلمات قصيرة/غامضة ba, ot, te؛ ولا يوجد codespell workflow حي | NO_IMPORT حتى تحديد سياسة false positives |
| .gitleaks.toml | extend.useDefault = true فقط | لا يضيف قواعد أو allowlist مخصصة؛ لا قيمة عملية واضحة فوق قواعد Gitleaks الافتراضية | NO_IMPORT |
| .markdownlint-cli2.jsonc | خمسة قواعد بنيوية مع استثناء docs/audit | الاستثناء يشير إلى مسار تاريخي، ولا يوجد markdownlint workflow/حزمة حيّة | SALVAGE IDEA ONLY؛ يلزم baseline على docs الحالية |

لا يُعد غياب هذه الملفات في main عيباً بحد ذاته؛ التفعيل يجب أن يكون متناسباً مع أدوات CI الحالية ونتائجها.

## التقارير التاريخية الستة
- audit_index.txt
- branch_comparison.txt
- branches_summary.txt
- commits_summary.txt
- repo_audit_report.txt
- status_summary.txt

هذه التقارير تتعلق بمستودع المصدر القديم live-egypt/CostOfStaying وبحالة Git وتفرعاته وcommit graph وworking tree في لقطات مؤرخة، ومنها 2026-11-14. وهي مفيدة كدليل provenance تاريخي لفهم التفرعات السابقة، لكنها ليست وصفاً لحالة ar-and-you/CostOfStaying الحالية ولا أساساً لحذف فروع/ملفات أو إعادة تطبيق commits.

توجد بيانات status تشير إلى ملفات غير متتبعة/مخرجات تقارير، كما أن تقرير التدقيق الكبير يضم سجل أوامر ومخرجات Git من بيئة قديمة. لذلك لا يُعاد نشرها في ملفات main الحية ولا تُستخدم لإثبات سلامة الرأس الحالي.

## القرار
- ملفات الإعداد الأربعة: لا نقل مباشر؛ لا واحدة منها تقدم إعداداً جاهزاً وآمناً دون تعديل/اختبار.
- التقارير الستة: ARCHIVE_ONLY / HISTORICAL EVIDENCE؛ تبقى في الحزمة الأرشيفية ولا تكرر داخل المستودع الحي.
- لا حذف من ملفات المصدر، ولا تعديل لإعدادات الجذر الحالية.
- لا تشغيل codespell/markdownlint/gitleaks في هذه المرحلة، ولا ادعاء فحص أمني.

## الحدود والمتابعة
- لا يثبت هذا التقرير أن جميع ملفات الجذر في كل ZIP قد تمت مراجعتها؛ النطاق هو المسارات العشرة archive-only المحددة في مصفوفة المرحلة 33.
- تبقى مراجعة ملفات .github/ وtools/ وملفات الجذر ذات اختلاف المحتوى محكومة بتقارير مراحلها السابقة؛ لا يعاد فتحها بلا دليل جديد.
- المرحلة التالية تنتقل إلى مطابقة محتوى وثائق الأرشيف الفريدة التي لم تدخل في salvage سابق، مع تجنب إعادة مراجعة مجموعات security/quality/architecture/game systems المحسومة في المراحل 36–39.

**الحالة:** STAGE 48 COMPLETE — TEN ARCHIVE-ONLY ROOT PATHS TRIAGED; HISTORICAL REPORTS RETAINED OUTSIDE LIVE MAIN.
