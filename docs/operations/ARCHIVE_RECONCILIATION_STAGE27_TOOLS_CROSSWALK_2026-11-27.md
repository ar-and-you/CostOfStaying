# المرحلة 27 — مطابقة أدوات المشروع وفرز الأدوات الأرشيفية (2026-11-27)

## الرأس والنطاق

- HEAD قبل المرحلة: 8daae2002c040acbc7910b12499f0313ef510841.
- جرد 14 مساراً فريداً تحت tools/ في الأرشيفات السبعة.
- الشجرة الحية تتضمن 7 مسارات مشتركة بالاسم من هذه المجموعة؛ كما تتضمن tools/docs_audit.sh وtools/playtest/test_cli.py اللذين لم يظهرا في مسارات الأدوات الأرشيفية.

## Crosswalk المحتوى للمسارات المشتركة

تمت مقارنة Git blob SHA1 للنسخ الأرشيفية مع SHA الحي:

| المسار | النسخ الأرشيفية المختلفة | النتيجة |
|---|---:|---|
| tools/balance_simulator/README.md | 1 | PATH_EXISTS_CONTENT_DIFF |
| tools/balance_simulator/presets.json | 1 | EXACT_GIT_BLOB_MATCH |
| tools/balance_simulator/simulate.py | 1 | PATH_EXISTS_CONTENT_DIFF |
| tools/balance_simulator/sweep.py | 1 | PATH_EXISTS_CONTENT_DIFF |
| tools/balance_simulator/test_parity.py | 1 | PATH_EXISTS_CONTENT_DIFF |
| tools/playtest/README.md | 1 | PATH_EXISTS_CONTENT_DIFF |
| tools/playtest/run_sessions.py | 1 | PATH_EXISTS_CONTENT_DIFF |

- نتيجة هذه الدفعة: تطابق واحد، وستة اختلافات محتوى.
- ملفات الأدوات الحية تبقى المرجع؛ لا استبدال من الأرشيف لمجرد أن الاسم متطابق.
- المسار presets.json هو تطابق بايتّي مثبت، فلا حاجة لإعادة استيراده.

## أدوات أرشيفية فقط — 7 مسارات

1. tools/agents/dispatch.py: يعتمد على EXECUTION_BOARD وAGENT_CHARTER وWORK_PLAN_V2_DETAILED وDOCUMENT_GOVERNANCE الأرشيفية، وعلى أسماء أدوار/بوابات غير مثبتة في نموذج التشغيل الحي. لا يعمل كإضافة مستقلة إلى main.
2. tools/audit.py: يعتمد على docs/governance/DOCUMENT_GOVERNANCE.md ومسارات حوكمة قديمة، ويفحص فهرساً/كتالوجاً لم يعد موجوداً. لا ينقل دون إعادة تصميم على الهيكل الحي؛ لدى main أداة docs_audit.sh.
3. tools/repo_setup/README.md وset_owner.py: يتطلبان .github/CODEOWNERS.template ويولدان CODEOWNERS؛ القالب غير موجود في main. لا تشغّل الأداة ولا تولد ملف ملكية من دون قرار وصلاحيات/مالك حقيقيين.
4. tools/github_config_tests/README.md وtest_github_config.py: يتوقعان Dependabot وCODEOWNERS وruleset وworkflows مثل security.yml وrepo-quality.yml وunity-mobile.yml، بينما الشجرة الحية تضم ثلاثة workflows فقط (ci.yml وcore-ci.yml وdocs-governance.yml). الاختبار سيقيس بنية غير موجودة ويعطي فشلاً/نتيجة مضللة إن نُقل كما هو.
5. tools/balance_simulator/__pycache__/simulate.cpython-312.pyc: ملف bytecode مولد مرتبط بإصدار Python؛ لا يُضم إلى المصدر ولا يُستورد.

## مسارات الأدوات الحية فقط

- tools/docs_audit.sh: أداة تدقيق الوثائق الحالية.
- tools/playtest/test_cli.py: اختبارات CLI الحالية لمحاكي الجلسات.

لا يعني غياب هذه المسارات من الأرشيف أنها لا تملك مصادر سابقة؛ لكن مساراتها/أدوارها الحالية ليست موجودة في الجرد الأرشيفي.

## القرار

- KEEP_LIVE لأدوات main؛ لا استبدال.
- ARCHIVE_ONLY / DO_NOT_IMPORT للأدوات السبعة القديمة حتى يعاد تصميمها وتثبت تبعياتها واختباراتها.
- استثناء pycache: DO_NOT_IMPORT مطلقاً.
- لا حذف أو نقل أو استيراد أو تشغيل للأدوات الأرشيفية.
- لا تشغيل اختبارات أو CI في هذه المرحلة.

**الحالة:** STAGE 27 COMPLETE — TOOLS CROSSWALKED; LEGACY TOOLING BLOCKED BY MISSING/STALE DEPENDENCIES.
