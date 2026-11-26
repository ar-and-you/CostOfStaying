---
name: security-supply-chain-review
description: Review threat boundaries, secrets, dependencies, workflow permissions, external tool data flows, and verification gaps.
---

# Security Supply Chain Review

1. Establish the exact SHA and identify assets, actors, trust boundaries, entry points, and privileged actions.
2. Inspect authentication and authorization, replay/idempotency, input validation, rate limits, resource bounds, logging, privacy, and session lifecycle.
3. Inspect GitHub Actions permissions, pinned and third-party actions, workflow triggers, secrets, artifacts, fork/PR behavior, and `pull_request_target` usage.
4. Inspect NuGet and Python dependencies, lockfiles, package sources, transitive dependencies, maintenance, licenses, and known vulnerabilities using available authoritative tools.
5. Distinguish scanners that are configured from scans that actually completed; distinguish successful workflow runs from readable alert inventories.
6. Model external provider, plugin, MCP, and agent data flows and permissions. Mark inaccessible settings, alerts, and external facts as `UNKNOWN`, not `CLEAN`.
7. Report severity, likelihood, impact, exploit preconditions, exact evidence, mitigation, verification method, and residual risk.
8. Do not print, retrieve, or transmit secret values. Do not claim a clean history or completed scan when the corresponding evidence is unavailable.
9. Do not modify workflows, dependencies, permissions, integrations, or secrets without explicit approval.

Prefer least privilege, read-only access first, immutable action references, and reproducible verification.
