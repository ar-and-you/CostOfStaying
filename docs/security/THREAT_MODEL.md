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
| Secret leak | أسرار خارج Git وفحص تاريخ المستودع | Secret scanning وPush protection آخر حالة معروفة لهما active في قراءة 2026-11-24؛ لم تتوفر قراءة أحدث، ولا يوجد تقرير مسح تاريخ متخصص أو gitleaks في Actions |
| Dependency compromise | تثبيت ومراجعة الاعتماديات | إصدارات Actions مثبتة بـcommit SHA؛ آخر قراءة لتنبيهات Dependabot أعادت 403، وتحديثات الأمان الآلية آخر حالة موثقة لها disabled في 2026-11-24؛ لا استنتاج عن خلو التنبيهات |
| CI compromise | أقل صلاحية وعزل المهام | يوجد Domain/Python CI وCore CI بصلاحية `contents: read`؛ CodeQL default setup مهيأ للـC# وPython وActions. نجح `actionlint v1.7.12` و`zizmor v1.33.1` في Domain/Python CI على رأس المراجعة `d8c66e85d2d4b8353595b24dd2df38382ead2adb`؛ ما زالا غير مدمجين في `main`، ولا يوجد gitleaks |
| Pending-session exhaustion | TTL وauth rate limits | SessionRegistry/ConnectionRegistry bounded، لكن لا pending TTL أو failed-auth rate limit داخل Core؛ host مسؤول | 
| Queue/payload abuse | raw frame quotas + bounded validator | Core queue bounded وpayload validator fail-closed؛ validator يعمل قبل auth ويجب أن يكون bounded، ولا توجد raw frame/serializer quotas | 
| Mutable command payload | immutable DTO/ownership | validator يعيد التحقق بعد dequeue لكنه لا clone/freeze؛ host مسؤول عن immutable DTO أو exclusive ownership | 
| Event/ledger memory growth | retention/caps/persistence | Domain event/request/echo stores وCore BetrayalLedger._events/_scores بلا سياسة retention/cap | 
| Cross-match ledger contamination | per-match scope enforcement | على `main` عند `e7e5b5e...` لم يكن `CoreComposition` مربوطاً بـ`MatchId`؛ على فرع المراجعة `security/core-trust-boundaries-phase0` أصبح `Create(MatchId)` إلزامياً وربطت موارد المباراة بالنطاق نفسه مع اختبارات عزل. هذا التغيير لم يُدمج في `main` بعد؛ global active-match cap ودورة حياة المضيف ما زالا غير محسومين |

## وضع CI والأدلة المرتبطة بالالتزام

- `main` الحالي عند SHA `e7e5b5e54f46490c44aaf30888280d55239bb670`؛ لم تُرجع القراءة تشغيلات CI مرتبطة بهذا SHA، لذلك لا تسجل هذه اللقطة نجاحاً حديثاً على `main`.
 - فرع المراجعة `security/core-trust-boundaries-phase0` عند SHA `d8c66e85d2d4b8353595b24dd2df38382ead2adb` أكمل بنجاح:
  - Domain and Python CI — run `46131891429`.
  - Core CI (.NET 10) — run `46131891394`.
  - Documentation Governance — run `46131891367`.
- ضمن Domain/Python CI على SHA نفسه: `actionlint v1.7.12` و`zizmor v1.33.1` نجحا؛ workflow hygiene 7/7، Python compile، Domain NUnit 62/62، simulator 11 tests، CLI 2/2، وsmoke test 1/1.
- Core NUnit نجح 257/257. اكتملت CodeQL analyses للغات C# وPython وActions بنجاح؛ PR CodeQL check أفاد بعدم وجود تنبيهات جديدة في الملفات التي غيّرها PR. هذا لا يثبت خلو كل التنبيهات المفتوحة في المستودع.
- CodeFactor أعاد `failure` مع issue واحدة بدرجة Minor؛ لم تتوفر annotation لتحديد الملف/السطر، لذلك لم تُنسب إلى ملف بالتخمين ولم يُنفذ autofix أعمى. Codacy أعاد `success` وذكر عدم وجود issues على SHA الحالي.
- لا يوجد دليل على خلو CodeQL alerts أو Dependabot alerts. صلاحية التكامل الحالية `admin=false`؛ branch-protection endpoint أعاد `403 Resource not accessible by integration` في 2026-11-28، لذا الحماية وإعدادات secret scanning/push protection `UNKNOWN / NOT VERIFIED`.
- لا تثبت هذه النتائج وجود Unity أو Android build أو transport/server runtime أو اختبار شبكة/لعب بشري.

لكل تهديد لاحقاً: Asset وEntry Point وAbuse Case وImpact وControl وTest وEvidence وResidual Risk. لا تُعامل هذه الوثيقة كدليل على أن الضابط موجود.
