# سجل القرارات المفتوحة وبوابات الاعتماد

**تاريخ اللقطة:** 2026-11-27  
**المرجع:** main  
**الحالة:** سجل تجميعي للتتبع فقط؛ ليس ADR ولا موافقة على أي قرار.

يمنع هذا السجل تحويل المقترحات أو التنفيذ التاريخي أو نتائج المحاكاة إلى سياسة نافذة. الإغلاق يتطلب قرار مالك مكتوباً، مع تحديث المصدر الحاكم والفهرس والأدلة.

## 1. بوابات التنفيذ D0–D9

| البوابة | الحالة | الدليل/المتبقي |
|---|---|---|
| D0 — توثيق مسارات الشبكة والخادم كتوجهات مقترحة | COMPLETE | ROADMAP يصف Unity/network/server كمسارات مستقبلية لا كتنفيذ قائم |
| D1 — مصالحة واعتماد ADR catalogue | OPEN_OWNER_DECISION | لا توجد ADR مقبولة في docs/adr؛ تعارض ADR-005 وفجوة ADR-024 ومرشحات أخرى موثقة في المرحلة 22 |
| D2 — عقد Domain/Core وVitality/lifecycle | OPEN_OWNER_DECISION | مصفوفة توافق المرحلة 58 جاهزة؛ لا mapping معتمد ولا ترحيل |
| D3 — branch protection وسياسة الفروع | BLOCKED_BY_EVIDENCE_AND_OWNER | قراءة 2026-11-22 تاريخية؛ endpoint أعاد 403 في 2026-11-26؛ الحالة الحالية UNKNOWN، ولا تغيير إعدادات دون قرار |
| D4 — قرارات المنتج D-R1..D-R7 | OPEN_OWNER_DECISION | جميع القرارات أدناه مفتوحة |
| D5a — Unity editor/version | OPEN_TECHNICAL_SPIKE | لا Unity project أو prototype؛ لا اعتماد إصدار نهائي |
| D5b — transport/network library | OPEN_TECHNICAL_SPIKE | لا adapter أو prototype مقارنة؛ العميل/الخادم غير موجودين |
| D6 — hosting/operations | OPEN_OWNER_DECISION | لا اختيار مزود أو نموذج نشر أو server runtime |
| D7 — consent/privacy/retention protocol | OPEN_OWNER_DECISION | لا بروتوكول موافقة/احتفاظ/حذف معتمد لجلسات بشرية |
| D8 — 10 جلسات بشرية وقرار القواعد/التوازن | BLOCKED_ON_HUMAN_PLAYTEST | سجلات المحاكاة العشرة لا تحتسب جلسات بشرية؛ راجع PLAYTEST_PLAN والمرحلة 24 |
| D9 — pre-production exit criteria | OPEN | لا توجد حزمة قبول نهائية تثبت جاهزية الإنتاج |

## 2. قرارات المنتج D-R1–D-R7

كلها OPEN حتى اعتماد مالك مكتوب:

| الرمز | القرار المطلوب |
|---|---|
| D-R1 | شرط الفوز والنهاية: LastSurvivor أو TimeLimit أو مزيج محدد |
| D-R2 | حالات اللاعب ودلالات الموت/الإقصاء/Spirit ودورة الحياة |
| D-R3 | الانقطاع وإعادة الاتصال وأثرهما على التصويت والمباراة |
| D-R4 | قواعد التصويت والإجماع والمهلة والنتائج الحدية |
| D-R5 | قيم التوازن والاقتصاد ومعدلات/مدد الأنظمة |
| D-R6 | اقتصاد Spirit وتفاعلاته وحدود تأثيره |
| D-R7 | منظور اللاعب/الكاميرا والعرض وتجربة المعلومات |

لا تعتمد هذه القرارات من ADR أرشيفي أو قيمة محاكاة أو سلوك تنفيذ غير موثق كقرار منتج.

## 3. عيوب Domain الهندسية

| الرمز | الحالة | الملخص |
|---|---|---|
| D-01 | FIXED_IN_LIVE | احتساب Debuff حسب مدة نشاطه داخل Tick طويل |
| D-02 | FIXED_IN_LIVE | رفض Debuff/config للقيم غير الصالحة |
| D-03 | OPEN_OWNER_DECISION | مجموعات Match/Player العامة قابلة للتعديل؛ يلزم API encapsulation وخطة توافق |
| D-04 | OPEN_OWNER_DECISION | Events/RequestIds/EchoIds ومدخلات النص بلا سياسة retention/size |
| D-05 | OPEN_OWNER_DECISION | أثر موت/انقطاع اللاعب بعد فتح التصويت |
| D-06 | OPEN_OWNER_DECISION | سياسة Collapse عند تأخر Tick: skip أم bounded catch-up أم replay |
| D-07 | OPEN_OWNER_DECISION | حد Domain/Core وخرائط الأنواع والهويات والوقت |
| D-08 | VERIFIED_AT_SHA | RequestId null/empty/whitespace مرفوض؛ Domain CI نجح 62/62 على SHA bfbed74ae24e8e5e49baff84e7a18b0e99b55807 (run 45916990172). لا يعني ذلك persistence أو concurrent replay guarantees |

D-08 موثق على SHA المذكور فقط؛ لا تنقل نتيجته إلى SHA أحدث دون check run جديد. D-01/D-02 مثبتان باختبارات الانحدار وضمن تشغيل Domain CI نفسه.

## 4. فجوات الأدلة الخارجية

- Branch protection: UNKNOWN بسبب 403؛ rulesets الفارغة في القراءة المتاحة لا تحسم جميع أشكال الحماية.
- Dependabot alerts وCode Scanning alerts: لم تتوفر قراءة موثوقة بسبب 403؛ لا تسجلها على أنها خالية.
- Secret scanning/Push protection: آخر حالة موثقة تعود إلى 2026-11-24؛ تحتاج قراءة حديثة.
- CodeQL: سجل التشغيلات يثبت بعض التحليلات على SHAs محددة؛ لا يثبت خلو التنبيهات.
- Unity/Android/server/network: لا runtime أو build فعلي في الشجرة.
- Human playtest: لم تُسجل جلسات بشرية معتمدة؛ بيانات المحاكاة ليست بديلاً.

## 5. قاعدة الإغلاق

1. لا إغلاق بوابة بعبارة أو تعليق داخل كود فقط؛ يلزم مصدر حاكم وقرار موثق عند الحاجة.
2. لا تغيير branch protection أو required checks أو push policy دون D3.
3. لا تغيير public Domain API أو retention/caps دون قرار D-03/D-04.
4. لا تغيير قواعد التصويت أو catch-up دون D-R3/D-R4/D-06.
5. لا دمج Domain/Core دون D1/D2/D-07.
6. لا جمع playtest بشري دون D7، ولا اعتماد التوازن دون D8.

**مصادر التفاصيل:** ADVISORY_PLAN_2026-11-26.md، DOMAIN_DEFECT_REVIEW_2026-11-25.md، STATUS_AND_EVIDENCE.md، DOMAIN_CORE.md، PLAYTEST_PLAN.md، وتقارير المراحل 22 و24 و54–58.
