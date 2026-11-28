# سجل القرارات المفتوحة وبوابات الاعتماد

**تاريخ اللقطة:** 2026-11-27  
**المرجع:** main  
**الحالة:** سجل تجميعي للتتبع فقط؛ ليس ADR ولا موافقة على أي قرار.

يمنع هذا السجل تحويل المقترحات أو التنفيذ التاريخي أو نتائج المحاكاة إلى سياسة نافذة. الإغلاق يتطلب قرار مالك مكتوباً، مع تحديث المصدر الحاكم والفهرس والأدلة.

## 1. بوابات التنفيذ D0–D9

| البوابة | الحالة | الدليل/المتبقي |
|---|---|---|
| D0 — توثيق مسارات الشبكة والخادم كتوجهات مقترحة | COMPLETE | ROADMAP يصف Unity/network/server كمسارات مستقبلية لا كتنفيذ قائم |
| D1 — مصالحة واعتماد ADR catalogue | OPEN_OWNER_DECISION | لا توجد ADR مقبولة في docs/adr؛ تعارض ADR-005 وفجوة ADR-024 ومرشحات أخرى موثقة في المرحلة 22 |
| D2 — عقد Domain/Core وVitality/lifecycle | OPEN_OWNER_DECISION | مصفوفة توافق المرحلة 58 جاهزة؛ لا mapping معتمد ولا ترحيل |
| D3 — branch protection وسياسة الفروع | BLOCKED_BY_EVIDENCE_AND_OWNER | قراءة 2026-11-22 تاريخية؛ endpoint أعاد 403 في 2026-11-26؛ الحالة الحالية UNKNOWN، ولا تغيير إعدادات دون قرار |
| D4 — قرارات المنتج D-R1..D-R7 | OPEN_OWNER_DECISION | جميع القرارات أدناه مفتوحة |
| D5a — Unity editor/version | OPEN_TECHNICAL_SPIKE | لا Unity project أو prototype؛ لا اعتماد إصدار نهائي |
| D5b — transport/network/authentication boundary | OPEN_TECHNICAL_SPIKE | لا adapter أو credential provider؛ host يجب أن يصادق credentials ويربط account→ActorId، ويضع Pending TTL/rate limits ويغلق الجلسات stale |

تنبيه المرحلة 68: payload validator ينفذ قبل auth admission، لذا يجب أن يبقى bounded/synchronous/no-I/O؛ raw frame/serializer quotas وموثوقية ReceivedAt/ServerTick مسؤولية host قبل استدعاء Core.
| D6 — hosting/operations | OPEN_OWNER_DECISION | لا اختيار مزود أو نموذج نشر أو server runtime |
| D7 — consent/privacy/retention protocol | OPEN_OWNER_DECISION | لا بروتوكول موافقة/احتفاظ/حذف معتمد لجلسات بشرية |
| D8 — 10 جلسات بشرية وقرار القواعد/التوازن | BLOCKED_ON_HUMAN_PLAYTEST | سجلات المحاكاة العشرة لا تحتسب جلسات بشرية؛ راجع PLAYTEST_PLAN والمرحلة 24 |
| D9 — pre-production exit criteria | OPEN | لا توجد حزمة قبول نهائية تثبت جاهزية الإنتاج |

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
| D-03 | OPEN_OWNER_DECISION | مجموعات Match/Player العامة قابلة للتعديل؛ يلزم API encapsulation وخطة توافق |
| D-04 | OPEN_OWNER_DECISION | Domain Events/RequestIds/EchoIds وCore BetrayalLedger._events/_scores ومدخلات النص/payload بلا سياسة retention/size أو global match cap |
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
- لا global active-match cap في CoreComposition، ولا payload size limit داخل TCommand. Create() لا يستقبل MatchId ولا يفرض instance-per-match؛ composition واحد قد يشارك BetrayalLedger بين عدة MatchIds إذا أسيء استخدامه.
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


## 8. متابعة حزمة التوثيق والأمن — 2026-11-28

هذه البنود إضافات تتبعية ولا تستبدل القرارات أو البوابات التاريخية أعلاه. يتقاطع F-003 مع D-04 القائم؛ يبقى D-04 مصدر القرار الأصلي للاحتفاظ، بينما يربط F-003 الجرد التشغيلي وخطة المتابعة.

| ID | الموضوع | المالك | تاريخ الاستحقاق | الحالة | مرجع |
|---|---|---|---|---|---|
| F-001 | استكمال Gameplay Authorization: concrete Commands وPolicies وHandlers واختبارات الرفض لكل أمر معتمد | مالك المنتج/المعمارية | TBD | OPEN | تقرير المرحلة 78؛ `docs/security/TRUST_BOUNDARIES.md` |
| F-002 | Trusted Gameplay Handler لتسجيل Betrayal، مع عقد معتمد لـActorId↔Player.Id وهوية Match | مالك المنتج/المعمارية | TBD | OPEN | تقرير المرحلة 78؛ `docs/security/TRUST_BOUNDARIES.md` |
| F-003 | قرار retention لكل مخزن وربطه بدورة حياة المباراة وإعادة التشغيل وإعادة التشغيل الحرفي؛ مكمل لـD-04 وليس بديلاً عنه | مالك المنتج/التشغيل | TBD | OPEN | D-04؛ `docs/security/GIT_HISTORY_SCAN_PLAN.md` (للفحص التاريخي فقط) |
| DOC-01 | تسجيل وثيقة المرحلة 78 في فهرس docs وإزالة orphan الذي تسبب في فشل Docs Governance | مالك الحوكمة | 2026-11-28 | RESOLVED | Docs Governance run 46146903123: success على SHA cfa54fcdb415fdd2e4b2298d8ae56265d926bc26 |
| DOC-02 | إنشاء بيان متابعة جديد ببصمات فعلية، دون تعديل MANIFEST.json أو SHA256SUMS.txt التاريخيين | مالك الحوكمة | 2026-11-28 | RESOLVED | `docs/operations/FOLLOWUP_MANIFEST_2026-11-28.md`؛ بصمات SHA-256 مسجلة، والملف التاريخي لم يُمسّ |
| GOV-01 | حسم التعارض الظاهري بين سياسة Direct-to-main ووجود PR #2؛ السياسة الحالية لا تتغير بهذه الإضافة | مالك المشروع | TBD | OPEN | `docs/governance/GIT_POLICY_RESOLUTION.md` |

### D-10 — تحذيرات XML documentation في Domain

| الرمز | الحالة | المالك | الاستحقاق | الوصف والدليل |
|---|---|---|---|---|
| D-10 | OPEN | مالك المعمارية | TBD | عشرة تحذيرات Missing XML comment ظهرت في Domain CI على SHA `41c722333597d6dd28d0ddf062be3e564a48bd47`، في `DomainEvent.cs` و`Debuff.cs` و`Clock.cs`. التشغيل `46150214725` انتهى SUCCESS؛ التحذيرات غير حاجزة وليست دليلاً على فشل الاختبارات. يُحدد المالك لاحقاً ما إذا كانت ستُعالج ومتى، دون تحويلها إلى شرط CI أو إصلاح مفترض قبل قرار. |

### قاعدة التحديث

لا تُغلق البنود عند إنشاء الوثائق. يتطلب الإغلاق أدلة مرتبطة بـSHA، ونتائج تشغيل فعلية عند انطباقها، وقرار مالك لأي مسألة منتج/سياسة.
