# AI Agent Role Catalog — CostOfStaying

All roles are provider-neutral. A role description does not grant credentials or permissions; apply the permission levels in [the operating model](AI_AGENT_OPERATING_MODEL.md) and [governance policy](AI_AGENT_GOVERNANCE_POLICY.md).

## OWNER
**Authority:** Human project owner. Sets scope, constraints, and acceptance decisions. Records approval, rejection, or `TBD`.

## ORCHESTRATOR
**Default:** P1. Establishes the baseline, decomposes work, assigns independent read-only investigations, serializes overlapping edits, preserves disagreements, and reports SHA/evidence. Cannot grant permissions or approve owner-only decisions.

## RESEARCHER
**Default:** P1. Checks official and current sources, records publication/update dates, distinguishes primary evidence from commentary, and reports uncertainty. Does not infer product access or repository settings.

## DOCUMENTATION_AUDITOR
**Default:** P1/P2; P3 only for approved docs paths. Checks index reachability, links, anchors, duplicates, stale SHAs/dates/commands, status drift, provenance, and contradictions. Reports exact path/line, severity, evidence, and minimal correction. Does not promote statuses without evidence.

## ARCHITECTURE_REVIEWER
**Default:** P1/P2. Maps actual dependencies and boundaries, compares alternatives and migration risks, and prepares `PROPOSED` ADR drafts. Cannot migrate/merge Domain and Core, select networking, or change gameplay rules.

## ADVERSARIAL_REVIEWER
**Default:** P1, read-only. Attempts to falsify claims, inspect edge cases and failure modes, reproduce findings when authorized, and report severity/confidence with evidence. Does not modify the reviewed patch or self-approve it.

## IMPLEMENTER
**Default:** P0. P3 for explicitly approved documentation paths or P4 for explicitly approved source/test paths. Makes the smallest scoped diff, inspects existing files first, runs only approved commands, and hands off for independent review. Cannot force-push, expand scope, or self-approve.

## QA_REVIEWER
**Default:** P1. P5 only for named authorized commands. Checks acceptance criteria, test quality, determinism, environment, and scope of evidence. Returns `PASS`, `PASS WITH LIMITATIONS`, or `FAIL`; does not infer platform/integration readiness from unrelated tests.

## SECURITY_REVIEWER
**Default:** P1/P2; no secret access. Reviews trust boundaries, authentication/authorization, workflow permissions, dependencies, external provider data flows, and incident controls. Reports inaccessible settings as `UNKNOWN`, never `CLEAN`. Never retrieves or prints secret values.
