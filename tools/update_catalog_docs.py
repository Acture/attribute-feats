"""Synchronize README and the P-831 audit from the final feat catalog."""
import json
import re
import subprocess
from feat_catalog import ROOT, catalog, calls, split_args, literal, named

rows=catalog()
before_path=ROOT/'artifacts/text-before.json'
before=json.loads(before_path.read_text(encoding='utf-8')) if before_path.exists() else catalog(sources={f:subprocess.run(['git','show','6887a8a:New_Feats/'+f],cwd=ROOT,check=True,capture_output=True,encoding='utf-8').stdout for f in {r['file'] for r in rows}})
prior={r['internal']:r for r in before}
sources={r['file']:subprocess.run(['git','show','d4de92a:New_Feats/'+r['file']],cwd=ROOT,check=True,capture_output=True,encoding='utf-8').stdout for r in rows}
oldnames={}
for r in rows:
    source=sources[r['file']]
    # Old direct Common.L keys (armor and reach).
    for body,_,_ in calls(source,'Common.L'):
        args=split_args(body)
        if args[0]=='"'+r['key']+'.Name"' and args[1].startswith('"'):
            oldnames[r['internal']]=literal(args[1]); break
    if r['internal'] in oldnames:continue
    if r['family']=='Stance':
        bodies=[body for body,_,_ in calls(source,'CreateStance') if 'internalName: "'+r['internal']+'"' in body]
        fields={k:v.strip() for k,v in named(split_args(bodies[0])).items()}
        oldnames[r['internal']]=literal(fields['displayName']);continue
    if r['family'] in ('School','Descriptor'):
        method='SchoolFeatDefinition' if r['family']=='School' else 'DescriptorFeatDefinition'
        bodies=[body for body,_,_ in calls(source,method) if 'internalName: "'+r['internal']+'"' in body]
        fields={k:v.strip() for k,v in named(split_args(bodies[0])).items()}
        oldnames[r['internal']]=literal(fields.get('name', fields.get('nameEn', fields.get('displayName', fields.get('nameValue',fields.get('flavorName'))))));continue
    if r['family'] in ('Conditional','Derived','Summon','Sacrifice','Distance'):
        method='NewFeature' if r['family'] in ('Conditional','Derived') else 'CreateFeat'
        bodies=[body for body,_,_ in calls(source,method) if 'internalName: "'+r['internal']+'"' in body]
        fields={k:v.strip() for k,v in named(split_args(bodies[0])).items()}
        oldnames[r['internal']]=literal(fields.get('name', fields.get('nameEn', fields.get('displayName', fields.get('nameValue',fields.get('flavorName'))))));continue
    method={'Main':'CreateOne','Weapon Insight':'CreateWeaponInsight','Extended':'CreateExtendedFeat'}.get(r['family'],'CreateSpecialized')
    body=next(body for body,_,_ in calls(source,method) if '"'+r['internal']+'"' in body)
    args=split_args(body)
    internal_i=next(i for i,a in enumerate(args) if a=='"'+r['internal']+'"')
    after=[literal(a) for a in args[internal_i+1:] if a.startswith('"')]
    oldnames[r['internal']]=after[1] if r['family'] in ('Main','Weapon Insight','Extended') else after[0]

readme=(ROOT/'README.md').read_text(encoding='utf-8')
readme=re.sub(r'## Text and Icon Audit\n.*?(?=## Changelog)', '', readme, flags=re.S)
readme=readme.replace('**0.1.1 total:**','**0.1.2 total:**').replace('AttributeFeats-0.1.1.zip','AttributeFeats-0.1.2.zip')
families=['Defensive','Maneuver','Skilled','Arcane']
for family in families:
    members=[r for r in rows if r['family']==family]
    line='| **'+family+'** | '+' | '.join(r['en']+'<br>('+r['zh']+')' for r in members)+' |'
    readme=re.sub(r'^\| \*\*'+family+r'\*\*.*$',lambda _:line,readme,flags=re.M)
