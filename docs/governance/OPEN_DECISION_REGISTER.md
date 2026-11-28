# سجل القرارات المفتوحة وبوابات الاعتماد

**تاريخ اللقطة:** 2026-11-28  
**مرجع `main`:** `e7e5b5e54f46490c44aaf30888280d55239bb670`  
**فرع المراجعة الأمني:** `security/core-trust-boundaries-phase0`؛ آخر رأس متحقق هو `ec7312fe043388c935c7fd5f97a3397e7c8b2416` (غير مدمج). نجحت عليه Core CI وDomain/Python CI وDocumentation Governance؛ CodeFactor لا يزال يسجل Minor واحدة على خط الأساس السابق، ولا توجد annotation قابلة للتعيين من تكامل GitHub. نتائج CodeQL/Codacy المذكورة لاحقاً تخص رؤوساً أقدم ولا تُنسب إلى هذا الرأس.  
**الحالة:** سجل تجميعي للتتبع فقط؛ ليس ADR ولا موافقة على أي قرار.

يمنع هذا السجل تحويل المقترحات أو التنفيذ التاريخي أو نتائج المحاكاة إلى سياسة نافذة. الإغلاق يتطلب قرار مالك مكتوباً، مع تحديث المصدر الحاكم والفهرس والأدلة.

## 1. بوابات التنفيذ D0–D9

| البوابة | الحالة | الدليل/المتبقي |
|---|---|---|
| D0 — توثيق مسارات الشبكة والخادم كتوجهات مقترحة | COMPLETE | ROADMAP يصف Unity/network/server كمسارات مستقبلية لا كتنفيذ قائم |
| D1 — مصالحة واعتماد ADR catalogue | OPEN_OWNER_DECISION | لا توجد ADR مقبولة في docs/adr؛ تعارض ADR-005 وفجوة ADR-024 ومرشحات أخرى موثقة في المرحلة 22 |
| D2 — عقد Domain/Core وVitality/lifecycle | OPEN_OWNER_DECISION | مصفوفة توافق المرحلة 58 جاهزة؛ لا mapping معتمد ولا ترحيل |
| D3 — branch protection وسياسة الفروع | BLOCKED_BY_EVIDENCE_AND_OWNER | قراءة 2026-11-22 تاريخية؛ endpoint أعاد 403 في 2026-11-28؛ الحالة UNKNOWN/NOT VERIFIED، ولا تغيير إعدادات دون قرار |
| D4 — قرارات المنتج D-R1..D-R7 | OPEN_OWNER_DECISION | جميع القرارات أدناه مفتوحة |
| D5a — Unity editor/version | OPEN_TECHNICAL_SPIKE | لا Unity project أو prototype؛ لا اعتماد إصدار نهائي |
| D5b — transport/network/authentication boundary | OPEN_TECHNICAL_SPIKE | لا adapter أو credential provider؛ host يجب أن يصادق credentials ويربط account→ActorId، ويضع Pending TTL/rate limits ويغلق الجلسات stale |

| D6 — hosting/operations | OPEN_OWNER_DECISION | لا اختيار مزود أو نموذج نشر أو server runtime |
| D7 — consent/privacy/retention protocol | OPEN_OWNER_DECISION | لا بروتوكول موافقة/احتفاظ/حذف معتمد لجلسات بشرية |
| D8 — 10 جلسات بشرية وقرار القواعد/التوازن | BLOCKED_ON_HUMAN_PLAYTEST | سجلات المحاكاة العشرة لا تحتسب جلسات بشرية؛ راجع PLAYTEST_PLAN والمرحلة 24 |
| D9 — pre-production exit criteria | OPEN | لا توجد حزمة قبول نهائية تثبت جاهزية الإنتاج |

تنبيه المرحلة 68: payload validator ينفذ قبل auth admission، لذا يجب أن يبقى bounded/synchronous/no-I/O؛ raw frame/serializer quotas وموثوقية ReceivedAt/ServerTick مسؤولية host قبل استدعاء Core.

## 2. قرارات المنتج D-R1–D-R7

كلها OPEN حتى اعتماد مالك مكتوب:

