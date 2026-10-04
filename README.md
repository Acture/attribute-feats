# AttributeFeats

A Pathfinder: Wrath of the Righteous mod that adds **build-enabling** feats based on character attributes. Choose a Main Attribute feat for broad cross-stat support, or mix focused families to build unconventional defenders, duelists, casters, and summoners.

> **Design philosophy:** AttributeFeats is a *build enabler*, not a power booster. Default 0.1.0 settings open new archetypes without inflating raw damage or attack counts. Extra power is opt-in through `Power Level` and `EnablePowerMode`.

## Feat Families

**0.1.1 total:** Main (6) + Specialized (24) + Stance (6) + Conditional (6) + Replacement (12) + Summon (6) + SummonerSacrifice (3) + ReactiveArmor (2) + Derived (6) + SpellTag (17) + PolearmMaster (1) + DistanceDamage (3) = **92 feats**.

### Main Attribute Mastery (6 feats, mutually exclusive)

| Feat | Attribute |
|---|---|
| Apex Predator | Strength |
| Embodied Grace | Dexterity |
| Living Bulwark | Constitution |
| Architect of Self | Intelligence |
| Wellspring of Insight | Wisdom |
| Crown of Will | Charisma |

### Specialized Adept (24 feats)

| Family | Strength | Dexterity | Constitution | Intelligence | Wisdom | Charisma |
|---|---|---|---|---|---|---|
| Defensive | Titan's Stance | Flowing Form | Iron Bulwark | Calculated Defense | Stoic Vigilance | Indomitable Presence |
| Maneuver | Crushing Grip | Deft Hand | Unyielding Hold | Tactical Bind | Predictive Lock | Domineering Throw |
| Skilled | Practiced Hand | Effortless Skill | Tireless Practice | Polymath's Touch | Quiet Mastery | Inspired Versatility |
| Arcane | Spell-Forged Will | Quickcast Reflex | Spell-Tempered Body | Scholar of the Weave | Oracle's Intuition | Sorcerous Presence |

### Stance (6 feats)

| Feat | Attribute |
|---|---|
| Brutal Stance | Strength |
| Liquid Form | Dexterity |
| Endless Vigor | Constitution |
| Tactical Mind | Intelligence |
| Centered Mind | Wisdom |
| Commanding Presence | Charisma |

### Conditional Trigger (6 feats)

| Feat | Attribute |
|---|---|
| Endless Resolve | Constitution |
| First Blood | Dexterity |
| Vendetta | Charisma |
| Patient Hunter | Wisdom |
| Berserker's Last Stand | Strength |
| Tactical Reading | Intelligence |

### Replacement (12 feats)

**Weapon Insight (6)**

| Feat | Attribute |
|---|---|
| Crushing Form | Strength |
| Duelist's Eye | Dexterity |
| Iron Stance | Constitution |
| Tactical Strike | Intelligence |
| Predictive Cut | Wisdom |
| Theatrical Combat | Charisma |

**Extended Replacement (6)**

| Feat | Attribute | Effect |
|---|---|---|
| Inner Sentinel | Wisdom | Wis-to-AC when unarmored |
| Calculated Grip | Intelligence | Int-to-CMB |
| Unyielding Will | Charisma | Cha-to-CMD |
| Brutal Defender | Strength | Str-to-CMD |
| Lightfoot Defense | Dexterity | Dex-to-AC when unarmored (extended pool) |
| Iron Endurance | Constitution | Con-to-HP scaling |

### Greater Summoning (6 feats)

| Feat | Attribute |
|---|---|
| Bloodline of Beasts | Strength |
| Quickened Pact | Dexterity |
| Vital Pact | Constitution |
| Tactical Binding | Intelligence |
| Insightful Summons | Wisdom |
| Magnetic Calling | Charisma |

### Reactive Armor (2 feats)
- Spiked Defense
- Bulwark of Steel

### Derived Stat Conversion (6 feats)
- Arcane Aegis
- Martial Insight
- Skilled Defender
- Mystic Vitality
- Soul Bulwark
- Sword Saint

