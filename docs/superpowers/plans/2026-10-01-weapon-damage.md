# Attribute Weapon Damage Implementation Plan

> **For agentic workers:** Use the executing-plans skill to implement this plan task-by-task.

**Goal:** Fulfill issue #7 with standalone attribute-to-weapon-damage feats and a choice of replacement or addition in settings.

**Architecture:** Six parametrized feats select an attribute and a proficient weapon category, under Attribute Feats > Weapon Damage. A rulebook component adjusts only the base weapon damage after the game resolves its normal attribute and hand multipliers. A small pure calculation is shared with executable regression tests.

**Tech Stack:** C#, net48, BlueprintCore 2.8.6, installed Wrath assemblies; dependency-free net10 test runner.

## Behavior

- Each selection costs one feat and applies to one weapon category. The same attribute can be selected again for another category; exact duplicates are blocked by the game's parameter selection.
- Replace (default): use the selected modifier only if it improves damage; preserve the normal weapon damage multiplier, including off-hand and two-handed rules. Existing Dexterity-to-damage is respected.
- Add: retain existing damage and add the positive selected modifier once, without hand multipliers.
- Only weapons already applying an attribute modifier qualify. No spell damage, attack bonus, or other stat changes.
- Weapon Damage Mode is read once per damage calculation from current settings, so switching applies to existing feats without restarting or respeccing. Descriptions show both rules and refer to the setting for the active mode. Other settings still require restarting. These feats do not require Power Mode. Existing EnableMutex restricts this family to one attribute when enabled.
- Conditional combat mechanics and broader configuration remain outside this change.

## Tasks

- [x] Add failing tests in tests/WeaponDamage.Tests for replacement, addition, multipliers, negative values, and eligibility; run to confirm failure (10 expected failures with the initial no-op implementation).
- [x] Implement New_Feats/WeaponDamageRules.cs and run the tests (18/18 passed).
- [x] Add New_Feats/WeaponDamageFeats.cs with the rulebook component and six parametrized blueprints; extend FeatSelection, FeatRegistry, and stable Guids.
- [x] Add the serialized mode in Settings.cs and selector/explanation in Main.cs; exclude tests from the mod compilation.
- [x] Update README and CHANGELOG; compile against installed game without running the Deploy target.
- [x] Review the final change and fix findings. Independent review found no blocking issues; inspected native parameter selection, mutex, damage construction and multiplier rules.

## Validation

Run `dotnet run --project tests/WeaponDamage.Tests -p:NuGetAudit=false` for calculation tests. Compile the mod with `dotnet msbuild 'attribute feats.csproj' -restore -t:Compile` and an explicit WrathInstallDir. This avoids the Build target's automatic deployment.

In-game checks still required: navigate all three selection levels; choose Intelligence/Longsword; select a different weapon category later; reject exact duplicates; test both modes with Str 12/Int 20 in single-hand, two-hand, and off-hand attacks; verify an unrelated weapon, spell, or weapon with no damage attribute is unchanged; check existing Dexterity-to-damage; save/reload; switch modes both ways and attack again without restarting. Reopen any already displayed character-sheet damage preview to request a fresh calculation.

Additional verification: XML deserialization defaults old settings to Replace, both enum modes roundtrip, and existing setting values are preserved. All 133 original blueprint GUIDs remain unchanged; the combined grouping and damage changes introduce 24 unique blueprint GUIDs. Automated tests do not replace the in-game checks above; no build was deployed to the game installation.

Live-mode follow-up: `tests/VerifyLiveWeaponDamageMode.ps1` loads the compiled mod and game types and verifies that the same component follows Replace -> Add -> Replace without recreation, plus the default when settings are absent. The test failed against the captured-mode implementation and passes with live lookup. This verifies mode lookup without running an actual attack. Final review found no blocking regressions.

Initialization-resilience follow-up: each family-registration step and each family-menu step has its own exception boundary. Healthy later steps continue, and root publication is attempted even after a failure. Each registration batch snapshots pending menu entries; failure restores those entries so incomplete feats (including feats whose mutex pass has not run) are not offered. This affects menu collections only, not existing blueprint objects. Logs name the failing step and report partial initialization; the one-attempt guard is retained because previously created blueprints cannot be rolled back safely. `tests/Initialization.Tests` links the real registry and selection code with stand-ins for game/BlueprintCore APIs and family creation. Fault injection covers all registration steps, failed menu construction, simultaneous failures, root publication failure, duplicate-call prevention, and failure after leaves have been collected (single family, multiple families, and a direct-root feat). The original implementation passed 1/19 checks; three additional partial-registration cases also failed before collection rollback was added. The final implementation passes 22/22. This is orchestration coverage, not an in-game test.
