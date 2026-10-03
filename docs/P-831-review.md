# P-831 / OSS-77 final text and icon audit

Scope: 92 existing feats and their visible buffs/toggles. Baseline published text: `d4de92a` (0.1.1); inherited AGY text: `6887a8a`. Internal names, GUIDs, component values and triggers are preserved. The final names and lore are in `Localization/FeatText.json`; rules remain separate in the family builders.

## Representative changes

| Mechanism | Before | Final description |
|---|---|---|
| Main scaling | Full modifier implied for every bonus | Balanced DC/BAB/power uses half, rounded down; Legacy_AllFull uses full; minimum 0 |
| Conditional | README said being hit / missing | Ally death within 30 meters / first weapon attack even on a miss |
| Intelligence stance | Ordinary attacks claimed to take an INT penalty | Current inverse rank is clamped at 0, producing no penalty for positive INT; this existing mechanism issue is explicitly exposed |
| Distance | 30+ / 15–25 feet | Actual continuous thresholds: >29 / >14 and ≤25 feet |
| Reach | Polearm Master implies weapon restriction | Long-Reach Gambit: any weapon category, doubled reach and −4 damage |
| Summoning | Grandiose transformation or official divine approval implied | Short original pact imagery, with no size change or religious prerequisite implied |

## Series and terminology

All 92 English titles and all 92 Chinese titles are unique. Repeated names on a feat, its toggle, and its buff intentionally identify one ability. CMB/CMD use 战技加值/战技防御; weapon attack substitution is distinguished from the additive Extended family. Mutex restrictions depend on EnableMutex. Negative modifiers are clamped where the source rank config does so; the Intelligence stance inverse rank is documented separately.

