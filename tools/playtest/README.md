# Playtest Harness — v0.2 (SIMULATION ONLY)

**الحالة: أداة محاكاة استكشافية؛ ليست اختبار لعب بشرياً.**

يشغّل `tools/playtest/run_sessions.py` مباريات اصطناعية باستخدام محاكي Python. لا يتصل ببِلد اللعبة أو Unity أو خادم شبكي، ولا يجمع فهم اللاعبين أو معدل الانسحاب البشري. لذلك لا تحقق مخرجاته خطة `docs/quality/PLAYTEST_PLAN.md` ولا أي بوابة اعتماد تتطلب جلسات بشرية.

## الاستخدام

شغّل من جذر المستودع:

```bash
python3 tools/playtest/run_sessions.py --sessions 10 --preset default --seed 1 --output /tmp/costofstaying-sim
python3 tools/playtest/run_sessions.py --sessions 10 --preset fast-decay --seed 100 --output /tmp/costofstaying-fast-decay
```

يجب أن تكون قيمة `--sessions` عدداً صحيحاً موجباً (1 أو أكثر)؛ يرفض السكربت الصفر والقيم السالبة قبل إنشاء أي ملفات. إذا حُدد `--output` بمسار نسبي، يُفسر نسبةً إلى جذر المستودع. المسار الافتراضي هو مجلد مؤقت خارج المستودع. أبقِ المخرجات خارج Git، ولا ترفع سجلات خام أو بيانات قد تكشف هوية مشارك.

## المخرجات الفعلية

ينشئ السكربت أسماء مؤرخة، ولا ينشئ أسماء ثابتة مثل `summary.csv` أو `report.md`:

- `session-YYYYMMDD-HHMMSS-NN.json` — سجل لكل مباراة محاكاة.
- `summary-YYYYMMDD-HHMMSS.csv` — ملخص CSV للمباريات التي شُغّلت.
- `report-YYYYMMDD-HHMMSS.md` — تقرير توزيع النتائج والمتوسطات.

يتضمن سجل الجلسة `simulator_version` و`preset` و`seed` وملاحظة صريحة بأن التشغيل محاكاة. لا تُعد هذه السجلات سجلات اختبار بشري أو دليلاً على صلاحية التوازن.

## حدود التفسير

- لا تستخدم كلمة `VERIFIED` لوصف توازن اللعبة اعتماداً على هذه الأداة.
- عدد الجلسات الاصطناعية لا يحقق حد العينة البشرية في `docs/quality/PLAYTEST_PLAN.md`.
- لا تستنتج فهم اللاعبين أو العدالة أو quit rate من بيانات المحاكي.
- لا تمثل الأداة C# ولا تثبت parity بين Python وC#.
- يجب توثيق إصدار القواعد والـbuild وموافقة المشاركين والبيانات المجهولة الهوية في مسار الاختبار البشري المعتمد، مع تخزين السجلات الخام خارج Git.

راجع `tools/playtest/run_sessions.py` و`docs/quality/PLAYTEST_PLAN.md` قبل تفسير أي نتيجة.
