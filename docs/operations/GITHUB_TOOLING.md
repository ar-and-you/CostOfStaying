# أدوات GitHub — الحالة الفعلية وخطة الاستكمال

**آخر مراجعة:** 2026-11-22  
**آخر SHA تحقق:** `b16fe9916b03b25db575fed5eac495182605725c`

تعرض هذه الوثيقة ما هو موجود فعلاً وما لم يُفعّل بعد. يجب التمييز بين وجود workflow وبين نجاح تشغيله، وبين نجاح الاختبارات المحددة وبين اكتمال الحوكمة والأمن.

## 1. Workflows

| الملف | الغرض | المشغلات | الحالة على آخر SHA |
|---|---|---|---|
| `.github/workflows/ci.yml` | اختبارات Domain على .NET 8 واختبارات محاكي Python | push إلى main، وPR إلى main، وتشغيل يدوي | نجح على `b16fe9916b03b25db575fed5eac495182605725c` |
| `.github/workflows/core-ci.yml` | اختبارات Core المستقل باستخدام .NET 10 | push إلى main، وPR إلى main، وتشغيل يدوي | نجح على `b16fe9916b03b25db575fed5eac495182605725c` |

يستخدم كل workflow صلاحية `contents: read` فقط. مراجع Actions المستخدمة مثبتة على commit SHAs كاملة؛ راجع ملفات YAML لأرقام الإصدارات المثبتة.

## 2. التشغيل الفعلي

- [Domain and Python CI — run 44961177878](https://github.com/ar-and-you/CostOfStaying/actions/runs/44961177878): نجحت وظيفة `Domain (.NET 8) and simulator regressions (Python)` (job `137020103578`).
- [Core CI (.NET 10) — run 44961177876](https://github.com/ar-and-you/CostOfStaying/actions/runs/44961177876): نجحت وظيفة `Build and test standalone Core` (job `137020103662`).

كلا التشغيلين من نوع `push`، وحالتهما `completed/success`، ويرتبطان بالـSHA `b16fe9916b03b25db575fed5eac495182605725c`. توجد تشغيلات ناجحة سابقة على SHA `65e005b8122836d8a5de7560e0da7727270bf023` أيضاً، لكنها لا تستبدل التحقق من SHA الأحدث.

تثبت هذه النتائج نجاح الاختبارات التي يشغلها هذان الـworkflowان فقط؛ لا تثبت parity بين اللغات أو تكامل Unity أو نجاح فحوص أمنية أخرى.

## 3. غير موجود أو غير مفعّل

- لا يوجد `security.yml` أو CodeQL أو gitleaks workflow.
- لا يوجد `repo-quality.yml` أو اختبارات إعداد GitHub في `tools/github_config_tests/`.
- لا يوجد `unity-mobile.yml` أو بناء Unity/Android.
- لا يظهر إعداد Dependabot أو ملف CODEOWNERS في الشجرة الحالية.
- لا توجد repository rulesets في فحص 2026-11-22، وأعاد endpoint حماية الفرع `404 Branch not protected`.
- لا توجد أداة `tools/audit.py` أو `tools/repo_setup/` في الشجرة التي فُحصت.

## 4. الأمان والحماية

ملفا CI الحاليان لا يقدمان مسحاً أمنياً متخصصاً. لا تحفظ مفاتيح API أو كلمات المرور أو ملفات `.env` أو شهادات التوقيع داخل Git. قواعد `.gitignore` لا تمسح التاريخ ولا تغني عن فحص الأسرار. لم يُتحقق من تفعيل GitHub Secret scanning أو Push protection.

أظهرت قراءة إعدادات الفرع المؤرخة 2026-11-22 أن `main` غير محمي في ذلك الفحص. لم تُفعّل حماية أو قواعد جديدة هنا. أي قرار لتفعيل الحماية يحتاج موافقة المالك ومراجعة أثره على سياسة التحديث المباشر الحالية.

## 5. التحقق المحلي

Domain من جذر المستودع:

```bash
dotnet test tests/CostOfStaying.Domain.Tests/CostOfStaying.Domain.Tests.csproj --configuration Release
python3 tools/balance_simulator/test_parity.py
```

Core من مجلد الاختبارات:

```bash
cd tests/CostOfStaying.Core.Tests
dotnet test CostOfStaying.Core.Tests.csproj --configuration Release
```

في Sandbox المستخدم لهذه المراجعة، نجحت اختبارات محاكي Python `7/7` باستخدام Python 3.12.3. لم يُشغّل .NET محلياً لعدم توفر SDK؛ نتائج .NET أعلاه من GitHub Actions.

## 6. خطوات الاستكمال

1. إضافة gitleaks وCodeQL كدفعة أمنية مستقلة بعد تثبيت الإصدارات ومراجعة الصلاحيات.
2. إضافة Dependabot بعد التحقق من مسارات المشاريع وملفات الاعتماديات.
3. إضافة اختبارات إعداد GitHub وactionlint/zizmor بعد مراجعة تبعياتها.
4. حسم قرار حماية `main` مع المالك؛ لا تفعّل قواعد قد تعطل سياسة التحديث المباشر دون موافقته.
5. إضافة Unity workflow فقط بعد وجود مشروع Unity وإصدار محرر وتراخيص صالحة.
