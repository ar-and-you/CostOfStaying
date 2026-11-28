# المرحلة 21 — مطابقة Git blob للوثائق المشتركة مع الأرشيف (2026-11-27)

## الرأس والنطاق

- HEAD قبل المرحلة: `ee694eb12349bc56e8ce2c8fd29b4a45744f1d9b`.
- شجرة main recursive غير مقتطعة: 214 tracked blobs.
- مصدر الأرشيف: الحاوية `ملفات لعبة .zip` ذات SHA-256 `e0a972b42f68f4c6dd23f6400e45f69e53c026850d2e85d6be2b2cf29f5094f6`، وتضم سبعة أرشيفات داخلية سبق جردها.
- النطاق: 37 مساراً مشتركاً بالاسم بين docs في الأرشيفات وmain. لا يعاد فحص ملفات تقارير المراحل 17–20.

## المنهج

استُخرجت بايتات الملفات من الأرشيفات الداخلية وربطت عبر `normalized_path` و`original_path` في جرد `SOURCE_ARCHIVES_FILE_INVENTORY.tsv`. حُسب Git blob SHA1 لكل نسخة أرشيفية وفق ترويسة Git `blob <byte_length>\0` ثم قورن مع Git blob SHA1 الذي أعادته شجرة main عند الرأس أعلاه. لم تُقارن SHA-256 مع SHA1 مباشرة.

## النتائج

- 37 مساراً حياً مشتركاً بالاسم.
- 247 ظهوراً أرشيفياً لهذه المسارات الـ37.
- 68 محتوى Git blob أرشيفياً فريداً موزعاً على المسارات.
- **37/37 مساراً مختلف المحتوى عن كل نسخه الأرشيفية**؛ لم يطابق blob الحي أياً من النسخ الأرشيفية للمسار نفسه.
- توزيع عدد النسخ الأرشيفية المختلفة لكل مسار: 17 مساراً بنسخة واحدة، 13 مساراً بنسختين، 6 مسارات بثلاث نسخ، ومسار واحد بسبع نسخ.
- لا يثبت الاختلاف وحده أن كل فقرة حية أحدث أو أفضل؛ لكنه يمنع الاستبدال الآلي ويجعل main النسخة الحاكمة.

## المسارات التي جرى crosswalk لها

### وثائق الجذر
- `docs/00_PROJECT_CHARTER.md` — 3 نسخ أرشيفية مختلفة.
- `docs/01_GAME_DESIGN_DOCUMENT.md` — نسختان.
- `docs/02_GAME_RULES_SPECIFICATION.md` — نسختان.
- `docs/03_PLAYER_EXPERIENCE_AND_CORE_LOOP.md` — نسخة واحدة.
- `docs/GLOSSARY.md` — 3 نسخ.
- `docs/README.md` — 7 نسخ.

### المعمارية
- `docs/architecture/BACKEND_AND_HOSTING.md` — نسخة واحدة.
- `docs/architecture/DATA_ARCHITECTURE.md` — نسخة واحدة.
- `docs/architecture/DEPLOYMENT_ARCHITECTURE.md` — نسخة واحدة.
- `docs/architecture/DOMAIN_CORE.md` — 3 نسخ.
- `docs/architecture/NETWORK_ARCHITECTURE.md` — نسختان.
- `docs/architecture/TECHNICAL_ARCHITECTURE.md` — 3 نسخ.

### التشغيل والأدوات
- `docs/operations/BACKUP_AND_RECOVERY.md` — نسخة واحدة.
- `docs/operations/GITHUB_TOOLING.md` — نسختان.
- `docs/operations/MONITORING_AND_LOGGING.md` — نسخة واحدة.
- `docs/operations/TOOLING_MAP.md` — نسختان.
- `docs/operations/UNITY_MOBILE_CI.md` — نسخة واحدة.

### الجودة والاختبارات
- `docs/quality/BALANCE_RISK_REVIEW.md` — نسخة واحدة.
- `docs/quality/DEVICE_COMPATIBILITY.md` — 3 نسخ.
- `docs/quality/PERFORMANCE_BUDGET.md` — نسخة واحدة.
- `docs/quality/PERFORMANCE_TEST_PROTOCOL.md` — نسخة واحدة.
- `docs/quality/PLAYTEST_PLAN.md` — نسختان.
- `docs/quality/SECURITY_TEST_PLAN.md` — 3 نسخ.
- `docs/quality/TEST_STRATEGY.md` — نسختان.
- `docs/quality/VERIFICATION_2026-11-20.md` — نسخة واحدة.