| الرمز | القرار المطلوب |
|---|---|
| D-R1 | شرط الفوز والنهاية: LastSurvivor أو TimeLimit أو مزيج محدد |
| D-R2 | حالات اللاعب ودلالات الموت/الإقصاء/Spirit ودورة الحياة |
| D-R3 | الانقطاع وإعادة الاتصال وأثرهما على التصويت والمباراة؛ Core يتطلب SessionId جديداً بعد Disconnected لكنه لا يقرر استعادة Match/roster أو multi-session policy |
| D-R4 | قواعد التصويت والإجماع والمهلة والنتائج الحدية |
| D-R5 | قيم التوازن والاقتصاد ومعدلات/مدد الأنظمة |
| D-R6 | اقتصاد Spirit وتفاعلاته وحدود تأثيره |
| D-R7 | منظور اللاعب/الكاميرا والعرض وتجربة المعلومات |

لا تعتمد هذه القرارات من ADR أرشيفي أو قيمة محاكاة أو سلوك تنفيذ غير موثق كقرار منتج.

## 3. عيوب Domain الهندسية

| الرمز | الحالة | الملخص |
|---|---|---|
| D-01 | FIXED_IN_LIVE | احتساب Debuff حسب مدة نشاطه داخل Tick طويل |
| D-02 | FIXED_IN_LIVE | رفض Debuff/config للقيم غير الصالحة |
| D-03 | OPEN_OWNER_DECISION | Match.Players/Votes/Events/ProcessedRequestIds وPlayer.Debuffs/CollectedEchoIds مكشوفة كـmutable collections؛ التعديل المباشر قد يتجاوز roster/vote/dedupe/debuff/event invariants. جرد الآثار وخطة encapsulation والتوافق في STATE_RETENTION_INVENTORY.md؛ لا تغيير API حتى قرار المالك |
| D-04 | OPEN_OWNER_DECISION | Domain Events/ProcessedRequestIds/CollectedEchoIds وCore BetrayalLedger._events/_scores وبعض النصوص بلا retention/size policy؛ لا persistence/snapshot/restore أو replay عبر restart. راجع `docs/security/STATE_RETENTION_INVENTORY.md` و`docs/architecture/STATE_OWNERSHIP_AND_RECOVERY.md`. Core registries والـqueues bounded لكل instance بسعات يحددها host، لكن لا global match cap |
| D-05 | OPEN_OWNER_DECISION | أثر موت/انقطاع اللاعب بعد فتح التصويت |
| D-06 | OPEN_OWNER_DECISION | سياسة Collapse عند تأخر Tick: skip أم bounded catch-up أم replay |
| D-07 | OPEN_OWNER_DECISION | حد Domain/Core وخرائط الأنواع والهويات والوقت |
| D-08 | VERIFIED_AT_SHA | RequestId null/empty/whitespace مرفوض؛ Domain CI نجح 62/62 على SHA 94b341a681930e875fdbeee6ff3ccecb77476a14 (run 45919285815). لا يعني ذلك persistence أو concurrent replay guarantees |

D-08 موثق على SHA 94b341a681930e875fdbeee6ff3ccecb77476a14؛ تشغيل Domain CI على هذا الرأس أعاد نجاح 62/62، لذلك جُدد الدليل دون توسيع نطاق الضمان إلى persistence أو concurrency. D-01/D-02 مثبتان باختبارات الانحدار وضمن تشغيل Domain CI نفسه.

## 4. تغطية عقود Core في ADR الأرشيفية 008–020

المرحلة 60 قارنت نصوص الأرشيف مع Core الحي. هذا تقاطع تنفيذ/اختبارات فقط، ولا يحول ADR الأرشيفية إلى قرارات مقبولة على main:

| ADR | وضع Core الحالي | الفجوة المتبقية |
|---|---|---|
| 008 Command envelope/sequence | Implemented + NUnit | التسلسل per-session وفي الذاكرة؛ admission policy ليست اعتماداً منتجياً |
| 009 Match player registry | Implemented + NUnit | membership فقط؛ لا يقرر قواعد lifecycle أو multi-session |
| 010 Session lifecycle | Implemented + NUnit | reconnect policy تحتاج قراراً؛ Disconnected لا يعاد تفعيله |
| 011 Bounded session registry | Implemented + NUnit | سعة التشغيل وسياسة تعدد الجلسات لكل Actor مفتوحة |
| 012 Authenticated command admission | Implemented + NUnit | يعتمد على actor resolved من session؛ لا credential provider |
| 013 Safe session cleanup | Implemented + NUnit | queue/replay cleanup؛ لا persistence |
| 014 Victim scope | VictimId type only | أنواع الضحايا المسموحة ما زالت TBD |
| 015 Command identity/idempotency | Partial | لا CommandId مستقل ولا result store دائم أو idempotency عبر reconnect/restart |
| 016 Connection/session binding | Implemented + NUnit | transport adapter غير موجود؛ mapping in-memory فقط |
| 017 Credential validation/actor binding | Boundary only | لا identity provider أو credential validator أو account-to-ActorId mapping |
| 018 Match roster admission | Implemented + NUnit | لا policy تعدد الجلسات ولا transport routing |
| 019 Match authorization boundary | Generic fail-closed service implemented | لا command-specific gameplay policies معتمدة |
| 020 Typed payload validation | Generic validator gate implemented | لا concrete gameplay DTOs ولا raw frame/serializer quotas داخل Core |

**حالة D1:** OPEN_OWNER_DECISION. بعض نسخ ADR-008..014 تصف APPROVED بموجب ADR-027 داخل الأرشيف، لكن لا توجد سجلات ADR مقبولة في docs/adr على main. لا تُعامل هذه الحالة الأرشيفية كاعتماد نافذ.

## 5. عقد Queue admission وتنظيف Session

### ملحق سعة Core (المرحلة 64)

- SessionRegistry وMatchPlayerRegistry وConnectionSessionRegistry وCommandAdmissionQueue وCommandSequenceTracker محدودة بسعات صريحة من host.
- BetrayalLedger._events غير محدود، و_scores لا يملك سقفاً مستقلاً؛ exact replay behavior يعتمد على سجل الأحداث.
- لا global active-match cap في CoreComposition، ولا payload size limit داخل TCommand. على فرع hardening الحالي يتطلب `Create(MatchId)` معرفاً صالحاً؛ MatchLifecycle وMatchPlayerRegistry وVitalityAccount وCommandAdmissionQueue والـmatch service factories مربوطة بالـscope نفسه، مع رفض تسجيل حساب أو queue من MatchId آخر؛ اجتاز Core CI عدد 257/257 اختباراً على الرأس الحالي `d8c66e85d2d4b8353595b24dd2df38382ead2adb` (run `46131891394`)، ولم يُدمج التغيير في main.
- يلزم host roster/authority validation قبل BetrayalLedger.TryRecord، مع سياسة event retention وmatch concurrency قبل الإنتاج.

مراجعة المرحلة 61 أكدت سلوك التنفيذ والاختبارات التالية:

- CommandAdmissionQueue يفحص صلاحية envelope ثم queue capacity قبل استهلاك sequence؛ FullQueue_RejectsWithoutConsumingSequence يثبت أن نفس sequence يمكن إعادة محاولته بعد زوال backpressure.
- بعد نجاح admission، يستهلك sequence حتى لو رفضت طبقة gameplay الأمر لاحقاً؛ هذا يحمي replay ولا يعني نجاح gameplay command.
- ResetSession يرفض إزالة replay state طالما يوجد command من الجلسة في queue.
- SessionCleanupService يشترط SessionState.Closed، ويستدعي ResetSession، ثم يزيل الجلسة؛ التعليق يلزم host بانتظار اكتمال أي handler سبق dequeue.
- ConnectionAwareSessionCleanupService يمنع التنظيف ما دام session مربوطاً باتصال، ويجب حقنه بنفس ConnectionSessionRegistry الذي تستخدمه ingress.
- الاختبارات تغطي queue full/duplicate/jump، reset مع pending، cleanup قبل/بعد drain، connection binding/detach، وإعادة استخدام سعة tracked sessions.
- هذه الخدمات single-writer/server-serialized؛ لا تدعي thread safety أو persistence أو distributed cleanup.
- MatchCommandPayloadValidationService يعيد التحقق قبل handler handoff لكنه لا ينسخ أو يجمد TCommand؛ يجب أن تكون DTOs immutable أو تنتقل ملكيتها حصرياً إلى host ولا تتغير بعد handoff. حدود frame/serializer تُفرض قبل deserialization.

لا يحتاج هذا الجزء تغييراً حالياً. يبقى transport ingress/egress وin-flight command coordinator وdurable replay store مسؤوليات host مستقبلي.

