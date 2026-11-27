# المرحلة 78 — تثبيت حدود الثقة وبدء الإغلاق الأمني لـCore

**التاريخ:** 2026-11-27  
**الحالة:** PHASE 0 PARTIAL — DOCUMENTED BASELINE; OWNER PRODUCT DECISIONS OPEN  
**العمل:** تعديل وثائقي فقط؛ لم تُعدّل ملفات Source أو Tests.

## 1. مرجع العمل

- Repository: `ar-and-you/CostOfStaying`
- Default branch: `main`
- HEAD قبل العمل: `d6b97b57951df47cb59bfa4cef8657677fcc7f2e`
- Branch العمل: `security/core-trust-boundaries-phase0`
- صلاحية GitHub المتاحة: push=true، admin=false، maintain=false.
- لم يُدفع أي تغيير إلى `main` ولم يُنشأ merge commit.

## 2. مراجعة الأدلة الحية

تمت قراءة الملفات التالية على HEAD المذكور:

| الملف | Git blob SHA | نتيجة الفحص |
|---|---|---|
| `docs/security/TRUST_BOUNDARIES.md` | `0b6003f4ee5045f48050f23f90c3ddd873a0a18c` | وصف مختصر للحالة المقترحة؛ لا يميز بما يكفي بين Core والـHost ومسؤوليات Gameplay. |
| `docs/governance/OPEN_DECISION_REGISTER.md` | `f8341b55532ea37a74d62cf7dff3def51d792173` | D-R1..D-R7 مفتوحة؛ D-04 وD-019 وD-020 تسجل حدود الاحتفاظ والتفويض والتحقق. |
| `src/CostOfStaying.Core/CoreComposition.cs` | `48ce319284f961e81e2e2862ff996bd1fa7ee2fb` | `Create()` بلا MatchId؛ property عامة تكشف `IBetrayalLedger`. |
| `src/CostOfStaying.Core/Contracts/IMatchCommandAuthorizationPolicy.cs` | `05d4b44c0995d41e61fac1bce4688442b2734d93` | عقد generic لقرار policy، بلا تسجيل مركزي لأنواع الأوامر. |
| `src/CostOfStaying.Core/Services/MatchCommandAuthorizationService.cs` | `9778c733dfa5b0c4fcd3c599a489c0eba0caaef4` | policy non-null مطلوبة؛ تقييم قبل admission وبعد dequeue؛ لا default Gameplay policy. |
| `src/CostOfStaying.Core/Contracts/IBetrayalLedger.cs` | `a58c02545d832a1c3d0bcba8a9cc683e69085d73` | `TryRecord` exposed عبر interface عامة ولا تحمل membership contract. |
| `src/CostOfStaying.Core/Domain/BetrayalLedger.cs` | `9f671e8399eecf651caa8b95ff36485169004110` | يتحقق من validity/sequence/exact replay؛ لا يتحقق من عضوية الطرفين أو authority. |
| `docs/operations/ARCHIVE_RECONCILIATION_STAGE65_CORECOMPOSITION_SCOPE_ISOLATION_2026-11-27.md` | `b8a4b6f8325c2a9518374ce72ce9e8c0552a6d8e` | القرار التاريخي KEEP_CURRENT_API مؤقتاً؛ أُعيد فتحه بتوجيه مالك المشروع الحالي نحو فرض MatchId. |
| `docs/architecture/DATA_ARCHITECTURE.md` | `1dc945c3fed55a5d3b7257e1e4d885314246cc4e` | لا مخزن دائم أو سياسة retention معتمدة. |
| `docs/architecture/DOMAIN_CORE.md` | `d49cd2d9950d5f319171414b5ebff0ea0f04e9aa` | Domain/Core منفصلان؛ لا gameplay handler متكامل داخل Core. |
| `docs/02_GAME_RULES_SPECIFICATION.md` | `0d906d5bda011799b37e53bfb16e076c252ad7d5` | قواعد اللعبة PROPOSED FOR PLAYTEST، ولا تصلح كأساس لسياسات إنتاجية نهائية. |
| `docs/systems/BETRAYAL_SYSTEM.md` | `fe18f0427a4540d31a83716015016ae5ccd7a22d` | النقطة مرتبطة بنقل ناجح؛ عتبات التأثيرات غير معتمدة. |
| `docs/systems/SUFFERING_TRANSFER_SYSTEM.md` | `55cef3ed7a096f9a7163b0305c6702106576354e` | نقل Domain موجود، لكن لا transport/server adapter ولا مسار يربطه بـCore ledger. |

