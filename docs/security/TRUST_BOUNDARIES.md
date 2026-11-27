# حدود الثقة — CostOfStaying Core

**الحالة: BASELINE DOCUMENTED / HOST BOUNDARY NOT IMPLEMENTED.**  
**النطاق:** وصف التنفيذ الحي في Core، والالتزامات المطلوبة من أي مضيف مستقبلي. هذه الوثيقة لا تثبت وجود عميل متصل أو Transport أو Dedicated Server أو مصادقة Credentials في `main`.

## 1. قواعد حاكمة

1. العميل والجهاز والشبكة ومدخلات النقل غير موثوقة.
2. لا يقرر العميل نتائج اللعب أو تغييرات الحالة؛ يرسل نوايا/أوامر فقط.
3. لا تُعد هوية Actor موثوقة إلا بعد أن يحلها مضيف موثوق من جلسة مصادق عليها. Core الحالي لا يصادق Credentials بنفسه.
4. لا يُقبل أمر حساس إلا بسياسة Gameplay صريحة ومحددة لنوع الأمر. غياب السياسة أو فشل تهيئتها يجب أن يمنع تشغيل مسار الأمر؛ لا توجد سياسة سماح افتراضية. مصانع admission الأدنى التي لا تطبق Gameplay policy أصبحت internal وليست public ingress.
5. لا يثبت اجتياز بوابة Admission وحده أن الفعل نُفّذ أو أن أثره مشروع؛ يجب على معالج Gameplay الموثوق إعادة التحقق من شروط الفعل قبل تغيير الحالة.
6. يجب أن تكون كل حالة وسجل Match-scoped معزولة عن المباريات الأخرى. أصبح `CoreComposition` يتطلب `MatchId` صالحاً ويربط به ledger وMatchLifecycle وMatchPlayerRegistry وVitalityAccount وCommandAdmissionQueue والـmatch services؛ اجتازت اختبارات Core على فرع المراجعة، والإغلاق على main ينتظر المراجعة والدمج.
7. لا يُعتمد مزود خارجي أو يُدخل إلى الحل قبل إغلاق F-001 وF-002 بأدلة واختبارات.

## 2. مناطق الثقة

| المنطقة | مستوى الثقة | المسؤولية/الافتراض |
|---|---|---|
| Client device / game client | غير موثوق | يرسل intent فقط؛ يمكن تعديله أو إعادة تشغيله أو تزوير payloads. |
| Public network / transport | غير موثوق | افترض replay وdelay وreorder وdisconnect وoversized frames؛ لا Transport فعلي في main. |
| Host ingress / credential validation | غير منفذ | يتحقق من credentials، ويربط account بهوية Actor موثوقة، ويضع حدود frame/serialization/rate/TTL قبل Core. |
| Core session and command services | منفذ جزئياً | يتحقق من حالة الجلسة وربط Actor والعضوية والتسلسل والسعة وسياسة التفويض المقدمة من التطبيق. لا يصادق credentials ولا يثبت مصدر ReceivedAt/ServerTick. |
| Gameplay handler / state owner | غير منفذ كطبقة متكاملة | يجب أن يملك قرار تنفيذ الفعل وتغيير الحالة، ويتحقق من قواعد اللعب والعضوية والسلطة على الأطراف قبل mutation أو event recording. |
| Match runtime state | Domain منفذ جزئياً؛ Core primitives منفصلة | يجب أن تكون مملوكة لمباراة واحدة؛ لا يوجد حالياً دمج Domain/Core أو host ينسق دورة الحياة. |
| Betrayal ledger | primitive منفذ داخل Core | يتحقق من صحة الحقول وتسلسل الحدث وexact replay؛ واجهته العامة أصبحت read-only، وكتابة الأحداث assembly-internal. لا يتحقق من عضوية Actor/Victim أو سلطة الفعل داخل primitive. |
| Persistence / backend | غير منفذ | لا قاعدة بيانات أو مخزن دائم أو استعادة أو مزود معتمد. |
| Repository / CI | سطح حساس | تحكم الوصول والأسرار وسلسلة التوريد مسؤوليات تشغيلية مستقلة؛ لا يُفترض خلو التنبيهات دون قراءة موثوقة. |

