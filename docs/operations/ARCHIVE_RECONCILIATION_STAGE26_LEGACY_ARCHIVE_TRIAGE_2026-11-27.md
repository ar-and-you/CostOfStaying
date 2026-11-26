# المرحلة 26 — مصالحة ملفات الأرشيف التاريخية ولقطات المستودع القديمة (2026-11-27)

## الرأس والنطاق

- HEAD قبل المرحلة: 9d3f6c90950a9194c8b22105aa7c5c69291805e6.
- تمت مراجعة 10 مسارات أرشيفية تحت docs/archive/ في مجموعة ZIP.
- تشمل: CI baseline مؤرخ، ستة تقارير نصية من legacy-full-backup، نسختين من وثائق حزمة v3.0، وسجل OPEN_DECISIONS superseded.

## النتائج

1. **legacy-full-backup-2026-11-15:** ملفات audit_index وbranch_comparison وbranches_summary وcommits_summary وrepo_audit_report وstatus_summary تمثل snapshot تاريخياً حول main عند cb57b259 (2026-11-14/15)، لا حالة main الحالية. محتوياتها تسجل فروعاً وcommits تاريخية لا يجوز إعادة إنشائها أو دمجها دون فحص Git حي مستقل.
2. **حالة النظافة:** status_summary يسجل ملفات غير متتبعة/مخرجات audit في لحظة الالتقاط؛ لذلك لا يصح وصف محتوى النسخة الاحتياطية بأنه snapshot نظيف تماماً دون مراعاة working-tree state.
3. **حجم التقرير:** repo_audit_report.txt يقارب 10.65MB ويتضمن dump كبيراً من شجرة/محتويات المستودع التاريخي. هو دليل مرجعي لا ملف تشغيل أو وثيقة حية؛ لا حاجة لنسخه إلى main.
4. **legacy-v3.0-package:** نسخة docs/archive من CONSOLIDATION_NOTES وUPLOAD_GUIDE تصرح بأنها SUPERSEDED، وتوضح أن خطواتها تصف حزمة v3.0 أحادية Core ولا تتطابق مع الشجرة الحالية التي تبقي Domain وCore منفصلين. تعليمات الرفع لا تُنفذ.
5. **superseded/OPEN_DECISIONS.md:** يصرح بأنه مستبدل بـ DECISION_RESOLUTIONS.md؛ يحفظ لتفسير تاريخ OD-01..13 ولا يعاد إلى مساره الحي.
6. **baseline-update-2026-11-18/ci-baseline.md:** يصرح بأنه PROPOSED ونتائج محلية جزئية بلا SHA أو GitHub Actions run؛ لا يستخدم كدليل VERIFIED.

## المصالحة مع main

توجد بالفعل وثائق حية تصف مصدر الحقيقة والقرارات المفتوحة وحدود التحقق، ومنها ROADMAP.md وdocs/adr/README.md وdocs/governance/DECISION_RESOLUTIONS.md وdocs/governance/STATUS_AND_EVIDENCE.md وdocs/operations/RECONCILIATION_2026-11-26.md. لا توجد حاجة لإعادة استيراد اللقطات التاريخية أو إعادة فتح تعليمات superseded.

## القرار

- تصنيف المسارات العشرة ARCHIVE_ONLY / HISTORICAL SNAPSHOT.
- الحفاظ عليها ضمن الأرشيفات الأصلية وسجلات المصدر؛ لا نقلها إلى main.
- لا إعادة إنشاء فروع تاريخية ولا دفع bundle أو ZIP.
- لا حذف أو استبدال أو استيراد.
- لا تشغيل اختبارات أو CI.

**الحالة:** STAGE 26 COMPLETE — LEGACY SNAPSHOT/ARCHIVE MATERIAL TRIAGED; LIVE MAIN REMAINS SOLE AUTHORITY.
