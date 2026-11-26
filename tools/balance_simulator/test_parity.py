#!/usr/bin/env python3
"""
Regression tests for the Python simulator only.
The legacy filename is retained for compatibility. These tests do not execute
or compare against the C# Domain implementation and therefore do not establish
cross-language parity.
"""
import pathlib
import sys
sys.path.insert(0, str(pathlib.Path(__file__).parent))
from simulate import MatchSim, PRESETS, validate_presets

def make_config(player_count=4):
    cfg = dict(PRESETS["default"])
    cfg["playerCount"] = player_count
    cfg["collapseIntervalSeconds"] = 20.0
    cfg["collapseIntervalThreeAliveSeconds"] = 15.0
    cfg["baseDecayPerSecond"] = 0.1
    cfg["matchTimeLimitSeconds"] = 1200.0
    return cfg

def test_preset_validation_accepts_all_repository_presets():
    validated = validate_presets(PRESETS)
    assert set(validated) == {"default", "fast-decay", "slow-decay"}
    # Older exploratory presets omit this field; retain the Domain default.
    assert validated["fast-decay"]["hintCooldownSeconds"] == 15.0


def test_preset_validation_rejects_missing_unknown_and_invalid_values():
    valid = dict(PRESETS["default"])

    missing = dict(valid)
    del missing["playerCount"]
    try:
        validate_presets({"broken": missing})
        raise AssertionError("missing required field was accepted")
    except ValueError as exc:
        assert "playerCount" in str(exc)

    unknown = dict(valid)
    unknown["playerCounnt"] = 4
    try:
        validate_presets({"broken": unknown})
        raise AssertionError("unknown field was accepted")
    except ValueError as exc:
        assert "playerCounnt" in str(exc)

    invalid = dict(valid)
    invalid["tickHz"] = 0
    try:
        validate_presets({"broken": invalid})
        raise AssertionError("zero tickHz was accepted")
    except ValueError as exc:
        assert "tickHz" in str(exc)

    invalid = dict(valid)
    invalid["startingVitality"] = float("nan")
    try:
        validate_presets({"broken": invalid})
        raise AssertionError("non-finite vitality was accepted")
    except ValueError as exc:
        assert "startingVitality" in str(exc)


def test_vitality_decay_and_nonfinite_tick():
    ms = MatchSim(make_config(player_count=1), seed=0)
    ms.tick(float("nan"))
    ms.tick(float("inf"))
    assert ms.players["P0"].vitality == 100.0
    ms.tick(10.0)
    assert abs(ms.players["P0"].vitality - 99.0) < 1e-9

def test_debuff_decay_stops_at_expiry_during_long_tick():
    ms = MatchSim(make_config(player_count=1), seed=0)
    ms.players["P0"].debuffs.append(
        {"id": "short-lived", "bonus": 0.5, "remaining": 1.0, "transferable": True}
    )

    # Base decay applies for five seconds; the debuff bonus applies for one.
    ms.tick(5.0)

    assert abs(ms.players["P0"].vitality - 99.0) < 1e-9
    assert ms.players["P0"].debuffs == []


def test_tick_reschedules_remaining_collapses_when_player_dies():
    ms = MatchSim(make_config(player_count=4))
    ms.players["P0"].vitality = 0.05

    ms.tick(1.0)

    assert ms.players["P0"].state == "Spirit"
    # Three survivors use the 15s interval. Preserve the 19/20 remaining
    # fraction from the original 20s interval at t=1: 1 + 0.95*15.
    for player in list(ms.players.values())[1:]:
        assert abs(player.next_collapse_at - 15.25) < 1e-9
        assert player.last_interval == 15.0


def test_initial_collapse_interval_uses_roster_size():
    three = MatchSim(make_config(player_count=3))
    four = MatchSim(make_config(player_count=4))
    assert {p.next_collapse_at for p in three.players.values()} == {15.0}
    assert {p.next_collapse_at for p in four.players.values()} == {20.0}

def test_transfer_moves_debuff_and_rejects_invalid_distances():
    ms = MatchSim(make_config(player_count=2), seed=0)
    ms.players["P0"].debuffs.append(
        {"id": "test", "bonus": 0.05, "remaining": 30, "transferable": True}
    )
    assert ms.try_transfer("P0", "P1", float("nan"), "bad-nan") == "RejectedInvalidDistance"
    assert ms.try_transfer("P0", "P1", -1.0, "bad-negative") == "RejectedInvalidDistance"
    assert ms.try_transfer("P0", "P1", 1.5, "req-1") == "Success"
    assert len(ms.players["P0"].debuffs) == 0
    assert len(ms.players["P1"].debuffs) == 1
    assert ms.players["P0"].betrayal == 1

def test_unanimous_vote_ends_immediately():
    ms = MatchSim(make_config(player_count=3))
    ms._open_vote()
    voters = list(ms.votes)
    for voter in voters[:-1]:
        assert ms.cast_vote(voter, "Yes") is True
        assert ms.outcome is None
    assert ms.cast_vote(voters[-1], "Yes") is True
    assert ms.outcome == "CollectiveSurrender"

def test_last_survivor_precedes_time_limit():
    cfg = make_config(player_count=4)
    ms = MatchSim(cfg)
    for player in list(ms.players.values())[:3]:
        player.state = "Spirit"
    ms.now = cfg["matchTimeLimitSeconds"]
    ms._evaluate_end()
    assert ms.outcome == "LastSurvivor"
    assert ms.winner == "P3"

def test_single_player_match_ends_at_time_limit():
    cfg = make_config(player_count=1)
    cfg["matchTimeLimitSeconds"] = 1.0
    ms = MatchSim(cfg)
    ms.now = 1.0
    ms._evaluate_end()
    assert ms.outcome == "TimeLimit"
    assert ms.winner == "P0"

def test_gameplay_transfer_is_rejected_after_completion():
    ms = MatchSim(make_config(player_count=2))
    ms.outcome = "TimeLimit"
    assert ms.try_transfer("P0", "P1", 1.0, "after-match") == "RejectedMatchNotActive"

if __name__ == "__main__":
    tests = [
        test_preset_validation_accepts_all_repository_presets,
        test_preset_validation_rejects_missing_unknown_and_invalid_values,
        test_vitality_decay_and_nonfinite_tick,
        test_debuff_decay_stops_at_expiry_during_long_tick,
        test_tick_reschedules_remaining_collapses_when_player_dies,
        test_initial_collapse_interval_uses_roster_size,
        test_transfer_moves_debuff_and_rejects_invalid_distances,
        test_unanimous_vote_ends_immediately,
        test_last_survivor_precedes_time_limit,
        test_single_player_match_ends_at_time_limit,
        test_gameplay_transfer_is_rejected_after_completion,
    ]
    for test in tests:
        test()
        print(f"PASS {test.__name__}")
    print(f"{len(tests)} Python simulator regression tests passed; C# parity not tested.")
