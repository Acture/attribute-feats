"""Prepare separate built-in image generation prompts; does not call an image API."""
import json
import hashlib
import re
from pathlib import Path
from feat_catalog import ROOT, catalog

SUBJECTS = '''
ApexPredator|a massive rune-engraved gauntlet raised amid amber sparks
EmbodiedGrace|a flowing silver humanoid silhouette leaving a curved silver motion trail
LivingBulwark|an adamantine armored torso lit from cracks by amber vitality
ArchitectOfSelf|a luminous blue anatomical silhouette within fine geometric diagrams
WellspringOfInsight|a cosmic eye over rippling still dark water
CrownOfWill|a commanding gold crown floating above shadowed shoulders
CrushingForm|a heavy warhammer head crashing against a steel guard, amber impact sparks
DuelistsEye|a gloved hand holding an elegant curved dueling sword with a precise silver arc
IronStance|a leather boot planted into solid stone, a sword hilt firmly gripped above it
TacticalStrike|a steel blade intersecting a few luminous geometric lines at a measured angle
PredictiveCut|a poised blade meeting the faint ghost outline of an approaching blade
TheatricalCombat|a sweeping rapier and a rich crimson cloak curling through the dark
InnerSentinel|an open unarmored hand held in calm guard before a faint blue circular ward
CalculatedGrip|a gloved hand gripping an armored wrist at a joint, fine amber anatomical lines
UnyieldingWill|a dignified armored figure standing upright under a restrained golden crown motif
BrutalDefender|a massive armored boot braced against a cracked stone floor
LightfootDefense|a light leather boot gliding between two blurred blade points, cool teal wind
IronEndurance|a warm glowing heart enclosed by weathered riblike steel plates
TitansStance|a massive weathered tower shield braced by a muscular armored forearm, amber impact sparks
FlowingForm|a slender cloaked duelist twisting around an incoming blade, teal fabric motion
IronBulwark|a scarred steel breastplate with overlapping robust plates and warm highlights
CalculatedDefense|a steel shield overlaid by sparse luminous blue trajectory vectors
StoicVigilance|a calm watchful eye above an upright protective hand, pale blue aura
IndomitablePresence|a regal gold-trimmed breastplate whose posture confronts an approaching sword
CrushingGrip|a powerful leather-gloved fist gripping a bent steel ring, amber light
DeftHand|two dexterous gloved fingers precisely turning a sword guard aside, silver light
UnyieldingHold|an armored hand holding an iron chain taut against a rooted stone pedestal
TacticalBind|a gloved hand manipulating a single articulated armored elbow, fine blue pivot lines
PredictiveLock|a crane-shaped silver ornament beside two crossing wrists in a precise grappling hold
DomineeringThrow|a forceful gauntlet pushing a shield off balance, red-gold motion
PracticedHand|a powerful craftsman's hand holding a broad smithing hammer over a dark anvil
EffortlessSkill|a nimble gloved hand turning an ornate lockpick in a dark iron lock
TirelessPractice|a worn hand writing by low candlelight beside a weathered tool
PolymathsTouch|an open aged book, a brass compass and a small glass alchemical vial, blue light
QuietMastery|a traveler's hand resting on an old walking staff beside a weathered map
InspiredVersatility|a silver theatrical half-mask with an elegant scroll ribbon, warm gold light
SpellForgedWill|a smithing hammer striking a blue glowing rune into dark iron
QuickcastReflex|a slender hand forming a precise spell gesture, cyan light curling between fingers
SpellTemperedBody|an armored forearm holding a purple magical flame steady above an open palm
ScholarOfTheWeave|an open dark leather spellbook with layered luminous blue diagrams above its pages
OraclesIntuition|a serene hand touching a softly glowing blue leyline thread
SorcerousPresence|a gold signet ring above a violet magical sigil that radiates outward
BrutalStance|a heavy red-lit sword in a forceful downward swing
LiquidForm|a willow branch curving around a silver blade in teal wind
EndlessVigor|an armored foot rooted into a mountain ledge, amber glow
TacticalMind|a chess rook and crossed blade under fine blue strategic lines
CenteredMind|a blade mirrored in perfectly still midnight blue water
CommandingPresence|a gold-edged crimson battle banner over dark armored shoulders
EndlessResolve|a battered shield held steady at the edge of a dark precipice
FirstBlood|a poised dagger beside a coiled emerald viper, red light at the blade tip
Vendetta|a clenched gauntlet holding a fallen companion's broken sword, red-gold embers
PatientHunter|a long poised blade with a silver crane feather, calm blue light
BerserkersLastStand|a battered red-lit gauntlet gripping a notched greatsword amid fading embers
TacticalReading|a watchful eye above two crossed blades with a few precise blue diagram lines
ArcaneAegis|a luminous blue translucent magical shield surrounding a dark steel pauldron
MartialInsight|a worn steel helm and three restrained silver arcs indicating practiced awareness
SkilledDefender|an aged open field manual beneath a defensively angled steel blade
MysticVitality|a warm glowing heart wrapped by branching blue leyline threads
SoulBulwark|a golden soul-shaped flame sheltered inside a dark blue spectral breastplate
SwordSaint|a steel sword blade inscribed with luminous violet spell geometry
BloodlineOfBeasts|a broad spectral beast paw rising from a gold-lit summoning circle
QuickenedPact|a lean translucent wolf paw stepping through a teal summoning circle, wind trail
VitalPact|a sturdy spectral beast torso with a warm glowing heart within a green summoning circle
TacticalBinding|a spectral creature's head sheltered inside an angular blue magical ward
InsightfulSummons|a summoner's open hand meeting a spectral beast forehead, calm blue connecting light
MagneticCalling|a gold-tipped staff commanding a shadowy summoned beast beneath a warm sigil
BodyOfMyPact|a weary bare hand passing a single crimson light into a spectral beast paw
DoubledBond|two linked hands and a spectral beast paw around a violet glowing arcane crucible
EmpoweredSacrifice|a tarnished gold crown shedding red light into a powerful spectral beast forelimb
AggressorsEdge|a dagger pressing past a steel shield at close range, bright amber impact
MarksmansFocus|a taut dark longbow and single silver arrow aimed toward a distant amber horizon
OptimalRange|a sweeping poleblade crossing a single blue glowing curved distance arc
PureWarder|an upright open palm before a layered translucent blue abjuration ward
MasterCaller|a gold-ringed magical doorway opening into deep violet space
SeersEdge|a crystal eye above a brass divination lens, fine violet patterns
HeartsTyrant|an ornate golden heart-shaped pendant entwined by soft violet magic threads
Spellforge|a controlled blazing orange magical orb above a dark stone focus
Veilweaver|a silver half-mask emerging from violet illusion smoke, delicate luminous folds
DeathSpeaker|a pale skull beside a dim blue spirit lantern, respectful somber mood
ShapeShifter|a sculptor's hand shaping a silver cube into an organic leaf, violet energy
InnerFlame|a single curling golden phoenix-shaped flame above dark embers
FrozenHeart|a faceted glacial crystal resting over a frost-covered gauntlet, icy blue light
StormChannel|a dexterous gloved hand guiding one branching blue lightning arc
EtchingMind|a green glass alchemical vial and a small engraved copper plate being corroded
ResonantVoice|a dark horn emitting three bright golden concentric sound ripples
EthericMind|a violet translucent force sphere bending around a floating steel fragment
RadiantSoul|a radiant golden sun-shaped orb cradled between two shadowed hands
HollowHeart|a withered branch around a small dark violet flame, muted bone and indigo
SubtleTyrant|a silver head silhouette encircled by fine violet puppet threads from a gloved hand
SpikedDefense|a dark armored shoulder bristling with steel barbs, an enemy blade scraping it
BulwarkOfSteel|a layered steel breastplate with a sturdy gold-lit shield silhouette behind it
PolearmMaster|a long dark poleblade held at maximum extension, an amber arc indicating reach
'''

