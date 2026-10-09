# WotR Homebrew development (formerly AttributeFeats)

## Repository layout

- The Mod ships as one assembly built from several projects under `src/`:
  `WotRHomebrew` (UMM entry, registration order, `Info.json`, icons, optional
  ModMenu page and packaging), `WotRHomebrew.Logic` (game-independent logic
  and GUIDs, referenced by the tests), `WotRHomebrew.Core` (menus, budget and
  exclusion enforcement, settings, localization and shared builders), and
  theme projects `WotRHomebrew.Attributes`, `.Aptitudes`, `.Martial`,
  `.Defense`, `.Magic`, `.Summoning` and `.Styles`, one file per feat family.
  Group new families by play theme, not by inspiration source. The entry
  project merges every project and BlueprintCore into `WotRHomebrew.dll`
  with ILRepack after build. Keep the UMM Id `AttributeFeats`, the install
  folder `Mods/AttributeFeats` and the settings XML root unchanged so
  existing installs, settings and saves keep working. Shared game references live in `src/WrathMod.props`.
  Open `WotRHomebrew.slnx` from the repository root.
- Keep tests in `tests/`, CLI tools in `scripts/`, and workflows in `.github/`.
- Use `doc/` as the sole public documentation directory and `notes/` for the
  private submodule. Do not recreate a parallel `docs/` directory.
- Build outputs, intermediate files, packages and test reports go in ignored
  `artifacts/`. Keep the shared local `GamePath.props` at the root and untracked.
- Keep `Repository.json` at the root for existing release metadata consumers.
- Offline game tests use the public `external/wotr-testing` submodule
  (https://github.com/Acture/wotr-testing). Change the library there and update the
  gitlink here only to a published commit. Keep local game-file snapshots in ignored
  `vendor/wotr/` (`external/wotr-testing/scripts/New-WotrSnapshot.ps1`); never
  commit or publish them.
- Build for verification with `dotnet build WotRHomebrew.slnx -p:DeployMod=false`
  to avoid copying files into the installed game. Repository contracts run with
  `pwsh -NoProfile -File scripts/Test-RepositoryContracts.ps1`.
- Build a release ZIP without deploying via `dotnet build WotRHomebrew.slnx
  -c Release -p:DeployToGame=false`. Follow [doc/validation.md](doc/validation.md)
  for calculation, initialization, localization, asset and package checks.

## Documentation

- Read [README.md](README.md) for the project and the complete notes workflow.
- [doc/](doc/README.md) contains public documentation maintained with this code
  repository. Put reviewed usage, configuration, integration and contributor
  documentation there; keep the README as the entry point and CHANGELOG for releases.
- Internal design, balance proposals, mod comparisons and testing research belong
  in [notes/首页.md](notes/首页.md) and its linked
  documents. This is a private submodule, not a directory of files to commit in
  the public code repository. Linear remains the source for tasks and status.
- The submodule path is `notes`, its remote is
  `https://github.com/Acture/obsidian-vault.git`, and the editable project branch is
  `project/wotr-homebrew`. Only edit this project's notes at that checkout's root.
  The central vault's master aggregates them under `wotr-homebrew/`.
- Initialize the pinned version with `git submodule update --init --recursive -- notes`.
  Inspect the actual commit, branch and worktree before editing; initialization
  may leave detached HEAD. Preserve unpublished commits and uncommitted changes.
- Follow the central vault's
  [project onboarding guide](https://github.com/Acture/obsidian-vault/blob/master/项目接入.md).
  Install its mandatory push boundary hook in each notes clone as described in
  README, use UTF-8 Python on Windows, and never bypass a failing hook. Do not
  merge the entire vault master into the project branch or rewrite its history.
- Commit and successfully push notes to their project branch before staging the
  parent repository's `notes` gitlink. Verify the notes HEAD is reachable from the
  published branch. Never commit a pointer to an unpublished local notes commit.
- Public documentation in `doc/`, README and CHANGELOG must be usable without
  `notes/`. The submodule reference does not grant private-repository access;
  ordinary users should clone with `--no-recurse-submodules`.
  Code builds and public CI must work without the
  private submodule. Do not enable recursive checkout for public CI merely to
  obtain internal design notes.
- Existing documents and any legacy doc branch remain until content and history
  are accounted for. Preserve unrelated local modifications during migrations.

## Validation

- Confirm repository checks and GitHub-managed CodeQL results on the latest PR
  commit before merging. Keep the CodeQL rule on the default branch enabled.
- Keep historical research observations distinct from current implementation and
  test evidence. Static repository checks do not prove feats work in the game.
- Use CLI and code-based checks; do not use computer use or launch a game as part
  of a documentation-only change.


## Project notes submission

`doc/` is reserved for public documentation. Private research notes live in `notes/`, which tracks `project/wotr-homebrew` in `Acture/obsidian-vault`; start at `notes/首页.md`. This checkout's root contains only this project's notes. Master places these notes under `wotr-homebrew/`. Keep automation and vault configuration on master. Preserve existing local edits when updating a checkout.

Install or refresh the trusted submission tools in Git metadata, including in new clones:

```powershell
$env:PYTHONUTF8 = "1"
git -C notes fetch origin refs/heads/master:refs/remotes/origin/master
git -C notes show origin/master:.github/scripts/install_push_hook.py | python -X utf8 -c "import sys; exec(sys.stdin.read())" --repo notes --source-ref origin/master
$notesCommonGitDir = git -C notes rev-parse --path-format=absolute --git-common-dir
```

After committing specific note files, submit through `python -X utf8 "$notesCommonGitDir/hooks/notes-boundary/submit_project.py" --repo notes` with authenticated `gh`. The remote requires `notes-boundary/root/wotr-homebrew` from GitHub Actions. Only after successful submission should this repository commit and push the `notes` gitlink. See the central repository's `项目接入.md` for initialization, updates and conflict handling.