## 3. قرارات التنفيذ الوثائقي

1. استبدال `TRUST_BOUNDARIES.md` بوصف تفصيلي لمناطق الثقة وملكية المكونات والتحقق وحدود الضمانات.
2. تسجيل توجيه deny-by-default مع توضيح أن non-null policy مطبقة لكل service instance، بينما completeness registry غير مثبت.
3. تسجيل حظر المزود الخارجي حتى إغلاق F-001 وF-002.
4. تسجيل أن Betrayal membership/authority يجب أن يتحقق منها Trusted Gameplay Handler، لا افتراض أن authorization admission وحده يحمي ledger.
5. تسجيل توجيه فرض MatchId بعد جرد scope resources.
6. إبقاء retention مفتوحاً حتى تحديد lifecycle/workload.

## 4. نتائج لا يجوز تجاوزها

### F-001 — مفتوحة
لا توجد concrete Gameplay command DTOs أو handlers أو policies إنتاجية معتمدة في Core. بناء سياسة لكل أمر يتطلب أولاً تحديد الأوامر وقواعد المنتج. إضافة registry الآن بلا قائمة أوامر معتمدة ستخلق abstraction بلا coverage حقيقي.

### F-002 — مفتوحة
لا يثبت مسار authorization الحالي أن كل استدعاء لـ`TryRecord` صادر من Gameplay handler موثوق. كما أن `CoreComposition.BetrayalLedger` يكشف الواجهة علناً. يلزم جرد المستهلكين وبناء مسار handler موحد؛ لا يجوز اختراع handler أو event semantics من غير قواعد معتمدة.

### F-003 — مفتوحة
لا توجد قيم معتمدة لمدة المباراة أو عدد اللاعبين أو الاستئناف أو RPO/RTO. لذلك لم تُضف caps أو eviction أو snapshots.

### F-004 — مفتوحة
التقرير التاريخي للمرحلة 65 يثبت أن composition الواحد يمكن أن ينشئ موارد Match لمعرفات مختلفة وأن ledger لا يحمل MatchId. توجيه المالك الحالي يطلب فرض العزل؛ يلزم قبل التغيير جرد كل استعمالات `CoreComposition.Create()` وتصنيف factories إلى Match/Host/Session scope وتحديث جميع الاختبارات والمستهلكين.

## 5. حالة المرحلة 0

**منجز:** توثيق حدود الثقة، التمييز بين Implemented/Required/Host Responsibility/Not Implemented/Owner Decision، وتسجيل بوابة عدم التكامل الخارجي.

**غير مكتمل:** سيناريو lifecycle المنتج. لم يحدد مالك المشروع بعد:
- الحد الأدنى/الأقصى للاعبين.
- مدة المباراة والحد الأقصى التشغيلي.
- سياسة reconnect/resume.
- استمرار المباراة بعد restart ومتطلبات RPO/RTO.
- معدل أحداث Betrayal المتوقع.
- بيانات ما بعد المباراة ومدة الاحتفاظ.

لذلك المرحلة 0 **جزئية وليست مكتملة**.

## 6. التغييرات في فرع العمل

- `docs/security/TRUST_BOUNDARIES.md` — تحديث شامل.
- `docs/governance/OPEN_DECISION_REGISTER.md` — إضافة SEC-01..SEC-06 وحالة القرارات المعلقة.
- `docs/README.md` — إضافة رابط هذا التقرير.

لا يوجد تغيير في Source أو Tests أو Dependencies.

## 7. التحقق

- لم تُشغّل اختبارات .NET أو CI؛ التغيير وثائقي.
- لم تُجرَ تغييرات على `main`.
- يلزم مراجعة diff على فرع العمل قبل إنشاء PR.
