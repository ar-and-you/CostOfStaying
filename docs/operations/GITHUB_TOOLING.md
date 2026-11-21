# أدوات GitHub — الحالة الفعلية وخطة الاستكمال

**آخر مراجعة:** 2026-11-22

تعرض هذه الوثيقة ما أضيف وما لم يُستورد بعد. يجب التمييز بين وجود workflow وبين نجاح تشغيله، وبين نجاح الاختبارات المحددة وبين اكتمال الحوكمة والأمن.

## 1. Workflows

| الملف | الغرض | المشغلات | الحالة على SHA الأول |
|---|---|---|---|
| `.github/workflows/ci.yml` | اختبارات Domain على .NET 8 واختبارات محاكي Python | push إلى main، وPR إلى main، وتشغيل يدوي | نجح التشغيل الأول على `65e005b8122836d8a5de7560e0da7727270bf023` |
| `.github/workflows/core-ci.yml` | اختبارات Core المستقل باستخدام .NET 10 | push إلى main، وPR إلى main، وتشغيل يدوي | نجح التشغيل الأول على `65e005b8122836d8a5de7560e0da7727270bf023` |

يستخدم كل workflow صلاحية `contents: read` فقط. لا يشغّل `tools/playtest/run_sessions.py` ولا يستدعي `tools/audit.py`. مراجع `actions/checkout` و`actions/setup-dotnet` و`actions/setup-python` مثبتة على commit SHAs كاملة؛ راجع ملفات YAML لأرقام الإصدارات المثبتة.

### التشغيل الفعلي

- [Domain and Python CI — run 44960751847](https://github.com/ar-and-you/CostOfStaying/actions/runs/44960751847): نجحت وظيفة `Domain (.NET 8) and simulator regressions (Python)` (job `137018796082`). ظهرت عشرة تنبيهات compiler غير حاجبة عن تعليقات XML مفقودة لأعضاء عامة في `DomainEvent.cs` و`Debuff.cs` و`Clock.cs`.
- [Core CI (.NET 10) — run 44960751864](https://github.com/ar-and-you/CostOfStaying/actions/runs/44960751864): نجحت وظيفة `Build and test standalone Core` (job `137018796044`).

كلتا النتيجتين مرتبطتان بـSHA `65e005b8122836d8a5de7560e0da7727270bf023`. تثبتان نجاح الاختبارات التي يشغلها هذان الـworkflowان فقط؛ لا تثبتان parity بين اللغات أو تكامل Unity أو نجاح فحوص أمنية أخرى.

## 2. غير موجود في هذه الدفعة

العناصر التالية ليست جزءاً من ملفات التحديث ولا ينبغي وصفها بأنها مفعّلة:

- `security.yml` أو CodeQL أو gitleaks workflow.
- `repo-quality.yml` أو اختبارات إعداد GitHub.
- `unity-mobile.yml` أو بناء Unity/Android.
- Dependabot configuration.
- ملف CODEOWNERS فعّال أو قوالب Issues/PR.
- ruleset مفعّل.
- `tools/audit.py` و`tools/github_config_tests/` و`tools/repo_setup/`.

## 3. الأمان

ملفا CI لا يقدمان مسحاً أمنياً متخصصاً. لا تُحفظ مفاتيح API أو كلمات المرور أو ملفات `.env` أو شهادات التوقيع داخل Git. فحص grep محدود، عند إضافته لاحقاً، لا يُعد بديلاً عن gitleaks أو CodeQL.

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

في Sandbox المستخدم لهذه المراجعة، نجح اختبار محاكي Python `7/7` باستخدام Python 3.12.3. لم يُشغّل .NET محلياً لأن SDK غير مثبت؛ نتائج .NET الواردة أعلاه من GitHub Actions.

## 6. خطوات الاستكمال

1. تحقق من تشغيلات Actions على كل SHA لاحق يغيّر workflows أو مصادر الاختبار؛ لا تُعمّم نتيجة التشغيل الأول على commits لاحقة.
2. استورد أدوات تدقيق الوثائق واختبارات GitHub بعد مراجعة تبعياتها ومساراتها.
3. أضف gitleaks وCodeQL كدفعة أمان مستقلة بعد تثبيت الإصدارات ومراجعة الصلاحيات.
4. أضف Dependabot وCODEOWNERS بعد التأكد من المسارات والمالك.
5. أضف Unity workflow فقط بعد وجود مشروع Unity وإصدار محرر وتراخيص صالحة.
6. احسم مع مالك المستودع ما إذا كان يريد حماية `main`، لأن الفحص الحالي لم يُظهر قواعد حماية ولم تُغيّر الإعدادات هنا.
