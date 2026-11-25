# AI and Engineering Toolchain — CostOfStaying

**Status:** Mixed inventory and recommendations. A recommendation does not mean installed, licensed, connected, or approved.

## 1. Verified repository baseline

At baseline `8a35e2e8d447fd13b5fb2aa1fea188c6fbd5be48` (2026-11-25):

- `AGENTS.md` and repository governance documents exist.
- GitHub Actions contains Domain/Python CI, Core CI, and Documentation Governance.
- `tools/docs_audit.sh` checks relative Markdown links and reachability from `docs/README.md`.
- C# Domain/Core projects and NUnit test projects exist; Python simulation tools exist.
- No Unity project files (`Assets/`, `ProjectSettings/`, `Packages/manifest.json`) or Android build pipeline were found in that inspected tree.
- No provider-neutral `.agents/skills/` directory existed at the baseline.

Re-check the live tree before relying on this snapshot.

## 2. Documentation quality stack

| Tool | Purpose | Adoption recommendation | Guardrail |
|---|---|---|---|
| `tools/docs_audit.sh` | Relative links and index reachability | Keep as baseline | Does not validate all Markdown syntax, anchors, or external HTTP status |
| markdownlint-cli2 | Markdown style consistency | Pilot on docs and pin version | Agree on Arabic/English rules before enforcing |
| lychee | External and internal link checks | Pilot with allowlist and timeout | Handle rate limits, auth-only links, and flaky hosts |
| Vale | Terminology/style consistency | Optional docs-only pilot | Maintain a project vocabulary for Vitality, Betrayal, Spirit, etc. |
| cspell | Spelling checks | Optional after technical dictionary | Avoid noisy transliteration findings |

Adopt one tool at a time, pin versions, review false positives, and make the check reproducible locally before making it required.

## 3. .NET and C# quality

| Tool | Purpose | Recommendation |
|---|---|---|
| .NET SDK and `dotnet test` | Build and tests | Continue using the repository's pinned SDK strategy |
| `dotnet format --verify-no-changes` | Formatting and analyzer validation | Pilot after `.editorconfig` alignment |
| Microsoft .NET analyzers | Static analysis | Enable incrementally; baseline existing warnings |
| NUnit | Existing unit test framework | Retain unless a documented need emerges |
| FsCheck | Property-based testing | Pilot for domain invariants and time/state boundaries |
| Coverlet | Coverage measurement | Add only when coverage reporting will guide tests; avoid vanity thresholds |
| BenchmarkDotNet | Microbenchmarking | Use after identifying hot paths and establishing a baseline |
| Stryker.NET | Mutation testing | Pilot on stable Domain tests |

Any package addition requires compatibility, maintenance, license, transitive dependency, security, and removal-plan review.

## 4. Python quality

| Tool | Purpose | Recommendation |
|---|---|---|
| Ruff | Lint and format | First candidate for a small, pinned pilot |
| pytest | Test runner and fixtures | Consider if the test suite grows beyond its current structure |
| Hypothesis | Property-based testing | Useful for simulator edge cases and invariants |
| mypy | Static typing | Introduce gradually where annotations are maintained |
| coverage.py | Coverage report | Optional; use as diagnostic evidence, not a quality proxy |

A Python-only test must not be described as C#/Python parity without a direct cross-implementation comparison.

## 5. Supply-chain and workflow security

Evaluate CodeQL, Dependabot alerts/updates, secret scanning, push protection, Gitleaks, actionlint, zizmor, dependency review, and license inventory. Verify each setting and alert list from GitHub with sufficient permissions; an inaccessible API is `UNKNOWN`, not `CLEAN`. Pin GitHub Actions to reviewed immutable commit SHAs, use least-privilege workflow permissions, and keep secrets out of logs and artifacts.

## 6. Unity and runtime tools — gated future work

Do not add Unity or networking packages before the project selects an editor version and approves architecture. Once a Unity project exists, pin the editor and package manifest/lock, add EditMode/PlayMode tests, and validate Android builds against explicit SDK/NDK/JDK versions. Select Unity Netcode, FishNet, or another transport only after requirements, threat model, server authority, latency, and load tests are documented.

## 7. Dependency approval checklist

Before adding a library or service, document: problem solved; alternatives; supported target frameworks/runtimes; maintenance and release cadence; known vulnerabilities; license and commercial restrictions; transitive dependencies; size/performance impact; offline/reproducible build behavior; secret/data access; update owner; rollback/removal plan; and explicit owner approval.
