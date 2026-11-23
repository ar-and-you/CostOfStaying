# مسرد CostOfStaying

| المصطلح | التعريف المرجعي |
|---|---|
| Alive | لاعب حي داخل المباراة وفق `PlayerState.Alive`. |
| Backend | خدمات خارج خادم المباراة مثل الحسابات والمطابقة والحفظ؛ المزود غير مختار. |
| Betrayal | كلفة تضاف إلى مصدر كل نقل ناجح وفق الخط المقترح؛ ليست نقاط فوز. عتباتها غير معتمدة. |
| Collapse | حدث لعب يرفع معدل تآكل Vitality ويضيف Debuff إذا توفرت سعة؛ لا يقتل مباشرة. |
| Core | مشروع .NET منفصل يوفر حالياً عقوداً وخدمات للأوامر والجلسات والهوية والتسجيل. ليس هو Domain gameplay. |
| Dedicated Server | خادم مستقل مخطط ليملك حالة المباراة؛ لا يوجد خادم قابل للتشغيل حالياً. |
| Debuff | عبء/تأثير نشط قد يضيف إلى معدل التآكل؛ خصائص التكديس والنقل النهائية تخضع للمراجعة. |
| Domain | مشروع `CostOfStaying.Domain` الحالي؛ يحتوي `Match`, `Player`, `MatchConfig` ومنطق اللعبة. |
| Echo Fragment | عنصر مقترح لدور Spirit، يخضع لتصميم واختبار إضافيين. |
| Eliminated | قيمة في `PlayerLifecycleState` المستقل داخل Core؛ ليست قيمة في `PlayerState` الخاص بـDomain. وجودها في Core لا يحسم اعتمادها كحالة منتج موحدة. |
| MatchState | دورة حياة المباراة؛ منفصلة عن حالة اللاعب. |
| PlayerLifecycleState | enum مستقل في Core يتضمن Alive, Spirit, Eliminated، مع عمليات إحياء/إقصاء في primitive. لا تخلطه بنموذج Domain أو تعتبره قراراً موحداً للمنتج. |
| PlayerState | enum مشروع Domain: Connecting, Alive, Dead, Spirit, Disconnected, Spectating. مسار Vitality=0 الحالي ينتقل مباشرة من Alive إلى Spirit؛ وجود Dead في enum لا يعني أنه انتقال runtime فعّال. |
| Spirit | حالة يضبطها Domain عند وصول Vitality إلى صفر؛ ليست خروجاً فورياً من المباراة. |
| Suffering Transfer | نقل كامل لعبء قابل للنقل من مصدر إلى هدف؛ القواعد والقيم ما زالت للاختبار. |
| TBD | لم يُحسم. |
| VERIFIED | نتيجة تحقق مباشر مربوطة بـSHA وبيئة وأمر/تشغيل ودليل. |
| Vitality | المورد الذي يتآكل بمرور الوقت؛ يبدأ في مواصفة الاختبار من 100. |
| Witness Score | مقياس مقترح لتقدم Spirit؛ لا يحدد الفائز الحي. |
