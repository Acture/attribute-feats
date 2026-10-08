# AC's Homebrew

> Formerly **AttributeFeats**. The UMM mod Id (`AttributeFeats`), the DLL name and all feat IDs are unchanged, so existing settings and saves keep working.

A Pathfinder: Wrath of the Righteous mod that adds **build-enabling** feats based on character attributes. Choose a Main Attribute feat for broad cross-stat support, or mix focused families to build unconventional defenders, duelists, casters, and summoners.

> **Design philosophy:** AC's Homebrew is a *build enabler*, not a power booster. Main feat defaults focus on build-enabling stats. Standalone Weapon Damage feats let you choose attribute-based damage without taking a Main feat; their default Replace mode avoids adding a second attribute bonus.

## Feat Families

During character creation and level-up, expand **AC's Homebrew**, then a **feat family**, then choose the **individual feat**. Each family has 2–9 choices; Long-Reach Gambit (formerly Polearm Master) sits directly inside Attribute Feats because it has no variants. Each choice grants one feat, with no extra feat cost for either menu level. You can return to the same family at later feat choices, subject to the selected feat's usual prerequisites and mutual-exclusion rules. Existing characters keep their learned feats and effects.

The 17 menus are Main Attribute Mastery, Defensive Adept, Maneuver Adept, Skilled Adept, Arcane Adept, Stance, Conditional Trigger, Weapon Insight, Extended Replacement, Greater Summoning, Summoner Sacrifice, Reactive Armor, Derived Stat Conversion, Spell School Specialist, Spell Descriptor Specialist, Distance Damage, and Weapon Damage.

**0.1.2 total:** Main (6) + Specialized (24) + Stance (6) + Conditional (6) + Replacement (12) + Summon (6) + SummonerSacrifice (3) + ReactiveArmor (2) + Derived (6) + SpellTag (17) + PolearmMaster (1) + DistanceDamage (3) + WeaponDamage (6) = **98 feats** (92 existing feats and 6 new Weapon Damage feats).

### Main Attribute Mastery (6 feats, mutually exclusive)

| Feat | 中文名称 | Attribute |
|---|---|---|
| Titan's Apotheosis | 泰坦登阶 | Strength |
| Quicksilver Incarnate | 水银具现 | Dexterity |
| Adamantine Vessel | 万劫金身 | Constitution |
| Architect of Self | 灵枢架构师 | Intelligence |
| Ocular of the Cosmos | 寰宇天心 | Wisdom |
| Sovereign of Wills | 至高皇威 | Charisma |

### Specialized Adept (24 feats)

| Family | Strength | Dexterity | Constitution | Intelligence | Wisdom | Charisma |
|---|---|---|---|---|---|---|
| **Defensive** | Colossus Bastion<br>(巨灵重障) | Wind-Dancer's Shroud<br>(风舞虚影) | Inured Carapace<br>(百炼金身) | Analytical Aegis<br>(算律之盾) | Third Eye Vigil<br>(天目清照) | Majesty's Reproach<br>(凛然天威) |
| **Maneuver** | Grip of the Behemoth<br>(比蒙扼击) | Fulcrum of the Viper<br>(灵蛇巧掣) | Deep-Root Clinch<br>(沉洋扼锁) | Anatomical Pivot<br>(机理断节) | Crane's Anticipation<br>(玄鹤听劲) | Audacious Overthrow<br>(叱喝倾山) |
| **Skilled** | Giantwright's Craft<br>(巨匠巧工) | Thief-King's Panache<br>(妙手绝尘) | Ascetic Diligence<br>(苦行研磨) | Encyclopedic Synthesis<br>(格物万象) | Wanderer's Lucidity<br>(云水澄明) | Silver-Tongued Virtuoso<br>(锦绣天潢) |
| **Arcane** | Mage-Hammer Inscription<br>(铁骨铸咒) | Somatic Velocity<br>(疾影手印) | Crucible of the Conduit<br>(鼎炉承法) | Archmage's Codex<br>(万法源流) | Gnostic Channel<br>(玄鉴通幽) | Sovereign Decrees<br>(天宪法旨) |

### Stance (6 feats)

