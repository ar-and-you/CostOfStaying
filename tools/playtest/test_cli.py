#!/usr/bin/env python3
"""CLI regression tests for the simulated-playtest harness."""
import pathlib
import subprocess
import sys
import unittest


SCRIPT = pathlib.Path(__file__).with_name("run_sessions.py")


class RunSessionsCliTests(unittest.TestCase):
    def run_cli(self, session_count):
        return subprocess.run(
            [sys.executable, str(SCRIPT), "--sessions", str(session_count)],
            capture_output=True,
            text=True,
            check=False,
        )

    def test_rejects_zero_sessions(self):
        result = self.run_cli(0)
        self.assertEqual(result.returncode, 2)
        self.assertIn("--sessions must be a positive integer", result.stderr)

    def test_rejects_negative_sessions(self):
        result = self.run_cli(-1)
        self.assertEqual(result.returncode, 2)
        self.assertIn("--sessions must be a positive integer", result.stderr)


if __name__ == "__main__":
    unittest.main()
