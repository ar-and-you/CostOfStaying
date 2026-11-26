# Security Policy

## Supported versions

This repository is in pre-production. Security fixes are considered against the current `main` branch unless the maintainer explicitly identifies another supported revision. No production service or released binary is represented by this policy.

## Reporting a vulnerability

Please do **not** disclose suspected vulnerabilities in a public issue, discussion, pull request, or commit.

1. If GitHub's private vulnerability reporting is enabled for this repository, use the repository's **Report a vulnerability** flow.
2. If that private reporting option is unavailable, contact the repository owner through a private channel and include a concise description, affected commit or path, impact, and reproducible steps where safe.
3. Do not include real user data, credentials, tokens, or exploit payloads that could harm other users.

The repository has not published a dedicated security contact address or a guaranteed response/remediation SLA. Do not assume that a report has been received until the maintainer acknowledges it.

## Scope and limitations

- A successful CI or CodeQL run is not proof that the repository is vulnerability-free.
- Dependabot and Code Scanning alert contents may be unavailable to a reviewer without the required GitHub permissions; inaccessible alerts must be reported as **UNKNOWN**, not clean.
- Do not test against systems, accounts, or infrastructure that you do not own or have explicit permission to assess.
- Coordinate disclosure with the maintainer before publishing technical details.
