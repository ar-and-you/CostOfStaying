# Repository Layout and Structural Boundaries

**Status:** DRAFT — Proposed  
**Scope:** Repository structure and file-placement rules  
**Project:** CostOfStaying

## 1. Purpose

This document defines the intended repository layout of CostOfStaying and establishes the boundaries governing file placement, configuration discovery, documentation organization, and generated or sensitive content.

Its purpose is to preserve a maintainable structure without breaking .NET tooling or changing project governance implicitly.

This document does not authorize changes to branch strategy, roadmap authority, architecture decisions, or application behavior.

## 2. Repository Structure

The repository is organized around the following responsibilities:

- `.github/`: GitHub workflows, issue templates, pull-request templates, and repository automation.
- `src/`: Production source code.
- `tests/`: Automated tests.
- `tools/`: Standalone development, audit, validation, and maintenance tools.
- `docs/`: Architecture, governance, quality, roadmap, security, systems, and operational documentation.

The repository root retains files required for project discovery, build orchestration, repository-wide tooling, and contributor orientation.

**Inclusion in the root is justified by toolchain requirement, not by convenience.**

## 3. Root-Level File Policy

The repository root is not restricted to README, LICENSE, and `.gitignore`.

Files may remain in the root when their location is required or expected by the relevant toolchain.

The following table provides examples, not an exhaustive or closed list:

| File | Responsibility |
|---|---|
| `README.md` | Project overview and entry point |
| `LICENSE` | Project licensing |
| `.gitignore` | Git ignore rules |
| `.gitattributes` | Git attribute configuration |
| `.editorconfig` | Editor and formatting conventions |
| `global.json` | .NET SDK selection |
| `CostOfStaying.sln` | Solution entry point |
| `Directory.Packages.props` | Central package version management |
| `Directory.Build.props` | Shared MSBuild properties, if present |
| `Directory.Build.targets` | Shared MSBuild targets, if present |
| `CONTRIBUTING.md` | Contribution guidance |
| `SECURITY.md` | Security reporting guidance |
| `CODE_OF_CONDUCT.md` | Community conduct |
| `.gitleaks.toml` | Gitleaks configuration |
| `.markdownlint-cli2.jsonc` | Markdown lint configuration |

Any future root-level addition must satisfy the requirements in Section 4.

A root-level file must not be moved solely to reduce visual clutter.

Before changing its location, verify:

1. Whether the tool automatically discovers the file.
2. Whether the tool supports a custom configuration path.
3. Whether all local and CI commands use the correct path.
4. Whether the change affects nested projects or build behavior.
5. Whether automated tests validate the new configuration.

## 4. .NET Configuration Boundaries

.NET and MSBuild configuration files must remain discoverable by the .NET SDK and MSBuild.

In particular:

- `global.json` must remain discoverable from the intended working directories.
- `Directory.Build.props` and `Directory.Build.targets` must preserve their intended directory-scoped discovery behavior.
- `Directory.Packages.props` must remain discoverable by the projects that rely on central package management.
- The solution file must remain accessible to documented build and test commands.

Moving these files requires explicit validation of the resulting SDK, restore, build, and test behavior.

No configuration relocation is considered a purely cosmetic change.

## 5. Documentation Boundaries

Documentation belongs in `docs/` according to its purpose.

Suggested categories:

- `docs/architecture/`: Architecture descriptions and structural documentation.
- `docs/governance/adr/`: Architecture Decision Records.
- `docs/quality/`: Quality standards and verification records.
- `docs/roadmap/`: Supporting roadmap documentation.
- `docs/security/`: Security policies and assessments.
- `docs/operations/`: Operational procedures and maintenance guidance.
- `docs/systems/`: System-level specifications.

The authoritative roadmap remains governed by the currently approved project decision.

**As of this document's drafting, `ROADMAP.md` remains the authoritative reference and must not be deleted or replaced without an explicit governance decision.**

If this status changes, this document must be updated in the same commit that records the new governance decision.

ADR status must reflect its actual approval state. A proposed decision must not be presented as an accepted or implemented policy.

## 6. Generated, Temporary, and Sensitive Content

The following content must not be committed as ordinary repository source:

- ZIP delivery archives.
- Git bundle backup files.
- Local build outputs such as `bin/` and `obj/`.
- Temporary extraction directories.
- Tool caches and generated temporary files.
- Local environment files containing secrets.
- Private keys, access tokens, credentials, and other sensitive material.

Required exceptions must be explicitly documented and justified.

A `.gitignore` rule does not remove files already tracked by Git. Repository history and tracked files must be checked separately when investigating accidental inclusion.

## 7. Repository Structure Changes

Any structural change must include:

1. A clearly defined scope.
2. A list of affected paths.
3. Tooling and build-discovery impact analysis.
4. Required test and CI validation.
5. Documentation updates where applicable.
6. A review of whether the change affects existing governance decisions, and if so, an explicit reference to the decision that authorizes it.

The reference must appear in the commit message or the associated pull request description, not only in review comments.

Changes must not silently modify application behavior, domain boundaries, branch strategy, roadmap authority, or accepted ADRs.

## 8. Security Verification

Security checks must be reported independently.

- **Gitleaks:** Detects exposed secrets and credentials in repository content and, when configured, Git history.
- **CodeQL:** Performs code analysis to identify security vulnerabilities and related coding issues.

A successful Gitleaks scan does not establish that CodeQL analysis has passed, and vice versa.

Reports must distinguish between:
- Passed
- Failed
- Not run
- Not available
- Requires investigation

## 9. Governance Separation

Repository organization and project governance are separate concerns.

This document does not:

- Approve ADR-028.
- Approve the HYBRID branch strategy.
- Change the current direct-to-main working policy.
- Authorize deletion of `ROADMAP.md`.
- Activate branch protection.
- Approve `WORK_PLAN_V2_DETAILED.md`.
- Change the approved project phase order as defined in the current authoritative roadmap.

Any such change requires its own explicit decision and traceable record.

## 10. Adoption and Maintenance

**Current status: DRAFT — Not yet binding.**

This document becomes an authoritative structural reference only after explicit project approval and a traceable repository commit.

Future structural changes must be evaluated against these boundaries and validated using the relevant project tooling.

The goal is not to minimize the number of root files. The goal is to preserve tool compatibility, clear responsibilities, traceable decisions, and a maintainable repository.
