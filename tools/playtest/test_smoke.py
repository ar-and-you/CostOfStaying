#!/usr/bin/env python3
"""Bounded CI smoke test for the simulated-playtest harness; not a human playtest."""
import csv
import json
import pathlib
import subprocess
import sys
import tempfile
import unittest


SCRIPT = pathlib.Path(__file__).with_name("run_sessions.py")


class RunSessionsSmokeTests(unittest.TestCase):
    def test_one_seeded_session_writes_complete_simulation_artifacts(self):
        with tempfile.TemporaryDirectory(prefix="costofstaying-playtest-") as output:
            result = subprocess.run(
                [
                    sys.executable,
                    str(SCRIPT),
                    "--sessions",
                    "1",
                    "--preset",
                    "default",
                    "--seed",
                    "17",
                    "--output",
                    output,
                ],
                capture_output=True,
                text=True,
                check=False,
                timeout=30,
            )

            self.assertEqual(
                result.returncode,
                0,
                msg=f"stdout:\n{result.stdout}\nstderr:\n{result.stderr}",
            )

            output_dir = pathlib.Path(output)
            session_files = sorted(output_dir.glob("session-*.json"))
            summary_files = sorted(output_dir.glob("summary-*.csv"))
            report_files = sorted(output_dir.glob("report-*.md"))

            self.assertEqual(len(session_files), 1)
            self.assertEqual(len(summary_files), 1)
            self.assertEqual(len(report_files), 1)

            session = json.loads(session_files[0].read_text(encoding="utf-8"))
            self.assertEqual(session["seed"], 17)
            self.assertEqual(session["preset"], "default")
            self.assertIn("SIMULATED", session["note"])

            with summary_files[0].open(encoding="utf-8", newline="") as stream:
                rows = list(csv.DictReader(stream))
            self.assertEqual(len(rows), 1)
            self.assertEqual(rows[0]["seed"], "17")

            report = report_files[0].read_text(encoding="utf-8")
            self.assertIn("SIMULATED runs only", report)
            self.assertIn("Human playtests", report)


if __name__ == "__main__":
    unittest.main()
