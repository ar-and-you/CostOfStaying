# تقرير دمج الوثائق والسياسات — حزمة 2026-11-23

| البيان | القيمة |
|---|---|
| تاريخ إعداد التقرير | 2026-11-24 |
| المستودع | `ar-and-you/CostOfStaying` |
| الفرع المستهدف | `main` |
| SHA الأساس قبل التغيير | `1b209b79261cba617bb68615746f115970422a8f` |
| نوع الدفعة | وثائق وسياسات Markdown فقط |

## النتيجة والنطاق

جرت مقارنة حزمة `CostOfStaying-DOCUMENTATION-CANONICAL-2026-11-23.zip` بالشجرة الحية وبالتعديلات المحلية السابقة على `main`. نُقلت الأفكار المتوافقة انتقائياً، وصُححت المسارات والادعاءات التي لم تسندها الشجرة أو نتائج الاختبار، وحُفظت السجلات التاريخية والقرارات المفتوحة. لم تُعتمد قواعد لعب أو قرارات معمارية معلقة بهذه الدفعة.

يتألف الفرق النهائي المتوقع من **58 ملف Markdown: 44 ملفاً معدلاً و14 ملفاً جديداً**، شاملاً هذا التقرير. لا يحتوي الفرق على source أو tests أو tools أو workflows أو ملفات إعداد تطبيق/بناء أو ZIP. لم تتغير إعدادات GitHub. يُحظر force push وإعادة كتابة التاريخ؛ وسيكون إدخال التحديث إلى `main` دفعاً عادياً وفق السياسة الحالية.

## فحص الحزمة واختيار المحتوى

بصمة ZIP المرفوع `778e8d2cb59347c2f6464f701cfc05f9cb70a780ade0c3d64a5d4109277396ac`. اجتاز اختبار CRC للأرشيف (`testzip` بلا خطأ)، واحتوى على 198 عضواً؛ لم تُرصد مسارات مكررة أو غير آمنة أو روابط رمزية. تطابقت 167 بصمة من `MANIFEST.json` و168 بصمة من `SHA256SUMS.txt` بما فيها بصمة manifest. استُخرج المحتوى إلى مجلد تدقيق معزول ولم يُنفذ أي ملف منه. فُحصت 59 وثيقة Markdown نشطة و61 إحالة محلية داخل الحزمة عند المراجعة؛ أما المواد التاريخية فخضعت لجرد منفصل.

تحتوي شجرة `docs/archive/` في الحزمة على 107 ملفات إجمالاً: 105 مواد تاريخية، بالإضافة إلى ملفي الفهرس والجرد. لم تُستورد هذه المواد التاريخية بالجملة. اقتصر ما أضيف تحت `docs/archive/` في المستودع على `README.md` و`SOURCE_ARCHIVE_INVENTORY.md`. لم يُستبدل `.git` أو تاريخ `main`، وحُفظت التعديلات المحلية السابقة بدلاً من استبدالها بنسخ الحزمة.

استُخدمت نتائج 14 مراجعة مستقلة مع عملية التوليف لتحديد ما يُقبل أو يوفّق أو يُستبعد. رُفض الاستيراد الآلي لملفات الجذر التي تخص حزمة التسليم، ولم تُضف `SECURITY.md` أو `CODE_OF_CONDUCT.md` أو ملفات تشغيل غير موجودة لمجرد ورودها في المرشح.

## التوفيق الموضوعي

- أضيف فهرس موحد يربط إلى الملفات الموجودة، وسياسات تشغيلية مستمرة لضبط الوثائق وحالات الأدلة والتغيير وGit والحوكمة والوكلاء. تصبح هذه السياسات نافذة عند إدراجها في `main`، لكنها لا تتجاوز قرارات المالك ولا تعتمد قواعد لعب أو بنية تقنية.
- وُضحت حالة المنتج بوصفه في مرحلة ما قبل الإنتاج. بقيت القواعد `PROPOSED FOR PLAYTEST`، وبقيت خيارات الشبكة والاستضافة والبناء واللعب البشري والأداء والتوافق مسائل مفتوحة ما لم يثبت تنفيذها أو اعتمادها.
- أزيلت سلطة ADR غير الموجودة أو الأرشيفية. فُصل نموذج `PlayerState` في Domain عن `PlayerLifecycleState` في Core، ووُثّق أن `Dead` قيمة في enum Domain لكنها ليست انتقال الموت الحالي، بينما `Eliminated` يظهر في نموذج Core لا في enum Domain. بقيت حافة `TimeLimit` عند لاعب حي واحد قراراً مفتوحاً.
- ضُبط تتبع المتطلبات على مسارات المستودع الفعلية، مع التمييز بين وجود الكود واختبارات الوحدة وبين تكامل الشبكة أو التشغيل الإنتاجي. بقي اختبار `test_parity.py` اختباراً لمحاكي Python فقط، وبقيت جلسات `run_sessions.py` محاكاة لا اختبار لعب بشري.
- صُححت ادعاءات تشغيل الأدوات والأمن لتربط كل نتيجة بـSHA وتاريخ وحدودها. أُبقيت الوقائع التاريخية في سجلاتها، وأُلحقت بها تحديثات مؤرخة بدلاً من محوها. عُدّل رابط تاريخي كان يشير إلى ملف غير موجود ليشرح غيابه ويحيل إلى جرد الأرشيف المتاح.

## إعدادات GitHub والأدلة الخارجية

إعادة القراءة للفرع والإعدادات في 2026-11-24 كانت للقراءة فقط. كان `main` عند SHA الأساس قبل التغييرات المحلية، وأظهر فحص الحماية `404 Branch not protected` وكانت قائمة rulesets فارغة. Secret scanning وPush protection مفعّلان، وDependabot security updates معطّلة. لم يُنشأ ruleset أو branch protection ولم تُفعّل أتمتة Dependabot أو PRs.