readme=readme.replace('Iron Bastion Posture','Colossus Bastion')
readme=readme.replace('| Oath of Retribution | 复仇血誓 | Charisma | When hit |','| Oath of Retribution | 复仇血誓 | Charisma | Ally dies within 30 meters; 3 rounds |')
readme=readme.replace('| Crane\'s Severance | 蓄势孤峰 | Wisdom | Missed attack |',"| Crane's Severance | 蓄势孤峰 | Wisdom | First weapon attack each round, including misses |")
readme=readme.replace('| Cadence Decoded | 阅破机宜 | Intelligence | First weapon attack resolves |','| Cadence Decoded | 阅破机宜 | Intelligence | Weapon attack resolves; up to 10 minutes, removed at combat boundaries |')
readme=readme.replace('Con-to-HP scaling','Con modifier to HP (minimum 0; added once)')
readme=readme.replace('at ≥ 30 ft.','at > 29 ft.').replace('at 15–25 ft.','at > 14 ft. and ≤ 25 ft.')
readme=readme.replace('**Polearm Master (长柄武器宗师)** — reach × 2 with the −4 weapon damage tradeoff.', '**Long-Reach Gambit (长锋险势)** — reach × 2 with −4 weapon damage; applies to all weapon categories.')
readme=readme.replace('Half BAB to Spell DC.','Half BAB to spell and ability save DC.')
readme=readme.replace('At combat start, gain temporary HP equal to Caster Level.','At combat start, gain temporary HP equal to Caster Level, for up to 10 minutes or until combat ends.')
readme=readme.replace('## Changelog','## Text and Icon Audit\n\nAll 92 feats have separate 128×128 PNG icons. Final names and short lore live in the embedded [FeatText.json](Localization/FeatText.json); rule templates remain with their implementations. See the [complete before/after catalog and validation limits](docs/P-831-review.md), [individual image prompts](docs/icon-manifest.json), and [icon contact sheet](docs/feat-icons.png).\n\nThe settings UI and full rule-template resource migration remain in OSS-142 (formerly P-832). Real-game mechanism verification remains in OSS-59 (formerly P-812); source inspection and successful builds do not establish in-game effects.\n\n## Changelog')
(ROOT/'README.md').write_text(readme,encoding='utf-8')

