# المرحلة 61 — مراجعة backpressure وتنظيف جلسات Core (2026-11-27)

## الرأس والنطاق

- HEAD قبل المرحلة: d9dbef77cccc05f2c281ede2d19481121085f591.
- تمت مراجعة CommandAdmissionQueue وCommandSequenceTracker وSessionCleanupService وConnectionAwareSessionCleanupService واختباراتها.
- الهدف: التحقق من atomicity للـsequence والـqueue cleanup قبل host integration.
- مراجعة ساكنة؛ لا تغيير source أو tests.

## النتائج

1. **Queue capacity قبل sequence:** TryEnqueue يتحقق من صلاحية envelope ثم من Capacity قبل استدعاء TryAccept. لذلك full queue لا يستهلك sequence.
2. **Sequence بعد admission:** عند قبول الأمر، يُستهلك sequence قبل وضع envelope في FIFO. إذا رفضت gameplay validation لاحقاً، يبقى sequence مستهلكاً لمنع replay.
3. **Session cleanup:** ResetSession يرفض إذا بقي أي envelope للجلسة في queue. SessionCleanupService يتطلب Closed، ويزيل replay state بعد drain ثم يحذف SessionRegistry entry.
4. **In-flight boundary:** تعليق الخدمة يطلب من caller الانتظار حتى ينتهي أي command سبق dequeue؛ لا يوجد coordinator أو lock داخل Core يثبت ذلك تلقائياً.
5. **Connection binding:** ConnectionAwareSessionCleanupService يمنع إزالة session طالما ConnectionSessionRegistry يثبت وجود binding؛ يجب استخدام نفس registry في ingress وcleanup.
6. **Capacity release:** إزالة session بعد drain تطلق tracked-session capacity وتسمح ببدء sequence جديد لجلسة جديدة.

## تغطية الاختبارات

- FullQueue_RejectsWithoutConsumingSequence.
- DuplicateSequence_IsRejected وExcessiveSequenceJump_IsRejectedWithoutQueueMutation.
- ResetSession_IsRejectedWhileCommandsRemainQueued وResetSession_ReleasesCapacityAfterQueueDrains.
- TryRemoveClosedSession_RejectsWhileCommandsRemainQueued وTryRemoveClosedSession_RemovesAfterQueueDrainAndReleasesReplayCapacity.
- Cleanup_IsBlockedUntilClosedSessionIsDetachedFromTransportConnection وCleanup_CanRemoveDisconnectedSessionAfterItIsClosedAndDetached.

## حدود التنفيذ

- كل الخدمات موصوفة single-writer/server-serialized؛ لا thread safety أو distributed consistency.
- replay state في الذاكرة ومرتبط بعمر Session؛ لا durable idempotency عبر restart.
- لا transport ingress/egress أو in-flight handler coordinator أو host shutdown protocol داخل Core.
- آخر تشغيل Core موثق 246/246 على SHA bfbed74ae24e8e5e49baff84e7a18b0e99b55807؛ لم يُشغّل الاختبار في هذه المرحلة.

## القرار

- KEEP_LIVE لسلوك queue/cleanup الحالي والاختبارات.
- لا تعديل code؛ لا تغيير سياسة reconnect أو multi-session أو persistence.
- تظل مسؤوليات host المستقبلية واضحة قبل تكامل transport.

**الحالة:** STAGE 61 COMPLETE — QUEUE BACKPRESSURE AND SESSION CLEANUP CONTRACTS VERIFIED BY SOURCE/TEST REVIEW.
