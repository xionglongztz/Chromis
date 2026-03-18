<div align="center">
  <img src="https://raw.githubusercontent.com/xionglongztz/Chromis/refs/heads/master/ChromisBanner.png" alt="Banner"/>
</div>

# Chromis 🐟

Extract dominant colors from images using K-Means, Median Cut, or Octree algorithms.  
Chromis is a lightweight, cross-platform .NET library for color extraction, designed with zero UI dependencies and clean data structures.  

## ✨ Features

- 🎨 Extract dominant colors from images
- ⚡ Multiple algorithms:
  - K-Means
  - Median Cut
  - Octree
- 🧩 Cross-platform (.NET Framework / .NET 6+)
- 🪶 Lightweight and dependency-free public API
- 🔄 Compatible with legacy System.Drawing types (via overloads)  

## 📦 Installation

```bash
dotnet add package Chromis
```
## 🧠 Basic Usage

### C#
```csharp
using System;
using System.Collections.Generic;
using System.Drawing;

public List<Color> GetPixelsFromImage(Image image, int stepCount = 5)
{
    if (stepCount < 1)
        throw new ArgumentOutOfRangeException(nameof(stepCount), "stepCount must greater than 1");
    var pixels = new List<Color>();
    using (var bmp = new Bitmap(image))
    {
        for (int x = 0; x < bmp.Width; x += stepCount)
        {
            for (int y = 0; y < bmp.Height; y += stepCount)
            {
                pixels.Add(bmp.GetPixel(x, y));
            }
        }
    }
    return pixels;
}
```
### Usage
```csharp
using Chromis;

var sampledColors = GetPixelsFromImage(pictureBoxMain.Image);
var rgbColors = new List<RGBColor>();
foreach (var color in sampledColors)
{
    rgbColors.Add(RGBColor.FromRGB(color.R, color.G, color.B));
}
var colorInfos = ColorExtractor.Extract(rgbColors, 10);
foreach (var ci in colorInfos)
{
    Console.WriteLine($"{ci.Color.R}, {ci.Color.G}, {ci.Color.B} - {ci.Ratio:P}");
}
```

### VB.NET
```vbnet
Imports System;
Imports System.Collections.Generic;
Imports System.Drawing;

Public Function GetPixelsFromImage(image As Image, Optional stepCount As Integer = 5) As List(Of Color)
    If stepCount < 1 Then Throw New ArgumentOutOfRangeException(NameOf(stepCount), "stepCount must greater than 1")
    Dim pixels As New List(Of Color)()
    Using bmp = New Bitmap(image)
        For x = 0 To bmp.Width - 1 Step stepCount
            For y = 0 To bmp.Height - 1 Step stepCount
                pixels.Add(bmp.GetPixel(x, y))
            Next
        Next
    End Using
    Return pixels
End Function
```
### Usage
```vbnet
Imports Chromis

Dim pixels As New List(Of ColorExtractor.RGBColor)
For Each sampledColor In GetPixelsFromImage(PictureBoxMain.Image)
    pixels.Add(ColorExtractor.RGBColor.FromRGB(sampledColor.R, sampledColor.G, sampledColor.B))
Next
Dim colorInfos = ColorExtractor.Extract(pixels, 10)
For Each ci In colorInfos
    Console.WriteLine($"{ci.Color.R}, {ci.Color.G}, {ci.Color.B} - {ci.Ratio:P}")
Next
```
 - `R`, `G`, `B`: RGB color values  
 - `Ratio`: Percentage of this color in the image (0 ~ 1)

## 📊 Algorithms

Chromis supports multiple color quantization algorithms:

- **K-Means** – balanced and accurate
- **Median Cut** – fast and classic
- **Octree** – memory efficient

## 📄 License

[Apache-2.0 License](LICENSE.txt)
