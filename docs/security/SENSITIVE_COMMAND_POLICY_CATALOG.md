# الحد الأدنى لعقد الأوامر الحساسة وسياسات التفويض

**الحالة: INVENTORY / NOT A GAMEPLAY RULE APPROVAL**  
**الغرض:** حصر عمليات تغيير الحالة الموجودة فعلياً وتحديد ما يلزم قبل ربطها بأوامر وسياسات Gameplay.  
**قاعدة القراءة:** هذه الوثيقة تميز بين سلوك الكود الحالي، وخط أساس مقترح للاختبار، وقرارات المنتج غير المحسومة. لا تحوّل القيم أو القواعد المقترحة إلى صلاحيات إنتاجية.

## 1. نتيجة الجرد

لا توجد في Core حالياً concrete Gameplay command DTOs أو Gameplay handlers موصولة بالطابور. استخدام `string` في اختبارات authorization هو fixture اختباري فقط، وليس عقد أمر منتج.

لذلك يسجل هذا الجرد **عمليات mutation المعروفة**، ولا يخترع أسماء Commands أو أدواراً أو صلاحيات غير موجودة في التنفيذ.

| العملية الموجودة | موضع التنفيذ | الحالة التي يمكن أن تتغير | ما يثبته التنفيذ الحالي | من يحق له إصدارها / مرحلة المباراة | سياسة القبول والرفض |
|---|---|---|---|---|---|
| تقليل Vitality — `Lose(uint)` | `Core/Domain/VitalityAccount.cs` | Current، وقد تنتقل Alive إلى Spirit | لا mutation إذا لم تكن Alive أو كان المقدار صفراً؛ خلاف ذلك يخفض Current ويضبط Spirit عند الصفر | **TBD**؛ لا توجد command DTO أو policy إنتاجية تربط العملية بمصدر موثوق أو phase | guard داخلي للحالة فقط؛ ليس تفويضاً. أي ingress حساس بلا policy صريحة يجب أن يرفض |
| استعادة Vitality — `Restore(uint)` | `Core/Domain/VitalityAccount.cs` | Current | تعمل فقط في Alive وبمقدار موجب، وتحد النتيجة بـMaximum | **TBD** | guard داخلي للحالة فقط؛ مصدر الاستعادة/الحدود/المرحلة غير معتمدة |
| إحياء — `TryResurrect(uint)` | `Core/Domain/VitalityAccount.cs` | Spirit → Alive وCurrent | يتطلب Spirit ومقداراً موجباً؛ يحد Current بـMaximum | **TBD**؛ تعليق الكود يحيل التفويض والتكلفة وcooldown إلى المستدعي، لكن لا توجد سياسة معتمدة | غياب policy أو عدم حسم شروطها = رفض؛ لا تستنتج صلاحية Spirit أو أي دور من primitive |
| إقصاء — `TryEliminate()` | `Core/Domain/VitalityAccount.cs` | Alive/Spirit → Eliminated | يرفض فقط إذا كانت الحالة Eliminated؛ وإلا يجعلها نهائية | **TBD**؛ لا توجد قاعدة منتج أو عملية Domain موحدة معتمدة | غياب policy صريحة = رفض |
| نقل المعاناة — `Match.TryTransfer(sourceId,targetId,distanceMeters,requestId)` | `Domain/Match.cs` | Debuffs للطرفين، LastTransferAt، ProcessedRequestIds، BetrayalPoints، Events | الكود يتحقق من نشاط المباراة، المدخلات، التكرار، اختلاف الطرفين، وجود اللاعبين، Alive، وجود عبء قابل للنقل، المسافة، cooldown، وسعة الهدف؛ ثم ينقل العبء ويزيد Betrayal عند النجاح | **TBD** كصلاحية أمر: لا يوجد Core command DTO أو host adapter يثبت هوية المصدر أو يقيس المسافة. قواعد RULE-02 ما زالت PROPOSED FOR PLAYTEST | الشروط الحالية هي منطق aggregate وليست إثباتاً لهوية المستدعي أو موثوقية distance؛ يجب رفض ingress غير المصرح |
| تسجيل حدث Betrayal — `BetrayalLedger.TryRecord(BetrayalEvent)` | `Core/Domain/BetrayalLedger.cs` | سجل الأحداث ودرجة Actor | يتحقق من بنية الحدث، عدم self-betrayal، تسلسل الحدث وexact replay | **TBD**؛ يتطلب handler موثوقاً وعضوية الطرفين وسلطة الفعل | لا يوجد مسار Gameplay موثوق مكتمل حالياً؛ لا تسجيل حتى اكتماله |

## 2. حدود الهوية التي تمنع اختراع تحقق العضوية

- `MatchPlayerRegistry` يفهرس الأعضاء بواسطة `ActorId`.
- `BetrayalEvent` يحمل `ActorId` للمصدر و`VictimId` للهدف.
- `VictimId` نوع مستقل عن `ActorId`، وتعريفه الحالي هو Transfer target identity. لا توجد في Core واجهة أو دالة موثقة تحوّل `VictimId` إلى `ActorId`.
- لذلك لا يجوز افتراض أن تساوي القيم الرقمية يثبت تطابق الهوية. قبل فحص عضوية Victim يلزم اعتماد mapping صريح أو تعديل عقد الهوية بقرار توافق.
- لا توجد حالياً Gameplay handler أو خدمة تربط `AuthenticatedCommand<T>` بتسجيل `BetrayalEvent`.

**القرار الآمن المؤقت:** لا يُنشأ مسار كتابة جديد للـledger ولا تُعتبر F-002 مغلقة. تبقى واجهة `IBetrayalLedger` للقراءة فقط وكتابة ledger داخلية. أي محاولة تسجيل لا تمر عبر مسار موثوق مكتمل يجب ألا تُنفذ.

