# بيان متابعة التوثيق والحوكمة — 2026-11-28

**الحالة:** IMPLEMENTED — DOCUMENTATION PACKAGE COMMITTED; FINAL CI VERIFICATION PENDING  
**المستودع:** `ar-and-you/CostOfStaying`  
**الفرع:** `main`  
**SHA المرجعي للملفات المفهرسة:** `07ab377732979447651a1fd5253a4cd96cde11e8`  
**تاريخ التسجيل:** 2026-11-28

## 1. النطاق

هذه متابعة لحزمة DOC-01 / DOC-02 / GOV-01. تشمل خمس وثائق جديدة وتحديث فهرس الوثائق وسجل القرارات المفتوحة. لا تشمل Source أو Tests أو Dependencies أو Workflows.

## 2. بصمات SHA-256 الفعلية

حُسبت البصمات من محتوى UTF-8 الفعلي للملفات الموجودة على SHA المرجعي أعلاه:

| الملف | SHA-256 | الحجم UTF-8 (bytes) |
|---|---|---:|
| `docs/security/SECURITY_STATUS.md` | `1b8d03d6d585cc155b09b0db869cbf4e74119e5b39e58b83ee5a0290d616760e` | 3119 |
| `docs/quality/CI_MATRIX.md` | `9584c288903c630615559e3b165b16d6cd1ea0817b95c0dc1cc93873ae06cc36` | 2829 |
| `docs/governance/GIT_POLICY_RESOLUTION.md` | `b13bd4483c3dcbc121d432baa6d68433a9226eacef6f6c84bd1127028bccfacb` | 2285 |
| `docs/security/GIT_HISTORY_SCAN_PLAN.md` | `4d63d1844f28be78084339a839f2485010f4fa93c07c02fd41613f05bee723ee` | 2448 |
| `docs/quality/PARITY_PLAN.md` | `c355c064824b9294b66d7f15f03a185291419cedd937693a40b8f336521fca66` | 2304 |
| `docs/README.md` | `721e5265f0348e7137eea25f292e548bddcedecc23d0f30842fc571d52930882` | 24223 |
| `docs/governance/OPEN_DECISION_REGISTER.md` | `016eba7ea1628516ccca85766c4cc421d613f468faa2832b933b362421a2af45` | 14702 |

**حدود manifest:** لا تُدرج بصمة هذا الملف نفسه؛ إدراج SHA-256 ذاتي داخل محتواه ينشئ مرجعاً دائرياً غير قابل للتثبيت. لا يُعد هذا manifest بديلاً عن `MANIFEST.json` أو `SHA256SUMS.txt` التاريخيين، ولم يُعدّل أي منهما.

## 3. توثيق تشغيل Documentation Governance

| Run ID | SHA | النتيجة | الملاحظة |
|---:|---|---|---|
| 46146772876 | `07ab377732979447651a1fd5253a4cd96cde11e8` | FAILURE | محاولة أولى؛ كان الفهرس يشير إلى هذا البيان قبل إنشائه. أُنشئ البيان في متابعة منفصلة لإزالة الرابط المفقود. |

**التحقق النهائي على commit الذي يضيف هذا البيان:** PENDING. يجب تسجيل Run ID والنتيجة النهائية بعد اكتمال تشغيل `Documentation Governance` على SHA هذا commit. لا يُعلن `VERIFIED` قبل نجاح التشغيل المرتبط به.

## 4. الملفات المرجعية التي لم تُعدّل

- `MANIFEST.json` — محفوظ دون تعديل.
- `SHA256SUMS.txt` — محفوظ دون تعديل.
- لا ADRs أُنشئت أو قُبلت.
- لم يُدمج PR #2 ولم تتغير سياسة Git.
- لم تُغيّر إعدادات GitHub أو Branch protection.

## 5. الحالة التشغيلية

- DOC-01: إصلاح الفهرس وإضافة رابط تقرير المرحلة 78؛ ينتظر نجاح docs audit لإغلاق التحقق.
- DOC-02: بيان المتابعة أُنشئ مع بصمات الملفات الفعلية أعلاه.
- F-001 / F-002 / F-003: OPEN؛ هذه الوثائق لا تغلق قرارات المنتج أو التفويض أو الاحتفاظ.
- GOV-01: OPEN؛ Direct-to-main ما زالت السياسة الموثقة النافذة.
