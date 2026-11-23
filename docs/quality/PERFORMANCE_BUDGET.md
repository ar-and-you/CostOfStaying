# ميزانية الأداء — TBD حتى وجود build قابل للقياس

**الحالة: TBD.** لا يوجد Unity/Android build أو Dedicated Server قابل للقياس في `main`؛ لذلك لا توجد حدود FPS أو RAM أو latency معتمدة.

عند توافر build، يجب تحديد ميزانية منفصلة للعميل والخادم:

- Client: FPS وframe time (median/P95)، CPU/GPU، RAM/PSS، GC allocations، زمن تحميل المشاهد، حرارة/طاقة، وحجم التطبيق.
- Network: RTT، packet loss، jitter، bandwidth، reconnect success، وحجم الرسالة.
- Server: tick duration/P95، CPU/RAM لكل مباراة، عدد المباريات/اللاعبين، queue length، crash/restart، وزمن الاستعادة.

كل حد رقمي يحتاج جهازاً/بيئة وإعداد جودة وسيناريو حمل وإصدار build موثقاً. لا تعتمد أرقاماً عامة قبل القياس. لا تستخدم محاكاة Python كقياس أداء عميل أو خادم.
