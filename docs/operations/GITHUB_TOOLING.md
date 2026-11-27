# تحديث حالة الرأس — 2026-11-28

- `main` HEAD عند آخر قراءة: `e7e5b5e54f46490c44aaf30888280d55239bb670`؛ لم تُرجع القراءة الحالية تشغيلات CI مرتبطة بهذا SHA، لذا لا يُسجل نجاح حديث على `main`.
- فرع المراجعة `security/core-trust-boundaries-phase0` عند SHA `5f8db71a34ed4b680dd0d7a0d24f0726e6c8924a` أكمل Domain/Python CI (run `46078265183`)، وCore CI (.NET 10) (run `46078265114`)، وDocumentation Governance (run `46078265167`) بنجاح. هذه التشغيلات تخص ذلك الرأس قبل التعديلات الوثائقية الحالية، ويجب إعادة التحقق على SHA الناتج.
- أحدث CodeQL موثق في هذا السجل يخص SHA `60d15bc8f7aafb99f31ea2d61a6fd750719e53d8`؛ لا يوجد دليل CodeQL على `main` الحالي أو رأس فرع المراجعة المذكور.
- نتائج التشغيلات مقيدة بالـSHA المذكور، ولا تثبت وجود Unity أو Android أو transport/server runtime أو خلو تنبيهات CodeQL.
- آخر قراءة branch-protection بتاريخ 2026-11-28 أعادت 403؛ الحالة UNKNOWN/NOT VERIFIED. قراءة repository rulesets أعادت `[]` ضمن النطاق المتاح ولا تحسم قواعد المؤسسة أو الحماية الموروثة.
- لم تُغيّر إعدادات GitHub في هذه المراجعة.

## 0.6 تحقق CI حي على SHA السابق — `94b341a681930e875fdbeee6ff3ccecb77476a14` (2026-11-27)

| الفحص | Run ID | Job ID | النتيجة |
|---|---:|---:|---|
| Domain and Python CI | 45919285815 | 140245891186 | success — Domain NUnit 62/62؛ Python simulator 11/11؛ workflow hygiene 6/6 |
| Core CI (.NET 10) | 45919285831 | 140245891232 | success — Core NUnit 248/248 |
| Documentation Governance | 45919285865 | 140245891424 | success |
| CodeQL dynamic — C# | 45919285741 | 140245895248 | success |
| CodeQL dynamic — Python | 45919285741 | 140245895605 | success |
| CodeQL dynamic — Actions | 45919285741 | 140245895659 | success |

هذه النتائج مرتبطة حصراً بـSHA `94b341a681930e875fdbeee6ff3ccecb77476a14`. سجلات الوظائف تؤكد 62/62 Domain، و248/248 Core، و11 اختبار simulator، و6 اختبارات workflow hygiene. نجاح CodeQL لا يكشف قائمة alerts ولا يثبت خلوها. لم تتغير إعدادات GitHub؛ قراءة branch protection والتنبيهات ما زالت محدودة الصلاحيات.

---



## 0.7 تحقق CI وCodeQL على HEAD الحالي — SHA `60d15bc8f7aafb99f31ea2d61a6fd750719e53d8` (2026-11-27)

