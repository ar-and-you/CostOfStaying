---
name: verification-evidence
description: Verify scoped acceptance criteria and produce reproducible evidence tied to a commit SHA.
---

# Verification Evidence

1. Confirm target SHA, environment, allowed commands, and acceptance criteria.
2. Run only explicitly authorized commands.
3. Capture exact command, tool/runtime versions, exit status, test counts, workflow URL, and artifact/log references.
4. State precisely what the check proves and what it does not prove.
5. Return `PASS`, `PASS WITH LIMITATIONS`, or `FAIL` with unresolved gaps.

Never infer platform, integration, security, or human-playtest readiness from unrelated checks.
