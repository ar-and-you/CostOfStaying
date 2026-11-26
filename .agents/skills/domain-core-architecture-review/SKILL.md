---
name: domain-core-architecture-review
description: Review Domain/Core responsibilities, dependency direction, public APIs, state ownership, lifecycle boundaries, and migration options without making architecture decisions.
---

# Domain Core Architecture Review

1. Inspect source files, project files, tests, workflows, target frameworks, and the current ADR index at the exact requested SHA.
2. Map each responsibility to its actual implementation: match rules, state, identity, authorization, commands, sessions, lifecycle, persistence, and transport.
3. Map dependency direction and ownership; distinguish implemented behavior from intended or proposed architecture.
4. Identify duplicate state, mutable collection escape hatches, public API compatibility risks, hidden coupling, and unclear lifecycle ownership.
5. Trace each material claim to an exact source path and relevant test; do not infer runtime integration from interfaces or unit tests.
6. Evaluate cohesion, coupling, testability, invariants, migration risk, and the no-migration alternative.
7. Present options and trade-offs as an ADR draft with status `PROPOSED`; mark owner decisions `TBD`.
8. Require explicit invariants, contract tests, migration sequencing, rollback strategy, and owner approval before implementation.

Do not migrate, merge, rename, or duplicate projects; do not change gameplay behavior or public APIs during the review.
