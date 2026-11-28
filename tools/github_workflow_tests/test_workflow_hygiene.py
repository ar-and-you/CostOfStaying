from pathlib import Path
import re
import unittest


ROOT = Path(__file__).resolve().parents[2]
WORKFLOW_DIR = ROOT / ".github" / "workflows"
EXPECTED_WORKFLOWS = {"ci.yml", "core-ci.yml", "docs-governance.yml"}
SHA_PIN = re.compile(r"^[^@\s]+@([0-9a-f]{40})$")


def workflow_files():
    candidates = set(WORKFLOW_DIR.glob("*.yml")) | set(WORKFLOW_DIR.glob("*.yaml"))
    return sorted(path for path in candidates if path.name != ".gitkeep")


def job_sections(text):
    """Return indentation-scoped job blocks without requiring a YAML dependency."""
    lines = text.splitlines()
    try:
        jobs_index = next(i for i, line in enumerate(lines) if line == "jobs:")
    except StopIteration:
        return []

    sections = []
    current_name = None
    current_lines = []
    for line in lines[jobs_index + 1:]:
        if line and not line[0].isspace():
            break
        match = re.match(r"^  ([A-Za-z0-9_-]+):\s*$", line)
        if match:
            if current_name is not None:
                sections.append((current_name, "\n".join(current_lines)))
            current_name = match.group(1)
            current_lines = [line]
        elif current_name is not None:
            current_lines.append(line)
    if current_name is not None:
        sections.append((current_name, "\n".join(current_lines)))
    return sections


def step_sections(job_text):
    """Find every step, including unnamed steps."""
    lines = job_text.splitlines()
    starts = [
        index for index, line in enumerate(lines)
        if re.match(r"^      -\s+", line)
    ]
    return [
        "\n".join(lines[start:starts[position + 1] if position + 1 < len(starts) else len(lines)])
        for position, start in enumerate(starts)
    ]


class WorkflowHygieneTests(unittest.TestCase):
    @classmethod
    def setUpClass(cls):
        cls.files = workflow_files()
        cls.contents = {path: path.read_text(encoding="utf-8") for path in cls.files}

    def test_expected_workflows_are_present(self):
        self.assertEqual(
            {path.name for path in self.files},
            EXPECTED_WORKFLOWS,
            "Update the expected set deliberately when adding/removing a tracked workflow.",
        )

    def test_top_level_permissions_are_read_only(self):
        for path, text in self.contents.items():
            with self.subTest(workflow=path.name):
                match = re.search(
                    r"(?ms)^permissions:\s*\n(?P<body>(?:^[ \t]+[^\n]*\n?)*)",
                    text,
                )
                self.assertIsNotNone(match, "Explicit top-level permissions are required.")
                entries = re.findall(
                    r"(?m)^  ([A-Za-z0-9_-]+):\s*([A-Za-z]+)\s*$",
                    match.group("body"),
                )
                self.assertEqual(entries, [("contents", "read")])

    def test_job_level_permissions_are_explicitly_restricted(self):
        for path, text in self.contents.items():
            for job_name, section in job_sections(text):
                with self.subTest(workflow=path.name, job=job_name):
                    declarations = re.findall(r"(?m)^    permissions:(.*)$", section)
                    for declaration in declarations:
                        self.assertEqual(
                            declaration.strip(),
                            "",
                            "Job-level permissions must use an explicit block; inline forms are not accepted.",
                        )
                    blocks = re.findall(
                        r"(?m)^    permissions:\s*\n((?:^      [^\n]*\n?)*)",
                        section,
                    )
                    self.assertEqual(
                        len(blocks),
                        len(declarations),
                        "Every job-level permissions declaration must be parsed and checked.",
                    )
                    for block in blocks:
                        entries = re.findall(
                            r"(?m)^      ([A-Za-z0-9_-]+):\s*([A-Za-z]+)\s*$",
                            block,
                        )
                        self.assertEqual(entries, [("contents", "read")])

    def test_external_actions_are_pinned_to_full_commit_shas(self):
        for path, text in self.contents.items():
            references = re.findall(r"(?m)^\s*uses:\s*([^\s#]+)", text)
            with self.subTest(workflow=path.name):
                self.assertTrue(references, "Expected at least one action reference.")
                for reference in references:
                    self.assertRegex(reference, SHA_PIN)

    def test_every_job_has_a_timeout(self):
        for path, text in self.contents.items():
            sections = job_sections(text)
            with self.subTest(workflow=path.name):
                self.assertTrue(sections, "Workflow must declare at least one job.")
                for job_name, section in sections:
                    self.assertRegex(
                        section,
                        r"(?m)^    timeout-minutes:\s*[1-9][0-9]*\s*$",
                        f"Job {job_name} must have a positive timeout-minutes.",
                    )

    def test_checkout_steps_disable_persisted_credentials(self):
        for path, text in self.contents.items():
            with self.subTest(workflow=path.name):
                checkout_count = 0
                for job_name, job in job_sections(text):
                    for block in step_sections(job):
                        if re.search(r"(?m)^\s+uses:\s*actions/checkout@", block):
                            checkout_count += 1
                            self.assertRegex(
                                block,
                                r"(?m)^\s+persist-credentials:\s*false\s*$",
                                f"Checkout in job {job_name} must disable persisted credentials.",
                            )
                self.assertGreater(checkout_count, 0, "Expected an explicit checkout step.")

    def test_untrusted_privileged_triggers_are_not_introduced_silently(self):
        for path, text in self.contents.items():
            with self.subTest(workflow=path.name):
                self.assertNotRegex(
                    text,
                    r"(?m)^  (pull_request_target|workflow_run):",
                    "Privileged triggers require a separate security review.",
                )


if __name__ == "__main__":
    unittest.main()
