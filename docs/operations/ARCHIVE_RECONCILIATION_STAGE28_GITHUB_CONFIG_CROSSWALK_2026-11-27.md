# المرحلة 28 — مطابقة إعدادات GitHub وWorkflows الأرشيفية (2026-11-27)

## الرأس والنطاق

- HEAD قبل المرحلة: bb29058afd533ac424ebf1b803c885c33ab5042c.
- جرى فحص 27 مساراً تحت .github/ و15 مساراً تحت github/ في الأرشيفات.
- شجرة main الحية تضم .github/workflows/ci.yml وcore-ci.yml وdocs-governance.yml و.gitkeep فقط.

## Crosswalk للمسارات المشتركة

| المسار | نسخ الأرشيف | نتيجة المقارنة |
|---|---:|---|
| .github/workflows/ci.yml | 2 مختلفة | PATH_EXISTS_CONTENT_DIFF — الحي لا يطابق أياً منهما |
| .github/workflows/core-ci.yml | 1 | PATH_EXISTS_CONTENT_DIFF — الحي مختلف |
| .github/workflows/docs-governance.yml | 0 | LIVE_ONLY_PATH |

لا يُستبدل أي Workflow حي بنسخة أرشيفية. اختلاف ملفات CI يستلزم مراجعة triggers وpermissions وcheckout/setup versions وSDKs وtest commands وsecrets قبل أي تغيير.

## إعدادات أرشيفية فقط

تشمل المسارات الـ25 غير الموجودة في .github الحي:
- CODEOWNERS وCODEOWNERS.template.
- قوالب Issue وPull Request.
- dependabot.yml وملفات متطلبات npm/ci-tools.
- rulesets/main-protection.json وzizmor.yml.
- workflows تاريخية: codeql.yml وcontrast-scan.yml وdebricked.yml وfrogbot-scan-and-fix.yml وmobsf.yml وnowsecure.yml وrepo-quality.yml وsecurity.yml وsecuritycodescan.yml وunity-mobile.yml، إضافة إلى workflows README.

هذه الملفات لا تُفعّل بمجرد وجودها في ZIP. بعضها يعتمد على خدمات/أسرار أو مشاريع Unity/Android أو أدوات خارجية غير مثبتة في main. ملف CODEOWNERS يحتاج مالكاً فعلياً، والقالب الأرشيفي يحوي placeholder. ruleset قد يغيّر شروط الدمج والدفع، فلا يُطبق دون مراجعة صلاحيات وقرار صريح.

## مجلد github/ القديم

- 15 مساراً تحت github/ لا تحت .github/.
- 13 من 15 لها نسخة counterpart مطابقة بايتياً تحت .github/ في الأرشيف؛ github/workflows/README.md وgithub/workflows/ci.yml مختلفان عن counterpart.
- GitHub يتعامل مع .github كمسار إعدادات المستودع؛ لا تنقل مجلد github/ كبديل أو شجرة إعدادات موازية.
- هذه النسخ لا تُعد Workflows نشطة لمجرد وجودها في ZIP.

## الضوابط والقرار

- KEEP_LIVE لثلاثة Workflows في main؛ لا استبدال.
- ARCHIVE_ONLY / DO_NOT_IMPORT لجميع إعدادات .github الأرشيفية حتى تدقيق workflow-by-workflow وsecret/dependency requirements.
- لا إنشاء CODEOWNERS أو Dependabot أو rulesets ولا تفعيل security scanners أو Unity mobile workflow تلقائياً.
- لا حذف أو استيراد أو نقل أو تغيير إعدادات GitHub.
- لم تُشغّل CI أو Actions.

**الحالة:** STAGE 28 COMPLETE — GITHUB CONFIG PATH/CURRENT WORKFLOW CROSSWALK; LEGACY WORKFLOW ADOPTION BLOCKED PENDING REVIEW.
