param(
    [Parameter(Mandatory = $true)][string]$WrathInstallDir,
    [string]$ModAssembly
)

# Run with Windows PowerShell 5.1 against the compiled mod and real game types.
$ErrorActionPreference = 'Stop'
if (!$ModAssembly) { $ModAssembly = Join-Path (Split-Path $PSScriptRoot -Parent) 'artifacts/bin/ACHomebrew/Debug/AttributeFeats.dll' }
$managed = Join-Path $WrathInstallDir 'Wrath_Data/Managed'
foreach ($dependency in @('Assembly-CSharp.dll', 'UnityModManager/UnityModManager.dll', 'UnityModManager/0Harmony.dll')) {
    [void][System.Reflection.Assembly]::LoadFrom((Join-Path $managed $dependency))
}
$mod = [System.Reflection.Assembly]::LoadFrom((Resolve-Path $ModAssembly))
$settingsType = $mod.GetType('ACHomebrew.ModSettings', $true)
$componentType = $mod.GetType('ACHomebrew.Feats.AttributeWeaponDamage', $true)
$settingsField = $mod.GetType('ACHomebrew.Mod', $true).GetField('Settings', [System.Reflection.BindingFlags]'Static,Public')
$settings = [Activator]::CreateInstance($settingsType)
$settingsField.SetValue($null, $settings)
# No Unity runtime is needed to verify mode lookup on an already existing component.
$component = [System.Runtime.Serialization.FormatterServices]::GetUninitializedObject($componentType)
$modeMember = $componentType.GetMember('Mode')[0]
function Read-Mode {
    if ($modeMember -is [System.Reflection.PropertyInfo]) { return $modeMember.GetValue($component, $null) }
    return $modeMember.GetValue($component)
}

foreach ($expected in @('Replace', 'Add', 'Replace')) {
    $settings.WeaponDamage = [Enum]::Parse($settings.WeaponDamage.GetType(), $expected)
    $actual = (Read-Mode).ToString()
    if ($actual -ne $expected) { throw "Existing component did not follow live setting: expected $expected, got $actual" }
}
$settingsField.SetValue($null, $null)
if ((Read-Mode).ToString() -ne 'Replace') { throw 'Missing settings must default to Replace' }
'PASS: existing component follows Replace -> Add -> Replace without recreation; missing settings default to Replace.'
