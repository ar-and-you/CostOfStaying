# المساهمة في CostOfStaying

## الحالة ومصادر الحقيقة

المشروع في مرحلة ما قبل الإنتاج. قبل أي تعديل، اقرأ `AGENTS.md` و`README.md` و`ROADMAP.md`، ثم راجع سجل السياسات ومواصفة النظام والقرار المعماري ذي الصلة إن كان مقبولاً على `main`. عند التعارض، اتبع قرار المالك المكتوب ومصادر الحقيقة في `docs/governance/PROJECT_GOVERNANCE.md`؛ رقم ADR بلا سجل مقبول لا يحمل سلطة. لا تعامل الأرشيفات على أنها قرارات نافذة؛ راجع `docs/adr/README.md` و`docs/operations/RECONCILIATION_2026-11-21.md`.

لا تعتمد قيماً مقترحة للتوازن أو تغيّر قواعد اللعب على أنها نهائية دون قرار صريح. استخدم حالات المشروع مثل `PROPOSED` و`PROPOSED FOR PLAYTEST` و`APPROVED` و`IMPLEMENTED` و`VERIFIED` و`TBD` بدقة. لا تصف المحاكاة بأنها اختبار لعب بشري، ولا تصف اختبار Python بأنه مقارنة parity بين C# وPython.

## Git والتحديثات

الفرع الدائم الحالي هو `main`، وتوثّق `ROADMAP.md` و`docs/governance/GIT_AND_MERGE_POLICY.md` إدخال التغييرات المعتمدة مباشرة إلى `main`. اتبع السياسة الحية وتعليمات مالك المستودع لهذه المهمة. لا تفرض PR-only ولا تنشئ سياسة فروع جديدة من تلقاء نفسك، ولا تتجاوز حماية المستودع. إذا رُفض الدفع أو ظهر تعارض، أوقف العملية ووثّق السبب. يحظر `force push` واستبدال تاريخ Git أو مجلد `.git`.

اجعل كل تغيير محدوداً وقابلاً للمراجعة. لا ترفع أسراراً أو ملفات `.env` أو شهادات توقيع أو مخرجات محاكاة. حدّث الفهارس والتقارير والبصمات ذات الصلة عندما تتغير الوثائق أو إعدادات CI.

## التحقق المحلي

اختبارات Domain تستهدف .NET 8؛ شغّلها من جذر المستودع حتى يُقرأ `global.json` الجذري:

```bash
dotnet test tests/CostOfStaying.Domain.Tests/CostOfStaying.Domain.Tests.csproj --configuration Release
python3 tools/balance_simulator/test_parity.py
```

اختبارات Core تستهدف .NET 10 وتستخدم `global.json` مستقلاً. شغّلها من مجلد الاختبارات:

```bash
cd tests/CostOfStaying.Core.Tests
dotnet test CostOfStaying.Core.Tests.csproj --configuration Release
```

`test_parity.py` يختبر محاكي Python فقط. لا تشغّل `tools/playtest/run_sessions.py` ضمن CI؛ مخرجاته محاكاة اصطناعية وليست جلسات بشرية أو فحص CI.

## GitHub Actions

توجد ثلاثة Workflows: `.github/workflows/ci.yml` لـDomain/Python، و`.github/workflows/core-ci.yml` لـCore/.NET 10، و`.github/workflows/docs-governance.yml` لتدقيق الوثائق. يعمل خطا Domain/Python وCore عند الدفع وطلبات الدمج وبجدولة أسبوعية، ويتضمن الأول فحص صياغة ملفات Python عبر `compileall`. يمنح خطا البناء والاختبار `contents: read` فقط ويثبتان إصدارات Actions على SHAs كاملة. وجود YAML أو نجاح اختبار محلي لا يثبت نجاح GitHub Actions. سجّل نتيجة كل تشغيل مع رابط التشغيل وcommit SHA وإصدارات SDK وعدد الاختبارات. لا تصف CI بأنه `VERIFIED` قبل ظهور تشغيل فعلي ناجح على SHA محدد.

## التوثيق والتصميم

استخدم أسماء الوثائق الموجودة، وأضف روابطها إلى `docs/README.md` وسجلها في `docs/governance/POLICY_REGISTER.md` عند انطباقه. أي قرار معماري جديد يتطلب ADR واضحاً وموافقة موثقة؛ لا تستنتج الاعتماد من تعليق كود أو نسخة أرشيف. يبقى Domain وCore منفصلين كما هما في الشفرة حتى يصدر قرار معتمد بشأن الحد أو الترحيل. راجع `ROADMAP.md` لمعرفة الأعمال المنجزة والمفتوحة وحدود ما جرى اختباره.
