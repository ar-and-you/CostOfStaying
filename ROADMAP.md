
# CostOfStaying — Development Roadmap

This is the authoritative implementation sequence. Complete one task at a time; obtain explicit approval before each task, and do not skip phases without an approved decision.

## Engineering baseline

| Area | Decision |
|---|---|
| Domain library | `netstandard2.1` |
| Automated tests | Domain tests: `.NET 8`; Core tests: `.NET 10` (separate SDK selection) |
| Standalone development tools | Python simulator and playtest-log generator; exploratory only |
| Domain time abstraction | `IMatchClock` in the current source; architectural acceptance is not recorded in an ADR |
| Test time control | Deterministic `ManualClock` |
| Unity integration | Separate integration layer; keep Unity APIs out of Domain |
| Permanent and working branch | `main`; all repository work is performed directly on `main` |

`docs/adr/` currently has no accepted ADR documents; do not treat proposed rules or balance values as approved decisions.

## Phase 0 — Repository Setup & Guardrails

**Objective:** Establish a clean, predictable repository and working agreement.

- [x] 0.1 Inspect the existing repository structure. (Inspected 2026-11-19; initial `main` contained `.gitignore` and `README.md`.)
- [x] 0.2 Establish the project directory structure. (Source, tests, and tools are present in the audited working tree.)
- [x] 0.3 Review `.gitignore` for .NET, Python, and Unity. Removed the global `*.meta` ignore (Unity metadata must be versioned) and added Python cache/environment exclusions; continue reviewing as toolchains expand.
- [ ] 0.4 Add `.editorconfig` and formatting conventions.
- [ ] 0.5 Establish contribution, agent, and engineering workflow documentation.
- [ ] 0.6 Review and configure repository safeguards compatible with the approved direct-to-`main` workflow.
- [x] 0.7 Verify repository structure and guardrails on GitHub. (Read-only inspection on 2026-11-21 found no GitHub Actions workflows or `main` branch protection; this does not imply CI validation.)

**Git policy:** `main` is the only permanent and working branch. All approved repository changes are committed directly to `main`. Do not create task branches or pull requests. Do not force-push, delete the permanent branch, or bypass repository protections. Configure repository safeguards so they do not contradict the approved direct-to-`main` workflow. If an existing protection rule blocks an authorized direct update, stop and report the blocker; do not attempt to bypass it.

## Phase 1 — Documentation

**Objective:** Make scope, workflows, and architectural decisions discoverable.

- [x] 1.1 Document current project purpose, development status, and local verification commands in the README.
- [ ] 1.2 Document project scope and explicit non-goals without inventing gameplay requirements.
- [ ] 1.3 Establish the ADR format and review existing ADRs.
- [ ] 1.4 Finalize ADR-001: Domain architecture and technology targets.
- [ ] 1.5 Finalize ADR-002: Time abstraction (`IMatchClock` is the current source API; final decision remains pending).
- [ ] 1.6 Review consistency across the roadmap, agent instructions, and contribution guide.

## Phase 2 — Domain Core

**Objective:** Build the platform-independent domain foundation.

- [x] 2.1 Domain project exists at `src/CostOfStaying.Domain/` and targets `netstandard2.1`.
- [ ] 2.2 Define domain boundaries from approved game requirements.
- [x] 2.3 Domain time is provided through `IMatchClock`; confirm the public name/decision in a future ADR before treating it as final.
- [x] 2.4 `Match` receives time via `IMatchClock` and has no Unity dependency; its production clock uses monotonic `Stopwatch` time, not wall-clock reads.
- [x] 2.5 Domain and Core build successfully in local test runs; inspect dependencies further if new packages are introduced.

## Phase 3 — Automated Tests

**Objective:** Verify domain behavior independently of Unity.

- [x] 3.1 Domain tests exist at `tests/CostOfStaying.Domain.Tests/` and target `net8.0`; Core tests are separate and target `net10.0`.
- [x] 3.2 NUnit is configured for the test projects.
- [x] 3.3 Domain tests reference the Domain project.
- [x] 3.4 Tests use the deterministic `ManualClock` implementation.
- [x] 3.5 Added regression coverage for finite configuration/input validation, config snapshots, inactive-match commands, timer initialization, vote completion, and end-condition precedence including a single-player time limit.
- [x] 3.6 Local verification on 2026-11-21: Domain `net8.0` Release tests 45/45 passed; Core `net10.0` tests 246/246 passed. These are not CI results.

## Phase 4 — Development Tools

**Objective:** Add only tools with a demonstrated development or validation need.

- [ ] 4.1 Identify and approve required tools.
- [ ] 4.2 Implement tools with isolated responsibilities.
- [x] 4.3 Documented local simulator commands and verified seven Python regression tests plus smoke runs; these are exploratory, not approved balance evidence.

## Phase 5 — CI/CD

**Objective:** Automate repeatable validation. No GitHub Actions workflow is currently installed.

- [ ] 5.1 Add GitHub Actions workflows using the required SDKs.
- [ ] 5.2 Restore and build the Domain and test projects.
- [ ] 5.3 Run automated tests.
- [ ] 5.4 Configure applicable repository checks without requiring pull requests or blocking the approved direct-to-`main` workflow.
- [ ] 5.5 Verify results from an actual GitHub Actions run.

## Phase 6 — Unity Integration

**Objective:** Integrate the Domain library without coupling it to Unity.

- [ ] 6.1 Verify compatibility with the selected Unity version and scripting profile.
- [ ] 6.2 Define the managed-plugin integration procedure.
- [ ] 6.3 Integrate the compiled Domain library.
- [ ] 6.4 Implement the Unity-side `IMatchClock` adapter.
- [ ] 6.5 Validate integration in the Unity Editor.
- [ ] 6.6 Document the integration and its verification.

## Task protocol

For every task: identify its scope and acceptance criteria; ask `Are you ready to start the current task: [Task Name]?`; wait for explicit approval; perform only that task; verify what can actually be verified; report changed paths, commands, results, and blockers; then wait for the user's `DONE` before moving on.

Never claim that work is committed, pushed, tested, or integrated unless it is verified. Inspect files before replacing them. Record significant architectural decisions as ADRs. Do not add speculative game rules or unrelated work.

## Status note

The initial live `main` inspected for this audit was `a0c86d3` (2026-11-21). The code import and audit changes are considered integrated only after a normal GitHub push is verified. No CI workflow was present at inspection time.
