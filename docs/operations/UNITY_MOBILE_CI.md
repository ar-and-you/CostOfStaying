# CI لـ Unity على Android وiOS — v1.0
**الحالة: PROPOSED | آخر مراجعة: 2026-11-16 | الملف: `.github/workflows/unity-mobile.yml` | لم يُشغَّل قط ولا يوجد مشروع Unity في المستودع | القرار: ADR-025 وADR-026**

## ما يفعله الملف
| Job | يعمل عند | ما ينتجه |
|---|---|---|
| `detect` | دائماً | يفحص وجود `ProjectSettings/ProjectVersion.txt` وسراً للترخيص، ويكتب ملخصاً |
| `unity-tests` | مشروع Unity + ترخيص | نتائج EditMode كـ artifact |
| `build-android` | وسم `mobile-v*` أو تشغيل يدوي | AAB موقّع إن وُجدت أسرار التوقيع |
| `export-ios` | وسم أو تشغيل يدوي | مشروع Xcode كـ artifact (على Linux) |
| `compile-ios` | بعد التصدير | بناء غير موقّع على iOS Simulator (macOS) |

**بلا مشروع Unity أو بلا ترخيص تُتخطى كل المراحل** وتنجح `detect` بملخص يوضح السبب، فلا يكسر الملف أول دفعة. لا بناء على Pull Request، والاختبارات فقط تعمل عليه.

## ما تغيّر عن المسودة المقترحة سابقاً
| المسودة | هنا | السبب |
|---|---|---|
| `unity-builder@v4`، `unity-test-runner@v4` | `unity-builder` v6.0.0 وَ`unity-test-runner` v4.4.0 مثبتان بـSHA | أحدث إصدارات وقت الفحص؛ التثبيت يمنع تغيير الـaction من تحتنا |
| `unityVersion: 6000.3.0f1` | `unityVersion: auto` | يقرأ الإصدار من `ProjectVersion.txt`؛ لا رقم مكتوب يدوياً قد يخالف المشروع |
| `buildPath` | `buildsPath` | `buildPath` ليس مدخلاً صالحاً في `action.yml` المثبت؛ كان سيُتجاهل ويفشل رفع الـartifact |
| تشغيل على كل push وPR دون شرط | مشروط بـ`detect` | المستودع بلا Unity؛ كان سيفشل |
| بناء iOS بـ`macos-14` مع `unity-builder` | تصدير على Linux ثم بناء Xcode غير موقّع على `macos-15` | النمط الموثق للتصدير ثم التوقيع؛ لم يُتحقق بتشغيل |
| `actions/cache` لمجلد `Library` | بلا cache | zizmor يصنّفه خطر cache poisoning في workflow ينشر artifacts؛ يُضاف لاحقاً بقرار مالك |
| `GITHUB_TOKEN` وتشغيل الاختبارات دون `checks: write` | `checks: write` على job الاختبارات فقط | يلزمه لإنشاء check run |
| `coverageEnabled` الافتراضي | `coverageEnabled: false` | تقليل مخاطر الفشل في أول تشغيل |

## الأسرار المطلوبة
تُضاف في Settings → Secrets and variables → Actions (والأفضل داخل Environment):

| Secret | الاستخدام |
|---|---|
| `UNITY_LICENSE` | محتوى ملف الترخيص (.ulf) للترخيص الشخصي |
| `UNITY_SERIAL` | الرقم التسلسلي بدلاً من الملف (ترخيص Pro) |
| `UNITY_EMAIL` وَ`UNITY_PASSWORD` | حساب Unity المرتبط بالترخيص |
| `ANDROID_KEYSTORE_BASE64` | ملف keystore بعد تحويله إلى Base64 |
| `ANDROID_KEYSTORE_PASS` وَ`ANDROID_KEYALIAS_NAME` وَ`ANDROID_KEYALIAS_PASS` | بيانات التوقيع |

اختبار الإعدادات يمنع الإشارة إلى أي سر غير هذه القائمة. لا تضع مفاتيح الإنتاج لبيئة التطوير؛ أنشئ keystore منفصلاً للاختبار.

### تحويل keystore إلى Base64
```bash
# Linux أو macOS
base64 -i release.keystore | tr -d '\n' > release.keystore.base64
```
```powershell
# Windows PowerShell
[Convert]::ToBase64String([IO.File]::ReadAllBytes("release.keystore")) | Set-Content release.keystore.base64
```
انسخ محتوى الملف الناتج إلى `ANDROID_KEYSTORE_BASE64`، ولا ترفع `.keystore` ولا `.base64` إلى Git (`.gitignore` يمنع الامتداد الأول). احتفظ بنسخة احتياطية آمنة؛ فقدان المفتاح قد يمنع تحديث تطبيق منشور بالتوقيع نفسه.

## قبل التشغيل الأول
1. أنشئ مشروع Unity في الجذر (`Assets/` وَ`Packages/` وَ`ProjectSettings/`) بإصدار 6.3 LTS الفعلي.
2. اضبط في Unity: Package Name، وMinimum/Target API (Android)، وIL2CPP وARM64، وBundle Version Code، وإعدادات iOS. القيم TBD ولم تُحدد في الوثائق.
3. أضف الأسرار أعلاه.
4. شغّل Workflow يدوياً (`workflow_dispatch`) قبل الاعتماد على الوسوم.

## حدود وغير متحقق
- **لم يُشغَّل شيء:** لا ترخيص Unity، ولا بناء Android أو iOS، ولا فحص للـartifacts.
- توفر صورة Docker لإصدار Unity المحدد على Linux (بما فيها وحدة iOS) لم يُتحقق منه.
- اسم scheme `Unity-iPhone` وبنية مجلد التصدير افتراضات؛ الخطوة تبحث عن أول `.xcodeproj`.
- `compile-ios` بناء غير موقّع على Simulator؛ **لا يرفع إلى TestFlight ولا App Store**. النشر يحتاج حساب Apple Developer وشهادات وProvisioning وApp Store Connect API Key وFastlane أو `xcodebuild`، وقراراً موثقاً في ADR لاحق.
- `macos-15` اسم تسمية runner افترضه الملف؛ إن لم يتوفر يلزم تغييره.

## سلم الحالة
- **IMPLEMENTED:** بعد دمج الملف على `main` في commit محدد يُذكر في السجل؛ لا يعني ذلك وجود مشروع Unity.
- **VERIFIED (لكل منصة منفصلة):** ترخيص يعمل، واختبارات EditMode ناجحة، وAAB مبني ومثبت ومُشغَّل على جهاز Android فعلي، ومشروع iOS مُصدَّر ومبني ومُشغَّل على Simulator أو جهاز، مع حفظ نتيجة كل خطوة وأرقام commit.
