# GitHub workflow hygiene tests

Run from the repository root:

```bash
python3 -m unittest discover -s tools/github_workflow_tests -v
```

The tests use only the Python standard library. They cover the three tracked workflows and check:

- explicit top-level `contents: read` permissions;
- full 40-character commit SHA pins for referenced external Actions;
- positive `timeout-minutes` on each job;
- `persist-credentials: false` on every checkout step, including unnamed steps;
- no unreviewed job-level permission overrides beyond explicit read-only permissions;
- no silent introduction of privileged `pull_request_target` or `workflow_run` triggers.

These standard-library tests are deliberately narrow supplemental guardrails. The Domain/Python CI workflow also runs `actionlint` for workflow syntax/schema/expression checks and `zizmor` for static GitHub Actions security analysis. All three are static checks: they do not replace review of GitHub settings, secret scanning, runtime CI evidence, or host/network security testing. Update the expected workflow set and this document when the workflow inventory changes.
