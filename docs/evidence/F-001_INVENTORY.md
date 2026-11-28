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

## 9. Bypass candidates

| ID | المرشح | الدليل | المطلوب لإغلاقه |
|---|---|---|---|
| BC-01 | إنشاء admission عام دون Policy | `CreateCommandAdmissionService` | تحديد الاستخدام ومنعه/تقييده في gameplay ingress. |
| BC-02 | إنشاء match admission دون Policy | `CreateMatchCommandAdmissionService` | إثبات عدم استخدامه للأوامر أو تقييده. |
| BC-03 | استدعاء Domain public mutators مباشرة | APIs في `Match.cs` | إثبات أن Host adapter وحده يملك المراجع ويفرض التفويض. |
| BC-04 | تعديل مجموعات Match/Player مباشرة | خصائص المجموعات العامة | قرار وخطة D-03؛ لا تغيير كاسر هنا. |
| BC-05 | تسجيل Betrayal مباشرة | `IBetrayalLedger.TryRecord` وCoreComposition getter | Trusted Handler وعقد D2؛ يرتبط بـF-002. |
| BC-06 | تزوير المسافة/الوقت | `TryTransfer`, `Tick`, `CommandEnvelope.ReceivedAt` | اشتقاق القيم من Host موثوق. |
| BC-07 | تعديل payload بعد admission | payload wrapper لا ينسخ/يجمد TCommand | immutable payload أو exclusive ownership مع post-dequeue validation. |
| BC-08 | تجاوز payload validation | factory authorization لا يفرض validator | تحديد متطلبات payload لكل أمر وtransport quotas. |

## 10. استنتاجات وحدود

1. Core لديه بنية تفويض generic، لا سياسات Gameplay أو Handlers إنتاجية ملموسة.
2. وجود Authorization service لا يجعلها إلزامية لكل admission factory أو Domain API.
3. Domain يفرض قواعد داخلية جزئية لكنه لا يصادق caller ولا يثبت موثوقية الوقت/المسافة.
4. لا توجد طبقة Transport/Host runtime في الشجرة تثبت تدفق Client إلى Core/Domain.
5. لا يثبت هذا الجرد وحده غياب تكاملات خارج المستودع؛ يلزم استكمال إثبات عدم bypass في مرحلة مستقلة.
6. لا يتضمن هذا الجرد اعتماد أسماء أوامر أو قواعد لعب جديدة.

## 11. ملفات المصدر المرجعية

`src/CostOfStaying.Core/Contracts/` · `CoreComposition.cs` · `src/CostOfStaying.Core/Domain/` · `src/CostOfStaying.Core/Services/` · `src/CostOfStaying.Domain/Match.cs` · `Player.cs` · `Intent.cs` · `tests/CostOfStaying.Core.Tests/` · `tests/CostOfStaying.Domain.Tests/`
