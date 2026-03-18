# ASCII Art
Write-Host "   ____ _                         _     " -ForegroundColor Cyan
Write-Host "  / ___| |__  _ __ ___  _ __ ___ (_)___ " -ForegroundColor Cyan
Write-Host " | |   | '_ \| '__/ _ \| '_ ` _ \| / __|" -ForegroundColor Cyan
Write-Host " | |___| | | | | | (_) | | | | | | \__ \" -ForegroundColor Cyan
Write-Host "  \____|_| |_|_|  \___/|_| |_| |_|_|___/" -ForegroundColor Cyan
Write-Host

# 定义
$scriptDir = $PSScriptRoot # 脚本路径

# 读取 Chromis.vbproj
$projectFile = Join-Path $scriptDir "Chromis.vbproj"
$content = Get-Content -Path $projectFile -Raw -ErrorAction Stop

# 正则匹配版本号
$pattern = '<Version>(.*?)</Version>'
$match = [regex]::Match($content, $pattern)

# 提取版本号并去除前后空格
$version = $match.Groups[1].Value.Trim()
if ([string]::IsNullOrWhiteSpace($version)) {
    Write-Error "Version is empty" -ForegroundColor Red
    Start-Sleep -Seconds 5 # 自动退出
    exit 1
}

# 构建标签名
$tagName = "v$version"
Write-Host "Version: $version"

# 检查本地与远程版本号
$localExists = $false
git rev-parse -q --verify "refs/tags/$tagName" > $null 2>&1
if ($LASTEXITCODE -eq 0) {
    $localExists = $true
    Write-Host "Deleting local version..."
    git tag -d $tagName
}
$remoteExists = $false
git ls-remote --exit-code origin "refs/tags/$tagName" > $null 2>&1
if ($LASTEXITCODE -eq 0) {
    $remoteExists = $true
    Write-Host "Deleting remote version..."
    git push origin :refs/tags/$tagName
}

git tag $tagName
git push origin $tagName

Write-Host "Nuget publish successfully." -ForegroundColor Green
Start-Sleep -Seconds 5 # 自动退出