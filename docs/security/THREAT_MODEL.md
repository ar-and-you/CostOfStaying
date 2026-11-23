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
| Secret leak | أسرار خارج Git وفحص تاريخ المستودع | Secret scanning وPush protection مفعّلان على GitHub؛ لا يوجد تقرير مسح تاريخ متخصص، ولا gitleaks في Actions |
| Dependency compromise | تثبيت ومراجعة الاعتماديات | إصدارات Actions في CI مثبتة بـcommit SHA؛ تفعيل تنبيهات Dependabot سُجل في مراجعة 2026-11-22، لكن تعذر فحص التنبيهات عبر API في 2026-11-24 (403). تحديثات الأمان الآلية معطّلة ولا توجد مراجعة آلية شاملة |
| CI compromise | أقل صلاحية وعزل المهام | يوجد Domain/Python CI وCore CI بصلاحية `contents: read`؛ CodeQL default setup مهيأ للـC# وPython وActions، ولا يوجد gitleaks أو فحص workflow متخصص |

## وضع CI

- ملفا CI موجودان ويشغّلان اختبارات محددة على GitHub Actions.
- آخر تشغيلين موثّقين لاختباري Domain/Python وCore على `main` عند `1b209b79261cba617bb68615746f115970422a8f` انتهيا بنجاح؛ راجع `docs/operations/GITHUB_TOOLING.md`.
- أظهر الفحص المؤرخ 2026-11-24 أن `main` غير محمي وأن قائمة rulesets فارغة؛ راجع وثيقة أدوات GitHub.
- أعادت واجهة قراءة تنبيهات Code Scanning `403 Resource not accessible by integration` في 2026-11-24؛ لم تُثبت هنا حالة أي تنبيه. لا تُعامل نجاح الاختبارات على أنه إثبات لأمن CI أو التطبيق أو الشبكة.

لكل تهديد لاحقاً: Asset وEntry Point وAbuse Case وImpact وControl وTest وEvidence وResidual Risk. لا تُعامل هذه الوثيقة كدليل على أن الضابط موجود.
