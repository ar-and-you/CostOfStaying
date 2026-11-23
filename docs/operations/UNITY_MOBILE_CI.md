# Unity وCI المحمول — خطة تفعيل مستقبلية

**الحالة: NOT IMPLEMENTED.** لا يوجد Unity project أو workflow لـUnity/Android/iOS في شجرة `main` التي فُحصت في 2026-11-23.

## شروط البدء

- اختيار Unity Editor version وتثبيته.
- إنشاء مشروع Unity فعلي ومراجعة packages وscripting backend/compatibility profile.
- تحديد Android target وSDK/NDK وminimum API والأجهزة المرجعية.
- تحديد iOS scope، ووجود macOS/Xcode عند الحاجة.
- مراجعة رخص GameCI/Unity والاعتماديات والتكاليف.
- حفظ تراخيص Unity ومفاتيح التوقيع خارج Git وفي GitHub Environments بصلاحيات محدودة.

## خطوط CI المستقبلية

1. تحقق import/project structure.
2. EditMode tests.
3. PlayMode tests عند وجود scenes قابلة للاختبار.
4. build Android APK/AAB مع signing آمن.
5. iOS build منفصل عند قرار دعم المنصة.
6. حفظ artifacts وlogs وربطها بـSHA.
7. device smoke/performance tests على أجهزة فعلية.

لا تنشئ workflow شكلياً قبل وجود مشروع قابل للبناء وملفات license/SDK المطلوبة. لا تسجل اختبارات محمولة كناجحة قبل تشغيلها فعلياً.
