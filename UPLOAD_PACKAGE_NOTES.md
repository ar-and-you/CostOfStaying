# CostOfStaying — Import and Reconciliation Notes

**Updated:** 2026-11-21  
**Canonical source archive:** `CostOfStaying-FINAL-verified-2026-11-20.zip`  
**Archive SHA-256:** `628a351cc21989c1b6a0152b61bfa5e9d0e1b6d25b42420dcce7ab016a6b0946`  
**Destination:** `https://github.com/ar-and-you/CostOfStaying`

## Source precedence

The live GitHub `main` branch is the authority for current tracked files and history. The supplied consolidated archive is a source reference for selective reconciliation, not a replacement repository.

The bundle's `main` ref is `8b811b0c23329481dd49de0586616f75558a1f89`. The live repository was at `422059214b2c2738856633bcfa4fb729bb6d0ffa` when reviewed. Both share base `fc8dd378c3066285f67dac0b73e6ade160233da0`, but the bundle is divergent and is not a fast-forward update. Do not push the bundle branch wholesale, replace `.git`, or force-push.

## Resolved conflicts

- Preserve live `main`, its root `ROADMAP.md`, source files and tests.
- Do not replace current files merely because a same-path copy exists in an archive.
- Keep Domain and Core separate until the applicable architecture decision and migration plan are present in live governance and reviewed.
- `test_parity.py` is a Python-only regression test, not proof of C# parity.
- Simulated sessions are not human playtest evidence.
- Add two independent CI lines because Domain uses .NET 8 and Core tests use .NET 10.
- Workflow configuration is not a successful CI result; check Actions after pushing.
- Follow the current live `ROADMAP.md` direct-to-main instruction for this update. An archive-only hybrid branch-policy decision is not silently promoted to live policy.

## Excluded from this update

The archive contains additional governance, audit, security, community, and repository-quality material. These items are not bulk-imported. In particular, `tools/audit.py`, GitHub configuration tests, repository setup helpers, security workflows, Dependabot and CODEOWNERS require a separate compatibility review.

## Verification limits

Archive integrity checks passed. Python checks run against the extracted source archive are reported separately and do not automatically establish results for the live repository. The current execution environment did not have the .NET SDK installed, so no new .NET build/test result is claimed here. Unity, Android and dedicated-server integrations remain unverified.

## تعليمات الوكيل المضافة في هذه الحزمة

أُضيف `AGENTS.md` كملف جديد في جذر المستودع. صيغت تعليماته بحيث لا تتجاوز ADRs أو مواصفات القواعد أو `ROADMAP.md`. وتتبنى سياسة Git الموثقة حالياً في `ROADMAP.md` بدلاً من فرض سياسة PR جديدة غير معتمدة.
