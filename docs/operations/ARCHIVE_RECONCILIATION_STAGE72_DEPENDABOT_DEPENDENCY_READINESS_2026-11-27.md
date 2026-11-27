# المرحلة 72 — جرد الاعتماديات وجاهزية Dependabot (2026-11-27)

## الرأس والنطاق

- HEAD قبل المرحلة: `f5d57cd980af97f4d6465a2204078b01aa03ffdc`.
- المستودع: `ar-and-you/CostOfStaying`، الفرع الافتراضي `main`.
- فُحصت شجرة المستودع وملفات NuGet وSDK وملفات GitHub Actions ووثيقة إدارة الأسرار.
- لم تُغيّر إعدادات GitHub؛ صلاحيات الاتصال المتاحة لا تثبت صلاحية إدارة إعدادات الأمن أو قراءة التنبيهات.

## 1. جرد الاعتماديات الحية

| النظام | الملفات/النطاق | الملاحظة |
|---|---|---|
| NuGet — Domain tests | `Directory.Packages.props` و`tests/CostOfStaying.Domain.Tests/CostOfStaying.Domain.Tests.csproj` | إدارة مركزية؛ Microsoft.NET.Test.Sdk 17.11.1، NUnit 4.2.2، NUnit3TestAdapter 4.6.0، coverlet.collector 6.0.2 |
| NuGet — Core tests | `tests/CostOfStaying.Core.Tests/CostOfStaying.Core.Tests.csproj` | إصدارات صريحة: Microsoft.NET.Test.Sdk 17.12.0، NUnit 3.14.0، NUnit3TestAdapter 4.6.0 |
| Core test package policy | `tests/CostOfStaying.Core.Tests/Directory.Packages.props` | يعطل Central Package Management محلياً لهذا المسار؛ لا تدمج إدارته مع جذر المستودع دون مراجعة |
| .NET SDK | `global.json` و`tests/CostOfStaying.Core.Tests/global.json` | SDK 8.0.400 لمسار Domain، و10.0.100 لمسار Core tests؛ كلاهما latestFeature مع منع prerelease |
| GitHub Actions | ثلاثة workflows في `.github/workflows/` | مراجع actions مثبتة على commit SHAs كاملة في الملفات المفحوصة |
| Python | أدوات المحاكاة واللعب واختبارات workflow | لم يظهر `requirements.txt` أو `pyproject.toml` أو lockfile Python في الشجرة؛ لا تُخترع حزمة Python لمجرد إضافة Dependabot |
| تطبيق/مكتبات تشغيل | مشروعا Domain وCore | يستهدفان netstandard2.1 ولا يحتويان PackageReference خارجي في ملفات csproj المفحوصة |

## 2. الحالة الفعلية لإعداد Dependabot

- لا يوجد `.github/dependabot.yml` في الشجرة عند HEAD قبل المرحلة.
- لم تُثبت قراءة حية موثوقة لحالة Dependabot alerts أو dependency graph أو security updates في هذه المرحلة.
- سجل التشغيل السابق في `docs/operations/GITHUB_TOOLING.md` يذكر أن Dependabot security updates كانت معطلة في قراءة 2026-11-24، وأن قراءة التنبيهات أعادت 403. هذه معلومة مؤرخة وليست قراءة جديدة بتاريخ المرحلة 72.
- بيانات مستودع GitHub المتاحة للاتصال تُظهر `permissions.admin=false` و`permissions.push=true`. هذا لا يمنح صلاحية إدارة إعدادات الأمن، ولا يتيح تأكيد أو تعديل إعدادات Dependabot.
- لا توجد أدلة تتيح تحديد عدد التنبيهات المفتوحة أو شدتها؛ لا تسجل التنبيهات على أنها صفر أو خالية.

## 3. قرار التنفيذ الآمن

لم يُنشأ `.github/dependabot.yml` ولم تُفعّل تحديثات أمنية أو PRs آلية في هذه المرحلة، للأسباب التالية:

1. سياسة `ROADMAP.md` الحية تنص على الدفع المباشر إلى `main` وتحظر إنشاء task branches وPRs ضمن التوجيه الحالي.
2. Dependabot version updates وsecurity updates ينشئان PRs؛ تفعيلهما دون مواءمة سياسة Git سيكون تعارضاً مع السياسة الحية.
3. صلاحية الاتصال الحالية لا تشمل `admin`، لذلك لا يمكن تعديل إعدادات المستودع أو التحقق من حالتها.
4. لا يجوز إخفاء هذا التعارض عبر ملف إعداد شكلي أو الادعاء بأن التنبيهات تمت مراجعتها.

## 4. متطلبات الإغلاق

يلزم قرار مالك مكتوب يختار أحد المسارين:

- **المسار A — اعتماد PRs الآلية:** تعديل سياسة Git صراحةً للسماح بـDependabot PRs، وتحديد من يراجعها، وجدول التحديث، وحدود الإصدارات، وسياسة التجميع، والتراجع.
- **المسار B — الإبقاء على الدفع المباشر:** عدم تفعيل Dependabot PRs حالياً؛ استخدام تنبيهات الثغرات وقراءة التنبيهات فقط إذا أمكن تفعيلها دون تغيير سياسة الدمج، مع توثيق من يتولى تحديث الإصدارات يدوياً.

بعد القرار، ومع صلاحية إدارية مناسبة:

1. قراءة حالة dependency graph وDependabot alerts وsecurity updates مباشرة.
2. إعداد `.github/dependabot.yml` للمسارات NuGet والجذر GitHub Actions فقط؛ لا إضافة Python ecosystem بلا manifest.
3. تحديد cadence وopen PR limits وسياسة grouping بما يتوافق مع القرار.
4. تفعيل/التحقق من security updates وإعادة قراءة alerts.
5. توثيق التنبيهات الفعلية مع advisory/version/fixed version وقرار المعالجة.
6. اختبار أول PR أو التحقق من عدم إنشاء PRs غير مرغوبة، وربط النتائج بـSHA.

## 5. النتيجة

- **الجرد:** COMPLETE للنطاقات والملفات الظاهرة في الشجرة.
- **إعداد Dependabot:** NOT CONFIGURED في الشجرة.
- **تنبيهات Dependabot:** UNKNOWN / NOT VERIFIED.
- **صلاحية إدارة الإعدادات:** غير متاحة عبر الاتصال الحالي (`admin=false`).
- **تفعيل PRs الآلية:** BLOCKED_PENDING_OWNER_GIT_POLICY_DECISION.
- لم تُغيّر الاعتماديات أو ملفات البناء أو إعدادات GitHub، ولم تُشغّل اختبارات برمجية في هذه المرحلة؛ هذه مراجعة جرد وسياسة.

## المراجع

- `Directory.Packages.props`
- `tests/CostOfStaying.Core.Tests/Directory.Packages.props`
- `global.json` و`tests/CostOfStaying.Core.Tests/global.json`
- `.github/workflows/ci.yml` و`core-ci.yml` و`docs-governance.yml`
- `docs/security/SECRETS_MANAGEMENT.md`
- `docs/operations/GITHUB_TOOLING.md`
- `ROADMAP.md`
- `docs/governance/OPEN_DECISION_REGISTER.md`

**الحالة:** STAGE 72 COMPLETE — DEPENDENCY INVENTORY RECORDED; DEPENDABOT CONFIGURATION AND ALERT REVIEW BLOCKED BY PERMISSIONS AND OWNER GIT-POLICY DECISION.
