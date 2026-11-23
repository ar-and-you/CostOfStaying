# معمارية البناء والنشر — الحالة الفعلية والخطة المستقبلية

**الحالة:** مكتبات .NET وCI موجودة؛ نشر Unity/Android/server غير منفذ.

## ما هو موجود

- `ci.yml`: Domain على .NET 8 واختبار محاكي Python.
- `core-ci.yml`: Core على .NET 10.
- كلا workflowين بصلاحية `contents: read`؛ لم يتحولا إلى نشر منتج.

## ما ليس موجوداً

لا Unity project، ولا build Android/iOS، ولا Docker image لخادم اللعبة، ولا Dedicated Server، ولا Backend أو بيئة Staging/Production. لا يوجد artifact نشر لعبة موثق.

## تسلسل مقترح عند دخول التنفيذ

1. تثبيت إصدارات Unity/packages وSDK/NDK وFishNet بعد prototype.
2. بناء Client وServer منفصلين مع commit SHA/build provenance.
3. فصل secrets والتوقيع عن Git، وتقييد الوصول عبر environments.
4. التحقق من نتائج unit/integration/security قبل النشر.
5. نشر staging ثم smoke/load/network tests ثم قرار مالك للنشر الأوسع.
6. تجربة rollback واستعادة قبل production.

كل artifact مستقبلي يجب أن يحمل SHA، رقم build، target، الإصدارات، hashes، وسجل الاختبارات. لا تدّعِ Staging أو Production قبل وجود deployment فعلي.
