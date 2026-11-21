# فهرس وثائق CostOfStaying

هذا الفهرس يوجّه إلى الوثائق الموجودة في `main`. راجع `README.md` للحالة المختصرة وأوامر التحقق، و`CONTRIBUTING.md` لمسار المساهمة والاختبارات، و`ROADMAP.md` لترتيب العمل وحالاته. حالة ما قبل الإنتاج تنطبق على المشروع كله؛ لا تعني الوثيقة أو الكود وحدهما اعتماد القواعد أو التوازن.

## ميثاق وتصميم اللعب

- [ميثاق المشروع](00_PROJECT_CHARTER.md)
- [وثيقة تصميم اللعبة](01_GAME_DESIGN_DOCUMENT.md)
- [مواصفة القواعد](02_GAME_RULES_SPECIFICATION.md)
- [تجربة اللاعب والحلقة الأساسية](03_PLAYER_EXPERIENCE_AND_CORE_LOOP.md)
- [المصطلحات](GLOSSARY.md)
- [أنظمة اللعب](systems/)

## المعمارية والقرارات

- [المعمارية التقنية](architecture/TECHNICAL_ARCHITECTURE.md)
- [حدود Domain وCore وحالة الترحيل](architecture/DOMAIN_CORE.md)
- [طبقة البيانات](architecture/DATA_ARCHITECTURE.md)
- [الشبكات](architecture/NETWORK_ARCHITECTURE.md)
- [الاستضافة الخلفية](architecture/BACKEND_AND_HOSTING.md)
- [النشر](architecture/DEPLOYMENT_ARCHITECTURE.md)
- [تخطيط المستودع](architecture/REPO_LAYOUT.md)
- [حالة سجلات ADR ومصادرها](adr/README.md)

## الجودة والاختبار

- [استراتيجية الاختبارات](quality/TEST_STRATEGY.md)
- [خطة اللعب التجريبي](quality/PLAYTEST_PLAN.md)
- [مراجعة مخاطر التوازن](quality/BALANCE_RISK_REVIEW.md)
- [متطلبات الأداء واختبارات الأداء](quality/PERFORMANCE_BUDGET.md) و[البروتوكول](quality/PERFORMANCE_TEST_PROTOCOL.md)
- [توافق الأجهزة](quality/DEVICE_COMPATIBILITY.md)
- [نطاق التحقق وسجل نتائجه](quality/VERIFICATION_SCOPE_NOTE_2026-11-21.md) و[تحقق 2026-11-20](quality/VERIFICATION_2026-11-20.md)

## الأمن والخصوصية

- [معمارية الأمن](security/SECURITY_ARCHITECTURE.md)
- [نموذج التهديد](security/THREAT_MODEL.md)
- [حدود الثقة](security/TRUST_BOUNDARIES.md)
- [إدارة الأسرار](security/SECRETS_MANAGEMENT.md)
- [خصوصية البيانات](security/DATA_PRIVACY.md)
- [الاستجابة للحوادث](security/INCIDENT_RESPONSE.md)
- [خطة اختبار الأمن](quality/SECURITY_TEST_PLAN.md)

## العمليات والتكامل

- [حالة المصالحة مع المستودع الحي](operations/RECONCILIATION_2026-11-21.md)
- [أدوات GitHub وCI](operations/GITHUB_TOOLING.md)
- [خريطة الأدوات والفجوات](operations/TOOLING_MAP.md)
- [النسخ الاحتياطي والاستعادة](operations/BACKUP_AND_RECOVERY.md)
- [المراقبة والتسجيل](operations/MONITORING_AND_LOGGING.md)
- [تكامل Unity وCI المحمول](operations/UNITY_MOBILE_CI.md)

## حالة القرارات

`docs/adr/README.md` يسجل أن شجرة `main` الحالية لا تحتوي ADRs معتمدة، ويحدد طريقة التعامل مع نسخ الأرشيف المتباينة. لا تُرفع حالة عنصر إلى `APPROVED` أو `VERIFIED` من هذا الفهرس وحده.
