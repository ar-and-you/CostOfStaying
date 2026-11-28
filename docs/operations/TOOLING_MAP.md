# خريطة الأدوات — الموجود والفجوات

> تنبيه حوكمي: أرقام ADR المشار إليها في نسخ سابقة هي مواد أرشيفية ما لم توجد في `docs/adr/` بحالة مقبولة على `main`.

**لقطة `main` الحالية:** `e7e5b5e54f46490c44aaf30888280d55239bb670` (2026-11-27)؛ لم تُرجع القراءة تشغيلات CI مرتبطة بهذا SHA، فلا نسجل نجاحاً حديثاً على `main`. **آخر رأس مصدر/اختبارات متحقق على فرع المراجعة الأمني:** `d8c66e85d2d4b8353595b24dd2df38382ead2adb` (2026-11-28). تلت هذا الرأس تحديثات توثيقية فقط؛ تشغيلات CI على أحدث رأس بعد هذه التحديثات قيد الانتظار. اكتملت Domain/Python CI (run `46131891429`)، وCore CI (.NET 10) (run `46131891394`)، وDocumentation Governance (run `46131891367`) بنجاح على SHA نفسه. اجتاز `actionlint v1.7.12` و`zizmor v1.33.1`، وworkflow hygiene (7/7)، وDomain NUnit (62/62)، وCore NUnit (257/257)، واختبارات simulator (11)، وCLI (2/2)، وsmoke test (1). CodeQL C#/Python/Actions اكتمل بنجاح، وCodacy أعلن 0 issues على SHA الحالي. CodeFactor ما زال `failure` مع Minor واحدة؛ لا تتوفر annotation قابلة للفحص لتحديد الملف/السطر، لذلك لم تُجرَ إصلاحات تخمينية. جميع الأدلة مقيدة بالـSHA ولا تثبت تكاملات غير موجودة.

| المجال | الموجود | ما لا يثبته |
|---|---|---|
| Git/GitHub | `main` ومستودع GitHub؛ آخر قراءة branch protection بتاريخ 2026-11-27 أعادت 403، والحالة UNKNOWN/NOT VERIFIED؛ repository rulesets أعادت `[]` ضمن نطاق القراءة | لا يثبت أن `main` محمي أو غير محمي على مستوى المؤسسة أو عبر قواعد غير متاحة للتكامل |
| Domain | C# `netstandard2.1` واختبارات NUnit/.NET 8 | لا يثبت تشغيل خادم أو Unity |
| Core | C# `netstandard2.1` وخدمات/عقود واختبارات NUnit/.NET 10 | لا يثبت دمج قواعد Domain داخل Core |
| Domain/Python CI | `ci.yml`؛ run `45283036745` `completed/success` على SHA `589e3be` | يثبت الوظائف المحددة في workflow فقط؛ لا يثبت الأمن أو التوازن أو الشبكة |
| Core CI | `core-ci.yml`؛ run `45283036869` `completed/success` على SHA `589e3be` | يثبت بناء واختبارات Core المحددة فقط؛ لا يثبت Unity أو server integration |
| CodeQL | تشغيل إعداد افتراضي ديناميكي `45283036409` `completed/success` على SHA `589e3be` | لا يوجد workflow متتبع؛ نجاح التشغيل لا يثبت خلو التنبيهات |
| محاكي Python | simulate/presets/sweep/test | لا يمثل قواعد C# ولا توازناً معتمداً |
| Playtest harness | جلسات محاكاة | ليست جلسات بشرية |

## فجوات مفتوحة

أضيف `actionlint` و`zizmor` إلى `.github/workflows/ci.yml` على فرع المراجعة الأمني، وقد نجحا على SHA `d8c66e85d2d4b8353595b24dd2df38382ead2adb`؛ لم يدمجا في `main`. أما gitleaks وCODEOWNERS وفحوص network integration وUnity Test Framework وبناء/أداء Android وDedicated Server وBackend وhuman playtest فما زالت غير متاحة/غير منفذة في الشجرة المفحوصة. Secret scanning وPush protection آخر حالة موثقة لهما في 2026-11-24 ولم تُؤكدا مجدداً. تعذرت قراءة تنبيهات Dependabot وCode Scanning وSecret Scanning بسبب حدود صلاحيات التكامل، فلا يُعرف محتواها أو عددها. Dependabot security updates معطّلة وفق آخر قراءة موثقة ولا يوجد `dependabot.yml`. Branch protection endpoint أعاد 403 في 2026-11-28؛ الحالة UNKNOWN/NOT VERIFIED. Repository rulesets أعادت `[]` ضمن نطاق القراءة فقط، ولا يثبت ذلك غياب الحماية الموروثة أو على مستوى المؤسسة.

