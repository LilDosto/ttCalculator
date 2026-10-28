$ErrorActionPreference = "Stop"
Add-Type -AssemblyName System.Drawing

function Create-IconBitmap([int]$size) {
    $bmp = New-Object System.Drawing.Bitmap($size, $size, [System.Drawing.Imaging.PixelFormat]::Format32bppArgb)
    $g = [System.Drawing.Graphics]::FromImage($bmp)
    $g.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::HighQuality
    $g.InterpolationMode = [System.Drawing.Drawing2D.InterpolationMode]::HighQualityBicubic
    $g.PixelOffsetMode = [System.Drawing.Drawing2D.PixelOffsetMode]::HighQuality
    $g.TextRenderingHint = [System.Drawing.Text.TextRenderingHint]::AntiAliasGridFit
    $g.Clear([System.Drawing.Color]::Transparent)

    $s = [float]$size / 256.0

    # 1. Rounded Square Background
    $margin = 12.0 * $s
    $rectSize = $size - (2.0 * $margin)
    $radius = 54.0 * $s
    $d = $radius * 2.0

    $path = New-Object System.Drawing.Drawing2D.GraphicsPath
    $path.AddArc($margin, $margin, $d, $d, 180, 90)
    $path.AddArc($margin + $rectSize - $d, $margin, $d, $d, 270, 90)
    $path.AddArc($margin + $rectSize - $d, $margin + $rectSize - $d, $d, $d, 0, 90)
    $path.AddArc($margin, $margin + $rectSize - $d, $d, $d, 90, 90)
    $path.CloseFigure()

    $rect = New-Object System.Drawing.RectangleF($margin, $margin, $rectSize, $rectSize)
    $bgBrush = New-Object System.Drawing.Drawing2D.LinearGradientBrush(
        $rect,
        [System.Drawing.Color]::FromArgb(255, 17, 24, 39),
        [System.Drawing.Color]::FromArgb(255, 10, 15, 29),
        45.0
    )
    $g.FillPath($bgBrush, $path)

    # Subtle inner glow / grid lines clipped to rounded rect
    $g.SetClip($path)

    $gridPen = New-Object System.Drawing.Pen([System.Drawing.Color]::FromArgb(22, 56, 189, 248), [Math]::Max(1.0, 1.5 * $s))
    for ($i = 1; $i -le 4; $i++) {
        $pos = $margin + ($rectSize * $i / 5.0)
        $g.DrawLine($gridPen, $pos, $margin, $pos, $margin + $rectSize)
        $g.DrawLine($gridPen, $margin, $pos, $margin + $rectSize, $pos)
    }

    # 2. Glowing Sine Wave / Graph Curve across the background
    $curvePts = New-Object 'System.Collections.Generic.List[System.Drawing.PointF]'
    for ($x = 0; $x -le 100; $x++) {
        $px = $margin + ($x / 100.0) * $rectSize
        $t = ($x / 100.0) * [Math]::PI * 2.2 - 0.5
        $py = ($size * 0.62) - [Math]::Sin($t) * (36.0 * $s) - ($x / 100.0) * (24.0 * $s)
        $curvePts.Add((New-Object System.Drawing.PointF($px, $py)))
    }
    $ptsArray = $curvePts.ToArray()

    # Glow layers
    $glowPen1 = New-Object System.Drawing.Pen([System.Drawing.Color]::FromArgb(35, 14, 165, 233), (18.0 * $s))
    $glowPen1.StartCap = [System.Drawing.Drawing2D.LineCap]::Round
    $glowPen1.EndCap = [System.Drawing.Drawing2D.LineCap]::Round
    $g.DrawCurve($glowPen1, $ptsArray)

    $glowPen2 = New-Object System.Drawing.Pen([System.Drawing.Color]::FromArgb(80, 56, 189, 248), (10.0 * $s))
    $glowPen2.StartCap = [System.Drawing.Drawing2D.LineCap]::Round
    $glowPen2.EndCap = [System.Drawing.Drawing2D.LineCap]::Round
    $g.DrawCurve($glowPen2, $ptsArray)

    $curvePen = New-Object System.Drawing.Pen([System.Drawing.Color]::FromArgb(240, 56, 189, 248), [Math]::Max(1.5, 4.5 * $s))
    $curvePen.StartCap = [System.Drawing.Drawing2D.LineCap]::Round
    $curvePen.EndCap = [System.Drawing.Drawing2D.LineCap]::Round
    $g.DrawCurve($curvePen, $ptsArray)

    # 3. Matrix Brackets [ ] on left and right inside
    $bracketPen = New-Object System.Drawing.Pen([System.Drawing.Color]::FromArgb(140, 129, 140, 248), [Math]::Max(1.5, 5.0 * $s))
    $bTop = 52.0 * $s
    $bBot = 204.0 * $s
    $bLeft = 40.0 * $s
    $bRight = 216.0 * $s
    $bTick = 16.0 * $s

    # Left bracket
    $g.DrawLine($bracketPen, $bLeft + $bTick, $bTop, $bLeft, $bTop)
    $g.DrawLine($bracketPen, $bLeft, $bTop, $bLeft, $bBot)
    $g.DrawLine($bracketPen, $bLeft, $bBot, $bLeft + $bTick, $bBot)

    # Right bracket
    $g.DrawLine($bracketPen, $bRight - $bTick, $bTop, $bRight, $bTop)
    $g.DrawLine($bracketPen, $bRight, $bTop, $bRight, $bBot)
    $g.DrawLine($bracketPen, $bRight, $bBot, $bRight - $bTick, $bBot)

    # 4. Main Center Typography "f(x)" + Integral/Function accent
    $fHook = [char]0x0192
    $fText = "${fHook}(x)"
    if ($size -ge 32) {
        $fontMain = New-Object System.Drawing.Font("Segoe UI", (76.0 * $s), ([System.Drawing.FontStyle]::Bold -bor [System.Drawing.FontStyle]::Italic), [System.Drawing.GraphicsUnit]::Pixel)
        $sf = New-Object System.Drawing.StringFormat
        $sf.Alignment = [System.Drawing.StringAlignment]::Center
        $sf.LineAlignment = [System.Drawing.StringAlignment]::Center

        # Drop shadow
        $shadowBrush = New-Object System.Drawing.SolidBrush([System.Drawing.Color]::FromArgb(170, 0, 0, 0))
        $g.DrawString($fText, $fontMain, $shadowBrush, ($size / 2.0) + (3.0 * $s), ($size * 0.43) + (4.0 * $s), $sf)

        $textBrush = New-Object System.Drawing.Drawing2D.LinearGradientBrush(
            (New-Object System.Drawing.PointF(0, ($size * 0.18))),
            (New-Object System.Drawing.PointF(0, ($size * 0.72))),
            [System.Drawing.Color]::FromArgb(255, 255, 255, 255),
            [System.Drawing.Color]::FromArgb(255, 186, 230, 253)
        )
        $g.DrawString($fText, $fontMain, $textBrush, ($size / 2.0), ($size * 0.43), $sf)

        # Small badge "ttCalc" at bottom
        $fontSub = New-Object System.Drawing.Font("Consolas", (28.0 * $s), [System.Drawing.FontStyle]::Bold, [System.Drawing.GraphicsUnit]::Pixel)
        $subBrush = New-Object System.Drawing.SolidBrush([System.Drawing.Color]::FromArgb(240, 56, 189, 248))
        $g.DrawString("ttCalc", $fontSub, $subBrush, ($size / 2.0), ($size * 0.79), $sf)

        $fontMain.Dispose()
        $fontSub.Dispose()
        $shadowBrush.Dispose()
        $textBrush.Dispose()
        $subBrush.Dispose()
        $sf.Dispose()
    } else {
        $fontSmall = New-Object System.Drawing.Font("Segoe UI", (115.0 * $s), ([System.Drawing.FontStyle]::Bold -bor [System.Drawing.FontStyle]::Italic), [System.Drawing.GraphicsUnit]::Pixel)
        $sf = New-Object System.Drawing.StringFormat
        $sf.Alignment = [System.Drawing.StringAlignment]::Center
        $sf.LineAlignment = [System.Drawing.StringAlignment]::Center
        $g.DrawString("$fHook", $fontSmall, [System.Drawing.Brushes]::White, ($size / 2.0), ($size * 0.48), $sf)
        $fontSmall.Dispose()
        $sf.Dispose()
    }

    $g.ResetClip()

    # Outer glowing border
    $borderBrush = New-Object System.Drawing.Drawing2D.LinearGradientBrush(
        $rect,
        [System.Drawing.Color]::FromArgb(220, 56, 189, 248),
        [System.Drawing.Color]::FromArgb(180, 99, 102, 241),
        45.0
    )
    $borderPen = New-Object System.Drawing.Pen($borderBrush, [Math]::Max(1.5, 6.0 * $s))
    $g.DrawPath($borderPen, $path)

    $borderPen.Dispose()
    $borderBrush.Dispose()
    $bracketPen.Dispose()
    $curvePen.Dispose()
    $glowPen1.Dispose()
    $glowPen2.Dispose()
    $gridPen.Dispose()
    $bgBrush.Dispose()
    $path.Dispose()
    $g.Dispose()

    return $bmp
}

