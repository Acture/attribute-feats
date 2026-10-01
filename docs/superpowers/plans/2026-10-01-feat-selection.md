# Nested Attribute Feats Implementation Plan

> **For agentic workers:** Execute this focused change in the existing issue worktree. Verify each step before marking it complete.

**Goal:** Organize the existing feats as Attribute Feats -> family -> individual feat in level-up selections.

**Architecture:** Each family owns a feat factory and a collection of its configured blueprints. Family menus are added to one root menu after all feat effects and prerequisites are configured. Only the root is registered in outer feat selections. Polearm Master has no variants and is a direct child of the root.

**Tech Stack:** C# / .NET Framework 4.8, BlueprintCore 2.8.6, Pathfinder: Wrath of the Righteous.

## Scope and constraints

- Two menu levels, followed by the final feat; selecting a feat still costs one feat choice.
- This grouping phase does not change damage, balance, settings, or prerequisites. The subsequently approved standalone damage feature is tracked in [weapon-damage plan](2026-10-01-weapon-damage.md).
- Preserve every existing feat GUID, effect, rank, name, and mutual-exclusion rule.
- Root selection uses Default mode so previously visited families remain selectable. Family selections use OnlyNew mode so owned one-rank feats cannot be purchased again.
- Children retain their Feat group and remain visible inside their menus; suppress their automatic insertion into outer lists.
- Each menu has a permanent, unique GUID. All 133 existing blueprint GUIDs are preserved.
- Work starts from origin/master at d4de92a after the user-requested rebase.

## Menu contents

| Family | Feats |
|---|---:|
| Main Attribute Mastery | 6 |
| Defensive Adept | 6 |
| Maneuver Adept | 6 |
| Skilled Adept | 6 |
| Arcane Adept | 6 |
| Stance | 6 |
| Conditional Trigger | 6 |
| Weapon Insight | 6 |
| Extended Replacement | 6 |
| Greater Summoning | 6 |
| Summoner Sacrifice | 3 |
| Reactive Armor | 2 |
| Derived Stat Conversion | 6 |
| Spell School Specialist | 8 |
| Spell Descriptor Specialist | 9 |
| Distance Damage | 3 |

The root contains these 16 menus plus Polearm Master: 91 grouped feats + 1 direct feat = 92.

## Implementation and validation

- [x] Confirm SkipAddToSelections is available in installed BlueprintCore 2.8.6.
- [x] Verify root registration requires For(selection, updateSelections: true).Configure().
- [x] Verify the game's nested selection checks recurse into eligible children and bypass the ordinary feature rank limit for menus.
- [x] Verify all six vanilla feat-list parents populated by BlueprintCore use Default mode, allowing repeated access. Combat-only FighterFeatSelection remains unaffected.
- [x] Give Root and each family their own instance in New_Feats/FeatSelection.cs, with a per-instance dictionary keyed by GUID.
- [x] Route all 15 existing creation sites to the correct family. Specialized Adept maps its four existing subfamilies explicitly; Weapon Insight/Extended Replacement and School/Descriptor use separate menus.
- [x] Configure families after MutexPass, then configure Root. Keep automatic outer registration disabled for all children; explicitly register only Root.
- [x] Compile against the installed game assemblies without running Deploy.
- [x] Obtain independent review of category coverage, existing GUIDs, effect-preserving changes, and repeated nested selection.
- [x] Update README and CHANGELOG to describe the nested structure.
- [x] Run git diff --check.

Compile command:

```powershell
dotnet msbuild 'attribute feats.csproj' -restore -t:Compile '-p:WrathInstallDir=G:/SteamLibrary/steamapps/common/Pathfinder Second Adventure' '-p:RestoreSources=C:/Users/actur/.nuget/packages' -p:NuGetAudit=false -v:minimal
```

## In-game acceptance checks

1. Restart with the updated mod. Open a general feat choice: only Attribute Feats should appear at the outer level. Expand it to see the 16 family menus and Polearm Master; families should contain the counts above, without duplicate or missing feats.
2. Choose Main Attribute Mastery -> Apex Predator and complete level-up: the character gains that feat for one feat choice.
3. On another feat choice, select a feat from a different family. Also test two choices in one level-up and undo/reselect.
4. With EnableMutex disabled and after restarting, take two different Main feats on successive choices. The same family must remain accessible, but an already-owned feat must not be purchasable again.
5. With EnableMutex enabled, taking a Main feat prevents the other Main feats. Unrelated eligible families remain available.
6. Load a pre-update save: owned feats and effects remain intact, and eligible unowned feats remain accessible through their families.
7. Verify combat-only bonus feat lists have not gained the unrestricted root menu. Select Polearm Master directly from the root and confirm it costs one choice.

Compilation and static review do not establish these in-game UI checks passed. The game has not been launched for this change.