| الفحص | Run ID | Check run IDs | النتيجة |
|---|---:|---|---|
| Domain and Python CI | [45979032679](https://github.com/ar-and-you/CostOfStaying/actions/runs/45979032679) | 140440309145 | completed / success |
| Core CI (.NET 10) | [45979032628](https://github.com/ar-and-you/CostOfStaying/actions/runs/45979032628) | 140437943795 | completed / success |
| Documentation Governance | [45979032548](https://github.com/ar-and-you/CostOfStaying/actions/runs/45979032548) | 140440278573 | completed / success |
| CodeQL dynamic — C# / Python / Actions | [45979033244](https://github.com/ar-and-you/CostOfStaying/actions/runs/45979033244) | 140437951581 / 140437950908 / 140437951445 | completed / success |

أكدت قراءة check-runs أن الوظائف الست كلها مرتبطة بالـSHA نفسه. اسم تشغيل CodeQL في Actions هو `Push on main`؛ أما أسماء الفحوص الفعلية فهي Analyze (csharp)، Analyze (python)، وAnalyze (actions)، وجميعها اكتملت بنجاح. نجاح التحليل لا يكشف قائمة alerts ولا يثبت خلوها. كما لا تثبت هذه التشغيلات تكامل Unity/Android أو transport/Dedicated Server أو human playtest.

# أدوات GitHub — الحالة الفعلية وخطة الاستكمال

## 0.5 تحقق CI بعد تقوية fail-closed dequeue — SHA `d9c4e93e94c38d1c91c25414c96174eb4d8fa78e` (2026-11-27)

| الفحص | Run ID | النتيجة |
|---|---:|---|
| Domain and Python CI | [45918459199](https://github.com/ar-and-you/CostOfStaying/actions/runs/45918459199) | success — 62/62 Domain tests |
| Core CI (.NET 10) | [45918459203](https://github.com/ar-and-you/CostOfStaying/actions/runs/45918459203) | success — 248/248 Core tests |
| Documentation Governance | [45918459124](https://github.com/ar-and-you/CostOfStaying/actions/runs/45918459124) | success |
| CodeQL dynamic | [45918459371](https://github.com/ar-and-you/CostOfStaying/actions/runs/45918459371) | Python وActions success؛ C# in_progress عند آخر قراءة |

هذا التشغيل يتضمن اختبارَي post-dequeue exception المضافين في المرحلة 62. النتائج تخص SHA d9c4e93 فقط؛ CodeQL C# لم يكتمل، ولا يثبت التشغيل خلو alerts أو وجود transport/server/Unity runtime.

## 0.5 قراءة إعدادات GitHub محدودة الصلاحيات — SHA `35ddccfb149ba70b0fdc9c20ecbc6ebf8ccda5ed` (2026-11-27)

- Branch protection endpoint على `main`: HTTP 403 `Resource not accessible by integration`؛ الحالة `UNKNOWN / NOT VERIFIED`.
- Repository rulesets مع `includes_parents=true`: `[]` في نطاق القراءة المتاح.
- Dependabot alerts وCode Scanning alerts وSecret Scanning alerts: endpoints غير مدعومة في GitHub integration الحالي؛ الحالة UNKNOWN، ولا يجوز استنتاج عدم وجود تنبيهات.
- لم تتغير أي إعدادات. راجع [تقرير المرحلة 54](ARCHIVE_RECONCILIATION_STAGE54_GITHUB_SETTINGS_READONLY_2026-11-27.md).


## 0.4 أحدث تحقق مباشر — SHA `bfbed74ae24e8e5e49baff84e7a18b0e99b55807` (2026-11-27)

| الفحص | Run ID | النتيجة |
|---|---:|---|
| Domain and Python CI | [45916990172](https://github.com/ar-and-you/CostOfStaying/actions/runs/45916990172) | `completed / success` |
| Core CI (.NET 10) | [45916990210](https://github.com/ar-and-you/CostOfStaying/actions/runs/45916990210) | `completed / success` |
| Documentation Governance | [45916990106](https://github.com/ar-and-you/CostOfStaying/actions/runs/45916990106) | `completed / success` |
| CodeQL dynamic | [45916990557](https://github.com/ar-and-you/CostOfStaying/actions/runs/45916990557) | `in_progress` وقت التحديث |

تضمنت Domain/Python نجاح Python compileall، واختبارات `tools/github_workflow_tests`، وDomain NUnit، واختبارات محاكي Python وCLI. اكتملت Core NUnit بنجاح. نجح فحص روابط Markdown وإمكانية الوصول من الفهرس؛ خطوة PR scope guard تخطت في Push event كما هو متوقع. هذه النتائج تخص SHA أعلاه فقط. لا يثبت نجاح CodeQL خلو التنبيهات، ولا تثبت هذه التشغيلات وجود Unity أو خادم أو transport أو human playtest.


## 0.3 أحدث تحقق مباشر — SHA `3c4c27ad16a2b5d8547f11a181ea75f3cf54f9fe` (2026-11-26)

تم ربط تشغيلات Push بنتيجة الالتزام نفسه بعد تحديث المهارات الست وفهرسة تقارير مصالحة الأرشيف:

| الفحص | Run ID | النتيجة |
|---|---:|---|
| Core CI (.NET 10) | 45892814768 | `completed / success` |
| Domain and Python CI | 45892814794 | `completed / success` |
| Documentation Governance | 45892814780 | `completed / success` |
| CodeQL dynamic — C# / Python / Actions | 45892814453 | `completed / success` |

تفاصيل التنفيذ: Core NUnit نجح؛ Domain NUnit نجح؛ Python 3.12 syntax ومحاكي Python واختبارات CLI للمحاكاة نجحت؛ فحص روابط Markdown وإمكانية الوصول من فهرس الوثائق نجح. خطوة نطاق التوثيق المقيدة بحدث Pull Request كانت `skipped` في حدث Push، وهو متوقع ولا يمثل فشلاً. وظائف CodeQL للغات C# وPython وGitHub Actions اكتملت بنجاح.

نجاح تحليل CodeQL لا يثبت أن قائمة Code Scanning alerts خالية؛ لم تُقرأ قائمة التنبيهات في هذا التحقق. كما لا يثبت هذا التشغيل وجود Unity أو Android build أو transport/network runtime أو human playtest.


## 0.2 أحدث تحقق حي — SHA `92b63dbaabb3f5cdebaa3fce30f35031fac60e6f` (2026-11-26)

أعيد التحقق من رأس `main` وربط نتائج Actions بالـSHA نفسه. اكتملت جميع التشغيلات التالية بنجاح:

| الفحص | Run ID | النتيجة |
|---|---:|---|
| Domain and Python CI | [45827062818](https://github.com/ar-and-you/CostOfStaying/actions/runs/45827062818) | `completed/success` |
| Core CI (.NET 10) | [45827062740](https://github.com/ar-and-you/CostOfStaying/actions/runs/45827062740) | `completed/success` |
| Documentation Governance | [45827062727](https://github.com/ar-and-you/CostOfStaying/actions/runs/45827062727) | `completed/success` |
| CodeQL dynamic — C# / Python / Actions | [45827062137](https://github.com/ar-and-you/CostOfStaying/actions/runs/45827062137) | `completed/success` |

تفاصيل Domain/Python تؤكد نجاح Python syntax وDomain NUnit واختبارات محاكي Python واختبارات CLI لمحاكاة الجلسات. وتفاصيل Core تؤكد نجاح Core NUnit. نجح تدقيق روابط Markdown وقابلية الوصول من فهرس الوثائق؛ تخطّت خطوة حارس نطاق PR لأنها لم تكن PR event. وأكملت وظائف CodeQL للغات C# وPython وGitHub Actions خطوات التحليل بنجاح.

هذه النتائج تخص SHA `92b63dbaabb3f5cdebaa3fce30f35031fac60e6f` فقط. نجاح CodeQL لا يكشف قائمة التنبيهات ولا يثبت خلوها. كما لا تثبت هذه التشغيلات وجود Unity أو Android أو transport أو Dedicated Server أو تكافؤ C#↔Python أو جلسات لعب بشرية.

**حالة الفرع في القراءة الحية:** `main` عند SHA أعلاه، وGitHub يعيد `protected: false` و`protection.enabled: false` في endpoint الفرع؛ لم تُغيّر إعدادات الحماية. لا تزال نتائج قواعد المؤسسة أو إعدادات غير متاحة خارج نطاق هذه القراءة غير محسومة.

---

## 0.1 أحدث تحقق حي — SHA `ffb45991bb5cce06c7be665f28ef41f160424a3c` (2026-11-26)

أعيد التحقق من رأس `main` وسجلات Actions بعد إضافة تحقق موجب لعدد جلسات أداة المحاكاة واختبارات CLI الخاصة به. اكتملت التشغيلات التالية بنجاح على SHA نفسه:

| الفحص | Run ID | النتيجة |
|---|---:|---|
| Domain and Python CI | [45800901162](https://github.com/ar-and-you/CostOfStaying/actions/runs/45800901162) | `completed/success` |
| Core CI (.NET 10) | [45800901177](https://github.com/ar-and-you/CostOfStaying/actions/runs/45800901177) | `completed/success` |
| Documentation Governance | [45800901295](https://github.com/ar-and-you/CostOfStaying/actions/runs/45800901295) | `completed/success` |
| Push on main (dynamic security workflow) | [45800901845](https://github.com/ar-and-you/CostOfStaying/actions/runs/45800901845) | `completed/success` |

تؤكد تفاصيل وظيفة Domain/Python نجاح خطوات Domain NUnit وPython simulator و`tools/playtest/test_cli.py`. وتؤكد تفاصيل Core CI نجاح اختبارات Core NUnit. تخطّت Documentation Governance خطوة حارس نطاق PR لأنها لم تكن PR event؛ نجح تدقيق روابط Markdown وقابلية الوصول من الفهرس. اسم تشغيل `Push on main` لا يكفي وحده لتسمية الفحص CodeQL؛ لا تُنسب إليه تغطية لغات أو قواعد تحليل محددة دون فحص تفاصيله.

هذه النتائج تخص SHA `ffb45991bb5cce06c7be665f28ef41f160424a3c` فقط. لا تثبت Unity أو Android أو transport أو أداء منصة أو جلسات لعب بشرية أو خلو تنبيهات CodeQL/Dependabot.


**آخر مراجعة قراءة فقط لإعدادات GitHub:** 2026-11-26 (مع endpoint حماية الفرع محجوباً بصلاحية 403)
**SHA الأساسي لنتائج CI وCodeQL الموثقة تاريخياً:** `1b209b79261cba617bb68615746f115970422a8f`
**SHA لتشغيلات المتابعة:** `589e3beb65980f0b11c1207c3a905b76e2f01028`.

تسجل هذه الوثيقة ما فُحص فعلياً وما بقي غير مؤكد. نجاح CI يثبت الاختبارات التي شغّلها فقط؛ ولا يثبت اكتمال الأمن أو الحوكمة. نتائج SHA الأساس التاريخية ونتائج SHA المتابعة منفصلة أدناه.

## 0. لقطة التحقق الحية — SHA `ad22b9ddb21ca48f9e30aa7f3560cf3e56f632c4` (2026-11-26)

أعيدت قراءة رأس `main` وcheck-runs مباشرة. اكتملت الوظائف الست التالية بنجاح على SHA نفسه:

| الفحص | Run ID | النتيجة |
|---|---:|---|
| Domain and Python CI | [45786760809](https://github.com/ar-and-you/CostOfStaying/actions/runs/45786760809) | `completed/success` |
| Core CI (.NET 10) | [45786760709](https://github.com/ar-and-you/CostOfStaying/actions/runs/45786760709) | `completed/success` |
| Documentation Governance | [45786760800](https://github.com/ar-and-you/CostOfStaying/actions/runs/45786760800) | `completed/success` |
| CodeQL dynamic — C# | [45786760616](https://github.com/ar-and-you/CostOfStaying/actions/runs/45786760616) | `completed/success` |
| CodeQL dynamic — Python | [45786760616](https://github.com/ar-and-you/CostOfStaying/actions/runs/45786760616) | `completed/success` |
| CodeQL dynamic — Actions | [45786760616](https://github.com/ar-and-you/CostOfStaying/actions/runs/45786760616) | `completed/success` |

هذه النتائج تثبت تشغيلات SHA أعلاه فقط. نجاح CodeQL لا يكشف قائمة التنبيهات ولا يثبت خلوها.

### قراءة إعدادات حماية الفرع في 2026-11-26

- `GET /branches/main/protection` أعاد `403 Resource not accessible by integration`. لذلك حالة branch protection **UNKNOWN / NOT VERIFIED** في هذه المراجعة؛ لا تُحوّل 403 إلى ادعاء بأن الفرع محمي أو غير محمي.
- `GET /repos/{owner}/{repo}/rulesets` أعاد `[]` في نطاق القراءة المتاح. هذه القراءة لا تحسم وحدها إعدادات الحماية التي تعذر الوصول إليها أو قواعد المؤسسة الموروثة.
- لا يُفعّل ruleset ولا تتغير سياسة الدفع قبل قرار المالك D3 وصلاحية قراءة/كتابة مناسبة.

## 1. لقطة التشغيل التاريخية — SHA `2e0304bf5151b77b618053f664efaf5ad6efdef5`

تحتوي الشجرة على **ثلاثة Workflows متتبعة**، إضافة إلى تشغيل CodeQL الديناميكي الذي لا يقابله ملف Workflow متتبع:

| Workflow | Run ID | الحالة | ما يثبته |
|---|---:|---|---|
| Domain and Python CI | [45704564814](https://github.com/ar-and-you/CostOfStaying/actions/runs/45704564814) | `completed/success` | اختبارات Domain على .NET 8 ومحاكي Python كما يحدده `ci.yml` |
| Core CI (.NET 10) | [45704564923](https://github.com/ar-and-you/CostOfStaying/actions/runs/45704564923) | `completed/success` | بناء واختبارات Core كما يحددها `core-ci.yml` |
| Documentation Governance | [45704564834](https://github.com/ar-and-you/CostOfStaying/actions/runs/45704564834) | `completed/success` | تدقيق روابط Markdown وقابلية الوصول من `docs/README.md` |
| CodeQL default setup (dynamic) | [45704564631](https://github.com/ar-and-you/CostOfStaying/actions/runs/45704564631) | `completed/success` | تشغيل CodeQL الديناميكي؛ ليس Workflow متتبعاً في المستودع |

هذه النتائج تخص SHA المذكور فقط، وهو **الرأس السابق لدفعة الإصلاح الحالية**. لا تُنسب إلى commit الإصلاح حتى تُقرأ تشغيلاته الفعلية. نجاح CodeQL لا يثبت خلو قائمة التنبيهات، كما أن تعذر قراءة التنبيهات بصلاحية API لا يعني عدم وجودها.


## 2. Workflows ونتائج CI

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

## 3. إعدادات الأمن والاعتماديات

- **Secret scanning وPush protection:** مفعّلان حسب قراءة إعدادات المستودع في 2026-11-24. فحص الأنماط غير التابعة لمزود وفحوص صلاحية الأسرار معطّلان. لا يوجد تقرير حديث يثبت مسحاً كاملاً لتاريخ Git بأداة متخصصة.
- **Dependabot alerts وقراءة التنبيهات:** كانت المراجعة السابقة بتاريخ 2026-11-22 تسجل أن تنبيهات Dependabot ورسم الاعتماديات مفعّلان. في إعادة التحقق بتاريخ 2026-11-24 أعادت `GET /dependabot/alerts?per_page=1` الخطأ `403 Resource not accessible by integration`؛ لذلك لا يمكن تأكيد عدد التنبيهات أو محتواها أو خلو المستودع منها. حالة تفعيل التنبيهات ورسم الاعتماديات لم تُحسم مجدداً بهذه القراءة.
- **Dependabot security updates:** معطّلة في قراءة 2026-11-24. لم يُضف `.github/dependabot.yml`، ولن تُفعّل التحديثات الأمنية الآلية دون توجيه جديد من مالك المشروع؛ تجنباً لفتح PRs آلية. لا تخلط بين تنبيهات Dependabot (التي لم تُقرأ) والتحديثات الآلية.
- **CodeQL:** أظهرت المراجعة المؤرخة 2026-11-22 إعداد GitHub default setup للغات C# وPython وGitHub Actions، باستخدام query suite `default` وthreat model `remote_and_local`. لا ينتج ذلك ملف workflow في المستودع. اكتمل التشغيل الأول [45004680726](https://github.com/ar-and-you/CostOfStaying/actions/runs/45004680726) بحالة `completed/success` على SHA الأساس `1b209b79261cba617bb68615746f115970422a8f`. لم تُعد قراءة إعداد CodeQL في فحص 2026-11-24.
- **قراءة Code Scanning:** أعادت `GET /code-scanning/alerts?per_page=1` في 2026-11-24 الخطأ `403 Resource not accessible by integration`. نجاح تشغيل CodeQL لا يكشف وحده قائمة التنبيهات؛ لا يمكن الجزم بعدم وجود نتائج.
- لا يوجد `security.yml` أو gitleaks أو اختبارات إعداد GitHub في `tools/github_config_tests/`. أضيف تشغيل `actionlint` و`zizmor` داخل `ci.yml` على فرع المراجعة الأمني فقط؛ يلزم انتظار CI على الرأس الناتج قبل تسجيلهما كـVERIFIED، ولا يعدّان جزءاً من `main` قبل الدمج.

## 4. حماية الفرع والحوكمة

- `main` هو الفرع الافتراضي. أعاد فحص الحماية بتاريخ 2026-11-24 `404 Branch not protected`، وكانت قائمة repository rulesets مع `includes_parents=true` فارغة (`[]`).
- تنص سياسة `ROADMAP.md` الحالية على الدفع المباشر إلى `main`. لم تُفعّل حماية قد تعارضها، ولم يُفرض شرط Pull Request أو required checks.
- لا يُستخدم force push أو إعادة كتابة للتاريخ.
- قبل إنشاء ruleset نشط، يلزم اختيار المالك بين: (أ) اعتماد PRs وفحوص required checks، بما يغيّر سياسة الدفع المباشر؛ أو (ب) الإبقاء على الدفع المباشر مع حظر force-push والحذف فقط. لا تُنشأ القاعدة قبل هذا الاختيار.
- إذا اختيرت سياسة PR لاحقاً، فأسماء الوظائف التي شُغّلت فعلياً هي `Domain (.NET 8) and simulator regressions (Python)` و`Build and test standalone Core`، ويجب التحقق من ظهورها على أحدث SHA قبل فرضها كفحوص مطلوبة.

## 5. التحقق المحلي وحدوده

نجحت اختبارات محاكي Python سابقاً `7/7` في Sandbox باستخدام Python 3.12.3. لم يُشغّل .NET محلياً في تلك المراجعة لعدم توفر SDK؛ نتائج Domain وCore أعلاه من GitHub Actions على SHA الأساس. لا يثبت ذلك تكامل Unity أو parity بين C# وPython أو سلامة تطبيق/خادم شبكي غير موجود. يجب تسجيل أي تشغيل لاحق مع SHA الدقيق والبيئة والأمر والنتيجة.

## 6. ما بقي

تظل مطلوبة مراجعة تنبيهات Dependabot بعد إتاحة صلاحية القراءة، ومراجعة التنبيهات الناتجة من Code Scanning بعد منح صلاحية القراءة، وإعادة قراءة حالة إعداد CodeQL، وحسم إعداد حماية `main` مع المالك. تحديثات Dependabot الأمنية الآلية وملف `dependabot.yml` مؤجلان حتى يعتمد المالك سلوك PRs التلقائية. تبقى أعمال gitleaks وفحوص workflow المتخصصة خارج الإعدادات المفعّلة حالياً.