## 3. ملكية المكونات والتحقق

| المكوّن | ما يملكه/يديره | ما يتحقق منه حالياً | ما لا يضمنه |
|---|---|---|---|
| Host (مستقبلي) | credential lifecycle، account→Actor mapping، transport timestamps، frame limits، rate limits، connection lifecycle | يجب أن يتحقق من مصدر الهوية وصحة النقل قبل إنشاء envelope | لا يوجد تنفيذ في main؛ لا يجوز وصفه كضابط قائم. |
| Session lifecycle/registry | جلسات Core وسعتها وحالتها وربط Actor بعد المصادقة | صلاحية SessionId والحالة وActorId في المسارات التي تستخدمها | لا يتحقق من كلمة مرور أو token أو هوية خارجية. |
| MatchPlayerRegistry | roster لمباراة واحدة | وجود Actor ضمن roster وسعة التسجيل، ورفض VitalityAccount المنتمي إلى MatchId آخر | لا يقرر وحده صلاحية كل فعل أو حالة اللاعب. |
| Command envelope/queue | command payload reference، sequence/admission، queue capacity، MatchId الخاص بالطابور | envelope والـsequence والحدود المحددة للطابور؛ queue يحمل MatchId وتتحقق factories من تطابق scope | لا يثبت سلامة payload من ناحية Gameplay ولا ينسخ/يجمد DTO. |
| Payload validation service | تمرير payload validator قبل/بعد مسار التفويض وفق تركيب الخدمة | نتيجة validator المقدمة من التطبيق | لا يفرض raw frame/serializer quotas ولا يضمن immutability للـTCommand. |
| MatchCommandAuthorizationService<T> | بوابة تفويض عامة لنوع T | session→Actor، roster، phase/player context، policy قبل admission وبعد dequeue | لا يوفر سياسات Gameplay فعلية تلقائياً ولا يضمن أن كل command type في المنتج مسجل. مصانع admission-only أصبحت internal لمنع public bypass. |
| Gameplay policy<T> | قرار سماح/رفض لنوع أمر محدد؛ يملكها التطبيق/المضيف | يجب أن تكون pure وside-effect-free وأن ترفض الحالات غير المصرح بها | لا توجد حالياً سياسات Gameplay إنتاجية معتمدة أو أنواع أوامر Gameplay ملموسة في Core. |
| Gameplay handler (مستقبلي) | تنفيذ الفعل وامتلاك mutation | يجب أن يعيد التحقق من preconditions والعضوية والسلطة والحالة الحالية | لا يوجد handler متكامل موصول بطابور Core في main. |
| BetrayalLedger | سجل الأحداث والدرجات داخل عمر instance | event validity، actor/victim non-zero، عدم self-betrayal، sequence monotonicity، exact replay | لا يتحقق من roster membership أو authorization؛ لا retention/persistence؛ single-writer فقط. |
| CoreComposition | إنشاء Core services ويمتلك ledger واحداً لمباراة واحدة | يفرض `Create(MatchId)` صالحاً؛ MatchLifecycle وMatchPlayerRegistry وVitalityAccount وCommandAdmissionQueue وmatch services تُنشأ/تُقبل فقط ضمن MatchId نفسه | لا يفرض global active-match cap؛ host مسؤول عن إنشاء composition مستقل لكل مباراة وإدارة دورة حياته. |
| Domain Match | gameplay aggregate الحالي: Vitality/Collapse/Transfer/Vote/Spirit/events | قواعد المجال داخل aggregate وفق API الحالي | ليس Core host ولا transport؛ بعض المدخلات مثل distance يقدمها caller؛ لا ضمان شبكي. |

## 4. سلسلة الأمر وحدودها الحالية

السلسلة المقصودة:

