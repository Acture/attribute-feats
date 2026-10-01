# AttributeFeats

A Pathfinder: Wrath of the Righteous mod that adds **build-enabling** feats based on character attributes. Choose a Main Attribute feat for broad cross-stat support, or mix focused families to build unconventional defenders, duelists, casters, and summoners.

> **Design philosophy:** AttributeFeats is a *build enabler*, not a power booster. Main feat defaults focus on build-enabling stats. Standalone Weapon Damage feats let you choose attribute-based damage without taking a Main feat; their default Replace mode avoids adding a second attribute bonus.

## Feat Families

During character creation and level-up, expand **Attribute Feats**, then a **feat family**, then choose the **individual feat**. Each family has 2–9 choices; Polearm Master sits directly inside Attribute Feats because it has no variants. Each choice grants one feat, with no extra feat cost for either menu level. You can return to the same family at later feat choices, subject to the selected feat's usual prerequisites and mutual-exclusion rules. Existing characters keep their learned feats and effects.

The 17 menus are Main Attribute Mastery, Defensive Adept, Maneuver Adept, Skilled Adept, Arcane Adept, Stance, Conditional Trigger, Weapon Insight, Extended Replacement, Greater Summoning, Summoner Sacrifice, Reactive Armor, Derived Stat Conversion, Spell School Specialist, Spell Descriptor Specialist, Distance Damage, and Weapon Damage.

**Current total:** Main (6) + Specialized (24) + Stance (6) + Conditional (6) + Replacement (12) + Summon (6) + SummonerSacrifice (3) + ReactiveArmor (2) + Derived (6) + SpellTag (17) + PolearmMaster (1) + DistanceDamage (3) + WeaponDamage (6) = **98 feats** (92 in 0.1.1, plus 6 unreleased).

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

### Weapon Damage (6 feats)

Choose **Attribute Feats → Weapon Damage → an attribute → a proficient weapon category**. The six attributes are Strength, Dexterity, Constitution, Intelligence, Wisdom, and Charisma. Each choice costs one feat and changes only weapon damage for that category. You can choose the same attribute again for another category, but cannot take the exact same attribute/category twice.

The **Weapon Damage Mode** setting offers two behaviors. Switching takes effect on the next damage calculation, including already learned feats; no restart or respec is needed:

| Mode | Effect | Example: Str 12 (+1), Int 20 (+5), one-handed longsword |
|---|---|---|
| Replace (default) | Use the chosen modifier if it improves the existing attribute damage; retain the weapon's normal two-handed/off-hand multiplier. Existing Dexterity-to-damage is considered. | `1d8 + 5` |
| Add | Keep normal damage and add the chosen positive modifier once, without two-handed/off-hand scaling of the extra bonus. | `1d8 + 1 + 5` |

Both modes require an attack that already applies an attribute modifier to weapon damage. Ordinary crossbows and other attacks without a damage attribute receive no benefit. Neither mode changes attack rolls, spells, or other stats, and neither requires Power Mode. With `EnableMutex` on, this family allows one attribute; with it off, replacement uses the best applicable attribute while addition stacks the selected bonuses.

## Settings

> Weapon Damage Mode applies on the next damage calculation. Other settings require restarting.

| Setting | Default | Effect |
|---|---|---|
| Weapon Damage Mode (`WeaponDamage`) | `Replace` | Standalone Weapon Damage feats use a better chosen attribute instead of the existing damage attribute. `Add` adds the positive chosen modifier once. Independent of Main feat settings and Power Mode. |
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

- Each family enforces its own intra-family mutex (controlled by `EnableMutex`): Main 6-way, each Specialized subfamily 6-way, Stance 6-way, Weapon Insight 6-way, Weapon Damage 6-way, Greater Summoning 6-way, Summoner Sacrifice 3-way, Spell Tag School 8-way, Spell Tag Descriptor 9-way.
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

- Existing feat GUIDs are unchanged. Settings files without `WeaponDamage` default to `Replace`; both damage modes use the same new feat GUIDs, so switching modes requires no respec.
- **0.1.1 → 0.1.x is non-breaking.** Settings carry over; XML serialization adds the new `EnableMutex` field as `true` by default.
- **0.1.0 → 0.1.1 upgrades** keep all existing feats (GUIDs unchanged). New feats appear in the level-up feat list and Commanding Presence Stance now applies its 30-ft ally aura correctly.
- 0.0.x → 0.1.x is a redesign; back up saves first.

## Building from Source

- Set `WrathInstallDir`, `WrathPath`, or `WRATH_PATH`, or let the project generate `GamePath.props` from `Player.log`.
- Run `dotnet build "attribute feats.csproj"`.
- The Deploy target copies files into the local UMM mod folder and creates a release zip in `bin\`.
- To compile without deploying, run `dotnet msbuild "attribute feats.csproj" -restore -t:Compile -p:WrathInstallDir="<game directory>"`.
- Run the standalone damage calculation checks with the .NET 10 SDK: `dotnet run --project tests/WeaponDamage.Tests`.
- Run initialization failure checks with `dotnet run --project tests/Initialization.Tests`. These exercise the real registry and menu orchestration with stand-ins for game/BlueprintCore APIs and family creation; they do not start Unity or verify in-game UI behavior.
- With Windows PowerShell 5.1, check settings compatibility using `powershell.exe -NoProfile -ExecutionPolicy Bypass -File tests/VerifySettings.ps1 -WrathInstallDir "<game directory>"`.
- After compiling, verify live mode switching on an existing component with `powershell.exe -NoProfile -ExecutionPolicy Bypass -File tests/VerifyLiveWeaponDamageMode.ps1 -WrathInstallDir "<game directory>"`. This loads the compiled mod and game types without starting Unity.

## Changelog

See [CHANGELOG.md](./CHANGELOG.md).

## Credits

Thanks to @CasDragon for code snippets and ideas. AttributeFeats grew out of earlier Redditor class-feat experiments and was rebuilt for the 0.1.0 build-enabler release.