if __name__ == "__main__":
    subjects = dict(line.split("|", 1) for line in SUBJECTS.strip().splitlines())
    existing = {"ArchitectOfSelf": "ArchitectOfSelf", "ApexPredator": "TitansApotheosis", "EmbodiedGrace": "QuicksilverIncarnate", "LivingBulwark": "AdamantineVessel", "WellspringOfInsight": "OcularOfTheCosmos", "CrownOfWill": "SovereignOfWills", "BrutalStance": "BerserkersOverrun", "LiquidForm": "FlowingReed", "EndlessVigor": "TitansRespiration", "TacticalMind": "GrandmastersGambit", "CenteredMind": "MirrorOfStillWaters", "CommandingPresence": "VanguardsBanner", "EndlessResolve": "DefianceAtThePrecipice"}
    style = "Composition: one bold central silhouette readable at 128x128; close crop, square canvas, full-bleed charcoal background. Style: detailed dark fantasy digital oil painting matching native Pathfinder Wrath of the Righteous feat art, tactile metal, cloth and brushwork, dramatic rim light, restrained palette with luminous accents. Constraints: exactly one icon, no text, no letters, no numbers, no logos, no UI, no border, no frame, no white margins, no multiple panels."
    manifest = []
    imported = {}
    for mapping_file in (ROOT / "artifacts").glob("import*.json"):
        mapping = json.loads(mapping_file.read_text(encoding="utf-8"))
        if isinstance(mapping, list):
            imported.update({item["filename"]: item for item in mapping})
    initial_path = ROOT / "artifacts/initial-prompts.json"
    initial_prompts = json.loads(initial_path.read_text(encoding="utf-8")) if initial_path.exists() else {}
    previous_path = ROOT / "docs/icon-manifest.json"
    previous = {item["filename"]: item for item in json.loads(previous_path.read_text(encoding="utf-8"))} if previous_path.exists() else {}
    for row in catalog():
        internal = row["internal"]
        filename = existing.get(internal, re.sub(r'[^A-Za-z0-9]', '', row["en"])) + ".png"
        prompt = f"Use case: stylized-concept. Asset type: square fantasy RPG feat icon for AttributeFeats. Primary request: {row['en']} ({row['zh']}). Depict {subjects[internal]}. {style}"
        inherited = internal in existing or internal == "ArchitectOfSelf"
        provenance = imported.get(filename, {})
        prior = previous.get(filename, {})
        asset = dict(internal=internal, family=row["family"], filename=filename,
                     nameEn=row["en"], nameZh=row["zh"],
                     prompt=None if inherited else provenance.get("prompt", initial_prompts.get(filename, prior.get("prompt", prompt))),
                     origin="inherited at 6887a8a" if inherited else "built-in image_gen")
        if provenance.get("source"):
            asset["generatedSource"] = provenance["source"].replace("\\", "/").split("/")[-1]
        elif prior.get("generatedSource"):
            asset["generatedSource"] = prior["generatedSource"]
        path = ROOT / "Icons" / filename
        if path.exists():
            asset["sha256"] = hashlib.sha256(path.read_bytes()).hexdigest()
        manifest.append(asset)
    assert len(subjects) == len(manifest) == 92
    (ROOT / "docs").mkdir(exist_ok=True)
    (ROOT / "docs/icon-manifest.json").write_text(json.dumps(manifest, ensure_ascii=False, indent=2) + "\n", encoding="utf-8")
    print("Prepared 92 individual icon prompts.")
