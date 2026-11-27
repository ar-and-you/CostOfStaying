# المرحلة 62 — تقوية اختبارات فشل التفويض والتحقق بعد dequeue (2026-11-27)

## الرأس والنطاق

- HEAD قبل المرحلة: 0a2f84ef9f7981d295f600c33c7a3fe5bf06d126.
- النطاق: MatchCommandAuthorizationService وMatchCommandPayloadValidationService.
- الهدف: إثبات fail-closed عند استثناء policy/validator في مرحلة post-dequeue، وتوثيق ملكية payload.
- تغييرات source comments/tests فقط؛ لا تغيير في قواعد التفويض أو queue semantics.

## التغييرات

1. أضيف اختبار TryTakeNext_PolicyExceptionFailsClosedAndKeepsAcceptedSequenceConsumed:
   - admission الأول ينجح.
   - policy ترمي exception عند إعادة التقييم بعد dequeue.
   - الخدمة ترجع Unavailable، ويُفرغ queue.
   - إعادة نفس sequence تُرفض Conflict؛ sequence الذي قُبل سابقاً يظل مستهلكاً.
2. أضيف اختبار TryTakeNext_ValidatorExceptionFailsClosedAndKeepsAcceptedSequenceConsumed:
   - payload يمر بالتحقق قبل admission.
   - validator يرمي exception بعد dequeue.
   - الخدمة ترجع Unavailable، ولا يُسلّم command إلى handler.
   - إعادة نفس sequence تُرفض Conflict.
3. صُحح XML comment في MatchCommandPayloadValidationService: إعادة التحقق تكشف mutation أثناء الانتظار، لكنها لا تنسخ أو تجمد TCommand. يجب على host استخدام immutable DTO أو نقل ownership حصرياً ومنع التعديل بعد handoff.
4. أضيف هذا القيد إلى OPEN_DECISION_REGISTER؛ raw frame/serializer quotas تظل مسؤولية transport قبل deserialization.

## الحدود

- لا سياسة gameplay جديدة ولا permissive default.
- policy/validator يجب أن يبقيا bounded, synchronous, deterministic, side-effect-free, no I/O.
- الخدمات single-writer/server-serialized؛ لا thread safety أو distributed guarantees.
- لم يُشغّل .NET محلياً؛ تم التحقق عبر Domain CI وCore CI وDocumentation Governance على SHA الناتج؛ CodeQL C# بقي in_progress عند آخر قراءة.

**الحالة:** STAGE 62 COMPLETE — POST-DEQUEUE EXCEPTION PATHS NOW HAVE REGRESSION COVERAGE; PAYLOAD OWNERSHIP CONTRACT CLARIFIED.
