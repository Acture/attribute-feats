# P-831 / OSS-77 verification

Validated on 2026-10-04 against the local WotR managed assemblies and BlueprintCore 2.8.6, with .NET SDK 10.0.401 and .NET Framework 4.8.

```powershell
dotnet build 'attribute feats.csproj' -c Release --no-restore -p:DeployToGame=false
dotnet build tests/FeatText/FeatText.csproj -c Release --no-restore
./tests/FeatText/bin/Release/net48/FeatText.exe
python tools/validate_assets.py --release bin/AttributeFeats-0.1.2.zip
```

- Release and text-check builds: 0 warnings, 0 errors.
- 8 embedded-resource / fallback checks passed outside the game.
- 92 feat definitions; 196 bilingual resource entries; unique English and Chinese feat names.
- Blueprint constructor identities, GUID constants / inline GUIDs and configurator mechanics match the inherited branch. Text and icons are the intended exceptions.
- 92 separate PNG files, each 128×128, with 92 distinct SHA-256 values. Every feat resolves to its assigned icon; associated objects share parent artwork.
- Release ZIP includes all 92 images with byte-for-byte correspondence to `Icons/`; the only shipped DLL is `AttributeFeats.dll`; manifest version is 0.1.2.
- All images decoded successfully for the [contact sheet](feat-icons.png). Visual inspection found readable subject silhouettes at the shipped resolution.
- 13 inherited images were retained; 79 new images were generated separately with built-in `image_gen`. Exact prompts, generation source IDs and shipped-file hashes are in [icon-manifest.json](icon-manifest.json). Original new images remain in the generator's default directory, with worktree copies under ignored `artifacts/icon-sources/`.

These checks establish source/resource/asset/build/packaging correctness. They do not establish actual combat effects, live locale switching or in-game font/layout correctness. See [the scope and mechanism follow-ups](P-831-review.md); OSS-59 owns real-game effect evidence and OSS-142 owns the remaining localization infrastructure.
