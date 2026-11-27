# المرحلة 67 — مراجعة حدود المصادقة والجلسات Pending (2026-11-27)

## الرأس والنطاق

- HEAD قبل المرحلة: 41c4642b80a48754f071f14fd28230bce2c5105e.
- تمت مراجعة SessionLifecycle وSessionRegistry وConnectionSessionRegistry وCommandAdmissionService واختباراتها.
- الهدف: توضيح حد الثقة بين transport ingress ومصادقة credentials وإنشاء ActorId.

## السلوك الحالي

1. Host ينشئ SessionId server-issued ويسجله كـPending.
2. ConnectionSessionRegistry يربط TransportConnectionId server-issued بالـPending session قبل المصادقة؛ هذا الربط لا يصادق الجلسة.
3. GetSessionId يسمح بحل Pending أو Authenticated session حتى يتمكن trusted ingress من التحقق من credentials.
4. SessionLifecycle.TryAuthenticate يقبل ActorId غير صفري من application layer موثوق، ويربطه مرة واحدة.
5. CommandAdmissionService يرفض الأوامر ما لم تكن الجلسة Authenticated.
6. Core لا يخزن credentials أو access tokens ولا يتحقق منها، ولا ينشئ account-to-ActorId mapping.

## الفجوات

- لا credential validator أو identity provider أو token/session credential contract.
- لا pending-session TTL أو authentication failure counter/rate limiter.
- Pending sessions تشغل SessionRegistry/ConnectionSessionRegistry capacity حتى close/detach/cleanup.
- لا سياسة multi-session per ActorId أو account revocation.
- Host يجب أن يغلق stale Pending sessions، ويحدد rate limits، ويتحقق من credentials قبل TryAuthenticate، ويضمن أن TransportConnectionId صادر من transport trusted ولا يعاد استخدامه خلال process lifetime.

## الاختبارات الموجودة

- Bind requires registered Pending session.
- Binding does not authenticate session.
- Pending/Disconnected/Closed sessions cannot pass authenticated command admission.
- SessionLifecycle authenticates one ActorId exactly once and rejects second association.
- ConnectionSessionRegistry enforces one-to-one connection/session binding.

## القرار

- KEEP_CURRENT_CORE_BOUNDARY؛ لا إضافة credentials إلى Core ولا تعديل API.
- D5b يبقى OPEN_TECHNICAL_SPIKE؛ يجب اختيار credential provider/transport وpending TTL/rate limits في host design.
- أضيفت المسؤوليات إلى NETWORK_ARCHITECTURE وسجل القرارات.
- لا code/tests changes.

**الحالة:** STAGE 67 COMPLETE — AUTHENTICATION REMAINS A TRUSTED HOST RESPONSIBILITY; PENDING SESSION RESOURCE POLICY OPEN.
