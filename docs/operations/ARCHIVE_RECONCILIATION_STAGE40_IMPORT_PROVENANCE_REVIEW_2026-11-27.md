# المرحلة 40 — تدقيق أصل الاستيراد وملف IMPORT_MANIFEST التاريخي (2026-11-27)

## الرأس والمصادر

- HEAD قبل المرحلة: becf8ee6573c48146af0a87c17ec9d6d5558b0d4.
- المصادر الأرشيفية: docs/governance/migration/PROVENANCE.md وBRANCH_REGISTER_2026-11-15.md وINTEGRITY_REVIEW_2026-11-15.md وIMPORT_MANIFEST.sha256.
- المقارنة تمت على محتوى أرشيف CostOfStaying-final 1 الموجود داخل الحاوية الأصلية، لا على main الحالي.

## نتيجة فحص manifest

- يحتوي IMPORT_MANIFEST.sha256 على 107 إدخالات hash.
- 106 إدخالات تخص ملفات Core-line (src/docs/tests) بحسب توزيع المسارات؛ وإدخال واحد إضافي هو .github/workflows/core-ci.yml.
- جميع المسارات الـ107 موجودة في أرشيف CostOfStaying-final 1؛ لا توجد مسارات manifest مفقودة.
- 99/107 بصمة SHA-256 تطابق المحتوى في ذلك الأرشيف.
- 8 بصمات لا تطابق نسخة CostOfStaying-final 1، لكن كل بصمة متوقعة موجودة في نسخة أرشيفية بديلة من المسار نفسه.
- فحص diff لهذه الملفات الثمانية يثبت أن الاختلاف الوحيد هو سطر Status في ADR-006 وADR-008 وADR-009 وADR-010 وADR-011 وADR-012 وADR-013 وADR-014؛ تتوافق هذه النتيجة مع سجل CHANGELOG التاريخي الذي يذكر تعديل سطر الحالة فقط في ثمانية ADRs.
- لذلك لا تُصنف فروق 8/107 كفساد أو تلف؛ manifest يحتفظ ببصمات الاستيراد الأصلية قبل تعديلات الحالة اللاحقة.

## حدود provenance

- PROVENANCE.md وBRANCH_REGISTER يصفان المستودع السابق وmain عند cb57b25 وفروعاً تاريخية؛ لا يثبتان حالة الفروع أو PRs الحالية.
- INTEGRITY_REVIEW يذكر أن الفحص التاريخي كان ساكناً في معظمه ولم يشغّل NUnit؛ لا ينقل أي نتيجة VERIFIED إلى main الحالي.
- سجل الاستيراد يثبت الأصل والتغييرات في وقتها، لكنه لا يجيز استيراد ADRs إلى docs/adr/ الحالية ولا اعتماد status APPROVED في نسخ الأرشيف.
- لا تعاد تسمية الفروع القديمة أو استعادتها أو دفعها.

## القرار

- حفظ manifest وprovenance كسجل تاريخي؛ لا نقلها إلى مجلدات السياسات الحية.
- اعتبار 8 فروق status-line مقصودة ومفسرة، لا اختلافات محتوى مجهولة.
- لا حذف أو تعديل أو استيراد أو دفع فروع/bundles.
- لم تُشغّل اختبارات أو CI.

**الحالة:** STAGE 40 COMPLETE — IMPORT PROVENANCE RECONCILED; 107/107 PATHS PRESENT, 99 HASH MATCHES, 8 EXPLAINED STATUS-LINE VARIANTS.
