# المرحلة 75 — دمج فحص Playtest المحاكى وأدوات Audit في CI (2026-11-27)

## الرأس والنطاق

- HEAD قبل المرحلة: `18fa3d11630279f09eda62eacb85957c5a19583e`.
- تمت مراجعة شجرة `tools/` وملفات CI الثلاثة وخطة اللعب البشري وملفات المحاكاة.
- النطاق: تشغيل smoke test محدود لمحاكي اللعب في CI، وتحديد ما هو موجود فعلياً من أدوات التدقيق.
- لا يُعد هذا اختبار لعب بشرياً ولا اعتماداً لقواعد اللعبة أو توازنها.

## 1. ما كان موجوداً قبل التعديل

| الأداة | الحالة السابقة |
|---|---|
| `tools/playtest/test_cli.py` | داخل Domain/Python CI؛ يختبر رفض عدد الجلسات صفر/سالب |
| `tools/playtest/run_sessions.py` | محاكي جلسات اصطناعية؛ لا يتصل ببناء اللعبة أو Unity أو خادم |
| `tools/docs_audit.sh` | يعمل في Documentation Governance؛ يفحص روابط Markdown وقابلية الوصول من فهرس الوثائق |
| `tools/github_workflow_tests/test_workflow_hygiene.py` | يعمل في Domain/Python CI؛ يفحص قائمة workflows والصلاحيات ومراجع SHA وtimeouts وcheckout وtriggers |
| `tools/audit.py` | غير موجود في شجرة main المفحوصة |
| `tools/github_config_tests/` | غير موجود في الشجرة المفحوصة |
| repository setup helpers | لم تظهر أدوات إعداد مستقلة ضمن `tools/` |

لا تُنسب تغطية أمنية أو تدقيق إعدادات GitHub إدارية إلى اختبارات workflow hygiene؛ نطاقها هو محتوى YAML المتتبع فقط.

## 2. التعديل المنفذ

أُضيف `tools/playtest/test_smoke.py`، ويشغّل جلسة محاكاة واحدة ببذرة ثابتة داخل `TemporaryDirectory`، ثم يتحقق من:

- خروج CLI بنجاح ضمن مهلة 30 ثانية.
- إنشاء ملف JSON واحد للجلسة وCSV واحد للملخص وMarkdown واحد للتقرير.
- ثبات seed وpreset المتوقعين في سجل الجلسة.
- وجود وسم SIMULATED في سجل الجلسة والتقرير.
- وجود صف واحد في CSV وعدم تسرب المخرجات إلى المستودع.

أُضيفت خطوة `Run bounded simulated-playtest smoke test` إلى `.github/workflows/ci.yml`. ويشمل `compileall` الحالي الملف الجديد تلقائياً لأنه يفحص مجلد `tools/playtest` كاملاً.

حُدّث `tools/playtest/README.md` لتوضيح نطاق smoke test وحدوده.

## 3. لماذا هذا النطاق آمن ومناسب

- جلسة واحدة فقط؛ لا حملة محاكاة طويلة أو غير محدودة داخل CI.
- seed وpreset ثابتان لتسهيل إعادة الإنتاج.
- كل المخرجات داخل مجلد مؤقت يحذف بعد الاختبار.
- timeout يمنع تعليق الوظيفة بلا حد.
- الاختبار يتحقق من سلامة مسار الأداة وملفات المخرجات، لا من صحة توازن اللعبة.
- لا يرسل بيانات خارجية ولا يقرأ أسراراً ولا يتصل بخدمة لعب.

## 4. ما بقي خارج النطاق

1. لا توجد أداة `tools/audit.py` في الشجرة الحية؛ لا يمكن دمج ملف غير موجود دون العثور على مصدر موثوق ومراجعة محتواه.
2. لا توجد اختبارات `tools/github_config_tests/`؛ فحص YAML لا يثبت إعدادات المستودع الفعلية أو Branch protection أو Dependabot alerts.
3. لا يوجد GitHub administration permission في الاتصال الحالي.
4. لا يُشغّل CI جلسات بشرية أو يبني Unity/Android أو يتصل بخادم/transport غير موجود.
5. لا يحل smoke test محل خطة `docs/quality/PLAYTEST_PLAN.md` أو شرط عشر جلسات بشرية صالحة.
6. لا تُستورد أدوات Audit أرشيفية لمجرد تشابه الاسم؛ يلزم provenance ومراجعة دلالية واختبارات.

## 5. الملفات المتغيرة في المرحلة

- `tools/playtest/test_smoke.py` — جديد.
- `.github/workflows/ci.yml` — إضافة خطوة smoke test محدودة.
- `tools/playtest/README.md` — توثيق تشغيل CI وحدود التفسير.

## 6. التحقق

تُربط نتيجة الاختبار والتشغيل بـSHA الذي يحتوي التغييرات. عند إعداد هذا التقرير كانت تشغيلات CI على SHA المرحلة السابقة قد بدأت/اكتملت جزئياً؛ لا يُعلن نجاح smoke test الجديد قبل قراءة تشغيل Domain/Python على SHA الذي يتضمن `test_smoke.py`. يجب تحديث سجل التحقق بعد انتهاء تشغيلات Actions الفعلية.

**الحالة:** STAGE 75 IMPLEMENTED — BOUNDED SIMULATED-PLAYTEST SMOKE TEST ADDED TO CI; LEGACY AUDIT TOOL IMPORT REMAINS OUT OF SCOPE PENDING VERIFIED SOURCE.
