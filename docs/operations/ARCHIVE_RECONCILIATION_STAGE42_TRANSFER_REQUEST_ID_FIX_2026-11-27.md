# المرحلة 42 — إصلاح RequestId الفارغ في Suffering Transfer (2026-11-27)

## الرأس والنطاق

- HEAD قبل التغيير: 765149fe8fc65323da410b24f776556af17d21ac.
- العيب: `Match.TryTransfer` كان يتجاوز فحص وتسجيل idempotency عند RequestId null/empty/whitespace، ويسمح بنقل ناجح دون مفتاح منع تكرار.
- فحص البحث في المستودع أكد أن استدعاءات الاختبارات الحالية تستخدم معرفات غير فارغة.

## التغيير

1. إضافة `TransferResult.RejectedInvalidRequestId` في نهاية enum للحفاظ على القيم العددية السابقة.
2. رفض `string.IsNullOrWhiteSpace(requestId)` بعد فحص حالة المباراة والمسافة وقبل أي mutation أو event.
3. إضافة اختبار `Transfer_Rejects_Null_Empty_And_Whitespace_RequestIds_Without_SideEffects`؛ يثبت عدم تغير Debuffs للطرفين وBetrayal وProcessedRequestIds وEvents.
4. تصحيح تعليق API: الرفض لا يغير gameplay state، لكن بعض الرفضات تسجل أحداث تدقيق.
5. تحديث وثيقة Suffering Transfer ومراجعة عيوب Domain.

## الحدود المتبقية

- `ProcessedRequestIds` مجموعة داخل الذاكرة وعمر Match واحد؛ لا تمنع التكرار عبر restart أو بين عمليات متعددة.
- إعادة استخدام RequestId صالح لا تعيد تطبيق النقل ولا تضيف Betrayal، لكنها تسجل `TransferRejected` في `Events`. لم يتغير هذا السلوك لأن قرار اعتبار سجل التدقيق أثراً جانبياً مستقلاً لم يُحسم.
- الاختبار الجديد يغطي المدخلات الفارغة فقط؛ لا يثبت معالجة متوازية أو persistence أو transport.
- لم تُشغّل اختبارات .NET محلياً لعدم توفر SDK. نتيجة GitHub Actions على SHA الجديد هي بوابة التحقق التالية.

## القرار

- الإصلاح محدود ولا يغير قواعد اللعب أو قيم التوازن.
- لا تغيير لسياسة الأحداث أو الاحتفاظ أو التزامن.
- الحالة `IMPLEMENTED`؛ تبقى `VERIFIED` معلقة على CI مربوط بالرأس الجديد.

**الحالة:** STAGE 42 IMPLEMENTED IN MAIN — CI VERIFICATION PENDING.
