# هيكل المستودع — الواقع الحالي وسياسة الإضافة

> تنبيه حوكمي: أرقام ADR المشار إليها في نسخ سابقة هي مواد أرشيفية ما لم توجد في `docs/adr/` بحالة مقبولة على `main`.

## المسارات المرجعية

- `src/CostOfStaying.Domain/`: منطق المباراة والقواعد الحالية.
- `src/CostOfStaying.Core/`: عقود وخدمات الأوامر والجلسات.
- `tests/CostOfStaying.Domain.Tests/`: اختبارات Domain/.NET 8.
- `tests/CostOfStaying.Core.Tests/`: اختبارات Core/.NET 10.
- `tools/balance_simulator/`: محاكي Python واستكشاف معاملات.
- `tools/playtest/`: توليد جلسات اصطناعية.
- `docs/`: مواصفات، معمارية، سياسات، جودة، أمن، وتشغيل.
- `.github/workflows/ci.yml`: Domain/Python CI.
- `.github/workflows/core-ci.yml`: Core CI.

## قواعد الهيكل

1. لا تنقل ملفات إعدادات SDK أو MSBuild من مواقعها دون اختبار discovery.
2. أبقِ `global.json` الجذري خاصاً بخط .NET 8، وملف Core المحلي داخل مجلد اختبارات Core خاصاً بـ.NET 10.
3. لا تضع ZIP أو build output أو cache أو أسراراً في مستودع المصدر.
4. ملفات Unity `.meta` يجب ألا تُستبعد إذا أُضيف مشروع Unity مستقبلاً.
5. أضف الوثائق إلى الفهرس وراجع الروابط الداخلية.
6. لا تنقل Domain أو Core إلى شجرة Unity قبل اعتماد حد معماري وخطة ترحيل.
