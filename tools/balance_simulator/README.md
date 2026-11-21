# Balance Simulator — v0.2 (EXPLORATORY)
**الحالة: أداة استكشافية؛ نتائجها ليست قياساً أو توازناً معتمداً**

محاكي مبسط مكتوب بـ Python لاستكشاف أثر معدلات التآكل وفترات Collapse وعمليات Transfer. يستلهم بعض قواعد `src/CostOfStaying.Domain` لكنه ليس نسخة مكافئة تنفيذياً ولا بديلاً عن مكتبة C#.

> ⚠️ كل ما ينتجه هذا المحاكي هو **فرضية اختبار** وليس أداءً معتمداً.
> لا يُستخدم كلمة VERIFIED إلا بعد جلسات لعب موثقة على بِلد فعلي.

## التشغيل
```bash
python3 tools/balance_simulator/simulate.py --preset default --seed 42
python3 tools/balance_simulator/test_parity.py
python3 tools/balance_simulator/sweep.py --runs 1 --output /tmp/costofstaying-sim/sweep.csv
python3 tools/playtest/run_sessions.py --sessions 10 --output /tmp/costofstaying-sim
```

## الملفات
- `simulate.py` — محاكاة مباراة واحدة (4 لاعبين، قرارات عشوائية قابلة للتكرار).
- `sweep.py` — مسح باراميتري لمعدلات التآكل وفترات Collapse.
- `presets.json` — إعدادات استكشافية؛ لا يوجد تحقق آلي لتطابقها مع `MatchConfig`.

## العلاقة مع C# وحدود الاختبارات
لا توجد حالياً مقارنة آلية مباشرة بين Python وC#؛ لذلك لا يجوز وصف الاختبارات بأنها parity أو اعتبار نجاحها دليلاً على تطابق التنفيذين. يحتفظ الملف `test_parity.py` باسمه التاريخي فقط، ويختبر سلوك المحاكي Python وحده.

عند تعديل منطق C# أو قواعد اللعبة، راجع المحاكي يدوياً وحدّث اختبارات الانحدار، ثم أنشئ اختبارات cross-language حقيقية قبل ادعاء التكافؤ. القيم الحالية اقتراحات للاستكشاف، لا قرارات توازن معتمدة.

السجلات الناتجة محاكاة آلية لا جلسات لعب بشري، ولا تحقق أي شرط اعتماد أو مراجعة جودة.