## 3. سياسة التفويض: ما هو منفذ بالفعل

`MatchCommandAuthorizationService<TCommand>` يطبق الضمانات التالية:

1. يتطلب `IMatchCommandAuthorizationPolicy<TCommand>` غير null عند الإنشاء؛ غيابها construction failure.
2. يرفض envelope غير الصالح، والجلسة غير المصادق عليها، والـActor غير الموجود في roster قبل استدعاء policy.
3. ينشئ context من SessionId وActorId وMatchId وMatchPhase وPlayerLifecycleState وReceivedAt وcommand، ولا يأخذ هذه الحقائق من payload العميل.
4. يستدعي policy قبل queue admission ثم يعيد التقييم بعد dequeue وقبل تسليم الأمر للمعالج.
5. نتيجة policy غير المهيأة تتحول إلى `InvalidState`، والاستثناء يتحول إلى `Unavailable`؛ لا توجد permissive fallback.

كما تم إخفاء `CreateCommandAdmissionService<T>` و`CreateMatchCommandAdmissionService<T>` عن public API في CoreComposition؛ فهما بوابتان أدنى لا يجوز أن تكونا ingress عامّاً لأوامر Gameplay لأنها لا تفرض Gameplay policy. يظل الوصول الداخلي لهما محصوراً في تركيب Core والاختبارات الصديقة.

هذا يثبت **رفض غياب policy عند إنشاء مسار التفويض**، لكنه لا يثبت أن policy الممررة صحيحة أو restrictive، ولا يثبت أن جميع أنواع الأوامر في المنتج تمر بهذا المسار.

## 4. سجل الأوامر/السياسات المطلوبة

لا تسجل أي concrete command قبل وجود DTO فعلي ومواصفة معتمدة. عند إضافة كل أمر، يجب أن يكتمل السجل التالي:

| الحقل المطلوب | القيمة الحالية |
|---|---|
| Command type / DTO | لا يوجد في Core — TBD |
| Mutation target | يحدد من عمليات القسم 1 |
| Trusted issuer / role | TBD |
| Allowed match phases | TBD |
| Required actor lifecycle state | TBD |
| Target identity and membership mapping | TBD، خصوصاً VictimId → ActorId |
| Preconditions and server-derived facts | TBD |
| Explicit denial conditions / error mapping | TBD |
| Policy implementation and composition registration | غير موجودة للإنتاج |
| Positive, negative, stale-state and missing-policy tests | لا توجد tests لأوامر Gameplay فعلية |

**قاعدة الإطلاق:** أي خانة مطلوبة بقيت TBD تعني أن الأمر غير مفعّل؛ لا يُستبدل TBD بسماح افتراضي.

## 5. اختبارات البنية المطلوبة

الاختبارات الموجودة تثبت بالفعل:
- policy غير موجودة عند الإنشاء → `ArgumentNullException`.
- policy تعيد نتيجة default/Uninitialized → `InvalidState`.
- policy ترمي استثناء → `Unavailable`.
- رفض policy لا يستهلك queue sequence، وإعادة المحاولة ممكنة.
- إعادة تقييم policy بعد dequeue تمنع تسليم أمر أصبح stale.

أضيف إلى ذلك اختبار public API يمنع إظهار factories الخاصة بـsession-only وmembership-only admission كمسارات عامة. تبقى الاختبارات التالية غير ممكنة حتى وجود DTOs وسياسات فعلية:
- لكل command: allow/deny حسب phase وactor state.
- policy خاطئة أو permissive → deny وفق مواصفة الأمر المعتمدة.
- كل command حساس في catalog مسجل مرة واحدة ولا توجد أوامر حساسة خارج catalog.
- Betrayal: Actor غير عضو، Victim غير عضو، Actor غير مطابق للهوية المصادق عليها، والسلطة غير متحققة.

## 6. المراجع الحاكمة

- `src/CostOfStaying.Core/Domain/VitalityAccount.cs`
- `src/CostOfStaying.Core/Contracts/IVitalityAccount.cs`
- `src/CostOfStaying.Core/Domain/BetrayalLedger.cs`
- `src/CostOfStaying.Core/Contracts/IBetrayalLedger.cs`
- `src/CostOfStaying.Core/Domain/Identifiers.cs`
- `src/CostOfStaying.Core/Domain/MatchPlayerRegistry.cs`
- `src/CostOfStaying.Core/Services/MatchCommandAuthorizationService.cs`
- `src/CostOfStaying.Core/CoreComposition.cs`
- `src/CostOfStaying.Domain/Match.cs`
- `docs/02_GAME_RULES_SPECIFICATION.md` — PROPOSED FOR PLAYTEST
- `docs/systems/SUFFERING_TRANSFER_SYSTEM.md` — PROPOSED FOR PLAYTEST
- `docs/systems/BETRAYAL_SYSTEM.md` — PROPOSED FOR PLAYTEST

## 7. الحالة الأمنية

- **F-001:** fail-closed على مستوى public Core ingress صار أقوى بعد إخفاء factories التي تتجاوز Gameplay policy. لا تغلق بالكامل قبل وجود command catalogue إنتاجي واختبارات restrictive لكل أمر فعلي.
- **F-002:** الكتابة العامة المباشرة ممنوعة، لكن المسار الموثوق والتحقق من عضوية Victim وسلطة الفعل غير منفذ. تبقى مفتوحة.
- **F-003:** لا تغيير في retention أو replay semantics.
- **F-004:** منفذة ومختبرة على فرع المراجعة؛ لم تدمج في main.

هذه الوثيقة جرد ومواصفة ربط أمنية؛ لا تعتمد قواعد لعب جديدة ولا تعتبر نتائج المقترحات قرارات منتج.
