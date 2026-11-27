# Threat Model — v2.2

**الحالة: PROPOSED | نموذج لم يُراجع على نظام لعب/خادم منفذ**

النطاق المستهدف: عميل Android، الشبكة، Dedicated Server، Backend إن اعتُمد، ومفاتيح التوقيع. وجود Domain وCore واختبارات CI لا يعني وجود تطبيق عميل أو خادم أو خدمة منشورة.

| التهديد | الضابط المقصود | الحالة الفعلية |
|---|---|---|
| تزوير الصحة والضرر | حساب خادمي موثوق | غير منفذ؛ اختبارات Domain الحالية لا تنشئ خادماً |
| RPC spam | rate limits | غير منفذ |
| Replay | RequestId وidempotency | توجد اختبارات Domain لرفض RequestId المكرر في النقل؛ لا يوجد بروتوكول شبكة/خادم |
| Serialization abuse | schema وحد الحجم | غير منفذ |
| Speed/teleport | تحقق حركة خادمي | غير منفذ |
| Session theft | جلسات قابلة للإبطال | غير منفذ |
| Secret leak | أسرار خارج Git وفحص تاريخ المستودع | Secret scanning وPush protection آخر حالة معروفة لهما active في قراءة 2026-11-24؛ لم تتوفر قراءة أحدث، ولا يوجد تقرير مسح تاريخ متخصص أو gitleaks في Actions |
| Dependency compromise | تثبيت ومراجعة الاعتماديات | إصدارات Actions مثبتة بـcommit SHA؛ آخر قراءة لتنبيهات Dependabot أعادت 403، وتحديثات الأمان الآلية آخر حالة موثقة لها disabled في 2026-11-24؛ لا استنتاج عن خلو التنبيهات |
| CI compromise | أقل صلاحية وعزل المهام | يوجد Domain/Python CI وCore CI بصلاحية `contents: read`؛ CodeQL default setup مهيأ للـC# وPython وActions، ولا يوجد gitleaks أو فحص workflow متخصص |
| Pending-session exhaustion | TTL وauth rate limits | SessionRegistry/ConnectionRegistry bounded، لكن لا pending TTL أو failed-auth rate limit داخل Core؛ host مسؤول | 
| Queue/payload abuse | raw frame quotas + bounded validator | Core queue bounded وpayload validator fail-closed؛ validator يعمل قبل auth ويجب أن يكون bounded، ولا توجد raw frame/serializer quotas | 
| Mutable command payload | immutable DTO/ownership | validator يعيد التحقق بعد dequeue لكنه لا clone/freeze؛ host مسؤول عن immutable DTO أو exclusive ownership | 
| Event/ledger memory growth | retention/caps/persistence | Domain event/request/echo stores وCore BetrayalLedger._events/_scores بلا سياسة retention/cap | 
| Cross-match ledger contamination | per-match scope enforcement | CoreComposition موصوف instance-per-match لكن API لا يربطه بـMatchId؛ host يجب أن يفرض scope |

## وضع CI والأدلة المرتبطة بالالتزام

- ملفات CI المتتبعة تشغّل اختبارات محددة لـDomain/Python وCore، ويوجد تدقيق وثائق مستقل. وجود الملفات لا يثبت نجاح تشغيل حديث.
- أحدث مجموعة نتائج مكتملة موثقة في سجل الأدوات عند إعداد هذه اللقطة تخص SHA `94b341a681930e875fdbeee6ff3ccecb77476a14`، لا HEAD الحالي `0ebb36090f0c1ab254d4402c2a738dd6a7d5b1a5`: Domain NUnit `62/62`، Python simulator `11/11`، workflow hygiene `6/6`، Core NUnit `248/248`، Documentation Governance `success`، وCodeQL للغات C# وPython وGitHub Actions `completed/success`.
- لا تُسقط نتائج `94b341a...` على HEAD الحالي `0ebb360...`؛ يلزم تشغيل أو قراءة check-runs مرتبطة بالـSHA الحالي قبل وصفه بأنه متحقق.
- نجاح الاختبارات يثبت الوظائف التي شُغّلت فقط، ولا يثبت خلو CodeQL alerts أو Dependabot alerts، ولا وجود Unity أو Android build أو خادم/شبكة فعلية.
- قراءة branch-protection بتاريخ 2026-11-27 لم تكن متاحة بسبب `403 Resource not accessible by integration`؛ الحالة `UNKNOWN / NOT VERIFIED`. أعادت قراءة repository rulesets المتاحة `[]`، لكنها لا تحسم قواعد المؤسسة أو أي طبقات حماية خارج نطاق القراءة.
- أعادت واجهات قراءة التنبيهات/الإعدادات ذات الصلة أخطاء صلاحيات أو لم تكن مدعومة في التكامل؛ لا تستنتج خلو المستودع من التنبيهات. Secret scanning وPush protection آخر حالة موثقة لهما هي القراءة المؤرخة 2026-11-24، ولم تتوفر إعادة تأكيد أحدث.

لكل تهديد لاحقاً: Asset وEntry Point وAbuse Case وImpact وControl وTest وEvidence وResidual Risk. لا تُعامل هذه الوثيقة كدليل على أن الضابط موجود.

لكل تهديد لاحقاً: Asset وEntry Point وAbuse Case وImpact وControl وTest وEvidence وResidual Risk. لا تُعامل هذه الوثيقة كدليل على أن الضابط موجود.
