# معمارية الشبكة — تصميم مستقبلي

**الحالة:** `PROPOSED`. لا يوجد بروتوكول شبكة أو FishNet adapter أو Dedicated Server قابل للتشغيل في `main`.

## نموذج الثقة المستهدف

- جهاز العميل غير موثوق.
- الشبكة العامة غير موثوقة.
- خادم المباراة — عند بنائه — هو مصدر الحقيقة لحالة المباراة.
- Backend لا يملك tick المباراة إلا بقرار معماري صريح.

## عقد الأوامر المقترح

يرسل العميل intent فقط. يتحقق الخادم من هوية actor، الجلسة، MatchId، الحالة الحالية، الأذونات، حدود payload، المعدل، رقم التسلسل، ومفتاح idempotency. يرفض الطلب غير الصالح دون mutation. لا يرسل العميل `SetHealth` أو `TransferSucceeded` أو `VotePassed` أو نتائج محسوبة.

## ما يغطيه الكود الآن

Domain يطبق منطقاً خادمياً داخل عملية واحدة ويقبل distance كمدخل caller؛ Core يوفر خدمات/عقوداً للأوامر والجلسات والتحقق. لا يوجد transport أو authentication integration أو rate limiter شبكي أو server deployment. اختبارات الوحدة لا تثبت بروتوكولاً متصلاً.

## اختبارات مطلوبة قبل Alpha

Replay، إعادة ترتيب وتسلسل الرسائل، payload limits، rate limits، انقطاع وإعادة اتصال، تبديل actor/session، حالات لاعب/مباراة خاطئة، RTT وloss وjitter، وقياس server tick/CPU/bandwidth. قيم 100/200/300ms و1/3/5% نقاط اختبار مقترحة وليست SLA.
