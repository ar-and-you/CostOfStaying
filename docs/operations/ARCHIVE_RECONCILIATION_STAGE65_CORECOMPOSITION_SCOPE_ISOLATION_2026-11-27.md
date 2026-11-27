# المرحلة 65 — التحقق من عزل match scope في CoreComposition (2026-11-27)

## الرأس والنطاق

- HEAD قبل المرحلة: c4de4483ab6410b0e61c179ca6a388601937d7f0.
- تمت مراجعة CoreComposition وBetrayalLedger وBetrayalEvent واختبارات CoreCompositionTests.
- الهدف: التحقق من أن ledger فعلاً match-scoped، لا مجرد وصف XML.

## النتيجة

- CoreComposition يصف نفسه بأنه one instance per match scope، وينشئ BetrayalLedger واحداً داخل constructor.
- Create() لا يأخذ MatchId؛ ويمكن للinstance نفسه إنشاء أكثر من MatchLifecycle وMatchPlayerRegistry لمعرفات مختلفة.
- BetrayalEvent لا يحمل MatchId؛ الترتيب يعتمد EventSequenceId فقط، والدرجات تجمع حسب ActorId.
- لذلك إذا أعاد host استخدام composition واحد لأكثر من match، فسيشترك ledger بين هذه المباريات وقد تتداخل sequence IDs والدرجات.
- CoreCompositionTests تثبت أن compositionين منفصلين يملكان ledgerين منفصلين، لكنها لا تفرض instance-per-match داخل API.

## المعالجة المطلوبة قبل server integration

1. إما ربط CoreComposition بMatchId واحد عند الإنشاء ورفض إنشاء موارد match-scoped بمعرف مختلف.
2. أو نقل BetrayalLedger إلى Match-scoped object مستقل يحمل MatchId، مع تضمين MatchId في event identity/persistence.
3. إضافة اختبار يمنع مشاركة ledger بين MatchIds ضمن نفس composition.
4. عدم تنفيذ الخيار قبل قرار API/architecture واضح؛ لا نضيف كسر توافق تلقائياً.

## القرار

- KEEP_CURRENT_API مؤقتاً مع توثيق أن instance-per-match مسؤولية host.
- إضافة caveat إلى DOMAIN_CORE وOPEN_DECISION_REGISTER وتقرير المرحلة 64.
- لا تغيير source أو tests أو dependencies.
- هذا خطر تكامل محتمل، لا دليل على cross-match contamination في runtime حالي لأن لا server host موجود.

**الحالة:** STAGE 65 COMPLETE — MATCH-SCOPE ISOLATION IS A HOST CONTRACT, NOT ENFORCED BY CURRENT CORE API.
