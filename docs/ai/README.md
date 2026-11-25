# AI Engineering Operations — CostOfStaying

**Status:** ACTIVE operational guidance where consistent with the repository's existing governance; provider selection and external data sharing remain owner-controlled.  
**Scope:** AI-assisted research, documentation, review, implementation support, and verification.  
**Authority:** `AGENTS.md`, `docs/governance/AI_AND_AUTOMATION_POLICY.md`, `docs/governance/GIT_AND_MERGE_POLICY.md`, accepted ADRs, and explicit owner instructions take precedence.

## Operating documents

- [Agent operating model](AI_AGENT_OPERATING_MODEL.md) — roles, handoffs, permissions, and evidence.
- [Agent governance and safety policy](AI_AGENT_GOVERNANCE_POLICY.md) — mandatory controls and prohibited actions.
- [Model and agent evaluation protocol](AI_MODEL_EVALUATION_PROTOCOL.md) — repeatable, evidence-based selection.
- [Toolchain recommendations](AI_TOOLCHAIN_RECOMMENDATIONS.md) — current tools, candidates, and adoption gates.
- [Role catalog](AGENT_ROLE_CATALOG.md) — task boundaries and expected deliverables for each role.

## Portable Agent Skills

The repository's provider-neutral skills live under `.agents/skills/`. Each skill defines its purpose, inputs, process, outputs, and safety constraints. A skill is an instruction package, not an executable permission or a guarantee that a particular agent product discovers it.

## Current limitations

- No external model provider is designated as the project's permanent agent.
- No external provider connection, subscription, API key, or agent execution is implied by these files.
- No agent has permission to push, merge, alter repository settings, access secrets, or approve gameplay/architecture decisions by default.
- Domain/Core architecture, gameplay rules, balance values, Unity, Android, networking, and hosting remain subject to their existing decision gates.

## Integration and verification record

- [Documentation and AI governance push — 2026-11-25](../operations/AI_DOCUMENTATION_PUSH_2026-11-25.md) — exact commit, CI evidence, and remaining gates.
