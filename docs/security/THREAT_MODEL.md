# Threat Model — v2.2

**الحالة: PROPOSED | نموذج لم يُراجع على نظام لعب/خادم منفذ**

النطاق المستهدف: عميل Android، الشبكة، Dedicated Server، Backend إن اعتُمد، ومفاتيح التوقيع. وجود Domain وCore واختبارات CI لا يعني وجود تطبيق عميل أو خادم أو خدمة منشورة.

| التهديد | الضابط المقصود | الحالة الفعلية |
|---|---|---|
| تزوير الصحة والضرر | حساب خادمي موثوق | غير منفذ؛ اختبارات Domain الحالية لا تنشئ خادماً |
| RPC spam | rate limits | غير منفذ |
| Replay | RequestId وidempotency | توجد اختبارات Domain لرفض RequestId المكرر في النقل؛ لا يوجد بروتوكول شبكة/خادم |
| Serialization abuse | schema وحد الحجم | غير منفذ |
| Speed/teleport | تحقق حركة خادمي | غير منفذ |
| Session theft | جلسات قابلة للإبطال | غير منفذ |
| Secret leak | أسرار خارج Git وفحص تاريخ المستودع | سياسة توثيقية؛ لا يوجد ماسح أسرار مفعّل في GitHub Actions |
| Dependency compromise | تثبيت ومراجعة الاعتماديات | إصدارات Actions في CI مثبتة بـcommit SHA؛ لا توجد Dependabot أو مراجعة آلية شاملة مثبتة |
| CI compromise | أقل صلاحية وعزل المهام | يوجد Domain/Python CI وCore CI بصلاحية `contents: read`؛ لم يُفعّل CodeQL أو gitleaks أو فحص workflow متخصص |

## وضع CI

- ملفا CI موجودان ويشغّلان اختبارات محددة على GitHub Actions.
- آخر تشغيلين موثّقين على `main` عند `b16fe9916b03b25db575fed5eac495182605725c` انتهيا بنجاح؛ راجع `docs/operations/GITHUB_TOOLING.md`.
- الفحص المؤرخ 2026-11-22 لم يُظهر حماية فرع `main` أو repository rulesets؛ راجع وثيقة أدوات GitHub للحالة المؤرخة.
- لا تُعامل نجاح الاختبارات على أنه إثبات لأمن CI أو التطبيق أو الشبكة.

لكل تهديد لاحقاً: Asset وEntry Point وAbuse Case وImpact وControl وTest وEvidence وResidual Risk. لا تُعامل هذه الوثيقة كدليل على أن الضابط موجود.
