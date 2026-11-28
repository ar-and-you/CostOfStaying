# F-001 — نموذج تهديد أوامر Gameplay والتفويض

> **الحالة:** Draft — مستند إلى تنفيذ المستودع، وليس اعتماداً أمنياً أو قرار منتج.  
> **التاريخ:** 2026-11-28  
> **SHA المرجعي:** `a942cc48cb7373deca076cbfa4e351e2f0c1f36a`

## 1. حدود النظام الحالية

المستودع يحتوي Core pure .NET وDomain gameplay assembly، ولا يحتوي طبقة Transport/Network أو Host runtime موصولة. لذلك فإن Client وHost أدناه جهات تهديد للتكامل المستقبلي، وليسا مكونات منفذة يمكن تتبعها end-to-end.

المسار الآمن المقصود جزئياً:  
`untrusted input → Host authentication/binding → server-created CommandEnvelope<T> → payload validation → session/membership admission → required Gameplay Policy → bounded queue → post-dequeue revalidation → trusted handler → mutation`

هذا المسار غير مفروض على كل مستهلك: توجد admission factories دون Policy، ولا توجد Gameplay Handlers ملموسة، وDomain public APIs لا تستدعي Core authorization تلقائياً.

## 2. الجهات الفاعلة ومستوى الثقة

| الجهة | مدخلاتها/قدراتها | مستوى الثقة | الضابط المطلوب |
|---|---|---|---|
| Client/player client | intents وPlayerId وRequestId وTargetId وChoice وHint/Echo وclaimed distance/time | غير موثوق | لا تُقبل claims كهوية أو وقت أو مسافة أو صلاحية. |
| Host/transport adapter | يصادق credentials ويربط connection بـActorId/SessionId وينشئ envelope | موثوق مشروط بصحة التنفيذ | binding موثق، serializer/frame quotas، rate limits، server clock/position. |
| Core application caller | ينشئ services/policies ويستهلك AuthenticatedCommand | server-owned لكنه ليس آمناً تلقائياً | فرض authorization على كل gameplay ingress ومنع المسارات البديلة. |
| Trusted gameplay handler | ينفذ mutation بعد authorization | موثوق ومحدود الصلاحية | استخدام ActorId المحلول من Core، وعدم الوثوق بهوية payload. |
| Domain/Core library consumer | يستدعي public APIs أو يعدّل collections إذا امتلك المراجع | غير موثوق عند حدود API عامة | ownership وencapsulation وعدم مشاركة mutable references. |
| Server scheduler/internal | يستدعي Tick وعمليات النظام | موثوق ضمن صلاحيات محددة | فصل system authority عن player authority ومصدر وقت monotonic. |

## 3. عمليات تغيير الحالة الموجودة فعلياً

| العملية | التغيير | متطلبات الثقة |
|---|---|---|
| `Match.AddPlayer` / `Start` | roster وlifecycle | Host/administrative authority. |
| `Match.Tick` | vitality/debuff/collapse/death/vote/endgame | server scheduler وdelta مشتق من monotonic clock. |
| `Match.TryTransfer` | debuff وbetrayal/cooldown/events | session + actor binding + membership + policy + target eligibility + server-measured distance + idempotency. |
| `Match.CastVote` | vote state وقد ينهي المباراة | session + actor binding + electorate membership + vote window + enum/duplicate semantics. |
| `Match.CollectEcho` | echo IDs/scores/events | session + actor binding + membership + Spirit state + echo validity. |
| `Match.TrySendHint` | cooldown/events | session + actor binding + membership + Spirit state + validation/rate limit. |
| `IVitalityAccount.Lose/Restore/TryResurrect/TryEliminate` | vitality/player lifecycle | actor ownership + operation-specific authority + state/cost/cooldown. |
| `IMatchLifecycle.TryTransition` | MatchPhase | server authority + transition + product preconditions. |
| `IBetrayalLedger.TryRecord` | betrayal event/score | trusted event source + membership + identity mapping + sequence/replay semantics. |
| `IMatchPlayerRegistry.Register/Remove` | membership | roster authority + session/lifecycle coordination. |

هذه عمليات/واجهات قائمة وليست Command Catalog معتمداً.

## 4. سيناريوهات التهديد

