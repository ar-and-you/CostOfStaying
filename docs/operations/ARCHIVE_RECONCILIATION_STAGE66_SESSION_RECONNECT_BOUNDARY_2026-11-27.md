# المرحلة 66 — مراجعة دورة حياة Session وسياسة إعادة الاتصال (2026-11-27)

## الرأس والنطاق

- HEAD قبل المرحلة: 3d4dc880b3e1b7c9a41b08fe0b1cb02a0e967389.
- تمت مراجعة SessionLifecycle وSessionRegistry وConnectionSessionRegistry وخدمات cleanup واختباراتها.
- الهدف: فصل session reconnect primitive عن product/player reconnect policy.

## النتائج

1. SessionLifecycle states هي Pending → Authenticated → Disconnected → Closed.
2. TryAuthenticate يقبل Pending فقط؛ Disconnected لا يمكن إعادة تفعيله. Core يتطلب SessionId جديداً لإعادة الاتصال التقنية.
3. TryDisconnect يحتفظ بActorId، لكنه لا ينفذ detach للاتصال أو queue cleanup تلقائياً.
4. SessionRegistry bounded حسب capacity، لكنه لا يفرض active-session limit لكل ActorId.
5. ConnectionSessionRegistry يفرض one-to-one connection↔session، ويسمح لعدة sessions تحمل ActorId نفسه.
6. ConnectionId يجب أن يكون server-issued وألا يعاد استخدامه خلال عمر العملية.
7. لا credentials/timers/lease expiry/reconnect token أو reconnect coordinator داخل Core.
8. إزالة session تتطلب Close، ثم detach، ثم queue drain/reset، ثم cleanup؛ هذه خطوات منفصلة ومسؤولية host.

## الاختبارات الموجودة

- Authenticate exactly once; rejects second actor association.
- Disconnect only from Authenticated; preserves ActorId.
- DisconnectedSession_CannotBeReauthenticated.
- ClosedSession_IsTerminal.
- Connection registry rejects duplicate connection/session bindings and rejects non-live binding states.
- Cleanup tests require closed, detached, and drained session.

## القرار

- KEEP_CURRENT_SESSION_PRIMITIVES؛ لا تغيير كود أو tests.
- D-R3 يبقى OPEN_OWNER_DECISION: Core يحدد SessionId جديداً، لكنه لا يقرر استعادة اللاعب إلى المباراة أو التصويت أو roster.
- multi-session per ActorId policy تبقى OPEN؛ لا account/credential provider أو transport adapter.
- أضيفت حدود التنفيذ إلى NETWORK_ARCHITECTURE.md وسجل القرارات.

**الحالة:** STAGE 66 COMPLETE — SESSION RECONNECT BOUNDARY DOCUMENTED; PRODUCT RECONNECT POLICY REMAINS OPEN.
