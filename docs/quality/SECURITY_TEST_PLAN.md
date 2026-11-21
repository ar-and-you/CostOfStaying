# Security Test Plan — v2.1
**الحالة: PROPOSED | اختبارات Domain منفذة (ADR-005) | لا VERIFIED قبل CI أخضر**

## النطاق
اختبر عند وجود خادم: تزوير health وdamage وammo؛ RPC spam؛ replay للنقل والهجوم والتصويت؛ نقل متزامن؛ هدف ميت أو من مباراة أخرى؛ payload كبير؛ speed وteleport؛ وجلسة مزورة.

لا تختبر Energy كقاعدة حتى تُعتمد. لا تختبر عتبات Betrayal كقواعد.

## التنفيذ الحالي

| الاختبار | الملف | يغطي |
|---|---|---|
| Server Authority | `tests/CostOfStaying.Domain.Tests/SecurityTests.cs` | رفض مسافة العميل المزعومة، منع replay بـ RequestId، Betrayal فقط عند النجاح |
| Transfer Atomicity | `TransferTests.cs` | لا تغيير عند الفشل، idempotency، حدود المدى/التبريد/السعة |
| CI Heuristic | `ci.yml` → grep | أنماط أسرار شائعة (يفشل الـPR) |
| Secret scan | `security.yml` → gitleaks (تاريخ + شجرة) | لا أسرار في Git؛ PROPOSED حتى يُشغَّل على GitHub |
| تحليل ساكن | `security.yml` → CodeQL (C# وPython وActions) | ثغرات الكود وworkflows؛ لا يثبت سلطة الخادم |
| سلسلة التوريد في CI | `tools/github_config_tests/` + zizmor + actionlint | تثبيت actions بـSHA، صلاحيات دنيا، منع الحقن وتسريب الأسرار في workflows |

النجاح: الطلب غير الصالح يُرفض دون تغيير الحالة أو crash. كل ثغرة مكتشفة تصبح regression test قبل إغلاقها.

## المراجع الموحدة
- المصادر: `governance/SOURCES_AND_REFERENCES.md` EXT-GITLEAKS / EXT-CODEQL / SRC-ADR-002 / SRC-ADR-026
- الأدلة: `REQUIREMENTS_TRACEABILITY.md` NET-001/TRF-001
