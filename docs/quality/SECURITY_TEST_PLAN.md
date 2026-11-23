# Security Test Plan — v2.2

**الحالة: PROPOSED | توجد اختبارات Domain ذات صلة بالأمن | CodeQL default setup كان مهيأ وأول تحليل نجح على SHA الأساس؛ تعذر قراءة قائمة التنبيهات عبر API (403، 2026-11-24)**

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
| Secret scanning | إعداد GitHub للمستودع | Secret scanning وPush protection مفعّلان؛ لا يوجد `security.yml` أو gitleaks workflow، ولم يُثبت مسح تاريخ كامل متخصص |
| Static analysis security | CodeQL default setup على GitHub | المراجعة المؤرخة 2026-11-22 سجّلت إعداد C# وPython وGitHub Actions باستخدام query suite `default`؛ اكتمل التشغيل الأول `45004680726` بنجاح على SHA الأساس `1b209b79261cba617bb68615746f115970422a8f`. لم يُعد فحص إعداد CodeQL في 2026-11-24، وقراءة التنبيهات أعادت 403؛ لا استنتاج عن عدم وجود تنبيهات |
| Dependabot alerts | إعداد GitHub للمستودع | المراجعة السابقة سجّلت تفعيل التنبيهات ورسم الاعتماديات بتاريخ 2026-11-22؛ أعادت قراءة التنبيهات في 2026-11-24 `403 Resource not accessible by integration`، لذلك لم يوثق عددها أو حالتها ولم يُحسم إعداد التنبيهات مجدداً |
| Dependabot security updates | إعداد GitHub للمستودع | معطّل حسب فحص 2026-11-24 لتجنب PRs تحديث آلية؛ لم يُضف `dependabot.yml`، ولا يُفعّل دون توجيه المالك |
| Workflow supply-chain lint | غير مهيأ | لا توجد اختبارات `tools/github_config_tests/` أو تشغيل موثق لـactionlint/zizmor |

نجاح CI الحالي يثبت نجاح الاختبارات التي يشغّلها الخطان فقط، ولا يثبت اجتياز مراجعة أمنية شاملة أو سلامة خادم غير موجود.

## معيار النجاح الأمني

الطلب غير الصالح يُرفض دون تغيير الحالة أو crash. كل ثغرة مؤكدة تصبح regression test قبل إغلاقها. وتحتاج ضوابط الخادم والشبكة إلى تنفيذ واختبار تكاملي عند إنشاء تلك المكونات.

## الأدلة

- اختبارات Domain: `SecurityTests.cs` و`TransferTests.cs`.
- نتائج CI الفعلية وروابطها وSHA المختبر: `docs/operations/GITHUB_TOOLING.md`.
- حدود التهديدات غير المنفذة: `docs/security/THREAT_MODEL.md`.

لا تُرفع حالة أي عنصر إلى `VERIFIED` إلا بدليل مباشر يطابق نطاقه.
