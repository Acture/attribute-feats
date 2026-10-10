# Validation

Run these checks from the repository root. GitHub Actions runs the first six
commands on Windows and Linux.

```powershell
pwsh -NoProfile -File scripts/Test-RepositoryContracts.ps1
dotnet run --project tests/WeaponDamage.Tests --configuration Release
dotnet run --project tests/FeatBudget.Tests --configuration Release
dotnet run --project tests/Initialization.Tests --configuration Release
dotnet run --project tests/FeatText --configuration Release
python scripts/validate_assets.py
```

Use PowerShell 7, the .NET 10 SDK and Python 3.10 or newer. Localization tests
declare Newtonsoft.Json as a NuGet dependency; they need no game assemblies.
They compile the production text loader and registration/refresh methods with
an in-memory replacement for the external localization APIs. They check all
all bilingual resource entries (289 at the time of writing), every menu, weapon and budget key, EN → zhCN → EN
refresh, runtime budget text, English fallback, compiled fallback and
encyclopedia tagging calls.

Feat budget checks run the production budget rules without game assemblies:
exactly full and exceeded count/point limits, both limits together, cross-family
pooling, parametrized choices and ranks, over-budget characters after a lowered
limit, consecutive picks in one level-up, and cancelled or changed picks under
Wrath's replay of pending choices. Initialization checks confirm that only leaf
feats are budgeted, menus are not, and rolled-back registrations are excluded.
These are rule tests. They do not prove the game's level-up and respec screens,
tooltips or saved characters behave the same way.

The repository contract checks preserve the 133 published blueprint identifiers
and allow new unique IDs. Asset checks compare the existing families' component
calls and inline IDs with a committed baseline extracted from v0.1.1, then check
every PNG mapping (152 at the time of writing), hash, dimension and catalog name. These are static checks of
identity and component configuration, not proof of combat behavior.

With a local Wrath installation configured in the ignored root `GamePath.props`:

```powershell
dotnet build ACHomebrew.slnx -p:DeployMod=false
powershell -NoProfile -File tests/VerifySettings.ps1 -WrathInstallDir "<game directory>"
powershell -NoProfile -File tests/VerifyLiveWeaponDamageMode.ps1 -WrathInstallDir "<game directory>"
powershell -NoProfile -File tests/VerifyHarmonyTargets.ps1 -WrathInstallDir "<game directory>"
dotnet build ACHomebrew.slnx -c Release -p:DeployToGame=false
python scripts/validate_assets.py --release artifacts/packages/ACHomebrew-0.2.0.zip
```

The reflection checks run in Windows PowerShell 5.1 against installed
assemblies without starting the game. VerifyHarmonyTargets resolves every
Harmony patch target in the built mod against the game assembly. They check settings serialization, including feat budget defaults, and
whether an existing weapon-damage component reads changed mode settings.
The package check verifies loader metadata, icon coverage and bytes, and that
no dependency/game DLLs or documentation directories are included.

## Offline mechanics with the real game files

```powershell
pwsh -NoProfile -File scripts/Invoke-OfflineMechanicsTests.ps1
```

This Windows-only check needs the public `external/wotr-testing` submodule
(`git submodule update --init -- external/wotr-testing`). It loads the installed
(or `vendor/wotr/` snapshot) game
assemblies, vanilla blueprints and settings in a .NET Framework test process
without starting the game. It applies Main Attribute Mastery and the retired
Titan's Apotheosis to a vanilla unit and removes them, runs a failure control and probes Bloodline of Beasts through the
game's summon rule. Reports are written to `artifacts/test-results/offline-mechanics/`.
See [offline-mechanics-testing.md](offline-mechanics-testing.md) for the inputs,
every environment adaptation, result classes and limits. Public CI does not run
it because it needs licensed game files.

The checks above do not execute attacks in Wrath, prove all feats work, exercise
the real Unity locale-change hook, or verify the game's font/layout behavior.
Those remain game integration checks. Keep their results separate from CI.

Feat budget game checks require a real Wrath session; a missing game, zero
cases or failed initialization is not a pass. With each limit mode enabled,
confirm that level-up and respec screens agree with the character's final feats;
picks in one level-up, cancelling and changing picks, Weapon Damage categories
and Trickster prerequisite skipping respect the budget; the tooltip and settings
usage match; reloading a save keeps the same usage; and lowering a limit marks
the character over budget without removing feats. Also check that vanilla and
other mods' feats are not counted.

To refresh public asset documentation after an intentional name or icon change:

```powershell
python scripts/update_catalog_docs.py
python scripts/update_icon_manifest.py
powershell -NoProfile -File scripts/icon-contact-sheet.ps1
```

The contact-sheet command requires Windows/System.Drawing.
