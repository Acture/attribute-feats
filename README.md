# AttributeFeats

A Pathfinder: Wrath of the Righteous mod that adds **build-enabling** feats based on character attributes. Choose a Main Attribute feat for broad cross-stat support, or mix focused families to build unconventional defenders, duelists, casters, and summoners.

> **Design philosophy:** AttributeFeats is a *build enabler*, not a power booster. Default 0.1.0 settings open new archetypes without inflating raw damage or attack counts. Extra power is opt-in through `Power Level` and `EnablePowerMode`.

## Feat Families

**0.1.0 total:** Main (6) + Specialized (24) + Stance (6) + Conditional (4) + Replacement (9) + Summon (6) + ReactiveArmor (2) + Derived (6) + SpellTag (13) = **76 feats**.

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

### Conditional Trigger (4 feats)

| Feat | Attribute |
|---|---|
| Endless Resolve | Constitution |
| First Blood | Dexterity |
| Vendetta | Charisma |
| Patient Hunter | Wisdom |

### Replacement (9 feats)

**Weapon Insight (6)**

| Feat | Attribute |
|---|---|
| Crushing Form | Strength |
| Duelist's Eye | Dexterity |
| Iron Stance | Constitution |
| Tactical Strike | Intelligence |
| Predictive Cut | Wisdom |
| Theatrical Combat | Charisma |

**Extended Replacement (3)**

| Feat | Attribute |
|---|---|
| Inner Sentinel | Wisdom |
| Calculated Grip | Intelligence |
| Unyielding Will | Charisma |

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

### Spell Tag Specialist (13 feats)
- **School (8):** Pure Warder, Master Caller, Seer's Edge, Heart's Tyrant, Spellforge, Veilweaver, Death Speaker, Shape-Shifter
- **Descriptor (5):** Inner Flame, Frozen Heart, Storm Channel, Etching Mind, Resonant Voice

## Settings

> Settings should be treated as restart-required after changes.

| Setting | Default | Effect |
|---|---|---|
| Power Level | `Balanced` | `Balanced`: full scaling for attributes, defenses, maneuvers, skills, caster level, and spell penetration; reduced scaling for spell DC, BAB, and Power Mode bonuses. `Legacy_AllFull`: all rank-based Main feat bonuses use full modifier scaling. |
| Include Self in Attribute Stack | OFF | A Main feat may add its chosen attribute to itself. |
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

- Each family keeps its own intra-family mutex rules (for example: Main 6-way, each Specialized subfamily 6-way, Stance 6-way, Weapon Insight 6-way, Greater Summoning 6-way, Spell Tag School 8-way, and Spell Tag Descriptor 5-way).
- `Main(X)` is mutually exclusive with same-attribute Specialized feats.
- The 0.1.0 release mutex pass also blocks same-attribute Stance, Weapon Insight, themed Conditional, Extended Replacement, and Spell Tag feats where that attribute identity would otherwise double-count.
- Different-attribute combinations stack normally (for example: `Apex Predator` + `Stoic Vigilance` + `Polymath's Touch` + `Theatrical Combat`).
- Main feats use **Inherent** bonuses; most non-Main bonuses are **Untyped** or use stat replacement, so cross-attribute combinations remain the intended way to build.
- Some effects, such as Wisdom-to-AC style bonuses, may also stack with compatible vanilla class features. That is intentional for build-enabler playstyles.

## Installation (Unity Mod Manager)

1. Install [Unity Mod Manager](https://www.nexusmods.com/site/mods/21) for Pathfinder: Wrath of the Righteous.
2. Build or download `AttributeFeats-0.1.0.zip`.
3. Drop the zip into UMM.
4. Enable the mod in-game.

## Save Compatibility

- **0.1.0 resets mod settings; reconfigure once after upgrade.**
- Existing saves should still load, but this is a large redesign from 0.0.x, so backing up saves first is recommended.

## Building from Source

- Set `WrathInstallDir`, `WrathPath`, or `WRATH_PATH`, or let the project generate `GamePath.props` from `Player.log`.
- Run `dotnet build "attribute feats.csproj"`.
- The Deploy target copies files into the local UMM mod folder and creates a release zip in `bin\`.

## Changelog

See [CHANGELOG.md](./CHANGELOG.md).

## Credits

Thanks to @CasDragon for code snippets and ideas. AttributeFeats grew out of earlier Redditor class-feat experiments and was rebuilt for the 0.1.0 build-enabler release.