# المرحلة 29 — مطابقة ملفات الجذر وإعدادات البناء العامة (2026-11-27)

## الرأس والنطاق

- HEAD قبل المرحلة: 2cae5a68b3f950a1b77e6ec68d6ded11688390b6.
- شملت المقارنة 10 ملفات جذر مشتركة بالاسم بين الأرشيفات وmain، و4 إعدادات أرشيفية فقط، و6 تقارير audit تاريخية موجودة في جذر الأرشيفات.

## Crosswalk الملفات المشتركة

| الملف | نسخ الأرشيف | النتيجة |
|---|---:|---|
| .editorconfig | 1 | PATH_EXISTS_CONTENT_DIFF |
| .gitignore | 2 | PATH_EXISTS_CONTENT_DIFF |
| CODE_OF_CONDUCT.md | 1 | PATH_EXISTS_CONTENT_DIFF |
| CONTRIBUTING.md | 1 | PATH_EXISTS_CONTENT_DIFF |
| CostOfStaying.sln | 2 | EXACT_GIT_BLOB_MATCH |
| Directory.Packages.props | 1 | PATH_EXISTS_CONTENT_DIFF |
| LICENSE | 1 | EXACT_GIT_BLOB_MATCH |
| README.md | 5 | PATH_EXISTS_CONTENT_DIFF |
| SECURITY.md | 1 | PATH_EXISTS_CONTENT_DIFF |
| global.json | 1 | EXACT_GIT_BLOB_MATCH |

النتيجة: 3 تطابقات بايتية و7 اختلافات محتوى. تبقى النسخ الحية للملفات السبعة المختلفة؛ لا استبدال.

## إعدادات أرشيفية فقط

- .codespellrc: إعداد codespell مع قائمة استثناءات قصيرة؛ لا توجد أداة/خط CI حية في main تثبت استخدامه.
- .gitattributes: يضبط ملفات Unity مثل .unity و.prefab و.asset و.meta كـbinary. هذا يحتاج مراجعة قبل الاستعمال؛ ملفات Unity النصية وخصوصاً .meta يفيد عادة إبقاؤها قابلة للمراجعة النصية، فلا يُعتمد هذا الإعداد كما هو.
- .gitleaks.toml: يمتد إلى القواعد الافتراضية فقط؛ لا يوجد workflow حي يستخدم هذا الملف حالياً.
- .markdownlint-cli2.jsonc: يفعّل مجموعة قواعد هيكلية ويستثني docs/audit؛ يحتاج حزمة/أمر تشغيل واضحاً قبل إضافته.

لا يُستورد أي من هذه الإعدادات لمجرد أنها موجودة في الأرشيف. يلزم تحديد أداة التنفيذ، الإصدار، workflow المستهلك، ومخرجاته المتوقعة، ثم اعتماد تغيير منفصل.

## تقارير الجذر التاريخية

ستة ملفات جذرية (audit_index.txt وbranch_comparison.txt وbranches_summary.txt وcommits_summary.txt وrepo_audit_report.txt وstatus_summary.txt) تطابق بايتياً نظائرها تحت docs/archive/legacy-full-backup-2026-11-15/؛ لا تُنشأ لها نسخ جذرية موازية في main. تمت مصالحتها في المرحلة 26.

## ملاحظات التوافق

- global.json الحي يثبت .NET SDK 8.0.400 مع latestFeature، بينما Core tests تستخدم global.json محلياً منفصلاً؛ لا تُستبدل الإعدادات المركزية دون مراعاة نطاق كل مشروع.
- Directory.Packages.props الحي يوضح صراحةً استثناء Core.Tests من Central Package Management عبر ملف أقرب. لا تُستبدل هذه البنية بنسخة أرشيفية دون مراجعة inheritance وPackageReference وSDK behavior.
- تطابق solution وLICENSE وglobal.json يعني تطابق البايتات مع نسخة أرشيفية، لا أنه يجب إعادة استيرادها.

## القرار

- KEEP_LIVE للملفات الحية المشتركة؛ 3 تطابقات و7 اختلافات.
- ARCHIVE_ONLY / REVIEW_BEFORE_ADOPTION للإعدادات الأربعة.
- لا نسخ جذرية مكررة لتقارير audit الستة.
- لا حذف أو استبدال أو نقل أو استيراد.
- لا تشغيل build أو lint أو secret scan في هذه المرحلة.

**الحالة:** STAGE 29 COMPLETE — ROOT CONFIG CROSSWALK; FOUR ARCHIVE-ONLY TOOL CONFIGS REQUIRE EXPLICIT TOOLCHAIN ADOPTION.
