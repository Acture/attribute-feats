using AttributeFeats;
using AttributeFeats.New_Feats;

var failures = 0;
var cases = 0;
void Check(string name, WeaponDamageMode mode, int current, int chosen, float multiplier,
    int expected, bool matches = true, bool hasDamageAttribute = true)
{
    cases++;
    var actual = WeaponDamageRules.CalculateAdjustment(mode, matches, hasDamageAttribute,
        current, chosen, multiplier);
    if (actual == expected) return;
    failures++;
    Console.Error.WriteLine($"FAIL {name}: expected adjustment {expected}, got {actual}");
}

Check("replace Str +1 with Int +5", WeaponDamageMode.Replace, 1, 5, 1f, 4);
Check("add Int +5 to Str +1", WeaponDamageMode.Add, 1, 5, 1f, 5);
Check("do not downgrade existing Dexterity damage", WeaponDamageMode.Replace, 7, 5, 1f, 0);
Check("same attribute is not counted twice in replacement", WeaponDamageMode.Replace, 5, 5, 1f, 0);
Check("addition deliberately stacks with existing same attribute", WeaponDamageMode.Add, 5, 5, 1f, 5);
Check("two-handed replacement keeps rounding", WeaponDamageMode.Replace, 1, 5, 1.5f, 6);
Check("off-hand replacement keeps rounding", WeaponDamageMode.Replace, 1, 5, .5f, 2);
Check("additional game multipliers are preserved", WeaponDamageMode.Replace, 1, 5, 2f, 8);
Check("two hands do not multiply the added bonus", WeaponDamageMode.Add, 1, 5, 1.5f, 5);
Check("off hand does not halve the added bonus", WeaponDamageMode.Add, 1, 5, .5f, 5);
Check("negative original modifier is not multiplied", WeaponDamageMode.Replace, -2, 5, 1.5f, 9);
Check("replacement can reduce an existing penalty", WeaponDamageMode.Replace, -3, -1, .5f, 2);
Check("addition ignores negative modifiers", WeaponDamageMode.Add, 1, -2, 1f, 0);
Check("zero addition", WeaponDamageMode.Add, 1, 0, 1f, 0);
foreach (var mode in new[] { WeaponDamageMode.Replace, WeaponDamageMode.Add })
{
    Check($"{mode} ignores other weapons", mode, 1, 5, 1f, 0, matches: false);
    Check($"{mode} does not invent a damage attribute", mode, 0, 5, 1f, 0, hasDamageAttribute: false);
}

Console.WriteLine($"{cases - failures}/{cases} weapon damage checks passed.");
return failures == 0 ? 0 : 1;
