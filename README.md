# AttributeFeats

A Pathfinder: Wrath of the Righteous mod that adds **build-enabling** feats based on character attributes. Choose a Main Attribute feat for broad cross-stat support, or mix focused families to build unconventional defenders, duelists, casters, and summoners.

> **Design philosophy:** AttributeFeats is a *build enabler*, not a power booster. Default 0.1.0 settings open new archetypes without inflating raw damage or attack counts. Extra power is opt-in through `Power Level` and `EnablePowerMode`.

## Feat Families

**0.1.1 total:** Main (6) + Specialized (24) + Stance (6) + Conditional (6) + Replacement (12) + Summon (6) + SummonerSacrifice (3) + ReactiveArmor (2) + Derived (6) + SpellTag (17) + PolearmMaster (1) + DistanceDamage (3) = **92 feats**.

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
| **Defensive** | Iron Bastion Posture<br>(撼山不动岳) | Dance of the Zephyr<br>(凌波游虚步) | Impenetrable Citadel<br>(渊渟铁瓮躯) | Prescient Deflection<br>(灵台逆照枢) | Unshaken Center<br>(止水澄明境) | Regal Aura<br>(皇图霸道罡) |
| **Maneuver** | Colossal Pincer<br>(崩岳裂碑抓) | Flickering Disarm<br>(穿林游丝手) | Anchor of Living Stone<br>(坚若磐陀锁) | Fulcrum Analysis<br>(洞虚枢机扣) | Way of the Reed<br>(随缘借力摔) | Commanding Subjugation<br>(慑心跪伏御) |
| **Skilled** | Herculean Craft<br>(裂地开山巧) | Thief of Shadows<br>(乘影掠光步) | Plowman's Fortitude<br>(磨勘九死魄) | Sage's Archive<br>(琅嬛洞观算) | All-Seeing Intuition<br>(观机神会相) | Silver-Tongued Arbiter<br>(绝缨折冲辩) |
| **Arcane** | Titan's Mana-Forge<br>(熔天燃髓力) | Flicker-Cast Cadence<br>(掣电弹指咒) | Blood-Wellspring Matrix<br>(鼎镬烹身脉) | Grand Arcanist's Lexicon<br>(万象演道谱) | Epiphany of the Leyline<br>(冥契通玄感) | Font of Primal Sorcery<br>(绝代天潢嗣) |

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
| Oath of Retribution | 复仇血誓 | Charisma | When hit |
| Crane's Severance | 蓄势孤峰 | Wisdom | Missed attack |
| Gorum's Last Stand | 狂神绝唱 | Strength | HP < 25% |
| Cadence Decoded | 阅破机宜 | Intelligence | First weapon attack resolves |

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
| Adamantine Mettle | 生机洪炉 | Constitution | Con-to-HP scaling |

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
- **Dawn of the Soul (法相初明)** — At combat start, gain temporary HP equal to Caster Level.
- **Blade of the Spell-Saint (剑圣咒痕)** — Half BAB to Spell DC.

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
- **Polearm Master (长柄武器宗师)** — reach × 2 with the −4 weapon damage tradeoff.

### Distance-Based Damage (3 feats)
- **Point-Blank Ruin (咫尺绝杀)** — +4 damage at ≤ 10 ft.
- **Horizon's Deadeye (苍穹神击)** — +4 damage at ≥ 30 ft.
- **Harmonic Cleave (流光截角)** — +4 damage at 15–25 ft.

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
- **Cross-family same-attribute mutex was removed in 0.1.1.** Combinations like `Titan's Apotheosis` (Str Main) + `Iron Bastion Posture` (Str Defensive) + `Berserker's Overrun` (Str Stance) are now allowed — same-attribute stacking is a deliberate build option, not a bug.
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

- Set `WrathInstallDir`, `WrathPath`, or `WRATH_PATH`, or let the project generate `GamePath.props` from `Player.log`.
- Run `dotnet build "attribute feats.csproj"`.
- The Deploy target copies files into the local UMM mod folder and creates a release zip in `bin\`.

## Changelog

See [CHANGELOG.md](./CHANGELOG.md).

## Credits

Thanks to @CasDragon for code snippets and ideas. AttributeFeats grew out of earlier Redditor class-feat experiments and was rebuilt for the 0.1.0 build-enabler release.