# فهرس وثائق CostOfStaying

هذا الفهرس يوجّه إلى الملفات الموجودة في `main`. يوضح `README.md` حالة المشروع المختصرة، ويحدد `CONTRIBUTING.md` و`AGENTS.md` طريقة العمل. يحتفظ `ROADMAP.md` بأولويات التنفيذ. المشروع في مرحلة ما قبل الإنتاج؛ وجود وثيقة أو كود لا يعني اعتماد قاعدة لعب أو قرار معماري.

## الحوكمة والسياسات

- [سياسة الإفصاح الأمني](../SECURITY.md) — طريقة الإبلاغ الخاص عن الثغرات وحدود الاستجابة.
- [مدونة قواعد السلوك](../CODE_OF_CONDUCT.md) — قواعد المشاركة والتواصل.
- [حوكمة المشروع ومصادر الحقيقة](governance/PROJECT_GOVERNANCE.md)
- [سجل السياسات المرجعية](governance/POLICY_REGISTER.md)
- [ضبط الوثائق والفهرسة](governance/DOCUMENT_CONTROL.md)
- [التحكم في التغيير والاعتماد](governance/CHANGE_CONTROL.md)
- [سياسة Git والدمج](governance/GIT_AND_MERGE_POLICY.md)
- [حالات العمل ومعايير الأدلة](governance/STATUS_AND_EVIDENCE.md)
- [سياسة الوكلاء والأتمتة](governance/AI_AND_AUTOMATION_POLICY.md)
- [سجل الحالة المؤرخ للمشروع](governance/PROJECT_STATUS.md)
- [خطة التنفيذ الاستشارية — 2026-11-26 (PROPOSED)](plans/ADVISORY_PLAN_2026-11-26.md)
- [سجل مصالحة الإصلاح — 2026-11-26](operations/RECONCILIATION_2026-11-26.md)
- [تتبع المتطلبات وحدود التحقق](traceability/REQUIREMENTS_TRACEABILITY.md)

تسري السياسات الإجرائية التي يسجلها `POLICY_REGISTER.md` ضمن نطاقها اعتباراً من إدخالها إلى `main`. لا تعتمد هذه السياسات قرارات اللعب أو المعمارية المفتوحة.

## تشغيل الذكاء الاصطناعي والوكلاء

- [دليل تشغيل الذكاء الاصطناعي والمهارات والأدوار](ai/README.md)
- [نموذج تشغيل الوكلاء](ai/AI_AGENT_OPERATING_MODEL.md)
- [سياسة حوكمة الوكلاء والسلامة](ai/AI_AGENT_GOVERNANCE_POLICY.md)
- [بروتوكول تقييم النماذج والوكلاء](ai/AI_MODEL_EVALUATION_PROTOCOL.md)
- [سلسلة الأدوات والحزم](ai/AI_TOOLCHAIN_RECOMMENDATIONS.md)

هذه الوثائق لا تثبت اتصال مزود خارجي أو توفر نموذج/خطة أو اعتماد أداة مدفوعة. لا تُمنح صلاحيات الكتابة أو التنفيذ أو العمليات البعيدة تلقائياً.

## ميثاق وتصميم اللعبة

- [ميثاق المشروع](00_PROJECT_CHARTER.md)
- [وثيقة تصميم اللعبة](01_GAME_DESIGN_DOCUMENT.md)
- [مواصفة قواعد اللعبة](02_GAME_RULES_SPECIFICATION.md)
- [تجربة اللاعب والحلقة الأساسية](03_PLAYER_EXPERIENCE_AND_CORE_LOOP.md)
- [المصطلحات](GLOSSARY.md)
- [أنظمة اللعب](systems/)

قواعد اللعبة في حالة `PROPOSED FOR PLAYTEST`. تفاصيل النتائج الحدية والاتصال ودورة حياة اللاعب ما زالت بحاجة إلى قرار مالك؛ لا تُستنتج الموافقة من كود أو ملف تاريخي.