### الأمن والأنظمة
- `docs/security/DATA_PRIVACY.md` — نسخة واحدة.
- `docs/security/INCIDENT_RESPONSE.md` — نسخة واحدة.
- `docs/security/SECRETS_MANAGEMENT.md` — نسختان.
- `docs/security/SECURITY_ARCHITECTURE.md` — نسخة واحدة.
- `docs/security/THREAT_MODEL.md` — نسخة واحدة.
- `docs/security/TRUST_BOUNDARIES.md` — نسخة واحدة.
- `docs/systems/BETRAYAL_SYSTEM.md` — نسختان.
- `docs/systems/COLLAPSE_SYSTEM.md` — نسختان.
- `docs/systems/COMBAT_SYSTEM.md` — نسختان.
- `docs/systems/ENDGAME_SYSTEM.md` — نسختان.
- `docs/systems/SPIRIT_SYSTEM.md` — نسختان.
- `docs/systems/SUFFERING_TRANSFER_SYSTEM.md` — نسخة واحدة.

## قرارات المرحلة

- `KEEP_LIVE` لجميع المسارات الـ37؛ لا يوجد تطابق بايتّي مع نسخها الأرشيفية.
- لا استبدال ولا نقل ولا حذف ولا استيراد.
- لا يُستنتج من crosswalk وحده أن كل التفاصيل الأرشيفية غير مفيدة؛ أي مادة لا تزال ذات قيمة يجب نقلها لاحقاً بانتقاء دلالي إلى الوثيقة الحية، لا باستبدال الملف كاملاً.
- لم تُشغّل اختبارات أو CI، ولم يحدث تحقق runtime.
- تصحيح دقة: مسار `docs/operations/BUILD_AND_RELEASE.md` ليس ضمن شجرة main الحية عند HEAD هذا، ولذلك لا يدخل في تقاطع الـ37؛ ظهوره في قائمة أولية أثناء تجميع الدفعة كان إدراجاً خاطئاً ولا يغير أرقام المرحلة 20.

**الحالة:** `STAGE 21 COMPLETE — 37/37 DOCS CONTENT CROSSWALKED; SEMANTIC SALVAGE REVIEW OPEN.`


## ملحق إعادة التحقق على main الحالي — 2026-11-28

أعيد حساب المقارنة نفسها دون إعادة تحليل المحتوى الدلالي أو إعادة فحص الملفات المحسومة، بعد تحرك `main` من الرأس التاريخي `ee694eb12349bc56e8ce2c8fd29b4a45744f1d9b` إلى `e7e5b5e54f46490c44aaf30888280d55239bb670`.

- شجرة GitHub الحالية غير مقتطعة: 314 مدخلاً.
- أعيد استخراج الحاوية `ملفات لعبة .zip` (SHA-256 `e0a972b42f68f4c6dd23f6400e45f69e53c026850d2e85d6be2b2cf29f5094f6`) مع أرشيفاتها المتداخلة؛ 8 ZIPs في هذا المسار التكراري، دون أخطاء CRC.
- ظلت المسارات الـ37 نفسها موجودة في `main` الحالي.
- أعيد حساب 247 ظهوراً أرشيفياً و68 Git blob أرشيفياً فريداً لهذه المسارات.
- **37/37 Git blob حياً مختلف عن جميع النسخ الأرشيفية للمسار نفسه**؛ صفر تطابقات، وصفر مسارات مفقودة من جانب الأرشيف.
- لم يُستبدل أو يُستورد أو يُحذف أي ملف. يظل قرار `KEEP_LIVE` لجميع المسارات قائماً، وتظل أي استفادة دلالية انتقائية منفصلة عن الاستبدال البايتي.

هذه إعادة تحقق للبصمات على SHA أحدث، وليست إعادة اعتماد لمحتوى الأرشيف أو قراراً معمارياً أو دليلاً على جودة/اختبار الوثائق.
