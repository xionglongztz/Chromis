<div align="center">
  <img src="https://raw.githubusercontent.com/xionglongztz/Chromis/refs/heads/master/ChromisBanner.png" alt="Banner"/>
</div>

# Chromis 🐟

Extract dominant colors from images using K-Means, Median Cut, or Octree algorithms.  
[Chromis](https://www.nuget.org/packages/Chromis) is a lightweight, cross-platform .NET library for color extraction, designed with zero UI dependencies and clean data structures.  

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
//.NET Framework
using System;
using System.Collections.Generic;
using System.Drawing;

public List<Color> GetPixelsFromImage(Image image, int stepCount = 5)
{
    if (stepCount < 1)
        throw new ArgumentOutOfRangeException(nameof(stepCount), "stepCount must be positive");
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
//.NET
using System;
using System.Collections.Generic;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.PixelFormats;

public List<Rgba64> GetPixelsFromImage(Image image, int stepCount = 5)
{
    if (stepCount <= 0)
        throw new ArgumentException("stepCount must be positive");
    var pixels = new List<Rgba64>();
    var rgbaImage = image as Image<Rgba64>;
    bool needDispose = false;
    if (rgbaImage == null)
    {
        rgbaImage = image.CloneAs<Rgba64>();
        needDispose = true;
    }
    try
    {
        var frame = rgbaImage.Frames[0];
        for (int y = 0; y < frame.Height; y += stepCount)
        {
            var row = frame.GetPixelMemoryGroup(y);
            for (int x = 0; x < frame.Width; x += stepCount)
            {
                pixels.Add(row.Span[x]);
            }
        }
    }
    finally
    {
        if (needDispose)
            rgbaImage?.Dispose();
    }
    return pixels;
}
```
### Usage
```csharp
using Chromis;

var sampledColors = GetPixelsFromImage(PictureBox1.Image);
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
'.NET Framework
Imports System;
Imports System.Collections.Generic;
Imports System.Drawing;

Public Function GetPixelsFromImage(image As Image, Optional stepCount As Integer = 5) As List(Of Color)
    If stepCount < 1 Then Throw New ArgumentOutOfRangeException(NameOf(stepCount), "stepCount must be positive")
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

'.NET
Imports SixLabors.ImageSharp
Imports SixLabors.ImageSharp.Advanced
Imports SixLabors.ImageSharp.PixelFormats

Public Function GetPixelsFromImage(image As Image, Optional stepCount As Integer = 5) As List(Of Rgba64)
    If stepCount <= 0 Then Throw New ArgumentException("stepCount must be positive")
    Dim pixels As New List(Of Rgba64)()
    Dim rgbaImage As Image(Of Rgba64) = TryCast(image, Image(Of Rgba64))
    Dim needDispose As Boolean = False
    If rgbaImage Is Nothing Then
        rgbaImage = image.CloneAs(Of Rgba64)()
        needDispose = True
    End If
    Try
        Dim frame = rgbaImage.Frames(0)
        For y As Integer = 0 To frame.Height - 1 Step stepCount
            Dim row = frame.GetPixelMemoryGroup(y)
            For x As Integer = 0 To frame.Width - 1 Step stepCount
                pixels.Add(row.Span(x))
            Next
        Next
    Finally
        If needDispose Then rgbaImage?.Dispose()
    End Try
    Return pixels
End Function
```
### Usage
```vbnet
Imports Chromis

Dim pixels As New List(Of ColorExtractor.RGBColor)
For Each sampledColor In GetPixelsFromImage(PictureBox1.Image)
    pixels.Add(ColorExtractor.RGBColor.FromRGB(sampledColor.R, sampledColor.G, sampledColor.B))
Next
Dim colorInfos = ColorExtractor.Extract(pixels, 10)
For Each ci In colorInfos
    Console.WriteLine($"{ci.Color.R}, {ci.Color.G}, {ci.Color.B} - {ci.Ratio:P}")
Next
```
 - `R`, `G`, `B`: RGB color values (0 ~ 255)  
 - `Ratio`: Percentage of this color in the image (0 ~ 1)

## 📊 Algorithms

Chromis supports multiple color quantization algorithms:

- **K-Means** – balanced and accurate
- **Median Cut** – fast and classic
- **Octree** – memory efficient

## 📄 License

[Apache-2.0 License](LICENSE.txt)
