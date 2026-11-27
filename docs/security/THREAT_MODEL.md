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

## وضع CI

- ملفا CI موجودان ويشغّلان اختبارات محددة على GitHub Actions.
- أحدث evidence موثق هنا: على SHA d9c4e93، Domain 62/62 وCore 248/248 وDocumentation Governance success؛ CodeQL Python/Actions success وC# in_progress عند آخر قراءة. هذه النتائج لا تثبت خلو alerts؛ راجع GITHUB_TOOLING.
- قراءة 2026-11-22 التي قالت إن main غير محمي أصبحت تاريخية؛ قراءة branch-protection اللاحقة في 2026-11-26 أعادت 403، لذلك الحالة الحالية UNKNOWN. قراءة rulesets الفارغة لا تحسم كل أشكال الحماية.
- أعادت واجهة قراءة تنبيهات Code Scanning `403 Resource not accessible by integration` في 2026-11-24؛ لم تُثبت هنا حالة أي تنبيه. لا تُعامل نجاح الاختبارات على أنه إثبات لأمن CI أو التطبيق أو الشبكة.

لكل تهديد لاحقاً: Asset وEntry Point وAbuse Case وImpact وControl وTest وEvidence وResidual Risk. لا تُعامل هذه الوثيقة كدليل على أن الضابط موجود.
