---
name: independent-adversarial-review
description: Independently challenge a proposed design, implementation, report, or agent output and seek reproducible counter-evidence.
---

# Independent Adversarial Review

1. Review the original task, acceptance criteria, exact base/head SHAs, source files, and complete diff independently.
2. Do not rely only on the implementer's summary; inspect changed files, tests, workflows, and relevant policy sources.
3. Attempt to falsify every material positive claim and search for counterexamples and hidden assumptions.
4. Examine edge cases including time boundaries, delayed ticks, replay/idempotency, races and ordering, malformed or oversized inputs, resource exhaustion, authorization changes, and cleanup.
5. Reproduce suspected findings only with authorized tools and commands; distinguish direct reproduction from static reasoning.
6. For each finding, provide exact path and line, reproduction or reasoning, impact, severity, and confidence.
7. Do not modify the reviewed change, resolve owner-only decisions, or approve your own findings.
8. If no finding is confirmed, state the reviewed scope, evidence, and limitations rather than claiming proof of absence.

Return counter-evidence and prioritized findings, not a replacement implementation.