### Spell Tag Specialist (17 feats)
- **School (8):** Pure Warder, Master Caller, Seer's Edge, Heart's Tyrant, Spellforge, Veilweaver, Death Speaker, Shape-Shifter
- **Descriptor (9):** Inner Flame, Frozen Heart, Storm Channel, Etching Mind, Resonant Voice, Etheric Mind (Force / Int), Radiant Soul (Positive Energy / Cha), Hollow Heart (Negative Energy / Wis), Subtle Tyrant (Mind-Affecting / Cha)

### Summoner Sacrifice (3 feats)

Trade your own ability scores for amplified buffs to your summoned creatures (via the same `SummonedUnitBuff` pattern as Greater Summoning).

- **Body of My Pact** — −all six attributes on self, +equal value to every attribute on summons (1:1)
- **Doubled Bond** — −half on self, +full on summons (1:2 transfer)
- **Empowered Sacrifice** — −Charisma on self, +Strength on summons

### Polearm Master (1 feat)
- **Polearm Master** — reach × 2 with the −4 weapon damage tradeoff. Classic spear/pike trade.

### Distance-Based Damage (3 feats)

Distance-gated +4 weapon damage triggers; pick the band that fits your build.

- **Aggressor's Edge** — +4 damage at ≤ 10 ft (in your face)
- **Marksman's Focus** — +4 damage at ≥ 30 ft (long range)
- **Optimal Range** — +4 damage at the 15–25 ft sweet spot

## Settings

> Settings should be treated as restart-required after changes.

| Setting | Default | Effect |
|---|---|---|
| Power Level | `Balanced` | `Balanced`: full scaling for attributes, defenses, maneuvers, skills, caster level, and spell penetration; reduced scaling for spell DC, BAB, and Power Mode bonuses. `Legacy_AllFull`: all rank-based Main feat bonuses use full modifier scaling. |
| Include Self in Attribute Stack | OFF | A Main feat may add its chosen attribute to itself. |
| **Enable Mutex** | **ON** | When ON, each family enforces its intra-family mutex. When OFF, every mutex prerequisite is skipped — you may take every feat at once. Cross-family same-attribute mutex was removed in 0.1.1 regardless of this toggle. |
| EnableAttributes | ON | Adds Main feat bonuses to the six ability scores. |
| EnableDefenses | ON | Enables AC, CMD, saving throw, and initiative scaling. |
| EnableManeuvers | ON | Enables Combat Maneuver Bonus scaling. |
| EnableChecks | ON | Enables Bluff, Diplomacy, and Intimidate scaling. |
| EnableSkills | ON | Enables all skill scaling. |
| EnableCasterDC | ON | Enables spell save DC scaling where supported. |
| EnableCasterLevel | ON | Enables caster level scaling where supported. |
| EnableSpellPenetration | ON | Enables spell penetration scaling. |
| EnableBAB | OFF | Enables reduced Base Attack Bonus scaling. |
| EnablePowerMode | OFF | Enables attack bonus, damage, AoOs, sneak attack, HP, speed, and fixed +1 reach bonuses. This is intentionally overpowered. |

## Stacking Rules

- Each family enforces its own intra-family mutex (controlled by `EnableMutex`): Main 6-way, each Specialized subfamily 6-way, Stance 6-way, Weapon Insight 6-way, Greater Summoning 6-way, Summoner Sacrifice 3-way, Spell Tag School 8-way, Spell Tag Descriptor 9-way.
- **Cross-family same-attribute mutex was removed in 0.1.1.** Combinations like `Apex Predator` (Str Main) + `Titan's Stance` (Str Defensive) + `Brutal Stance` (Str Stance) are now allowed — same-attribute stacking is a deliberate build option, not a bug.
- Set `EnableMutex = OFF` in mod settings to disable every mutex prerequisite (including intra-family). You can then take any combination of feats; gather every Specialized stat-bonus for a single attribute, or every Stance, etc. Use at your own risk — this is a power option, not the intended baseline.
- Main feats use **Inherent** bonuses; most non-Main bonuses are **Untyped** or use stat replacement, so cross-attribute combinations remain the intended way to build.
- Some effects, such as Wisdom-to-AC style bonuses, may also stack with compatible vanilla class features. That is intentional for build-enabler playstyles.

