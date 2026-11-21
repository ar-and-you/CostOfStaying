# أدوات GitHub في المشروع — v1.0
**الحالة: PROPOSED | آخر مراجعة: 2026-11-16 | القرار: [ADR-026](../governance/adr/ADR-026-GITHUB-TOOLING-BASELINE.md) (APPROVED) | لم يُشغَّل أي workflow على GitHub بعد**

يصف ما هو موجود فعلاً في `.github/` وكيف يُختبر، وما يجب تفعيله يدوياً في إعدادات GitHub. كل ما هنا PROPOSED حتى تنجح تشغيلات حقيقية على `main`. خريطة الأدوات الأوسع (خارج GitHub أيضاً) في [TOOLING_MAP.md](TOOLING_MAP.md)، ومسار Unity للموبايل في [UNITY_MOBILE_CI.md](UNITY_MOBILE_CI.md).

## 1. مكونات GitHub
| المكوّن | الاستخدام | الملف أو الإعداد | الحالة |
|---|---|---|---|
| Actions | بناء وفحص واختبار | `.github/workflows/*.yml` | ملفات موجودة، لم تُشغَّل |
| Dependabot | تحديث nuget وactions وpip وnpm | `.github/dependabot.yml` | ملف موجود |
| CODEOWNERS | تحديد المراجعين | `.github/CODEOWNERS.template` + `tools/repo_setup/set_owner.py` | **قالب غير فعّال حتى تشغّل الأداة بمالكك** (OD-04) |
| Rulesets | حماية `main` | `.github/rulesets/main-protection.json` | قالب معطل (`enforcement: disabled`) لمالك وحيد: صفر مراجعين وفحوص إلزامية؛ يُستورد يدوياً |
| Issue forms وPR template | توحيد البلاغات والمراجعة | `.github/ISSUE_TEMPLATE/`, `.github/PULL_REQUEST_TEMPLATE.md` | موجودة |
| Secrets وEnvironments | أسرار Unity والتوقيع | إعدادات المستودع | يدوي؛ انظر القسم 5 |
| Code scanning | عرض نتائج CodeQL | إعدادات المستودع | يدوي؛ يحتاج مستودعاً عاماً أو GHAS |
| Secret scanning وPush protection | منع تسريب الأسرار | إعدادات المستودع | يدوي |
| Dependency graph | تغذية dependency-review وDependabot | إعدادات المستودع | يدوي |
| Releases وGit LFS وProjects وDiscussions | إصدارات وأصول كبيرة وتخطيط | — | غير مفعّلة؛ تُقرر لاحقاً (ADR-026) |

## 2. محركات التحليل والاختبار
| الأداة | ماذا تفحص | أين تعمل | ما لا تثبته |
|---|---|---|---|
| CodeQL (csharp, python, actions) | ثغرات في الكود وworkflows | `security.yml` → `CodeQL (…)` | لا تثبت قواعد اللعبة |
| gitleaks | أسرار في تاريخ Git والشجرة | `security.yml` → `Secret scan (gitleaks)` | لا تكشف أسراراً بصيغ غير معروفة |
| dependency-review | حزم جديدة بثغرات في PR | `security.yml` → `Dependency review` (اختياري) | لا تفحص الحزم الموجودة مسبقاً |
| actionlint | أخطاء صياغة workflows | `repo-quality.yml` → `Workflow lint` | لا تقيّم الأمان |
| zizmor | مخاطر أمن workflows (حقن، صلاحيات، تثبيت) | `repo-quality.yml` → `Workflow lint` | وضع offline فقط |
| اختبارات الإعدادات (66 اختباراً) | سلامة الإعدادات وتوافقها مع الوثائق | `repo-quality.yml` → `GitHub config tests` | لا تشغّل أي workflow |
| markdownlint-cli2 وcodespell وaudit.py | بنية Markdown وإملاء وحوكمة الوثائق | `repo-quality.yml` → `Docs quality` | لا تتحقق من صحة المحتوى |
| dotnet format | تنسيق C# حسب `.editorconfig` | `repo-quality.yml` → `dotnet format (…)` | ليست تحليلاً دلالياً |
| NUnit (Domain وCore) | قواعد Domain وCore | `ci.yml`، `core-ci.yml` | لا تثبت اتصال الشبكة |
| Unity Test Framework | اختبارات EditMode | `unity-mobile.yml` (مشروط) | لا يوجد مشروع Unity بعد |

**CodeQL وأدوات الجودة لا تثبت صحة `Vitality` وBetrayal وSpirit.** هذه تُختبر باختبارات القواعد، ومنع الغش يحتاج اختبارات عدائية منفصلة (غير موجودة بعد).

