$ErrorActionPreference = 'SilentlyContinue'

$raw = [Console]::In.ReadToEnd()
if ([string]::IsNullOrWhiteSpace($raw)) { exit 0 }

try { $payload = $raw | ConvertFrom-Json } catch { exit 0 }

$paths = New-Object System.Collections.Generic.List[string]
$ti = $payload.tool_input
if ($ti) {
    if ($ti.file_path) { $paths.Add([string]$ti.file_path) }
    if ($ti.edits) { foreach ($e in $ti.edits) { if ($e.file_path) { $paths.Add([string]$e.file_path) } } }
}

$targets = $paths | Where-Object { $_ -and ($_ -match '\.cs$') } | Select-Object -Unique
if (-not $targets) { exit 0 }

$cwd = $payload.cwd
if (-not $cwd) { $cwd = (Get-Location).Path }

Push-Location $cwd
try {
    foreach ($f in $targets) {
        if (Test-Path -LiteralPath $f) {
            & dotnet csharpier format $f *> $null
        }
    }
} finally {
    Pop-Location
}

exit 0