## المعمارية والقرارات

- [المعمارية التقنية المقترحة](architecture/TECHNICAL_ARCHITECTURE.md)
- [حدود Domain وCore والحالة الفعلية](architecture/DOMAIN_CORE.md)
- [معمارية البيانات](architecture/DATA_ARCHITECTURE.md)
- [معمارية الشبكة المقترحة](architecture/NETWORK_ARCHITECTURE.md)
- [الخلفية والاستضافة](architecture/BACKEND_AND_HOSTING.md)
- [معمارية النشر](architecture/DEPLOYMENT_ARCHITECTURE.md)
- [تخطيط المستودع — مسودة غير ملزمة](architecture/REPO_LAYOUT.md)
- [فهرس ADR وقواعد القبول](adr/README.md)
- [قالب ADR](adr/ADR_TEMPLATE.md)

لا يحتوي `main` حالياً سجلات ADR مقبولة. القالب والفهرس لا يشكلان بحد ذاتهما قراراً معمارياً.

## الجودة والتحقق

- [استراتيجية الاختبارات](quality/TEST_STRATEGY.md)
- [مراجعة عيوب Domain والإصلاحات — 2026-11-25](quality/DOMAIN_DEFECT_REVIEW_2026-11-25.md)
- [مصالحة تقرير الفحص العميق الأرشيفي مع المصدر الحي — 2026-11-26](quality/DEEP_AUDIT_ARCHIVE_CROSSCHECK_2026-11-26.md)
- [خطة اختبار اللعب البشري](quality/PLAYTEST_PLAN.md)
- [مراجعة مخاطر التوازن](quality/BALANCE_RISK_REVIEW.md)
- [ميزانية الأداء المقترحة](quality/PERFORMANCE_BUDGET.md)
- [بروتوكول قياس الأداء](quality/PERFORMANCE_TEST_PROTOCOL.md)
- [توافق الأجهزة](quality/DEVICE_COMPATIBILITY.md)
- [خطة اختبار الأمن](quality/SECURITY_TEST_PLAN.md)
- [نتائج التحقق المؤرخة 2026-11-23](quality/VERIFICATION_SCOPE_NOTE_2026-11-23.md)
- [نتائج التحقق التاريخية 2026-11-21](quality/VERIFICATION_SCOPE_NOTE_2026-11-21.md)
- [سجل تحقق 2026-11-20 التاريخي](quality/VERIFICATION_2026-11-20.md)

اختبار `test_parity.py` يتحقق من محاكي Python فقط. وتشغيلات `run_sessions.py` محاكاة آلية لا تحل محل جلسات اللعب البشرية. كل نتيجة تحقق مقيدة بالـSHA والبيئة والتشغيل الذي أنتجها.

## الأمن والخصوصية

- [معمارية الأمن — أهداف مقترحة](security/SECURITY_ARCHITECTURE.md)
- [نموذج التهديد](security/THREAT_MODEL.md)
- [حدود الثقة](security/TRUST_BOUNDARIES.md)
- [إدارة الأسرار](security/SECRETS_MANAGEMENT.md)
- [خصوصية البيانات — توجيه مقترح](security/DATA_PRIVACY.md)
- [الاستجابة للحوادث — خطة مقترحة](security/INCIDENT_RESPONSE.md)

قراءة قوائم تنبيهات Dependabot وCode Scanning تعذرت بصلاحيات API في الفحص المؤرخ؛ لا يثبت ذلك غياب التنبيهات أو خلوها. لا توجد في هذه الوثائق قناة إفصاح أو SLA مؤكدة ما لم تُحدّد وتُعتمد صراحةً.

## التشغيل والتكامل

