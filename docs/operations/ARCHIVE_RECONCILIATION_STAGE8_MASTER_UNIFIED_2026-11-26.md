# المرحلة 8 — مصالحة CostOfStaying_MASTER_UNIFIED_2026-11-19.zip

**التاريخ:** 2026-11-26  
**المستودع المرجعي:** `ar-and-you/CostOfStaying`  
**الفرع:** `main`  
**SHA الشجرة الحية المقروءة:** `7c9985d34e92d00413a457711a711f1579d7c517`  
**الحالة:** `ARCHIVE COMPARED — NO SOURCE IMPORT APPROVED`

## 1. النطاق وسلامة الأرشيف

- الملف: `CostOfStaying_MASTER_UNIFIED_2026-11-19.zip`
- SHA-256 للحاوية: `116c9e797aa6f2527eb79aa939b1a5a5366386ed87bda5c2a23662eb16fb5f29`
- عدد إدخالات ZIP: 270؛ لا أخطاء CRC (`testzip() = None`).
- الملفات العادية: 270.
- بصمات SHA-256 الفريدة داخل الحزمة: 270.
- لا يحتوي الأرشيف على ZIP متداخل.
- أُنشئت نسخة استخراج وفهرس مستقلان محلياً، ولم تتغير الحزمة الأصلية.

## 2. منع تكرار المراجعة

قورنت الحزمة بعد تطبيع بادئة `CostOfStaying/` مع ملف `CostOfStaying-main-8b811b0.zip` الموجود داخل `CostOfStaying-FINAL-upload-package.zip`، الذي سبق توثيق فحصه في المرحلة 7.18.

| نتيجة المقارنة | العدد |
|---|---:|
| ملفات MASTER UNIFIED | 270 |
| ملفات حزمة 8b811b0 | 270 |
| المسارات المشتركة بعد التطبيع | 268 |
| ملفات متطابقة بايتياً ضمن المسارات المشتركة | 258 |
| مسارات مشتركة مختلفة المحتوى | 10 |
| ملفات خاصة بـMASTER UNIFIED | 2 |
| ملفات خاصة بحزمة 8b811b0 | 2 |

**قاعدة عدم التكرار:** لا يعاد فتح أو تدقيق الملفات الـ258 المتطابقة؛ يُحال إلى سجل المرحلة 7.18. وتقتصر المراجعة الإضافية على الفروقات العشرة والملفات الأربعة الخاصة بالحزمتين.

## 3. الفروقات العشرة

| المسار | طبيعة الفرق | قرار هذه الجولة |
|---|---|---|
| `.editorconfig` | حزمة 8b811b0 تضيف قواعد تنسيق Python/Shell وMakefile | لا استيراد؛ افحص الإعداد الحي قبل أي تحسين |
| `.gitignore` | فروق في cache وobj وملفات التدقيق المؤرشفة | لا استيراد؛ النسخة الحية هي المرجع |
| `CONTRIBUTING.md` | حزمة 8b811b0 تضيف سياسة فروع هجينة مرتبطة بـADR-028 | رفض كسياسة نافذة؛ لا تنقل ADR-028 أو سياسة الفروع من أرشيف |
| `README.md` | اختلاف نقطة البداية وروابط الحوكمة وإضافة قسم حوكمة في حزمة 8b811b0 | الاحتفاظ بـREADME الحي |
| `docs/00_PROJECT_CHARTER.md` | صياغة حالة Domain Core مختلفة | الاحتفاظ بميثاق main الحالي؛ لا تنقل توصيف الحالة التاريخي |
| `docs/README.md` | حزمة 8b811b0 تضيف فهرسة لملفات governance/BRANCHING_MODEL وADR-028 | لا استيراد؛ تلك المسارات ليست جزءاً من شجرة main الحالية |
| `docs/governance/CONFLICT_RESOLUTIONS.md` | حزمة 8b811b0 تضيف تسويات 32–36، بعضها مرتبط بقرارات/سياسات قديمة | أرشيف تاريخي؛ لا تدمج قراراته |
| `docs/governance/DECISIONS_LOG.md` | حزمة 8b811b0 تضيف ADR-028 وادعاء اعتماد سياسة هجينة | لا اعتماد؛ يتعارض مع سياسة main الحية |
| `docs/governance/DOCUMENT_GOVERNANCE.md` | إضافات كتالوج BRANCHING_MODEL وADR-028 | لا استيراد؛ بنية الحوكمة الحية مختلفة |
| `docs/roadmap/WORK_PLAN_V2_DETAILED.md` | تصحيح مرجع دخول الخطة من SHA قديم إلى fc8dd37 | الخطة تاريخية/مقترحة وليست مرجع التنفيذ الحي |

لم يظهر في الفروقات العشرة ما يبرر استبدال الملف الحي أو نقل السياسة القديمة. بعض التعديلات في حزمة 8b811b0 مفيدة تاريخياً لفهم مسار الحوكمة، لكنها لا تتقدم على الحالة الحالية.

## 4. ملفات الحزمتين غير المشتركة

**خاص بـMASTER UNIFIED:**
- `MERGE_MANIFEST.md`: سجل تجميع يحدد FINAL-verified-2026-11-20 كقاعدة، ويصرح أن نقل Domain إلى Core وUnity/FishNet وAndroid لم يكتمل. مفيد كدليل مصدر تاريخي، لكنه لا يعكس الشجرة الحية في 2026-11-26؛ القرار `ARCHIVE_ONLY`.
- `SHA256SUMS.txt`: manifest بصمات خاص بتلك اللقطة؛ يحتفظ به كبيانات تحقق للحزمة ولا يُستخدم كـmanifest للمستودع الحالي.