## 3. الـ Workflows
| الملف | المشغلات | يمنع الدمج؟ (عند تفعيل الـruleset) | الحالة |
|---|---|---|---|
| `ci.yml` | push/PR | نعم | قائم؛ لم يُشغَّل هنا |
| `core-ci.yml` | push/PR (paths)، أسبوعي، يدوي | نعم | قائم؛ لم يُشغَّل هنا |
| `repo-quality.yml` | push/PR، يدوي | نعم | PROPOSED |
| `security.yml` | push/PR، أسبوعي، يدوي | CodeQL وgitleaks نعم؛ dependency-review لا | PROPOSED |
| `unity-mobile.yml` | PR (مسارات Unity)، وسوم `mobile-v*`، يدوي | لا | PROPOSED، يتخطى نفسه بلا مشروع Unity أو ترخيص |
| `codeql.yml` `contrast-scan.yml` `debricked.yml` `frogbot-scan-and-fix.yml` `mobsf.yml` `nowsecure.yml` `securitycodescan.yml` | يدوي فقط | لا | قوالب موردين غير مفعّلة |

ترتيب التنفيذ المقترح عند أول دفعة: `repo-quality.yml` ثم `security.yml` ثم `ci.yml` وَ`core-ci.yml`؛ Unity لا يعمل حتى يوجد مشروع.

## 4. الإصدارات المثبتة
### Actions (تُحدَّث عبر Dependabot)
| Action | الإصدار | الـ SHA (commit) | تُستخدم في |
|---|---|---|---|
| `actions/checkout` | v4.6.2 | `11d5960a326750d5838078e36cf38b85af677262` | ci.yml |
| `actions/checkout` | v7.0.1 | `3d3c42e5aac5ba805825da76410c181273ba90b1` | core-ci.yml, repo-quality.yml, security.yml, unity-mobile.yml |
| `actions/dependency-review-action` | v5.0.0 | `a1d282b36b6f3519aa1f3fc636f609c47dddb294` | security.yml |
| `actions/download-artifact` | v8.0.2 | `9000827ccba6bdab643e8b6fd33ac0654aef8333` | unity-mobile.yml |
| `actions/setup-dotnet` | v4.3.1 | `67a3573c9a986a3f9c594539f4ab511d57bb3ce9` | ci.yml |
| `actions/setup-dotnet` | v6.1.0 | `0f4a354dd5547a28c70b7853dbd95d5b67886e73` | core-ci.yml, repo-quality.yml |
| `actions/setup-node` | v7.1.0 | `949feb2413d6458794dcd2491c4babbbce0c15c1` | repo-quality.yml |
| `actions/setup-python` | v5.6.0 | `a26af69be951a213d495a4c3e4e4022e16d87065` | ci.yml |
| `actions/setup-python` | v7.1.0 | `6d4fd22b20465ccc0f7354476588e8df7c9ca0e0` | repo-quality.yml |
| `actions/upload-artifact` | v4.6.2 | `ea165f8d65b6e75b540449e92b4886f43607fa02` | ci.yml |
| `actions/upload-artifact` | v7.0.2 | `cf430e030ddbb5b0abf93d22962f4752f3646cd9` | core-ci.yml, unity-mobile.yml |
| `game-ci/unity-builder` | v6.0.0 | `eb1b9fba120c6e62c9fb7a7a81d6c107ce004c45` | unity-mobile.yml |
| `game-ci/unity-test-runner` | v4.4.0 | `32e57712352b500e17974b245a6dce9e11a73213` | unity-mobile.yml |
| `github/codeql-action/analyze` | v4.39.3 | `33d7b85da1a04171e211befe2c739303b9a4c066` | security.yml |
| `github/codeql-action/init` | v4.39.3 | `33d7b85da1a04171e211befe2c739303b9a4c066` | security.yml |

### ملفات ثنائية (تُنزَّل وتُفحص بـ SHA-256)
| الأداة | الإصدار | SHA-256 للأرشيف `linux x64/amd64` |
|---|---|---|
| gitleaks | 8.30.1 | `551f6fc83ea457d62a0d98237cbad105af8d557003051f41f3e7ca7b3f2470eb` |
| actionlint | 1.7.12 | `8aca8db96f1b94770f1b0d72b6dddcb1ebb8123cb3712530b08cc387b349a3d8` |

### حزم Python وNode
مثبتة في `.github/requirements/ci-tools.txt` (PyYAML 6.0.3، zizmor 1.32.0، codespell 2.4.3) وَ`package.json` + `package-lock.json` (markdownlint-cli2 0.23.2).

## 5. ما يجب تفعيله يدوياً في GitHub
1. **Settings → Actions → General:** صلاحيات `GITHUB_TOKEN` الافتراضية للقراءة فقط.
2. **Settings → Advanced Security:** تفعيل Dependency graph وDependabot alerts وSecret scanning وPush protection؛ وCode scanning إن كان المستودع عاماً أو لديك GHAS.
3. **Settings → Secrets and variables → Actions → Variables:**
   - `CODEQL_ENABLED` = `false` فقط إذا كان المستودع خاصاً بلا GHAS (القيمة الافتراضية: يعمل CodeQL).
   - `DEPENDENCY_REVIEW_ENABLED` = `true` بعد تفعيل Dependency graph.