`Untrusted transport input → host credential/actor resolution → bounded deserialization → typed command envelope → payload validation → session/roster checks → explicit command policy → bounded admission/sequence → dequeue revalidation → trusted gameplay handler → state mutation / domain event`

الحالة الفعلية:
- لا يوجد transport ingress أو credential provider أو Dedicated Server في main.
- Core يملك primitives للجلسات والعضوية والـqueue والـpayload validation والـauthorization.
- `MatchCommandAuthorizationService<TCommand>` يتطلب policy غير null عند الإنشاء، ويعيد التحقق قبل القبول وبعد dequeue. هذا fail-closed على مستوى instance، لكنه ليس registry يثبت اكتمال سياسات جميع أنواع الأوامر.
- لا توجد concrete Gameplay command DTOs أو سياسات لعب إنتاجية معتمدة أو handlers تربط Core بـDomain. راجع [جرد الأوامر الحساسة وسياسات التفويض](SENSITIVE_COMMAND_POLICY_CATALOG.md)؛ كل mutation معروفة مصنفة مع TBD صريح.
- يجب أن تكون DTOs immutable أو تنتقل ملكيتها حصرياً للمضيف بعد handoff؛ validator لا يجمد الكائن.
- حدود frame size وdecompression وserializer recursion والـrate limiting تقع قبل Core في host/transport.

## 5. Betrayal: فصل المسؤوليات

- `BetrayalLedger` مسؤول عن integrity محلية للسجل: event validity، التسلسل، كشف exact replay، وحساب score.
- Membership مسؤولية roster/match context.
- Authorization policy مسؤولة عن حق Actor في طلب الفعل في phase/state محددة.
- Trusted Gameplay Handler مسؤول عن التحقق النهائي من Actor وVictim والسلطة والـpreconditions ثم إصدار الحدث.
- لا يكفي مرور الأمر عبر `MatchCommandAuthorizationService` لإثبات أن التسجيل جاء من handler موثوق.
- `CoreComposition.BetrayalLedger` يكشف الآن واجهة قراءة عامة فقط؛ أزيل `TryRecord` من `IBetrayalLedger` وأصبح method داخلياً على `BetrayalLedger` لمنع المستهلك الخارجي من التسجيل المباشر أو الوصول إليه عبر cast.
- `VictimId` و`ActorId` نوعان منفصلان، ولا يوجد حالياً mapping contract بينهما؛ لذلك لا يمكن ادعاء التحقق من عضوية Victim في `MatchPlayerRegistry` قبل حسم هذا الربط.
- هذا تخفيف فعلي لمسار التجاوز العام، لكنه لا يثبت وجود Trusted Gameplay Handler ولا يضيف membership/authority validation؛ لذلك تبقى F-002 مفتوحة.
- لا تُضف retention أو eviction قبل قرار عمر المباراة ودلالات replay والاستعادة.

## 6. Match isolation ودورة الحياة

**الوضع الحالي بعد تعديل F-004 على فرع العمل:** `CoreComposition.Create(MatchId)` يتطلب معرفاً صالحاً. `VitalityAccount` و`CommandAdmissionQueue` يحملان MatchId غير قابل للتغيير، ويرفض `MatchPlayerRegistry.Register` حساباً من مباراة أخرى. `CreateMatchLifecycle()` و`CreateMatchPlayerRegistry(capacity)` وmatch service factories تستخدم/تتحقق من MatchId الخاص بالتركيب. لا يمكن استخدام composition واحد أو queue/account واحد لتبادل حالة بين MatchIds عبر API العامة. اجتاز Core CI عدد 256/256 اختباراً على الرأس `289affbd14970b7c374fce3e9a861bd06e7c78fa`؛ الدمج في main لم يحدث بعد.

