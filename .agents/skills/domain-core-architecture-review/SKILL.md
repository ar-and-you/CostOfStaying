---
name: domain-core-architecture-review
description: Review Domain/Core boundaries and design risks without changing architecture.
---

# Domain Core Architecture Review

1. Inspect project files, references, tests, target frameworks, and current ADR index at the specified SHA.
2. Map actual dependencies and ownership; distinguish present code from proposed architecture.
3. Evaluate cohesion, coupling, dependency direction, testability, lifecycle ownership, and migration risk.
4. Present alternatives, trade-offs, and required evidence as an ADR draft only.
5. Mark unresolved owner decisions `TBD`.

Do not migrate, merge, or rename projects; do not change gameplay behavior.
