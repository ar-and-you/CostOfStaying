# المساهمة في CostOfStaying

## الحالة ومصادر الحقيقة

المشروع في مرحلة ما قبل الإنتاج. قبل أي تعديل، اقرأ `AGENTS.md` و`README.md` و`ROADMAP.md`، ثم راجع مواصفة النظام والقرار المعماري ذي الصلة إن كان موجوداً على `main`. عند التعارض، تكون الأولوية للـADR المعتمد، ثم مواصفة القواعد، ثم وثيقة النظام. لا تعامل الأرشيفات غير المستوردة على أنها قرارات نافذة؛ راجع `docs/adr/README.md` و`docs/operations/RECONCILIATION_2026-11-21.md`.

لا تعتمد قيماً مقترحة للتوازن أو تغيّر قواعد اللعب على أنها نهائية دون قرار صريح. استخدم حالات المشروع مثل `PROPOSED` و`PROPOSED FOR PLAYTEST` و`APPROVED` و`IMPLEMENTED` و`VERIFIED` و`TBD` بدقة. لا تصف المحاكاة بأنها اختبار لعب بشري، ولا تصف اختبار Python بأنه مقارنة parity بين C# وPython.

## Git والتحديثات

الفرع الدائم الحالي هو `main`، وتوثّق `ROADMAP.md` سياسة إدخال التغييرات المعتمدة مباشرة إلى `main`. اتبع السياسة الحية وتعليمات مالك المستودع لهذه المهمة. لا تنشئ سياسة فروع/مراجعات جديدة من تلقاء نفسك، ولا تتجاوز حماية المستودع. إذا رُفض الدفع أو ظهر تعارض، أوقف العملية ووثّق السبب. يحظر `force push` واستبدال تاريخ Git أو مجلد `.git`.

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

الـworkflows هما `.github/workflows/ci.yml` لـDomain/Python و`.github/workflows/core-ci.yml` لـCore/.NET 10. يمنح كل منهما `contents: read` فقط. وجود YAML أو نجاح اختبار محلي لا يثبت نجاح GitHub Actions. سجّل نتيجة كل تشغيل مع رابط التشغيل وcommit SHA وإصدارات SDK وعدد الاختبارات. لا تصف CI بأنه `VERIFIED` قبل ظهور تشغيل فعلي ناجح على SHA محدد.

## التوثيق والتصميم

استخدم أسماء الوثائق الموجودة، وأضف روابطها إلى `docs/README.md`. أي قرار معماري جديد يتطلب سجلاً منفصلاً بحالة واضحة وتبعاته؛ لا تستنتج اعتماد قرار من تعليق كود أو نسخة أرشيف. يبقى الفصل الحالي بين Domain وCore قائماً إلى أن تُراجع قراراته وتُعتمد خطة ترحيل. راجع `ROADMAP.md` لمعرفة الأعمال المنجزة والمفتوحة وحدود ما جرى اختباره.
