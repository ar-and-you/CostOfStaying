# خريطة الأدوات — v1.0
**الحالة: PROPOSED (قائمة اقتراحات، لا اعتماد) | آخر مراجعة: 2026-11-16 | مصدرها: قوائم أدوات قُدّمت من المالك ومراجعتها مقابل المستودع**

هذه خريطة لما قد يفيد المشروع، مصنفة حسب المجال والمرحلة. **لا يعني ورود أداة هنا اعتمادها.** الاعتماد يحتاج ADR يسمي الأداة ويذكر أسرارها وتكلفتها، والتراخيص والأسعار تُتحقق من مصدرها قبل أي اعتماد (لم يُتحقق منها هنا). ما هو مفعّل فعلاً في المستودع موثق في [GITHUB_TOOLING.md](GITHUB_TOOLING.md).

مفتاح العمود «الحالة»: **قائم** = ملف أو إعداد موجود في المستودع (غير مُشغَّل على GitHub بعد)، **مقترح** = لم يُضف، **مؤجل** = له شرط مسبق غير متحقق.

المراحل: P0 وثائق، P1 Prototype، P2 خادم، P3 اختبار لعب، P4 ما قبل الإنتاج.

## 1. إدارة المشروع والكود
| الأداة | الاستخدام | المرحلة | الحالة |
|---|---|---|---|
| Git وGitHub (PR وIssues) | تاريخ ومراجعة ومهام | P0 | قائم (قوالب Issues وPR) |
| GitHub Projects وMilestones | لوحة مراحل MVP | P0 | مقترح |
| GitHub Actions | بناء وفحص واختبار | P0 | قائم (5 workflows نشطة أو مشروطة) |
| Branch protection / Rulesets | منع الدمج دون فحوص | P0 | قائم كقالب معطل |
| CODEOWNERS | تحديد المراجعين | P0 | قائم كقالب + أداة توليد (`tools/repo_setup/set_owner.py`) |
| Conventional Commits | توحيد رسائل الـcommit | P0 | مقترح |
| GitHub Releases | حفظ نسخ prototype | P1 | مؤجل (لا شيء يُنشر) |
| Git LFS | أصول Unity الكبيرة | P1 | مؤجل (قرار حصة تخزين) |
| GitHub Discussions | نقاشات غير مرتبطة بمشكلة | P0 | مقترح |

## 2. تحليل الكود
| الأداة | الاستخدام | المرحلة | الحالة |
|---|---|---|---|
| CodeQL | ثغرات C# وPython وworkflows | P0 | قائم (`security.yml`) |
| dotnet format وEditorConfig | تنسيق C# | P0 | قائم (`repo-quality.yml`) |
| Roslyn/.NET analyzers | أخطاء وقواعد C# | P1 | مؤجل (قد تكسر `TreatWarningsAsErrors` في Core؛ تُقاس أولاً) |
| StyleCop وMeziantou Analyzer | أسلوب وقواعد إضافية | P1 | مؤجل |
| SonarQube Community Build | جودة وتعقيد وتكرار | P4 | مؤجل (خادم ورمز) |
| Semgrep | قواعد مخصصة لسلطة الخادم | P2 | مؤجل (القواعد المخصصة غير مكتوبة) |
| NetArchTest | اختبارات حدود معمارية | P1 | مقترح (الحدود الآن بـ`grep` في `core-ci.yml`) |
| PVS-Studio وNDepend | تحليل متقدم ومعماري | P4 | مقترح؛ التراخيص تُتحقق |

## 3. الأسرار والاعتماديات
| الأداة | الاستخدام | المرحلة | الحالة |
|---|---|---|---|
| gitleaks | تسريب أسرار | P0 | قائم (`security.yml`) |
| GitHub Secret scanning | كشف أسرار على GitHub | P0 | يدوي (إعدادات) |
| Dependabot | تحديث الاعتماديات | P0 | قائم |
| dependency-review | حزم جديدة بثغرات في PR | P0 | قائم (اختياري بمتغير) |
| OSV-Scanner وOWASP Dependency-Check | ثغرات الاعتماديات | P2 | مؤجل (لا lockfiles) |
| TruffleHog وSnyk وRenovate | بدائل أو مكملات | — | غير مطلوب الآن |

لا تُحفظ داخل المستودع: مفاتيح Play Console، وشهادات توقيع Android وiOS، وكلمات مرور الخادم، وملفات `.env`، ومفاتيح التحليلات أو الدفع. `.gitignore` واختبارات الإعدادات تحرس الأنماط الشائعة.

## 4. Unity
| الأداة | الاستخدام | المرحلة | الحالة |
|---|---|---|---|
| Unity 6.3 LTS وUnity Hub | المحرك المستهدف | P1 | لم يُنشأ مشروع |
| Unity Test Framework | EditMode وPlayMode | P1 | مقترح (`unity-mobile.yml` جاهز عند وجود مشروع) |
| Profiler وMemory Profiler وFrame Debugger | قياس CPU وGPU والذاكرة | P1 | مقترح؛ لا ادعاء أداء بلا قياس |
| Profile Analyzer | مقارنة قياسين | P3 | مقترح |
| Addressables | تحميل الأصول | P3 | مؤجل |
| GameCI (unity-builder، unity-test-runner) | بناء واختبار Unity في CI | P1 | قائم في `unity-mobile.yml` (لم يُشغَّل) |

