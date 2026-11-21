# Secrets Management — v2.2
**الحالة: PROPOSED | فحص أسرار منفذ في ملفات CI (لم يُشغَّل على GitHub بعد) | لا مخزن أسرار إنتاجي | لا أسرار مُهيّأة في GitHub الآن**

لا تحفظ API keys أو tokens أو signing keys أو passwords في Git أو ScriptableObjects أو إعدادات العميل أو logs. التعمية ليست حماية للأسرار.

عند وجود بناء: أسرار خارج المستودع، أقل صلاحية، فصل البيئات، وتدوير عند الاشتباه.

## الفحص الحالي
- **`security.yml` → `Secret scan (gitleaks)`:** gitleaks 8.30.1 مثبت بـSHA-256، يفحص تاريخ Git الكامل والشجرة، ويفشل عند أي تسريب (`--exit-code 1`). إعداده `.gitleaks.toml` (القواعد الافتراضية). شُغّل محلياً على المستودع: لا تسريبات.
- **`ci.yml` → grep heuristic:** أنماط شائعة (`AKIA*` وَ`ghp_*` وَ`sk-live-*`) تفشل الـPR. كان ينزّل gitleaks 8.18.0 بلا فحص بصمة وبفشل غير مانع (`|| echo`)؛ أُزيل واستُبدل بفحص `security.yml`.
- **`.gitignore`:** يمنع `.env` وَ`.env.*` وَ`*.pem` وَ`*.key` وَ`*.pfx` وَ`*.p12` وَ`*.p8` وَ`*.jks` وَ`*.keystore` وَ`*.mobileprovision` وَ`*.apk` وَ`*.aab` وَ`*.ipa`. اختبار `tools/github_config_tests` يتحقق من وجود هذه الأنماط ومن عدم وجود ملفات توقيع مُلتزَم بها.
- قوالب الموردين (`codeql.yml` وغيرها) تبقى `workflow_dispatch` فقط، وليست بوابات جودة.
- **GitHub Secret scanning وPush protection:** إعداد يدوي في المستودع (انظر `operations/GITHUB_TOOLING.md` §5)، لم يُتحقق من تفعيله.

## جرد أسرار CI المخططة
لا يوجد أي منها مُهيّأ الآن. تُضاف عند إنشاء مشروع Unity (`operations/UNITY_MOBILE_CI.md`)، والأفضل داخل Environment مخصص لكل بيئة.

| السر | يستخدمه | ما يحميه | المالك | التدوير |
|---|---|---|---|---|
| `UNITY_LICENSE` أو `UNITY_SERIAL` | `unity-mobile.yml` | ترخيص Unity | TBD | عند الاشتباه أو تغيير الحساب |
| `UNITY_EMAIL`، `UNITY_PASSWORD` | `unity-mobile.yml` | حساب Unity | TBD | عند الاشتباه |
| `ANDROID_KEYSTORE_BASE64`، `ANDROID_KEYSTORE_PASS`، `ANDROID_KEYALIAS_NAME`، `ANDROID_KEYALIAS_PASS` | `unity-mobile.yml` (`build-android`) | توقيع تطبيق Android | TBD | فقدان المفتاح قد يمنع تحديث تطبيق منشور بالتوقيع نفسه؛ احتفظ بنسخة احتياطية آمنة |
| مفاتيح Apple (شهادة Distribution، Provisioning، App Store Connect API Key) | غير مستخدمة بعد | توقيع iOS ونشره | TBD | تُحدد في ADR نشر لاحق |

قواعد:
1. لا سر داخل `run`؛ يمرّ عبر `env` أو `with` فقط (يحرسه `tools/github_config_tests`).
2. لا تُستخدم مفاتيح Production في Development أو Staging؛ أنشئ keystore منفصلاً للاختبار.
3. `GITHUB_TOKEN` للقراءة فقط افتراضياً، ويُمنح صلاحيات إضافية لكل job عند الحاجة (`checks: write` لاختبارات Unity، و`security-events: write` لـCodeQL).
4. عند الاشتباه بتسريب: دوّر السر فوراً، وانظر `INCIDENT_RESPONSE.md`.

## المراجع الموحدة
- `governance/SOURCES_AND_REFERENCES.md` EXT-GITLEAKS / SRC-ADR-026
- `REPOSITORY_STRUCTURE.md` + `.gitignore` (أسرار مستبعدة)
- `operations/GITHUB_TOOLING.md` و`operations/UNITY_MOBILE_CI.md`
