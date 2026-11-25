# فهرس وثائق CostOfStaying

هذا الفهرس يوجّه إلى الملفات الموجودة في `main`. يوضح `README.md` حالة المشروع المختصرة، ويحدد `CONTRIBUTING.md` و`AGENTS.md` طريقة العمل. يحتفظ `ROADMAP.md` بأولويات التنفيذ. المشروع في مرحلة ما قبل الإنتاج؛ وجود وثيقة أو كود لا يعني اعتماد قاعدة لعب أو قرار معماري.

## الحوكمة والسياسات

- [حوكمة المشروع ومصادر الحقيقة](governance/PROJECT_GOVERNANCE.md)
- [سجل السياسات المرجعية](governance/POLICY_REGISTER.md)
- [ضبط الوثائق والفهرسة](governance/DOCUMENT_CONTROL.md)
- [التحكم في التغيير والاعتماد](governance/CHANGE_CONTROL.md)
- [سياسة Git والدمج](governance/GIT_AND_MERGE_POLICY.md)
- [حالات العمل ومعايير الأدلة](governance/STATUS_AND_EVIDENCE.md)
- [سياسة الوكلاء والأتمتة](governance/AI_AND_AUTOMATION_POLICY.md)
- [سجل الحالة المؤرخ للمشروع](governance/PROJECT_STATUS.md)
- [تتبع المتطلبات وحدود التحقق](traceability/REQUIREMENTS_TRACEABILITY.md)

تسري السياسات الإجرائية التي يسجلها `POLICY_REGISTER.md` ضمن نطاقها اعتباراً من إدخالها إلى `main`. لا تعتمد هذه السياسات قرارات اللعب أو المعمارية المفتوحة.

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
- [أدوات GitHub وحالة CI والأمن](operations/GITHUB_TOOLING.md)
- [خريطة الأدوات والفجوات](operations/TOOLING_MAP.md)
- [النسخ الاحتياطي والاستعادة — خطة مستقبلية](operations/BACKUP_AND_RECOVERY.md)
- [المراقبة والتسجيل — تصور مستقبلي](operations/MONITORING_AND_LOGGING.md)
- [تكامل Unity وCI المحمول — غير منفذ](operations/UNITY_MOBILE_CI.md)
- [تقرير الدمج الوثائقي المؤرخ](operations/MAIN_DOCUMENTATION_MERGE_2026-11-23.md)

## الأرشيف التاريخي

- [تنبيه الأرشيف وحدود سلطته](archive/README.md)
- [جرد مصادر ZIP وحدود التحقق](archive/SOURCE_ARCHIVE_INVENTORY.md)

ملفات `docs/archive/` مواد تاريخية لا تمثل سياسة نافذة أو دليلاً على حالة `main` الحالية. لا تُستورد سجلات ADR منها إلى `docs/adr/` دون مراجعة وقرار موثق.