## 5. الشبكة والخادم
| الأداة | الاستخدام | المرحلة | الحالة |
|---|---|---|---|
| FishNet (الإصدار يُثبَّت عند prototype، ADR-001) | النقل والاستدعاء | P1–P2 | APPROVED (اتجاه)؛ الإصدار TBD |
| Linux Dedicated Server | الخادم | P2 | APPROVED (اتجاه) |
| Docker وDocker Compose | بيئة خادم موحدة | P2 | مقترح |
| tc/netem | محاكاة تأخير وفقدان حزم | P2 | مقترح |
| Wireshark | تحليل حركة الشبكة | P2 | مقترح |
| Prometheus وGrafana وLoki وOpenTelemetry | مقاييس وسجلات | P4 | مقترح؛ انظر MONITORING_AND_LOGGING |
| Sentry وCrashlytics | تقارير الأعطال | P3–P4 | مقترح بعد أول نسخة تعمل |
| Uptime Kuma وNetdata وPortainer | مراقبة وإدارة | P4 | اختياري |
| Nginx أو Caddy | Reverse proxy وTLS عند خدمات HTTP | P4 | مؤجل (لا خدمات HTTP) |

## 6. الاختبار
| النوع | الأداة | الحالة |
|---|---|---|
| وحدات Domain وCore | NUnit | قائم (لم تُشغَّل اختبارات NUnit هنا) |
| تكافؤ المحاكي والكود | `tools/balance_simulator/test_parity.py` | قائم؛ شُغّل محلياً: `parity ok` |
| محاكاة Playtest | `tools/playtest/run_sessions.py` | قائم؛ المخرجات فرضيات لا أدلة لعب |
| إعدادات GitHub | `tools/github_config_tests` | قائم (66 اختباراً) |
| Property-based (FsCheck) | اختبار حالات عشوائية للقواعد | مقترح |
| Mutation testing (Stryker.NET) | كشف اختبارات لا تفشل أبداً | مقترح |
| اختبارات شبكة وعميل عدائي | أوامر بلا صلاحية، وقيم سالبة، وreplay، وانتحال Spirit | مقترح؛ الأهم لسلطة الخادم |
| حمل (k6، Locust، JMeter) | خدمات HTTP فقط | مؤجل (لا HTTP) |
| BenchmarkDotNet | قياس كود .NET | مقترح عند الحاجة |

## 7. البيانات والتوثيق
| الأداة | الاستخدام | الحالة |
|---|---|---|
| Markdown وADR | توثيق وقرارات | قائم |
| Mermaid وPlantUML وdraw.io | مخططات | مقترح |
| MkDocs وDocusaurus | موقع توثيق | مقترح |
| ScriptableObjects وJSON Schema وYAML | بيانات اللعبة والإعدادات | مقترح |
| SQLite وPostgreSQL وRedis | بيانات دائمة وجلسات | مؤجل: لا مزود Backend معتمد (ADR-003) |
| OpenAPI وPostman/Bruno | واجهات HTTP | مؤجل (لا واجهات HTTP) |

## 8. الأصول والصوت
| الأداة | الاستخدام | الحالة |
|---|---|---|
| Blender وKrita وGIMP وInkscape وAseprite | نماذج وصور | اختياري |
| Audacity وREAPER وLMMS | صوت وموسيقى | اختياري |
| FMOD وWwise | صوت تفاعلي | مؤجل |
| أدوات توليد النماذج أو الصور | نماذج أولية | حذر: يلزم تحقق من الترخيص وحفظ مصدر كل أصل |

أولوية الصوت والجو العام في لعبة نفسية جماعية قد تفوق التفاصيل الرسومية في أول Prototype؛ يلزم ملف يحفظ ترخيص كل صوت وصورة ونموذج.

## 9. Android وiOS والنشر
| الأداة | الاستخدام | الحالة |
|---|---|---|
| Android SDK/NDK وOpenJDK وGradle وadb | بناء وتشخيص Android | داخل صور GameCI عند البناء؛ محلياً مقترح |
| Android Studio Profiler وPerfetto وAndroid GPU Inspector | أداء Android | مقترح (P3) |
| Xcode وInstruments | بناء وأداء iOS | مقترح؛ يحتاج macOS |
| Fastlane | أتمتة النشر | مؤجل (ADR نشر لاحق) |
| Google Play Internal Testing وFirebase App Distribution | توزيع اختبار | مؤجل |
| TestFlight وApp Store Connect | توزيع iOS | مؤجل؛ يحتاج حساب Apple Developer |
| APK Analyzer | حجم الحزمة | مقترح |

## 10. الذكاء الاصطناعي كمساعد
مفيد في شرح الأخطاء واقتراح اختبارات ومراجعة ADR وتلخيص السجلات وتوليد قوالب CI. لا يقرر وحده: منطق الخادم، وصلاحيات الشبكة، ومفاتيح الأمان، والتوازن النهائي، وقبول كود لم يُراجع، واستخدام أصول بلا ترخيص واضح.

## 11. مراحل مقترحة (لا تُفعَّل كلها دفعة واحدة)
- **P0:** GitHub Issues/Projects، ADR، gitleaks، `repo-quality.yml`، `security.yml`.
- **P1:** مشروع Unity، Test Framework، Profiler، FishNet، `unity-mobile.yml`.
- **P2:** Docker للخادم، tc/netem، Wireshark، اختبارات عدائية.
- **P3:** Memory Profiler، Android Profiler، اختبارات الأجهزة، Sentry أو Crashlytics.
- **P4:** SonarQube، مراقبة الخادم، التوزيع، فحص صور Docker، اختبار النسخ الاحتياطي.
