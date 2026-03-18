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
var colors = Chromis.Extract("image.jpg", 10);
foreach (var color in colors)
{
    Console.WriteLine($"{color.R}, {color.G}, {color.B} - {color.Ratio:P}");
}
```

### VB.NET
```vbnet
Dim colors = Chromis.Extract("image.jpg", 10)
For Each color In colors
    Console.WriteLine($"{color.R}, {color.G}, {color.B} - {color.Ratio:P}")
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
