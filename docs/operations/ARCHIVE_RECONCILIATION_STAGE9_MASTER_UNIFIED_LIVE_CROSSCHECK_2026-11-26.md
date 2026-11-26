# المرحلة 9 — إعادة مصالحة MASTER UNIFIED مع main الحالي

**التاريخ:** 2026-11-26  
**المصدر:** `CostOfStaying_MASTER_UNIFIED_2026-11-19.zip`  
**المستودع:** `ar-and-you/CostOfStaying`  
**الفرع:** `main`  
**رأس الالتزام المقروء:** `17a1f4cabd3b1df68d97326183e38b2da8421b35`  
**SHA شجرة Git:** `17a1f4cabd3b1df68d97326183e38b2da8421b35`  
**الحالة:** `TARGETED LIVE CROSS-CHECK COMPLETE — FULL 270-FILE LIVE RECONCILIATION OPEN`

## 1. منع إعادة العمل والتحقق من الأرشيف

المرحلة 8 موثقة مسبقاً في [ARCHIVE_RECONCILIATION_STAGE8_MASTER_UNIFIED_2026-11-26.md](ARCHIVE_RECONCILIATION_STAGE8_MASTER_UNIFIED_2026-11-26.md). لم أعد فحص الـ258 ملفاً المتطابقاً في المقارنة السابقة مع حزمة 8b811b0، ولم أكرر تحليل الفروقات العشرة المسجل هناك. هذه الجولة تضيف تحققاً موجهاً من عينة الملفات مع رأس `main` الأحدث.

- SHA-256 للحاوية: `116c9e797aa6f2527eb79aa939b1a5a5366386ed87bda5c2a23662eb16fb5f29`.
- 270 ملفاً، 270 بصمة SHA-256 فريدة، بلا ZIP متداخل، وCRC سليم.
- تم التحقق من 237 إدخالاً موجوداً في `SHA256SUMS.txt`؛ جميعها تطابق (237/237). لا يشمل هذا الادعاء الملفات الـ33 غير الموجودة في manifest.
- شجرة `main` الحالية غير مقتطعة: 201 ملفاً متتبعاً، منها 50 تحت `src/` و33 تحت `tests/`.

## 2. مقارنة مصدر/اختبار موجّهة مع main

تمت قراءة بصمات Git blob من GitHub للملفات الحية ومقارنتها ببصمات Git blob المحسوبة من ملفات الأرشيف.

### مطابق بايتياً في العينة

- `src/CostOfStaying.Domain/Player.cs`
- `src/CostOfStaying.Domain/Intent.cs`
- `src/CostOfStaying.Domain/Clock.cs`
- `src/CostOfStaying.Core/CoreComposition.cs`
- `src/CostOfStaying.Core/Services/MatchCommandAuthorizationService.cs`
- `tests/CostOfStaying.Domain.Tests/TransferTests.cs`
- `global.json`
- `Directory.Packages.props`
- `CostOfStaying.sln`

**القرار:** `KEEP_LIVE / NO_IMPORT`. لا توجد إضافة في نسخة الأرشيف لهذه المسارات.

### موجود في main لكنه مختلف عن الأرشيف

- `src/CostOfStaying.Domain/Match.cs`
- `src/CostOfStaying.Domain/Debuff.cs`
- `src/CostOfStaying.Domain/MatchConfig.cs`
- `tests/CostOfStaying.Domain.Tests/CollapseTests.cs`
- `tests/CostOfStaying.Domain.Tests/SecurityTests.cs`
- `tests/CostOfStaying.Domain.Tests/VitalityTests.cs`
- `.github/workflows/ci.yml`
- `.github/workflows/core-ci.yml`

الاختلاف ليس سبباً للاستبدال. القراءة المباشرة للنسخ الحية تؤكد وجود تحسينات/حواجز لا تظهر في نسخة الأرشيف، منها:
- رفض قيم Tick غير الموجبة وغير المحدودة في `Match.Tick`.
- التحقق من إعدادات `MatchConfig` وإنشاء snapshot معزول من الإعدادات.
- التحقق من finite/non-negative في `Debuff`.
- تقسيم حساب Vitality عند حدود انتهاء Debuff في tick طويل.
- تهيئة مؤقت Collapse موحد لكل اللاعبين عند بدء المباراة.

**القرار:** `KEEP_LIVE`. لا تُستبدل هذه الملفات بنسخ 2026-11-19 الأقدم.

## 3. مقارنة تغطية الاختبارات

| مجموعة الاختبارات | نسخة الأرشيف | main الحالي | الزيادة الحية الظاهرة |
|---|---:|---:|---|
| CollapseTests | 3 اختبارات | 4 | `Start_Assigns_The_Same_Initial_Collapse_Timer_To_Every_Player` |
| SecurityTests | 4 | 9 | 5 اختبارات إضافية للتحقق من config snapshot والقيم غير المحدودة والمسافات غير الصالحة وما بعد انتهاء المباراة |
| VitalityTests | 6 | 7 | اختبار مساهمة Debuff فقط خلال المدة الفعلية داخل tick طويل |
| TransferTests | 10 | 10 | تطابق بايتياً |

