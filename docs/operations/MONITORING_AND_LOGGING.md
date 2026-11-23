# المراقبة والتسجيل — خطة مستقبلية

**الحالة: PROPOSED.** لا توجد منصة monitoring أو production server في `main`.

عند تنفيذ الخادم، اجمع الحد الأدنى المفيد من telemetry: startup/shutdown، build/SHA، crash، tick duration، queue depth، disconnect/reconnect، أخطاء authorization/replay، وأداء الخدمة.

قواعد السجلات:
- لا تسجل tokens أو كلمات مرور أو بيانات شخصية غير لازمة.
- استخدم correlation IDs غير حساسة، وحدد retention وصلاحيات الوصول.
- افصل application logs عن security audit logs.
- حدد sampling وredaction ومراقبة سلامة السجلات.
- لا تعتمد تنبيهاً دون مالك واستجابة محددين.

لا تدّع وجود dashboards أو alerting أو SLOs قبل نشرها واختبارها.
