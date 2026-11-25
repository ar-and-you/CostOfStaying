# سجل السياسات المرجعية

> تنبيه حوكمي: أرقام ADR المشار إليها في نسخ سابقة هي مواد أرشيفية ما لم توجد في `docs/adr/` بحالة مقبولة على `main`.

| المعرّف | السياسة | الحالة | المرجع | نطاقها |
|---|---|---|---|---|
| GOV-001 | مصدر الحقيقة وفض التعارض | ACTIVE | `PROJECT_GOVERNANCE.md` | كل المشروع |
| GOV-002 | ضبط الوثائق والفهرس | ACTIVE | `DOCUMENT_CONTROL.md` | الوثائق |
| GOV-003 | تغيير السياسات والقرارات | ACTIVE | `CHANGE_CONTROL.md` | جميع التغييرات |
| GOV-004 | Git والدمج | ACTIVE | `GIT_AND_MERGE_POLICY.md` + `ROADMAP.md` | المستودع |
| GOV-005 | حالات العمل والأدلة | ACTIVE | `STATUS_AND_EVIDENCE.md` | التقارير وCI |
| GOV-006 | الوكلاء والأتمتة | ACTIVE | `AI_AND_AUTOMATION_POLICY.md` + `AGENTS.md` | الأتمتة |
| GOV-007 | تشغيل الوكلاء وحوكمتهم | ACTIVE | `../ai/AI_AGENT_OPERATING_MODEL.md` + `../ai/AI_AGENT_GOVERNANCE_POLICY.md` | استخدام الوكلاء والمهارات ومزودي النماذج |
| SEC-001 | إدارة الأسرار | ACTIVE | `../security/SECRETS_MANAGEMENT.md` | إدارة الأسرار في المستودع والـCI |
| GAME-001 | قواعد اللعبة | PROPOSED FOR PLAYTEST | `../02_GAME_RULES_SPECIFICATION.md` | تصميم اللعب فقط |
| ARCH-001 | حد Domain/Core والتكدس | TBD | `../architecture/DOMAIN_CORE.md` و`../adr/README.md` | المعمارية |
| NET-001 | الشبكة وخادم المباراة | PROPOSED | `../architecture/NETWORK_ARCHITECTURE.md` | شبكة مستقبلية |
| DEP-001 | Backend والاستضافة | TBD | `../architecture/BACKEND_AND_HOSTING.md` | بنية تشغيل مستقبلية |

`ACTIVE` هنا يعني أن السياسة الإجرائية نافذة بعد اعتمادها/دمجها، وليس أن كل نظام أو خاصية مذكورة في الوثيقة منفذة. لا تمنح GOV-007 أي وكيل صلاحية تقنية أو موافقة على قرار المالك.