Golarion references are inspiration, not requirements or new official canon. Gorum does not teach abandoning armor: [Archives of Nethys](https://aonprd.com/DeityDisplay.aspx?ItemName=Gorum). Irori is associated with self-perfection: [Archives of Nethys](https://aonprd.com/DeityDisplay.aspx?ItemName=Irori). Aldori inspiration is grounded in [Aldori Defender](https://aonprd.com/ArchetypeDisplay.aspx?FixedName=Fighter%20Aldori%20Defender). Boneyard imagery does not assert Pharasma endorses necromancy. Hercules attribution was removed. All other descriptive practices are original flavor.

## Validation boundary and mechanism follow-ups

OSS-59 (formerly P-812) is still Backlog, with no completed real-game effect report. This work verifies source correspondence, text resources, fallback behavior, blueprint identity, assets, compilation and packaging. Locale switching, visual font/layout behavior and actual combat effects still require the real game. OSS-142 (formerly P-832) owns settings UI and remaining template resource migration.

Existing mechanism concerns preserved for separate verification: reactive armor UnitArmor conditions omit a Unit evaluator (installed BPC/game metadata indicate a null evaluator makes the condition false); Intelligence stance inverse scaling is clamped after multiplication; stance toggles lack an activation group when learning mutex is disabled; Charisma ally aura scaling is attached to the ally and needs caster-source verification. Text does not claim these have passed runtime tests.

Reactive armor condition investigation is tracked as [OSS-289](https://linear.app/acturea/issue/OSS-289/attributefeats验证并修复护甲反应专长的-unitarmor-条件上下文), a child of OSS-77.

Descriptor specialists match specific groups and apply one penalty per other matching group; multi-tag spells and abilities can combine bonuses and penalties. Positive matching group is Cure/RestoreHP/ChannelPositiveHeal/ChannelPositiveHarm; negative matching group is ChannelNegativeHeal/ChannelNegativeHarm/NegativeLevel. School and descriptor DC/caster-level changes are settings gated.

## Complete coverage and name comparison

| Family | Stable internal name | Published English name | Final EN / zhCN |
|---|---|---|---|
| Main | `ApexPredator` | Apex Predator | Titan's Apotheosis / 泰坦登阶 |
| Main | `EmbodiedGrace` | Embodied Grace | Quicksilver Incarnate / 水银具现 |
| Main | `LivingBulwark` | Living Bulwark | Adamantine Vessel / 万劫金身 |
| Main | `ArchitectOfSelf` | Architect of Self | Architect of Self / 灵枢架构师 |
| Main | `WellspringOfInsight` | Wellspring of Insight | Ocular of the Cosmos / 寰宇天心 |
| Main | `CrownOfWill` | Crown of Will | Sovereign of Wills / 至高皇威 |
| Weapon Insight | `CrushingForm` | Crushing Form | Titan's Momentum / 巨灵摧破 |
| Weapon Insight | `DuelistsEye` | Duelist's Eye | Aldori Finesse / 阿尔多里绝诣 |
| Weapon Insight | `IronStance` | Iron Stance | Stout Grounding / 磐固千钧 |
| Weapon Insight | `TacticalStrike` | Tactical Strike | Geometer's Edge / 规矩之锋 |
| Weapon Insight | `PredictiveCut` | Predictive Cut | Karmic Interception / 因果断隙 |
| Weapon Insight | `TheatricalCombat` | Theatrical Combat | Swashbuckler's Flourish / 游侠华彩 |
| Extended | `InnerSentinel` | Inner Sentinel | Ascetic's Ward / 云水自真 |
| Extended | `CalculatedGrip` | Calculated Grip | Anatomical Leverage / 筋络推演 |
| Extended | `UnyieldingWill` | Unyielding Will | Monarch's Stature / 帝胄岳立 |
| Extended | `BrutalDefender` | Brutal Defender | Titan's Footing / 巨灵固步 |
| Extended | `LightfootDefense` | Lightfoot Defense | Zephyr's Grace / 穿风灵步 |
| Extended | `IronEndurance` | Iron Endurance | Adamantine Mettle / 生机洪炉 |
| Defensive | `TitansStance` | Titan's Stance | Colossus Bastion / 巨灵重障 |
| Defensive | `FlowingForm` | Flowing Form | Wind-Dancer's Shroud / 风舞虚影 |
| Defensive | `IronBulwark` | Iron Bulwark | Inured Carapace / 百炼金身 |
| Defensive | `CalculatedDefense` | Calculated Defense | Analytical Aegis / 算律之盾 |
| Defensive | `StoicVigilance` | Stoic Vigilance | Third Eye Vigil / 天目清照 |
| Defensive | `IndomitablePresence` | Indomitable Presence | Majesty's Reproach / 凛然天威 |
| Maneuver | `CrushingGrip` | Crushing Grip | Grip of the Behemoth / 比蒙扼击 |
| Maneuver | `DeftHand` | Deft Hand | Fulcrum of the Viper / 灵蛇巧掣 |
| Maneuver | `UnyieldingHold` | Unyielding Hold | Deep-Root Clinch / 沉洋扼锁 |
| Maneuver | `TacticalBind` | Tactical Bind | Anatomical Pivot / 机理断节 |
| Maneuver | `PredictiveLock` | Predictive Lock | Crane's Anticipation / 玄鹤听劲 |
| Maneuver | `DomineeringThrow` | Domineering Throw | Audacious Overthrow / 叱喝倾山 |
| Skilled | `PracticedHand` | Practiced Hand | Giantwright's Craft / 巨匠巧工 |
| Skilled | `EffortlessSkill` | Effortless Skill | Thief-King's Panache / 妙手绝尘 |
| Skilled | `TirelessPractice` | Tireless Practice | Ascetic Diligence / 苦行研磨 |
| Skilled | `PolymathsTouch` | Polymath's Touch | Encyclopedic Synthesis / 格物万象 |
| Skilled | `QuietMastery` | Quiet Mastery | Wanderer's Lucidity / 云水澄明 |
| Skilled | `InspiredVersatility` | Inspired Versatility | Silver-Tongued Virtuoso / 锦绣天潢 |
| Arcane | `SpellForgedWill` | Spell-Forged Will | Mage-Hammer Inscription / 铁骨铸咒 |
| Arcane | `QuickcastReflex` | Quickcast Reflex | Somatic Velocity / 疾影手印 |
| Arcane | `SpellTemperedBody` | Spell-Tempered Body | Crucible of the Conduit / 鼎炉承法 |
| Arcane | `ScholarOfTheWeave` | Scholar of the Weave | Archmage's Codex / 万法源流 |
| Arcane | `OraclesIntuition` | Oracle's Intuition | Gnostic Channel / 玄鉴通幽 |
| Arcane | `SorcerousPresence` | Sorcerous Presence | Sovereign Decrees / 天宪法旨 |
| Stance | `BrutalStance` | Brutal Stance | Berserker's Overrun / 破阵裂山势 |
| Stance | `LiquidForm` | Liquid Form | Willow in the Gale / 惊鸿穿林势 |
| Stance | `EndlessVigor` | Endless Vigor | Mountain's Deep Roots / 不动磐峰势 |
| Stance | `TacticalMind` | Tactical Mind | Grandmaster's Gambit / 弈者静待势 |
| Stance | `CenteredMind` | Centered Mind | Mirror of Still Waters / 明镜止水势 |
| Stance | `CommandingPresence` | Commanding Presence | Vanguard's Banner / 金戈铁旌势 |
| Conditional | `EndlessResolve` | Endless Resolve | Defiance at the Precipice / 绝境砥柱 |
| Conditional | `FirstBlood` | First Blood | Ambush of the Viper / 封喉首刃 |
| Conditional | `Vendetta` | Vendetta | Oath of Retribution / 复仇血誓 |
| Conditional | `PatientHunter` | Patient Hunter | Crane's Severance / 蓄势孤峰 |
| Conditional | `BerserkersLastStand` | Berserker's Last Stand | Gorum's Last Stand / 狂神绝唱 |
| Conditional | `TacticalReading` | Tactical Reading | Cadence Decoded / 阅破机宜 |
| Derived | `ArcaneAegis` | Arcane Aegis | Arcane Aegis / 魔能天衣 |
| Derived | `MartialInsight` | Martial Insight | War-Hardened Reflexes / 百战身魄 |
| Derived | `SkilledDefender` | Skilled Defender | Scholar's Positioning / 通识御敌 |
| Derived | `MysticVitality` | Mystic Vitality | Ley-Infused Vitality / 灵脉淬体 |
| Derived | `SoulBulwark` | Soul Bulwark | Dawn of the Soul / 法相初明 |
| Derived | `SwordSaint` | Sword Saint | Blade of the Spell-Saint / 剑圣咒痕 |
| Summon | `BloodlineOfBeasts` | Bloodline of Beasts | Behemoth's Heritage / 比蒙遗脉 |
| Summon | `QuickenedPact` | Quickened Pact | Zephyr's Covenant / 风灵疾契 |
| Summon | `VitalPact` | Vital Pact | Titan's Lifespring / 巨怪生机 |
| Summon | `TacticalBinding` | Tactical Binding | Aegis of the Schema / 天元魔阵 |
| Summon | `InsightfulSummons` | Insightful Summons | Empathic Communion / 神契灵犀 |
| Summon | `MagneticCalling` | Magnetic Calling | Dominator's Calling / 御统王令 |
| Sacrifice | `BodyOfMyPact` | Body of My Pact | Martyr's Transference / 形神替生 |
| Sacrifice | `DoubledBond` | Doubled Bond | Eldritch Crucible / 双生法炼 |
| Sacrifice | `EmpoweredSacrifice` | Empowered Sacrifice | Tribute of Iron Dominion / 夺冕化蛮 |
| Distance | `AggressorsEdge` | Aggressor's Edge | Point-Blank Ruin / 咫尺绝杀 |
| Distance | `MarksmansFocus` | Marksman's Focus | Horizon's Deadeye / 苍穹神击 |
| Distance | `OptimalRange` | Optimal Range | Harmonic Cleave / 流光截角 |
| School | `PureWarder` | Pure Warder | Aegis of the Pure Warder / 绝界镇魔使 |
| School | `MasterCaller` | Master Caller | Sovereign Gatekeeper / 统界辟门者 |
| School | `SeersEdge` | Seer's Edge | Eye of the Chronomancer / 溯时先知 |
| School | `HeartsTyrant` | Heart's Tyrant | Sovereign of the Heart / 倾心国主 |
| School | `Spellforge` | Spellforge | Pyre of the Architect / 灾变筑城师 |
| School | `Veilweaver` | Veilweaver | Phantasmagoria Maestro / 织影幻圣 |
| School | `DeathSpeaker` | Death Speaker | Harvester of the Boneyard / 冥河渡魂人 |
| School | `ShapeShifter` | Shape-Shifter | Sculptor of Prime Matter / 塑质造化使 |
| Descriptor | `InnerFlame` | Inner Flame | Pyre of the Phoenix / 炽皇凤涅 |
| Descriptor | `FrozenHeart` | Frozen Heart | Stillness of the Glacial Void / 极渊玄冰 |
| Descriptor | `StormChannel` | Storm Channel | Tempest-Dancer / 御雷疾影 |
| Descriptor | `EtchingMind` | Etching Mind | Vitriolic Equation / 腐解算律 |
| Descriptor | `ResonantVoice` | Resonant Voice | Herald of the Shattered Sky / 破霄神音 |
| Descriptor | `EthericMind` | Etheric Mind | Axiom of Unseen Force / 虚空定则 |
| Descriptor | `RadiantSoul` | Radiant Soul | Fountain of Solar Dawn / 金阳圣晖 |
| Descriptor | `HollowHeart` | Hollow Heart | Vigil of the Gloom / 死寂枯荣 |
| Descriptor | `SubtleTyrant` | Subtle Tyrant | Puppeteer of the Mind / 惑魂主宰 |
| Reactive Armor | `SpikedDefense` | Spiked Defense | Barbed Carapace / 逆鳞铁刺 |
| Reactive Armor | `BulwarkOfSteel` | Bulwark of Steel | Citadel of Steel / 铸铁城阙 |
| Reach | `PolearmMaster` | Polearm Master | Long-Reach Gambit / 长锋险势 |

## Associated visible objects

There are 35 buffs and 6 stance toggles. They share the parent feat name and artwork. No ordinary Ability blueprint is created. The 92 feats plus these 41 visible objects, a Charisma aura area, and a skill-rank property total 135 blueprint identities.

| Parent internal name | Visible associated identities |
|---|---|
| `BrutalStance` | `BrutalStanceBuff`, `BrutalStanceActivatable` |
| `LiquidForm` | `LiquidFormBuff`, `LiquidFormActivatable` |
| `EndlessVigor` | `EndlessVigorBuff`, `EndlessVigorActivatable` |
| `TacticalMind` | `TacticalMindBuff`, `TacticalMindActivatable` |
| `CenteredMind` | `CenteredMindBuff`, `CenteredMindActivatable` |
| `CommandingPresence` | `CommandingPresenceBuff`, `CommandingPresenceActivatable`, `CommandingPresenceAllyBuff` |
| `EndlessResolve` | `EndlessResolveTriggerBuff` |
| `FirstBlood` | `FirstBloodTriggerBuff` |
| `Vendetta` | `VendettaTriggerBuff` |
| `PatientHunter` | `PatientHunterTriggerBuff` |
| `BerserkersLastStand` | `BerserkersLastStandTriggerBuff` |
| `TacticalReading` | `TacticalReadingTriggerBuff` |
| `SoulBulwark` | `SoulBulwarkTriggerBuff`, `SoulBulwarkTempBuff` |
| `BloodlineOfBeasts` | `BloodlineOfBeastsOuterBuff`, `BloodlineOfBeastsInnerBuff` |
| `QuickenedPact` | `QuickenedPactOuterBuff`, `QuickenedPactInnerBuff` |
| `VitalPact` | `VitalPactOuterBuff`, `VitalPactInnerBuff` |
| `TacticalBinding` | `TacticalBindingOuterBuff`, `TacticalBindingInnerBuff` |
| `InsightfulSummons` | `InsightfulSummonsOuterBuff`, `InsightfulSummonsInnerBuff` |
| `MagneticCalling` | `MagneticCallingOuterBuff`, `MagneticCallingInnerBuff` |
| `BodyOfMyPact` | `BodyOfMyPactOuterBuff`, `BodyOfMyPactInnerBuff` |
| `DoubledBond` | `DoubledBondOuterBuff`, `DoubledBondInnerBuff` |
| `EmpoweredSacrifice` | `EmpoweredSacrificeOuterBuff`, `EmpoweredSacrificeInnerBuff` |
| `BulwarkOfSteel` | `BulwarkOfSteelBuff` |
| All three Distance feats | `DistanceDamageFlatBonusBuff` (Distance Damage / 距离伤害; shares AggressorsEdge artwork) |

Internal helpers: `CommandingPresenceArea`, `SkilledDefenderSkillRanksProperty`. Other feat families have no additional visible buff or toggle blueprints.


## Lore before/after (inherited AGY draft → final resources)

Every row includes the previously inherited English/Chinese lore and the final shortened equivalent; these are audit evidence, not alternative player text.

### Titan's Apotheosis / 泰坦登阶

**Before EN:** Primal Gravity. To you, strength is not merely physical mass, but the foundational gravity by which reality holds together. Like the primordial titans who reshaped Golarion before the gods raised walls against the dark, every contest of power bows to your crushing sovereignty.

**Before ZH:** 原初重力。 力之至极，并非筋肉之凡力，而是支撑现世运转的原初重力。正如神明筑墙封绝暗渊之前、挥手重塑山河的亘古泰坦，万象诸界的一切权柄皆在汝之霸力前俯首称臣。

**Final EN:** You train as though every task were a stone to be lifted, making strength the foundation of your whole bearing.

**Final ZH:** 你将每一次试炼都视作待举之石，以力量奠定身心的根基。

### Quicksilver Incarnate / 水银具现

**Before EN:** Unanchored Motion. You move with the frictionless purity of living quicksilver, slipping through the grasp of fate itself. Motion and stillness become choices you author, leaving the world to desperately strike at where you were a heartbeat ago.

**Before ZH:** 不羁灵动。 身形宛若具现之水银，穿行于因果与杀伐的缝隙之间。动静起落皆随心生意动，任凭天地间刀光如幕，所及者唯有汝已逝去一刹之残影。

**Final EN:** Balance, breath, and movement flow together like quicksilver, carrying your discipline beyond the blade.

**Final ZH:** 步法、呼吸与重心如水银般连贯，将敏捷的修习带到兵刃之外。

### Adamantine Vessel / 万劫金身

**Before EN:** Crucible of the Mountain. Your body is forged in the deep veins of the earth where adamantine forms under the crushing weight of continents. What mortals call agony or poison washes over you like rain upon granite, feeding a vitality that cannot be extinguished.

**Before ZH:** 崇山熔炉。 此躯如在大陆重压下的地脉深处百炼而成的精金。凡人所谓之裂体剧痛与穿肠剧毒，落于汝身不过如山雨淋漓，反化作深不见底、万劫不灭之浩荡生机。

**Final EN:** Years of hardship have taught your body to meet every ordeal with the patience of tempered metal.

**Final ZH:** 多年的磨砺让你的身躯学会以百炼金属般的沉稳迎接试炼。

### Architect of Self / 灵枢架构师

**Before EN:** Anatomy of the Theorem. In the grand tradition of Nexian arcane schematics, the mortal form is merely a rough theorem awaiting proof and optimization. Through rigorous calculation and sacred geometry, you re-inscribe your own physical laws to transcend mortal boundaries.

**Before ZH:** 定理形构。 秉承内克斯奥术学派之玄思，凡胎肉身不过是待以理性规正之粗糙算式。借由严密数学与神圣几何之构析，汝重编身骨律法，令智慧之辉凌驾于血肉樊笼之上。

**Final EN:** You study your own habits like an arcane diagram, turning careful understanding into practiced control.

**Final ZH:** 你如研读奥术图谱般审视自身习惯，以理解换来娴熟的控制。

### Ocular of the Cosmos / 寰宇天心

**Before EN:** Still Waters of the Void. Treading the contemplative discipline of Irori, you settle your consciousness into utter stillness. When the turbulence of ego subsides, the tides of causality and the whispers of the Great Beyond become as legible as ripples upon calm water.

**Before ZH:** 灵渊止水。 步入兼爱明心之悟境，神思归于深渊古井般的终极寂静。当私欲波澜尽平，诸界因果的流转脉络与幽微隐兆皆洞若观火，如照平湖清漪，无所遁形。

**Final EN:** Following the ideal of self-perfection associated with Irori, you make quiet attention the center of your practice.

**Final ZH:** 你借鉴伊洛里的自我完善之道，以宁静而专注的觉察统摄修习。

### Sovereign of Wills / 至高皇威

**Before EN:** The Royal Decree. Reality bends not to physical lever or silent prayer, but to the audacity of imperial conviction. When you speak, the surrounding tapestry of fate yields to your proclamation, recognizing a soul whose sheer presence refuses to be denied.

**Before ZH:** 王庭御令。 现世所屈从者，非蛮力之杠杆，亦非幽微之祷祝，乃傲岸不屈之皇道王权。言出法随，周遭命运之织锦皆遵汝之意志而易向，唯此真王不容置疑。

**Final EN:** Your confidence gathers scattered effort into a single purpose, as a sovereign gathers a wavering court.

**Final ZH:** 你的自信将纷乱的行动凝成一个目标，宛如君主整肃摇摆的王庭。

### Titan's Momentum / 巨灵摧破

**Before EN:** <i>Force Finds the Gap.</i> You do not seek delicate angles when sheer mass can shatter them. Momentum and crushing weight turn every swing into an avalanche that drives through shields and armor alike.

**Before ZH:** <i>势破万钧。</i>不假精微机巧，唯仗移山之伟力。动量与蛮劲倾泻如崩山裂石，哪怕坚甲重盾亦在其霸道轰击下荡然无存。

**Final EN:** Rather than chase delicate openings, you commit your weight to a weapon's path and trust its momentum.

**Final ZH:** 你不追逐细小的空隙，而将身量压入兵刃的轨迹，借其惯势出手。

### Aldori Finesse / 阿尔多里绝诣

**Before EN:** <i>The Flow of the Blade.</i> Rooted in the legendary dueling disciplines of Restov, your weapon dances on the razor edge of balance. Grace and split-second precision exploit the faintest opening before an enemy can react.

**Before ZH:** <i>剑随游丝。</i>源自雷斯托夫剑爵的不传秘技。刃尖悬于分毫之隙，以绝伦灵敏与无瑕准度切入破绽，敌未觉察而已受剑创。

**Final EN:** Inspired by Aldori dueling traditions, you guide the blade with balance and exacting footwork.

**Final ZH:** 你借鉴阿尔多里的决斗传统，以重心与精确步法驾驭剑锋。

### Stout Grounding / 磐固千钧

**Before EN:** <i>Anchor of Iron.</i> Like an ancient megalith facing tempest winds, you anchor every swing into the ground beneath your feet. Unwavering stamina and sheer physical density guide your weapon's trajectory true.

**Before ZH:** <i>立地生根。</i>如千古顽石迎击暴风狂澜。将周身耐力与磐固身躯与大地相联，以雄浑底力贯通兵刃，招式沉稳如岳、无懈可击。

**Final EN:** A steady breath and a planted stance keep your weapon true even when fatigue begins to bite.

**Final ZH:** 即使疲惫袭来，沉稳的呼吸与扎实的站姿仍让兵刃沿着预定轨迹前行。

### Geometer's Edge / 规矩之锋

**Before EN:** <i>The Measured Vector.</i> Trajectories, fulcrums, and velocities resolve into crisp geometrical lines before your mind's eye. Every cut is calculated with mathematical certainty, landing with devastating economy.

**Before ZH:** <i>规矩方圆。</i>在敏锐心智中，弹道、支点与轨迹化作严整的几何图谱。每一次挥击皆经精微筹算，以最小能耗切中命门死线。

**Final EN:** You see the duel as intersecting lines, choosing each stroke by angle rather than impulse.

**Final ZH:** 你将交锋视作交错的线条，依角度而非冲动选择每一次出手。

### Karmic Interception / 因果断隙

**Before EN:** <i>Severing the Intention.</i> Before an enemy's muscles can twitch to unleash an attack, your serene awareness has already perceived its karmic trajectory. You strike not where they are, but where their fate arrives.

**Before ZH:** <i>见微知著。</i>敌机方萌，心识已照。不待其筋肉发劲，超然直觉已先一步断定因果去向，截击于必经之隙。

**Final EN:** You watch the intention behind a motion and meet your opponent where their next step will lead.

**Final ZH:** 你察看动作背后的意图，在对手下一步将至之处迎击。

### Swashbuckler's Flourish / 游侠华彩

**Before EN:** <i>Dazzling Bravura.</i> Combat is a grand stage, and your audacious spectacle commands every eye. With hypnotic feints and sheer theatrical swagger, your weapon finds openings created by your irresistible presence.

**Before ZH:** <i>纵横惊鸿。</i>沙场如氍毹，气场压万夫。以华丽炫目的假动作与傲世豪情摄人心魄，使敌心神失据，兵刃顺势夺命。

**Final EN:** A flourish invites the eye to follow one story while your weapon pursues another.

**Final ZH:** 华丽的虚招引导敌人的目光，兵刃则沿另一条轨迹逼近。

### Ascetic's Ward / 云水自真

**Before EN:** <i>Still at the Center.</i> In the tradition of ascetic hermits and monastery masters, true defense is not found in iron plates, but in absolute stillness of mind. The danger you perceive before it manifests is the danger that fails to touch you.

**Before ZH:** <i>身如止水。</i>承袭苦行宗师与山岳隐士的心法，真正的守御不在顽铁重甲，而在于明澈澄空的心意。见机于微者，刃锋莫能侵其分毫。

**Final EN:** Lightly burdened, you guard yourself through a traveler's quiet awareness and measured movement.

**Final ZH:** 轻装行走时，你凭旅人的沉静觉察与适度挪移守护自身。

### Anatomical Leverage / 筋络推演

**Before EN:** <i>Leverage by Design.</i> Grappling, tripping, and disarming are mere physics applied to living anatomy. By calculating fulcrums, joint limits, and weight distribution, you topple giants with calculated efficiency.

**Before ZH:** <i>骨骼力学。</i>擒拿、摔跌与卸武，无非是作用于血肉机巧的力学解析。洞悉敌之关节支点与重心位移，便能以精微杠杆之力掀翻巍峨巨怪。

**Final EN:** Anatomy turns a grapple into a study of joints, leverage, and the direction of resistance.

**Final ZH:** 关节、杠杆与抵抗的方向，使每一次擒抱都成为对身体结构的实践。

### Monarch's Stature / 帝胄岳立

**Before EN:** <i>The Emperor's Gravity.</i> Sovereign presence radiates outward, imposing your will upon reality. Opponents who attempt to sweep, grapple, or displace you find themselves rebuffed by an insurmountable aura of authority.

**Before ZH:** <i>帝胄皇威。</i>至尊霸气油然而生，将无形威严化为立足乾坤的渊渟岳峙。妄图摔跌、纠缠或击退你的敌手，皆在其无上威压前铩羽受制。

**Final EN:** You hold your ground with the bearing of someone who expects to be obeyed.

**Final ZH:** 你以不容轻忽的威仪站稳阵地，令自己的坚持清晰可见。

### Titan's Footing / 巨灵固步

**Before EN:** <i>Rooted Mountain.</i> Like a rooted ironwood tree or a colossal titan, your colossal weight and sinew anchor directly into the stone. Foes attempting to leverage you off your feet succeed only in breaking their own leverage.

**Before ZH:** <i>生根盘石。</i>如古老铁木深扎重岩，如巨灵大君拔地参天。无可撼动的千钧骨肉锁固地脉，任何撼动你平衡的妄图，终将自行折断其杠杆。

**Final EN:** Strong legs and a deliberate stance let you answer force without surrendering your footing.

**Final ZH:** 强健的双腿与审慎的站姿，让你在抵抗外力时守住立足之处。

### Zephyr's Grace / 穿风灵步

**Before EN:** <i>Untouched in Motion.</i> Flowing like the wind across a blade's edge, your evasive footwork never remains in the space a weapon falls. Steel and claws taste only empty air left in your wake.

**Before ZH:** <i>踏风无痕。</i>身如长风绕刃而转，步似流云游走无定。任何落向身侧的利刃与利爪，撕裂的不过是你留在原地的残影微尘。

**Final EN:** With little armor to hamper you, every small shift of weight becomes part of your guard.

**Final ZH:** 少了重甲的牵制，每一次细微的重心转移都成为防守的一部分。

### Adamantine Mettle / 生机洪炉

**Before EN:** <i>Crucible of Vitality.</i> Your heart beats like a great forge bellows, filling veins with primeval resilience. Wounds that would fell lesser warriors merely stoke the inextinguishable furnace of your physical resolve.

**Before ZH:** <i>气血烘炉。</i>强韧的心脏如熔炉风箱轰鸣运转，将磅礴血气倾注周身筋脉。足以致寻常武者横死的重创，反更淬炼激荡出你体内源源不绝的不灭生机。

**Final EN:** You have learned to carry hardship in your flesh as a forge carries the day's heat.

**Final ZH:** 你学会如熔炉蓄热般承受艰辛，让磨砺沉淀于血肉之中。

### Colossus Bastion / 巨灵重障

**Before EN:** <i>Kinetic Displacement.</i> Your massive muscular bulk and grounded posture create a localized barrier of kinetic displacement. Strikes that would cleave a normal warrior shudder off your braced sinews like hail against an ironclad fortress gate.

**Before ZH:** <i>力场偏折。</i>以万钧身魄与深厚桩步筑成偏折动量之重障。足以劈山断岳之凶险斩击，落于千锤百炼之筋肉间，皆若飞雪叩击精钢要塞之门。

**Final EN:** You brace like a fortress buttress, letting practiced strength support your whole defense.

**Final ZH:** 你如要塞的扶壁般支撑阵线，让娴熟的力量托住整个防御。

### Wind-Dancer's Shroud / 风舞虚影

**Before EN:** <i>Aldori Cadence.</i> Moving with the sublime cadence of an Aldori swordlord, you do not block steel—you vacate the very space it aims to claim. The air itself seems to twist around your wake, blinding enemy aim.

**Before ZH:** <i>阿尔多里韵律。</i>承袭阿尔多里剑圣流派之神髓。从不与顽铁硬碰，唯自锋芒所向之方寸虚空中飘然而逝，旋身引风，教敌兵每每刺空。

**Final EN:** You borrow the rhythm of a swaying reed, keeping your guard in motion rather than fixing it in place.

**Final ZH:** 你借芦苇摇曳的韵律不断调整守势，不将防守锁在一处。

### Inured Carapace / 百炼金身

**Before EN:** <i>Living Scar-Tissue.</i> What does not kill you crystallizes within your marrow into adamantine density. Blades deflect from ossified muscle, and toxins break against blood that has conquered every known plague of Golarion.

**Before ZH:** <i>金石道体。</i>历尽千创百孔，骨骼肌理早已淬炼为晶化精金。刀劈钝折，诸毒退散，体内流淌之血脉曾踏平葛拉利昂之万般灾厄，百劫不坏。

**Final EN:** Old scars remind you how to endure a blow and keep your composure under strain.

**Final ZH:** 旧伤教会你承受冲击，也教会你在压力下保持镇定。

### Analytical Aegis / 算律之盾

**Before EN:** <i>Trajectory Matrix.</i> To an analytical mind, an attack is simply a parabolic vector with a predictable apex. By calculating velocity, weapon balance, and reach before a blow connects, you step outside the arc where the blade cannot follow.

**Before ZH:** <i>弹道矩阵。</i>于大智者眼中，战阵击杀无非是带有既定抛物线之运动矢量。推演其势能、重心与盲区，先敌一步立于刃锋所不能及之算理死角。

**Final EN:** You read the angle of an approaching strike and place your guard where it will matter.

**Final ZH:** 你读出来袭兵刃的角度，将防守放在真正需要的位置。

### Third Eye Vigil / 天目清照

**Before EN:** <i>Preternatural Awareness.</i> Long meditation upon the ethereal currents has awakened an instinctive awareness that transcends eyesight. Malice and ambush are felt as distinct ripples in the air, giving you time to answer danger before it strikes.

**Before ZH:** <i>超然感通。</i>长年静坐参禅，神念早已透视以太微光。杀意未现，虚空已荡漾微澜；未卜先知，无论暗袭毒计抑或幻蛊邪法，皆在天目清照下无所遁形。

**Final EN:** Patient attention catches small changes in a foe's bearing before they become a committed attack.

**Final ZH:** 耐心的观察捕捉敌人姿态的细微变化，先于其决意出手作出准备。

### Majesty's Reproach / 凛然天威

**Before EN:** <i>Sovereign Stature.</i> Your bearing radiates such haughty, indomitable nobility that striking you feels like an act of blasphemy. Foes waver as self-doubt and primal awe sap the lethal purpose from their swings.

**Before ZH:** <i>帝胄皇威。</i>顾盼间自生帝胄君临之威仪，令拔刃相向者如犯亵渎重罪。强敌心旌神摇，傲气杀意尽为这股凌厉天威所摄，兵刃犹疑难发。

**Final EN:** Your unyielding bearing turns defense into a contest of resolve as much as steel.

**Final ZH:** 你的坚定威仪使防守既是兵刃的较量，也是意志的交锋。

### Grip of the Behemoth / 比蒙扼击

**Before EN:** <i>Primal Leverage.</i> Channeling the primeval strength of the great beasts, your hands clamp onto armor, horns, and limbs like siege machinery, snapping bone and pinning foes through pure brute superiority.

**Before ZH:** <i>荒蛮扼杀。</i>运转上古荒蛮巨兽之凶性，五指并拢如攻城机枢生生嵌合。扣其铁甲、折其筋骨，以绝对蛮力将一切抵抗碾碎为泥。

**Final EN:** You bring the weight of a great beast to every grip, push, and struggle for position.

**Final ZH:** 你将巨兽般的力量投入每一次抓握、推挤与位置争夺。

### Fulcrum of the Viper / 灵蛇巧掣

**Before EN:** <i>Dynamic Redirection.</i> You never resist an opponent's momentum; you hook a wrist or sweep an ankle at the precise pivot point, converting their charge into a violent face-first collision with the earth.

**Before ZH:** <i>借势倾敌。</i>从不以力相抗，唯于千钧一发之际挑其腕节、绊其脚踝。顺水推舟，借敌之奔袭冲力反制其身，令其轰然仆地。

**Final EN:** A small turn at the right pivot can redirect more force than a head-on struggle.

**Final ZH:** 在恰当支点的一次轻转，往往比迎面角力更能改变力量的方向。

### Deep-Root Clinch / 沉洋扼锁

**Before EN:** <i>Suffocating Asphyxiation.</i> Once your hold is established, you outlast your quarry's stamina completely. With the cold patience of a constrictor, your unyielding lungs grind down their desperate thrashing until they submit.

**Before ZH:** <i>长息窒竭。</i>一旦错身合锁，生机之深沉便成定局之锁钥。如巨蟒绞杀猎物般冷酷持久，以绵绵不绝之气血磨尽猎物的最后一丝挣扎。

**Final EN:** Endurance lets you maintain a hold after the first contest of strength has passed.

**Final ZH:** 耐力让你在最初的力量较量之后，仍能维持稳固的控制。

### Anatomical Pivot / 机理断节

**Before EN:** <i>Skeletal Engineering.</i> By mapping the mechanical weak points of the humanoid and monstrous skeletal structure, you twist joints against their natural articulation with effortless efficiency.

**Before ZH:** <i>骸骨机枢。</i>观万灵骸骨如发条转轴。准确洞悉关节与肌腱之天然逆向，轻施巧劲反折其轴，令敌身不由己溃不成军。

**Final EN:** You study how joints move together and use that knowledge to guide a maneuver.

**Final ZH:** 你研究关节如何协同活动，并以这份知识引导战技。

### Crane's Anticipation / 玄鹤听劲

**Before EN:** <i>Attuned Weight.</i> Sensing where an opponent plans to step before their weight even transfers, you intercept their limbs at the initiation of their stride, turning their own intentions against them.

**Before ZH:** <i>神意先占。</i>神意通明，听劲化劲。在敌重心尚未转移之前已洞察其发力意向，于虚实转换之刹那截其锋芒，先发而制人。

**Final EN:** You feel an opponent's balance through contact, answering their movement with your own.

**Final ZH:** 你从接触中感知对手的重心，以自己的动作回应其变化。

### Audacious Overthrow / 叱喝倾山

**Before EN:** <i>Theatrical Humiliation.</i> With a commanding roar and flamboyant disdain, you fling your opponent to the floor as if discarding worthless garbage, inspiring allies and demoralizing the enemy lines.

**Before ZH:** <i>万夫莫当。</i>叱咤声如惊雷破阵，举手投足尽展豪雄霸气。将披甲强敌若弃敝履般掷翻于地，把角力博弈化作震撼全场的凯旋宣告。

**Final EN:** An assertive step and a forceful challenge lend conviction to your attempt to unseat a foe.

**Final ZH:** 果断的步伐与强势的挑战，让你动摇敌人站位的行动更有决心。

### Giantwright's Craft / 巨匠巧工

**Before EN:** <i>Forge-Tempered Labor.</i> Endless years of wielding massive sledgehammers, felling ancient ironwoods, and forcing unyielding mechanisms grant your mighty hands an unmatched, steady precision.

**Before ZH:** <i>千钧之工。</i>历经挥动万钧巨锤、开山拓荒之淬砺。浑厚膂力化作最稳健之基底，纵使最为繁难沉重之工巧，亦在绝对力量操弄下轻巧如愿。

**Final EN:** Work that tires others teaches you the discipline of applying strength with care.

**Final ZH:** 让他人疲惫的劳作，教会你谨慎运用力量的技艺。

### Thief-King's Panache / 妙手绝尘

**Before EN:** <i>Invisible Prestidigitation.</i> Your fingers weave through delicate tumblers, pickpocketing, and tumbling maneuvers with the sublime grace of falling silk, turning the hardest thievery into fine art.

**Before ZH:** <i>穿花妙手。</i>十指翻飞若落樱拂水，轻灵身法如穿林飞燕。锁簧微鸣、暗度陈仓，诸般繁琐杂学在绝妙敏捷催动下，皆成神乎其技的优雅艺术。

**Final EN:** Your hands learn each task as a rhythm, returning to its fine motions with a performer's ease.

**Final ZH:** 你的双手将工作记作节奏，细小动作也能如表演般自然重现。

### Ascetic Diligence / 苦行研磨

**Before EN:** <i>Mettle of the Anchorite.</i> Where brilliant scholars collapse from exhaustion and nimble artisans cramp with fatigue, your iron constitution allows you to study, practice, and refine your craft through endless sleepless vigils.

**Before ZH:** <i>铁砚磨穿。</i>凡夫智穷于神疲，巧匠力竭于形惫。唯汝凭金石气血苦度寒暑，彻夜推敲，将寻常百艺千锤百炼至通神化境。

**Final EN:** Where inspiration fades, patient repetition keeps your craft moving forward.

**Final ZH:** 灵感消退之处，耐心的反复练习仍推动你的技艺前行。

### Encyclopedic Synthesis / 格物万象

**Before EN:** <i>Universal Blueprint.</i> To you, all branches of knowledge are interconnected facets of one grand cosmic design. Planar astronomy decodes lost ruins, and alchemy illuminates the biology of ancient aberrations.

**Before ZH:** <i>博洽古今。</i>天下万法，殊途同归。以穷理尽性之哲思融会百家，以星相几何照彻古墓暗阁，以炼金秘要剖解异界畸变，博洽冠绝当世。

**Final EN:** You connect unfamiliar problems to knowledge already gathered, building bridges between disciplines.

**Final ZH:** 你将陌生问题与已有知识相连，在不同学问之间架起桥梁。

### Wanderer's Lucidity / 云水澄明

**Before EN:** <i>Primal Empathy.</i> You master the world not by reading parchment, but by listening to its subtle heartbeat. Instinctive clarity whispers the hidden path across mountains, deciphering deceit without words.

**Before ZH:** <i>谛听万籁。</i>行万里路，体万物情。不滞于文牍断章，而听山川呼吸、察人心微澜。灵台通明，虽不言而洞晓万方机巧。

**Final EN:** Travel and observation have taught you to hear what a task requires before reaching for a tool.

**Final ZH:** 行旅与观察教会你先听懂事情的需要，再伸手取用工具。

### Silver-Tongued Virtuoso / 锦绣天潢

**Before EN:** <i>Audacious Charlatanism.</i> Impeccable charm and overwhelming bravado bridge any chasm in your formal training. When you lie, analyze lore, or charm a king, the sheer brilliance of your performance makes reality conform.

**Before ZH:** <i>风华绝代。</i>卓绝自信与旷世风华跨越经验之鸿沟。无论是舌战群儒、密探宫闱，抑或弄巧弄险，举首投足间皆教举世景仰、化假为真。

**Final EN:** Confidence carries your performance through unfamiliar work, inviting others to believe in your command.

**Final ZH:** 自信让你在陌生事务中仍保持从容，也让旁人愿意相信你的掌握。

### Mage-Hammer Inscription / 铁骨铸咒

**Before EN:** <i>Kinetic Sorcery.</i> You bend unstable arcane currents across your weapon and limbs through sheer crushing force, hammering spells into reality like incandescent iron forged upon an enchanted anvil.

**Before ZH:** <i>蛮霸注能。</i>以刚猛筋骨强锁狂暴魔能，如神匠抡锤砸击赤铁，将磅礴杀伐之势熔铸于符文咒令之中，法理威猛霸道，莫可撄其锋。

**Final EN:** You approach an incantation as a smith approaches iron, shaping it with disciplined exertion.

**Final ZH:** 你如铁匠对待生铁般对待咒语，以严整的发力塑成法术。

### Somatic Velocity / 疾影手印

**Before EN:** <i>Flicker Gestures.</i> Your somatic incantations unfold in blurred flourishes between heartbeats, releasing complex metamagic and spell-forms faster than enemy counter-mages can formulate a response.

**Before ZH:** <i>流光结印。</i>施法手势快逾流光惊鸿，指尖印契于刹那明灭间结成。敌方反制之咒尚未启唇，毁天灭地之法印已然呼啸破空。

**Final EN:** Exact, practiced gestures give your spellwork the cadence of a deft duelist.

**Final ZH:** 精准而娴熟的手势，使你的施法带上灵巧剑客的节奏。

### Crucible of the Conduit / 鼎炉承法

**Before EN:** <i>Living Leyline.</i> Your flesh and blood serve as an insulated crucible for the most volatile planar magics. Arcane backlash that would vaporize frail wizards is absorbed harmlessly into your boundless vitality.

**Before ZH:** <i>血肉熔炉。</i>将一身血肉铸就为吞吐天地灵潮之活体鼎炉。足以将凡俗法师撕裂融化的狂暴反噬，尽数被深沉生机纳为薪柴，咒力沉凝雄浑。

**Final EN:** You make bodily endurance part of your magical practice, learning to bear the effort of channeling power.

**Final ZH:** 你将身体的耐力纳入魔法修习，学会承受引导力量的消耗。

### Archmage's Codex / 万法源流

**Before EN:** <i>The Prime Paradigm.</i> Magic is neither gift nor miracle; it is the ultimate science. By cross-referencing planar laws and ancient treatises, you cast with the unerring mathematical perfection of a high archmage.

**Before ZH:** <i>大奥术师真典。</i>魔法既非恩赐，亦非奇迹，乃虚空至高之严密数理。研索三千法则，条分缕析，令每一道法术皆如神来之笔，穷极奥术造化。

**Final EN:** Every spell becomes a proposition to study, refine, and set beside the work of earlier arcanists.

**Final ZH:** 每一道法术都是可研读与改进的命题，与前人的奥术成果相互印证。

### Gnostic Channel / 玄鉴通幽

**Before EN:** <i>Resonance of the Void.</i> You draw upon magic not by memorizing ink on sheepskin, but by tuning your soul to the primeval song that reverberates through the planes, giving your spells irresistible spiritual weight.

**Before ZH:** <i>通灵契道。</i>不滞死理，唯契天机。将神魂校准于漫贯万界的原初天籁，法随心动，言合天道，令群魔难脱此玄妙法网。

**Final EN:** You listen for the cadence beneath an incantation and let attentive instinct guide its expression.

**Final ZH:** 你聆听咒语深处的节律，以专注的直觉引导它的表达。

### Sovereign Decrees / 天宪法旨

**Before EN:** <i>Mandate of the Monarch.</i> The magical weave submits because you do not ask—you command. Your incantations ring with the terrifying resonance of the First Kings, making spell resistance wither before your absolute authority.

**Before ZH:** <i>神皇玉律。</i>诸界灵潮因吾言而俯首。咒言响遏行云，蕴藏太古龙皇与创世王侯之无上律令，所过之处法抗崩散、万象顺从。

**Final EN:** You speak an incantation with the confidence of a decree, giving its form the weight of conviction.

**Final ZH:** 你以宣告法旨般的自信吟诵咒语，让信念为法术的形式添上分量。

### Berserker's Overrun / 破阵裂山势

**Before EN:** Gorum's Reckless Abandon. In the bloody ethos of the Iron God, armor is a craven distraction. Dropping all defense, you throw the entirety of your body weight and reckless fury into unstoppable, earth-cleaving assaults.

**Before ZH:** 狂神破阵。 遵奉铁甲战神之霸道信条：守御乃战阵懦夫之伪饰。尽弃铠甲之护，将全副身量与嗜血狂意倾注于每一次挥击之上，每击皆带裂山荡寇之威。

**Final EN:** You commit to the next blow with a battle zeal reminiscent of Gorum, leaving less attention for your guard.

**Final ZH:** 你以令人想起戈鲁姆的战意全力挥击，也因此分出更少心力守护自身。

### Willow in the Gale / 惊鸿穿林势

**Before EN:** Sinuous Evasion. Yielding like willow branches before an axe, you twist and skim through the air. Blades find only empty space, though prioritizing total avoidance leaves little momentum for counter-attacks.

**Before ZH:** 惊鸿避刃。 如狂风过隙中的柔韧杨柳，身如惊鸿，穿花掠影。任凭敌刃狂澜倾泻，唯求不沾微尘，虽令反击招架暂失先手，却教强敌连连空挥。

**Final EN:** Like a willow in a gale, you favor yielding movement over the force of a committed strike.

**Final ZH:** 你如狂风中的柳枝般顺势而动，将余力留给闪避而非重击。

### Mountain's Deep Roots / 不动磐峰势

**Before EN:** Anchor of the Earth. Planting your heels into subterranean stone and drawing deep grounding breaths, you turn your body into an immovable monolith of endurance, absorbing battering blows while sacrificing reach and tempo.

**Before ZH:** 磐岳归根。 含胸拔背，气沉丹田，双足若古松生根深扎厚土。化作不动之肉身峰峦，源源吞纳重击摧折，固若金汤却不逞口舌攻伐。

**Final EN:** You settle into a mountain's patient stillness, gathering yourself at the cost of offensive tempo.

**Final ZH:** 你如山岳般沉稳地收束自身，并为此放慢进攻的节奏。

### Grandmaster's Gambit / 弈者静待势

**Before EN:** The Waiting Blade. Like a grandmaster studying a chessboard, you hold your weapon in poised repose, refusing to initiate until an enemy overextends into the exact trap you have orchestrated.

**Before ZH:** 弈者断局。 若国手对弈，横剑藏锋而隐忍不发。任凭强敌喧嚣，唯俟其步伐失序、露出一瞬破绽，立时以寒光霆击断其胜局。

**Final EN:** You study the openings left by passing foes, keeping a careful account of their movement.

**Final ZH:** 你研读敌人移动时留下的空隙，审慎把握他们的行动轨迹。

### Mirror of Still Waters / 明镜止水势

**Before EN:** Void of the Ascetic. Retreating into a state of absolute spiritual equilibrium, the battlefield reflects upon your surface without creating a ripple. Defenses become impregnable, though violence holds no charm for you.

**Before ZH:** 澄澈自照。 收敛神识入于虚空明镜之中。战尘喧扰皆过眼云烟，百邪莫侵、万法不破，于极致清宁间消弭兵戈戾气。

**Final EN:** You quiet the impulse to strike, accepting a gentler attack in exchange for a steadier guard.

**Final ZH:** 你平息急于出手的冲动，以较轻的攻势换取更稳固的守势。

### Vanguard's Banner / 金戈铁旌势

**Before EN:** Warlord's Rallying Banner. Exposing yourself fearlessly at the front of the battle line, your radiant presence and ringing war-cries banish dread from your companions' hearts, elevating their swords with the certainty of triumph.

**Before ZH:** 铁旌号令。 挺身屹立于两军锋矢交错之处，铠光耀目，叱喝惊雷。舍身立威以定军心，令三十步内同袍热血沸腾，剑锋所指无坚不摧。

**Final EN:** You place your companions' courage before your own safety and become the standard they rally around.

**Final ZH:** 你将同伴的勇气置于自身安危之前，成为众人聚拢的旗帜。

### Defiance at the Precipice / 绝境砥柱

**Before EN:** <i>Mettle of the Dying Boar.</i> When blood blinds your vision and the body totters at death's brink, your marrow ignites with primeval obstinacy. Your bruised bones harden into adamantine against the reaper's scythe.

**Before ZH:** <i>困兽之斗。</i>当鲜血迷障双目、身躯摇摇欲坠之时，骨髓深处的蛮荒血性轰然觉醒。伤痕反成重甲，濒危之躯硬撼死神镰刀，化作不倒之坚壁。

**Final EN:** When injury narrows your choices, you answer with the stubborn patience of a cornered animal.

**Final ZH:** 伤势使选择愈发有限时，你以困兽般的坚忍回应危局。

### Ambush of the Viper / 封喉首刃

**Before EN:** <i>The Fatal Opening.</i> In the first heartbeat of engagement, before the enemy has set their footing or adjusted their shield, your weapon strikes with lethal, unhesitating finality.

**Before ZH:** <i>机先必杀。</i>双兵方触，战端甫启。乘敌阵未稳、心神仓惶之初，发迅雷之刃直取咽喉要害，一瞬锁定胜局。

**Final EN:** You practice the opening exchange until the first heartbeat of a fight feels familiar.

**Final ZH:** 你反复磨炼交锋的起手，让战斗最初的一瞬也有迹可循。

### Oath of Retribution / 复仇血誓

**Before EN:** <i>A Companion's Requiem.</i> A trusted ally's fall does not break your spirit—it unleashes a holy conflagration. Grief hardens into a merciless vow of vengeance that drives your weapon with apocalyptic fury.

**Before ZH:** <i>同袍血祭。</i>知交战殁，不折其志，反激起焚天怒火。悲恸化为诛绝仇寇之血誓，每记挥砍皆携带着为逝者索命的狂怒威能。

**Final EN:** A companion's death gives grief a direction, lending fierce purpose to the strokes that follow.

**Final ZH:** 同伴的阵亡为悲痛赋予方向，让接下来的挥击承载复仇的决心。

### Crane's Severance / 蓄势孤峰

**Before EN:** <i>Stillness Before the Severance.</i> While lesser warriors flail in reckless frenzy, you breathe with the rhythm of the duel. When the inevitable opening aligns, your patient strike descends with crushing karmic weight.

**Before ZH:** <i>定海断流。</i>庸夫贪刀乱舞，智者观时待变。于喧嚣中静候因果契合之一瞬，雷霆发轫，一刀斩尽千重因果。

**Final EN:** You reserve your fullest commitment for the first clean opportunity of each exchange.

**Final ZH:** 你将最充足的心力留给每一次交锋中最先到来的出手机会。

### Gorum's Last Stand / 狂神绝唱

**Before EN:** <i>Rage of the Doomed.</i> Staring into the open jaws of the abyss, all thought of self-preservation evaporates. If you are to fall, your dying fury will carve an army's worth of foes into the earth beside you.

**Before ZH:** <i>绝境死决。</i>身陷九死一生之绝域，尽焚求生之念。纵命陨当场，亦要以毕生神力抡碎山峦，拖拽千百强敌共葬深渊。

**Final EN:** At the edge of defeat, you trade caution for a final display of the Iron Lord's battle fervor.

**Final ZH:** 败亡临近时，你放下谨慎，以铁甲之神般的战斗热忱作最后一搏。

### Cadence Decoded / 阅破机宜

**Before EN:** <i>The Theorem Solved.</i> A single exchange of steel provides all the data your keen intellect needs. Having deciphered the opponent's balance, reach, and habits, every subsequent stroke strikes like an answered equation.

**Before ZH:** <i>算无遗策。</i>刃锋初接，已收尽敌势。洞悉其步法转折与破绽宿疾，解构其战术图谱，自此招招命中命门，克敌制胜。

**Final EN:** One exchange gives you a pattern to study, letting later attacks follow a better-informed line.

**Final ZH:** 一次交锋便为你提供可研读的线索，让之后的攻击沿着更明晰的轨迹展开。

### Arcane Aegis / 魔能天衣

**Before EN:** <i>Weave of Abjuration.</i> Residual arcane energy coats your form like an invisible mantle of deflection. Raw caster discipline turns every stray strand of magic into an instinctive ward that diverts lethal blades before they touch skin.

**Before ZH:** <i>法脉流形。</i>奔涌的奥术源能如无形天衣披覆周身。施法者长年修习沉淀的法力底蕴化为本能护体气场，将近身斩杀的锐刃自毫厘间偏转卸劲。

**Final EN:** Years of spellwork teach you to turn practiced magical control toward personal defense.

**Final ZH:** 多年的施法修习，让你学会将娴熟的魔力控制用于自身防守。

### War-Hardened Reflexes / 百战身魄

**Before EN:** <i>Battlefield Instincts.</i> Countless skirmishes have conditioned your reflexes into pure survival instinct. When a fireball explodes or a toxin seeps in, your combat-honed muscles and grit react before conscious thought can form.

**Before ZH:** <i>铁血砥砺。</i>尸山血海的淬炼令机体生出超凡的求生直觉。无论是法术轰炸的炽烈余波，还是蚀骨剧毒的暗算浸染，千锤百炼的身魄皆能先于心念自发抵御。

**Final EN:** Battle has trained your responses until endurance, movement, and resolve share the same rhythm.

**Final ZH:** 战斗锤炼你的反应，使耐力、行动与意志有了共同的节律。

### Scholar's Positioning / 通识御敌

**Before EN:** <i>Omnidisciplinary Awareness.</i> From architectural understanding of terrain to biological analysis of anatomy and athletic equilibrium, your encyclopedic expertise informs every step. You evade danger simply by never occupying a disadvantaged position.

**Before ZH:** <i>博识兼修。</i>无论是对战场地势的建筑学洞察，还是对敌手骨肉机巧的生理解构，浩瀚的学识化作规避危局的无上准绳。知己知彼，步步先机，自立于不败之地。

**Final EN:** Lessons gathered from many crafts become practical answers to an enemy's approach.

**Final ZH:** 从各门技艺中积累的经验，成为你应对敌人来势的实际手段。

### Ley-Infused Vitality / 灵脉淬体

**Before EN:** <i>Arcane Font of Flesh.</i> Unbounded magical force saturates your organs, sinew, and blood. Rather than withering under eldritch power, your mortal biology is fundamentally strengthened, sustained by a perpetual reservoir of vital energy.

**Before ZH:** <i>灵潮融血。</i>磅礴的奥能奔流日夜冲刷四肢百骸，肉身非但未被异界魔能侵蚀，反与本源法力彻底融汇，气血充盈饱满，化为生生不息的寿元洪流。

**Final EN:** You make the discipline of channeling magic part of the discipline of sustaining yourself.

**Final ZH:** 你将引导魔法的修习融入维持自身生机的修习。

### Dawn of the Soul / 法相初明

**Before EN:** <i>Spirit's Resplendent Vanguard.</i> At the first scent of bloodshed, your inner soul awakens with incandescent clarity. A blazing psychic barrier surges forth to envelope you, absorbing the initial brunt of hostile fury.

**Before ZH:** <i>灵台定照。</i>杀意初现之际，本命法相灵光乍现，神华自生。一道璀璨耀目的心能罡气应激而发，在战端初启之刹那替身躯承受狂暴冲击。

**Final EN:** When battle begins, your practiced spellwork gathers around you like a brief mantle of dawn.

**Final ZH:** 战斗开始时，娴熟的施法力量如短暂的晨辉般聚拢在你周围。

### Blade of the Spell-Saint / 剑圣咒痕

**Before EN:** <i>Synthesis of Steel and Sorcery.</i> The deadly discipline of weapon mastery infuses your incantations. Every gesture is delivered with the unerring finality of a master swordsman's coup de grâce, making your spells nearly impossible to resist.

**Before ZH:** <i>剑咒合一。</i>将登峰造极的剑道杀意熔炼于每一道法咒之中。施法手势如宗师拔刀般决绝肃杀、无懈可击，令敌手神魂受摄，极难抵御法术威能。

**Final EN:** The precision learned with a weapon gives you another way to shape demanding magic.

**Final ZH:** 从兵刃上学来的精准，为你驾驭复杂魔法提供另一条途径。

### Behemoth's Heritage / 比蒙遗脉

**Before EN:** <i>Predatory Primacy.</i> The raw, untamed savagery of prehistoric behemoths answers your conjuration. Your summoned beasts arrive swollen with colossal sinew, tearing into the vanguard with primeval ferocity.

**Before ZH:** <i>荒古蛮性。</i>史前比蒙的洪荒凶性响应你的召唤阵式。降临于现世的灵兽肌骨暴涨，以撕裂山岳的狂暴巨力撕碎当面之敌。

**Final EN:** You lend your summoned companions the forceful bearing that guides your own movements.

**Final ZH:** 你将支配自身行动的强劲力量借给召来的伙伴。

### Zephyr's Covenant / 风灵疾契

**Before EN:** <i>Gale-Rider's Shroud.</i> You bind your summons to the capricious currents of the elemental planes of air. Your allies manifest surrounded by swirling updrafts, darting and flanking with supernatural velocity.

**Before ZH:** <i>疾风咒契。</i>你将召唤盟约与气元素位面的无羁狂风相缔结。现身的生物周身裹挟风暴旋流，穿梭于刀光剑影间，身如脱兔、迅疾莫测。

**Final EN:** The rhythm of your agile steps becomes a pattern for the creatures answering your call.

**Final ZH:** 你灵巧步伐的节奏，成为回应召唤的生物可以追随的范式。

### Titan's Lifespring / 巨怪生机

**Before EN:** <i>The Indomitable Tether.</i> Your conjurations draw from the boundless vitality of the earth's deep roots. Called creatures possess thick, fibrous hides and unflagging endurance, remaining standing through apocalyptic onslaughts.

**Before ZH:** <i>大地生机。</i>你的通灵印记自地脉深处的生命泉眼汲取活力。受召之物皮糙肉厚、耐力无穷，纵受狂轰滥炸亦能昂然伫立。

**Final EN:** You draw on your own hardiness when preparing a body for a summoned companion.

**Final ZH:** 为召来的伙伴塑成躯体时，你借鉴自身承受磨砺的耐力。

### Aegis of the Schema / 天元魔阵

**Before EN:** <i>Geometric Abjuration.</i> Exact planar coordinates and rigorous arcane geometries envelop your summons in interlocking kinetic wards. Foes find their blows deflected by visible mathematical equations.

**Before ZH:** <i>天元轨则。</i>以严谨至极的位面坐标与几何符阵构筑召唤回路。受召生物周身覆以流转的几何力场，使敌手的刃锋沿着折射偏角滑开。

**Final EN:** You plan a summoned creature's defenses as carefully as an architect plans a wall.

**Final ZH:** 你如建筑师设计城墙般，仔细安排召唤生物的防守。

### Empathic Communion / 神契灵犀

**Before EN:** <i>Shared Awareness.</i> A quiet, transcendent thread connects your spiritual awareness to the minds of your servants. Forewarned by your third eye, they sidestep spells and shrug off curses as if sharing your foresight.

**Before ZH:** <i>心印相通。</i>以神识灵犀为纽带，将超然直觉投射于受召生物心窍。它们如获先知之眼，在法术呼啸与诅咒降临前敏锐躲避、心如止水。

**Final EN:** Attentive guidance helps a summoned companion meet danger with steadier instincts.

**Final ZH:** 专注的引导帮助召来的伙伴，以更稳固的本能迎接危险。

### Dominator's Calling / 御统王令

**Before EN:** <i>Imperial Decree.</i> Your conjuration is no mere plea across planar boundaries, but an absolute mandate. Infused with your indomitable majesty, your creatures strike with fearless ferocity and sovereign intent.

**Before ZH:** <i>帝令敕召。</i>你的召唤非是祈求位面生灵的援手，而是降下无可违抗的君王敕令。受令而来的异界大军沐浴皇威，舍生忘死、击无不克。

**Final EN:** Your confident command gives summoned companions a clear purpose when they enter the fray.

**Final ZH:** 自信的指挥让召来的伙伴在投入战斗时拥有明确目标。

### Martyr's Transference / 形神替生

**Before EN:** <i>What you surrender, your servants inherit.</i> You willingly drain your own vital energies, intellect, and worldly presence across the conjuration circle, feeding raw spirit directly into your minions to elevate them to terrifying heights.

**Before ZH:** <i>舍己塑灵。</i>割裂自身气血、灵慧与威仪，尽数灌入通灵法阵。以施法者本命元神为薪柴，换取召来异界使者全方位的惊世蜕变。

**Final EN:** You accept a body's burden so that the creatures bound to your call may stand stronger.

**Final ZH:** 你甘愿让自身承受负担，使回应你召唤的生物更加强健。

### Eldritch Crucible / 双生法炼

**Before EN:** <i>Asymmetrical Resonance.</i> Through arcane harmonic resonance, you stretch each spark of sacrificed essence across the summoning circle twofold. A minor toll upon your vessel unlocks disproportionate planar ascendancy.

**Before ZH:** <i>法脉谐振。</i>洞悉异界位面的回音法则，将献祭的精魄于通灵阵中激荡放大。微损施法本体，即可撬动受召军团成倍的位面威能。

**Final EN:** An exacting pact magnifies what you surrender, letting sacrifice feed a companion's strength.

**Final ZH:** 严密的契约放大你付出的代价，让牺牲转为伙伴的力量。

### Tribute of Iron Dominion / 夺冕化蛮

**Before EN:** <i>Crown Surrendered to Claws.</i> You strip away the haughty grace of command, channeling raw monarchic authority into pure, brutal muscle. Your minions lose all subtlety, transfigured into hulking juggernauts of annihilation.

**Before ZH:** <i>折冠铸殛。</i>剥离统御者的从容仪度，将全部支配欲念熔铸为受召者撕碎万物的暴戾蛮力。麾下爪牙褪尽精巧，化作摧山撼岳的嗜血巨灵。

**Final EN:** You yield some commanding presence to give a summoned body greater physical force.

**Final ZH:** 你让渡部分统御的气势，为召来的躯体换取更强的筋骨之力。

### Point-Blank Ruin / 咫尺绝杀

**Before EN:** <i>Crowd the Guard.</i> You hit hardest once you step inside an enemy's reach, crowding their guard and driving your weapon into gaps with suffocating, bone-crushing violence.

**Before ZH:** <i>近身封喉。</i>欺身步入敌刃中门以内，以贴身挤压封死敌之招架空间。在毫厘咫尺间全力迸发破坏力，刃碎重铠、骨断筋折。

**Final EN:** You train to deliver a committed blow in the cramped space inside an opponent's guard.

**Final ZH:** 你磨炼在逼近敌人守势的狭窄空间内全力出手的技巧。

### Horizon's Deadeye / 苍穹神击

**Before EN:** <i>Draw of the Distant String.</i> Distance grants clarity. As space opens, you read wind, drop, and motion with supernatural calm, releasing your projectile on an arc that strikes with catastrophic kinetic force.

**Before ZH:** <i>长空夺魄。</i>旷阔的视野赐予神识绝对清明。风向、重力与敌踪位移尽在推演之中，箭离弦如流星贯日，在远距终端迸发致命贯穿力。

**Final EN:** Distance gives you room to read a target's line and settle the weapon before release.

**Final ZH:** 距离为你留出判断目标轨迹的余地，也让兵刃在出手前更加稳定。

### Harmonic Cleave / 流光截角

**Before EN:** <i>The Golden Threshold.</i> Combat is measured in zones of maximum leverage. You instinctually maintain the ideal middle band of engagement, where the weapon's centrifugal acceleration and your balance reach their devastating apex.

**Before ZH:** <i>得机得势。</i>交锋胜负系于杠杆发力的最佳截角。在敌我相距的中距黄金带内，兵刃离心加速与身体重心的协调达到极点，挥砍轰杀如雷霆破空。

**Final EN:** You study the middle ground of an engagement, where spacing lets a weapon do its best work.

**Final ZH:** 你研究交锋的中间距离，让恰当间隔帮助兵刃发挥所长。

### Aegis of the Pure Warder / 绝界镇魔使

**Before EN:** <i>Wards Seen Before They Rise.</i> Serene spiritual insight lets you feel the subtle fault lines in hostile magic before it forms. Every abjuration you weave becomes an unyielding dimensional seal, turning hostile sorcery to ash against your wards.

**Before ZH:** <i>预见先机。</i>超然神识洞悉虚空法力之脉络，先于敌咒成形前截断其施法回路。所布防护结界如万载铁幕，令狂暴敌法触之即溃、封镇虚空。

**Final EN:** You approach abjuration with patient attention, choosing its wards over other branches of magic.

**Final ZH:** 你以耐心而专注的觉察修习防护术，并将它的结界置于其他魔法之前。

### Sovereign Gatekeeper / 统界辟门者

**Before EN:** <i>Command the Threshold.</i> The boundaries between the Great Beyond and the mortal world buckle before your imperial presence. When you summon beings or open spatial rifts, planar entities answer with unquestioning allegiance.

**Before ZH:** <i>界门独尊。</i>多元宇宙与主位面间的界膜在你霸道威仪下屈服洞开。凡你所敕召之异界军团、所撕裂之空间裂隙，皆奉汝意志为至高法度。

**Final EN:** Your magic finds its clearest voice at the threshold between a call and an answer.

**Final ZH:** 你的魔法在呼唤与回应的门槛之间，找到最清晰的表达。

### Eye of the Chronomancer / 溯时先知

**Before EN:** <i>Knowledge Drawn First.</i> Rigorous chronological calculus turns prophecy into a lethal tactical weapon. Every divination you cast is the inevitable conclusion of causal sequences that lesser minds could never anticipate.

**Before ZH:** <i>洞悉天机。</i>以无上灵台推演因果命运之严密算式。所有占卜预言皆如落子定局，将未来千万重变数化为洞若观火的必胜兵法。

**Final EN:** You compare signs and patterns until divination feels like reading a carefully kept record.

**Final ZH:** 你反复比对征兆与规律，让预言术如阅读详尽的记录般有章可循。

### Sovereign of the Heart / 倾心国主

**Before EN:** <i>Will as Sovereignty.</i> You do not coax or charm; you simply declare reality, and the minds of others willingly conform. Your psychic magnetism is so absolute that submitting to your enchantment feels like natural devotion.

**Before ZH:** <i>魅惑众生。</i>无需曲意逢迎，亦不必谄媚欺瞒；你的每一道敕令皆化为直击灵魂的崇高魅力，令受术者如饮甘饴、心悦诚服归顺座下。

**Final EN:** You study how conviction and desire give enchantment a foothold in another mind.

**Final ZH:** 你研究信念与欲望如何为惑控术提供触及他人心智的立足点。

### Pyre of the Architect / 灾变筑城师

**Before EN:** <i>Calculated Conflagration.</i> Destructive magic is not wild fury, but precision engineering. You refine every blast and conflagration through mathematical analysis until thermal yield and concussive shock arrive with devastating economy.

**Before ZH:** <i>析构破灭。</i>狂暴的塑能毁灭非是蛮力渲泄，而是精密的法理重构。精准计算每一丝元素聚变与爆轰角度，以最纯粹的毁灭算式夷平阵列。

**Final EN:** You favor the direct expression of energy, shaping evocation with an architect's deliberate design.

**Final ZH:** 你偏爱能量的直接表达，以建筑师般的审慎规划塑成塑能术。

### Phantasmagoria Maestro / 织影幻圣

**Before EN:** <i>Reality by Performance.</i> Your dramatic charisma gives falsehood the unyielding weight of truth. Phantasms and sensory tapestries spun from your imagination are so intoxicating that the universe itself confuses them for genuine reality.

**Before ZH:** <i>虚实莫辨。</i>绝代风华赋予镜花水月以颠扑不破的现世重量。指尖勾勒的千般幻象与光影迷宫如此逼真动魄，纵使天地规则亦为之淆乱。

**Final EN:** Light, expectation, and careful presentation become the instruments of your illusion craft.

**Final ZH:** 光影、期待与精心安排的呈现，成为你编织幻术的工具。

### Harvester of the Boneyard / 冥河渡魂人

**Before EN:** <i>Hear the Last Breath.</i> Deep, unblinking insight into Pharasma's river of souls grants you authority over death's threshold. Necromancy is no profane violation to you, but a quiet, chilling dialogue with mortality.

**Before ZH:** <i>冥渊引渡。</i>超然静观骨园与冥河滔滔逝水，彻悟死生大限之终极奥秘。死灵术非是亵渎之艺，而是于残躯碎魂间聆听死寂真理之权能。

**Final EN:** You study mortality's thresholds; the Boneyard is an image for that inquiry, not a claim of Pharasma's approval.

**Final ZH:** 你研习死生的界限；骨园只是这份探究的意象，并不代表获得法拉斯玛的认可。

### Sculptor of Prime Matter / 塑质造化使

**Before EN:** <i>Form as Formula.</i> To you, physical matter, bone, and flesh are mere variable constants waiting for rebalancing. Transmutation becomes an exact architectural art once you comprehend where reality's bonds intersect.

**Before ZH:** <i>万化由心。</i>在明睿智力之下，万物质性与生灵躯干皆为可重构的几何方程式。参透构象基石之枢纽，挥手间点石成金、化凡为圣。

**Final EN:** You consider every material form a structure whose relationships can be understood and rearranged.

**Final ZH:** 你将物质的形态视作可被理解与重新安排的结构关系。

### Pyre of the Phoenix / 炽皇凤涅

**Before EN:** <i>Authority of Embers.</i> Primeval flame recognizes the blaze in your spirit. Raging conflagrations surge with sovereign fury when your commanding aura bids them burn with incandescent heat.

**Before ZH:** <i>真火耀世。</i>太古烈焰呼应灵魂深处的炽烈威仪。赤炎如凤皇展翼遮天蔽日，随你的皇者令咒将世间一切污秽化为漫天熔灰。

**Final EN:** You give fire magic the fierce expression of your own conviction, borrowing the phoenix as its image.

**Final ZH:** 你让火焰魔法承载自身热烈的信念，借凤凰作为它的意象。

### Stillness of the Glacial Void / 极渊玄冰

**Before EN:** <i>Winter Without Tremor.</i> Meditative serenity cools the blood to absolute stillness. Your frost magic carries the quiet finality of ancient glaciers, extinguishing heat and life with sublime detachment.

**Before ZH:** <i>玄渊绝灭。</i>超然古拙的入定心境令万物生机归于沉寂。玄冰寒潮如万古不化之冰川冷酷蔓延，以绝对静滞冰封一切喧嚣。

**Final EN:** Still attention lends your cold magic the patient character of a glacier.

**Final ZH:** 沉静的专注赋予寒冷魔法如冰川般耐心而坚定的气质。

### Tempest-Dancer / 御雷疾影

**Before EN:** <i>Lightning in Motion.</i> Like a lightning rod moving across a storm front, your blinding agility guides electrical arcs with the precision of a rapier thrust, discharging thunderbolts into vital vulnerabilities.

**Before ZH:** <i>乘雷御电。</i>如疾电般灵动的步法穿梭于雷暴前沿。指尖引导的狂暴雷霆如决斗者的绝杀刺击，循着破绽顷刻灌注崩解之能。

**Final EN:** Your practiced gestures trace the paths by which lightning takes shape.

**Final ZH:** 娴熟的手势描出雷电逐渐成形的路径。

### Vitriolic Equation / 腐解算律

**Before EN:** <i>Corrosion by Design.</i> Acid is molecular entropy made manifest. Your intellect calculates the precise chemical weaknesses of armor, flesh, and stone, unleashing caustic liquefaction that dissolves all matter.

**Before ZH:** <i>蚀质天平。</i>强酸消融乃是分子结构崩解的明证。以精密的炼金数律解析护甲与骨肉之薄弱点，倾泻出将金石化为泥浆的剧毒蚀流。

**Final EN:** You bring an alchemist's attention to acid magic, considering how each surface yields to corrosion.

**Final ZH:** 你以炼金术师般的专注修习强酸魔法，思量不同表面如何承受腐蚀。

### Herald of the Shattered Sky / 破霄神音

**Before EN:** <i>Sound Given Command.</i> Sonic vibration resonates in harmony with your imperious vocal timbre. A single uttered word sends shockwaves shattering crystal, steel, and eardrums like thunderclaps at sea.

**Before ZH:** <i>破晓神啸。</i>声波震颤与君王之音完美谐振。真言一出，音浪化为实质的摧城狂涛，震碎金铁重甲，断敌筋骨神识。

**Final EN:** You shape sonic magic with the same conviction that carries your voice across a hall.

**Final ZH:** 你以让声音响彻厅堂的同一种信念，塑成音波魔法。

### Axiom of Unseen Force / 虚空定则

**Before EN:** <i>Thought Given Impact.</i> Pure arcane kinetic vectors materialize at your intellectual command. Unseen force solidifies into mathematical theorems that smash through shields and spatial barriers alike.

**Before ZH:** <i>灵能定界。</i>将纯粹的思维修为固化为不可动摇的力场矢线。无形力场化作坚不可摧的定理重击，轰碎一切凡俗护盾与位面阻隔。

**Final EN:** You picture invisible force as an exact arrangement of pressures and boundaries.

**Final ZH:** 你将无形的力场想象为压力与边界的精确排列。

### Fountain of Solar Dawn / 金阳圣晖

**Before EN:** <i>Grace That Rekindles.</i> Boundless celestial radiance pours through your transcendent presence. Positive energy floods your allies with solar vitality while searing the undead with blazing holy fury.

**Before ZH:** <i>昭昭如日。</i>宛若太阳晨曦般的圣洁光芒自你的灵魂奔涌而出。蓬勃生机抚平盟友伤痕，同时化作焚灭不死邪祟的炽烈金焰。

**Final EN:** The image of first light gives your positive-energy magic a clear and generous expression.

**Final ZH:** 初升晨光的意象，让正能量魔法有了明晰而慷慨的表达。

### Vigil of the Gloom / 死寂枯荣

**Before EN:** <i>Stillness Beyond Breath.</i> You observe the entropic stillness left behind when life recedes. Negative energy obeys your quiet discernment with chilling fidelity, withering life into ash.

**Before ZH:** <i>寂灭归墟。</i>深体生死循环、万象归寂之大道。阴邪死能如冥河幽流般如臂使指，无声无息间剥离血肉精气，令生者枯槁。

**Final EN:** You contemplate endings until negative-energy magic finds a restrained, deliberate form.

**Final ZH:** 你静思万物的终结，使负能量魔法呈现克制而审慎的形式。

### Puppeteer of the Mind / 惑魂主宰

**Before EN:** <i>Will Behind the Smile.</i> Your hypnotic charisma entwines itself with hostile psyches like invisible silk. Long before the target realizes their thoughts are no longer their own, your mental dominion is already complete.

**Before ZH:** <i>提线惑魂。</i>无孔不入的摄魂魅力如蛛丝般深植于敌手识海。敌方尚不自知心意已动，其神魂早已沦为受你肆意摆弄的提线木偶。

**Final EN:** You study the threads of attention through which mind-affecting magic takes hold.

**Final ZH:** 你研究影响心灵的魔法如何沿注意力的细线取得立足点。

### Barbed Carapace / 逆鳞铁刺

**Before EN:** <i>Retaliation in Iron.</i> Your armor is forged not merely for passive deflection, but bristling with predatory counter-spikes and interlocking serrated plates. Every close strike bites into your guard only to tear the attacker's own flesh to ribbons.

**Before ZH:** <i>荆棘反噬。</i>披挂之甲胄绝非被动格挡之死物，其上密布倒钩棘刺与机括咬合的利齿铁鳞。敌刃犯我中门之时，必遭荆甲逆鳞凶狠绞杀，反噬其血肉筋骨。

**Final EN:** You imagine armor as a thorned carapace, turning the idea of a close assault back upon its source.

**Final ZH:** 你将护甲想象为带刺的甲壳，让贴身攻击的意象反向指向来袭之处。

### Citadel of Steel / 铸铁城阙

**Before EN:** <i>Living Fortress.</i> Dwarven fortress smiths know that heavy mail and folded plate can become an impenetrable bulwark. With each rhythmically measured breath and foot plant, your layered harness absorbs incoming momentum, continuously reinforcing an impregnable shield of temporary vitality.

**Before ZH:** <i>身化铁城。</i>矮人要塞匠师的秘传技艺：重甲与折叠钢板在百战之躯上融为不可逾越的移动要塞。每随沉稳呼吸起伏吐纳，层叠重甲化解冲击余波，每轮重聚护体罡气。

**Final EN:** Layered armor becomes the image of a portable fortress, built to endure one exchange at a time.

**Final ZH:** 层叠甲胄化作移动要塞的意象，准备承受一次又一次交锋。

### Long-Reach Gambit / 长锋险势

**Before EN:** <i>Domain of the Spear-Point.</i> True spear-masters do not merely swing a weapon; they command the geometry of space. By adjusting hand placement along the haft to its maximum extension, you double your threat radius, controlling the battlefield through unyielding reach at the cost of leveraged cutting force.

**Before ZH:** <i>一寸长，一寸强。</i>真正的枪矛大宗师非但运用兵刃，更御统战场空间之维度。将握持手位退至枪杆末端极限，以牺牲近身发力杠杆为代价，换取触角倍增的绝对威慑半径，令万敌难近。

**Final EN:** You commit to a wider arc of engagement, accepting a less forceful blow as the price of distance.

**Final ZH:** 你将战线推向更宽的弧面，并接受出手力道减弱作为距离的代价。
