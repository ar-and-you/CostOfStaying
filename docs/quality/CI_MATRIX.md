# مصفوفة CI — ملفات Workflows الفعلية

> مصدر الحقيقة: ملفات `.github/workflows/` على `main`.
> مرجع اللقطة: `e7e5b5e54f46490c44aaf30888280d55239bb670` — التاريخ: 2026-11-28.
> يجب تحديث هذه المصفوفة عند تغيير أي Workflow.

## Workflows المتتبعة

### `ci.yml` — Domain and Python CI

الخطوات الفعلية وفق الملف:

1. إعداد .NET SDK من `global.json` الجذري.
2. إعداد Python 3.12.
3. `python3 -m compileall -q tools/balance_simulator tools/playtest` — فحص صياغة Python.
4. `python3 -m unittest discover -s tools/github_workflow_tests -v` — اختبارات Workflow Hygiene.
5. اختبارات Domain عبر NUnit باستخدام .NET 8.
6. `python3 tools/balance_simulator/test_parity.py` — اختبارات محاكي Python فقط؛ لا تشغّل C# ولا تقارن مخرجات اللغتين.
7. `python3 tools/playtest/test_cli.py` — اختبارات واجهة CLI لمحاكاة اللعب.
8. `python3 tools/playtest/test_smoke.py -v` — اختبار smoke محدود لمحاكاة اللعب.

### `core-ci.yml` — Core CI (.NET 10)

- إعداد .NET SDK من `tests/CostOfStaying.Core.Tests/global.json`.
- تشغيل NUnit من مجلد `tests/CostOfStaying.Core.Tests` باستخدام `dotnet test CostOfStaying.Core.Tests.csproj --configuration Release`.

### `docs-governance.yml` — Documentation Governance

- Checkout مع `fetch-depth: 0`.
- حارس نطاق PR خاص بفرع `docs/stabilization` عند تحقق شرطه.
- تشغيل `bash tools/docs_audit.sh` لفحص روابط Markdown الداخلية وقابلية الوصول إلى وثائق `docs/` من `docs/README.md`.

## عناصر منفصلة عن Workflows المتتبعة

- **CodeQL:** ظهرت تشغيلات ديناميكية في GitHub، لكن لا يوجد ملف Workflow CodeQL رابع متتبع في الشجرة المفحوصة. راجع [حالة الأمان](../security/SECURITY_STATUS.md).
- لا تُدرج `tools/audit.py` كخطوة CI؛ `ROADMAP.md` يذكر أنه غير موجود في شجرة `tools/` الحية وفق آخر مراجعة.
- لا تُدرج `run_sessions.py` كخطوة Workflow مستقلة؛ الموجود في CI هو اختبار smoke المحدود `test_smoke.py`.

## ما لا تثبته هذه المصفوفة

- لا تثبت تكافؤ Python وC#؛ راجع [خطة التكافؤ](PARITY_PLAN.md).
- لا تثبت فحص تاريخ Git أو خلوه من الأسرار.
- لا تثبت إجراء اختبار لعب بشري؛ المحاكاة الآلية ليست دليلاً بشرياً.
- وجود Workflow لا يثبت نجاحه. يجب ربط كل نتيجة تشغيل بـSHA كامل وRun ID.
