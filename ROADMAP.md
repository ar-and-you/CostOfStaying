# CostOfStaying — Development Roadmap

This is the current implementation sequence for the live repository. Do not treat proposed gameplay values or archive-only governance records as approved decisions. Keep the live Git history intact.

## Engineering baseline

| Area | Current state |
|---|---|
| Domain library | `src/CostOfStaying.Domain`, `netstandard2.1` |
| Core library | `src/CostOfStaying.Core`, `netstandard2.1`; separate from Domain today |
| Automated tests | Domain: `.NET 8`; Core: `.NET 10` with a separate test-directory `global.json` |
| Standalone tools | Python balance simulator, parameter sweep, and simulated-playtest log generator |
| Domain time abstraction | `IMatchClock`; tests use deterministic `ManualClock` |
| Unity integration | Not implemented; keep Unity APIs out of Domain/Core |
| Permanent branch | `main` |

The live `main` branch is authoritative for current files and history. The consolidated archive is a reference for selective import only; it is not a replacement branch. Archive-only ADRs must be imported and reconciled before being treated as current repository decisions.

## Git policy

Approved repository changes are committed directly to `main`, consistent with the current live repository policy and the owner's direct-push instruction for this reconciliation. Do not create task branches or pull requests under this current policy. Never force-push, delete `main`, replace `.git`, or bypass repository protections. If GitHub rejects an authorized direct update, stop and report the access or protection blocker.

The bundle includes a later hybrid-branch proposal (ADR-028), but its governance record is not present in the live `main` tree. Do not silently treat it as the active policy. A future explicit governance update may replace this section once the complete ADR record and dependent documents are reviewed and committed.

## Phase 0 — Repository setup and guardrails

- [x] 0.1 Inspect the existing repository and compare it with the supplied archives.
- [x] 0.2 Preserve the live `main` history and import source files selectively.
- [x] 0.3 Review `.gitignore` for .NET, Python and Unity; Unity `.meta` files must remain versioned.
- [x] 0.4 Record the archive/live-tree divergence in `docs/operations/RECONCILIATION_2026-11-21.md`.
- [x] 0.5 Add contribution guidance, a documentation index, and explicit verification/CI procedures.
- [x] 0.6 Review branch safeguards without bypassing them; on 2026-11-22 the branch-protection endpoint reported `main` unprotected and the repository rulesets endpoint returned an empty list.

## Phase 1 — Documentation and decisions

- [x] 1.1 Document the current project purpose, status, and local verification commands in `README.md`.
- [x] 1.2 Correct documentation that described absent tools, workflows, or integrations as active.
- [ ] 1.3 Import and reconcile the ADR catalogue; validate dependencies and cross-references.
- [ ] 1.4 Finalize the accepted Domain/Core boundary before any code migration.
- [ ] 1.5 Reconcile the archived ADR-028 branch-policy proposal with the live direct-to-main instruction.
- [ ] 1.6 Run the document audit after the missing audit utility is reviewed and imported.

The live `docs/adr/` directory contains no accepted ADR records. Reference archives include conflicting candidates (including two differently titled ADR-005 records), so 1.3–1.5 remain open pending a source-by-source review and owner decisions; see `docs/adr/README.md`. Do not import those records in bulk or infer acceptance.

## Phase 2 — Domain and Core architecture

- [x] 2.1 Domain exists at `src/CostOfStaying.Domain/` and targets `netstandard2.1`.
- [x] 2.2 Core exists at `src/CostOfStaying.Core/` and targets `netstandard2.1`.
- [x] 2.3 Domain uses `IMatchClock`; deterministic `ManualClock` support exists for tests.
- [ ] 2.4 Review the future Domain/Core boundary against the accepted ADR set.
- [ ] 2.5 Do not migrate or merge projects until the decision and implementation plan are approved.
- [ ] 2.6 Verify Unity compatibility after a Unity project and editor version exist.

## Phase 3 — Automated tests

- [x] 3.1 Domain NUnit tests exist and target `.NET 8`.
- [x] 3.2 Core NUnit tests exist and target `.NET 10`.
- [x] 3.3 Deterministic clock test support exists.
- [x] 3.4 Regression coverage exists for match lifecycle, timing, voting, input validation, and command admission.
- [x] 3.5 Obtain successful GitHub Actions runs for both test lines and record the tested commit SHA (`1b209b79261cba617bb68615746f115970422a8f`; see `docs/operations/GITHUB_TOOLING.md`).
- [ ] 3.6 Add cross-language tests only if a real C#↔Python parity contract is specified. The current `test_parity.py` does not provide this.

Previous local reports record Domain/Core results for their respective source snapshots. They are historical local results, not current GitHub Actions evidence.

## Phase 4 — Development tools

- [x] 4.1 Python balance simulator and parameter sweep exist as exploratory tools.
- [x] 4.2 Simulated-playtest harness exists; its output is not human-playtest evidence.
- [ ] 4.3 Review and import `tools/audit.py`, GitHub configuration tests, and repository setup helpers.
- [ ] 4.4 Define and test bot-voting assumptions before interpreting outcome distributions as balance evidence.

## Phase 5 — CI/CD

- [x] 5.1 Add a Domain/Python workflow at `.github/workflows/ci.yml`.
- [x] 5.2 Add a separate Core workflow at `.github/workflows/core-ci.yml` using `.NET 10`.
- [x] 5.3 Configure the Domain `.NET 8`, Python regression, and Core `.NET 10` test commands.
- [x] 5.4 Verify successful GitHub Actions runs and fix any environment-specific failures; both workflows succeeded on `1b209b79261cba617bb68615746f115970422a8f`.
- [ ] 5.5 Complete complementary security scanning, repository-quality checks, and dependency automation; CodeQL default setup is configured and its first analysis succeeded, but this wider work remains open.

Workflow configuration was added in the 2026-11-22 follow-up. Both workflows completed successfully on SHA `1b209b79261cba617bb68615746f115970422a8f`: [Domain/Python run 44966048222](https://github.com/ar-and-you/CostOfStaying/actions/runs/44966048222) and [Core run 44966048268](https://github.com/ar-and-you/CostOfStaying/actions/runs/44966048268). The Domain job emitted non-blocking XML-documentation warnings. The CI deliberately excludes simulated playtest sessions and absent audit utilities. See `docs/operations/GITHUB_TOOLING.md` for remaining limits and live GitHub settings.

## Phase 6 — Unity integration

- [ ] 6.1 Create/select the Unity project and verify its editor and scripting profile.
- [ ] 6.2 Define the managed-plugin integration procedure.
- [ ] 6.3 Integrate the approved library boundary.
- [ ] 6.4 Implement the Unity-side `IMatchClock` adapter.
- [ ] 6.5 Validate integration in the Unity Editor.
- [ ] 6.6 Add Unity/mobile CI only after a valid Unity project and build prerequisites exist.

## Phase 7 — Playtesting and balance

- [ ] 7.1 Review simulation assumptions and bot policies.
- [ ] 7.2 Conduct human playtest sessions under the approved consent and privacy rules.
- [ ] 7.3 Record outcomes, comprehension, quit behavior, and qualitative feedback.
- [ ] 7.4 Change balance values only through the applicable gameplay decision process.

## Task protocol

For each task, define scope and acceptance criteria, perform only the authorized work, run available verification, and report changed paths, commands, results, and blockers. Never claim a test, build, security scan, integration, or push succeeded unless its result is observed. Keep secrets, signing material, caches, and generated playtest outputs out of Git.