## 6. فجوات الأدلة الخارجية

- Branch protection: UNKNOWN بسبب 403؛ rulesets الفارغة في القراءة المتاحة لا تحسم جميع أشكال الحماية.
- Dependabot alerts وCode Scanning alerts: لم تتوفر قراءة موثوقة بسبب 403؛ لا تسجلها على أنها خالية.
- Secret scanning/Push protection: آخر حالة موثقة تعود إلى 2026-11-24؛ تحتاج قراءة حديثة.
- CodeQL: سجل التشغيلات يثبت بعض التحليلات على SHAs محددة؛ لا يثبت خلو التنبيهات.
- Unity/Android/server/network: لا runtime أو build فعلي في الشجرة.
- Human playtest: لم تُسجل جلسات بشرية معتمدة؛ بيانات المحاكاة ليست بديلاً.

## 7. قاعدة الإغلاق

1. لا إغلاق بوابة بعبارة أو تعليق داخل كود فقط؛ يلزم مصدر حاكم وقرار موثق عند الحاجة.
2. لا تغيير branch protection أو required checks أو push policy دون D3.
3. لا تغيير public Domain API أو retention/caps دون قرار D-03/D-04.
4. لا تغيير قواعد التصويت أو catch-up دون D-R3/D-R4/D-06.
5. لا دمج Domain/Core دون D1/D2/D-07.
6. لا جمع playtest بشري دون D7، ولا اعتماد التوازن دون D8.

**مصادر التفاصيل:** ADVISORY_PLAN_2026-11-26.md، DOMAIN_DEFECT_REVIEW_2026-11-25.md، STATUS_AND_EVIDENCE.md، DOMAIN_CORE.md، PLAYTEST_PLAN.md، وتقارير المراحل 22 و24 و54–58.


## 8. قرارات وحدود العمل الأمني — 2026-11-27

هذه الإضافة تسجل توجيهات مالك المشروع وخطة التحقق؛ ولا تعتبر Findings مغلقة أو ADR مقبولاً.

| الرمز | القرار/التوجيه | الحالة |
|---|---|---|
| SEC-01 | كل أمر حساس يحتاج authorization policy صريحة؛ لا permissive default. غياب الربط يجب أن يفشل عند composition/startup قبل قبول الأوامر. | OWNER_APPROVED_DIRECTION |
| SEC-02 | لا مزود خارجي أو تكامل هوية/تخزين قبل إغلاق F-001 وF-002 بأدلة واختبارات. | OWNER_APPROVED_GATE |
| SEC-03 | تسجيل Betrayal يجب أن يصدر من Gameplay handler موثوق بعد التحقق من هوية Actor وVictim وعضويتهما وسلطة الفعل. | OWNER_APPROVED_DIRECTION; IMPLEMENTATION_OPEN |
| SEC-04 | عزل MatchId يجب أن يكون invariant بنيوياً، بعد جرد موارد Core وتصنيف Match/Host/Session scope. | OWNER_APPROVED_DIRECTION; IMPLEMENTED_AND_TESTED_ON_REVIEW_BRANCH; MAIN_MERGE_PENDING |
| SEC-05 | لا retention cap أو eviction أو snapshot policy قبل تحديد عمر المباراة، الاستئناف، عدد اللاعبين، ومعدل الأحداث. | OWNER_APPROVED_CONSTRAINT |
| SEC-06 | لا اعتماد مزود خارجي ولا إضافة عقود IPlayerAuth/ISaveStore قبل إثبات الحاجة بعد إغلاق مراحل Core. | OWNER_APPROVED_GATE |

### قرارات مالك لا تزال مطلوبة

لم يقدم مالك المشروع بعد سيناريو دورة حياة مباراة معتمداً. تبقى القيم التالية **UNKNOWN / OPEN_OWNER_DECISION** ولا يجوز اختلاقها أو استنتاجها من قيم الاختبار:

- الحد الأدنى/الأقصى للاعبين في المباراة.
- مدة المباراة المتوقعة والحد الأقصى التشغيلي.
- هل يمكن استئناف المباراة بعد disconnect؟ وما نافذة الاستئناف؟
- هل تستمر المباراة بعد إعادة تشغيل المضيف؟ وإذا نعم، ما RPO/RTO المطلوب؟
- معدل/عدد أحداث Betrayal المتوقع في مباراة واحدة.
- ما البيانات التي يجب الاحتفاظ بها بعد انتهاء المباراة ومدة الاحتفاظ.