قبل إضافة أي أداة: وثّق سبب الحاجة، الترخيص، الإصدار المثبت، صلاحيات التشغيل، مسارات CI، وكيفية التعامل مع false positives. لا تضف خدمة مدفوعة بلا موافقة مالك.


## فحص ثابت لسلامة Workflows

أضيفت `tools/github_workflow_tests/` واختباراتها ضمن `ci.yml` باستخدام Python standard library فقط. تتحقق من:
- صلاحية `contents: read` على مستوى Workflow.
- تثبيت مراجع Actions على SHA كامل من 40 خانة.
- timeout موجب لكل Job.
- تعطيل `persist-credentials` في خطوات checkout.
- عدم إدخال `pull_request_target` أو `workflow_run` دون مراجعة أمنية مستقلة.

هذا فحص ثابت ضيق ومكمّل لنمط الملفات الحالية؛ actionlint يتحقق من صياغة Workflows، وzizmor يجري تحليلاً ثابتاً لمخاطر GitHub Actions. لا تحل هذه الأدوات محل قراءة إعدادات GitHub أو Secret Scanning أو اختبارات runtime.


## تحقق حي حديث — 2026-11-27

على SHA `bfbed74ae24e8e5e49baff84e7a18b0e99b55807`: Domain/Python CI run 45918459199، Core CI run 45918459203، وDocumentation Governance run 45918459124 اكتملت جميعها بنجاح. Domain/Python شغّل أيضاً `tools/github_workflow_tests` ونجحت. CodeQL dynamic run 45918459371 كان قيد التنفيذ عند آخر قراءة؛ لا تُسجل له نتيجة نجاح بعد. راجع [GITHUB_TOOLING.md](GITHUB_TOOLING.md) لسجل التشغيلات والحدود.


## إعدادات GitHub — قراءة محدودة الصلاحيات (2026-11-27)

Branch protection endpoint أعاد 403، لذا حالته UNKNOWN/NOT VERIFIED. Repository rulesets أعادت `[]` ضمن نطاق القراءة. Endpoints الخاصة بتنبيهات Dependabot وCode Scanning وSecret Scanning غير متاحة في التكامل الحالي؛ لا يمكن الجزم بوجود التنبيهات أو خلوها. Secret scanning وPush protection آخر حالة موثقة لهما تعود إلى 2026-11-24 ولم تُؤكد مجدداً. لم تُغيّر الإعدادات. التفاصيل في [GITHUB_TOOLING.md](GITHUB_TOOLING.md) وتقرير المرحلة 54.


## تحقق حي أحدث — 2026-11-27 — SHA `94b341a681930e875fdbeee6ff3ccecb77476a14`

- Domain/Python CI run 45919285815: Domain NUnit 62/62، Python simulator 11/11، وworkflow hygiene 6/6؛ success.
- Core CI run 45919285831: NUnit 248/248؛ success.
- Documentation Governance run 45919285865: success.
- CodeQL dynamic run 45919285741: C# وPython وActions جميعها completed/success.
- هذه نتائج على SHA المحدد فقط. CodeQL success لا يثبت خلو alerts؛ إعدادات branch protection وDependabot/Code Scanning/Secret Scanning alerts لا تزال غير قابلة للتحقق من صلاحيات التكامل الحالية.


## تحقق CI حي على رأس main — 2026-11-27 — SHA `60d15bc8f7aafb99f31ea2d61a6fd750719e53d8`

| الفحص | Run ID | Check run IDs | النتيجة |
|---|---:|---|---|
| Domain and Python CI | 45979032679 | 140440309145 | completed / success |
| Core CI (.NET 10) | 45979032628 | 140437943795 | completed / success |
| Documentation Governance | 45979032548 | 140440278573 | completed / success |
| CodeQL dynamic — C# / Python / Actions | 45979033244 | 140437951581 / 140437950908 / 140437951445 | completed / success |

جميع التشغيلات الأربعة أعلاه حدثت على SHA نفسه، وهو main HEAD في لقطة الفحص. نجاح Domain/Python وCore يثبت فقط الاختبارات المحددة في Workflows؛ ونجاح Documentation Governance يثبت فحوص الحوكمة المهيأة. اسم تشغيل CodeQL في Actions هو `Push on main`، لكن check-runs الفعلية تُظهر ثلاث وظائف Analyze (csharp/python/actions) مكتملة بنجاح. لا يثبت نجاح CodeQL خلو قائمة التنبيهات. كما لا يثبت أي من هذه التشغيلات وجود Unity أو Android build أو transport/network runtime أو Dedicated Server أو human playtest.