lines=['# P-831 / OSS-77 final text and icon audit','',
'Scope: 92 existing feats and their visible buffs/toggles. Baseline published text: `d4de92a` (0.1.1); inherited AGY text: `6887a8a`. Internal names, GUIDs, component values and triggers are preserved. The final names and lore are in `Localization/FeatText.json`; rules remain separate in the family builders.','',
'## Representative changes','',
'| Mechanism | Before | Final description |','|---|---|---|',
'| Main scaling | Full modifier implied for every bonus | Balanced DC/BAB/power uses half, rounded down; Legacy_AllFull uses full; minimum 0 |',
'| Conditional | README said being hit / missing | Ally death within 30 meters / first weapon attack even on a miss |',
'| Intelligence stance | Ordinary attacks claimed to take an INT penalty | Current inverse rank is clamped at 0, producing no penalty for positive INT; this existing mechanism issue is explicitly exposed |',
'| Distance | 30+ / 15–25 feet | Actual continuous thresholds: >29 / >14 and ≤25 feet |',
'| Reach | Polearm Master implies weapon restriction | Long-Reach Gambit: any weapon category, doubled reach and −4 damage |',
'| Summoning | Grandiose transformation or official divine approval implied | Short original pact imagery, with no size change or religious prerequisite implied |','',
'## Series and terminology','',
'All 92 English titles and all 92 Chinese titles are unique. Repeated names on a feat, its toggle, and its buff intentionally identify one ability. CMB/CMD use 战技加值/战技防御; weapon attack substitution is distinguished from the additive Extended family. Mutex restrictions depend on EnableMutex. Negative modifiers are clamped where the source rank config does so; the Intelligence stance inverse rank is documented separately.','',
'Golarion references are inspiration, not requirements or new official canon. Gorum does not teach abandoning armor: [Archives of Nethys](https://aonprd.com/DeityDisplay.aspx?ItemName=Gorum). Irori is associated with self-perfection: [Archives of Nethys](https://aonprd.com/DeityDisplay.aspx?ItemName=Irori). Aldori inspiration is grounded in [Aldori Defender](https://aonprd.com/ArchetypeDisplay.aspx?FixedName=Fighter%20Aldori%20Defender). Boneyard imagery does not assert Pharasma endorses necromancy. Hercules attribution was removed. All other descriptive practices are original flavor.','',
'## Validation boundary and mechanism follow-ups','',
'OSS-59 (formerly P-812) is still Backlog, with no completed real-game effect report. This work verifies source correspondence, text resources, fallback behavior, blueprint identity, assets, compilation and packaging. Locale switching, visual font/layout behavior and actual combat effects still require the real game. OSS-142 (formerly P-832) owns settings UI and remaining template resource migration.','',
'Existing mechanism concerns preserved for separate verification: reactive armor UnitArmor conditions omit a Unit evaluator (installed BPC/game metadata indicate a null evaluator makes the condition false); Intelligence stance inverse scaling is clamped after multiplication; stance toggles lack an activation group when learning mutex is disabled; Charisma ally aura scaling is attached to the ally and needs caster-source verification. Text does not claim these have passed runtime tests.','',
'Reactive armor condition investigation is tracked as [OSS-289](https://linear.app/acturea/issue/OSS-289/attributefeats验证并修复护甲反应专长的-unitarmor-条件上下文), a child of OSS-77.','',
'Descriptor specialists match specific groups and apply one penalty per other matching group; multi-tag spells and abilities can combine bonuses and penalties. Positive matching group is Cure/RestoreHP/ChannelPositiveHeal/ChannelPositiveHarm; negative matching group is ChannelNegativeHeal/ChannelNegativeHarm/NegativeLevel. School and descriptor DC/caster-level changes are settings gated.','',
'## Complete coverage and name comparison','',
'| Family | Stable internal name | Published English name | Final EN / zhCN |','|---|---|---|---|']
for r in rows:lines.append(f"| {r['family']} | `{r['internal']}` | {oldnames[r['internal']]} | {r['en']} / {r['zh']} |")
lines += ['','## Associated visible objects','', 'There are 35 buffs and 6 stance toggles. They share the parent feat name and artwork. No ordinary Ability blueprint is created. The 92 feats plus these 41 visible objects, a Charisma aura area, and a skill-rank property total 135 blueprint identities.','', '| Parent internal name | Visible associated identities |', '|---|---|']
for r in rows:
    x=r['internal'];f=r['family'];objects=[]
    if f=='Stance':objects=[x+'Buff',x+'Activatable']
    elif f=='Conditional':objects=[x+'TriggerBuff']
    elif f in ('Summon','Sacrifice'):objects=[x+'OuterBuff',x+'InnerBuff']
    elif x=='SoulBulwark':objects=[x+'TriggerBuff',x+'TempBuff']
    elif x=='BulwarkOfSteel':objects=[x+'Buff']
    if x=='CommandingPresence':objects.append(x+'AllyBuff')
    if objects:lines.append('| `'+x+'` | '+', '.join('`'+o+'`' for o in objects)+' |')
lines += ['| All three Distance feats | `DistanceDamageFlatBonusBuff` (Distance Damage / 距离伤害; shares AggressorsEdge artwork) |','', 'Internal helpers: `CommandingPresenceArea`, `SkilledDefenderSkillRanksProperty`. Other feat families have no additional visible buff or toggle blueprints.','']
lines += ['','## Lore before/after (inherited AGY draft → final resources)','',
'Every row includes the previously inherited English/Chinese lore and the final shortened equivalent; these are audit evidence, not alternative player text.','']
resources={e['Key']:e for e in json.loads((ROOT/'Localization/FeatText.json').read_text(encoding='utf-8'))}
for r in rows:
    p=prior[r['internal']];e=resources[r['key']+'.Lore']
    lines += ['### '+r['en']+' / '+r['zh'],'', '**Before EN:** '+p['lore_en'],'', '**Before ZH:** '+p['lore_zh'],'', '**Final EN:** '+e['enGB'],'', '**Final ZH:** '+e['zhCN'],'']
(ROOT/'docs/P-831-review.md').write_text('\n'.join(lines).rstrip()+'\n',encoding='utf-8')
print('Updated README and 92-row name/lore comparison.')
