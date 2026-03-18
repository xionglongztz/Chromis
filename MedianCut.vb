' Chromis - Image Dominant Colors Extracter
' Copyright 2026 xionglongztz
'
' Licensed under the Apache License, Version 2.0 (the "License");
' you may not use this file except in compliance with the License.
' You may obtain a copy of the License at
'
'     http://www.apache.org/licenses/LICENSE-2.0
'
' Unless required by applicable law or agreed to in writing, software
' distributed under the License is distributed on an "AS IS" BASIS,
' WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
' See the License for the specific language governing permissions and
' limitations under the License.
Imports Chromis.GlobalFcn
Imports SixLabors.ImageSharp
Imports SixLabors.ImageSharp.PixelFormats

Public Class MedianCut
    Implements IColorExtractor
    Public Sub New()

    End Sub
    ''' <summary>
    ''' 中位切分算法
    ''' </summary>
    ''' <param name="image">要被处理的图像</param>
    ''' <param name="colorCount">颜色点数</param>
    ''' <returns>包含具体颜色和比值的结构体</returns>
    Public Function Extract(image As Image, colorCount As Integer) As IReadOnlyList(Of ColorInfo) Implements IColorExtractor.Extract
        Dim pixels = GetPixelsFromImage(image)
        Dim colorCubes As New List(Of ColorCube) From {New ColorCube(pixels)}
        '切分直到得到足够的颜色块
        While colorCubes.Count < colorCount
            '找到最大的颜色范围
            Dim cubeToSplit = colorCubes.OrderByDescending(Function(c) c.Size).First()
            colorCubes.Remove(cubeToSplit)
            '切分颜色块
            Dim splitCubes = cubeToSplit.Split()
            colorCubes.AddRange(splitCubes)
        End While
        '计算每个颜色块的平均颜色和占比
        Dim totalPixels = pixels.Count
        Dim result = New List(Of ColorInfo)()
        For Each cube In colorCubes
            result.Add(New ColorInfo With {
                .Color = cube.AverageColor,
                .Ratio = cube.Pixels.Count / totalPixels
            })
        Next
        '按百分比降序排序
        Return result.OrderByDescending(Function(c) c.Ratio).ToList()
    End Function
    ''' <summary>
    ''' 颜色立方类
    ''' </summary>
    Private Class ColorCube
        Public Pixels As List(Of Rgba64)
        Public RedRange As Tuple(Of Integer, Integer)
        Public GreenRange As Tuple(Of Integer, Integer)
        Public BlueRange As Tuple(Of Integer, Integer)
        Public ReadOnly Property Size As Integer '获得ColorCube的大小
            Get
                Return (RedRange.Item2 - RedRange.Item1) *
                       (GreenRange.Item2 - GreenRange.Item1) *
                       (BlueRange.Item2 - BlueRange.Item1)
            End Get
        End Property
        Public ReadOnly Property AverageColor As Rgba64 '取平均颜色
            Get
                Dim r = 0, g = 0, b = 0
                For Each pixel In Pixels
                    r += pixel.R
                    g += pixel.G
                    b += pixel.B
                Next
                r \= Pixels.Count
                g \= Pixels.Count
                b \= Pixels.Count
                Return Color.FromRgb(r, g, b)
            End Get
        End Property
        Public Sub New(pixels As List(Of Rgba64))
            Me.Pixels = pixels
            CalculateRanges()
        End Sub
        Private Sub CalculateRanges()
            Dim minR = Integer.MaxValue, maxR = Integer.MinValue
            Dim minG = Integer.MaxValue, maxG = Integer.MinValue
            Dim minB = Integer.MaxValue, maxB = Integer.MinValue
            For Each pixel In Pixels
                minR = Math.Min(minR, pixel.R)
                maxR = Math.Max(maxR, pixel.R)
                minG = Math.Min(minG, pixel.G)
                maxG = Math.Max(maxG, pixel.G)
                minB = Math.Min(minB, pixel.B)
                maxB = Math.Max(maxB, pixel.B)
            Next
            RedRange = Tuple.Create(minR, maxR)
            GreenRange = Tuple.Create(minG, maxG)
            BlueRange = Tuple.Create(minB, maxB)
        End Sub
        Public Function Split() As List(Of ColorCube) '切分
            '确定哪个颜色通道的范围最大
            Dim rangeR = RedRange.Item2 - RedRange.Item1
            Dim rangeG = GreenRange.Item2 - GreenRange.Item1
            Dim rangeB = BlueRange.Item2 - BlueRange.Item1
            Dim maxRange = Math.Max(rangeR, Math.Max(rangeG, rangeB))
            Dim channelToSplit As Integer
            If maxRange = rangeR Then
                channelToSplit = 0 ' R
            ElseIf maxRange = rangeG Then
                channelToSplit = 1 ' G
            Else
                channelToSplit = 2 ' B
            End If
            '按选定通道的中位数排序
            Select Case channelToSplit
                Case 0
                    Pixels = Pixels.OrderBy(Function(p) p.R).ToList()
                Case 1
                    Pixels = Pixels.OrderBy(Function(p) p.G).ToList()
                Case 2
                    Pixels = Pixels.OrderBy(Function(p) p.B).ToList()
            End Select
            '在中位数处切分
            Dim medianIndex = Pixels.Count \ 2
            Dim cube1 = New ColorCube(Pixels.Take(medianIndex).ToList())
            Dim cube2 = New ColorCube(Pixels.Skip(medianIndex).ToList())
            Return New List(Of ColorCube) From {cube1, cube2}
        End Function
    End Class
End Class