- **T-01 انتحال الهوية:** client يرسل PlayerId/SessionId لشخص آخر. الضابط: host يربط connection بمصادقة فعلية وSessionId خادمي؛ Core يعيد حل ActorId ولا يثق في payload.
- **T-02 لاعب خارج المباراة:** جلسة صحيحة تحاول العمل في match لا ينتمي إليه ActorId. الضابط: membership عند admission وبعد dequeue، ثم تحقق handler من source/target membership.
- **T-03 تجاوز Policy:** استعمال `CreateCommandAdmissionService` أو `CreateMatchCommandAdmissionService` بدلاً من authorization service. الضابط: إثبات أن كل gameplay ingress يمر عبر Policy.
- **T-04 صلاحية متقادمة:** تتغير session/membership/phase/player state أثناء queue. الضابط: إعادة تقييم بعد dequeue وإعادة تحقق handler عند التنفيذ.
- **T-05 payload خبيث أو mutable:** payload غير صالح/ضخم أو يتغير بعد admission. الضابط: quotas في transport، validator bounded/no-I/O، immutable payload أو exclusive ownership، وإعادة validation.
- **T-06 تلاعب بالوقت/المسافة:** client يزوّد ReceivedAt/delta/distance. الضابط: Host يشتق الوقت والمسافة من مصادر authoritative؛ claims للـdiagnostics فقط.
- **T-07 replay/sequence abuse:** إعادة إرسال أو إعادة استخدام sequence/request ID. الضابط: sequence tracker وidempotency semantics؛ لا eviction قبل F-003/D-04.
- **T-08 إساءة استعمال عمليات الإدارة:** client يستدعي Start/Tick/transition أو يغيّر roster. الضابط: فصل host/system authority عن player authority.
- **T-09 Betrayal غير مستحقة:** استدعاء `IBetrayalLedger.TryRecord` دون حدث لعب موثوق. الضابط: trusted handler والتحقق من المصدر/الهدف/العضوية/السلطة وعقد D2؛ يتقاطع مع F-002.
- **T-10 تعديل الحالة خارج Aggregate:** تعديل مجموعات Domain العامة أو استدعاء public APIs مباشرة. الضابط: ownership وD-03 وقيود مراجع Host.

## 5. سيناريوهات الرفض المطلوبة

| الحالة | السلوك المطلوب | طبقة التحقق |
|---|---|---|
| Envelope غير صالح | رفض قبل queue | Core ingress |
| Session غير موجودة أو غير Authenticated | رفض | Session gate |
| ActorId غير صالح أو تغير binding | رفض | Authorization/dequeue |
| Actor ليس عضواً في match | رفض | Match admission |
| Policy null/uninitialized | فشل الإنشاء أو رفض؛ لا default allow | Composition/Policy |
| Policy ترفض أو ترمي استثناء | رفض fail-closed | Authorization |
| Policy تسمح قبل queue ثم ترفض بعدها | إسقاط دون handoff | Authorization |
| Payload validator يرفض أو يرمي | رفض fail-closed | Payload validation |
| Payload تغير بعد admission | رفض عند post-dequeue validation | Payload validation |
| Match phase/player state لا يسمحان | رفض | Gameplay Policy/handler/domain |
| target غير صالح أو غير عضو أو self-target | رفض | Policy/handler/domain |
| الوقت/المسافة غير موثوقين أو خارج الحدود | رفض | Host + policy/domain |
| sequence/replay أو queue capacity غير مقبول | رفض دون تكرار mutation | Queue/idempotency |
| Player command بلا Actor context مصادق عليه | رفض | Handler boundary |
| client يطلب host-only/system operation | رفض | Host authority boundary |

## 6. افتراضات لا تُعامل كحقائق

- لا توجد حالياً طبقة Client/Transport/Host منفذة يمكن تدقيق إعداداتها.
- لا توجد Gameplay Policies أو Handlers ملموسة.
- لا توجد أسماء أوامر أو أدوار منتج معتمدة هنا.
- اختبارات Core generic لا تثبت أن كل Domain mutator محمي.
- تسمية مكوّن ما Host لا تجعله موثوقاً دون authentication/authorization/server authority.
- لا يحدد هذا المستند قواعد لعب جديدة أو قيم مسافة/cooldown؛ يصف التنفيذ الحالي ويترك قرارات المنتج مفتوحة.

## 7. الاعتماديات والبوابات

- **F-001-A:** مراجعة هذا الجرد واستكمال أي ingress/mutation ناقص.
- **F-001-B:** كتالوج Proposed وموافقة مالك المنتج قبل Policies/Handlers.
- **F-001-C:** تنفيذ Policies/Handlers واختبارات الرفض بعد اعتماد الكتالوج.
- **D2:** عقد Domain/Core للهوية والوقت والحالة والأحداث والتسلسل.
- **F-002:** لا يبدأ قبل إغلاق F-001 واعتماد D2.
- **D-03:** تغليف مجموعات Domain العامة.
- **F-003/D-04:** retention وreplay semantics.

**الحكم:** Threat Model Draft؛ لا اعتماد لكتالوج أو Policy أو Handler، ولا إغلاق لـF-001.