**خاص بحزمة 8b811b0:**
- `docs/governance/BRANCHING_MODEL.md`
- `docs/governance/adr/ADR-028-HYBRID-BRANCHING-POLICY.md`

وجود هذين الملفين في الحزمة الأخرى لا يمنح سياسة ADR-028 نفاذاً في main. ويظل تعارضها مع قواعد المستودع الحالية سبباً كافياً لعدم استيرادها.

## 5. مقارنة عينة من الملفات بالمصدر الحي

تم فحص GitHub recursive tree عند SHA المذكور، ثم مقارنة ملفات مختارة مباشرة. أمثلة:

- `global.json` متطابق بايتياً مع الأرشيف.
- `src/CostOfStaying.Core/CoreComposition.cs` متطابق.
- `src/CostOfStaying.Core/Services/MatchCommandAuthorizationService.cs` متطابق.
- `src/CostOfStaying.Domain/Player.cs` متطابق.
- `src/CostOfStaying.Domain/Match.cs` مختلف؛ النسخة الحية أطول (24,671 مقابل 21,935 بايت)، لذلك لا تُستبدل بالنسخة الأقدم.
- ملفات مثل `.github/workflows/security.yml` و`.github/workflows/unity-mobile.yml` و`tools/audit.py` و`docs/architecture/PHASE1_CORE_IMPLEMENTATION.md` و`docs/architecture/PHASE1_TECHNICAL_MASTER.md` و`docs/audit/UPLOADED_PHASE1_SOURCE_AUDIT.md` غير موجودة بهذه المسارات في الشجرة الحية. عدم وجودها لا يعني أن محتواها ينبغي نقله؛ راجع تحليل التوافق أدناه.

هذه عينة مقارنة وليست ادعاءً بمقارنة كل 270 ملفاً مع main. جرد كل الملفات موثق في سجل TSV المحلي، ومقارنة الشجرة الكاملة تحتاج مرحلة مستقلة إذا لم تكن قد غُطيت في مراحل سابقة.

## 6. محتوى هندسي مفيد — مع حالة الاعتماد

### 6.1 تقرير فحص المصدر المرفوع

`docs/audit/UPLOADED_PHASE1_SOURCE_AUDIT.md` يحتوي تحذيرات تاريخية مفيدة:
- عدم اعتماد transcript أو snippets كأنها مشروع قابل للبناء.
- منع دمج نسخ متعددة من النوع نفسه أو إعادة إدخال Guid-based identity بعد اعتماد اتجاه مختلف.
- الفصل بين Vitality وlife state وnetwork presence وsession state وcommand eligibility.
- إبقاء Core خالياً من Unity/FishNet APIs.
- التمييز بين تصميم server-authoritative وبين تطبيقه الفعلي.

هذه النقاط متوافقة مع مبادئ التحقق الحالية، لكن نتائج التقرير وحالات ADR وأرقام PR/CI داخله تاريخية؛ لا تنقل كحالة حالية. ويجب عدم إعادة فتح هذه النقاط إن كانت مسجلة في تقارير المصالحة الحية.

### 6.2 PHASE1_CORE_IMPLEMENTATION وPHASE1_TECHNICAL_MASTER

تصفان خدمات Core وsession/connection binding وcommand admission وtyped payload validation، لكنهما تستندان إلى PRs منفصلة وغير مدمجة، وإلى أرقام اختبارات/تشغيلات تعود إلى نوفمبر 2026-11-14. لا يجوز اعتبارها إثباتاً بأن تلك PRs اندمجت أو أن Unity/FishNet/auth/network runtime نُفذت. تُعامل كأدلة تصميم وتاريخ تطوير فقط.

### 6.3 أدوات audit وGitHub config

- `tools/audit.py` يفترض وجود `docs/governance/DOCUMENT_GOVERNANCE.md` و`DECISIONS_LOG.md` و`docs/governance/adr/`؛ لا تتوافق هذه الاعتماديات مع بنية main الحالية. لا يُنقل كما هو.
- `tools/github_config_tests/test_github_config.py` يفترض مجموعة Workflows وDependabot وCODEOWNERS وrulesets لا تطابق التكوين الحي الحالي. لا يُنقل كما هو.
- Workflows الأمنية وUnity/Android الموجودة في الأرشيف ليست إثباتاً على تشغيلها أو توفر الأسرار أو وجود مشروع Unity. لا تُنسخ إلى `.github/workflows/` دون تصميم وتدقيق صلاحيات وسياق تشغيل واختبارات مستقلة.

## 7. القرارات

- 258 ملفاً متطابقاً: `NO_REAUDIT — REFER TO STAGE 7.18`.
- الفروقات العشرة: `KEEP_LIVE / ARCHIVE_ONLY` حسب الجدول.
- `MERGE_MANIFEST.md` و`SHA256SUMS.txt`: `ARCHIVE_ONLY`.
- ملفات ADR-028 وسياسة الفروع الهجينة: `REJECT_AS_LIVE_POLICY`.
- أدوات التدقيق القديمة: `REJECT_AS_DIRECT_OVERLAY`; يمكن إعادة تصميم اختبار محدد لاحقاً ليتوافق مع بنية main الفعلية.
- لا استبدال أو حذف أو نقل مصدر في هذه المرحلة.

## 8. ملفات الجرد المحلية

- `STAGE2_MASTER_UNIFIED_FILE_INVENTORY.tsv`: 270 مساراً مع الحجم وSHA-256 وGit blob SHA-1.
- `stage2_master_unified/`: استخراج الملفات للفحص.
- `STAGE1_TAHLIL_RECONCILIATION_2026-11-26.md`: نتائج المرحلة السابقة.

هذه المخرجات محلية في بيئة التحليل. هذا التقرير هو سجل القرار المنشور في المستودع؛ لا يدعي أن ملف TSV المحلي رُفع إلى GitHub.
