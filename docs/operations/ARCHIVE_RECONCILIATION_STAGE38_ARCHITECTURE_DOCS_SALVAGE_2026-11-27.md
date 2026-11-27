# المرحلة 38 — مراجعة salvage لوثائق المعمارية والشبكة الأرشيفية (2026-11-27)

## الرأس والنطاق

- HEAD قبل المرحلة: 9f9f0a4fb72938176ba62132d35ba57cade2e8ef.
- تمت مراجعة ست وثائق معمارية مشتركة بالاسم ومختلفة المحتوى: BACKEND_AND_HOSTING، DATA_ARCHITECTURE، DEPLOYMENT_ARCHITECTURE، DOMAIN_CORE، NETWORK_ARCHITECTURE، TECHNICAL_ARCHITECTURE.
- لا يعاد حساب بصمات المرحلة 21.

## نتائج المقارنة الدلالية

1. **BACKEND_AND_HOSTING:** الأرشيف يذكر PlayFab وNakama كخيارين؛ الوثيقة الحية تستخدم مقارنة محايدة بين managed hosting والاستضافة الذاتية بمعايير latency/cost/autoscaling/portability/recovery/privacy. لا توجد فائدة مثبتة من إضافة مزودين محددين قبل prototype.
2. **DATA_ARCHITECTURE:** مبادئ الذاكرة للحالة الحية، عدم حفظ كل tick، فصل الحساب/النتيجة/telemetry/security events، وتحديد الغرض والاحتفاظ والحذف موجودة بصياغة أحدث في الوثيقة الحية.
3. **DEPLOYMENT_ARCHITECTURE:** الأرشيف يصف workflows وartifacts وstaging كما لو كانت أجزاءً منفذة في حزمة قديمة؛ الوثيقة الحية تصحح بوضوح أن الموجود مكتبات وCI فقط، ولا Unity/Android/server/Backend deployment.
4. **DOMAIN_CORE:** النسخة الأرشيفية تصف Unified Core ونقل Domain إلى Core وحالات Alive/Spirit/Eliminated. هذا يتعارض مباشرةً مع الشجرة والوثيقة الحية التي تبقي Domain وCore منفصلين؛ DO_NOT_IMPORT.
5. **TECHNICAL_ARCHITECTURE:** يثبت Unity 6.3/FishNet/LangVersion 9.0 ويقرر Unified Core في الأرشيف. هذه اختيارات تاريخية غير مقبولة على main؛ لا تُعتمد.
6. **NETWORK_ARCHITECTURE:** نموذج العميل intent-only وحدود الثقة والاختبارات المطلوبة ممثلة بشكل أدق في الوثيقة الحية. مصفوفة RTT/loss/Jitter موجودة بالفعل كقيم اختبار مقترحة لا SLA.

## مرشحات تقنية غير ملزمة

- الأرشيف يقترح اختبار tick rates عند 10/15/20Hz، مع عدم تثبيت 20Hz قبل قياس CPU/bandwidth/latency. لا يظهر هذا النطاق العددي في وثيقة الشبكة الحية؛ يمكن حفظه كـcandidate test sweep بعد وجود server/transport، لا كقيمة تشغيل.
- الأرشيف يقترح MatchId عشوائياً ومعرفات أحداث فريدة؛ هذه ممارسة تصميمية معقولة لكنها تحتاج عقداً فعلياً مع serialization/persistence/replay قبل تحويلها إلى requirement.
- أمثلة PlayFab/Nakama تصلح لقائمة بحث أولية فقط، وليست توصية أو اختياراً.

## القرار

- KEEP_LIVE لوثائق المعمارية الست الحالية.
- ARCHIVE_ONLY / DO_NOT_IMPORT للنسخ التي تصف unified Core أو stack معتمداً أو deployment منفذاً.
- تسجيل tick-rate sweep وMatchId/event identity كمرشحين بحث فقط.
- لا إنشاء adapter أو server أو Backend أو تعديل الكود.
- لا حذف أو استبدال أو نقل أو استيراد.
- لم تُشغّل اختبارات أو قياسات شبكة.

**الحالة:** STAGE 38 COMPLETE — ARCHITECTURE DOCS RETAINED LIVE; TICK-RATE AND IDENTIFIER IDEAS LEFT AS NON-BINDING DESIGN CANDIDATES.
