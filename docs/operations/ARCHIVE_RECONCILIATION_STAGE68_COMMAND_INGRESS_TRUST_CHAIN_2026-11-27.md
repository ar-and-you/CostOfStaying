# المرحلة 68 — مراجعة ترتيب command ingress وحدود الثقة (2026-11-27)

## الرأس والنطاق

- HEAD قبل المرحلة: 76ab56433a96dc67180ecf979dd0bec79df27e97.
- تمت مراجعة CoreComposition وسلسلة MatchCommandPayloadValidationService → MatchCommandAuthorizationService → MatchCommandAdmissionService → CommandAdmissionService → CommandAdmissionQueue، واختباراتها.
- الهدف: تحديد ترتيب التحقق وحدود الثقة بين transport وCore وgameplay handler.

## الترتيب الفعلي

1. **Transport قبل Core:** raw frame limits وdecompression/serializer quotas؛ غير موجودة في المستودع.
2. **Connection binding:** server-issued TransportConnectionId يرتبط بـPending SessionId.
3. **Credential validation:** host يثبت credentials ويربط account بـActorId؛ لا يوجد provider داخل Core.
4. **Session authentication:** TryAuthenticate يربط ActorId مرة واحدة.
5. **Payload validation:** typed TCommand يتحقق قبل session/authorization admission حتى لا تستهلك المدخلات غير الصالحة queue/sequence.
6. **Authorization/membership:** يتحقق من session Authenticated، ActorId، match roster، phase/player state، وسياسة صريحة fail-closed.
7. **Sequence/queue:** per-session sequence مع queue capacity محدودة؛ queue full لا يستهلك sequence.
8. **After dequeue:** membership ثم authorization policy ثم payload validation يعاد التحقق منها؛ الفشل يdiscard command بعد استهلاك sequence.
9. **Handler:** Core يعيد AuthenticatedCommand؛ host هو من يستدعي gameplay handler.

## حدود الثقة

- Validator يسبق authentication؛ يجب أن يكون bounded، synchronous، deterministic، side-effect-free، بلا I/O. لا يجوز أن ينفذ regex/parse ثقيل أو allocations غير محدودة.
- TCommand لا يُنسخ أو يُجمد؛ host يوفر immutable DTO أو ينقل ownership حصرياً.
- ReceivedAt/ServerTick يجب أن ينشئه host من monotonic ingress clock؛ Core لا يستطيع إثبات أنه لم يأت من client.
- SessionId/ActorId وmatch membership وdistance لا تؤخذ من payload غير موثوق.
- CommandSequenceId يتحقق per-session؛ لا يعني durable idempotency بعد reconnect/restart.
- لا raw frame/serializer quotas أو transport parser أو handler أو actual network adapter داخل Core.

## التغييرات

- أضيف ترتيب ingress وجدول المسؤوليات إلى NETWORK_ARCHITECTURE.md.
- أضيف تحذير ترتيب payload validation قبل auth إلى D5b في OPEN_DECISION_REGISTER.
- لا تغيير source/tests/Workflows.
- آخر Core CI ناجح 248/248 على SHA d9c4e93؛ لا اختبار جديد في هذه المرحلة.

**الحالة:** STAGE 68 COMPLETE — COMMAND INGRESS TRUST ORDER DOCUMENTED; TRANSPORT/HOST RESPONSIBILITIES REMAIN OPEN.