4. **CODEOWNERS:** شغّل `python tools/repo_setup/set_owner.py @your-user` (أو `@org/team`) ثم ارفع `.github/CODEOWNERS` الناتج. لا يوجد ملف فعّال قبل ذلك.
5. **Environments:** `development` و`staging` و`production`، مع أسرار Unity والتوقيع ضمنها (انظر [UNITY_MOBILE_CI.md](UNITY_MOBILE_CI.md)).
6. **Rulesets:** بعد أول نجاح لكل الفحوص، استورد `main-protection.json` ثم غيّر `enforcement` إلى `active`. القالب مضبوط لمالك وحيد (`required_approving_review_count: 0`)؛ ارفع العدد وفعّل `require_code_owner_review` عند وجود فريق.

## 6. تشغيل الاختبارات محلياً
```bash
python -m pip install -r .github/requirements/ci-tools.txt
python -m unittest discover -s tools/github_config_tests -v
zizmor --offline .github/workflows
actionlint            # ملف ثنائي من إصدارات rhysd/actionlint
gitleaks git . && gitleaks dir .
npm ci --prefix .github/requirements && .github/requirements/node_modules/.bin/markdownlint-cli2 "**/*.md"
codespell
python tools/audit.py
```

## 7. نتائج الفحص المحلي (2026-11-17، لا تغني عن تشغيل GitHub)
| الفحص | النتيجة |
|---|---|
| actionlint 1.7.12 على كل workflows | لا ملاحظات (exit 0) |
| zizmor 1.32.0 (offline) | لا ملاحظات؛ 2 مُتجاهلتان موثقتان في `.github/zizmor.yml` (قالبا `nowsecure` و`securitycodescan`) |
| gitleaks 8.30.1 | لا تسريبات في التاريخ ولا في الشجرة |
| markdownlint-cli2 | 0 ملاحظات |
| codespell | 0 ملاحظات |
| `tools/audit.py` | 0 مشاكل و0 تحذيرات (الملفات المستوردة المطابقة لبصمتها معفاة من رأس الحالة، OD-09) |
| dotnet format `--verify-no-changes` | Domain: exit 0 (SDK 8.0.134)؛ Core: exit 0 (SDK 10.0.115) |
| اختبارات الإعدادات | 66 اختباراً ناجحاً (انظر القسم 8) |

الاستثناء: `global.json` في جذر المستودع يطلب SDK 8.0.400 والمثبت في بيئة الفحص 8.0.134، فشُغّل فحص Domain بعد تجاوزه في نسخة مؤقتة.

## 8. ماذا تضمن اختبارات الإعدادات
أُثبت أنها قادرة على الفشل: أُدخلت 34 طفرة متعمدة في نسخة مؤقتة (action بوسم بدل SHA، job بلا مهلة، سر داخل `run`، `pull_request_target`، قالب مورد بـpush، ruleset مفعّل، تنزيل بلا بصمة، فحص gitleaks لين، حالة ADR تخالف السجل، وADR يخالف نصه المتحكم، وعودة Dead كحالة قانونية، وخطأ إملائي، وعدّ مهام خاطئ، ومالك بديل في CODEOWNERS الفعّال، وغيرها) فاكتُشفت كلها. هذا لا يثبت أن الاختبارات تغطي كل حالة. تغطي: تثبيت كل action بـ SHA مع تعليق إصدار، وصلاحيات علوية ومهل زمنية لكل job، و`persist-credentials: false`، ومنع `pull_request_target` والأسرار داخل `run` والحقن بسياق الحدث وتنزيل ملف دون SHA-256 وخطوات الأمن اللينة، وعدم وجود تريغر تلقائي للقوالب، وبوابات Unity (detect وunityVersion التلقائي ومنع البناء على PR وiOS المُقسَّم)، وDependabot (المجلدات موجودة والتثبيت دقيق)، وقالب CODEOWNERS (الصياغة وتطابق كل نمط مع ملف) وأداة `set_owner.py` (ترفض المالك الوهمي وتولّد ملفاً صالحاً)، واتساق القرارات (حالات ADR في الملفات والسجل والكتالوج وجدول ADR-027، وعدم اعتبار Dead حالة قانونية في الوثائق الحاكمة، والأخطاء الإملائية المعروفة، وعدّ مهام اللوحة)، والـruleset (معطل ومطابق لأسماء jobs حقيقية)، والقوالب و`.gitignore` (أسرار وUnity) وعدم وجود مواد توقيع، وتوافق هذا الملف مع الـworkflows والـpins.

## 9. حدود
- **لا تشغيل فعلي على GitHub:** CodeQL وdependency-review وunity-mobile (بما فيه بناء macOS) لم تُجرَّب.
- الإصدارات المثبتة هي الأحدث وقت الفحص، دون فترة تهدئة.
- الاختبارات تفحص الملفات لا تشغيلها، فنجاحها لا يعني نجاح Actions.
