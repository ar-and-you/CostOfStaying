---
name: repo-baseline-audit
description: Establish an evidence-based repository baseline at an exact commit before changes or archive reconciliation.
---

# Repo Baseline Audit

1. Confirm repository identity, branch/ref, exact HEAD SHA, working-tree status when available, and the scope of granted permissions.
2. Read `AGENTS.md`, `README.md`, `docs/README.md`, `CONTRIBUTING.md`, `ROADMAP.md`, and relevant governance policies before inspection.
3. Build a tracked-file inventory from the live ref; distinguish tracked source, generated artifacts, ignored files, archives, and absent paths.
4. Compare archives by provenance, manifest, hashes, baseline SHA, and path-level differences. Never extract an archive as an overlay over an authoritative working tree.
5. Inspect project manifests, SDK pins, target frameworks, package sources, lockfiles, workflows, tests, and repository settings relevant to the task.
6. Record each statement as `FACT`, `INFERENCE`, `PROPOSAL`, or `TBD`, with exact paths, SHAs, and evidence.
7. Separate current facts from historical claims; do not infer Unity, Android, server, network, or production readiness from documentation alone.
8. Make no edits, commits, pushes, test runs, or settings changes unless the exact action and scope are separately authorized.
9. Return the baseline, inventory, discrepancies, risks, evidence gaps, and ordered next tasks.

Do not treat an archive, stale report, or prior summary as a substitute for inspecting the specified live ref.
