# المرحلة 56 — توضيح دلالات التصويت وCollapse catch-up (2026-11-27)

## الرأس والنطاق

- HEAD قبل المرحلة: 4e33dae713600ea2476096d20982f44992bad3e2.
- النطاق: D-05 (الموت/الانقطاع أثناء التصويت) وD-06 (تأخر Tick وCollapse catch-up).
- تمت مراجعة Match.Tick وOpenVote وCastVote وCloseVote، واختبارات VoteAndEndgame، ووثيقتي ENDGAME_SYSTEM وCOLLAPSE_SYSTEM.
- تعديل تعليق/توثيق فقط؛ لا تغيير منطق اللعبة أو نتيجة التصويت.

## D-05 — التصويت

- OpenVote يأخذ snapshot للاعبين Alive ويضع Abstain افتراضياً.
- Votes لا تتغير تلقائياً عند انتقال اللاعب إلى Spirit؛ CastVote يرفض صوتاً جديداً من غير Alive، لكن الصوت السابق يبقى.
- CloseVote ينجح فقط إذا كانت كل قيم snapshot Yes؛ Abstain/timeout يمنعان الإجماع.
- لا يوجد حدث disconnect/reconnect أو تكامل transport في Domain.
- كان تعليق CloseVote يعلن disconnect=No بلا تنفيذ يثبت ذلك؛ صُحح ليقول إن سياسة disconnect غير منفذة.
- أضيف شرح التنفيذ الحالي إلى ENDGAME_SYSTEM مع الإبقاء على قرار أثر الموت/الانقطاع OPEN.

**قرار:** لا تغيير سلوك أو استيراد حكم الأرشيف disconnect=No؛ يلزم قرار مالك/شبكة قبل اعتماد السياسة.

## D-06 — Collapse catch-up

- Tick يفحص كل لاعب Alive ويطبق Collapse واحداً كحد أقصى في الاستدعاء.
- عند التأخر، ApplyCollapse يضبط الموعد التالي إلى now + interval؛ لا يعيد عدة فترات فائتة.
- أضيف هذا الوصف إلى COLLAPSE_SYSTEM، مع فصل حقيقة التنفيذ عن القرار التصميمي.
- الخيارات (skip، bounded catch-up، replay) لها آثار مختلفة على Vitality والمدة والتزامن؛ لا اختيار تلقائي.

**قرار:** لا تغيير خوارزمية؛ تبقى سياسة catch-up OPEN إلى أن يعتمد مالك المنتج سلوك lag.

## التغييرات

- تصحيح تعليقين في Match.CloseVote لمنع ادعاء disconnect handling غير الموجود.
- تحديث ENDGAME_SYSTEM وCOLLAPSE_SYSTEM وDOMAIN_DEFECT_REVIEW.
- لا تعديل tests أو قواعد اللعب أو التوازن أو النقل.
- لا اختبارات محلية؛ يجب انتظار CI على SHA الناتج قبل وصف build بأنه ناجح.

**الحالة:** STAGE 56 COMPLETE — CURRENT VOTE/TICK BEHAVIOR DOCUMENTED; PRODUCT SEMANTICS REMAIN OPEN.
