# CostOfStaying
لعبة بقاء نفسي جماعية؛ كل ثانية تستنزف `Vitality`، ونقل المعاناة يؤخر الانهيار لكنه يضيف كلفة `Betrayal`. المشروع في مرحلة ما قبل الإنتاج، والقواعد وقيم التوازن المقترحة ليست قرارات نهائية.

## بنية المشروع
- `src/CostOfStaying.Domain/` — منطق المجال المستقل عن Unity، يستهدف `netstandard2.1`.
- `src/CostOfStaying.Core/` — مكونات أساسية مساندة، تستهدف `netstandard2.1`.
- `tests/CostOfStaying.Domain.Tests/` — اختبارات NUnit على `.NET 8`.
- `tests/CostOfStaying.Core.Tests/` — اختبارات NUnit مستقلة على `.NET 10`.
- `tools/` — محاكي Python استكشافي وسكربت لإخراج سجلات محاكاة.

## التحقق المحلي
من جذر المستودع، شغّل اختبارات المجال:

```bash
dotnet test tests/CostOfStaying.Domain.Tests/CostOfStaying.Domain.Tests.csproj
python3 tools/balance_simulator/test_parity.py
```

اختبارات Core تستخدم SDK منفصلاً؛ نفّذها من مجلدها حتى يختار `global.json` المحلي .NET 10:

```bash
cd tests/CostOfStaying.Core.Tests
dotnet test CostOfStaying.Core.Tests.csproj
```

سجلات `tools/playtest/run_sessions.py` محاكاة آلية فقط، وليست جلسات لعب بشرية ولا دليلاً على توازن معتمد. اختبارات Python لا تقارن التنفيذ مباشرة مع C#.

## حالة التكامل
حتى تاريخ 2026-11-21 لا توجد GitHub Actions workflows في الفرع `main`. لم يُتحقق من تكامل Unity أو بناء خادمي/شبكي؛ لا تفترض أن نجاح الاختبارات المحلية يثبت هذه الجوانب.
