# Offline mechanics tests

`tests/OfflineMechanics.Tests` runs WotR Homebrew (UMM Id `AttributeFeats`) against the real Pathfinder:
Wrath of the Righteous assemblies, vanilla blueprint pack and settings in an
ordinary .NET Framework test process. It does not start `Wrath.exe`, the Unity
player or the Unity Editor, and it does not touch saves or the installed `Mods`
folder.

The game-independent part is
[wotr-testing](https://github.com/Acture/wotr-testing) (`WotR.Testing.Offline`),
included as the public submodule `external/wotr-testing`. Its
[documentation](https://github.com/Acture/wotr-testing/blob/main/docs/offline.md)
describes how the game runs without Unity, every environment adaptation, the
result classes and the general limits. This page covers what the mod's tests
and what they found.

## Setup

- Windows, PowerShell 7 and the .NET 10 SDK.
- Initialize the submodule (it is public; `notes` stays optional):

  ```powershell
  git submodule update --init -- external/wotr-testing
  ```

- One copy of the game files: `-WotrInputRoot`, the `WOTR_INPUT_ROOT` variable, a
  snapshot in `vendor/wotr/`, or `WrathInstallDir` from the local `GamePath.props`,
  in that order. To create a snapshot:

  ```powershell
  pwsh -NoProfile -File external/wotr-testing/scripts/New-WotrSnapshot.ps1 -WrathInstallDir "<game directory>" -Destination vendor/wotr
  ```

  `vendor/wotr/` is ignored by Git. These are licensed game files: never commit,
  package or upload them.

## Running

```powershell
pwsh -NoProfile -File scripts/Invoke-OfflineMechanicsTests.ps1
pwsh -NoProfile -File scripts/Invoke-OfflineMechanicsTests.ps1 -WotrInputRoot vendor/wotr -Filter "FullyQualifiedName~MainAttribute"
```

The mod is built from this checkout without deploying it, against the same game
inputs. Reports go to `artifacts/test-results/offline-mechanics/`: `wotr-offline.trx`,
`environment.json` and `summary.json`/`summary.md`. The run fails on any failed,
errored or unexplained skipped test, on zero tests, missing reports or a timeout.
Paths that need the Unity engine are reported as `unity-runtime-required`, never
as passed.

Besides the game startup stages, the test project fails initialization unless the
mod logs "AttributeFeats: registry initialized.".

## Current coverage

| Test | Shows |
|---|---|
| Environment: inputs and build | Game, Unity and mod assemblies load from the prepared folder; the mod is this build |
| Environment: no game process | No `Wrath`/Unity process starts; `UnityPlayer.dll`, Unity's Mono runtime and `Wrath.exe` are not loaded |
| Environment: method bodies | Rewritten `Assembly-CSharp`, `Assembly-CSharp-firstpass` and `Owlcat.Runtime.Core` keep every method body |
| Environment: mod initialization | `Main.Load` and the `BlueprintsCache.Init` postfix register all families and menus |
| Main Attribute Mastery (Strength → Dexterity) | On a vanilla human pregen, the Strength modifier (minimum 0) appears as one inherent bonus on Dexterity and nowhere else; removal restores every stat |
| Retired Titan's Apotheosis | Half the Strength modifier (rounded down) appears as an inherent bonus on the other five attributes and nowhere else; removal restores every stat |
| Failure control | The same assertion fails when the mastery's stat component is removed |
| Summoning probe (Bloodline of Beasts) | Runs the game's `RuleSummonUnit` with a pre-made vanilla unit: the summoner keeps only the trigger buff; the summon gets the bonus buff, with the summoner as caster and the summoner's Strength modifier as the value |

These are evidence for the listed feats only. Other families, conditional
triggers, combat rolls, weapon damage and the in-game UI are not covered here;
see [validation.md](validation.md) for the remaining checks.

## Observations

- **Runtime.** The tests run on .NET Framework 4.8 with rewritten Unity copies, not
  on Unity's Mono. Confirm important results in the game.
- **Starting equipment.** The pregen's starting weapons and armor fail to equip
  offline, so weapon and armor feats need this resolved first.
- **Undead and constructs.** The game gives them no Constitution, so the
  Constitution bonus cannot show on them. The tests check that their unit is a
  living creature.
- **BlueprintCore validation.** During registration, BlueprintCore warns about
  multiple components on several feats where only one is allowed (for example
  `AttackStatReplacementFixed`). The warnings are listed in `environment.json`
  (`logWarnings`) and are not evaluated by these tests.
- **CI.** Public CI has no game files and does not run these tests.
