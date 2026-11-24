# نطاق نتائج التحقق — baseline 2026-11-23

**المستودع:** `ar-and-you/CostOfStaying`. **الفرع:** `main`. **SHA الأساس الذي تخصه التشغيلات:** `1b209b79261cba617bb68615746f115970422a8f`.

## تشغيلات CI وCodeQL المسجلة

| الفحص | Run ID | الحالة المسجلة | النطاق |
|---|---:|---|---|
| Domain and Python CI | [44966048222](https://github.com/ar-and-you/CostOfStaying/actions/runs/44966048222) | `completed / success` | وظيفة Domain على .NET 8 واختبارات محاكي Python المحددة في workflow |
| Core CI (.NET 10) | [44966048268](https://github.com/ar-and-you/CostOfStaying/actions/runs/44966048268) | `completed / success` | وظيفة بناء واختبار Core المحددة في workflow |
| GitHub CodeQL default setup | [45004680726](https://github.com/ar-and-you/CostOfStaying/actions/runs/45004680726) | `completed / success` وفق سجل الفحص | تحليل Actions وPython وC# في ذلك التشغيل؛ لا يوجد `codeql.yml` متتبع في الشجرة وقتها |

هذه التشغيلات تعود إلى SHA الأساس، ولا تمثل نتيجة تشغيل على commit الوثائق الحالي. نجاحها يثبت الوظائف المسماة فقط، ولا يثبت تكامل Unity أو الشبكة أو الأجهزة أو توازن اللعب أو خلو التنبيهات الأمنية.

## إعدادات GitHub — إعادة قراءة 2026-11-24

- `main` هو الفرع الافتراضي. أعاد endpoint حماية الفرع `404 Branch not protected`، وكانت قائمة repository rulesets فارغة.
- Secret scanning وPush protection مفعّلان. فحص الأنماط غير التابعة لمزود وفحوص صلاحية الأسرار معطّلان.
- Dependabot security updates معطّلة؛ لم يُضف `dependabot.yml` ولم تُنشأ إعدادات لتوليد PRs آلية.
- قراءتا قوائم Dependabot alerts وCode Scanning alerts أعادتا `403 Resource not accessible by integration`. لم يُعرف عدد التنبيهات أو محتواها. لا يصح استنتاج عدم وجود تنبيهات أو خلوها من هذا الخطأ.
- حالة تشغيل CodeQL أعلاه مسجلة على SHA الأساس؛ نجاحه لا يعني أن قائمة التنبيهات قُرئت أو أنه لم ينتج نتائج.

## حدود مستودع المصدر

في SHA الأساس، توجد workflowان متتبعان: `.github/workflows/ci.yml` و`.github/workflows/core-ci.yml`. الأول يشغل اختبارات Domain وPython regression المحددة، والثاني يختبر Core على .NET 10. لا يشغّل أي منهما `tools/playtest/run_sessions.py`، ولا يرفع جلسات playtest أو artifacts نشر، ولا يبني Unity أو Android/iOS أو خادماً.

`tools/balance_simulator/test_parity.py` يختبر محاكي Python فقط ولا يقارن بين تنفيذ C# وPython. والجلسات التي ينشئها `tools/playtest/run_sessions.py` اصطناعية وموسومة كمحاكاة؛ لا تدخل في عينة الجلسات البشرية المطلوبة.

## استعمال النتائج

تظل تقارير التحقق المحلية السابقة مرتبطة بالـSHA والبيئة والأوامر التي سجلتها. لا تُنقل أعدادها إلى CI الحالي. أي تحديث لاحق للحالة يجب أن يذكر SHA جديداً، وrun ID أو الأمر والبيئة، والنتيجة الدقيقة، وحدود ما تثبته.


## حالة أعمال التوثيق — 2026-11-24

عند إعداد هذه اللقطة كان `main` عند SHA `1b209b79261cba617bb68615746f115970422a8f`، ولم تكن التغييرات الوثائقية المعلّقة قد رُفعت أو سُجّلت، ولم توجد لها نتائج Actions بعد. يُحفظ هذا الوصف كسجل للحالة آنذاك فقط؛ انتقل `main` لاحقاً إلى SHA `589e3beb65980f0b11c1207c3a905b76e2f01028` وأصبحت له التشغيلات الموثقة في الملحق أدناه.

أُعيد فحص GitHub للقراءة فقط: `main` غير محمي وقائمة rulesets فارغة؛ Secret scanning وPush protection مفعّلان، وDependabot security updates معطّلة. تعذر قراءة قوائم Dependabot وCode Scanning بسبب `403 Resource not accessible by integration`، فلا نستنتج عدد التنبيهات أو محتواها أو خلو المستودع منها. لم تُفعّل إعدادات أو أتمتة جديدة في هذا الفحص.

## ملحق تشغيلات `main` — 2026-11-24

تحققت هذه التشغيلات على commit `589e3beb65980f0b11c1207c3a905b76e2f01028`؛ وقد أظهرت بيانات GitHub لكل منها الحالة `completed/success`:

| الفحص | Run ID | النطاق |
|---|---:|---|
| Domain and Python CI | [45283036745](https://github.com/ar-and-you/CostOfStaying/actions/runs/45283036745) | وظيفة Domain على .NET 8 واختبارات محاكي Python المعرفة في workflow |
| Core CI (.NET 10) | [45283036869](https://github.com/ar-and-you/CostOfStaying/actions/runs/45283036869) | البناء والاختبارات المحددة في workflow |
| CodeQL default setup | [45283036409](https://github.com/ar-and-you/CostOfStaying/actions/runs/45283036409) | تشغيل CodeQL ديناميكي؛ لا يوجد workflow CodeQL متتبع في المستودع |

لا تثبت هذه النتائج تكامل Unity أو الشبكة أو الأجهزة أو توازن اللعب، ولا تثبت خلو قوائم التنبيهات. بقيت قراءتا Dependabot وCode Scanning للتنبيهات رافعتين للخطأ 403 في فحص 2026-11-24.
