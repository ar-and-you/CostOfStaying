# F-001 — جرد أدلة أوامر Gameplay والتفويض

> **الحالة:** Draft — جرد تقني للقراءة فقط، وليس اعتماداً لسياسة Gameplay.  
> **تاريخ الجرد:** 2026-11-28  
> **SHA المرجعي:** `a942cc48cb7373deca076cbfa4e351e2f0c1f36a`  
> **النطاق:** Core وDomain والاختبارات ذات الصلة. لا يشمل PR #2 غير المدمج.

## 1. الخلاصة

يوفر Core بنية عامة للأوامر `TCommand`: envelope وqueue وsession admission وmatch membership وauthorization policy وpayload validation. لكنه لا يحتوي Gameplay Commands ملموسة أو تطبيقات Gameplay Policy أو Handlers مكتملة.

يوجد فرق مهم بين وجود `MatchCommandAuthorizationService<TCommand>` وبين فرض استعماله: `CoreComposition` يتيح إنشاء `CommandAdmissionService` أو `MatchCommandAdmissionService` دون Policy، كما يتيح إنشاء مسار authorization مع Policy مطلوبة صراحةً. لذلك لا يثبت وجود الخدمة أن كل ingress محمي.

Domain يحتوي عمليات لعب فعلية على `Match`، لكنه لا ينفذ مصادقة جلسة أو تفويض Actor. كما أن مجموعات الحالة العامة قابلة للتعديل مباشرة.

## 2. المنهج والحدود

- فُحصت شجرة Git recursive على SHA المرجعي وملفات Core Contracts/Domain/Services وDomain Match/Player/Intent والاختبارات ذات الصلة.
- هذا جرد لمحتوى المستودع؛ لا يثبت غياب تكاملات runtime خارج المستودع أو كل مسار انعكاسي.
- لا يُعد النوع العام `CommandEnvelope<TCommand>` أو `AuthenticatedCommand<TCommand>` Gameplay Command ملموساً.
- لم تُعدّل ملفات المصدر أو الاختبارات أو إعدادات التشغيل.

## 3. أنواع الأوامر والرسائل

| النوع | المسار | التصنيف | Gameplay Command ملموس؟ |
|---|---|---|---|
| `CommandEnvelope<TCommand>` | `src/CostOfStaying.Core/Domain/CommandEnvelope.cs` | server-side envelope عام | لا |
| `AuthenticatedCommand<TCommand>` | `src/CostOfStaying.Core/Domain/AuthenticatedCommand.cs` | نتيجة handoff بهوية محلولة | لا |
| `ClientIntent` | `src/CostOfStaying.Domain/Intent.cs` | DTO نية عميل مقترح | لا |
| `TransferIntent` | `src/CostOfStaying.Domain/Intent.cs` | DTO يحمل TargetId وclaimed distance | لا |
| `VoteIntent` | `src/CostOfStaying.Domain/Intent.cs` | DTO يحمل VoteChoice | لا |
| `HintIntent` | `src/CostOfStaying.Domain/Intent.cs` | DTO يحمل HintText | لا |
| `CollectEchoIntent` | `src/CostOfStaying.Domain/Intent.cs` | DTO يحمل EchoId | لا |

لا توجد أنواع Gameplay Commands ملموسة مثل Transfer/Vote/RecordBetrayal command في شجرة المصدر الحالية. DTOs في Domain موصوفة بأنها مدخلات غير موثوقة؛ PlayerId ادعاء عميل، وClaimedDistanceMeters تشخيصي فقط.

## 4. سياسات التفويض والخدمات

| المكوّن | المسار | الموجود فعلاً |
|---|---|---|
| `IMatchCommandAuthorizationPolicy<TCommand>` | `Core/Contracts/IMatchCommandAuthorizationPolicy.cs` | عقد Policy يورّده التطبيق؛ لا تطبيق Gameplay ملموس في الشجرة. |
| `MatchCommandAuthorizationService<TCommand>` | `Core/Services/MatchCommandAuthorizationService.cs` | تنفيذ generic يفرض Policy غير null، ويتحقق من الجلسة والهوية والعضوية وحالة اللاعب، ويعيد تقييم Policy قبل admission وبعد dequeue. |
| `ICommandPayloadValidator<TCommand>` | `Core/Contracts/ICommandPayloadValidator.cs` | عقد Validator يورّده التطبيق؛ لا validator Gameplay ملموس. |
| `MatchCommandPayloadValidationService<TCommand>` | `Core/Services/MatchCommandPayloadValidationService.cs` | wrapper اختياري للتحقق قبل admission وبعد dequeue؛ يرفض fail-closed عند الاستثناء. |
| Gameplay Policies | — | لا توجد تطبيقات ملموسة. |
| Gameplay Handlers | — | لا توجد تطبيقات ملموسة. |

