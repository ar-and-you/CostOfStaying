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
| `docs/security/SECURITY_STATUS.md` | `1b8d03d6d585cc155b09b0db869cbf4e74119e5b39e58b83ee5a0290d616760e` | 3119 |
| `docs/quality/CI_MATRIX.md` | `9584c288903c630615559e3b165b16d6cd1ea0817b95c0dc1cc93873ae06cc36` | 2829 |
| `docs/governance/GIT_POLICY_RESOLUTION.md` | `df24f59e456da676b0f1643c508931b22ca65fc87d24205f3df0953c850c2170` | 2971 |
| `docs/security/GIT_HISTORY_SCAN_PLAN.md` | `4d63d1844f28be78084339a839f2485010f4fa93c07c02fd41613f05bee723ee` | 2448 |
| `docs/quality/PARITY_PLAN.md` | `c355c064824b9294b66d7f15f03a185291419cedd937693a40b8f336521fca66` | 2304 |
| `docs/README.md` | `721e5265f0348e7137eea25f292e548bddcedecc23d0f30842fc571d52930882` | 24223 |
| `docs/governance/OPEN_DECISION_REGISTER.md` | `005d0dc64dfd36865044e40d76be4d3f7b9e7ce5b715f28cebde655bbc59b616` | 14817 |

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

## 3.1 إعادة التحقق على SHA الأحدث — 2026-11-28

| Workflow | Run ID | SHA | النتيجة | المدة/الملاحظات |
|---|---:|---|---|---|
| Documentation Governance | 46150214763 | `41c722333597d6dd28d0ddf062be3e564a48bd47` | SUCCESS | — |
| Core CI (.NET 10) | 46150214723 | `41c722333597d6dd28d0ddf062be3e564a48bd47` | SUCCESS | — |
| Domain and Python CI | 46150214725 | `41c722333597d6dd28d0ddf062be3e564a48bd47` | SUCCESS | 43s؛ تأكد اكتمال job وجميع خطوات Domain NUnit وPython regressions بنجاح. |

**ملاحظة غير حاجزة:** أظهرت صفحة تشغيل Domain/Python عشرة تحذيرات XML documentation من نوع Missing XML comment في `DomainEvent.cs` و`Debuff.cs` و`Clock.cs`. لم تفشل هذه التحذيرات التشغيل؛ سُجلت كدين توثيقي منفصل في OPEN_DECISION_REGISTER تحت D-10، ولا تمثل إصلاحاً منفذاً أو مانعاً للـCI.

**نتيجة التحقق:** التشغيلات الثلاثة أعلاه ناجحة على SHA واحد (`41c7223`). وبذلك يوجد تحقق CI ثلاثي مكتمل على كل من SHA `c516431` وSHA `41c7223`. هذا الإثبات خاص بهذين الرأسين ولا ينتقل تلقائياً إلى commits لاحقة.

## 3.2 Verification Trail — تسلسل التحقق

| SHA | Documentation Governance | Core CI | Domain/Python CI | الحكم |
|---|---|---|---|---|
| `c516431712247b2e52661b247b3530b55c031abe` | Run `46147107180` — SUCCESS | Run `46147107214` — SUCCESS | Run `46147107219` — SUCCESS | تحقق ثلاثي مكتمل |
| `41c722333597d6dd28d0ddf062be3e564a48bd47` | Run `46150214763` — SUCCESS | Run `46150214723` — SUCCESS | Run `46150214725` — SUCCESS | تحقق ثلاثي مكتمل |
| `655660d03b772ccb88ce79ac6dc13aa5acf92fa3` | Run `46154452659` — SUCCESS | Run `46154452627` — SUCCESS | لم يُطلب تشغيل/توثيق CI الثلاثي على هذا الرأس ضمن إجراء المتابعة الحالي | Documentation Governance متحقق؛ لا يُدّعى تحقق CI الثلاثي على هذا SHA |

**تفصيل تشغيل Documentation Governance على `655660d`:** workflow `.github/workflows/docs-governance.yml`، Run `46154452659`، event `push`، branch `main`، conclusion `success`، على SHA المطابق تماماً `655660d03b772ccb88ce79ac6dc13aa5acf92fa3`.

هذا الجدول يميز بين تحقق CI الثلاثي على SHAين سابقين وبين تحقق Documentation Governance على `655660d`. أي commit لاحق ينشئ رأساً جديداً ويتطلب إعادة تحقق على ذلك الرأس قبل نسب النتيجة إليه.

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