## Installation (Unity Mod Manager)

1. Install [Unity Mod Manager](https://www.nexusmods.com/site/mods/21) for Pathfinder: Wrath of the Righteous.
2. Build or download `AttributeFeats-0.1.1.zip`.
3. Drop the zip into UMM.
4. Enable the mod in-game.

## Save Compatibility

- **0.1.1 → 0.1.x is non-breaking.** Settings carry over; XML serialization adds the new `EnableMutex` field as `true` by default.
- **0.1.0 → 0.1.1 upgrades** keep all existing feats (GUIDs unchanged). New feats appear in the level-up feat list and Commanding Presence Stance now applies its 30-ft ally aura correctly.
- 0.0.x → 0.1.x is a redesign; back up saves first.

## Building from Source

- Set `WrathInstallDir`, `WrathPath`, or `WRATH_PATH`, or let the project generate the ignored, repository-root `GamePath.props` from `Player.log`.
- From the repository root, run `dotnet build AttributeFeats.slnx -p:DeployMod=false` to compile without deploying to the game.
- Build output is in `artifacts/bin/AttributeFeats/<Configuration>/`; intermediate files are in `artifacts/obj/`.
- To deploy, run `dotnet build AttributeFeats.slnx`. The Deploy target copies files into the local UMM mod folder and creates `artifacts/packages/AttributeFeats-<Version>.zip`.
- Run the game-independent repository checks with `pwsh -NoProfile -File scripts/Test-RepositoryContracts.ps1`. These check blueprint IDs and loader metadata; they do not test combat effects.

GitHub Actions runs the repository checks on Windows and Linux. GitHub-managed
CodeQL default setup scans C# and Actions for security issues; review its results
on the latest pull-request commit before merging. These static checks do not
replace in-game behavior tests.

### Repository layout

| Path | Contents |
|---|---|
| `src/AttributeFeats/` | Mod project, C# sources, resources, `App.config` and loader `Info.json` |
| `tests/` | Test projects and compatibility baselines |
| `scripts/`, `.github/` | Local commands and CI workflows |
| [doc/](doc/README.md) | Public documentation |
| `notes/` | Optional private notes submodule |
| `artifacts/` | Ignored build outputs, intermediate files, packages and test reports |

The root keeps `AttributeFeats.slnx`, shared build configuration, repository
configuration, README, CHANGELOG and `Repository.json`. The local `GamePath.props`
is shared by the Mod and test projects and must not be committed.

## Changelog

See [CHANGELOG.md](./CHANGELOG.md).

## Documentation

| Location | Audience and content | Access |
|---|---|---|
| [doc/](doc/README.md), README and CHANGELOG | Public usage, setup, supported behavior and contributor documentation | Included in the public code repository |
| `notes/` | Internal design drafts, investigations, experiment records and local mod inventories | Optional submodule; separate private-repository permission required |

Publish reviewed, user-facing documentation in `doc/` with the code. Keep internal
working records in `notes/`; public documentation and builds must remain usable
without it.

## Internal design and research notes

Design proposals, compatibility investigations and testing research live in the
private [project notes](notes/首页.md). The `notes/` Git submodule
uses the existing [Acture/obsidian-vault](https://github.com/Acture/obsidian-vault)
repository and its `project/attribute-feats` branch. Only edit this project's notes
at that checkout's root. Public installation instructions and the changelog remain
in this repository; building or using the mod does not require private notes access.

Cloning or forking this public repository does not grant access to the private
vault. The public `.gitmodules` file and gitlink expose its repository URL,
configured branch and pinned commit ID, but do not contain the notes or their Git
history. GitHub still requires separate authorization to fetch those contents.
Without it, recursive cloning or initializing `notes/` will fail at that step;
use the public clone command below instead.

The central vault's [project onboarding guide](https://github.com/Acture/obsidian-vault/blob/master/项目接入.md)
owns the shared workflow and push checks. The commands below apply it to this
project; they do not set up another synchronization system.

### Clone and initialize

For public code and documentation, skip the optional private submodule:

```powershell
git clone --no-recurse-submodules https://github.com/Acture/attribute-feats.git
```

With authenticated access to the private notes repository:

```powershell
git clone --recurse-submodules https://github.com/Acture/attribute-feats.git
cd attribute-feats
```

For an existing clone or a new worktree, initialize the version recorded by its
current code commit:

```powershell
git submodule update --init --recursive -- notes
git submodule status -- notes
git -C notes rev-parse HEAD
```

The parent repository records an exact notes commit. Initialization normally
leaves the submodule in detached HEAD; the `branch` entry in `.gitmodules` selects
the remote update source, but does not automatically check out an editable branch.
Use `git submodule update --init --recursive -- notes` after switching code
versions to restore their recorded notes version, only when the notes worktree is clean.

### Update and edit

First inspect `git status` and `git -C notes status`. Preserve any uncommitted
notes and unpublished commits before switching branches or updating the gitlink.
Fetch and check that the current notes commit is already part of the published
project branch:

```powershell
git -C notes fetch origin
git -C notes log --oneline origin/project/attribute-feats..HEAD
```

If the last command lists commits, stop and reconcile that work before switching.
For the first edit in a newly initialized clone, create the local tracking branch:

```powershell
git -C notes switch -c project/attribute-feats --track origin/project/attribute-feats
```

If that local branch already exists, use `git -C notes switch project/attribute-feats`
instead. Then update without rewriting history:

```powershell
git -C notes merge --ff-only origin/project/attribute-feats
git -C notes branch --show-current
```

Stop on divergence; do not force-push, discard local work, or merge the entire
vault `master` into this project branch. Follow the central guide for bringing
back changes made to this project's notes in the total vault.

Install the central repository's mandatory push boundary check in each notes
clone, and refresh it when the central checker changes. These PowerShell commands
use UTF-8 for Python on Windows and load the installer from the trusted vault:

```powershell
$env:PYTHONUTF8 = "1"
git -C notes fetch origin refs/heads/master:refs/remotes/origin/master
git -C notes show origin/master:.github/scripts/install_push_hook.py | python -X utf8 -c "import sys; exec(sys.stdin.read())" --repo notes --source-ref origin/master
```

This requires Python 3.10+, Git and authenticated `gh`. Keep `PYTHONUTF8=1` in the Windows shell used
for notes pushes. The installer preserves existing custom hooks and stops if they
need reconciliation. Hooks are local to a clone, are not copied by Git, and must
not be bypassed with `--no-verify`.

Edit files under `notes/`, then commit and submit the notes first. Stage only the
specific files you edited; this example stages the project homepage:

```powershell
git -C notes diff --stat
git -C notes add 首页.md
git -C notes commit -m "docs: update AttributeFeats design notes"
$notesCommonGitDir = git -C notes rev-parse --path-format=absolute --git-common-dir
python -X utf8 "$notesCommonGitDir/hooks/notes-boundary/submit_project.py" --repo notes
```

Only after that push succeeds, verify the published history and commit the parent
pointer. Run each step only if the preceding command succeeds:

```powershell
git -C notes fetch origin
git -C notes merge-base --is-ancestor HEAD origin/project/attribute-feats
git diff --submodule=log -- notes
git add -- notes
git commit -m "docs: update project notes reference"
git push
```

The ancestor check must exit with code 0. Keep unrelated staged work out of the
pointer commit and use the code repository's normal review branch for delivery.
The vault's existing project-to-master aggregation is separate from updating this
repository's gitlink.

To follow the latest published notes without editing, start with clean, fully
published notes and run `git submodule update --remote --checkout -- notes`.
Review and commit the resulting parent pointer using the same steps above.
Avoid this command when reproducing a fixed code version; normal initialization
uses the pinned commit instead. Any legacy `doc` branch remains until its content
and history have been verified separately.

## Credits

Thanks to @CasDragon for code snippets and ideas. AttributeFeats grew out of earlier Redditor class-feat experiments and was rebuilt for the 0.1.0 build-enabler release.
