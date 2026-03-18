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
Imports Chromis.ColorExtractor

Public Class KMeans
    Implements IColorExtractor
    Public Sub New()

    End Sub
    ''' <summary>
    ''' K-Means 聚类算法
    ''' </summary>
    ''' <param name="pixels">要被处理的像素点</param>
    ''' <param name="clusterCount">聚类点数量</param>
    ''' <returns>包含具体颜色和比值的结构体</returns>
    Public Function Extract(pixels As List(Of RGBColor), clusterCount As Integer) As IReadOnlyList(Of ColorInfo) Implements IColorExtractor.Extract
        Dim clusters = New List(Of Cluster)(clusterCount)
        '初始化随机中心点
        Dim rnd As New Random()
        For i = 0 To clusterCount - 1
            Dim randomPixel = pixels(rnd.Next(pixels.Count))
            clusters.Add(New Cluster(randomPixel))
        Next
        Dim changed As Boolean
        Dim maxIterations As Integer = 10
        Dim iteration = 0
        'K-Means 迭代
        Do
            changed = False
            iteration += 1
            '清空聚类点
            For Each cluster In clusters
                cluster.Points.Clear()
            Next
            '分配每个点到最近的聚类
            For Each pixel In pixels
                Dim nearestCluster As Cluster = Nothing
                Dim minDistance = Double.MaxValue
                For Each cluster In clusters
                    Dim distance = CalcEuclideanDis(pixel, cluster.Center)
                    If distance < minDistance Then
                        minDistance = distance
                        nearestCluster = cluster
                    End If
                Next
                nearestCluster.Points.Add(pixel)
            Next
            '重新计算聚类中心
            For Each cluster In clusters
                If cluster.Points.Count > 0 Then
                    Dim newCenter = CalculateAverageColor(cluster.Points)
                    If Not newCenter.Equals(cluster.Center) Then
                        cluster.Center = newCenter
                        changed = True
                    End If
                End If
            Next
        Loop While changed AndAlso iteration < maxIterations
        '计算每个聚类的百分比
        Dim totalPixels = pixels.Count
        Dim result = New List(Of ColorInfo)()
        For Each cluster In clusters
            If cluster.Points.Count > 0 Then
                result.Add(New ColorInfo With {
                    .Color = cluster.Center,
                    .Ratio = cluster.Points.Count / totalPixels
                })
            End If
        Next
        '按百分比降序排序
        Return result.OrderByDescending(Function(c) c.Ratio).ToList()
    End Function
    ''' <summary>
    ''' 聚类点类
    ''' </summary>
    Private Class Cluster
        Public Center As RGBColor
        Public Points As New List(Of RGBColor)
        Public Sub New(center As RGBColor)
            Me.Center = center
        End Sub
    End Class

    ''' <summary>
    ''' 计算平均颜色
    ''' </summary>
    ''' <param name="colors">颜色列表</param>
    ''' <returns>平均颜色</returns>
    Private Shared Function CalculateAverageColor(colors As List(Of RGBColor)) As RGBColor
        Dim r = 0, g = 0, b = 0
        For Each color In colors
            r += color.R
            g += color.G
            b += color.B
        Next
        r \= colors.Count
        g \= colors.Count
        b \= colors.Count
        Return RGBColor.FromRGB(r, g, b)
    End Function

    ''' <summary>
    ''' 计算两个颜色的欧几里得距离
    ''' </summary>
    ''' <param name="AColor">颜色A</param>
    ''' <param name="BColor">颜色B</param>
    ''' <returns>两个颜色的欧几里得距离</returns>
    Private Shared Function CalcEuclideanDis(AColor As RGBColor, BColor As RGBColor) As Integer
        Dim deltaR As Double = CDbl(AColor.R) - CDbl(BColor.R)
        Dim deltaG As Double = CDbl(AColor.G) - CDbl(BColor.G)
        Dim deltaB As Double = CDbl(AColor.B) - CDbl(BColor.B)
        Return Math.Sqrt(deltaR * deltaR + deltaG * deltaG + deltaB * deltaB)
    End Function

End Class
