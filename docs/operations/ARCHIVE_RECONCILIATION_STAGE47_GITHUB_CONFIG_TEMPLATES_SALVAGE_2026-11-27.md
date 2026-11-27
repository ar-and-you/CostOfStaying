# المرحلة 47 — مراجعة إعدادات GitHub والقوالب الأرشيفية غير المنقولة (2026-11-27)

## الرأس والنطاق
- HEAD قبل المرحلة: `d4c25a6482b18c3c4542ea65a9d6f1b08b9d3776`.
- النطاق: 13 مسار إعداد/قالب أرشيفياً غير موجود في main: CODEOWNERS template، خمسة Issue templates، PR template، Dependabot، ملفات requirements الثلاثة، ruleset template، وzizmor config.
- Workflows الأرشيفية وملف workflows README خضعت لمراجعة منفصلة في المرحلة 34؛ لا تكرار لها هنا.
- المرجع هو main؛ لا تُطبق أي إعدادات أرشيفية تلقائياً.

## نتائج المراجعة حسب الفئة

### 1. قوالب Issues وPull Requests
- `.github/ISSUE_TEMPLATE/adr_proposal.yml` و`feature_request.yml` يعكسان مسار ADR والحوكمة القديم؛ يشيران إلى `DOCUMENT_GOVERNANCE.md` و`SOURCES_AND_REFERENCES.md` واصطلاحات غير مطابقة بالضرورة لمصادر الحقيقة الحالية.
- `agent_task.yml` مرتبط بعشرة وكلاء وGATE-0..7 و`WORK_PLAN_V2_DETAILED` و`EXECUTION_BOARD` القديمة؛ لا يعكس كتالوج/مهارات الوكلاء الحالي.
- `bug_report.yml` يطلب تشغيل `python3 tools/audit.py` غير الموجود في main.
- `feature_request.yml` وPR template يربطان مراجعة التوازن/الوثائق بمسارات الحوكمة القديمة؛ الفكرة مفيدة لكن الصياغة تحتاج تحديثاً قبل الاستخدام.
- `ISSUE_TEMPLATE/config.yml` يحيل تقرير الثغرات إلى مستودع `live-egypt/CostOfStaying` القديم؛ هذا رابط غير صالح كقناة دعم للمستودع الحالي، ويجب عدم نقله.
- القرار: `DO_NOT_IMPORT AS-IS`. يمكن لاحقاً إنشاء قوالب مبسطة متوافقة مع سياسات main، مع رابط أمني رسمي للمستودع الحالي بعد التحقق.

### 2. CODEOWNERS
- `.github/CODEOWNERS.template` يحتوي placeholder `@REPLACE_WITH_OWNER` لكل المسارات؛ لا يصلح كملف CODEOWNERS فعال.
- أداة التوليد الأرشيفية في المرحلة 46 تعتمد على هذا القالب.
- القرار: `OWNER-DEPENDENT / NOT READY`. لا إنشاء CODEOWNERS أو قواعد مراجعة دون اسم حساب/فريق حقيقي وموافقة المالك.

### 3. Dependabot والاعتماديات
- `.github/dependabot.yml` يضبط NuGet وGitHub Actions وpip وnpm، لكن مسارات pip/npm تقع في `.github/requirements` ولا توجد هذه الحزم/الملفات في main حالياً.
- مسارات NuGet تتضمن الجذر ومجلد Core.Tests؛ لا يجوز افتراض أن بقية ecosytems لازمة أو صالحة دون ملفات manifest حية.
- `ci-tools.txt` يثبت PyYAML/zizmor/codespell؛ وpackage.json/lock يثبت markdownlint-cli2. هذه الحزم مرتبطة بـrepo-quality workflow الأرشيفي غير الموجود.
- القرار: `DO_NOT_IMPORT` حالياً. عند إضافة dependency automation يجب أن يقتصر config على manifests الموجودة فعلياً وأن تُختار حزم CI بعد قرار واعتماد.

### 4. Ruleset
- ملف `.github/rulesets/main-protection.json` له نسختان مختلفتان.
- النسختان تحملان `enforcement: disabled`; النسخة النهائية verified تضع required approvals = 0، والنسخة الأقدم = 1.
- قوائم required checks تشمل أسماء لا توجد في workflows الحالية، مثل Workflow lint وGitHub config tests وDocs quality وdotnet format وSecret scan؛ تطبيقها قد يمنع الدمج/الدفع بسبب checks غير منتجة.
- القرار: `DO_NOT_APPLY`. ملف JSON تاريخي لا يثبت أن ruleset فعّال على GitHub. يلزم قراءة إعدادات GitHub الحالية وصلاحيات المالك وتحديد checks التي تعمل فعلاً على SHA حديث قبل أي تطبيق.

### 5. zizmor config
- `.github/zizmor.yml` يستثني nowsecure.yml وsecuritycodescan.yml من excessive-permissions، وهما workflow templates تاريخيان لا يظهران في main.
- القرار: `DO_NOT_IMPORT` حتى يُعتمد zizmor workflow حي وتُعاد مراجعة الاستثناءات؛ لا تنقل ignores يتيمة.

## القرار الموحد

| المجموعة | القرار |
|---|---|
| Issue/PR templates | لا نقل مباشر؛ إعادة كتابة متوافقة عند الحاجة |
| CODEOWNERS template | ينتظر اعتماد مالك حقيقي |
| Dependabot + requirements | لا نقل قبل وجود manifests وقرار تبعيات |
| Ruleset | لا تطبيق؛ status contexts قديمة وenforcement معطل |
| zizmor config | لا نقل؛ الاستثناءات تخص workflows غير موجودة |

لا شيء في هذه الملفات يبرر تعديل main في هذه المرحلة. لا حذف أو استبدال أو نقل أو استيراد. تظل نسخ الأرشيف محفوظة كمراجع تاريخية.

## الحدود
- لم تُجرَ كتابة إلى إعدادات GitHub أو branch protection/rulesets.
- لم تُثبت تبعيات، ولم تُشغّل Dependabot أو zizmor أو actionlint.
- لم يُتحقق من صلاحيات owner/admin أو تفعيل Secret Scanning في هذه المرحلة.
- هذه مراجعة محتوى أرشيفي وليست إثباتاً لحالة إعدادات GitHub الفعلية.

**الحالة:** `STAGE 47 COMPLETE — ARCHIVED GITHUB TEMPLATES/CONFIG REVIEWED; NO DIRECT IMPORT OR RULESET APPLICATION.`
