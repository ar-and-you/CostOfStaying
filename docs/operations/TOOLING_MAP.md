# خريطة الأدوات — الموجود والفجوات

> تنبيه حوكمي: أرقام ADR المشار إليها في نسخ سابقة هي مواد أرشيفية ما لم توجد في `docs/adr/` بحالة مقبولة على `main`.

**لقطة `main` الحالية:** `e7e5b5e54f46490c44aaf30888280d55239bb670` (2026-11-27)؛ لم تُرجع القراءة الحالية تشغيلات CI مرتبطة بهذا SHA، لذا لا يُسجل نجاح CI حديث على `main`. **فرع المراجعة الأمني:** `security/core-trust-boundaries-phase0` عند `5f8db71a34ed4b680dd0d7a0d24f0726e6c8924a`؛ اكتملت Domain/Python CI وCore CI وDocumentation Governance بنجاح (runs `46078265183`, `46078265114`, `46078265167` على الترتيب). أحدث CodeQL موثق يخص SHA `60d15bc8f7aafb99f31ea2d61a6fd750719e53d8` فقط، ولا يُسقط على الرأسين الحاليين. جميع الأدلة مقيدة بالـSHA ولا تثبت تكاملات غير موجودة.

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

gitleaks وactionlint وzizmor وCODEOWNERS وفحوص network integration وUnity Test Framework وبناء/أداء Android وDedicated Server وBackend وhuman playtest ما زالت غير متاحة/غير منفذة في الشجرة المفحوصة. Secret scanning وPush protection مفعّلان في فحص 2026-11-24. كانت تنبيهات Dependabot ورسم الاعتماديات مسجلتين على أنهما مفعّلان في فحص 2026-11-22، لكن لم يُعد فحص إعداد التنبيهات؛ كما تعذرت قراءة قائمة التنبيهات في 2026-11-24 بسبب `403 Resource not accessible by integration`، ولا يُعرف محتواها أو عددها. Dependabot security updates معطّلة ولا يوجد `dependabot.yml`. تعذرت أيضاً قراءة تنبيهات Code Scanning بسبب 403. لا توجد repository rulesets وحماية `main` غائبة في فحص 2026-11-24.

قبل إضافة أي أداة: وثّق سبب الحاجة، الترخيص، الإصدار المثبت، صلاحيات التشغيل، مسارات CI، وكيفية التعامل مع false positives. لا تضف خدمة مدفوعة بلا موافقة مالك.


## فحص ثابت لسلامة Workflows

أضيفت `tools/github_workflow_tests/` واختباراتها ضمن `ci.yml` باستخدام Python standard library فقط. تتحقق من:
- صلاحية `contents: read` على مستوى Workflow.
- تثبيت مراجع Actions على SHA كامل من 40 خانة.
- timeout موجب لكل Job.
- تعطيل `persist-credentials` في خطوات checkout.
- عدم إدخال `pull_request_target` أو `workflow_run` دون مراجعة أمنية مستقلة.

هذا فحص ثابت ضيق لنمط الملفات الحالية؛ لا يحل محل actionlint أو zizmor أو قراءة إعدادات GitHub أو Secret Scanning، ولا يثبت سلامة runtime.


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