$sizes = @(16, 32, 48, 64, 128, 256)
$pngStreams = @()

foreach ($sz in $sizes) {
    $bmp = Create-IconBitmap $sz
    if ($sz -eq 256) {
        $pngPath = Join-Path $PSScriptRoot "logo_preview.png"
        $bmp.Save($pngPath, [System.Drawing.Imaging.ImageFormat]::Png)
    }
    $ms = New-Object System.IO.MemoryStream
    $bmp.Save($ms, [System.Drawing.Imaging.ImageFormat]::Png)
    $pngStreams += ,@($sz, $ms.ToArray())
    $ms.Dispose()
    $bmp.Dispose()
}

$icoPath = Join-Path $PSScriptRoot "logo.ico"
$fs = [System.IO.File]::Create($icoPath)
$bw = New-Object System.IO.BinaryWriter($fs)

# ICONDIR Header
$bw.Write([uint16]0) # Reserved
$bw.Write([uint16]1) # Type: 1 = ICO
$bw.Write([uint16]$sizes.Count) # Count

$offset = 6 + (16 * $sizes.Count)

foreach ($entry in $pngStreams) {
    $sz = $entry[0]
    $bytes = $entry[1]
    $wByte = if ($sz -ge 256) { [byte]0 } else { [byte]$sz }
    $hByte = if ($sz -ge 256) { [byte]0 } else { [byte]$sz }

    $bw.Write($wByte)
    $bw.Write($hByte)
    $bw.Write([byte]0) # ColorCount
    $bw.Write([byte]0) # Reserved
    $bw.Write([uint16]1) # Planes
    $bw.Write([uint16]32) # BitCount
    $bw.Write([uint32]$bytes.Length) # BytesInRes
    $bw.Write([uint32]$offset) # ImageOffset
    $offset += $bytes.Length
}

foreach ($entry in $pngStreams) {
    $bytes = $entry[1]
    $bw.Write($bytes)
}

$bw.Flush()
$bw.Close()
$fs.Close()
Write-Host "Multi-resolution ICO and preview PNG generated successfully at $icoPath"
