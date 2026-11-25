# Documentation and AI Governance Push — 2026-11-25

**Repository:** `ar-and-you/CostOfStaying`  
**Branch:** `main`  
**Change commit:** `9213e4e96e3a3a680fa39d02935208958238b229`  
**Parent:** `8a35e2e8d447fd13b5fb2aa1fea188c6fbd5be48`  
**Scope:** Documentation, AI governance, portable agent skills, and Documentation Governance workflow only. No gameplay, Domain/Core source, tests, build targets, or ADR decisions were changed.

## Changes pushed

- Added `docs/ai/` operating model, governance/safety policy, role catalog, model/agent evaluation protocol, and toolchain guidance.
- Added six provider-neutral skills under `.agents/skills/`: repository baseline audit, documentation governance, Domain/Core architecture review, independent adversarial review, security/supply-chain review, and verification evidence.
- Updated `docs/README.md` and `docs/governance/POLICY_REGISTER.md` to make the new guidance discoverable and record GOV-007.
- Updated `.github/workflows/docs-governance.yml` to run on pushes to `main` as well as pull requests. The workflow uses read-only repository permissions and retains the documentation-only guard for PRs from `docs/stabilization`.

## Verification evidence on the change SHA

| Check | Run ID | Result | Scope |
|---|---:|---|---|
| Documentation Governance | `45704375790` | `completed/success` | `tools/docs_audit.sh`: Markdown relative links and reachability from `docs/README.md` |
| Domain and Python CI | `45704375896` | `completed/success` | Domain NUnit tests on .NET 8 and Python simulator regression tests |
| Core CI (.NET 10) | `45704375842` | `completed/success` | Standalone Core build and NUnit tests |

The stabilization-only path-allow-list step was skipped because this was a push to `main`, not a PR from `docs/stabilization`. The documentation audit step itself passed.

## Governance status and limitations

- The repository's current Git policy permits direct pushes to `main`; this update followed that policy and the owner's explicit request. No force-push or history rewrite was used.
- Branch protection/ruleset state was not verified in this operation: the GitHub integration returned `403 Resource not accessible by integration` for the protection endpoint. Treat current protection status as `UNKNOWN`; do not infer enabled or disabled from that error.
- The live `docs/adr/` directory still has no accepted ADRs. Phase 1 remains open for ADR catalogue reconciliation and owner decisions (ROADMAP 1.3–1.5). The document audit and AI operating-model work (1.6–1.7) are complete on this SHA.
- No Unity project, Android build pipeline, multiplayer transport/server runtime, or human playtest was added or validated.
- No external model provider was connected or invoked. The agent roles and skills are provider-neutral instructions, not claims of provider availability, licensing, or execution.
- Toolchain items beyond the existing CI and docs audit are recommendations/pilots, not installed dependencies or required checks. No new runtime package was added.

## Next governance gates

1. Reconcile conflicting archived ADR candidates individually; do not bulk-import them.
2. Obtain owner decisions for Domain/Core ownership and any future branch-protection/PR policy change.
3. Re-check branch protection, repository rulesets, Dependabot, and Code Scanning with an integration that has the required read permissions.
4. Adopt any additional Markdown, security, or quality tool only after a pinned local pilot, false-positive review, and owner approval.
