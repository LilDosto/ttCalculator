
Add-Type -AssemblyName System.Drawing
$width = 256
$height = 256
$bmp = New-Object System.Drawing.Bitmap $width, $height
$g = [System.Drawing.Graphics]::FromImage($bmp)
$g.Clear([System.Drawing.Color]::FromArgb(30,30,30))
$g.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::AntiAlias

$pen = New-Object System.Drawing.Pen ([System.Drawing.Color]::FromArgb(0, 188, 212)), 12
$points = @(
    New-Object System.Drawing.PointF 128, 30
    New-Object System.Drawing.PointF 213, 79
    New-Object System.Drawing.PointF 213, 177
    New-Object System.Drawing.PointF 128, 226
    New-Object System.Drawing.PointF 43, 177
    New-Object System.Drawing.PointF 43, 79
)
$g.DrawPolygon($pen, $points)

$font = New-Object System.Drawing.Font "Segoe UI", 110, [System.Drawing.FontStyle]::Bold
$brush = [System.Drawing.Brushes]::White
$format = New-Object System.Drawing.StringFormat
$format.Alignment = [System.Drawing.StringAlignment]::Center
$format.LineAlignment = [System.Drawing.StringAlignment]::Center
$g.DrawString("M", $font, $brush, 130, 135, $format)

$iconName = "c:/Users/tolga/Desktop/Math/MathApp/MathApp/logo.ico"
$hicon = $bmp.GetHicon()
$icon = [System.Drawing.Icon]::FromHandle([IntPtr]$hicon)

$fs = [System.IO.File]::OpenWrite($iconName)
$icon.Save($fs)
$fs.Close()
$g.Dispose()
$bmp.Dispose()
Write-Host "Logo created successfully at $iconName"