| Feat | 中文名称 | Attribute |
|---|---|---|
| Berserker's Overrun | 破阵裂山势 | Strength |
| Willow in the Gale | 惊鸿穿林势 | Dexterity |
| Mountain's Deep Roots | 不动磐峰势 | Constitution |
| Grandmaster's Gambit | 弈者静待势 | Intelligence |
| Mirror of Still Waters | 明镜止水势 | Wisdom |
| Vanguard's Banner | 金戈铁旌势 | Charisma |

### Conditional Trigger (6 feats)

| Feat | 中文名称 | Attribute | Trigger |
|---|---|---|---|
| Defiance at the Precipice | 绝境砥柱 | Constitution | HP < 50% |
| Ambush of the Viper | 封喉首刃 | Dexterity | Combat round 1 |
| Oath of Retribution | 复仇血誓 | Charisma | Ally dies within 30 meters; 3 rounds |
| Crane's Severance | 蓄势孤峰 | Wisdom | First weapon attack each round, including misses |
| Gorum's Last Stand | 狂神绝唱 | Strength | HP < 25% |
| Cadence Decoded | 阅破机宜 | Intelligence | Weapon attack resolves; up to 10 minutes, removed at combat boundaries |

### Replacement (12 feats)

**Weapon Insight (6)**

| Feat | 中文名称 | Attribute |
|---|---|---|
| Titan's Momentum | 巨灵摧破 | Strength |
| Aldori Finesse | 阿尔多里绝诣 | Dexterity |
| Stout Grounding | 磐固千钧 | Constitution |
| Geometer's Edge | 规矩之锋 | Intelligence |
| Karmic Interception | 因果断隙 | Wisdom |
| Swashbuckler's Flourish | 游侠华彩 | Charisma |

**Extended Replacement (6)**

| Feat | 中文名称 | Attribute | Effect |
|---|---|---|---|
| Ascetic's Ward | 云水自真 | Wisdom | Wis-to-AC when unarmored/light armor |
| Anatomical Leverage | 筋络推演 | Intelligence | Int-to-CMB |
| Monarch's Stature | 帝胄岳立 | Charisma | Cha-to-CMD |
| Titan's Footing | 巨灵固步 | Strength | Str-to-CMD |
| Zephyr's Grace | 穿风灵步 | Dexterity | Dex-to-AC when unarmored/light armor |
| Adamantine Mettle | 生机洪炉 | Constitution | Con modifier to HP (minimum 0; added once) |

### Greater Summoning (6 feats)

| Feat | 中文名称 | Attribute | Buffed Summons Stat |
|---|---|---|---|
| Behemoth's Heritage | 比蒙遗脉 | Strength | Strength |
| Zephyr's Covenant | 风灵疾契 | Dexterity | Dexterity & Speed |
| Titan's Lifespring | 巨怪生机 | Constitution | Constitution |
| Aegis of the Schema | 天元魔阵 | Intelligence | AC |
| Empathic Communion | 神契灵犀 | Wisdom | Fortitude, Reflex, Will saves |
| Dominator's Calling | 御统王令 | Charisma | Attack Bonus |

### Summoner Sacrifice (3 feats)

Trade your own ability scores for amplified buffs to your summoned creatures.

- **Martyr's Transference (形神替生)** — −4 to all six attributes on self, +4 to all six attributes on summons (1:1 trade)
- **Eldritch Crucible (双生法炼)** — −2 to all six attributes on self, +4 to all six attributes on summons (1:2 amplification)
- **Tribute of Iron Dominion (夺冕化蛮)** — −4 Charisma on self, +8 Strength on summons (focused trade)

### Reactive Armor (2 feats)
- **Barbed Carapace (逆鳞铁刺)** — In armor, return 1d6 + armor bonus as untyped retaliation damage on melee hit.
- **Citadel of Steel (铸铁城阙)** — In medium/heavy armor, refresh temporary HP equal to armor bonus each round.

### Derived Stat Conversion (6 feats)
- **Arcane Aegis (魔能天衣)** — Half Caster Level to AC.
- **War-Hardened Reflexes (百战身魄)** — Half BAB to all saving throws.
- **Scholar's Positioning (通识御敌)** — One third of total Skill Ranks to AC.
- **Ley-Infused Vitality (灵脉淬体)** — Caster Level to Max HP.
- **Dawn of the Soul (法相初明)** — At combat start, gain temporary HP equal to Caster Level, for up to 10 minutes or until combat ends.
- **Blade of the Spell-Saint (剑圣咒痕)** — Half BAB to spell and ability save DC.

