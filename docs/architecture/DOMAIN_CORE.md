# Domain وCore — الحالة الفعلية وحدود المسؤولية

**الحالة:** وصف للفرع الحي، وليس ADR أو قراراً بأن هذا الحد معماري نهائي.

## 1. ما يوجد في `main`

| المسار | المسؤولية | الاستهداف |
|---|---|---|
| `src/CostOfStaying.Domain/` | مجمّع المباراة واللاعب وقواعد Vitality/Collapse/Transfer/Vote/Spirit والنتائج | `netstandard2.1` |
| `src/CostOfStaying.Core/` | عقود وخدمات admission/authorization/validation، هوية وتسلسل الأوامر، سجل اللاعبين والجلسات وتنظيفها | `netstandard2.1` |
| `tests/CostOfStaying.Domain.Tests/` | اختبارات Domain | `.NET 8` |
| `tests/CostOfStaying.Core.Tests/` | اختبارات Core | `.NET 10` عبر `global.json` محلي |

المشروعان منفصلان. Core لا يحتوي حالياً `Match` gameplay aggregate من Domain، ولا توجد هجرة كاملة. لا تنفذ النقل أو توحيد المشروعين اعتماداً على وثيقة أرشيفية.

## 2. ملاحظات تنفيذ موثقة

- Domain يستخدم `IMatchClock` وساعة قابلة للتحكم في الاختبارات.
- `Match` يحمل `Guid` للمباراة واللاعبين، وحالة `MatchState` و`PlayerState`.
- `PlayerState` enum يحتوي Connecting وAlive وDead وSpirit وDisconnected وSpectating.
- عندما تصل Vitality إلى صفر، `TransitionToSpirit` يغيّر الحالة مباشرة من Alive إلى Spirit. لا يوجد مسار active يضع PlayerState.Dead؛ لكن قيمة Dead ما زالت موجودة وتدخل في شرط `hasDeath` في نهاية المباراة.
- لا توجد قيمة `Eliminated` ضمن enum الحالي على `main`، رغم ظهورها في بعض سجلات الأرشيف ومقترحات القواعد.
- `Core` يملك بنية أوامر وجلسات مستقلة، ولا يساوي خادم شبكي أو تطبيقاً كاملاً.
- يختلف تمثيل Vitality بين النموذجين: يستخدم Domain النوع `double` في `Player.Vitality`، بينما يستخدم Core النوع `uint` في `IVitalityAccount` و`VitalityAccount`. لا يوجد عقد تحويل أو تكافؤ موثق بينهما؛ هذا وصف للاختلاف الحالي وليس قراراً بتوحيد النوع أو ترحيل النموذج.
- لا تحتوي المكتبتان مراجع Unity أو FishNet في المشروعين الحاليين.

## 3. ما لا يوجد

لا Unity project، ولا FishNet adapter، ولا Dedicated Server، ولا نقل Domain إلى Core، ولا Android/iOS build. وجود العقود وخدمات Core لا يثبت تكامل الشبكة أو المصادقة الإنتاجية.

## 4. قرار التغيير

ابقِ الحد الحالي مستقراً مؤقتاً. قبل الترحيل يلزم:
1. سجل ADR مقبول يحدد مسؤولية كل مشروع والواجهات المستهدفة.
2. تحليل الاعتماديات والـAPI العامة والاختبارات والـserialization والـcompatibility.
3. خطة ترحيل قابلة للتراجع، مع عدم حذف واجهات متوافقة قبل دورة إهمال موثقة.
4. اختبارات انحدار تغطي قواعد Domain وخدمات Core معاً.
5. تشغيل CI على SHA الترحيل وتحديث الوثائق والفهارس في الدفعة نفسها.

لا يوجد ADR مقبول على `main` حالياً؛ السجلات المتنافسة محفوظة في `docs/archive/` فقط.
