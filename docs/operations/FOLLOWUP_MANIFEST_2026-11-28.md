# بيان متابعة التوثيق والحوكمة — 2026-11-28

**الحالة:** IMPLEMENTED — DOCUMENTATION GOVERNANCE PASSES RECORDED ON TWO SUCCESSIVE REVISIONS  
**المستودع:** `ar-and-you/CostOfStaying`  
**الفرع:** `main`  
**SHA المرجعي الأساسي للملفات:** `c516431712247b2e52661b247b3530b55c031abe` (بصمة GIT_POLICY تخص النص المعدّل في هذا commit)  
**تاريخ التسجيل:** 2026-11-28

## 1. النطاق

هذه متابعة لحزمة DOC-01 / DOC-02 / GOV-01. تشمل خمس وثائق موضوعية جديدة وبيان المتابعة نفسه، وتحديث فهرس الوثائق وسجل القرارات المفتوحة. لا تشمل Source أو Tests أو Dependencies أو Workflows.

## 2. بصمات SHA-256 الفعلية

حُسبت البصمات من محتوى UTF-8 الفعلي للملفات الموجودة على SHA المرجعي أعلاه:

| الملف | SHA-256 | الحجم UTF-8 (bytes) |
|---|---|---:|
| `docs/security/SECURITY_STATUS.md` | `8f60681f73282867fa3238b13700de631fc39eb0e9217174edc52c1aa2f3ac4e` | 3119 |
| `docs/quality/CI_MATRIX.md` | `b36451c903919df7461d41a89d2e688fab4db46b73786a1bcf7cce8bbad805ef` | 2829 |
| `docs/governance/GIT_POLICY_RESOLUTION.md` | `55d78d39dad4b2b5b2d74c9701c2c3fb4d4319ece20d6a7fff556cb089d9f617` | 2971 |
| `docs/security/GIT_HISTORY_SCAN_PLAN.md` | `04ecd525efac1ec0dcfa89965fd5ee0215f8ae13486ba640880a810536c3bdae` | 2448 |
| `docs/quality/PARITY_PLAN.md` | `307228930150c4fa27ea06a73edd79fca77912d5e398f77d58e7a2712e40aaf4` | 2304 |
| `docs/README.md` | `e774cfc0d502945c41098955245f582e12d889b6ebcc9a8ddc668ad972238035` | 24223 |
| `docs/governance/OPEN_DECISION_REGISTER.md` | `c48349826dd134fd568868ba8c7edb93860058a759a2b1b6c5a35602d202f019` | 14817 |

**نطاق البصمات:** البصمات أعلاه هي SHA-256 لمحتوى UTF-8 الفعلي للملفات السبعة. لا تُدرج بصمة هذا البيان نفسه؛ إدراج SHA-256 ذاتي داخله ينشئ مرجعاً دائرياً، ويثبت Git commit SHA محتواه. لا يستبدل هذا البيان `MANIFEST.json` ولا `SHA256SUMS.txt`؛ فهما سجلان تاريخيان لحزمة سابقة ونطاق بصماتهما محدود بتلك الحزمة. الملفات الجديدة موثقة هنا فقط، ولم يُعدّل أي منهما.

## 3. توثيق تشغيل CI على SHA المرجعي

| Workflow | Run ID | SHA | النتيجة | الملاحظة |
|---|---:|---|---|---|
| Documentation Governance — المحاولة الأولى | 46146772876 | `07ab377732979447651a1fd5253a4cd96cde11e8` | FAILURE | Attempt 1: أشار `docs/README.md` إلى FOLLOWUP_MANIFEST قبل إنشائه؛ نتج رابط مفقود. الإصلاح: إنشاء البيان ثم إعادة التشغيل. |
| Documentation Governance — محاولة وسيطة | 46146903123 | `cfa54fcdb415fdd2e4b2298d8ae56265d926bc26` | SUCCESS | تحقق وسيط بعد إنشاء البيان. |
| Documentation Governance — إعادة تحقق | 46147047493 | `96c932cd32a4d8ef9b4bb310327c09179e4399da` | SUCCESS | إعادة تشغيل على مراجعة وسيطة. |
| Documentation Governance | 46147107180 | `c516431712247b2e52661b247b3530b55c031abe` | SUCCESS | تدقيق الروابط والوثائق اليتيمة على SHA المرجعي. |
| Core CI (.NET 10) | 46147107214 | `c516431712247b2e52661b247b3530b55c031abe` | SUCCESS | Core build/test على SHA المرجعي. |
| Domain and Python CI | 46147107219 | `c516431712247b2e52661b247b3530b55c031abe` | SUCCESS | حُسم التشغيل بنجاح بعد أن كان قيد التشغيل في التقرير الأول. |

النتائج تثبت الحالة على SHA المرجعي فقط. أي commit توثيقي لاحق يستلزم إعادة تشغيل Docs Governance على SHA الجديد؛ لا يُنقل نجاح سابق تلقائياً.

## 4. الملفات المرجعية التي لم تُعدّل

- `MANIFEST.json` — محفوظ دون تعديل.
- `SHA256SUMS.txt` — محفوظ دون تعديل.
- لا ADRs أُنشئت أو قُبلت.
- لم يُدمج PR #2 ولم تتغير سياسة Git.
- لم تُغيّر إعدادات GitHub أو Branch protection.

## 5. الحالة التشغيلية

- DOC-01: RESOLVED على SHA المرجعي؛ Documentation Governance run 46147107180 = SUCCESS.
- DOC-02: RESOLVED؛ البصمات الفعلية ونطاق MANIFEST التاريخي موضحان أعلاه.
- F-001 / F-002 / F-003: OPEN؛ هذه الوثائق لا تغلق قرارات المنتج أو التفويض أو الاحتفاظ.
- GOV-01: OPEN؛ المالك هو مالك المشروع، والاستحقاق TBD بانتظار تحديد المالك. Direct-to-main ما زالت السياسة النافذة.
