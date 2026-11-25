# Model and Agent Evaluation Protocol — CostOfStaying

**Status:** ACTIVE evaluation method; candidate rankings remain unassigned until tested.  
**Purpose:** Select tools by repeatable evidence rather than brand preference.

## 1. Candidate categories

Evaluate products and underlying models separately. Candidate categories may include ChatGPT-based analysis, coding agents, IDE assistants, CLI agents, and independent review models. A candidate is not connected or approved merely because it is named in this document.

## 2. Fixed benchmark tasks

Run the same tasks against the same commit and evidence pack:

1. Reconstruct repository state from a specified SHA without relying on prior summaries.
2. Detect a conflict between two policy documents and cite exact paths/versions.
3. Find broken relative links, orphaned documentation, and stale status claims.
4. Review a time-based Domain edge case (including an effect expiring within a tick) without inventing gameplay rules.
5. Propose property-based tests for invariants without changing the balance specification.
6. Assess Domain/Core boundaries without claiming an unaccepted ADR.
7. Threat-model a command-processing layer while accurately stating whether a network transport exists.
8. Review a known diff and produce severity-ranked, reproducible findings.

## 3. Scorecard

Score each dimension from 0 to 5 and preserve examples:

| Dimension | Weight |
|---|---:|
| Repository grounding and factual accuracy | 20% |
| Instruction and permission compliance | 20% |
| Defect detection and reasoning | 15% |
| Evidence and traceability | 15% |
| Completeness and prioritization | 10% |
| Reproducibility | 10% |
| Reviewer correction effort | 5% |
| Privacy, cost, and latency | 5% |

These weights are an initial evaluation rubric, not a claim that a particular model has achieved a score. The owner may revise them before a benchmark cycle; do not change weights mid-comparison.

## 4. Test protocol

- Pin the repository SHA, prompt, files, tool permissions, and acceptance criteria.
- Run candidates independently; do not expose competitors' answers before submission.
- Begin with read-only access. Test write operations only in a disposable environment after separate approval.
- Capture product name, model/version when available, date, tool calls, elapsed time, cost, outputs, and reviewer corrections.
- Record false positives, missed defects, unsupported claims, and permission violations.
- Repeat a task when output variance is material.

## 5. Selection gates

A candidate may be approved for a specific role only if it meets the owner's threshold, respects permissions, produces traceable and reproducible work, and passes privacy/cost review. A high average score cannot compensate for a critical security or authorization violation. No majority vote establishes correctness.