بناءً على ذلك، لا يمكن إعلان المرحلة 0 مكتملة بالكامل: تم تثبيت حدود الثقة والبوابات الأمنية، لكن سيناريو المنتج اللازم لحسم F-003 ما زال قرار مالك مفتوحاً.

### حالة F-001 وF-002 عند بدء التنفيذ

- F-001: Core يفرض policy غير null لكل `MatchCommandAuthorizationService<TCommand>` instance ويعيد التقييم قبل admission وبعد dequeue، كما توجد اختبارات رفض uninitialized/exception. في فرع المراجعة أصبحت مصانع admission-only داخلية، واختبار CoreComposition يتحقق من عدم ظهورها في public API. لا توجد concrete Gameplay command DTOs أو policies إنتاجية أو registry شامل يثبت اكتمال التغطية، ولا يمكن التحقق من wrong/permissive policy دون مواصفة لكل أمر. لذلك الحالة HARDENED_FAIL_CLOSED_AT_PUBLIC_INGRESS / NOT_CLOSED.
- F-002: `BetrayalLedger` يتحقق من بنية الحدث والتسلسل وexact replay، لكنه لا يتحقق من membership أو gameplay authority. `CoreComposition.BetrayalLedger` يعرض واجهة قراءة عامة فقط؛ `TryRecord` أصبح internal لمنع التسجيل من مستهلك خارجي أو عبر cast. لا يوجد بعد Trusted Gameplay Handler أو membership/authority checks. توجد فجوتا هوية غير محلولتين: Domain `Player.Id` من نوع `Guid` مقابل Core `ActorId` من نوع `ulong`، وCore `VictimId` من نوع مستقل لا يملك resolver إلى ActorId canonical. لا يجوز افتراض mapping عبر cast أو string أو مساواة القيم الرقمية. راجع `docs/architecture/DOMAIN_CORE.md` و`docs/security/SENSITIVE_COMMAND_POLICY_CATALOG.md`. لذلك تبقى F-002 مفتوحة.
- F-004: على فرع العمل أصبح `CoreComposition.Create(MatchId)` إلزامياً، كما أصبحت VitalityAccount وCommandAdmissionQueue مرتبطة بـMatchId وتُرفض مشاركة الحساب/الطابور عبر المباريات. اجتاز Core CI عدد 257/257 اختباراً على الرأس `d8c66e85d2d4b8353595b24dd2df38382ead2adb`؛ الحالة IMPLEMENTED_AND_TESTED_ON_REVIEW_BRANCH، والدمج في `main` ما زال معلقاً.
- على الرأس `d8c66e85d2d4b8353595b24dd2df38382ead2adb` (خط أساس سابق): Core CI run `46131891394` نجح (257/257 NUnit)، وDomain/Python CI run `46131891429` نجح (Domain NUnit 62/62، simulator 11، CLI 2/2، workflow hygiene 7، actionlint وzizmor، smoke test)، وDocumentation Governance run `46131891367` نجح. CodeQL C#/Python/Actions اكتمل بنجاح، وCodacy check `140940911744` أعلن 0 issues. CodeFactor check `140938675639` فشل مع Minor واحدة؛ تفاصيل annotation غير متاحة، لذلك لم تُنسب إلى ملف بالتخمين ولم يُنفذ autofix أعمى. هذه النتائج مرتبطة بالرأس المذكور فقط.
- **آخر رأس موثق ومتحقق:** `ec7312fe043388c935c7fd5f97a3397e7c8b2416`. نجحت عليه Documentation Governance run `46134938192`، وCore CI run `46134938176` (257/257 NUnit)، وDomain and Python CI run `46134938156` (Domain NUnit 62/62، و11 اختبار simulator، وactionlint v1.7.12، وzizmor v1.33.1، وworkflow hygiene، وPlaytest CLI وsmoke test). لم تُستخرج من هذا التشغيل نتيجة CodeQL أو Codacy أو CodeFactor جديدة؛ تبقى نتائجها السابقة مقيدة برؤوسها. جميع تغييرات الفرع، بما فيها F-004، لم تُدمج في `main`.
