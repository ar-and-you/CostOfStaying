# المرحلة 46 — مراجعة أدوات الأرشيف غير الموجودة في main (2026-11-27)

## الرأس والنطاق
- HEAD قبل المرحلة: `19e27c8b088921239b54c89d689e0caa7f95a3f2`.
- النطاق: ستة مسارات وظيفية أرشيفية تحت `tools/` غير موجودة في main، مع مراجعة اختلاف نسخ `tools/audit.py` و`tools/github_config_tests/*`.
- استُبعد `tools/balance_simulator/__pycache__/simulate.cpython-312.pyc` لأنه bytecode مولّد.
- تمت مراجعة الأدوات الحية ذات الصلة: `tools/docs_audit.sh`، balance_simulator، playtest، `AGENTS.md`، `docs/operations/GITHUB_TOOLING.md` وWorkflows الثلاثة الحالية.

## جرد الملفات والقرار

| المسار الأرشيفي غير الموجود في main | الغرض | القرار |
|---|---|---|
| `tools/agents/dispatch.py` | موزع مهام يقرأ EXECUTION_BOARD وAGENT_CHARTER وWORK_PLAN_V2_DETAILED | `DO_NOT_IMPORT`: يعتمد على مخطط حوكمة قديم |
| `tools/audit.py` | فحص روابط وفهرس/status headers واتساق ADR | `SALVAGE IDEAS ONLY`: توجد أداة حية لفحص روابط Markdown والوصول؛ منطق catalog/status/ADR يحتاج إعادة تصميم متوافقة مع الحوكمة الحالية |
| `tools/github_config_tests/README.md` | تعليمات تشغيل اختبارات إعداد GitHub | `DO_NOT_IMPORT`: تصف workflows وملفات اعتماد غير موجودة |
| `tools/github_config_tests/test_github_config.py` | اختبارات workflow hygiene وDependabot وCODEOWNERS وrulesets وsecurity وUnity | `DO_NOT_IMPORT AS-IS`: يعتمد على security.yml وrepo-quality.yml وunity-mobile.yml وdependabot.yml وrulesets وCODEOWNERS.template غير الموجودة |
| `tools/repo_setup/README.md` | شرح توليد CODEOWNERS | `OWNER-DEPENDENT`: يتطلب قالباً ومالكاً حقيقياً |
| `tools/repo_setup/set_owner.py` | توليد/فحص .github/CODEOWNERS | `DO_NOT_IMPORT NOW`: لا يعمل دون القالب وقرار المالك |

## اختلافات النسخ

### tools/audit.py
- نسخة `CostOfStaying-final.zip`: 4,322 bytes؛ نسخة `CostOfStaying-FINAL-verified-2026-11-20.zip`: 5,343 bytes.
- النسخة الأكبر تضيف التحقق من SHA-256 لملفات IMPORT_MANIFEST قبل إعفاء status header، وفحص إدراج ADR في DECISIONS_LOG.
- كلاهما يفترضان مسارات `docs/governance/DOCUMENT_GOVERNANCE.md` و`DECISIONS_LOG.md` و`docs/governance/adr/`؛ لا يتوافقان مباشرة مع بنية main الحالية.

### github_config_tests
- نسختا الاختبار مختلفتان: 22,363 و33,037 bytes.
- النسخة الأكبر توسع اختبارات YAML/workflow/secrets/download checks وCodeQL وUnity وDependabot وCODEOWNERS وrulesets والقوالب.
- الاختبارات مرتبطة بملفات غير موجودة في main؛ فشلها عند نقلها كما هي سيكون فشل fixture/config mismatch لا دليلاً على عيب في الملفات الحية.

## فرص salvage التي تستحق إعادة تصميم
1. اختبارات static config صغيرة للـ3 Workflows الحالية فقط: SHA pinning، أقل الصلاحيات، timeout، `persist-credentials: false`، ورفض triggers/expressions غير الآمنة.
2. فحوص catalog/status/ADR بعد تحديد مصدر الحقيقة الحالي والاستثناءات، دون نسخ قواعد DOCUMENT_GOVERNANCE القديمة.
3. لا تُنشأ CODEOWNERS قبل اعتماد مالك/فريق فعلي وقالب صالح واختبارات توافق.

## الحدود
- لا استيراد أو نقل أو حذف أو تعديل للأدوات الحية.
- لم تثبت تبعيات PyYAML/npm ولم تُشغّل أدوات الأرشيف.
- لم تُقرأ أو تُغيّر إعدادات GitHub الفعلية.
- التصنيف يخص إدخال الملفات إلى main؛ تبقى النسخ الأصلية في الأرشيف التاريخي.

**الحالة:** `STAGE 46 COMPLETE — LEGACY TOOLING REVIEWED; REUSABLE CHECK IDEAS IDENTIFIED; NO DIRECT IMPORT.`
