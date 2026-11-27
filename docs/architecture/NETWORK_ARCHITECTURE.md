# معمارية الشبكة — تصميم مستقبلي

**الحالة:** `PROPOSED`. لا يوجد بروتوكول شبكة أو FishNet adapter أو Dedicated Server قابل للتشغيل في `main`.

## نموذج الثقة المستهدف

- جهاز العميل غير موثوق.
- الشبكة العامة غير موثوقة.
- خادم المباراة — عند بنائه — هو مصدر الحقيقة لحالة المباراة.
- Backend لا يملك tick المباراة إلا بقرار معماري صريح.

## عقد الأوامر المقترح

يرسل العميل intent فقط. يتحقق الخادم من هوية actor، الجلسة، MatchId، الحالة الحالية، الأذونات، حدود payload، المعدل، رقم التسلسل، ومفتاح idempotency. يرفض الطلب غير الصالح دون mutation. لا يرسل العميل `SetHealth` أو `TransferSucceeded` أو `VotePassed` أو نتائج محسوبة.

## ما يغطيه الكود الآن

Domain يطبق منطقاً خادمياً داخل عملية واحدة ويقبل distance كمدخل caller؛ Core يوفر خدمات/عقوداً للأوامر والجلسات والتحقق. لا يوجد transport أو authentication integration أو rate limiter شبكي أو server deployment. اختبارات الوحدة لا تثبت بروتوكولاً متصلاً.

## اختبارات مطلوبة قبل Alpha

Replay، إعادة ترتيب وتسلسل الرسائل، payload limits، rate limits، انقطاع وإعادة اتصال، تبديل actor/session، حالات لاعب/مباراة خاطئة، RTT وloss وjitter، وقياس server tick/CPU/bandwidth. قيم 100/200/300ms و1/3/5% نقاط اختبار مقترحة وليست SLA.


## حدود Session lifecycle في Core الحالي

Core يعرّف SessionState: Pending, Authenticated, Disconnected, Closed. SessionLifecycle.TryDisconnect يحول Authenticated إلى Disconnected، ولا توجد عملية re-authenticate أو reconnect على نفس SessionId؛ TryAuthenticate يقبل Pending فقط. إعادة الاتصال التقنية تتطلب SessionId جديداً وتفويض ActorId من application layer. لا يحتفظ Core credentials أو timers أو transport objects.

SessionRegistry يسمح بأكثر من SessionId لنفس ActorId ولا يفرض سياسة multi-session. ConnectionSessionRegistry يفرض one-to-one بين TransportConnectionId وSessionId، لكنه يسمح لعدة sessions بالارتباط بنفس ActorId؛ يجب أن يخصص transport adapter ConnectionId جديداً غير معاد الاستخدام خلال عمر العملية. Detach/Close/queue drain خطوات منفصلة؛ لا يوجد reconnect coordinator أو session lease/timeout داخل Core.

هذا وصف للـprimitives الحالية فقط؛ لا يحسم هل اللاعب يعود إلى نفس Match/roster/vote، أو هل يسمح المنتج بأكثر من جلسة نشطة للحساب نفسه.


## Authentication and Pending Session boundary

ConnectionSessionRegistry binds a server-issued TransportConnectionId to a registered Pending Session before authentication so trusted ingress can resolve the session while validating credentials. The binding itself does not authenticate. SessionLifecycle.TryAuthenticate accepts only a validated ActorId and stores no credential/token; CommandAdmissionService rejects commands until the state is Authenticated. The application/host must validate credentials and bind the resulting account to a server-issued ActorId before calling TryAuthenticate.

A Pending session consumes bounded SessionRegistry and connection capacity until it is closed and cleaned up. Core has no pending-session TTL, authentication-failure counter, rate limiter, credential validator, identity provider, or account-to-ActorId mapping. Host must expire/close stale pending sessions, limit authentication attempts, and detach connection mappings on disconnect. TryAuthenticate is a trusted application boundary, not a credential check.


## ترتيب command ingress في Core

| المرحلة | المسؤول | الضابط |
|---|---|---|
| Raw frame / deserialize | Transport host (غير موجود حالياً) | يفرض frame-size, decompression, serializer quotas قبل إنشاء TCommand |
| Connection binding | ConnectionSessionRegistry | server-issued ConnectionId إلى Pending SessionId؛ لا authentication |
| Credential validation | Host identity provider (غير موجود) | يثبت الحساب ثم يختار ActorId server-issued |
| Session authentication | SessionLifecycle.TryAuthenticate | يربط ActorId مرة واحدة؛ لا يتحقق من credentials |
| Typed payload validation | MatchCommandPayloadValidationService | يعمل قبل session/authorization admission؛ يجب أن يكون bounded/synchronous/deterministic/no-I/O |
| Session + roster + authorization | MatchCommandAuthorizationService / MatchCommandAdmissionService | يثبت session Authenticated وActorId roster member ويطبق policy صريحة fail-closed |
| Sequence + bounded queue | CommandAdmissionService / CommandAdmissionQueue | يتحقق من per-session sequence وqueue capacity؛ full queue لا يستهلك sequence |
| Dequeue revalidation | Membership → policy → payload | يعيد التحقق من العضوية والحالة والسياسة والpayload؛ الفشل بعد dequeue يستهلك sequence ولا يسلّم الأمر |
| Gameplay handler | Host-owned | Core يعيد AuthenticatedCommand؛ host وحده يستدعي handler بعد التحقق |

**Trust constraints:** payload validation تسبق authentication كي لا تستهلك الأوامر غير الصالحة sequence/queue، لذلك validator يرى payload غير موثوق ويجب أن يكون سريعاً ومحدوداً بلا parsing ثقيل أو I/O. ReceivedAt/ServerTick يجب أن ينشئه host عند ingress؛ لا يُقبل timestamp من العميل. CommandSequenceId يمكن أن يأتي ضمن envelope لكنه يتحقق per-session. لا يثق host في SessionId/ActorId أو distance أو match state من payload. Core لا يملك raw frame limits أو transport parser أو handler.
