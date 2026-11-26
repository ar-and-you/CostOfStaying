# المرحلة 22 — مصالحة حوكمة الأرشيف وقرارات ADR المتنافسة (2026-11-27)

## الرأس والمصادر

- HEAD قبل المرحلة: 0e4fa4d17facab5819dba253221afc91a810b8d3.
- تمت مراجعة ملفات الأرشيف ذات المسارات غير الموجودة في main ضمن docs/governance/: 44 مساراً فريداً، منها 27 ملف ADR و17 وثيقة حوكمة/هجرة.
- المرجع الحاكم: docs/governance/PROJECT_GOVERNANCE.md وPOLICY_REGISTER.md وSTATUS_AND_EVIDENCE.md وdocs/adr/README.md وdocs/architecture/DOMAIN_CORE.md في main.
- هذه مراجعة دلالية وتصنيفية؛ لا تُنشئ قرار مالك ولا تنقل حالة وثيقة.

## النتيجة الحاكمة

تنص وثائق main الحالية صراحةً على أن أرقام ADR الواردة في نسخ سابقة مواد أرشيفية ما لم يوجد سجل مقبول في docs/adr/. ومجلد docs/adr/ لا يحتوي سجلات ADR مقبولة؛ كما يقرر سجل الحوكمة أن مالك المشروع هو صاحب اعتماد قواعد اللعب والقرارات المعمارية. لذلك لا تُعد عبارة APPROVED في نسخة ADR الأرشيفية اعتماداً نافذاً على main، ولا تُستورد المجموعة دفعة واحدة.

## نتائج فحص مجموعة ADR

- 27 ملف ADR أرشيفياً، مع 39 نسخة محتوى مميزة إجمالاً عبر الأرشيفات السبعة.
- تعارض ترقيم صريح: ملفان مختلفان يحملان الرقم ADR-005:
  - ADR-005-CI-AND-DOMAIN-CORE.md — مادة Domain/CI مصنفة في نسخها كـSUPERSEDED بـADR-021.
  - ADR-005-CORE-EXECUTABLE-BOUNDARY.md — حد Core تنفيذي مستقل، ونسخة أرشيفية منه تصف موافقة سابقة.
- فجوة تسلسل: لا يظهر ADR-024 ضمن المجموعة المفهرسة.
- توجد ملفات ADR بحالة PROPOSED، أو APPROVED بشروط/حدود غير مكتملة. وجود تنفيذ أو اختبارات في فرع تاريخي لا يثبت موافقة سياسة حالية.

### تعارضات ومواضيع تستلزم قراراً مستقلاً

1. **حد Domain/Core — ADR-021 وADR-023:** نسخ الأرشيف تصف قراراً بنقل قواعد Domain إلى Core وتوحيد النواة/حسم دورة الحياة. وثيقة DOMAIN_CORE.md الحية تصف المشروعين منفصلين، وتقول إن النقل غير منفذ ولا يوجد ADR مقبول على main. يبقى القرار TBD؛ لا نقل ولا إعادة هيكلة استناداً للأرشيف.
2. **دورة حياة اللاعب — ADR-007 مقابل التنفيذ الحالي:** ADR-007 الأرشيفي يوثق اختياراً سابقاً لحالات Alive/Spirit/Eliminated، بينما enum الحي في Domain يتضمن Connecting/Alive/Dead/Spirit/Disconnected/Spectating، ويصف DOMAIN_CORE.md انتقال Vitality إلى Spirit وعدم وجود مسار active إلى Dead. هذا اختلاف تاريخي/تنفيذي يجب أن يبقى موثقاً؛ لا يُعدّل الكود أو القواعد بناءً على ADR قديم.
3. **قواعد اللعب والعتبات — ADR-004 وADR-022:** كلاهما يترك قواعد اللعب/قيم Betrayal ضمن PROPOSED FOR PLAYTEST أو PROPOSED TEST VALUE. لا تُرفع الأرقام إلى قواعد نافذة ولا تُضاف اختبارات قبول لها دون جلسات بشرية وقرار مالك.
4. **المنصات — ADR-025:** نسخة الأرشيف تصف Android/iOS أولاً، لكنها PROPOSED وتذكر متطلبات بناء/توقيع غير مثبتة. تبقى وثيقة المنصات الحالية المرجع الوصفي، ولا يُنشأ pipeline من الملف التاريخي.
5. **أدوات GitHub — ADR-026:** المادة تصف baseline مقترحاً، وتذكر أن التنفيذ لم يكن مشغلاً. لا تُفعّل إعدادات أو workflows لمجرد وجود ADR.
6. **التكدس والاستضافة — ADR-001 وADR-003:** الاتجاه التقني لا يثبت اختيار إصدار/مزود أو صلاحية نشر. يلزم prototype وقياس واعتماد محدد النطاق.

### مواد قد تحمل قيمة مرجعية دون أن تكون قرارات نافذة

- ADR-008 إلى ADR-020 تتضمن عقوداً ومخاطر هندسية حول admission، sequence، player registry، session lifecycle، cleanup، idempotency، connection/session binding، credential validation، roster admission، authorization، وtyped payload validation.
- بعض هذه الموضوعات يتقاطع مع خدمات Core واختباراته الحالية. يمكن استخدام النصوص كقائمة تحقق لمراجعة التغطية، لكن لا تنقل سياسات gameplay أو transport أو identity غير المعتمدة إلى التنفيذ.
- ADR-002 يصف مبدأ server authority؛ يمكن مقارنته مع حدود الثقة الحية، دون الادعاء بوجود خادم إنتاجي أو transport adapter.

## وثائق الحوكمة الأخرى

- ملفات مثل DOCUMENT_GOVERNANCE.md وDECISIONS_LOG.md وOPEN_DECISIONS.md وCONFLICT_RESOLUTIONS.md وREQUIREMENTS_TRACEABILITY.md وSOURCES_AND_REFERENCES.md تحمل تاريخ قرارات ومراجع وقيود حالة.
- ملفات migration/ تحفظ provenance وbranch register وintegrity review تاريخية؛ لا تُحذف ولا تُعامل كحالة فروع حية.
- بعض الوثائق تعلن PROPOSED أو SUPERSEDED صراحةً؛ تحفظ كأدلة على سبب عدم الاستيراد.
- لا يوجد ما يبرر استيراد 44 ملفاً إلى المجلدات الحية. أي معلومة نافعة تُحوّل لاحقاً إلى بند مراجعة/تعارض موثق داخل وثائق main، لا بنسخ سجل كامل.

## القرار

- ARCHIVE_ONLY / HISTORICAL لمجموعة 44 مساراً؛ لا تُستورد كما هي.
- KEEP_LIVE لسياسات الحوكمة الحية وسجل ADR الحالي.
- إبقاء نقاط المراجعة: Domain/Core، دورة حياة اللاعب، قواعد اللعب/العتبات، platform targets، stack/hosting، وتحقق العقود الهندسية من ADR-008..020 مقابل Core الحي.
- لا حذف أو استبدال أو نقل أو استيراد.
- لم تُشغّل اختبارات أو CI.

**الحالة:** STAGE 22 COMPLETE — GOVERNANCE/ADR SEMANTIC TRIAGE; OWNER DECISIONS AND TECHNICAL COVERAGE REVIEW OPEN.
