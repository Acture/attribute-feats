# Changelog

## 0.1.1 — Mutex Relaxation + 17 New Feats

### Mutex Redesign
- **Cross-family same-attribute mutex removed.** Combinations like Apex Predator (Str Main) + Titan's Stance (Str Defensive) + Brutal Stance (Str Stance) now stack freely. The old release-time `MutexPass` cross-attribute pass is gone; only intra-family mutex remains.
- **New `EnableMutex` setting (default ON).** When OFF, every mutex prerequisite is skipped, including intra-family. Pick this for full collector / power-stack play; leave ON for the curated baseline.

### Added (17 new feats)
- **Conditional Trigger expansion (+2):** Berserker's Last Stand (Str, HP < 25%), Tactical Reading (Int, first attack of combat).
- **Extended Replacement expansion (+3):** Brutal Defender (Str → CMD), Lightfoot Defense (Dex → AC unarmored), Iron Endurance (Con → HP).
- **Spell Tag Descriptor expansion (+4):** Etheric Mind (Force / Int), Radiant Soul (Positive Energy / Cha), Hollow Heart (Negative Energy / Wis), Subtle Tyrant (Mind-Affecting / Cha). Each follows the same `+half mod` chosen-descriptor / `−half mod` other-descriptors tradeoff pattern.
- **Summoner Sacrifice (+3, Family 25):** Body of My Pact (1:1 transfer), Doubled Bond (1:2 transfer), Empowered Sacrifice (Cha → Str focused trade). Built on the same `SummonedUnitBuff` nested-buff pattern as Greater Summoning. Intra-family 3-way mutex.
- **Polearm Master (+1, Family 12):** Reach × 2 with a −4 weapon damage tradeoff. Classic spear/pike sweet spot, opt-in.
- **Distance Damage (+3, Family 24):** Aggressor's Edge (≤10 ft), Marksman's Focus (≥30 ft), Optimal Range (15–25 ft). Each grants +4 damage in its band via `AddContextStatBonus` + `AddInitiatorAttackWithWeaponTrigger` + `ContextConditionDistanceToTarget`.

### Fixed
- **Commanding Presence Stance** now applies its 30-ft Charisma-to-attack aura to allies. The 0.1.0 release shipped a self-only buff fallback because GUID exhaustion blocked the aura blueprint; the aura is now wired up via `AddAreaEffect` with a 30 ft cylinder filtered by `IsAlly()` and `IsCaster(negate: true)`.

### Changed
- README now lists `EnableMutex` in the settings table and updates the Stacking Rules section to reflect the cross-family mutex removal.
- Foundation GUIDs extended by 26 (nested classes for Conditional2, ExtendedReplacement2, SpellTagDescriptor2, PolearmMaster.Feature, DistanceDamage, SummonerSacrifice, plus the two Commanding Presence aura blueprints).

### Save Compatibility
- 0.1.0 saves load without change; 17 new feats appear in level-up lists.
- `EnableMutex` defaults to ON for upgrading installs (matches 0.1.0 mutex strictness minus the cross-family removal).

## 0.1.0 — Build Enabler Redesign

### Breaking Changes
- Mod settings reset on upgrade. Old `EnableBattles`, `EnableCaster`, and `progression` fields are gone; reconfigure once after installing 0.1.0.
- `Echo Spell` was removed. The duplicate-cast trigger relied on a non-functional spell-casting path and is no longer shipped.
- The old global progression cycler is replaced by `Power Level` (`Balanced` / `Legacy_AllFull`) plus granular feature toggles.
- Same-attribute mutex coverage is now enforced in the final release pass to prevent cross-family double-dipping.

### Added
- **76 feats total** across the 0.1.0 release families:
  - **Main Attribute Mastery (6):** Apex Predator, Embodied Grace, Living Bulwark, Architect of Self, Wellspring of Insight, Crown of Will.
  - **Specialized Adept (24):** Titan's Stance, Flowing Form, Iron Bulwark, Calculated Defense, Stoic Vigilance, Indomitable Presence, Crushing Grip, Deft Hand, Unyielding Hold, Tactical Bind, Predictive Lock, Domineering Throw, Practiced Hand, Effortless Skill, Tireless Practice, Polymath's Touch, Quiet Mastery, Inspired Versatility, Spell-Forged Will, Quickcast Reflex, Spell-Tempered Body, Scholar of the Weave, Oracle's Intuition, Sorcerous Presence.
  - **Stance (6):** Brutal Stance, Liquid Form, Endless Vigor, Tactical Mind, Centered Mind, Commanding Presence.
  - **Conditional Trigger (4):** Endless Resolve, First Blood, Vendetta, Patient Hunter.
  - **Replacement (9):** Crushing Form, Duelist's Eye, Iron Stance, Tactical Strike, Predictive Cut, Theatrical Combat, Inner Sentinel, Calculated Grip, Unyielding Will.
  - **Greater Summoning (6):** Bloodline of Beasts, Quickened Pact, Vital Pact, Tactical Binding, Insightful Summons, Magnetic Calling.
  - **Reactive Armor (2):** Spiked Defense, Bulwark of Steel.
  - **Derived Stat Conversion (6):** Arcane Aegis, Martial Insight, Skilled Defender, Mystic Vitality, Soul Bulwark, Sword Saint.
  - **Spell Tag Specialist (13):** Pure Warder, Master Caller, Seer's Edge, Heart's Tyrant, Spellforge, Veilweaver, Death Speaker, Shape-Shifter, Inner Flame, Frozen Heart, Storm Channel, Etching Mind, Resonant Voice.
- Granular redesign settings: `IncludeSelfInAttributeStack`, `EnableDefenses`, `EnableManeuvers`, `EnableChecks`, `EnableSkills`, `EnableCasterDC`, `EnableCasterLevel`, `EnableSpellPenetration`, `EnableBAB`, and `EnablePowerMode`.
- Release packaging now emits `AttributeFeats-0.1.0.zip` from the Deploy target during `dotnet build`.

### Changed
- Main feats now use flavor names while keeping their original GUIDs for save compatibility.
- Default behavior is retuned around the build-enabler philosophy: full scaling for attributes, defenses, maneuvers, skills, caster level, and spell penetration; reduced scaling for spell DC, BAB, and Power Mode bonuses while `Balanced` is selected.
- `Weapon Insight` remains a true attack-stat replacement path via `AddAttackStatReplacementFixed`; the unsafe untyped fallback path is gone.
- Self-stacking is now opt-in through `IncludeSelfInAttributeStack` instead of being assumed.

### Fixed
- Removed the duplicate `<Target Name="Deploy">` definition from `attribute feats.csproj`.
- Removed the old `BonusCasterLevel` double-scaling path from the Main / Specialized caster bonus flow.
- Added duplicate-rank registration protection in `Common.AddRank` to prevent repeated rank configs on the same feat.
- Locked down stable GUID handling with explicit save-compat comments and final duplicate GUID verification.
- Removed dead Echo Spell-era release baggage and aligned release packaging with the 0.1.0 artifact name.
