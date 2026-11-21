# CostOfStaying

لعبة بقاء نفسي جماعية؛ تستنزف `Vitality` بمرور الوقت، ويؤخر نقل المعاناة الانهيار لكنه يضيف كلفة `Betrayal`. المشروع في مرحلة ما قبل الإنتاج؛ القواعد وقيم التوازن المقترحة ليست قرارات نهائية.

## الحالة الفعلية للمشروع

- `src/CostOfStaying.Domain/`: منطق المجال المستقل عن Unity، ويستهدف `netstandard2.1`.
- `src/CostOfStaying.Core/`: طبقة Core منفصلة حالياً، وتستهدف `netstandard2.1`.
- `tests/CostOfStaying.Domain.Tests/`: اختبارات NUnit تستهدف `.NET 8`.
- `tests/CostOfStaying.Core.Tests/`: اختبارات NUnit تستهدف `.NET 10` وتستخدم `global.json` مستقلاً داخل مجلد الاختبارات.
- `tools/balance_simulator/`: محاكي Python وملفات presets ومسح المعاملات.
- `tools/playtest/`: مولد سجلات جلسات محاكاة آلية؛ لا ينتج جلسات لعب بشرية.
- `test_parity.py` يختبر سلوك محاكي Python فقط؛ لا يشغّل C# ولا يثبت التكافؤ بين اللغتين.
- لا يوجد مشروع Unity أو بناء Android/iOS أو خادم شبكي قابل للتشغيل في هذا المستودع حتى تاريخ هذه المراجعة.

## التحقق المحلي

من جذر المستودع، شغّل خط Domain وPython:

```bash
dotnet restore CostOfStaying.sln
dotnet build CostOfStaying.sln --configuration Release --no-restore
dotnet test CostOfStaying.sln --configuration Release --no-build
python3 tools/balance_simulator/test_parity.py
```

اختبارات Core تستخدم SDK منفصلاً. شغّلها من مجلدها حتى يقرأ `dotnet` ملف `global.json` المحلي:

```bash
cd tests/CostOfStaying.Core.Tests
dotnet restore CostOfStaying.Core.Tests.csproj
dotnet build CostOfStaying.Core.Tests.csproj --configuration Release --no-restore
dotnet test CostOfStaying.Core.Tests.csproj --configuration Release --no-build --no-restore
```

لتشغيل الأدوات الاستكشافية:

```bash
python3 tools/balance_simulator/simulate.py --preset default --seed 42
python3 tools/balance_simulator/sweep.py --runs 20
python3 tools/playtest/run_sessions.py --sessions 10 --preset default
```

كل مخرجات المحاكاة فرضيات استكشافية فقط؛ لا تمثل جلسات لعب بشرية ولا تثبت توازناً معتمداً.

## Git وCI

- يبقى `main` الفرع الدائم، وتُنفذ التحديثات المعتمدة مباشرة عليه وفق تعليمات `ROADMAP.md` الحالية.
- لا تستخدم `force push` ولا تستبدل تاريخ Git أو مجلد `.git`.
- يحتوي المستودع ملفي CI مستقلين: `.github/workflows/ci.yml` لـDomain/Python و`.github/workflows/core-ci.yml` لـCore/.NET 10. يشغّلان الاختبارات المسماة فقط، ولا يشغّلان `tools/playtest/run_sessions.py` أو `tools/audit.py`.
- إعداد workflow لا يثبت نجاح CI. يجب التحقق من نتيجة GitHub Actions الفعلية وربطها بـcommit SHA قبل وصف الفحوص بأنها `VERIFIED`.
- لا يوجد في هذه الدفعة مسح أمني مخصص مثل CodeQL أو gitleaks. فحص الأنماط البسيط، إن استُخدم، ليس بديلاً عنهما.
- راجع `docs/operations/RECONCILIATION_2026-11-21.md` لمعرفة التعارضات التي حُسمت والعناصر المؤجلة.

## التوثيق والمساهمة

- راجع `CONTRIBUTING.md` لسياسة المساهمة والاختبارات، و`docs/README.md` لفهرس الوثائق.
- سجلات ADR غير موجودة حالياً على `main`؛ يوضح `docs/adr/README.md` طريقة التعامل مع المصادر الأرشيفية المتعارضة.

## حدود التحقق

نتائج الفحص المحلي المسجلة في التقارير السابقة تخص النسخ والبيئات التي فُحصت وقتها. لا تُنسب إلى `main` أو إلى GitHub Actions إلا إذا ارتبطت بالالتزام الصحيح وظهرت نتيجة التشغيل الفعلية.

## تعليمات وكلاء الذكاء الاصطناعي

- راجع `AGENTS.md` قبل أي تغيير آلي في المستودع.
- عند اختلاف تعليمات الوكيل مع قرار معماري أو سياسة موثقة، ارجع إلى ترتيب مصادر الحقيقة المذكور في `AGENTS.md` وإلى القرار/الوثيقة الأعلى أولوية.
- اتبع سياسة Git الحالية في `ROADMAP.md`؛ لا تعتبر وجود AGENTS.md تغييراً تلقائياً لسياسة الفروع أو الدمج.
