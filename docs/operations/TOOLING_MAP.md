# خريطة الأدوات — الموجود والفجوات

> تنبيه حوكمي: أرقام ADR المشار إليها في نسخ سابقة هي مواد أرشيفية ما لم توجد في `docs/adr/` بحالة مقبولة على `main`.

**تاريخ SHA الأساسي:** 2026-11-23، SHA `1b209b79261cba617bb68615746f115970422a8f`. **إعادة قراءة إعدادات GitHub:** 2026-11-24.

| المجال | الموجود | ما لا يثبته |
|---|---|---|
| Git/GitHub | `main` ومستودع GitHub | لا يثبت أن الفرع محمي؛ أعاد فحص 2026-11-24 عدم وجود حماية أو rulesets |
| Domain | C# `netstandard2.1` واختبارات NUnit/.NET 8 | لا يثبت تشغيل خادم أو Unity |
| Core | C# `netstandard2.1` وخدمات/عقود واختبارات NUnit/.NET 10 | لا يثبت دمج قواعد Domain داخل Core |
| Domain/Python CI | `ci.yml`؛ run `44966048222` success على SHA المرجعي | لا يثبت الأمن أو التوازن أو الشبكة |
| Core CI | `core-ci.yml`؛ run `44966048268` success على SHA المرجعي | لا يثبت Unity أو server integration |
| CodeQL | إعداد GitHub الافتراضي وتشغيل Setup ديناميكي مسجل بنجاح على SHA الأساس | لا يوجد workflow متتبع؛ نجاح التشغيل لا يثبت خلو التنبيهات |
| محاكي Python | simulate/presets/sweep/test | لا يمثل قواعد C# ولا توازناً معتمداً |
| Playtest harness | جلسات محاكاة | ليست جلسات بشرية |

## فجوات مفتوحة

gitleaks وactionlint وzizmor وCODEOWNERS وفحوص network integration وUnity Test Framework وبناء/أداء Android وDedicated Server وBackend وhuman playtest ما زالت غير متاحة/غير منفذة في الشجرة المفحوصة. Secret scanning وPush protection مفعّلان في فحص 2026-11-24. كانت تنبيهات Dependabot ورسم الاعتماديات مسجلتين على أنهما مفعّلان في فحص 2026-11-22، لكن لم يُعد فحص إعداد التنبيهات؛ كما تعذرت قراءة قائمة التنبيهات في 2026-11-24 بسبب `403 Resource not accessible by integration`، ولا يُعرف محتواها أو عددها. Dependabot security updates معطّلة ولا يوجد `dependabot.yml`. تعذرت أيضاً قراءة تنبيهات Code Scanning بسبب 403. لا توجد repository rulesets وحماية `main` غائبة في فحص 2026-11-24.

قبل إضافة أي أداة: وثّق سبب الحاجة، الترخيص، الإصدار المثبت، صلاحيات التشغيل، مسارات CI، وكيفية التعامل مع false positives. لا تضف خدمة مدفوعة بلا موافقة مالك.
