# AI and Engineering Toolchain — CostOfStaying

**Status:** Mixed inventory and recommendations. A recommendation does not mean installed, licensed, connected, or approved.

## 1. Verified repository baseline

At baseline `8a35e2e8d447fd13b5fb2aa1fea188c6fbd5be48` (2026-11-25):

- `AGENTS.md` and repository governance documents exist.
- GitHub Actions contains Domain/Python CI, Core CI, and Documentation Governance.
- `tools/docs_audit.sh` checks relative Markdown links and reachability from `docs/README.md`.
- C# Domain/Core projects and NUnit test projects exist; Python simulation tools exist.
- No Unity project files (`Assets/`, `ProjectSettings/`, `Packages/manifest.json`) or Android build pipeline were found in that inspected tree.
- No provider-neutral `.agents/skills/` directory existed at that baseline.

Re-check the live tree before relying on this snapshot. The current repository now contains provider-neutral skills under `.agents/skills/`; the baseline statement above is historical.

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

## 8. Portable Agent Skills and external agent hosts — reviewed 2026-11-26

### Canonical project format

Keep the repository's provider-neutral skill source in `.agents/skills/<skill-name>/SKILL.md`. This is currently the best shared source for Codex and is also recognized as a workspace skill path by Gemini CLI and GitHub Copilot. Do not maintain duplicate copies unless a host requires a different path and the copies can be checked for drift.

| Host | Project skill location / integration | Adoption posture |
|---|---|---|
| OpenAI Codex | `.agents/skills/`; `AGENTS.md` for durable repository rules | Primary canonical path |
| Gemini CLI | `.agents/skills/` alias or `.gemini/skills/`; `GEMINI.md` is persistent context, not a substitute for task skills | Supported by upstream Gemini CLI documentation; require user consent when activating skills |
| GitHub Copilot | `.agents/skills/`, `.github/skills/`, or supported compatibility paths; custom agents under `.github/agents/` | Suitable for independent read-only review and scoped custom agents |
| Claude Code | `.claude/skills/`; project subagents under `.claude/agents/`; project instructions via `CLAUDE.md` | Optional compatibility mirror only when Claude Code is selected |

The current `.agents/skills/` set includes baseline audit, archive reconciliation, documentation governance, Domain/Core architecture review, adversarial review, security/supply-chain review, and verification evidence. Skill files are instructions; they do not grant credentials, tools, write permissions, or a separate agent process.

### Multi-agent execution pattern

1. ORCHESTRATOR establishes the exact SHA, policy constraints, and evidence ledger.
2. RESEARCHER, DOCUMENTATION_AUDITOR, ARCHITECTURE_REVIEWER, and SECURITY_REVIEWER conduct independent read-only investigations.
3. IMPLEMENTER handles only the approved allow-listed change.
4. ADVERSARIAL_REVIEWER independently attempts to falsify the change and its claims.
5. VERIFICATION records tests and workflow evidence tied to the resulting SHA.
6. OWNER alone accepts gameplay, architecture, branch-policy, external data-sharing, spending, and release decisions.

Parallelize only independent read-only work. Serialize edits to shared files. Never let an implementer self-approve. Do not send the same writable checkout to multiple agents concurrently.

### ChatGPT skills, plugins, and MCP

OpenAI's current Skills documentation describes skills as reusable workflows and plugins as a distribution mechanism that can package skills and connectors for ChatGPT and Codex surfaces. Use a plugin only when it supplies a concrete capability not already available through the built-in GitHub integration or the current toolset. Before connecting it, inspect publisher, scopes, read/write actions, data flow, retention, pricing, and uninstall path.

MCP is appropriate when an agent needs a specific external system (for example, issue tracking, CI, telemetry, or design records). Grant read-only access first; request write scopes only for a named task. Never connect a broad tool server with unrestricted tools merely to increase the number of integrations.

### External host security gates

- Review every third-party skill, script, hook, plugin, MCP server, and agent profile before installation; they may read files or execute commands.
- Pin external skill/plugin versions or commits where supported and retain source provenance.
- Use a sandbox or disposable checkout for first execution.
- Keep secrets outside Git and never paste credentials into prompts or generated files.
- External providers may receive source code and prompts; do not share repository content until the owner approves the provider and data handling.
- No external Claude, Gemini, or Copilot account/connection is asserted by this document. Availability and licensing must be checked in the actual user environment.

### Official host references

- [OpenAI — Build skills](https://learn.chatgpt.com/docs/build-skills)
- [OpenAI — Customization and subagents](https://learn.chatgpt.com/docs/customization/overview)
- [VS Code — Agent Skills and supported locations](https://code.visualstudio.com/docs/agent-customization/agent-skills)
- [Gemini CLI — Agent Skills](https://github.com/google-gemini/gemini-cli/blob/main/docs/cli/skills.md)
- [GitHub Copilot — Adding agent skills](https://docs.github.com/en/copilot/how-tos/copilot-cli/customize-copilot/add-skills)
- [GitHub Copilot — Custom agents](https://docs.github.com/en/copilot/how-tos/copilot-cli/use-copilot-cli/invoke-custom-agents)
- [Claude Code — Settings and project configuration](https://code.claude.com/docs/en/settings)
