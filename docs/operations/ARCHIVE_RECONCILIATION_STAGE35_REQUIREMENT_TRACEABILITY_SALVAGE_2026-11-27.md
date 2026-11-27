# المرحلة 35 — مصالحة متطلبات التتبع الأرشيفية مع خريطة المتطلبات الحية (2026-11-27)

## الرأس والمصادر

- HEAD قبل المرحلة: 09256b36d7d8338460047eb98e6e2dd34da93d21.
- المصدر الأرشيفي: docs/governance/REQUIREMENTS_TRACEABILITY.md (v2.1، PROPOSED).
- المصدر الحي: docs/traceability/REQUIREMENTS_TRACEABILITY.md وdocs/governance/POLICY_REGISTER.md وdocs/governance/STATUS_AND_EVIDENCE.md وملفات الجودة/المعمارية ذات الصلة.
- خريطة main الحية تصف snapshot عند SHA تاريخي وتصرح بأنها فهرس تتبع أولي، لا شهادة جودة أو موافقة قواعد.

## خريطة الربط

| مجموعة المتطلبات الأرشيفية | المقابل الحي | الحكم |
|---|---|---|
| GAME-001 / COL-001 | RULE-01 + Collapse system + Domain tests | مغطاة وظيفياً على مستوى Domain، مع بقاء قواعد التوازن PROPOSED FOR PLAYTEST |
| TRF-001 / BET-001 | RULE-02 + Transfer/Security tests | مغطاة كمنطق Domain؛ لا يثبت ذلك قياس مسافة شبكي موثوقاً أو معاملة موزعة |
| SP-001 | RULE-04 | مغطاة بوظائف Spirit الحالية؛ لا يوجد Echo world registry أو transport |
| END-001 | RULE-05 | مغطاة من حيث نتائج Domain؛ بعض حواف التصويت/الانقطاع ما زالت TBD |
| NET-001 | NET-AUTH + CORE-CMD/CORE-SESSION | جزئية؛ لا adapter أو خادم أو مصادقة إنتاجية |
| PERF-001 | PERFORMANCE_BUDGET + PERFORMANCE_TEST_PROTOCOL | الوثائق موجودة، لكن لا baseline قياس جهاز/شبكة موثق؛ المتطلب يبقى مفتوحاً |
| CI-001 | workflows الحية مثبتة بـSHA وتعليقات إصدار | الضابط موجود؛ لا automated workflow-hygiene test في main |
| CI-002 | ضوابط permissions وcheckout موجودة | لا دليل حي على فحص actionlint/zizmor/SARIF؛ مراجعة static منفصلة لازمة |
| CI-003 | permissions: read، timeout، persist-credentials:false | متحقق ساكناً في workflows الثلاثة |
| CI-004 | SECURITY.md وإشارات تاريخية لإعدادات GitHub | لا Gitleaks workflow حي؛ حالة إعدادات Secret Scanning تحتاج قراءة مباشرة |
| CI-005 | UNITY requirement | غير قابل للتنفيذ الآن؛ لا Unity project ولا workflow |
| CI-006 | vendor workflows/ruleset templates | غير موجودة في main؛ لا يلزم استيرادها حالياً |
| PLAT-001 | Android/device compatibility docs | Android هدف موصوف؛ لا build أو device evidence |
| PLAT-002 | iOS | يبقى TBD/غير معتمد حتى قرار مالك وبيئة macOS/Xcode |
| PLAT-003 | Domain/Core boundary | Core مستقل عن Unity حالياً؛ هذا وصف للتنفيذ لا اعتماد ADR معماري نهائي |

## الفجوات ذات القيمة

1. **الأداء:** احتفظ بمعرف متطلب قابل للتتبع يربط الميزانية بالبروتوكول ونتائج جهاز فعلي؛ لا ترفع الحالة إلى VERIFIED قبل baseline قابل للتكرار.
2. **أمن سلسلة التوريد:** يمكن إضافة فحص ثابت للـworkflows (مثل actionlint/zizmor أو بديل مكافئ) بعد اختيار الأداة وإصدارها وتحديد false-positive policy؛ لا تُنقل الاختبارات الأرشيفية لأنها تتوقع ملفات غير موجودة.
3. **الأسرار:** قرر هل يكفي إعداد GitHub Secret Scanning/Push Protection أم يلزم Gitleaks ضمن CI؛ افحص إعدادات المستودع مباشرة قبل تقرير الحالة.
4. **النقل/المصادقة:** لا تستخدم NET-001 لإثبات وجود server-side distance validation أو authentication؛ يلزم adapter وintegration tests قبل ذلك.
5. **المنصات:** Android/iOS تبقى أهدافاً وخططاً، لا نتائج تنفيذ.

## القرار

- لا تُستورد IDs أو روابط SRC-ADR الأرشيفية كما هي؛ فهي مرتبطة بكتالوج ADR قديم غير نافذ.
- لا تُعاد كتابة traceability الحية في هذه المرحلة؛ يلزم اعتماد مالك/مراجع المشروع لتثبيت أي معرفات جديدة أو تعديل حالة متطلب.
- تسجل هذه المرحلة خريطة salvage مقترحة للفجوات فقط.
- لا حذف أو نقل أو استيراد أو تعديل كود/workflows.
- لم تُشغّل اختبارات أو scanners أو CI.

**الحالة:** STAGE 35 COMPLETE — LEGACY REQUIREMENT MAP RECONCILED; PERFORMANCE/SECURITY/NETWORK EVIDENCE GAPS REMAIN OPEN.