### Spell Tag Specialist (17 feats)
- **School (8):**
  - Aegis of the Pure Warder / 绝界镇魔使 (Abjuration / Wis)
  - Sovereign Gatekeeper / 统界辟门者 (Conjuration / Cha)
  - Eye of the Chronomancer / 溯时先知 (Divination / Int)
  - Sovereign of the Heart / 倾心国主 (Enchantment / Cha)
  - Pyre of the Architect / 灾变筑城师 (Evocation / Int)
  - Phantasmagoria Maestro / 织影幻圣 (Illusion / Cha)
  - Harvester of the Boneyard / 冥河渡魂人 (Necromancy / Wis)
  - Sculptor of Prime Matter / 塑质造化使 (Transmutation / Int)
- **Descriptor (9):**
  - Pyre of the Phoenix / 炽皇凤涅 (Fire / Cha)
  - Stillness of the Glacial Void / 极渊玄冰 (Cold / Wis)
  - Tempest-Dancer / 御雷疾影 (Electricity / Dex)
  - Vitriolic Equation / 腐解算律 (Acid / Int)
  - Herald of the Shattered Sky / 破霄神音 (Sonic / Cha)
  - Axiom of Unseen Force / 虚空定则 (Force / Int)
  - Fountain of Solar Dawn / 金阳圣晖 (Positive Energy / Cha)
  - Vigil of the Gloom / 死寂枯荣 (Negative Energy / Wis)
  - Puppeteer of the Mind / 惑魂主宰 (Mind-Affecting / Cha)

### Polearm Master (1 feat)
- **Long-Reach Gambit (长锋险势)** — reach × 2 with −4 weapon damage; applies to all weapon categories.

### Distance-Based Damage (3 feats)
- **Point-Blank Ruin (咫尺绝杀)** — +4 damage at ≤ 10 ft.
- **Horizon's Deadeye (苍穹神击)** — +4 damage at > 29 ft.
- **Harmonic Cleave (流光截角)** — +4 damage at > 14 ft. and ≤ 25 ft.

### Weapon Damage (6 feats)

Choose **AC's Homebrew → Weapon Damage → an attribute → a proficient weapon category**. The six attributes are Strength, Dexterity, Constitution, Intelligence, Wisdom, and Charisma. Each choice costs one feat and changes only weapon damage for that category. You can choose the same attribute again for another category, but cannot take the exact same attribute/category twice.

The **Weapon Damage Mode** setting offers two behaviors. Switching takes effect on the next damage calculation, including already learned feats; no restart or respec is needed:

| Mode | Effect | Example: Str 12 (+1), Int 20 (+5), one-handed longsword |
|---|---|---|
| Replace (default) | Use the chosen modifier if it improves the existing attribute damage; retain the weapon's normal two-handed/off-hand multiplier. Existing Dexterity-to-damage is considered. | `1d8 + 5` |
| Add | Keep normal damage and add the chosen positive modifier once, without two-handed/off-hand scaling of the extra bonus. | `1d8 + 1 + 5` |

Both modes require an attack that already applies an attribute modifier to weapon damage. Ordinary crossbows and other attacks without a damage attribute receive no benefit. Neither mode changes attack rolls, spells, or other stats, and neither requires Power Mode. With `EnableMutex` on, this family allows one attribute; with it off, replacement uses the best applicable attribute while addition stacks the selected bonuses.

## Settings

> Weapon Damage Mode applies on the next damage calculation and the feat budget on the next selection check. Other settings require restarting.

