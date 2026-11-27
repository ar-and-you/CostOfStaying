# المرحلة 55 — تدقيق قابلية تعديل حالة Domain وحدود الاحتفاظ (2026-11-27)

## الرأس والنطاق

- HEAD قبل المرحلة: 6ea35a7d8edb2fbb22ad161eb8fc614d74064454.
- النطاق: D-03 (public mutable collections) وD-04 (unbounded event/id stores) في DOMAIN_DEFECT_REVIEW.
- تمت مراجعة Match.cs وPlayer.cs ومشروعي Domain/Core ومشروع اختبارات Domain، مع بحث استعمالات الخصائص في الشجرة.
- مراجعة ساكنة فقط؛ لا تعديل source أو API.

## D-03 — مجموعات عامة قابلة للتعديل

الأسطح الحالية:
- Match.Players: Dictionary<Guid, Player>.
- Match.Events: List<DomainEvent>.
- Match.ProcessedRequestIds: HashSet<string>.
- Match.Votes: Dictionary<Guid, VoteChoice>.
- Player.Debuffs: List<Debuff>.
- Player.CollectedEchoIds: HashSet<string>.

المخاطر:
- إدخال/حذف Player خارج AddPlayer قد يتجاوز سعة roster وحالة lifecycle.
- تعديل Votes مباشرة قد يغير نتيجة الإجماع دون CastVote.
- تعديل Events يتيح حذف/تزوير سجل المجال.
- حذف RequestId سابق قد يعيد فتح replay؛ إضافة معرفات زائفة قد ترفض طلبات مشروعة.
- تعديل Debuffs/Echo IDs يجاوز قواعد النقل والسعة ومنع جمع Echo مرتين.

الاستهلاك المرصود:
- لا runtime consumer أو Unity integration في الشجرة.
- Core لا يعتمد على Domain عبر ProjectReference.
- الاستخدامات المباشرة في source داخل aggregate؛ الاختبارات تقرأ المجموعات وتعدل Debuffs كـfixtures، وتستخدم InternalsVisibleTo للوصول إلى setters الداخلية.
- غياب مستهلك حي يقلل مخاطر الترحيل الآن، لكنه لا يجعل API العام آمناً لمستهلك خارجي.

المعالجة المقترحة:
1. إبقاء backing Dictionary/List/HashSet خاصة.
2. تعريض Players/Votes كـIReadOnlyDictionary مع ReadOnlyDictionary فعلي، وEvents/Debuffs كـReadOnlyCollection.
3. جعل ProcessedRequestIds وCollectedEchoIds داخلية، وتوفير count/query methods عند الحاجة.
4. نقل إعداد fixtures إلى internal methods/builders في test assembly؛ لا تترك mutators عامة لأجل الاختبارات.
5. اختبار عدم إمكان تعديل أو cast واجهات القراءة إلى backing collections.
6. تنفيذ التغيير قبل Unity integration أو نشر API، مع توثيق كسر التوافق.

**القرار:** D-03 يبقى OPEN_OWNER_DECISION؛ لا تعديل API في هذه المرحلة.

## D-04 — نمو مجموعات الذاكرة

- Events تنمو مع كل Emit، وتشمل أحداث الرفض.
- ProcessedRequestIds تنمو مع كل نقل ناجح وتبقى طوال عمر Match.
- CollectedEchoIds تنمو مع كل echoId فريد لكل لاعب.
- requestId وhintText وechoId لا تملك حدود طول موثقة في العقد الحي.
- لا توجد rate limiter أو transport/server runtime؛ الخطر متعلق بتصميم السعة والتكامل المقبل، لا بإثبات استغلال شبكي قائم.

لا يمكن اختيار TTL أو cap من دون سياسة:
- إزالة RequestId مبكراً قد تعيد فتح replay.
- تقليم Events قد يفقد أدلة التدقيق ما لم يوجد sink دائم.
- حد Echo/Hints/ID يحتاج عقد لعب وخادم وقياسات workload.
- Match time limit لا يحدد وحده معدل الأوامر أو حجم event log.

المطلوب من قرار المالك/التصميم قبل التنفيذ:
1. حد مدة المباراة ومعدل/عدد عمليات Transfer وEcho/Hint.
2. سياسة حجم requestId/echoId/hintText.
3. هل Events سجل كامل أم نافذة تشغيلية؟ وأين يحفظ الأرشيف الدائم؟
4. مدة idempotency وهل تمتد بعد restart/استعادة المباراة؟
5. سياسة رفض عند بلوغ الحد، لا إسقاط صامت للأحداث أو مفاتيح replay.

## تحديثات مرتبطة

- المرحلة 45 أغلقت body/assertion crosswalk لستة ملفات Core.Tests الأرشيفية.
- المرحلة 48 أضافت تغطية صريحة لمنع Spirit من التصويت، وتقوية Transfer atomicity، والتحقق من هوية Debuff المنتهية.
- المرحلة 52 أضافت workflow hygiene tests لثلاثة workflows متتبعة.
- المرحلة 53 سجلت 62/62 Domain و246/246 Core وDocumentation Governance success على SHA محدد؛ لا تنقل النتيجة إلى HEAD لاحق.
- المرحلة 54 وثقت تعذر قراءة Branch Protection/alert endpoints؛ الحالات تبقى UNKNOWN لا OFF.

## القرار النهائي للمرحلة

- لا تعديل source أو public API أو سياسة الاحتفاظ.
- لا استيراد ملفات أرشيفية ولا حذف/نقل.
- تسجيل D-03 وD-04 كقرارات مطلوبة قبل تكامل Unity/server أو نشر API.
- لم تُشغّل اختبارات جديدة في هذه المرحلة.

**الحالة:** STAGE 55 COMPLETE — MUTABILITY AND RETENTION RISKS MAPPED; API/RETENTION OWNER DECISIONS REQUIRED BEFORE HARDENING.
