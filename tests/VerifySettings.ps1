param([Parameter(Mandatory = $true)][string]$WrathInstallDir)

# Run with Windows PowerShell 5.1 (.NET Framework), matching the mod's runtime.
$ErrorActionPreference = 'Stop'
$root = Split-Path $PSScriptRoot -Parent
$umm = Join-Path $WrathInstallDir 'Wrath_Data/Managed/UnityModManager/UnityModManager.dll'
[void][System.Reflection.Assembly]::LoadFrom($umm)
Add-Type -Path @((Join-Path $root 'src/AttributeFeats.Core/Settings.cs'), (Join-Path $root 'src/AttributeFeats.Rules/WeaponDamageRules.cs')) -ReferencedAssemblies @($umm, 'System.Xml.dll')

$serializer = New-Object System.Xml.Serialization.XmlSerializer([AttributeFeats.ModSettings])
$old = $serializer.Deserialize([System.IO.StringReader]::new('<AttributeFeatsSettings><EnableMutex>false</EnableMutex><EnablePowerMode>true</EnablePowerMode></AttributeFeatsSettings>'))
if ($old.WeaponDamage -ne [AttributeFeats.WeaponDamageMode]::Replace -or $old.EnableMutex -ne $false -or $old.EnablePowerMode -ne $true) { throw 'Old settings compatibility failed' }
if ($old.EnableFeatCountLimit -or $old.EnableFeatPointLimit -or $old.MaxFeatCount -ne 6 -or $old.MaxFeatPoints -ne 10) { throw 'Old settings did not default to an unlimited feat budget' }
if ($old.FeatGroups -eq $null -or $old.FeatGroups.Count -ne 0) { throw 'Old settings did not default to built-in exclusion groups' }
$old.EnableFeatCountLimit = $true; $old.MaxFeatCount = 4; $old.EnableFeatPointLimit = $true; $old.MaxFeatPoints = 7
$group = New-Object AttributeFeats.FeatGroupSetting; $group.Id = 'Conditional'; $group.Enabled = $false; $group.Max = 3; $old.FeatGroups.Add($group)
foreach ($mode in @([AttributeFeats.WeaponDamageMode]::Replace, [AttributeFeats.WeaponDamageMode]::Add)) {
    $old.WeaponDamage = $mode
    $writer = [System.IO.StringWriter]::new()
    $serializer.Serialize($writer, $old)
    $loaded = $serializer.Deserialize([System.IO.StringReader]::new($writer.ToString()))
    if ($loaded.WeaponDamage -ne $mode -or $loaded.EnableMutex -ne $false -or $loaded.EnablePowerMode -ne $true) { throw "Roundtrip failed for $mode" }
    if ($loaded.FeatGroups.Count -ne 1 -or $loaded.FeatGroups[0].Id -ne 'Conditional' -or $loaded.FeatGroups[0].Enabled -or $loaded.FeatGroups[0].Max -ne 3) { throw "Exclusion group roundtrip failed for $mode" }
    if (-not $loaded.EnableFeatCountLimit -or $loaded.MaxFeatCount -ne 4 -or -not $loaded.EnableFeatPointLimit -or $loaded.MaxFeatPoints -ne 7) { throw "Feat budget roundtrip failed for $mode" }
}
'PASS: legacy XML defaults to Replace with feat budget limits off; damage modes, budget limits and exclusion groups roundtrip without changing existing settings.'
