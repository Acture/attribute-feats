param([Parameter(Mandatory=$true)][string]$MappingFile)
$ErrorActionPreference = 'Stop'
Add-Type -AssemblyName System.Drawing
$taskRoot = [IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..'))
$taskSources = Join-Path $taskRoot 'artifacts/icon-sources'
[IO.Directory]::CreateDirectory($taskSources) | Out-Null
foreach ($taskAsset in (Get-Content -LiteralPath $MappingFile -Raw -Encoding UTF8 | ConvertFrom-Json)) {
    $taskTarget = Join-Path $taskRoot ('src/ACHomebrew/Icons/' + $taskAsset.filename)
    if (Test-Path -LiteralPath $taskTarget) { throw "Refusing to overwrite $taskTarget" }
    Copy-Item -LiteralPath $taskAsset.source -Destination (Join-Path $taskSources $taskAsset.filename)
    $taskImage = [Drawing.Image]::FromFile($taskAsset.source)
    $taskBitmap = New-Object Drawing.Bitmap 128,128
    $taskGraphics = [Drawing.Graphics]::FromImage($taskBitmap)
    try {
        $taskGraphics.CompositingQuality = [Drawing.Drawing2D.CompositingQuality]::HighQuality
        $taskGraphics.InterpolationMode = [Drawing.Drawing2D.InterpolationMode]::HighQualityBicubic
        $taskGraphics.PixelOffsetMode = [Drawing.Drawing2D.PixelOffsetMode]::HighQuality
        $taskGraphics.DrawImage($taskImage, 0, 0, 128, 128)
        $taskBitmap.Save($taskTarget, [Drawing.Imaging.ImageFormat]::Png)
    } finally {
        $taskGraphics.Dispose(); $taskBitmap.Dispose(); $taskImage.Dispose()
    }
    Write-Output $taskAsset.filename
}
