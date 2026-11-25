# AI Agent Operating Model — CostOfStaying

**Status:** ACTIVE operating model, subordinate to owner decisions and repository governance.  
**Review cadence:** At each material change to tools, permissions, repository architecture, or data-sharing arrangements.

## 1. Core principles

1. The project owner is the final decision-maker. AI output is advisory until accepted through the applicable process.
2. Separate model, agent, skill, tool, credential, and permission. Mentioning a provider does not establish availability or authorization.
3. Assign one accountable task owner and at least one independent reviewer for material changes.
4. Resolve disagreements with source evidence, reproducible tests, and owner decisions—not model majority voting.
5. Every material claim must identify its repository path and commit SHA, test/workflow reference, or dated external source.
6. Distinguish `PROPOSED`, `ACCEPTED`, `IMPLEMENTED`, and `VERIFIED`. One state never implies another.
7. Parallelize independent read-only investigations; serialize edits to shared files and integrate through one accountable coordinator.
8. Never expand scope or permission without explicit authorization.

## 2. Roles and accountability

| Role | Responsibility | Default permission | Required output |
|---|---|---:|---|
| OWNER | Sets scope, constraints, and acceptance decisions | Human authority | Approval, rejection, or `TBD` |
| ORCHESTRATOR | Establishes baseline, decomposes work, coordinates handoffs | P1 | Task plan and evidence ledger |
| RESEARCHER | Checks official, current sources | P1 | Source-backed findings with dates |
| DOCUMENTATION_AUDITOR | Checks indexes, links, status claims, consistency, traceability | P1/P2 | Findings and proposed patches |
| ARCHITECTURE_REVIEWER | Compares designs and boundaries | P1/P2 | Alternatives and ADR draft only |
| ADVERSARIAL_REVIEWER | Attempts to falsify claims and find edge cases | P1 | Counter-evidence and severity-ranked findings |
| IMPLEMENTER | Applies a narrowly scoped approved patch | P0 by default; P3/P4 only when granted | Minimal diff and change log |
| QA_REVIEWER | Independently checks acceptance criteria and runs approved tests | P1; P5 only when granted | Pass/fail evidence and limitations |
| SECURITY_REVIEWER | Reviews threats, secrets, permissions, dependencies, workflows | P1/P2 | Threat findings and mitigations |

## 3. Permission levels

| Level | Meaning | Default |
|---|---|---|
| P0 | Read-only repository inspection | Allowed within authorized sources |
| P1 | Analysis and reporting | Allowed |
| P2 | Draft content outside the authoritative repository | Allowed |
| P3 | Write approved documentation paths | Explicit task-scoped approval required |
| P4 | Write approved source/test paths | Separate explicit approval required |
| P5 | Execute named verification commands | Explicit command/scope approval required |
| P6 | Remote GitHub writes, pushes, merges, settings, or releases | Human owner authorization for each operation; never delegated by implication |

Credentials do not increase the permission level. A tool's technical capability is not authorization to use it.

## 4. Standard workflow

`Intake → Baseline → Source inspection → Independent challenge → Options → Owner decision → Scoped change → Independent review → Verification → Owner closure`

Before changing files, record the target SHA, allowed paths, prohibited paths, acceptance criteria, required checks, and rollback/recovery approach. After the change, record exact paths, diff summary, commands actually run, results, and unresolved limitations.

## 5. Handoffs and conflicts

A handoff must contain: task ID, source SHA, objective, constraints, inspected paths, findings with evidence, proposed changes, unverified assumptions, and next action. If two agents disagree, preserve both claims, identify the exact source/version behind each, and request owner resolution when the conflict is a policy, gameplay, or architecture decision.

## 6. Model selection

Select a model or agent by measured task performance, repository grounding, instruction compliance, traceability, reproducibility, privacy, cost, and latency. Do not designate a provider based on marketing claims or an untested recommendation. See [evaluation protocol](AI_MODEL_EVALUATION_PROTOCOL.md).
