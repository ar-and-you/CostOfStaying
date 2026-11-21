# أدوات GitHub — الحالة الفعلية وخطة الاستكمال

**آخر مراجعة:** 2026-11-21

تعرض هذه الوثيقة ما يضاف في هذه الدفعة وما لم يُستورد بعد. يجب التمييز بين وجود workflow وبين نجاح تشغيله.

## 1. Workflows

| الملف | الغرض | المشغلات | الحالة |
|---|---|---|---|
| `.github/workflows/ci.yml` | اختبارات Domain على .NET 8 واختبارات محاكي Python | push إلى main، وPR إلى main، وتشغيل يدوي | إعداد مضاف؛ لا يُعد التحقق مكتملاً إلا بنتيجة مرتبطة بـSHA |
| `.github/workflows/core-ci.yml` | اختبارات Core المستقل باستخدام .NET 10 | push إلى main، وPR إلى main، وتشغيل يدوي | إعداد مضاف؛ لا يُعد التحقق مكتملاً إلا بنتيجة مرتبطة بـSHA |

يستخدم كل workflow صلاحية `contents: read` فقط. لا يشغّل `tools/playtest/run_sessions.py` ولا يستدعي `tools/audit.py`. لا يُعد workflow ناجحاً إلا بعد ظهور تشغيل فعلي ناجح على GitHub Actions وربطه بـcommit SHA المقصود؛ راجع ملاحق الحالة أدناه قبل اعتماد أي نتيجة.
مراجع `actions/checkout` و`actions/setup-dotnet` و`actions/setup-python` مثبتة على commit SHAs كاملة بدلاً من وسوم متحركة؛ راجع ملفات YAML نفسها لأرقام الإصدارات المثبتة.

## 2. غير موجود في هذه الدفعة

العناصر التالية ليست جزءاً من ملفات التحديث المقترحة ولا ينبغي وصفها بأنها مفعّلة:

- `security.yml` أو CodeQL أو gitleaks workflow.
- `repo-quality.yml` أو اختبارات إعداد GitHub.
- `unity-mobile.yml` أو بناء Unity/Android.
- Dependabot configuration.
- ملف CODEOWNERS فعّال أو قوالب Issues/PR.
- ruleset مفعّل.
- `tools/audit.py` و`tools/github_config_tests/` و`tools/repo_setup/`.

## 3. الأمان

ملفا CI هنا لا يقدمان مسحاً أمنياً متخصصاً. لا تُحفظ مفاتيح API أو كلمات المرور أو ملفات `.env` أو شهادات التوقيع داخل Git. فحص grep محدود، عند إضافته لاحقاً، لا يُعد بديلاً عن gitleaks أو CodeQL.

## 4. الحماية والصلاحيات

في فحص GitHub API بتاريخ 2026-11-22، أعاد endpoint الخاص بحماية الفرع `404 Branch not protected`، وأعادت واجهة rulesets قائمة فارغة. بناءً على ذلك، لم تظهر حماية فرع أو قواعد repository rulesets في ذلك الفحص؛ هذه ملاحظة مؤرخة وليست ضماناً دائماً بأن الإعدادات لم تتغير.

لم تُفعّل قواعد حماية في هذه المهمة. قبل أي تغيير مستقبلي، تحقّق من أسماء jobs الفعلية وصلاحيات المالك وتأثير القواعد على السياسة الحالية للتحديث المباشر إلى `main`.

## 5. التحقق المحلي

Domain وPython من جذر المستودع:

```bash
dotnet restore CostOfStaying.sln
dotnet build CostOfStaying.sln --configuration Release
dotnet test CostOfStaying.sln --configuration Release
python3 tools/balance_simulator/test_parity.py
```

Core من مجلد الاختبارات:

```bash
cd tests/CostOfStaying.Core.Tests
dotnet restore CostOfStaying.Core.Tests.csproj
dotnet test CostOfStaying.Core.Tests.csproj --configuration Release
```

## 6. خطوات الاستكمال

1. تحقق من أول تشغيل للـworkflows أعلاه.
2. استورد أدوات تدقيق الوثائق واختبارات GitHub بعد مراجعة تبعياتها ومساراتها.
3. أضف gitleaks وCodeQL كدفعة أمان مستقلة بعد تثبيت الإصدارات ومراجعة الصلاحيات.
4. أضف Dependabot وCODEOWNERS بعد التأكد من المسارات والمالك.
5. أضف Unity workflow فقط بعد وجود مشروع Unity وإصدار محرر وتراخيص صالحة.