هذا دليل واضح على أن اختبارات الأرشيف لهذه المجموعات أقدم من اختبارات main. لا يُعاد إدخال نسخها القديمة ولا يُحذف أي اختبار حي.

## 4. Workflows: توافق غير كافٍ للاستيراد

تضم الحزمة 13 ملف Workflow. الشجرة الحية الحالية تحتوي `.github/workflows/ci.yml` و`core-ci.yml` و`docs-governance.yml` و`.gitkeep` فقط.

الملفات الأرشيفية غير الموجودة في main تشمل:
- `codeql.yml`
- `contrast-scan.yml`
- `debricked.yml`
- `frogbot-scan-and-fix.yml`
- `mobsf.yml`
- `nowsecure.yml`
- `repo-quality.yml`
- `security.yml`
- `securitycodescan.yml`
- `unity-mobile.yml`
- `.github/workflows/README.md`

لا تُنسخ تلقائياً. بعضها يعتمد على أسرار خارجية أو GHAS أو أدوات/خدمات مورّدين، وUnity workflow لا يثبت وجود مشروع Unity أو إعداد Android. يلزم تصميم مستقل لكل workflow، وتحديد صلاحياته وأحداث تشغيله والأسرار المطلوبة، ثم فحصه واختباره قبل اعتماده.

## 5. أدوات الأرشيف غير الموجودة في main

### أدوات لا تصلح كنسخ مباشرة

- `tools/agents/dispatch.py`: يعتمد على `docs/governance/EXECUTION_BOARD.md` و`AGENT_CHARTER.md` و`docs/roadmap/WORK_PLAN_V2_DETAILED.md` وكتالوج `DOCUMENT_GOVERNANCE.md`؛ هذه ليست بنية main الحالية. لا ينقل كما هو.
- `tools/audit.py`: يعتمد على `DOCUMENT_GOVERNANCE.md` و`DECISIONS_LOG.md` ومجلد ADR قديم وmanifest استيراد غير متوافق مع تنظيم الوثائق الحي. لا ينقل كما هو.
- `tools/github_config_tests/test_github_config.py`: يفترض workflows وCODEOWNERS وrulesets وDependabot وتكويناً لا يطابق الشجرة الحية؛ لا ينقل كما هو. يمكن إعادة تصميم اختبارات محددة لتفحص الملفات الموجودة فعلاً.
- `tools/repo_setup/set_owner.py`: يتطلب `.github/CODEOWNERS.template` ومالكاً حقيقياً لتوليد CODEOWNERS؛ القالب غير موجود في main الحالي ولا ينبغي اختلاق المالك. لا ينقل أو يشغّل حالياً.

### أدوات المحاكاة واللعب

- `tools/balance_simulator/presets.json` مطابق بايتياً.
- `simulate.py` و`sweep.py` و`test_parity.py` مختلفة عن النسخ الحية؛ النسخة الحية تحمل تنبيهاً صريحاً أن المحاكي ليس mirror تنفيذياً لـC# ولا يثبت parity.
- `tools/playtest/run_sessions.py` مختلف؛ النسخة الحية تصرح أن الجلسات محاكاة آلية وليست Human Playtests أو بوابة ADR-004.
- `tools/playtest/test_cli.py` موجود في main ولا يوجد في هذه الحزمة.

**القرار:** `KEEP_LIVE` للأدوات الحية. لا استيراد للنسخ الأرشيفية. يمكن إعادة استخدام اختبارات config المفردة بعد تكييفها مع ملفات main الفعلية، لا استيراد الحزمة كاملة.

## 6. حدود التحقق

- المقارنة الحية في هذه الجولة عينة موجهة للملفات الأعلى خطورة، وليست مقارنة byte-for-byte لكل 270 مساراً.
- لم يتم تشغيل `dotnet test` محلياً؛ بيئة التحليل لا تحتوي على .NET SDK (`dotnet: command not found`).
- لم تُشغّل Workflows الأرشيفية، ولم تُفحص أسرار أو حسابات مورّدين.
- لا يثبت هذا التقرير صلاحية Unity/Android أو تكامل الشبكة أو اللعب البشري.

## 7. القرارات النهائية لهذه الجولة

- لا استبدال أو دمج لملفات المصدر أو الاختبارات.
- لا استيراد Workflows الأمنية/Unity من الأرشيف.
- لا استيراد أدوات audit/dispatch/repo-setup/config tests بصيغتها القديمة.
- الاحتفاظ بالنسخ الأصلية كأدلة تاريخية وعدم حذفها.
- تُحال الملفات المطابقة إلى سجلات الفحص السابقة؛ لا يعاد فحصها إلا إذا تغير SHA أو ظهرت حاجة محددة.
- تبقى المصالحة الكاملة لكل 270 ملفاً مع رأس main الحالي مفتوحة، وتحتاج جدول crosswalk شامل قبل إعلان الإغلاق.

**الخلاصة:** MASTER UNIFIED مفيد كمصدر تاريخي لتتبع تطور Domain/Core والاختبارات وسير العمل، لكنه لا يقدم في العينة المفحوصة نسخة أفضل من main الحالي. لا يوجد ملف مصدر من هذه الجولة يستحق الاستبدال المباشر.
