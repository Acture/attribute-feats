$ErrorActionPreference = 'Stop'
Add-Type -AssemblyName System.Drawing
$taskRoot = [IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..'))
$taskAssets = Get-Content -LiteralPath (Join-Path $taskRoot 'doc/icon-manifest.json') -Raw -Encoding UTF8 | ConvertFrom-Json
$taskColumns = 12
$taskCellWidth = 164
$taskCellHeight = 192
$taskRows = [int][Math]::Ceiling($taskAssets.Count / $taskColumns)
$taskSheet = New-Object Drawing.Bitmap ($taskColumns * $taskCellWidth),($taskRows * $taskCellHeight + 56)
$taskGraphics = [Drawing.Graphics]::FromImage($taskSheet)
$taskFont = New-Object Drawing.Font 'Microsoft YaHei',9
$taskTitleFont = New-Object Drawing.Font 'Microsoft YaHei',16
$taskBrush = New-Object Drawing.SolidBrush ([Drawing.Color]::FromArgb(230,225,214))
$taskFormat = New-Object Drawing.StringFormat
$taskFormat.Alignment = [Drawing.StringAlignment]::Center
try {
    $taskGraphics.Clear([Drawing.Color]::FromArgb(20,23,28))
    $taskGraphics.TextRenderingHint = [Drawing.Text.TextRenderingHint]::AntiAliasGridFit
    $taskGraphics.DrawString('AC''s Homebrew · 92 个独立专长图标 · 128 × 128', $taskTitleFont, $taskBrush, 18, 12)
    for ($taskIndex = 0; $taskIndex -lt $taskAssets.Count; $taskIndex++) {
        $taskAsset = $taskAssets[$taskIndex]
        $taskX = ($taskIndex % $taskColumns) * $taskCellWidth
        $taskY = [Math]::Floor($taskIndex / $taskColumns) * $taskCellHeight + 56
        $taskIcon = [Drawing.Image]::FromFile((Join-Path $taskRoot ('src/ACHomebrew/Icons/' + $taskAsset.filename)))
        try { $taskGraphics.DrawImageUnscaled($taskIcon, [int]($taskX + 18), [int]$taskY) }
        finally { $taskIcon.Dispose() }
        $taskLabel = $taskAsset.nameEn + "`n" + $taskAsset.nameZh
        $taskRect = New-Object Drawing.RectangleF ($taskX + 3),($taskY + 131),158,57
        $taskGraphics.DrawString($taskLabel, $taskFont, $taskBrush, $taskRect, $taskFormat)
    }
    $taskSheet.Save((Join-Path $taskRoot 'doc/feat-icons.png'), [Drawing.Imaging.ImageFormat]::Png)
} finally {
    $taskFormat.Dispose(); $taskBrush.Dispose(); $taskFont.Dispose(); $taskTitleFont.Dispose()
    $taskGraphics.Dispose(); $taskSheet.Dispose()
}
Write-Output 'doc/feat-icons.png'
