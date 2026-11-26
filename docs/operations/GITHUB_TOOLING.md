# أدوات GitHub — الحالة الفعلية وخطة الاستكمال

**آخر مراجعة قراءة فقط لإعدادات GitHub:** 2026-11-24
**SHA الأساسي لنتائج CI وCodeQL الموثقة تاريخياً:** `1b209b79261cba617bb68615746f115970422a8f`
**SHA لتشغيلات المتابعة:** `589e3beb65980f0b11c1207c3a905b76e2f01028`.

تسجل هذه الوثيقة ما فُحص فعلياً وما بقي غير مؤكد. نجاح CI يثبت الاختبارات التي شغّلها فقط؛ ولا يثبت اكتمال الأمن أو الحوكمة. نتائج SHA الأساس التاريخية ونتائج SHA المتابعة منفصلة أدناه.

## 0. لقطة التشغيل الحالية — SHA `2e0304bf5151b77b618053f664efaf5ad6efdef5`

تحتوي الشجرة على **ثلاثة Workflows متتبعة**، إضافة إلى تشغيل CodeQL الديناميكي الذي لا يقابله ملف Workflow متتبع:

| Workflow | Run ID | الحالة | ما يثبته |
|---|---:|---|---|
| Domain and Python CI | [45704564814](https://github.com/ar-and-you/CostOfStaying/actions/runs/45704564814) | `completed/success` | اختبارات Domain على .NET 8 ومحاكي Python كما يحدده `ci.yml` |
| Core CI (.NET 10) | [45704564923](https://github.com/ar-and-you/CostOfStaying/actions/runs/45704564923) | `completed/success` | بناء واختبارات Core كما يحددها `core-ci.yml` |
| Documentation Governance | [45704564834](https://github.com/ar-and-you/CostOfStaying/actions/runs/45704564834) | `completed/success` | تدقيق روابط Markdown وقابلية الوصول من `docs/README.md` |
| CodeQL default setup (dynamic) | [45704564631](https://github.com/ar-and-you/CostOfStaying/actions/runs/45704564631) | `completed/success` | تشغيل CodeQL الديناميكي؛ ليس Workflow متتبعاً في المستودع |

هذه النتائج تخص SHA المذكور فقط، وهو **الرأس السابق لدفعة الإصلاح الحالية**. لا تُنسب إلى commit الإصلاح حتى تُقرأ تشغيلاته الفعلية. نجاح CodeQL لا يثبت خلو قائمة التنبيهات، كما أن تعذر قراءة التنبيهات بصلاحية API لا يعني عدم وجودها.


## 1. Workflows ونتائج CI

| الملف | الغرض | النتيجة على SHA `1b209b7` |
|---|---|---|
| `.github/workflows/ci.yml` | اختبارات Domain على .NET 8 ومحاكي Python | نجح التشغيل [44966048222](https://github.com/ar-and-you/CostOfStaying/actions/runs/44966048222)، والوظيفة `Domain (.NET 8) and simulator regressions (Python)` نجحت (job `137035203934`) |
| `.github/workflows/core-ci.yml` | اختبارات Core المستقل باستخدام .NET 10 | نجح التشغيل [44966048268](https://github.com/ar-and-you/CostOfStaying/actions/runs/44966048268)، والوظيفة `Build and test standalone Core` نجحت (job `137035204253`) |

كلا التشغيلين `completed/success` ويرتبطان بالـSHA الأساسي `1b209b79261cba617bb68615746f115970422a8f`. يستخدم كل workflow صلاحية `contents: read`، ومراجع Actions مثبتة على commit SHAs كاملة. هذه نتائج على SHA الأساس، وليست تشغيلات على التغييرات الوثائقية الحالية. لا يشغّل هذان الملفان CodeQL أو gitleaks أو فحصاً متخصصاً لسلاسل التوريد.

### تشغيلات متابعة على `main` عند SHA `589e3be`

| الفحص | Run ID | الحالة | النطاق |
|---|---:|---|---|
| Domain and Python CI | [45283036745](https://github.com/ar-and-you/CostOfStaying/actions/runs/45283036745) | `completed/success` | اختبارات Domain على .NET 8 واختبارات محاكي Python المحددة في workflow |
| Core CI (.NET 10) | [45283036869](https://github.com/ar-and-you/CostOfStaying/actions/runs/45283036869) | `completed/success` | بناء واختبارات Core المحددة في workflow |
| CodeQL — تشغيل إعداد افتراضي ديناميكي | [45283036409](https://github.com/ar-and-you/CostOfStaying/actions/runs/45283036409) | `completed/success` | تشغيل CodeQL ديناميكي؛ لا يقابله workflow متتبع في المستودع |

تخص التشغيلات الثلاثة أعلاه SHA `589e3beb65980f0b11c1207c3a905b76e2f01028`، ولا توسّع نطاق الاختبارات المسجل لكل workflow. نجاح CodeQL لا يثبت أن قائمة التنبيهات قُرئت أو أنها خالية.

## 2. إعدادات الأمن والاعتماديات

- **Secret scanning وPush protection:** مفعّلان حسب قراءة إعدادات المستودع في 2026-11-24. فحص الأنماط غير التابعة لمزود وفحوص صلاحية الأسرار معطّلان. لا يوجد تقرير حديث يثبت مسحاً كاملاً لتاريخ Git بأداة متخصصة.
- **Dependabot alerts وقراءة التنبيهات:** كانت المراجعة السابقة بتاريخ 2026-11-22 تسجل أن تنبيهات Dependabot ورسم الاعتماديات مفعّلان. في إعادة التحقق بتاريخ 2026-11-24 أعادت `GET /dependabot/alerts?per_page=1` الخطأ `403 Resource not accessible by integration`؛ لذلك لا يمكن تأكيد عدد التنبيهات أو محتواها أو خلو المستودع منها. حالة تفعيل التنبيهات ورسم الاعتماديات لم تُحسم مجدداً بهذه القراءة.
- **Dependabot security updates:** معطّلة في قراءة 2026-11-24. لم يُضف `.github/dependabot.yml`، ولن تُفعّل التحديثات الأمنية الآلية دون توجيه جديد من مالك المشروع؛ تجنباً لفتح PRs آلية. لا تخلط بين تنبيهات Dependabot (التي لم تُقرأ) والتحديثات الآلية.
- **CodeQL:** أظهرت المراجعة المؤرخة 2026-11-22 إعداد GitHub default setup للغات C# وPython وGitHub Actions، باستخدام query suite `default` وthreat model `remote_and_local`. لا ينتج ذلك ملف workflow في المستودع. اكتمل التشغيل الأول [45004680726](https://github.com/ar-and-you/CostOfStaying/actions/runs/45004680726) بحالة `completed/success` على SHA الأساس `1b209b79261cba617bb68615746f115970422a8f`. لم تُعد قراءة إعداد CodeQL في فحص 2026-11-24.
- **قراءة Code Scanning:** أعادت `GET /code-scanning/alerts?per_page=1` في 2026-11-24 الخطأ `403 Resource not accessible by integration`. نجاح تشغيل CodeQL لا يكشف وحده قائمة التنبيهات؛ لا يمكن الجزم بعدم وجود نتائج.
- لا يوجد في المستودع حالياً `security.yml` أو gitleaks أو actionlint أو zizmor workflow، ولا اختبارات إعداد GitHub في `tools/github_config_tests/`.

## 3. حماية الفرع والحوكمة

- `main` هو الفرع الافتراضي. أعاد فحص الحماية بتاريخ 2026-11-24 `404 Branch not protected`، وكانت قائمة repository rulesets مع `includes_parents=true` فارغة (`[]`).
- تنص سياسة `ROADMAP.md` الحالية على الدفع المباشر إلى `main`. لم تُفعّل حماية قد تعارضها، ولم يُفرض شرط Pull Request أو required checks.
- لا يُستخدم force push أو إعادة كتابة للتاريخ.
- قبل إنشاء ruleset نشط، يلزم اختيار المالك بين: (أ) اعتماد PRs وفحوص required checks، بما يغيّر سياسة الدفع المباشر؛ أو (ب) الإبقاء على الدفع المباشر مع حظر force-push والحذف فقط. لا تُنشأ القاعدة قبل هذا الاختيار.
- إذا اختيرت سياسة PR لاحقاً، فأسماء الوظائف التي شُغّلت فعلياً هي `Domain (.NET 8) and simulator regressions (Python)` و`Build and test standalone Core`، ويجب التحقق من ظهورها على أحدث SHA قبل فرضها كفحوص مطلوبة.

## 4. التحقق المحلي وحدوده

نجحت اختبارات محاكي Python سابقاً `7/7` في Sandbox باستخدام Python 3.12.3. لم يُشغّل .NET محلياً في تلك المراجعة لعدم توفر SDK؛ نتائج Domain وCore أعلاه من GitHub Actions على SHA الأساس. لا يثبت ذلك تكامل Unity أو parity بين C# وPython أو سلامة تطبيق/خادم شبكي غير موجود. يجب تسجيل أي تشغيل لاحق مع SHA الدقيق والبيئة والأمر والنتيجة.

## 5. ما بقي

تظل مطلوبة مراجعة تنبيهات Dependabot بعد إتاحة صلاحية القراءة، ومراجعة التنبيهات الناتجة من Code Scanning بعد منح صلاحية القراءة، وإعادة قراءة حالة إعداد CodeQL، وحسم إعداد حماية `main` مع المالك. تحديثات Dependabot الأمنية الآلية وملف `dependabot.yml` مؤجلان حتى يعتمد المالك سلوك PRs التلقائية. تبقى أعمال gitleaks وفحوص workflow المتخصصة خارج الإعدادات المفعّلة حالياً.
