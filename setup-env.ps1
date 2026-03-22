param (
    [string]$Channel = "8.0"
)

Write-Host "========================================="
Write-Host "   Đang tải và cài đặt .NET SDK $Channel   "
Write-Host "========================================="

# Download dotnet-install script
$installScriptUrl = "https://dot.net/v1/dotnet-install.ps1"
$installScriptPath = "$PSScriptRoot\dotnet-install.ps1"

Write-Host "Đang tải script cài đặt từ: $installScriptUrl"
Invoke-WebRequest -Uri $installScriptUrl -OutFile $installScriptPath

# Run the installation script
Write-Host "Đang cài đặt .NET SDK (quá trình này có thể mất vài phút)..."
& $installScriptPath -Channel $Channel

# Add global path
$dotnetPath = "$HOME\AppData\Local\Microsoft\dotnet"
$env:Path = "$env:Path;$dotnetPath"

Write-Host ""
Write-Host "========================================="
Write-Host "Cài đặt thành công!"
Write-Host "Thư mục cài đặt: $dotnetPath"
Write-Host "Để có thể chạy được dự án, bạn hãy đóng và mở lại terminal/VS Code để nhận diện lệnh 'dotnet'."
Write-Host "Sau đó chạy lệnh: dotnet run"
Write-Host "========================================="
