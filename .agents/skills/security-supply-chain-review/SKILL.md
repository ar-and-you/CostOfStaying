---
name: security-supply-chain-review
description: Review secrets, dependency provenance, workflow permissions, and external tool data flows.
---

# Security Supply Chain Review

1. Read security and AI automation policies and establish exact SHA.
2. Inspect manifests, lockfiles, workflows, action references, permissions, and secret-handling patterns without exposing secret values.
3. Identify dependency maintenance, license, transitive risk, and pinning gaps.
4. Model external provider data flows and agent permissions; mark inaccessible settings as `UNKNOWN`.
5. Report severity, evidence, mitigation, and residual risk.

Never retrieve, print, or transmit secret values. Do not claim clean scans when results are unavailable.
