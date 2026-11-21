# CostOfStaying — Import Package Notes

Package date: 2026-11-21 (Africa/Tripoli)
Canonical source archive: CostOfStaying-FINAL-verified-2026-11-20.zip
Canonical source SHA-256: 628a351cc21989c1b6a0152b61bfa5e9d0e1b6d25b42420dcce7ab016a6b0946

## Purpose
This is a source-and-project-content overlay for the existing `ar-and-you/CostOfStaying` repository. It is not a replacement repository and must not replace Git history. Extract its contents into a working copy of the existing `main` branch, preserving files that are already present but intentionally omitted from this package.

## Preserved repository authority
Do not replace or delete the existing root `ROADMAP.md`, `.gitignore`, `.editorconfig`, or `README.md`. Keep the current `docs/architecture/REPO_LAYOUT.md` as already uploaded. These are governed by the current GitHub `main` state and were intentionally not duplicated in this ZIP.

## Intentionally excluded pending separate review/approval
- `.git/`: source archive Git metadata must never replace destination repository history.
- `.github/`: workflows, branch ruleset templates, Dependabot, CODEOWNERS and issue templates are withheld; no CI workflow or branch-protection change is authorized by this package.
- `docs/governance/` including ADRs and migration records: requires reconciliation against the latest owner decisions and current repository governance before import.
- `docs/roadmap/` and `docs/PROJECT_STATUS_AND_EXECUTION_REGISTER.md`: the existing root `ROADMAP.md` remains the sole binding phase/task sequence; work-plan documents remain proposed and are not imported here.
- `docs/audit/` and `docs/archive/`: historical branch/CI reports and superseded material are not part of the active import.
- Root `CONTRIBUTING.md` and `SECURITY.md`: contain legacy branch/PR and branch-support guidance inconsistent with the currently approved direct-to-main policy; they need revision before import.
- Root `README.md` and `.editorconfig`: the current GitHub versions are retained and are not overwritten.
- Generated playtest logs under `docs/quality/playtest-logs/` and all local caches/build outputs.

## Status and verification limits
This package is a staged source import, not a release-ready or production-ready declaration. Documentation marked PROPOSED remains proposed. The source archive's recorded Python checks passed as documented in its project index/manifest; .NET build/tests and Gitleaks were not rerun in the preparation environment because the required SDK/tool was unavailable. Unity, Android and dedicated-server/network builds were not performed.

## Manual import procedure
1. Clone or otherwise work from the existing `main` repository; do not initialize a new Git repository and do not replace `.git/`.
2. Extract this ZIP into the repository working directory, preserving the current root governance files listed above.
3. Review `git status` and the complete diff. Confirm no existing file was unintentionally overwritten and no workflow/ruleset was added.
4. Commit only after the owner reviews the exact changed paths. Continue using the approved direct-to-main workflow; do not create task branches or pull requests.
5. After upload, inspect the exact GitHub commit and run the required verification. Do not mark source or documentation VERIFIED solely because the ZIP passed integrity checks.