### 4.1 سلوك authorization

الخدمة generic تتحقق من envelope، وتعيد حل SessionId، وتتأكد من حالة Authenticated وActorId صالح، وتتحقق من وجود حساب اللاعب في match roster ومطابقة PlayerId، ثم تقيّم Policy قبل queue وبعد dequeue. الاستثناء يتحول إلى `Unavailable` و`Uninitialized` إلى `InvalidState`. هذه ضمانات عند استخدام الخدمة، وليست ضماناً بأن كل ingress يستخدمها.

## 5. Admission factories

المسار: `src/CostOfStaying.Core/CoreComposition.cs`.

| المصنع | المسار الناتج | Gameplay Policy؟ |
|---|---|---|
| `CreateCommandAdmissionService<TCommand>` | `CommandAdmissionService<TCommand>` | لا؛ session admission فقط. |
| `CreateMatchCommandAdmissionService<TCommand>` | `MatchCommandAdmissionService<TCommand>` | لا؛ يضيف match membership فقط. |
| `CreateMatchCommandAuthorizationService<TCommand>` | Match admission + `MatchCommandAuthorizationService<TCommand>` | نعم؛ يتطلب Policy صراحةً. |
| `CreateMatchCommandPayloadValidationService<TCommand>` | Payload validation wrapper فوق authorization | Validator مطلوب؛ الإنشاء اختياري. |

المكوّنات ذات الصلة: `ICommandAdmissionService<TCommand>`، `IMatchCommandAdmissionService<TCommand>`، `CommandAdmissionQueue<TCommand>`، `CommandAdmissionService<TCommand>`، `MatchCommandAdmissionService<TCommand>`. Admission/عضوية المباراة لا تعادل Gameplay authorization. أول مصنعين مرشحا bypass إذا استُخدما في gameplay ingress.

## 6. نقاط تغيير الحالة في Core

| النقطة | المسار | API mutating | حدود التفويض |
|---|---|---|---|
| Vitality/player lifecycle | `Core/Domain/VitalityAccount.cs`; `Core/Contracts/IVitalityAccount.cs` | `Lose`, `Restore`, `TryResurrect`, `TryEliminate` | primitive لا ينفذ authorization؛ التعليق يضعه في طبقة التطبيق. |
| Match lifecycle | `Core/Domain/MatchLifecycle.cs`; `Core/Contracts/IMatchLifecycle.cs` | `TryTransition` | يفرض state transitions فقط؛ لا يثبت سلطة المستدعي أو preconditions المنتج. |
| Match membership | `Core/Domain/MatchPlayerRegistry.cs`; `Core/Contracts/IMatchPlayerRegistry.cs` | `Register`, `Remove` | تغيير عضوية، لا gameplay authorization. |
| Session lifecycle | `Core/Domain/SessionLifecycle.cs` | `TryAuthenticate`, `TryDisconnect`, `TryClose` | lifecycle فقط؛ لا credentials أو transport binding. |
| Betrayal ledger | `Core/Domain/BetrayalLedger.cs`; `Core/Contracts/IBetrayalLedger.cs` | `TryRecord` | الواجهة عامة و`CoreComposition.BetrayalLedger` مكشوفة؛ لا Trusted Handler يثبت مصدر الحدث. |

## 7. نقاط تغيير الحالة في Domain

المسارات: `src/CostOfStaying.Domain/Match.cs` و`Player.cs`.

| العملية | API | أثر الحالة | تحقق Domain | فجوة التفويض |
|---|---|---|---|---|
| إضافة لاعب | `AddPlayer(Guid)` | roster وPlayer وحالة المباراة | حالة/تكرار/سعة | لا مصادقة أو سلطة Host. |
| بدء المباراة | `Start()` | lifecycle وحالة اللاعبين ومواعيد collapse | حالة ووجود لاعب | لا يتحقق من سلطة المستدعي. |
| تقدم المحاكاة | `Tick(double)` | vitality/debuff/death/collapse/vote/endgame | حالة وdelta finite موجب | مصدر الوقت مسؤولية Host. |
| نقل المعاناة | `TryTransfer(Guid, Guid, double, string)` | debuff وbetrayal/cooldown/events | عضوية داخل aggregate، alive، self-target، range، cooldown، capacity، request ID | المسافة يوردها المستدعي؛ لا session/actor auth. |
| التصويت | `CastVote(Guid, VoteChoice)` | Votes وقد ينهي المباراة | VoteOpen/electorate/alive/deadline/enum | لا ربط PlayerId بجلسة مصادق عليها. |
| جمع Echo | `CollectEcho(Guid, string)` | echo IDs/scores/events | gameplay active وSpirit وعدم التكرار | لا مصادقة أو سلطة. |
| إرسال Hint | `TrySendHint(Guid, string)` | cooldown/events | active وSpirit والنص وcooldown | لا مصادقة أو سلطة. |

