# المعمارية الأمنية — ضوابط مستهدفة

**الحالة: PROPOSED.** لا يوجد client/server deployment متصل أو Backend إنتاجي في `main`.

## مبادئ التصميم

- العميل غير موثوق ولا يقرر حالة المباراة أو النتائج.
- الخادم — عندما ينفذ — يتحقق من الهوية والجلسة والملكية والحالة والمدخلات.
- كل أمر حساس يجب أن يكون محدد الهوية والنطاق والتسلسل وقابلاً للرفض دون آثار جانبية جزئية.
- استخدم أقل صلاحية في GitHub Actions والتكاملات.
- لا تحفظ الأسرار أو بيانات شخصية غير لازمة في repository/logs.
- افصل أمن التطبيق عن أمن المستودع وسلسلة التوريد.

## ما هو موجود

Domain وCore يحتويان اختبارات لبعض ثوابت المدخلات والأوامر والهوية والجلسات. خطا CI الحاليان للـDomain/Python والـCore يعملان بصلاحية contents: read.

Core يوفّر session/sequence/queue/roster/authorization/payload-validation primitives مع fail-closed behavior واختبارات NUnit؛ لا يوجد credential provider أو concrete gameplay authorization policy أو transport adapter. queue والregistries محدودة بسعات يحددها host، لكن BetrayalLedger غير محدود ولا يفرض roster membership. Domain لا يزال يملك مجموعات public mutable وevent/request stores بلا retention caps.

## ما لم يثبت

لا توجد خدمة شبكة أو خادم أو مصادقة إنتاجية أو penetration test أو workflow gitleaks. CodeQL Setup الديناميكي ظهر في تشغيل GitHub محدد؛ لا يوجد codeql.yml متتبع على main وقت الفحص. لا يثبت ذلك تدقيقاً أمنياً كاملاً.

## معايير المراجعة

Threat model، مراجعة اعتماديات، secrets scan للشجرة والتاريخ، authorization/replay tests، privacy review، وإجراءات incident/rotation/recovery قبل الإطلاق.