أعادت قراءتا قوائم Dependabot alerts وCode Scanning alerts `403 Resource not accessible by integration`. لذلك لا يتضمن هذا الدمج أي ادعاء بعدد التنبيهات أو محتواها أو غيابها. تفعيل تنبيهات Dependabot سُجل في فحص سابق بتاريخ 2026-11-22، لكن لم يُحسم مجدداً في قراءة 24 نوفمبر. كما لم يُعد فحص إعداد CodeQL في هذه الجولة.

## الملفات المعدلة

**الجذر:** `AGENTS.md`، `CONTRIBUTING.md`، `README.md`، `ROADMAP.md`.

**`docs/`:** `00_PROJECT_CHARTER.md`، `01_GAME_DESIGN_DOCUMENT.md`، `02_GAME_RULES_SPECIFICATION.md`، `03_PLAYER_EXPERIENCE_AND_CORE_LOOP.md`، `GLOSSARY.md`، `README.md`.

**`docs/adr/`:** `README.md`.

**`docs/architecture/`:** `BACKEND_AND_HOSTING.md`، `DATA_ARCHITECTURE.md`، `DEPLOYMENT_ARCHITECTURE.md`، `DOMAIN_CORE.md`، `NETWORK_ARCHITECTURE.md`، `REPO_LAYOUT.md`، `TECHNICAL_ARCHITECTURE.md`.

**`docs/operations/`:** `BACKUP_AND_RECOVERY.md`، `GITHUB_TOOLING.md`، `MONITORING_AND_LOGGING.md`، `RECONCILIATION_2026-11-21.md`، `TOOLING_MAP.md`، `UNITY_MOBILE_CI.md`.

**`docs/quality/`:** `BALANCE_RISK_REVIEW.md`، `DEVICE_COMPATIBILITY.md`، `PERFORMANCE_BUDGET.md`، `PERFORMANCE_TEST_PROTOCOL.md`، `PLAYTEST_PLAN.md`، `SECURITY_TEST_PLAN.md`، `TEST_STRATEGY.md`، `VERIFICATION_2026-11-20.md`.

**`docs/security/`:** `DATA_PRIVACY.md`، `INCIDENT_RESPONSE.md`، `SECRETS_MANAGEMENT.md`، `SECURITY_ARCHITECTURE.md`، `THREAT_MODEL.md`، `TRUST_BOUNDARIES.md`.

**`docs/systems/`:** `BETRAYAL_SYSTEM.md`، `COLLAPSE_SYSTEM.md`، `COMBAT_SYSTEM.md`، `ENDGAME_SYSTEM.md`، `SPIRIT_SYSTEM.md`، `SUFFERING_TRANSFER_SYSTEM.md`.

## الملفات الجديدة

`docs/adr/ADR_TEMPLATE.md`؛ `docs/archive/README.md`؛ `docs/archive/SOURCE_ARCHIVE_INVENTORY.md`؛ `docs/governance/AI_AND_AUTOMATION_POLICY.md`؛ `docs/governance/CHANGE_CONTROL.md`؛ `docs/governance/DOCUMENT_CONTROL.md`؛ `docs/governance/GIT_AND_MERGE_POLICY.md`؛ `docs/governance/POLICY_REGISTER.md`؛ `docs/governance/PROJECT_GOVERNANCE.md`؛ `docs/governance/PROJECT_STATUS.md`؛ `docs/governance/STATUS_AND_EVIDENCE.md`؛ `docs/quality/VERIFICATION_SCOPE_NOTE_2026-11-23.md`؛ `docs/traceability/REQUIREMENTS_TRACEABILITY.md`؛ وهذا التقرير `docs/operations/MAIN_DOCUMENTATION_MERGE_2026-11-23.md`.

## التحقق وحدود الإثبات

تشغيلات Domain/Python رقم `44966048222` وCore رقم `44966048268` وCodeQL رقم `45004680726` نجحت على SHA الأساس `1b209b79261cba617bb68615746f115970422a8f` بحسب سجلاتها الموثقة. لا تخص هذه النتائج التغييرات الوثائقية الجديدة. لم تُشغّل اختبارات المصدر أو .NET أو Python لهذه الدفعة لأنها لا تعدل الشفرة أو الاختبارات؛ وتبقى متابعة Actions على SHA الجديد مطلوبة بعد الدفع.

نجاح فحص ZIP لا يثبت صحة كل ادعاء في محتواه. كما أن فشل صلاحية API لقراءة التنبيهات لا يعني خلوها. لم يُختبر Unity أو Android/iOS أو خادم/شبكة أو أداء أو أجهزة أو جلسات لعب بشرية في هذه الدفعة. لا تتضمن نتائج هذه المراجعة اعتماداً نهائياً للعبة أو شهادة أمن شاملة.

اكتملت فحوص ما قبل الالتزام على الشجرة المحلية: نجح `git diff --check`، وفُحصت 95 إحالة Markdown محلية عبر 58 ملفاً متغيراً دون روابط مكسورة، وكانت كل المسارات المتغيرة ملفات Markdown. لا توجد تغييرات في `src/` أو `tests/` أو `tools/` أو `.github/workflows/`. عند آخر مقارنة قبل الالتزام، كان `origin/main` ما زال عند SHA الأساس. لم تُشغّل اختبارات المنتج على وثائق فقط. بعد الدفع العادي، يُقرأ SHA البعيد وتُفحص ملفات الالتزام وتشغيلات Actions عليه؛ هذه الفحوص اللاحقة غير مدعاة كمنجزة هنا.
