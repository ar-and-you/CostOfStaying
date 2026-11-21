# تقرير التحقق التشغيلي — 2026-11-20
**الحالة: EVIDENCE LOG | نتائج فعلية من بيئة تحقق محلية (SDK 8.0.428 + SDK 10.0.404)، تغطي الفجوة الموثقة في [MERGE_REPORT](../governance/MERGE_REPORT_2026-11-19.md) §6**

## 1. البيئة
| المكوّن | الإصدار |
|---|---|
| .NET SDK (Domain/sln) | 8.0.428 (مطابق لـ `global.json`: 8.0.400 + rollForward latestFeature) |
| .NET SDK (Core.Tests) | 10.0.404 (مطابق لـ `tests/CostOfStaying.Core.Tests/global.json`: 10.0.100 + rollForward) |
| Python | 3.12 |
| النظام | Linux x64 |

## 2. نتائج البناء والاختبارات
| الفحص | الأمر | النتيجة |
|---|---|---|
| بناء الحل | `dotnet build CostOfStaying.sln` | ✅ نجاح، 0 أخطاء (95 تحذير CS1591 توثيق XML في Domain فقط، لا تفشل البناء؛ `TreatWarningsAsErrors=false` موثق) |
| اختبارات Domain | `dotnet test CostOfStaying.sln` | ✅ 37/37 ناجحة (0 فاشلة، 0 متجاوزة) |
| بناء Core | `dotnet build src/CostOfStaying.Core` | ✅ نجاح، 0 أخطاء، 0 تحذيرات (`TreatWarningsAsErrors=true`) |
| اختبارات Core | `dotnet test tests/CostOfStaying.Core.Tests` (من مجلده، SDK 10) | ✅ 246/246 ناجحة |
| تدقيق الوثائق | `python3 tools/audit.py` | ✅ 114 وثيقة، 0 مشاكل، 0 تحذيرات |
| اختبارات إعداد GitHub | `python3 tools/github_config_tests/test_github_config.py` | ✅ 66/66 ناجحة |
| تطابق المحاكاة | `python3 tools/balance_simulator/test_parity.py` | ✅ parity ok |

## 3. فحوص CI المحلية (مطابقة لـ `ci.yml`)
| الخطوة | النتيجة |
|---|---|
| `simulate.py --preset default --seed 42 --json` | ✅ نتيجة ضمن القيم الخمس المسموحة (LastSurvivor) |
| `run_sessions.py --sessions 10 --preset default` | ✅ 10 جلسات مكتملة بمتوسط 337.0s و87.5 نقل |
| مسح الأسرار (grep heuristic الخاص بـ CI) | ✅ لا تطابقات |

## 4. ملاحظات توازن من المحاكاة (3 presets × 6-10 جلسات)
- التوزيع في كل الجلسات المحاكاة كان 100% LastSurvivor.
- **السبب الجذري:** المحاكي (`tools/balance_simulator/simulate.py`) يفتح التصويت عند 3 أحياء لكن سياسة البوتات لا ترسل أي تصويت Yes؛ جميع الأصوات تبقى Abstain ⇒ غير متوافقة ⇒ يعود النمط إلى Running. لا يوجد مسار في المحاكاة يؤدي إلى CollectiveSurrender.
- مسار التصويت الموافق للتوافق الإجماعي **مغطى في اختبارات Domain** (`VoteAndEndgameTests.cs`) والكود فعلياً يدعمه؛ النقص في سياسة البوت التجريبية فقط.
- **التوصية:** قبل رفع ADR-004 إلى APPROVED يلزم (1) جلسات بشرية حقيقية كما ينص الباب، و(2) اختيارياً إضافة سياسة تصويت للبوتات في المحاكي لجعل توزيع النتائج خمسياً.

## 5. ملخص الحكم
- إصلاحا الدمج (MERGE_REPORT §3) **مؤكدان بالبناء والاختبار**: `InternalsVisibleTo` يعمل، واختبار `Collapse_Reschedule` (‎17.525‎) ينجح.
- لا أخطاء بناء، ولا فشل اختبارات، ولا روابط مكسورة، ولا حالات غير موثقة، ولا أسرار مكشوفة في المسح الاستدلالي.
- الفجوة الوحيدة الباقية هي بشرية بطبيعتها: جلسات اللعب البشري لبوابة ADR-004 وبناء Unity — كما هو موثق أصلاً في CHANGELOG.
