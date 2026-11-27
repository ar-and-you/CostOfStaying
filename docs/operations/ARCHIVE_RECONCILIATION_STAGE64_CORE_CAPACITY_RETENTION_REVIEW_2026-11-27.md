# المرحلة 64 — مراجعة حدود السعة والاحتفاظ في Core (2026-11-27)

## الرأس والنطاق

- HEAD قبل المرحلة: 52275a566d45ac8096aff2f906d9c3ffcfc48bab.
- تمت مراجعة CoreComposition وSessionRegistry وMatchPlayerRegistry وConnectionSessionRegistry وCommandAdmissionQueue وCommandSequenceTracker وBetrayalLedger واختباراتها.
- الهدف: تمييز المخازن المحدودة من المخازن غير المحدودة؛ لا تغيير source أو cap.

## المخازن ذات السعة المحددة

| المخزن | الحد | التحرير |
|---|---|---|
| SessionRegistry | capacity صريح | إزالة Closed بعد drain/reset |
| MatchPlayerRegistry | capacity صريح | Remove |
| ConnectionSessionRegistry | capacity صريح | TryDetach |
| CommandAdmissionQueue | capacity صريح | TryDequeue |
| CommandSequenceTracker | maximumTrackedSessions + maximumSequenceJump | ResetSession |

لا توجد defaults تشغيلية؛ host يحدد القيم عند الإنشاء. كل هذه الأنواع single-writer/server-serialized.

## المخازن غير المحدودة

1. BetrayalLedger._events: Dictionary لكل EventSequenceId/حدث؛ لا capacity أو eviction أو clear API.
2. BetrayalLedger._scores: Dictionary لكل ActorId مسجل؛ لا سقف مستقل ولا remove/reset API.
3. Domain Match.Events / ProcessedRequestIds / Player.CollectedEchoIds: موثقة في D-04؛ لا TTL أو cap.
4. Command payloads: Core generic TCommand لا يفرض حجم object أو raw frame limits.
5. Global match count: CoreComposition ينشأ لكل match scope ولا يوجد global registry/limit لعدد المباريات النشطة.

## أثر BetrayalLedger

- TryRecord يتحقق من EventSequenceId وActorId وVictimId وعدم self-betrayal، ويرفض sequence reuse غير المطابق.
- لا يتحقق من أن ActorId/VictimId أعضاء في MatchPlayerRegistry؛ هذا واجب host/authority layer قبل التسجيل.
- يحتفظ بالأحداث القديمة لتمييز exact replay (يرجع false) عن إعادة استخدام sequence مع payload مختلف (يرمي InvalidOperationException).
- أي eviction يغير replay semantics؛ لا ينفذ دون قرار retention واضح.

## Scope isolation finding

CoreComposition يوصي بإنشاء instance لكل match، لكنه لا يفرض ذلك: Create() بلا MatchId، ويمكن للinstance إنشاء أكثر من MatchLifecycle وMatchPlayerRegistry بمعرفات مختلفة، بينما BetrayalLedger واحد مشترك ولا يحمل BetrayalEvent أي MatchId. الاختبارات تثبت فقط أن compositionين منفصلين يملكان ledger منفصلين؛ لا تمنع مشاركة composition واحد بين matches. هذا قد يخلط sequence IDs وscores بين المباريات إذا خالف host العقد.

## القرار

- D-04 يتسع ليشمل Core BetrayalLedger إضافة إلى Domain collections.
- لا cap رقمي أو TTL أو eviction policy دون workload/مدة مباراة/سجل دائم/قرار مالك.
- قبل production host يجب تحديد active-match cap، payload size quotas، roster/authority validation، event persistence/retention، وسياسة cleanup عند نهاية match.
- لا تغيير code أو tests أو dependencies.
- لم تُشغّل اختبارات جديدة؛ آخر Core CI ناجح على SHA d9c4e93 كان 248/248.

**الحالة:** STAGE 64 COMPLETE — CORE CAPACITY BOUNDS MAPPED; BETRAYAL LEDGER AND HOST-LEVEL LIMITS REMAIN OPEN.