- [المصالحة مع المستودع الحي](operations/RECONCILIATION_2026-11-21.md)
- [مصالحة التوثيق والحوكمة — 2026-11-25](operations/RECONCILIATION_2026-11-25.md)
- [سجل إصلاح التوثيق والحوكمة — 2026-11-26](operations/RECONCILIATION_2026-11-26.md)
- [تقرير مصالحة الأرشيفات المرفوعة وخطة التحقيق — 2026-11-26](operations/ARCHIVE_RECONCILIATION_2026-11-26.md)
- [المرحلة 8: مصالحة MASTER UNIFIED — 2026-11-26](operations/ARCHIVE_RECONCILIATION_STAGE8_MASTER_UNIFIED_2026-11-26.md)
- [تقدم المصالحة على مستوى الملفات — 2026-11-26](operations/FILE_LEVEL_RECONCILIATION_PROGRESS_2026-11-26.md)
- [استكمال مصالحة الأرشيفات — المراحل 5–7](operations/ARCHIVE_RECONCILIATION_STAGES_5_7_2026-11-26.md)
- [تصحيح تداخل جرد الأرشيفات — 2026-11-26](operations/ARCHIVE_INVENTORY_CORRECTION_2026-11-26.md)
- [تحقق CI وCodeQL على SHA c8250e7 — 2026-11-26](operations/CI_RECONCILIATION_C8250E7_2026-11-26.md)
- [المرحلة 9 — مصالحة MASTER UNIFIED مع main الحالي — 2026-11-26](operations/ARCHIVE_RECONCILIATION_STAGE9_MASTER_UNIFIED_LIVE_CROSSCHECK_2026-11-26.md)
- [المرحلة 10 — جرد حزمة FINAL-verified والتحقق من manifest — 2026-11-26](operations/ARCHIVE_RECONCILIATION_STAGE10_FINAL_VERIFIED_INVENTORY_2026-11-26.md)
- [المرحلة 11 — تدقيق حزمة FINAL-upload-package وتحذير تعليمات الرفع القديمة — 2026-11-26](operations/ARCHIVE_RECONCILIATION_STAGE11_FINAL_UPLOAD_PACKAGE_2026-11-26.md)
- [المرحلة 12 — تدقيق AI Documentation Toolkit وتصنيف النسخ القديمة — 2026-11-27](operations/ARCHIVE_RECONCILIATION_STAGE12_AI_TOOLKIT_CLEANUP_2026-11-27.md)
- [المرحلة 13 — جرد CostOfStaying.zip ومصالحة مراجع الحزمة — 2026-11-27](operations/ARCHIVE_RECONCILIATION_STAGE13_COSTOFSTAYING_ZIP_2026-11-27.md)
- [المرحلة 14 — تحقق تحليل.zip وتصنيف حزمة المرحلة الأولى — 2026-11-27](operations/ARCHIVE_RECONCILIATION_STAGE14_TAHLIL_ZIP_2026-11-27.md)
- [المرحلة 15 — جرد ملفات لعبة.zip والتحقق من عدم تكرار الفهرس — 2026-11-27](operations/ARCHIVE_RECONCILIATION_STAGE15_GAME_ARCHIVE_2026-11-27.md)
- [المرحلة 16 — إعادة بناء جرد main الحي عند رأس 27 نوفمبر — 2026-11-27](operations/ARCHIVE_RECONCILIATION_STAGE16_LIVE_MAIN_SNAPSHOT_2026-11-27.md)
- [المرحلة 17 — مطابقة Git blob لعينة من الأرشيف مع main — 2026-11-27](operations/ARCHIVE_RECONCILIATION_STAGE17_ARCHIVE_LIVE_BLOB_CROSSWALK_2026-11-27.md)
- [المرحلة 18 — مطابقة Git blob لملفات الاختبارات مع الأرشيف — 2026-11-27](operations/ARCHIVE_RECONCILIATION_STAGE18_TESTS_BLOB_CROSSWALK_2026-11-27.md)
- [المرحلة 19 — جرد مسارات المصدر الأرشيفية القديمة وتعدد نسخ المشاريع — 2026-11-27](operations/ARCHIVE_RECONCILIATION_STAGE19_SOURCE_BLOB_CROSSWALK_2026-11-27.md)
- [المرحلة 20 — تقاطع مسارات الوثائق الأرشيفية مع main — 2026-11-27](operations/ARCHIVE_RECONCILIATION_STAGE20_DOCS_PATH_CROSSWALK_2026-11-27.md)
- [المرحلة 21 — مطابقة Git blob للوثائق المشتركة مع الأرشيف — 2026-11-27](operations/ARCHIVE_RECONCILIATION_STAGE21_DOCS_BLOB_CROSSWALK_2026-11-27.md)
- [المرحلة 22 — مصالحة حوكمة الأرشيف وقرارات ADR المتنافسة — 2026-11-27](operations/ARCHIVE_RECONCILIATION_STAGE22_GOVERNANCE_ADR_TRIAGE_2026-11-27.md)
- [المرحلة 23 — فرز تقارير التدقيق والتحقق الأرشيفية — 2026-11-27](operations/ARCHIVE_RECONCILIATION_STAGE23_AUDIT_EVIDENCE_TRIAGE_2026-11-27.md)
- [المرحلة 24 — مصالحة سجلات محاكاة اللعب الأرشيفية — 2026-11-27](operations/ARCHIVE_RECONCILIATION_STAGE24_SIMULATED_PLAYTEST_TRIAGE_2026-11-27.md)
- [المرحلة 25 — فرز خطط التنفيذ والمعمارية وحزم الرفع التاريخية — 2026-11-27](operations/ARCHIVE_RECONCILIATION_STAGE25_ROADMAP_ARCHITECTURE_TRIAGE_2026-11-27.md)
- [المرحلة 26 — مصالحة ملفات الأرشيف التاريخية ولقطات المستودع القديمة — 2026-11-27](operations/ARCHIVE_RECONCILIATION_STAGE26_LEGACY_ARCHIVE_TRIAGE_2026-11-27.md)
- [المرحلة 27 — مطابقة أدوات المشروع وفرز الأدوات الأرشيفية — 2026-11-27](operations/ARCHIVE_RECONCILIATION_STAGE27_TOOLS_CROSSWALK_2026-11-27.md)
- [المرحلة 28 — مطابقة إعدادات GitHub وWorkflows الأرشيفية — 2026-11-27](operations/ARCHIVE_RECONCILIATION_STAGE28_GITHUB_CONFIG_CROSSWALK_2026-11-27.md)
- [المرحلة 29 — مطابقة ملفات الجذر وإعدادات البناء العامة — 2026-11-27](operations/ARCHIVE_RECONCILIATION_STAGE29_ROOT_CONFIG_CROSSWALK_2026-11-27.md)
- [المرحلة 30 — مراجعة دلالية لاختبارات Core القديمة الأرشيفية — 2026-11-27](operations/ARCHIVE_RECONCILIATION_STAGE30_LEGACY_TEST_COVERAGE_2026-11-27.md)
- [المرحلة 31 — إغلاق مطابقة محتوى ملفات المصدر مع الأرشيف — 2026-11-27](operations/ARCHIVE_RECONCILIATION_STAGE31_SOURCE_FULL_BLOB_CROSSWALK_2026-11-27.md)
- [المرحلة 32 — مراجعة فروق محتوى اختبارات Domain الأرشيفية — 2026-11-27](operations/ARCHIVE_RECONCILIATION_STAGE32_DOMAIN_TEST_SEMANTIC_CROSSWALK_2026-11-27.md)
- [المرحلة 33 — مصفوفة المصالحة الشاملة للمسارات الأرشيفية مع main — 2026-11-27](operations/ARCHIVE_RECONCILIATION_STAGE33_GLOBAL_PATH_CROSSWALK_2026-11-27.md)
- [المرحلة 34 — استخراج ضوابط CI الناضجة من الأرشيف ومقارنتها بالـWorkflows الحية — 2026-11-27](operations/ARCHIVE_RECONCILIATION_STAGE34_CI_GUARDRAILS_SALVAGE_2026-11-27.md)
- [المرحلة 35 — مصالحة متطلبات التتبع الأرشيفية مع خريطة المتطلبات الحية — 2026-11-27](operations/ARCHIVE_RECONCILIATION_STAGE35_REQUIREMENT_TRACEABILITY_SALVAGE_2026-11-27.md)
- [المرحلة 36 — مراجعة salvage لوثائق الأمن والخصوصية الأرشيفية — 2026-11-27](operations/ARCHIVE_RECONCILIATION_STAGE36_SECURITY_DOCS_SALVAGE_2026-11-27.md)
- [المرحلة 37 — مراجعة salvage لوثائق الجودة والأداء واللعب الأرشيفية — 2026-11-27](operations/ARCHIVE_RECONCILIATION_STAGE37_QUALITY_DOCS_SALVAGE_2026-11-27.md)
- [المرحلة 38 — مراجعة salvage لوثائق المعمارية والشبكة الأرشيفية — 2026-11-27](operations/ARCHIVE_RECONCILIATION_STAGE38_ARCHITECTURE_DOCS_SALVAGE_2026-11-27.md)
- [المرحلة 39 — مراجعة salvage لوثائق أنظمة اللعب الأرشيفية — 2026-11-27](operations/ARCHIVE_RECONCILIATION_STAGE39_GAME_SYSTEMS_SALVAGE_2026-11-27.md)
- [المرحلة 40 — تدقيق أصل الاستيراد وملف IMPORT_MANIFEST التاريخي — 2026-11-27](operations/ARCHIVE_RECONCILIATION_STAGE40_IMPORT_PROVENANCE_REVIEW_2026-11-27.md)
- [المرحلة 41 — سجل موحد لمرشحات الاستفادة من الأرشيف — 2026-11-27](operations/ARCHIVE_RECONCILIATION_STAGE41_SALVAGE_CANDIDATE_REGISTER_2026-11-27.md)
- [المرحلة 42 — إصلاح RequestId الفارغ في Suffering Transfer — 2026-11-27](operations/ARCHIVE_RECONCILIATION_STAGE42_TRANSFER_REQUEST_ID_FIX_2026-11-27.md)
- [ملحق المرحلة 8 — مقارنة MASTER UNIFIED مع main وتحصين ignore — 2026-11-26](operations/ARCHIVE_RECONCILIATION_STAGE8_MAIN_CROSSCHECK_2026-11-26.md)
- [أدوات GitHub وحالة CI والأمن](operations/GITHUB_TOOLING.md)
- [خريطة الأدوات والفجوات](operations/TOOLING_MAP.md)
- [النسخ الاحتياطي والاستعادة — خطة مستقبلية](operations/BACKUP_AND_RECOVERY.md)
- [المراقبة والتسجيل — تصور مستقبلي](operations/MONITORING_AND_LOGGING.md)
- [تكامل Unity وCI المحمول — غير منفذ](operations/UNITY_MOBILE_CI.md)
- [تقرير الدمج الوثائقي المؤرخ](operations/MAIN_DOCUMENTATION_MERGE_2026-11-23.md)

## الأرشيف التاريخي

- [تنبيه الأرشيف وحدود سلطته](archive/README.md)
- [جرد مصادر ZIP وحدود التحقق](archive/SOURCE_ARCHIVE_INVENTORY.md)
- [جرد ملفات main الحية — لقطة 2026-11-26](archive/LIVE_MAIN_FILE_INVENTORY_2026-11-26.tsv)
- [جرد ملفات main الحية المحدّث — لقطة c4d333، 2026-11-26](archive/LIVE_MAIN_FILE_INVENTORY_2026-11-26_C4D333.tsv)

ملفات `docs/archive/` مواد تاريخية لا تمثل سياسة نافذة أو دليلاً على حالة `main` الحالية. لا تُستورد سجلات ADR منها إلى `docs/adr/` دون مراجعة وقرار موثق.
