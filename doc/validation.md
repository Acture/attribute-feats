# Validation

Run these checks from the repository root. They do not need the private `notes`
submodule. GitHub Actions runs the first five commands on Windows and Linux.

```powershell
pwsh -NoProfile -File scripts/Test-RepositoryContracts.ps1
dotnet run --project tests/WeaponDamage.Tests --configuration Release
dotnet run --project tests/Initialization.Tests --configuration Release
dotnet run --project tests/FeatText --configuration Release
python scripts/validate_assets.py
```

Use PowerShell 7, the .NET 10 SDK and Python 3.10 or newer. Localization tests
declare Newtonsoft.Json as a NuGet dependency; they need no game assemblies.
They compile the production text loader and registration/refresh methods with
an in-memory replacement for the external localization APIs. They check all
244 bilingual resource entries, the 48 new menu/weapon keys, EN → zhCN → EN
refresh, English fallback, compiled fallback and encyclopedia tagging calls.

The repository contract checks preserve the 133 published blueprint identifiers
and allow new unique IDs. Asset checks compare the existing families' component
calls and inline IDs with a committed baseline extracted from v0.1.1, then check
all 92 PNG mappings, hashes, dimensions and names. These are static checks of
identity and component configuration, not proof of combat behavior.

With a local Wrath installation configured in the ignored root `GamePath.props`:

```powershell
dotnet build AttributeFeats.slnx -p:DeployMod=false
powershell -NoProfile -File tests/VerifySettings.ps1 -WrathInstallDir "<game directory>"
powershell -NoProfile -File tests/VerifyLiveWeaponDamageMode.ps1 -WrathInstallDir "<game directory>"
dotnet build AttributeFeats.slnx -c Release -p:DeployToGame=false
python scripts/validate_assets.py --release artifacts/packages/AttributeFeats-0.1.2.zip
```

The two reflection checks run in Windows PowerShell 5.1 against installed
assemblies without starting the game. They check settings serialization and
whether an existing weapon-damage component reads changed mode settings.
The package check verifies loader metadata, icon coverage and bytes, and that
no dependency/game DLLs or documentation directories are included.

These checks do not execute attacks in Wrath, prove all feats work, exercise
the real Unity locale-change hook, or verify the game's font/layout behavior.
Those remain game integration checks. Keep their results separate from CI.

To refresh public asset documentation after an intentional name or icon change:

```powershell
python scripts/update_catalog_docs.py
python scripts/update_icon_manifest.py
powershell -NoProfile -File scripts/icon-contact-sheet.ps1
```

The contact-sheet command requires Windows/System.Drawing. Internal artwork
prompts and design/audit records belong in the private notes repository.
