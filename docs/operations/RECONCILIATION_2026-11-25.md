# مصالحة التوثيق والحوكمة — 2026-11-25

**نطاق التغيير:** توثيق وحوكمة فقط؛ لا تغيير في `src/` أو `tests/` أو قواعد اللعب أو ADRs أو إعدادات البناء.

## الحالة عند بدء المراجعة

- الرأس المحلي والبعيد قبل هذا التصحيح: `84df71ef2d5d5c9cef4979e4fe2df6460aa96bb`.
- `main` هو الفرع المرجعي، وسياسة المستودع الحالية تسمح بالدفع المباشر ولا تسمح بـ force-push.
- يحتوي المستودع على `.github/workflows/ci.yml` و`.github/workflows/core-ci.yml`، ولا يحتوي workflow لحوكمة الوثائق أو أداة `tools/audit.py` أو اختبارات إعداد GitHub.
- نتائج CI واختبارات .NET المسجلة في الوثائق تاريخية ومقيدة بـ SHA التشغيل؛ لا تُعد دليلاً على الرأس الحالي.

## التغييرات الانتقائية

1. إضافة `tools/docs_audit.sh` لفحص روابط Markdown وقابلية الوصول من `docs/README.md`.
2. إضافة `.github/workflows/docs-governance.yml` لتشغيل التدقيق على Pull Requests، ورفض تغييرات المصدر في فرع `docs/stabilization` فقط. لا يفعّل الملف حماية الفرع أو required checks تلقائياً.
3. توضيح `docs/quality/VERIFICATION_2026-11-20.md` بأن `tools/audit.py` واختبار إعداد GitHub المذكورين تاريخيان وغير موجودين في `main` الحالي، وبالتالي لا يمكن إعادة تشغيل نتائجهما من الرأس الحالي.
4. إضافة هذا السجل إلى فهرس الوثائق مع الإبقاء على تفاصيل الخصوصية وقيود الأمن الحالية.

## أدلة التحقق المحلية

- `bash -n tools/docs_audit.sh`: يجب أن ينجح.
- `bash tools/docs_audit.sh`: يجب أن يثبت عدم وجود روابط نسبية مكسورة أو وثائق Markdown يتيمة.
- `python3 tools/balance_simulator/test_parity.py`: نجاح 7/7؛ هذا اختبار لمحاكي Python فقط ولا يثبت parity مع C#.
- `python3 -m compileall -q tools`: نجاح.
- `dotnet` غير مثبت في بيئة Sandbox الحالية؛ لم تُدّعَ نتائج بناء أو اختبار .NET محلية.
- لا يُعلن نجاح GitHub Actions لهذا التغيير قبل تشغيل workflow فعلي على commit الجديد.

## حدود واعتمادات خارجية

- لا يضيف هذا التصحيح `security.yml` أو gitleaks أو Dependabot أو CODEOWNERS، ولا يثبت حالة Secret Scanning أو CodeQL أو حماية `main` الخارجية.
- لم تُنسخ أي حزمة ZIP أو مواد أرشيفية أو ADR تاريخية.
- الدفع إلى GitHub يتطلب مصادقة connector صالحة؛ لا تُعتبر المحاولة ناجحة ما لم يُثبت SHA البعيد بعد الدفع.