**حالة الإغلاق:** متطلبات ربط composition والـledger وموارد Match-scoped بـ`MatchId` ورفض تمرير موارد من مباراة أخرى نُفذت على هذا الفرع، مع اختبارات العزل المشار إليها أعلاه. لا تعِد هذه الفقرة فتح المتطلبات المنفذة؛ ما يزال على المضيف إدارة عدد المباريات النشطة وعمر كل composition. أما تصنيف موارد Host-scoped وSession-scoped ودورة حياتها التشغيلية فيظل جزءاً من تصميم المضيف، ولا يغير ضمان العزل البنيوي الذي يقدمه Core.

**سيناريو المنتج غير محسوم:** لم يعتمد مالك المشروع حتى الآن مدة المباراة، الحد الأعلى للاعبين، معدل الأحداث المتوقع، أو سياسة الاستئناف بعد الانقطاع/إعادة تشغيل الخادم. لا تفترض الوثيقة مباراة قصيرة أو طويلة، ولا تحدد أرقاماً من تلقاء نفسها. تبقى هذه قرارات مالك مفتوحة وتمنع اختيار retention/snapshot policy.

## 7. البيانات والحفظ

| الفئة | أمثلة | الحالة/المسؤولية |
|---|---|---|
| Match runtime | Vitality، Debuffs، timers، votes، roster | يفترض أن تكون في ذاكرة مضيف المباراة؛ لا خادم منشور ولا persistence منفذ. |
| Match result | النتيجة، المدة، إصدار القواعد | مرشح للحفظ بعد قرار المنتج والخصوصية؛ لا مخزن معتمد. |
| Session/transport | session state، connection binding، queue، sequence | مؤقتة داخل Core؛ لا persistence ولا استعادة بعد restart. |
| Betrayal history | events وscores | ذاكرة غير محدودة داخل ledger؛ exact replay يعتمد على event history؛ لا تقليم أو حذف تلقائي. |
| Account / identity | account mapping وcredentials | غير منفذ؛ لا تخزن أسرار العميل داخل Core. |
| Operational/security telemetry | crash، authorization failures، replay | سياسة جمع/وصول/احتفاظ مطلوبة قبل الإنتاج. |

## 8. قرارات وبوابات

1. **Deny-by-default:** اتجاه معماري معتمد من مالك المشروع: كل نوع أمر حساس يحتاج policy صريحة؛ لا permissive fallback؛ غياب الربط يمنع تهيئة/تشغيل الأمر. التنفيذ الحالي يفرض non-null policy لكل service instance لكنه لا يثبت completeness على مستوى التطبيق.
2. **F-001/F-002:** لا مزود خارجي أو خدمة هوية/تخزين قبل إغلاقهما بأدلة واختبارات.
3. **Match lifecycle:** مدة المباراة، عدد اللاعبين، الاستئناف، ونهاية عمر الموارد تحتاج قرار مالك قبل حسم F-003.
4. **ADR:** هذه الوثيقة تسجل baseline وحدود التنفيذ؛ لا تستبدل ADR مقبولاً ولا تحول قواعد اللعب المقترحة إلى قرارات معتمدة.
5. **Owner decisions:** تبقى قرارات المنتج غير المحددة مفتوحة؛ توثيقها صراحة أفضل من اختراع قيم أو افتراضات.

## 9. معايير الإغلاق الأمني

- **Authorization Integrity:** كل أمر حساس معروف ومسجل بسياسة صريحة restrictive، مع اختبارات allow/deny ورفض غياب التسجيل.
- **Betrayal Integrity:** كل تسجيل يصدر من handler موثوق بعد التحقق من هوية وعضوية الطرفين وسلطة الفعل؛ لا وصول جانبي مباشر.
- **Match Isolation:** كل حالة Match-scoped مربوطة بمباراة واحدة ومختبرة ضد cross-match access.
- **State Ownership:** لا mutation حساسة خارج المالك/handler المعتمد.
- **Host Boundary:** لا ادعاء بأمن الشبكة أو المصادقة قبل تنفيذ host/transport واختبارهما.
- كل إغلاق مرتبط بـcommit SHA وملفات واختبارات منفذة فعلياً؛ الوثائق وحدها لا تغلق Findings تنفيذية.
