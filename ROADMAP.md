# CostOfStaying — Development Roadmap

This is the authoritative implementation sequence. Complete one task at a time; obtain explicit approval before each task, and do not skip phases without an approved decision.

## Engineering baseline

| Area | Decision |
|---|---|
| Domain library | `netstandard2.1` |
| Automated tests | `.NET 10` |
| Standalone development tools | `.NET 10`, where applicable |
| Domain time abstraction | `IClock` |
| Test time control | Deterministic `FakeClock` or equivalent |
| Unity integration | Separate integration layer; keep Unity APIs out of Domain |
| Permanent branch | `main`; no direct pushes |

See `docs/adr/` for the rationale and consequences of accepted architecture decisions.

## Phase 0 — Repository Setup & Guardrails

**Objective:** Establish a clean, predictable repository and working agreement.

- [x] 0.1 Inspect the existing repository structure. (Inspected 2026-11-19; initial `main` contained `.gitignore` and `README.md`.)
- [ ] 0.2 Establish the agreed directory structure.
- [ ] 0.3 Review and finalize `.gitignore` for the selected toolchain.
- [ ] 0.4 Add `.editorconfig` and formatting conventions.
- [ ] 0.5 Establish contribution, agent, and engineering workflow documentation.
- [ ] 0.6 Configure `main` branch protection using repository-administrator access.
- [ ] 0.7 Verify repository structure and guardrails on GitHub.

**Git policy:** `main` is permanent and must not receive direct pushes. Use short-lived task branches and review before integration. Do not force-push or bypass protections. Delete temporary branches only after successful integration.

## Phase 1 — Documentation

**Objective:** Make scope, workflows, and architectural decisions discoverable.

- [ ] 1.1 Complete the project README.
- [ ] 1.2 Document project scope and explicit non-goals without inventing gameplay requirements.
- [ ] 1.3 Establish the ADR format and review existing ADRs.
- [ ] 1.4 Finalize ADR-001: Domain architecture and technology targets.
- [ ] 1.5 Finalize ADR-002: Time abstraction using `IClock`.
- [ ] 1.6 Review consistency across the roadmap, agent instructions, and contribution guide.

## Phase 2 — Domain Core

**Objective:** Build the platform-independent domain foundation.

- [ ] 2.1 Create `src/Domain/` project targeting `netstandard2.1`.
- [ ] 2.2 Define domain boundaries from approved game requirements.
- [ ] 2.3 Introduce `IClock` only where domain behavior needs the current time.
- [ ] 2.4 Ensure Domain has no Unity API dependency and no direct system-clock access.
- [ ] 2.5 Build and inspect the domain dependency graph.

## Phase 3 — Automated Tests

**Objective:** Verify domain behavior independently of Unity.

- [ ] 3.1 Create `tests/Domain.Tests/` targeting `.NET 10`.
- [ ] 3.2 Select and configure the test framework.
- [ ] 3.3 Reference the Domain project.
- [ ] 3.4 Implement a deterministic `FakeClock` for tests.
- [ ] 3.5 Add tests for implemented, approved domain behavior.
- [ ] 3.6 Run tests outside the Unity Editor and record results.

## Phase 4 — Development Tools

**Objective:** Add only tools with a demonstrated development or validation need.

- [ ] 4.1 Identify and approve required tools.
- [ ] 4.2 Implement tools with isolated responsibilities.
- [ ] 4.3 Document usage and verify behavior.

## Phase 5 — CI/CD

**Objective:** Automate repeatable validation.

- [ ] 5.1 Add GitHub Actions workflows using the required SDKs.
- [ ] 5.2 Restore and build the Domain and test projects.
- [ ] 5.3 Run automated tests.
- [ ] 5.4 Require applicable checks for pull requests after workflows are stable.
- [ ] 5.5 Verify results from an actual GitHub Actions run.

## Phase 6 — Unity Integration

**Objective:** Integrate the Domain library without coupling it to Unity.

- [ ] 6.1 Verify compatibility with the selected Unity version and scripting profile.
- [ ] 6.2 Define the managed-plugin integration procedure.
- [ ] 6.3 Integrate the compiled Domain library.
- [ ] 6.4 Implement the Unity-side `IClock` adapter.
- [ ] 6.5 Validate integration in the Unity Editor.
- [ ] 6.6 Document the integration and its verification.

## Task protocol

For every task: identify its scope and acceptance criteria; ask `Are you ready to start the current task: [Task Name]?`; wait for explicit approval; perform only that task; verify what can actually be verified; report changed paths, commands, results, and blockers; then wait for the user's `DONE` before moving on.

Never claim that work is committed, pushed, tested, or merged unless it is verified. Inspect files before replacing them. Record significant architectural decisions as ADRs. Do not add speculative game rules or unrelated work.

## Status note

The initial remote repository inspection was completed on 2026-11-19. The documentation/scaffolding in this bootstrap package is prepared locally only; it is not considered uploaded or integrated until GitHub confirms it.
