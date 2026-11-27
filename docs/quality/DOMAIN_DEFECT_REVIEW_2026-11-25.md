# مراجعة عيوب Domain وخطة الإصلاح — 2026-11-25

**نوع المراجعة:** فحص ساكن ومراجعة منطق المصدر واختبارات الانحدار.
**الفرع المرجعي قبل الإصلاح:** `main`
**SHA قبل الإصلاح:** `5c8e56fb52b2171b6f517edd89fc915fd9d7bda3`
**حالة المشروع:** Pre-production. قواعد اللعب وقيم التوازن ما زالت `PROPOSED FOR PLAYTEST`.

## 1. نطاق الأدلة

تمت مراجعة `Match.cs` و`Player.cs` و`Debuff.cs` و`MatchConfig.cs` واختبارات Domain ذات الصلة، ومواصفة القواعد ونظام Collapse ونظام Suffering Transfer. تمت مراجعة شجرة GitHub الحية، وملفات CI، وحزم ZIP المرفقة.

آخر تشغيلين مرجعيين قبل الإصلاح على SHA `5c8e56fb52b2171b6f517edd89fc915fd9d7bda3`:
- [Domain and Python CI](https://github.com/ar-and-you/CostOfStaying/actions/runs/45666459014) — success.
- [Core CI (.NET 10)](https://github.com/ar-and-you/CostOfStaying/actions/runs/45666459066) — success.

هذه النتائج تثبت الحالة السابقة فقط، ولا تثبت الإصلاحات الحالية.

## 2. العيوب التي عولجت في هذا التغيير

### D-01 — احتساب Debuff بعد انتهاء مدته داخل Tick طويل

**الأثر:** إذا كان عمر التأثير المتبقي أقصر من `deltaSeconds`، كان `Match.Tick` يحتسب `TotalDecayPerSecond` قبل إزالة التأثير، ويطبّق المعدل الأعلى على كامل الفترة. النتيجة تعتمد على حجم Tick وقد تستنزف Vitality أكثر من الاستحقاق.

**الإصلاح:** تقسيم فترة Tick إلى مقاطع تنتهي عند أقرب انتهاء Debuff. يحتسب كل مقطع معدل التأثيرات النشطة خلاله فقط، ثم تُزال التأثيرات المنتهية.

**اختبار الانحدار:** `Debuff_Contributes_Decay_Only_While_Its_Duration_Is_Active_During_Long_Tick`.

**حدود التغيير:** لا تتغير قيم التوازن أو مدة أي تأثير؛ يتغير فقط تطبيق المدة الفعلية.

### D-02 — قبول Debuff لقيم غير صالحة

**الأثر:** كان المُنشئ يقبل معرفاً فارغاً، ومكافأة استنزاف سالبة أو غير محدودة، ومدة صفرية/سالبة أو غير محدودة. وقد يؤدي ذلك إلى فساد الحساب أو حلقة Tick لا تتقدم.

**الإصلاح:** فرض معرف غير فارغ، ومكافأة finite وغير سالبة، ومدة finite وموجبة. كما أصبح `MatchConfig` يرفض مدة Debuff غير الموجبة عند إنشاء المباراة بدلاً من تأجيل الفشل إلى أول Collapse.

**الاختبارات:** `DebuffTests` واختبار إعداد المباراة في `SecurityTests`.

### D-08 — قبول RequestId فارغ في Suffering Transfer

**الأثر:** كان `Match.TryTransfer` يتجاوز فحص وتسجيل idempotency عندما يكون `requestId` null أو فارغاً أو whitespace؛ وبذلك يمكن أن ينجح النقل دون مفتاح يمنع إعادة التطبيق.

**الإصلاح في المرحلة 42:** إضافة `RejectedInvalidRequestId` ورفض `string.IsNullOrWhiteSpace(requestId)` قبل أي mutation. أضيف اختبار انحدار يمرر null وempty وwhitespace ويتحقق من ثبات Debuffs وBetrayal وProcessedRequestIds وEvents. أضيفت القيمة الجديدة في نهاية enum للحفاظ على الأرقام الحالية للقيم السابقة.

**الحدود:** تبقى `ProcessedRequestIds` مجموعة داخل الذاكرة وعمر Match واحد؛ لا توفر deduplication دائماً بعد restart. كما أن تكرار معرّف صالح يسجل `TransferRejected` في سجل الأحداث مع عدم تغيير حالة اللعب. لم تُحل سياسة الاحتفاظ D-04 أو ضمانات التزامن المتوازي.

## 3. نتائج ومخاطر لم تُغيّر

### D-03 — مجموعات Domain قابلة للتعديل من خارج المجمع

`Match.Players` و`Events` و`ProcessedRequestIds` و`Votes`، وكذلك `Player.Debuffs` و`CollectedEchoIds`، تعرض مجموعات قابلة للتعديل. يتيح ذلك تجاوز عمليات المجال الرسمية. لم يُكسر هذا الـAPI في هذا التغيير لأن الاختبارات والمستهلكين الحاليين يستخدمونه، ويجب حسم استراتيجية التوافق/التغليف قبل تغيير عام.

**مراجعة الاستهلاك في المرحلة 55:** البحث في الشجرة لم يجد runtime/Unity consumer يكتب هذه المجموعات؛ الاستخدامات المرصودة داخل aggregate واختبارات Domain. Core لا يملك ProjectReference إلى Domain ولا يوجد Unity project. لكن public mutability تسمح لمستهلك خارجي بإدخال/حذف Player خارج AddPlayer، تعديل Votes دون CastVote، تغيير/حذف Events، حذف RequestId ناجح لإعادة فتح replay، أو تعديل Debuffs/Echo IDs خارج قواعد المجال.

**المعالجة قبل أول تكامل:** backing collections خاصة؛ Players/Votes عبر ReadOnlyDictionary حقيقي، Events/Debuffs عبر ReadOnlyCollection، وإخفاء ProcessedRequestIds/CollectedEchoIds مع count/query APIs. تُنقل fixture mutations إلى internal builders/mutators لا يراها إلا test assembly عبر InternalsVisibleTo، وتضاف اختبارات تثبت أن read-only views لا يمكن cast منها إلى backing collection. هذا تغيير public API ويحتاج قرار توافق صريح؛ لم ينفذ هنا.

### D-04 — نمو سجل الأحداث ومفاتيح Idempotency

`Events` و`ProcessedRequestIds` لا يظهر لهما حد احتفاظ داخل مجمع Domain. قد يزيد استهلاك الذاكرة في مباراة طويلة أو عند كثرة الطلبات المرفوضة. لا يضاف حد عشوائي قبل تحديد سياسة الاحتفاظ وإعادة الإرسال والتدقيق.

**تفصيل المرحلة 55:** `Emit` يضيف كل حدث إلى List طوال عمر Match، بما في ذلك أحداث رفض النقل. كل Transfer ناجح يضيف RequestId إلى HashSet بلا TTL أو سقف، وتحتفظ Player بـCollectedEchoIds لكل معرف فريد. `TrySendHint` يخزن نص hint داخل الحدث، و`requestId` يدخل في تفاصيل بعض الرفض؛ لم يظهر حد طول موحد لهذه المدخلات. لا توجد rate limiter أو transport/server runtime في الشجرة، لذا هذا خطر سعة وتكامل مستقبلي وليس ادعاءً بوجود استغلال شبكي حالي.

**إضافة المرحلة 64 — Core:** CoreComposition ينشئ BetrayalLedger لكل match scope؛ ledger يحتفظ بكل BetrayalEvent في Dictionary بلا capacity أو retention API، كما يحتفظ بـActor score dictionary بلا حد مستقل. TryRecord لا يتحقق من Actor/Victim membership في MatchPlayerRegistry؛ لذلك host يجب أن يتحقق من roster/authority قبل التسجيل. exact-old-replay semantics تعتمد على الاحتفاظ بالحدث الأصلي، فلا يجوز eviction عشوائي.

لا يمكن تحديد cap رقمي من المصدر وحده: إخلاء RequestIds قبل نهاية المباراة قد يعيد فتح replay، وتقليص Events قد يفقد سجل التدقيق ما لم يوجد sink دائم، وحد Echo/Hint/ID يحتاج عقد منتج وقياس workload. المطلوب تحديد عمر المباراة، معدل/عدد الأوامر، حدود أحجام النصوص، حفظ الأحداث، ونطاق idempotency بعد restart.

### D-05 — سياسة تصويت اللاعب الذي يموت أو ينقطع أثناء التصويت

يفتح Domain التصويت بقائمة snapshot للاعبين Alive ويهيئ كل صوت إلى Abstain. إذا انتقل لاعب لاحقاً إلى Spirit، يبقى ضمن Votes؛ يرفض CastVote أي تغيير جديد منه، لكن صوته السابق يبقى. لا توجد معالجة disconnect/reconnect أو إزالة الناخب عند فقد الاتصال. تعليق قديم في CloseVote كان يصف disconnect=No رغم غياب حدث disconnect؛ صُحح التعليق في المرحلة 56 ليطابق حدود التنفيذ.

المواصفة الحية تترك أثر الموت/الانقطاع غير محسوم. يلزم قرار منتج/شبكة: هل يبقى snapshot ثابتاً، أم تزال الأصوات عند الخروج، أم يصبح الصوت Abstain/No، وما أثر reconnect؟ لا تُستورد سياسة No من الأرشيف.

### D-06 — Collapse catch-up

يعالج `Tick` Collapse واحداً كحد أقصى لكل لاعب Alive في الاستدعاء؛ وإذا تجاوز `now` الموعد، يضبط `ApplyCollapse` الموعد التالي إلى `now + interval`، فتُتجاوز الفترات الفائتة ولا يحدث catch-up متعدد. هذه حقيقة التنفيذ الحالية وليست سياسة منتج معتمدة. يلزم اختيار skip أو bounded catch-up أو replay للفترات الفائتة قبل تكامل خادم فعلي.

### D-07 — حدود Domain/Core

يبقى المشروعان منفصلين. لا توجد في `docs/adr/` قرارات مقبولة تسمح بنقل منطق Domain إلى Core أو توحيد lifecycle models. لم يتم أي ترحيل أو تغيير معماري.

## 4. فجوات التكامل والملفات غير الموجودة

تؤكد شجرة المستودع المفحوصة عدم وجود مشروع Unity قابل للفتح (مثل `ProjectSettings/` و`Packages/manifest.json` ومشاهد `.unity`)، أو إعداد بناء Android، أو خادم لعب/transport فعلي. توجد مكتبتا Domain وCore وأدوات محاكاة واختبارات فقط.

لا يمكن استكمال هذه المكونات بأمان عن طريق إنشاء ملفات شكلية. يلزم قبل ذلك اعتماد: إصدار Unity، حزمة الشبكة/transport، نموذج الاستضافة، المصادقة، بنية المشاهد والأصول، متطلبات Android، وأهداف الأداء والأجهزة. تبقى هذه العناصر `TBD` وليست محسوبة كإصلاحات مكتملة.

## 5. مصالحة الأرشيف المرفق

- اجتازت ملفات SHA-256 داخل حزمتَي Phase 1 والوثائق canonical التحقق.
- حزمة Phase 1 تشترط SHA أساساً `1b209b7...`؛ هذا ليس SHA الحالي، لذلك لم تُطبق تعليماتها القديمة.
- PR التوثيق رقم [#1](https://github.com/ar-and-you/CostOfStaying/pull/1) أُدمج فعلياً في 2026-11-24، ما يجعل محضر الإغلاق القديم بحاجة إلى قراءة مع SHA الدمج الفعلي.
- حزمة `Local Completion Package` تصف نفسها بأنها DRAFT / REVIEW REQUIRED وتمنع اعتبارها دليلاً على الدمج أو الاختبار. لم تُنسخ ملفاتها آلياً ولم تُحوّل أدوار الوكلاء أو تعيينات النماذج إلى سياسة نافذة.
- لم تُستورد ADRs تاريخية أو ملفات ثنائية أو مشروع Unity من الأرشيف.

## 6. التحقق والقيود

- تم إنشاء اختبارات انحدار للحالتين D-01 وD-02.
- بيئة الفحص الحالية لا تحتوي على .NET SDK؛ لذلك لم يتم ادعاء بناء أو اختبار NUnit محلي.
- يجب ربط نجاح Domain CI بالـSHA الجديد بعد دفع التغيير قبل وصف الإصلاح بأنه `VERIFIED`.
- لا يثبت نجاح Domain/Core CI وجود Unity أو Android أو شبكة أو تكافؤ C#↔Python أو نجاح جلسات لعب بشرية.

## 7. التوصيات التالية

1. حسم ADR Domain/Core قبل أي ترحيل.
2. اعتماد واجهات قراءة غير قابلة للتعديل أو snapshots لحالة Domain، مع خطة توافق API.
3. تعريف سياسة احتفاظ للأحداث وrequest IDs وحدود حجم المدخلات.
4. تحديد سياسة tick lag وcollapse catch-up.
5. اعتماد قرارات Unity/network/Android قبل إنشاء ملفات التكامل.
6. بناء اختبارات تكامل حقيقية بين transport وDomain/Core، ثم Unity EditMode/PlayMode وAndroid build validation.