### 7.1 المجموعات العامة القابلة للتعديل

- `Match.Players` — Dictionary عام.
- `Match.Events` — List عامة.
- `Match.ProcessedRequestIds` — HashSet عام.
- `Match.Votes` — Dictionary عام.
- `Player.Debuffs` — List عامة.
- `Player.CollectedEchoIds` — HashSet عامة.

هذه مرشحات bypass عند مشاركة مراجع Aggregate/Player مع مستهلك غير موثوق، وتتقاطع مع D-03. لا تعني أن العميل الشبكي يصل إليها حالياً؛ لا يوجد transport runtime موصول في الشجرة.

## 8. الاختبارات الموجودة

Core:
- `CommandEnvelopeTests.cs`
- `CommandAdmissionQueueTests.cs`
- `CommandAdmissionServiceTests.cs`
- `MatchCommandAdmissionServiceTests.cs`
- `MatchCommandAuthorizationServiceTests.cs`
- `MatchCommandPayloadValidationServiceTests.cs`
- `CommandIngressPipelineIntegrationTests.cs`
- `CoreCompositionTests.cs`
- `BetrayalLedgerTests.cs`

Domain:
- `TransferTests.cs`
- `VoteAndEndgameTests.cs`
- `SpiritTests.cs`
- `CollapseTests.cs`
- `VitalityTests.cs`
- `SecurityTests.cs`

وجود هذه الاختبارات يثبت سلوك الوحدات التي تغطيها، ولا يثبت وجود Gameplay Handlers أو عدم وجود bypass شامل.

## 9. Bypass candidates — الربط مع Threat Model

كل BC أدناه مرتبط بمعرّف تهديد من `F-001_THREAT_MODEL.md`؛ الاختبارات المذكورة متطلبات إثبات مستقبلية وليست اختبارات موجودة أو منفذة حالياً.

| ID | المرشح | الدليل/المسار | تهديد مرتبط | اختبار/دليل الإغلاق المطلوب |
|---|---|---|---|---|
| BC-01 | إنشاء command admission عام دون Gameplay Policy | `CoreComposition.CreateCommandAdmissionService<TCommand>` | T-03 | اختبار composition/architecture يثبت أن هذا المسار غير مستخدم لأي gameplay ingress، أو أنه غير متاح للطبقة المستهلكة؛ مع اختبار رفض ingress عند غياب Policy. |
| BC-02 | إنشاء match admission دون Gameplay Policy | `CoreComposition.CreateMatchCommandAdmissionService<TCommand>` | T-03 | اختبار مماثل يثبت عدم وصول gameplay commands إليه، وأن كل match gameplay ingress يستخدم authorization service بسياسة مطلوبة. |
| BC-03 | استدعاء Domain public mutators مباشرة | `Domain.Match.AddPlayer/Start/Tick/TryTransfer/CastVote/CollectEcho/TrySendHint` | T-08, T-10 | اختبار حدود/معمارية يثبت أن مراجع Match لا تُتاح لمصدر غير موثوق وأن كل استدعاء خارجي يمر عبر Host/Handler مفوض؛ اختبارات سلبية لمحاولات player caller تنفيذ عمليات Host-only. |
| BC-04 | تعديل مجموعات Match/Player مباشرة | `Match.Players/Events/ProcessedRequestIds/Votes` و`Player.Debuffs/CollectedEchoIds` | T-10 | اختبارات API/ownership تثبت أن المستهلك الخارجي لا يستطيع تعديل الحالة عبر المجموعات؛ يتطلب قرار D-03 وخطة توافق قبل تغيير public API. |
| BC-05 | تسجيل Betrayal مباشرة | `IBetrayalLedger.TryRecord` و`CoreComposition.BetrayalLedger` | T-09 | اختبار يثبت أن التسجيل لا يمكن الوصول إليه من gameplay caller غير موثوق، وأن التسجيل الفعلي يمر عبر Trusted Handler يتحقق من الحدث والعضوية والسلطة. **النطاق:** يُسجل ويُدرج في إثبات bypass ضمن F-001؛ أما تغيير الوصول إلى `internal`/تحويل الواجهة إلى read-only وتنفيذ Trusted Handler فيُنفذ ضمن F-002 بعد اعتماد D2. لا يُعد هذا المرشح مغلقاً بمجرد ترحيله إلى F-002. |
| BC-06 | تمرير مسافة/وقت غير موثوقين إلى عمليات الحالة | `Domain.Match.TryTransfer(..., distanceMeters, ...)`; `Match.Tick(double)`; `CommandEnvelope.ReceivedAt` | T-06 | اختبار يمرر claimed distance/time من العميل ويثبت تجاهلهما؛ ثم يثبت أن المسافة المستخدمة في Transfer والوقت المستخدم في Tick/ReceivedAt مشتقة من Host موثوق. بالنسبة إلى `TryTransfer` تحديداً، لا يُقبل `distanceMeters` من payload العميل. هذا ضمن نطاق F-001 عند ربط الأمر بالـHost/Handler، مع اعتماد mapping الزمن/التسلسل ذي الصلة في D2؛ ولا يُنشأ F-001-D حالياً. |
| BC-07 | تعديل payload بعد admission | `MatchCommandPayloadValidationService<TCommand>` لا ينسخ/يجمد `TCommand` | T-05 | اختبار يغيّر payload بين admission وdequeue ويتحقق من رفضه أو يثبت immutable DTO/exclusive ownership؛ ويعاد التحقق قبل handoff. |
| BC-08 | تجاوز payload validation | `CreateMatchCommandAuthorizationService<TCommand>` لا يفرض validator تلقائياً | T-05 | اختبار composition لكل command معتمد يثبت تركيب validator المطلوب ورفض payload غير صالح/مفرط الحجم؛ وتُختبر quotas الخام عند حد transport/Host. |

