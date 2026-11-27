# خريطة الأدوات — الموجود والفجوات

> تنبيه حوكمي: أرقام ADR المشار إليها في نسخ سابقة هي مواد أرشيفية ما لم توجد في `docs/adr/` بحالة مقبولة على `main`.

**SHA لقطة الشجرة:** `589e3beb65980f0b11c1207c3a905b76e2f01028` (2026-11-23). **إعادة قراءة إعدادات GitHub:** 2026-11-24. التشغيلات التاريخية على `1b209b79261cba617bb68615746f115970422a8f` موضحة في سجل الأدلة؛ أما التشغيلات على لقطة الشجرة الحالية فمبينة أدناه.

| المجال | الموجود | ما لا يثبته |
|---|---|---|
| Git/GitHub | `main` ومستودع GitHub | لا يثبت أن الفرع محمي؛ أعاد فحص 2026-11-24 عدم وجود حماية أو rulesets |
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

على SHA `bfbed74ae24e8e5e49baff84e7a18b0e99b55807`: Domain/Python CI run 45916990172، Core CI run 45916990210، وDocumentation Governance run 45916990106 اكتملت جميعها بنجاح. Domain/Python شغّل أيضاً `tools/github_workflow_tests` ونجحت. CodeQL dynamic run 45916990557 كان قيد التنفيذ عند آخر قراءة؛ لا تُسجل له نتيجة نجاح بعد. راجع [GITHUB_TOOLING.md](GITHUB_TOOLING.md) لسجل التشغيلات والحدود.


## إعدادات GitHub — قراءة محدودة الصلاحيات (2026-11-27)

Branch protection endpoint أعاد 403، لذا حالته UNKNOWN/NOT VERIFIED. Repository rulesets أعادت `[]` ضمن نطاق القراءة. Endpoints الخاصة بـDependabot/Code Scanning/Secret Scanning alerts غير مدعومة في التكامل الحالي؛ لا يمكن الجزم بوجود التنبيهات أو خلوها. لم تُغيّر الإعدادات. التفاصيل في [GITHUB_TOOLING.md](GITHUB_TOOLING.md) وتقرير المرحلة 54.
