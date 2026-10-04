param([Parameter(Mandatory = $true)][string]$WrathInstallDir)

# Run with Windows PowerShell 5.1 (.NET Framework), matching the mod's runtime.
$ErrorActionPreference = 'Stop'
$root = Split-Path $PSScriptRoot -Parent
$umm = Join-Path $WrathInstallDir 'Wrath_Data/Managed/UnityModManager/UnityModManager.dll'
[void][System.Reflection.Assembly]::LoadFrom($umm)
Add-Type -Path @((Join-Path $root 'src/AttributeFeats/Settings.cs'), (Join-Path $root 'src/AttributeFeats/New_Feats/WeaponDamageRules.cs')) -ReferencedAssemblies @($umm, 'System.Xml.dll')

$serializer = New-Object System.Xml.Serialization.XmlSerializer([AttributeFeats.ModSettings])
$old = $serializer.Deserialize([System.IO.StringReader]::new('<AttributeFeatsSettings><EnableMutex>false</EnableMutex><EnablePowerMode>true</EnablePowerMode></AttributeFeatsSettings>'))
if ($old.WeaponDamage -ne [AttributeFeats.WeaponDamageMode]::Replace -or $old.EnableMutex -ne $false -or $old.EnablePowerMode -ne $true) { throw 'Old settings compatibility failed' }
foreach ($mode in @([AttributeFeats.WeaponDamageMode]::Replace, [AttributeFeats.WeaponDamageMode]::Add)) {
    $old.WeaponDamage = $mode
    $writer = [System.IO.StringWriter]::new()
    $serializer.Serialize($writer, $old)
    $loaded = $serializer.Deserialize([System.IO.StringReader]::new($writer.ToString()))
    if ($loaded.WeaponDamage -ne $mode -or $loaded.EnableMutex -ne $false -or $loaded.EnablePowerMode -ne $true) { throw "Roundtrip failed for $mode" }
}
'PASS: legacy XML defaults to Replace; Replace and Add roundtrip without changing existing settings.'
