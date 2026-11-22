# Security Test Plan — v2.2

**الحالة: PROPOSED | توجد اختبارات Domain ذات صلة بالأمن | لا يوجد فحص أمني متخصص مفعّل في CI**

## النطاق

عند وجود خادم وشبكة، اختبر تزوير health وdamage وammo؛ RPC spam؛ replay للنقل والهجوم والتصويت؛ النقل المتزامن؛ هدفاً ميتاً أو من مباراة أخرى؛ payload كبيراً؛ speed وteleport؛ وجلسة مزورة.

لا تختبر Energy كقاعدة حتى تُعتمد. لا تختبر عتبات Betrayal كقواعد قبل اعتمادها.

## التنفيذ الحالي

| المجال | الملف أو الخط | التغطية والحالة |
|---|---|---|
| Domain security invariants | `tests/CostOfStaying.Domain.Tests/SecurityTests.cs` | اختبارات لحدود تعديل حالة Player، التحقق من مسافة النقل التي يمررها Domain، رفض RequestId المكرر، احتساب Betrayal عند نجاح النقل فقط، إعدادات/قيم Tick غير المنتهية، ورفض أوامر اللعب بعد اكتمال المباراة |
| Transfer atomicity | `tests/CostOfStaying.Domain.Tests/TransferTests.cs` | اختبارات النقل الذري، عدم إحداث آثار جانبية عند الفشل، idempotency، المدى، cooldown، والسعة |
| Domain/Python CI | `.github/workflows/ci.yml` | يشغّل اختبارات Domain NUnit واختبارات محاكي Python؛ لا يشغّل ماسح أسرار أو CodeQL |
| Core CI | `.github/workflows/core-ci.yml` | يشغّل اختبارات Core NUnit؛ لا يشغّل ماسحاً أمنياً |
| Secret scanning | لا يوجد workflow مفعّل | لا يوجد `security.yml` أو دليل تشغيل gitleaks في Actions |
| Static analysis security | لا يوجد workflow مفعّل | CodeQL غير مهيأ ضمن شجرة المستودع الحالية |
| Workflow supply-chain lint | غير مهيأ | لا توجد اختبارات `tools/github_config_tests/` أو تشغيل موثق لـactionlint/zizmor |

نجاح CI الحالي يثبت نجاح الاختبارات التي يشغّلها الخطان فقط، ولا يثبت اجتياز مراجعة أمنية شاملة أو سلامة خادم غير موجود.

## معيار النجاح الأمني

الطلب غير الصالح يُرفض دون تغيير الحالة أو crash. كل ثغرة مؤكدة تصبح regression test قبل إغلاقها. وتحتاج ضوابط الخادم والشبكة إلى تنفيذ واختبار تكاملي عند إنشاء تلك المكونات.

## الأدلة

- اختبارات Domain: `SecurityTests.cs` و`TransferTests.cs`.
- نتائج CI الفعلية وروابطها وSHA المختبر: `docs/operations/GITHUB_TOOLING.md`.
- حدود التهديدات غير المنفذة: `docs/security/THREAT_MODEL.md`.

لا تُرفع حالة أي عنصر إلى `VERIFIED` إلا بدليل مباشر يطابق نطاقه.
