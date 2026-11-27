# Security Test Plan — v2.2

**الحالة: PROPOSED | توجد اختبارات Domain/Core ذات صلة بالأمن | أحدث Domain/Core/Docs CI موثق على SHA 94b341a؛ Domain 62/62 وCore 248/248 وPython simulator 11/11 وworkflow hygiene 6/6؛ CodeQL C#/Python/Actions success؛ قراءة التنبيهات 403**

## النطاق

عند وجود خادم وشبكة، اختبر تزوير health وdamage وammo؛ RPC spam؛ replay للنقل والهجوم والتصويت؛ النقل المتزامن؛ هدفاً ميتاً أو من مباراة أخرى؛ payload كبيراً؛ speed وteleport؛ وجلسة مزورة.

لا تختبر Energy كقاعدة حتى تُعتمد. لا تختبر عتبات Betrayal كقواعد قبل اعتمادها.

## التنفيذ الحالي

| المجال | الملف أو الخط | التغطية والحالة |
|---|---|---|
| Domain security invariants | `tests/CostOfStaying.Domain.Tests/SecurityTests.cs` | اختبارات لحدود تعديل حالة Player، التحقق من مسافة النقل التي يمررها Domain، رفض RequestId المكرر، احتساب Betrayal عند نجاح النقل فقط، إعدادات/قيم Tick غير المنتهية، ورفض أوامر اللعب بعد اكتمال المباراة |
| Transfer atomicity | `tests/CostOfStaying.Domain.Tests/TransferTests.cs` | اختبارات النقل الذري، عدم إحداث آثار جانبية عند الفشل، idempotency، المدى، cooldown، والسعة |
| Domain/Python CI | `.github/workflows/ci.yml` | 62/62 Domain tests على SHA 94b341a؛ Python simulator 11/11 وworkflow-hygiene 6/6؛ لا يشغّل ماسح أسرار |
| Core CI | `.github/workflows/core-ci.yml` | 248/248 Core NUnit tests على SHA 94b341a؛ لا يشغّل ماسحاً أمنياً |
| Secret scanning | إعداد GitHub للمستودع | آخر حالة معروفة active في قراءة 2026-11-24؛ لم تتوفر قراءة أحدث. لا يوجد `security.yml` أو gitleaks workflow، ولم يُثبت مسح تاريخ كامل متخصص |
| Static analysis security | CodeQL dynamic default setup على GitHub | run `45919285741` على SHA 94b341a: C# وPython وActions completed/success. CodeQL ليس workflow متتبعاً في main؛ قراءة alerts أعادت 403، ولا استنتاج عن خلو التنبيهات |
| Dependabot alerts | إعداد GitHub للمستودع | المراجعة السابقة سجّلت تفعيل التنبيهات ورسم الاعتماديات بتاريخ 2026-11-22؛ أعادت قراءة التنبيهات في 2026-11-24 `403 Resource not accessible by integration`، لذلك لم يوثق عددها أو حالتها ولم يُحسم إعداد التنبيهات مجدداً |
| Dependabot security updates | إعداد GitHub للمستودع | معطّل حسب فحص 2026-11-24 لتجنب PRs تحديث آلية؛ لم يُضف `dependabot.yml`، ولا يُفعّل دون توجيه المالك |
| Workflow hygiene | `tools/github_workflow_tests/` ضمن Domain CI | فحوص ساكنة لصلاحيات/SHAs/مهل/checkout؛ لا توجد actionlint أو zizmor أو gitleaks workflows |

نجاح CI الحالي يثبت نجاح الاختبارات التي يشغّلها الخطان فقط، ولا يثبت اجتياز مراجعة أمنية شاملة أو سلامة خادم غير موجود.

| Core authorization/session/payload gates | Core NUnit | اختبارات admission, roster, authorization, payload validation, queue replay, connection binding، بما فيها fail-closed exceptions بعد dequeue؛ لا credential provider أو transport integration |

## معيار النجاح الأمني

الطلب غير الصالح يُرفض دون تغيير الحالة أو crash. كل ثغرة مؤكدة تصبح regression test قبل إغلاقها. وتحتاج ضوابط الخادم والشبكة إلى تنفيذ واختبار تكاملي عند إنشاء تلك المكونات.

## الأدلة

- اختبارات Domain: `SecurityTests.cs` و`TransferTests.cs`.
- نتائج CI الفعلية وروابطها وSHA المختبر: `docs/operations/GITHUB_TOOLING.md`.
- حدود التهديدات غير المنفذة: `docs/security/THREAT_MODEL.md`.

لا تُرفع حالة أي عنصر إلى `VERIFIED` إلا بدليل مباشر يطابق نطاقه.
