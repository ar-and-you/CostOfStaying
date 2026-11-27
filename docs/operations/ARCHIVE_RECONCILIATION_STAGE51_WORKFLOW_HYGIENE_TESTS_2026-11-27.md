# المرحلة 51 — إضافة فحوص ثابتة لسلامة GitHub Actions (2026-11-27)

## الرأس والنطاق
- HEAD الذي أضيفت عليه فحوص workflow: `19ea7ecbc16720ebccc761cfb9c79ab394eb224c`.
- مصدر الفكرة: فرص salvage في المرحلة 46، وفجوات CI-002/CI-003 في المرحلة 35، ومراجعة إعدادات GitHub في المرحلة 47.
- الهدف: تحويل مجموعة صغيرة من الضوابط الموجودة بالفعل في Workflows الحية إلى اختبارات regression قابلة للتشغيل دون إضافة dependency خارجية.

## التنفيذ

أُنشئت:
- `tools/github_workflow_tests/test_workflow_hygiene.py`
- `tools/github_workflow_tests/README.md`

وعُدّل:
- `.github/workflows/ci.yml` لإضافة خطوة `python3 -m unittest discover -s tools/github_workflow_tests -v` بعد إعداد Python.
- `docs/operations/TOOLING_MAP.md` لتسجيل نطاق الفحص وحدوده.

الاختبارات الستة تتحقق من:
1. وجود Workflows الثلاثة المتوقعة فقط: ci.yml وcore-ci.yml وdocs-governance.yml.
2. صلاحيات top-level صريحة ومحصورة في `contents: read`.
3. تثبيت كل Action خارجي على SHA كامل من 40 خانة.
4. وجود `timeout-minutes` موجب لكل Job.
5. `persist-credentials: false` لكل checkout.
6. عدم إدخال `pull_request_target` أو `workflow_run` دون مراجعة أمنية منفصلة.

كل التنفيذ يعتمد Python standard library؛ لم تُضف PyYAML أو npm أو أي تبعية جديدة.

## نتائج CI على SHA `19ea7ecbc16720ebccc761cfb9c79ab394eb224c`

| Workflow | Run ID | النتيجة |
|---|---:|---|
| Domain and Python CI | 45916838661 | `completed / success` |
| Core CI (.NET 10) | 45916838645 | `completed / success` |
| Documentation Governance | 45916838631 | `completed / success` |

تفاصيل Domain/Python تؤكد نجاح:
- Python syntax compileall.
- GitHub workflow hygiene tests.
- Domain NUnit.
- Python simulator regression.
- simulated-playtest CLI regression.

Core NUnit اكتمل بنجاح، وDocumentation Governance نجح. هذه نتائج على SHA `19ea7ecbc16720ebccc761cfb9c79ab394eb224c` تحديداً.

## الحدود

- هذه فحوص static محدودة مبنية على شكل ملفات Workflows الحالية؛ لا تحل محل actionlint أو zizmor.
- لا تقرأ إعدادات GitHub الفعلية أو Branch protection/rulesets أو Dependabot/Code Scanning alerts.
- لا تنفذ secret scanning ولا تثبت أمان Actions أو سلامة الكود runtime.
- لا تضيف CODEOWNERS أو Dependabot أو zizmor config أو rulesets من الأرشيف.
- لا تغيّر صلاحيات Workflow أو طريقة الدفع إلى main.

## القرار

- `KEEP_LIVE` للاختبار الجديد لأنه يحرس ضوابط CI الموجودة فعلياً ويستخدم standard library فقط.
- لا نقل مباشر لأدوات github_config_tests الأرشيفية؛ الاختبار الجديد يعيد تصميم نطاق صغير فقط ليتوافق مع الملفات الحية.
- لا حذف أو نقل أو استيراد لملفات المنتج أو ZIP الأصلية.

**الحالة:** `STAGE 51 COMPLETE — WORKFLOW HYGIENE REGRESSION TESTS ADDED; DOMAIN/CORE/DOCS CI VERIFIED ON THE IMPLEMENTATION SHA.`
