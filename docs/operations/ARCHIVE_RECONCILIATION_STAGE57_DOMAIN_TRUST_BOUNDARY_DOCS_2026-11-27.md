# المرحلة 57 — تصحيح تعليقات حدود الثقة والساعة والأحداث في Domain (2026-11-27)

## الرأس والنطاق

- HEAD قبل توثيق المرحلة: a5eca3edd1cd3bad6cd93afe8fe58d68fd2a9311.
- راجعت المرحلة سلسلة تصحيحات التعليقات التالية:
  - 1bdcc23048e6b9bde2b6595e4407002223f1da62 — Clock.cs.
  - 3a1c2c9fb567a691c60dc469d6fcf79a994d47d5 — Debuff.cs.
  - ff7b1c59283b8ced7a5235e27ce9ae48bd72bf69 — DomainEvent.cs.
  - 1658f52b9b36fbd71e7c701647a604148f041071 — Intent.cs.
  - 076a461bc6d02a2f7e3d0482b78c0986b37f9880 — Match.cs.
  - a5eca3edd1cd3bad6cd93afe8fe58d68fd2a9311 — Player.cs.
- الهدف: إزالة ادعاءات توحي بوجود server/network adapter أو trust boundary غير منفذ.

## التعديلات المسجلة

1. **IMatchClock:** وصفها كمصدر وقت يحقنه host؛ يجب على host الإنتاجي توفير monotonic clock. الواجهة لا تصادق المستدعي ولا تنشئ client/server trust boundary.
2. **MonotonicClock:** وصفها host-agnostic elapsed-time source، لا ساعة Dedicated Server مثبتة.
3. **Debuff.RemainingSeconds:** توثق أنها تنقص بواسطة Match.Tick وفق delta الممرر.
4. **DomainEvent:** الحدث صادر من Match aggregate؛ قد ينسخه host/network adapter مستقبلي، لكن Domain assembly لا تنفذ transport ولا تصادق مصدر العملية.
5. **ClientIntent/TransferIntent:** DTOs مقترحة لتكامل مستقبلي؛ PlayerId وRequestId وClaimedDistanceMeters مدخلات غير موثوقة. يجب على host ربط PlayerId بهوية مصادق عليها واشتقاق المسافة من مواقع موثوقة قبل استدعاء Match.
6. **Match/Tick:** الوصف أصبح Domain aggregate وساعة/دلتا يزودها المستدعي؛ لا transport أو authentication أو server runtime، ولا يتحقق aggregate من مصدر delta.
7. **Player:** التعليقات توضح أن scalar setters داخلية، لكن public collections ما زالت mutable وتشكل D-03؛ لا تُعامل setters الداخلية كحد أمني client/server.

## الأثر والحدود

- تغييرات XML/comments فقط؛ لا تغيير signatures أو runtime behavior أو قواعد اللعب.
- أزيلت إحالات غير دقيقة إلى ADR-002 باعتبارها إثباتاً لوجود خادم/مصادقة.
- لا يثبت هذا وجود monotonic host adapter في المنتج الحالي؛ الواجهة تسمح بحقن IMatchClock، والمسؤولية تقع على host عند إنشائه.
- D-03 لا يزال قائماً لأن collection properties العامة قابلة للتعديل.

## القرار

- KEEP_LIVE للتعليقات المصححة.
- لا Unity/network/server adapter أو replication runtime أضيف.
- لا اختبارات جديدة؛ لا تغيير في قواعد اللعب أو التوازن.

**الحالة:** STAGE 57 COMPLETE — DOMAIN TRUST/INTENT/CLOCK/EVENT COMMENTS NOW MATCH THE HOST-AGNOSTIC IMPLEMENTATION.
