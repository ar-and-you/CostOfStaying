---
name: verification-evidence
description: Verify scoped acceptance criteria and produce reproducible evidence tied to an exact commit SHA.
---

# Verification Evidence

1. Confirm the target SHA, environment, acceptance criteria, and explicitly allowed commands before execution.
2. Map each acceptance criterion to deterministic tests, CI jobs, or measurable evidence.
3. Obtain task-scoped authorization for the exact commands and scope; do not infer permission to execute from permission to inspect.
4. Run the narrowest relevant checks first, then broader build, test, and security checks when authorized.
5. Capture the exact command, tool/runtime versions, exit status, duration, test counts, workflow URL, and artifact/log references.
6. Tie every result to the exact SHA and environment; verify that workflow runs actually refer to that SHA.
7. Return `PASS`, `PASS WITH LIMITATIONS`, `FAIL`, or `NOT RUN` for each criterion, stating what the check proves and what it does not.
8. Record skipped, inaccessible, flaky, or inconclusive checks explicitly; never report them as passed.
9. Separate unit tests from integration, contract, network, device, performance, security-alert, and human-playtest evidence.
10. List unresolved gaps and residual risks; do not infer platform or production readiness from unrelated checks.

A successful check is evidence only for its defined scope, not proof that the whole project is defect-free.
