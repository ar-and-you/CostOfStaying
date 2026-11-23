# استراتيجية الاختبارات

> تنبيه حوكمي: أرقام ADR المشار إليها في نسخ سابقة هي مواد أرشيفية ما لم توجد في `docs/adr/` بحالة مقبولة على `main`.

**الحالة:** استراتيجية نافذة للاختبارات الحالية وخطة توسع مقترحة. لا تعني أن كل مستويات الاختبار منفذة.

## خطوط الاختبار الحالية

| الخط | المشروع | SDK | Workflow |
|---|---|---|---|
| Domain | `tests/CostOfStaying.Domain.Tests/` | .NET 8 من `global.json` الجذري | `.github/workflows/ci.yml` |
| Core | `tests/CostOfStaying.Core.Tests/` | .NET 10 من `tests/CostOfStaying.Core.Tests/global.json` | `.github/workflows/core-ci.yml` |
| Python simulator | `tools/balance_simulator/test_parity.py` | Python 3.12 في CI | `ci.yml` |

## نطاق التغطية

Domain tests تشمل Vitality/Collapse، Transfer، Spirit، Vote/Endgame، وأمن/مدخلات وفق الملفات الموجودة في مشروع الاختبار. Core tests تشمل معرفات، ledger، command envelope/sequence/admission/authorization/payload validation، session lifecycle/registry/cleanup، وcomposition.

## حدود التحقق

- `test_parity.py` لا يشغل C#؛ إنه اختبار رجعي لمحاكي Python.
- محاكاة `run_sessions.py` ليست human playtest ولا تدخل بوابة CI.
- لا Unity PlayMode/EditMode tests ولا network integration tests ولا Android tests.
- اختبارات Domain التي تمرر distance إلى المجمع لا تثبت أن خادماً شبكياً يقيس المسافة بنفسه.
- نجاح CI لا يثبت الأمن أو التوازن أو أداء منصة غير موجودة.

## شروط تسجيل VERIFIED

يجب تسجيل SHA، workflow/run URL أو الأمر، البيئة وإصدارات SDK، عدد الاختبارات الناجحة والفاشلة والمتجاوزة، والنطاق الذي يغطيه الاختبار. لا تستخدم VERIFIED خارج هذا النطاق.

## أوامر التنفيذ

انظر الأوامر الدقيقة في `README.md` و`CONTRIBUTING.md`. نفّذ Core من مجلد الاختبارات حتى يقرأ SDK 10.
