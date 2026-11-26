---
name: documentation-governance
description: Audit documentation indexes, links, status claims, provenance, traceability, and policy consistency without changing product or architecture decisions.
---

# Documentation Governance

1. Establish the exact repository SHA and read `AGENTS.md`, `README.md`, `docs/README.md`, `CONTRIBUTING.md`, `ROADMAP.md`, and active governance policies.
2. Inventory active documentation and archives; identify duplicates, orphaned documents, stale copies, and misplaced historical material.
3. Check relative links, anchors, external links, index reachability, glossary terminology, status labels, and stale SHA references.
4. Trace requirements through design, architecture, implementation, tests, and verification evidence.
5. Compare documentation claims with live source code, tests, workflows, and policy sources at the same SHA.
6. Preserve the distinctions between `PROPOSED`, `ACCEPTED`, `IMPLEMENTED`, `VERIFIED`, and `TBD`; never treat archive decisions as active authority.
7. Report findings with exact paths and lines, evidence, severity, confidence, and the smallest safe correction.
8. Identify owner-only policy, gameplay, and architecture decisions separately; do not resolve them by inference or model consensus.
9. Update the documentation index whenever an approved active document is added, moved, or renamed.
10. Do not mark a phase `CLOSED` unless every documented gate has direct, scope-matched evidence.

Do not modify gameplay rules, balance values, architecture, code, or CI without separate authorization. Do not import ADRs wholesale or describe automated simulation as human playtesting.
