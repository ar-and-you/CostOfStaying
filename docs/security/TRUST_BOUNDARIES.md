# حدود الثقة — هدف معماري غير منفذ بالكامل

**الحالة: PROPOSED.** لا يوجد عميل متصل أو شبكة أو Dedicated Server قابل للتشغيل في `main`.

## مناطق الثقة المستهدفة

- **Client device:** غير موثوق؛ يمكن تعديل التطبيق أو إرسال طلبات مصطنعة.
- **Public network:** غير موثوقة؛ افترض replay وdelay وreorder وقطع الاتصال.
- **Match server:** ينبغي أن يكون مصدر الحقيقة لحالة المباراة عند بنائه؛ غير موجود حالياً.
- **Backend:** لا يُمنح سوى الصلاحيات اللازمة لوظائف الحساب/المطابقة/التخزين إذا اعتُمد لاحقاً.
- **Repository/CI:** سطح حساس للأسرار وسلسلة التوريد والصلاحيات.

## عقد العميل المقترح

يرسل العميل نوايا/أوامر وليس نتائج محسومة. يجب أن يحسب النظام الموثوق Vitality والضرر والمدى والذخيرة والتصويت والنتائج. لا يقبل `SetHealth` أو `TransferSucceeded` أو `VotePassed` كحقائق يقررها العميل.

## ما يغطيه الكود الحالي

منطق Domain يعمل داخل عملية .NET ويستقبل بعض القيم مثل المسافة من caller. توجد عقود وخدمات Core للأوامر والجلسات والتحقق. لا يعني ذلك وجود مصادقة transport أو قياس مسافة خادمي فعلي أو rate limiting شبكي.

Core يتحقق من session Authenticated وmatch roster وauthorization policy وtyped payload قبل/بعد dequeue، ويطبق per-session sequence وbounded queue. Payload validation تسبق authentication، لذلك يجب أن تكون رخيصة ومحدودة. TryAuthenticate يستقبل ActorId موثوقاً من host؛ لا credential validation. BetrayalLedger لا يتحقق من roster membership، وCoreComposition لا يفرض instance-per-match.

## ما يجب اختباره قبل الاتصال

Actor/session binding، match scoping، replay/idempotency، sequence ordering، payload limits، rate limits، authorization، reconnect، clock monotonicity، validation server-side، وإدارة الأسرار.
