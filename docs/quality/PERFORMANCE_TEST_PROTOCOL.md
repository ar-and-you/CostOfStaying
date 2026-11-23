# بروتوكول قياس الأداء — مقترح

**الحالة: PROPOSED.** لا يوجد حالياً build قابل للتشغيل لإجراء القياس.

## المتطلبات المسبقة

سجل SHA وbuild number وUnity/editor/packages وOS/SDK والجهاز والمشهد وإعدادات الجودة ومصدر الطاقة/الشبكة. ثبّت workload ومدة warm-up ومدة التسجيل.

## القياسات

- Client: frame time وFPS وCPU/GPU وRAM/PSS وGC وloading وthermal/power.
- Network: RTT وloss وjitter وreordering وbandwidth وreconnect.
- Server: tick time وCPU/RAM وconcurrency وqueueing وcrash/recovery.

## التحليل

اعرض median وP95 وworst-case، لا المتوسط وحده. كرر التجربة على الأجهزة المرجعية. احفظ raw logs وtool versions وhashes. افصل نتائج editor عن device build. لا تضع حدود pass/fail إلا بعد اعتماد budget موثق.
