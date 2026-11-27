# المرحلة 41 — سجل موحد لمرشحات الاستفادة من الأرشيف (2026-11-27)

## الرأس والنطاق

- HEAD قبل المرحلة: 3fe0a71bc95a6e50dd618d8db7623316f3149372.
- يجمع هذا السجل فقط المواد التي ظهرت في مراجعات salvage للمراحل 22–40؛ لا يعيد جرد ZIP أو إعادة حساب بصمات.
- كل بند مقترح غير نافذ إلى أن يراجعه مالك المشروع ويُسجل التغيير في main وفق CHANGE_CONTROL.

## أ. عناصر ناضجة وموجودة بالفعل في main — لا تكرر استيرادها

| العنصر | الدليل الحي | القرار |
|---|---|---|
| Actions مثبتة بـcommit SHA وتعليق إصدار | workflows الثلاثة | KEEP_LIVE |
| permissions: contents: read | workflows الثلاثة | KEEP_LIVE |
| job timeouts | ci/core-ci/docs-governance | KEEP_LIVE |
| persist-credentials: false | checkout في workflows الثلاثة | KEEP_LIVE |
| فصل SDK Domain/Core | global.json الجذري وCore.Tests/global.json | KEEP_LIVE |
| Move/atomic transfer وRequestId/idempotency واختبارات replay | Domain/Core والخطين الاختباريين | KEEP_LIVE؛ لا نسخ عقود قديمة |
| 37 سيناريو اختبار قديم في Core.Tests | أسماء الحالات في Domain.Tests الحية | لا استعادة الملفات القديمة |
| سياسة خصوصية الحد الأدنى والاحتفاظ/الحذف والتصدير | DATA_PRIVACY الحية | KEEP_LIVE |
| عدم الخلط بين المحاكاة واللعب البشري | PLAYTEST_PLAN وSTATUS_AND_EVIDENCE | KEEP_LIVE |
| فصل أمن التطبيق عن أمن المستودع وCI | SECURITY_ARCHITECTURE/THREAT_MODEL | KEEP_LIVE |

## ب. مرشحات تحتاج قراراً أو مراجعة قبل اعتمادها

| المرشح | الفائدة المحتملة | الشرط قبل الاعتماد |
|---|---|---|
| تصنيف حوادث P0–P3 | توحيد الفرز الأولي للحوادث | تعريف النطاق والمالك والتصعيد؛ لا يتحول إلى SLA دون فريق/أزمنة |
| بروتوكول أداء افتراضي: جلسة ≥15 دقيقة وثلاث إعادات وthermal soak | رفع قابلية تكرار قياس الأداء | تحديد الأجهزة/build/workload وأداة القياس، ثم إدراجه كمنهج لا كميزانية |
| tick-rate sweep عند 10/15/20Hz | مقارنة CPU/latency/bandwidth قبل تثبيت معدل | وجود transport/server prototype وقياس فعلي |
| شبكة RTT 100/200/300ms وloss 1/3/5% مع jitter/reorder/reconnect | مصفوفة اختبار ضغط شبكي | تبقى قيم اختبار مقترحة وليست SLA؛ ربطها بسيناريو المنتج |
| إضافة workflow lint (actionlint/zizmor أو بديل) | كشف مخاطر YAML وسلسلة التوريد مبكراً | اختيار أداة وإصدار وتحديد false-positive policy وصلاحيات التشغيل |
| إضافة Gitleaks داخل CI | مسح أسرار للشجرة/التاريخ بأداة متخصصة | تحديد النطاق، سياسة استثناءات، وتجنب تكرار/تعارض إعدادات GitHub Secret Scanning |

## ج. عناصر لا تُعتمد من الأرشيف

- عتبات Betrayal 3/5/7+، جداول Collapse القديمة، أرقام الأسلحة، و30FPS/400MB/100MB/Redmi Note 8: فرضيات أو أرقام موروثة مرفوضة وليست قواعد أو ميزانيات.
- تحويل disconnect إلى No في تصويت Endgame: غير محسوم في الوثيقة الحية.
- Unified Core ونقل Domain إلى Core وقرارات ADR-021/023 الأرشيفية: تتعارض مع main الحالي ولا تُنفذ.
- اعتماد Unity 6.3/FishNet/PlayFab/Nakama أو إنشاء Unity/mobile/server pipeline: لا prototype ولا قرار مالك يبرره.
- استيراد CODEOWNERS أو rulesets أو Dependabot أو security workflows من الأرشيف: يتطلب مراجعة مستقلة وإعدادات/أسرار/مالكاً حقيقياً.
- فروع ZIP وPRs التاريخية ونتائج CI القديمة: لا تستعاد ولا تنقل حالة VERIFIED/APPROVED.

## د. عناصر تحقق لم تُحسم

1. Assertion/fixture-level comparison للحالات الاختبارية القديمة المختلفة المحتوى.
2. مراجعة دقيقة لكل وثيقة audit/governance أرشيفية فقط إذا ظهر طلب محدد لإعادة استخدام معلومة منها.
3. قراءة مباشرة لإعدادات GitHub Secret Scanning/Push Protection وCodeQL/Dependabot عند الحاجة إلى تقرير حالة حديث.
4. قياسات أداء وشبكة وجلسات بشرية؛ لا يمكن استنتاجها من ملفات الأرشيف أو المحاكاة.

## القرار

لا يوجد في الأرشيف ما يبرر overlay أو استبدالاً جماعياً. جميع المرشحات القابلة للاستخدام إما موجودة بالفعل في main أو محفوظة كاقتراحات غير ملزمة. تبقى الملفات الأصلية وسجلات provenance محفوظة؛ لا حذف أو نقل أو استيراد في هذه المرحلة.

**الحالة:** STAGE 41 COMPLETE — SALVAGE CANDIDATES CONSOLIDATED; OWNER REVIEW REQUIRED FOR NEW POLICIES/TEST DEFAULTS.