| Setting | Default | Effect |
|---|---|---|
| Weapon Damage Mode (`WeaponDamage`) | `Replace` | Standalone Weapon Damage feats use a better chosen attribute instead of the existing damage attribute. `Add` adds the positive chosen modifier once. Independent of Main feat settings and Power Mode. |
| Feat count limit (`EnableFeatCountLimit`, `MaxFeatCount`) | OFF, 6 | Maximum number of Homebrew feats per character. See [Feat Budget](#feat-budget). |
| Feat point limit (`EnableFeatPointLimit`, `MaxFeatPoints`) | OFF, 10 | Maximum total Homebrew feat cost per character. Can be enabled together with the count limit. |
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

## Feat Budget

The optional feat budget limits each character's Homebrew feats across all families. Enable a **count limit**, a **point limit**, or both, and set their maximums (0–99) in the mod settings. Changes apply on the next selection check, without restarting.

| Cost | Families |
|---|---|
| 3 | Main Attribute Mastery |
| 2 | Weapon Insight, Extended Replacement, Stance, Greater Summoning, Summoner Sacrifice, Derived Stat Conversion, Spell School Specialist, Spell Descriptor Specialist, Weapon Damage |
| 1 | Defensive, Maneuver, Skilled and Arcane Adept, Conditional Trigger, Reactive Armor, Distance Damage, Long-Reach Gambit |

- Only feats granted to the character count. Opening the AC's Homebrew menu or a family menu is free, and vanilla or other mods' feats are never counted.
- Each Weapon Damage weapon category is a separate feat with its own cost. A feat with multiple ranks counts once per rank.
- Picks earlier in the same level-up count toward later picks. Cancelling or changing a pick frees its budget; later picks that no longer fit are removed from the level-up, as with other prerequisites. Respec frees all budget.
- Feat tooltips show used and maximum feats or points, the feat's cost and why it cannot be chosen. The mod settings list each party member's usage.
- Effects that ignore feat prerequisites, such as the Trickster's, do not bypass the budget. Intra-family mutual exclusions still apply separately.
- Lowering a limit or loading an older save never removes feats. A character above the limit keeps every feat and its effects, is shown as over budget, and cannot choose more Homebrew feats until the limit is raised or the character is respecced.

Costs are initial strength tiers. Rebalancing individual feats is tracked separately from the budget.

## Stacking Rules

- The optional [feat budget](#feat-budget) limits combinations across all families; it does not replace the intra-family mutex.
- Each family enforces its own intra-family mutex (controlled by `EnableMutex`): Main 6-way, each Specialized subfamily 6-way, Stance 6-way, Weapon Insight 6-way, Weapon Damage 6-way, Greater Summoning 6-way, Summoner Sacrifice 3-way, Spell Tag School 8-way, Spell Tag Descriptor 9-way.
- **Cross-family same-attribute mutex was removed in 0.1.1.** Combinations like `Titan's Apotheosis` (Str Main) + `Colossus Bastion` (Str Defensive) + `Berserker's Overrun` (Str Stance) are now allowed — same-attribute stacking is a deliberate build option, not a bug.
- Set `EnableMutex = OFF` in mod settings to disable every mutex prerequisite (including intra-family). You can then take any combination of feats; gather every Specialized stat-bonus for a single attribute, or every Stance, etc. Use at your own risk — this is a power option, not the intended baseline.
- Main feats use **Inherent** bonuses; most non-Main bonuses are **Untyped** or use stat replacement, so cross-attribute combinations remain the intended way to build.
- Some effects, such as Wisdom-to-AC style bonuses, may also stack with compatible vanilla class features. That is intentional for build-enabler playstyles.

## Installation (Unity Mod Manager)

1. Install [Unity Mod Manager](https://www.nexusmods.com/site/mods/21) for Pathfinder: Wrath of the Righteous.
2. Build or download `ACHomebrew-0.1.2.zip`.
3. Drop the zip into UMM.
4. Enable the mod in-game.

## Save Compatibility

- Existing feat GUIDs are unchanged. Settings files without budget fields keep both limits off. Settings files without `WeaponDamage` default to `Replace`; both damage modes use the same new feat GUIDs, so switching modes requires no respec.
- **0.1.1 → 0.1.x is non-breaking.** Settings carry over; XML serialization adds the new `EnableMutex` field as `true` by default.
- **0.1.0 → 0.1.1 upgrades** keep all existing feats (GUIDs unchanged). New feats appear in the level-up feat list and Commanding Presence Stance now has its 30-ft ally aura wired; its Charisma scaling source still requires in-game verification.
- 0.0.x → 0.1.x is a redesign; back up saves first.

## Building from Source

- Set `WrathInstallDir`, `WrathPath`, or `WRATH_PATH`, or let the project generate the ignored, repository-root `GamePath.props` from `Player.log`.
- From the repository root, run `dotnet build ACHomebrew.slnx -p:DeployMod=false` to compile without deploying to the game.
- To build the release ZIP without deploying to the game, run `dotnet build ACHomebrew.slnx -c Release -p:DeployToGame=false`.
- Build output is in `artifacts/bin/ACHomebrew/<Configuration>/`; intermediate files are in `artifacts/obj/`.
- To deploy, run `dotnet build ACHomebrew.slnx`. The Deploy target copies files into the local UMM mod folder and creates `artifacts/packages/ACHomebrew-<Version>.zip`.
- Run the game-independent repository checks with `pwsh -NoProfile -File scripts/Test-RepositoryContracts.ps1`. These check blueprint IDs and loader metadata; they do not test combat effects.

- Run the standalone damage calculation checks with the .NET 10 SDK: `dotnet run --project tests/WeaponDamage.Tests`.
- Run initialization failure checks with `dotnet run --project tests/Initialization.Tests`. These exercise the real registry and menu orchestration with stand-ins for game/BlueprintCore APIs and family creation; they do not start Unity or verify in-game UI behavior.
- With Windows PowerShell 5.1, check settings compatibility using `powershell.exe -NoProfile -ExecutionPolicy Bypass -File tests/VerifySettings.ps1 -WrathInstallDir "<game directory>"`.
- After compiling, verify live mode switching on an existing component with `powershell.exe -NoProfile -ExecutionPolicy Bypass -File tests/VerifyLiveWeaponDamageMode.ps1 -WrathInstallDir "<game directory>"`. This loads the compiled mod and game types without starting Unity.

GitHub Actions runs the repository checks on Windows and Linux. GitHub-managed
CodeQL default setup scans C# and Actions for security issues; review its results
on the latest pull-request commit before merging. These static checks do not
replace in-game behavior tests.

### Repository layout

| Path | Contents |
|---|---|
| `src/ACHomebrew/` | Entry project: UMM loader, registration order, icons, `App.config`, loader `Info.json`, optional ModMenu page and packaging (merges every project and BlueprintCore into `AttributeFeats.dll`, the unchanged DLL name) |
| `src/ACHomebrew.Logic/` | Game-independent logic shared with the tests: budget costs, exclusion groups, scoring rules and blueprint GUIDs |
| `src/ACHomebrew.Core/` | In-game infrastructure: feat menus, budget and exclusion enforcement, settings, localization and shared feat builders |
| `src/ACHomebrew.Attributes/` | Main Attribute Mastery, Weapon Insight and Extended Replacement, Derived Stat Conversion, Weapon Damage, casting and resource attributes |
| `src/ACHomebrew.Aptitudes/` | Defensive, Maneuver, Skilled and Arcane aptitudes |
| `src/ACHomebrew.Martial/` | Stances, Conditional Triggers, distance feats, Long-Reach Gambit, Momentum and Execution |
| `src/ACHomebrew.Defense/` | Reactive Armor and Retaliation |
| `src/ACHomebrew.Magic/` | Spell School/Descriptor Specialists and Arcana (spell slots) |
| `src/ACHomebrew.Summoning/` | Greater Summoning, Summoner Sacrifice and Summoner links |
| `src/ACHomebrew.Styles/` | Stealth, Solo and Growth |
| `src/WrathMod.props` | Shared game references and `WrathInstallDir` resolution |
| `tests/` | Test projects and compatibility baselines |
| `scripts/`, `.github/` | Local commands and CI workflows |
| [doc/](doc/README.md) | Public documentation |
| `notes/` | Optional private notes submodule |
| `artifacts/` | Ignored build outputs, intermediate files, packages and test reports |

The root keeps `ACHomebrew.slnx`, shared build configuration, repository
configuration, README, CHANGELOG and `Repository.json`. The local `GamePath.props`
is shared by the Mod and test projects and must not be committed.

## Text and Icons

All 98 feats and the 18 selection menus have English and Simplified Chinese names and descriptions. The original 92 feats have individual 128×128 PNG icons; the six new Weapon Damage feats do not yet have bespoke artwork. Names and lore, along with the new menu and Weapon Damage descriptions, live in the embedded [FeatText.json](src/ACHomebrew.Core/Localization/FeatText.json). Other rule templates remain with their implementations.

See the [icon contact sheet](doc/feat-icons.png), [asset manifest](doc/icon-manifest.json), and [validation commands and limits](doc/validation.md). The settings UI remains in English. Source checks and builds do not establish actual combat effects or in-game text layout.

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
git -C notes commit -m "docs: update AC's Homebrew design notes"
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

Thanks to @CasDragon for code snippets and ideas. AC's Homebrew (formerly AttributeFeats) grew out of earlier Redditor class-feat experiments and was rebuilt for the 0.1.0 build-enabler release.