## 10. التقاطعات وحدود المسؤولية بين F-001 وF-002 وD2

### 10.1 `IBetrayalLedger.TryRecord` — BC-05 / F-002

- يُعامل `IBetrayalLedger.TryRecord` كمرشح Bypass ضمن جرد F-001 وإثباته؛ لأنه مدخل عام يغيّر حالة Betrayal ويمكن الوصول إلى التنفيذ عبر `CoreComposition.BetrayalLedger`.
- لا يُنفذ إصلاح سطح الوصول في F-001. الإصلاح المقصود — جعل الكتابة داخلية/محدودة، جعل الواجهة العامة للقراءة فقط، وإنشاء Trusted Gameplay Handler — ينتمي إلى F-002.
- يظل BC-05 مفتوحاً في إثبات F-001 إلى أن يثبت F-002 أن كل كتابة تمر عبر Handler موثوق. نقل بند التنفيذ إلى F-002 لا يساوي إغلاق Bypass ولا يبرر إعلان F-001 مغلقاً قبل وجود دليل الربط.
- يعتمد F-002 على عقد D2 المعتمد لهوية Actor/Player وMatch، وعلى اعتماد كتالوج F-001-B قبل تعريف الأمر النهائي.

### 10.2 `Match.TryTransfer` — BC-06 / F-001

- `Match.TryTransfer(Guid, Guid, double distanceMeters, string requestId)` يقبل المسافة من المستدعي؛ لذلك فإن تحقق النطاق/الحد داخل Domain لا يثبت أن المسافة حُسبت من مصدر موثوق.
- يُدرج هذا كـBC-06 ضمن F-001 لأنه يمس موثوقية مدخل أمر Gameplay وتفويضه. يجب أن يستمد Host/Handler المسافة من سياق authoritative، وألا يمرر claimed distance من العميل.
- اختبار الإغلاق المستقبلي يجب أن يثبت أن تغيير قيمة المسافة التي يرسلها العميل لا يغير قرار التفويض/المسافة المعتمدة، وأن القيمة المستخدمة مصدرها Host موثوق. لا يوجد هذا الإثبات حالياً.
- لا يلزم إنشاء F-001-D في هذه المرحلة؛ يسجل BC-06 ضمن F-001-C/إثبات Bypass، بينما تبقى مواءمة الوقت/التسلسل بين Core وDomain ضمن D2.

## 10. استنتاجات وحدود

1. Core لديه بنية تفويض generic، لا سياسات Gameplay أو Handlers إنتاجية ملموسة.
2. وجود Authorization service لا يجعلها إلزامية لكل admission factory أو Domain API.
3. Domain يفرض قواعد داخلية جزئية لكنه لا يصادق caller ولا يثبت موثوقية الوقت/المسافة.
4. لا توجد طبقة Transport/Host runtime في الشجرة تثبت تدفق Client إلى Core/Domain.
5. لا يثبت هذا الجرد وحده غياب تكاملات خارج المستودع؛ يلزم استكمال إثبات عدم bypass في مرحلة مستقلة.
6. لا يتضمن هذا الجرد اعتماد أسماء أوامر أو قواعد لعب جديدة.

## 11. ملفات المصدر المرجعية

`src/CostOfStaying.Core/Contracts/` · `CoreComposition.cs` · `src/CostOfStaying.Core/Domain/` · `src/CostOfStaying.Core/Services/` · `src/CostOfStaying.Domain/Match.cs` · `Player.cs` · `Intent.cs` · `tests/CostOfStaying.Core.Tests/` · `tests/CostOfStaying.Domain.Tests/`
