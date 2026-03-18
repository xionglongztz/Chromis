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
Imports System.IO
Imports Chromis.GlobalFcn
Imports SixLabors.ImageSharp
Public Class Chromis
    ''' <summary>
    ''' 提取类型
    ''' </summary>
    Public Enum ExtractType
        ''' <summary>
        ''' K-Means聚类
        ''' </summary>
        KMeans
        ''' <summary>
        ''' 中位切分
        ''' </summary>
        MedianCut
        ''' <summary>
        ''' 八叉树
        ''' </summary>
        Octree
    End Enum
    ''' <summary>
    ''' 提取图像的主要颜色
    ''' </summary>
    ''' <param name="image">要被处理的图像</param>
    ''' <param name="colorCount">颜色数量</param>
    ''' <param name="extractType">(可选)提取方式,默认为<seealso cref="Octree"/></param>
    Public Shared Function Extract(image As Image, colorCount As Integer, Optional extractType As ExtractType = ExtractType.Octree) As List(Of ColorInfo)
        Dim extractor = ExtractorFactory(extractType)(colorCount)
        Return extractor.Extract(image, colorCount)
    End Function
    ''' <summary>
    ''' 提取图像的主要颜色
    ''' </summary>
    ''' <param name="stream">要被处理的图像流</param>
    ''' <param name="colorCount">颜色数量</param>
    ''' <param name="extractType">(可选)提取方式,默认为<seealso cref="Octree"/></param>
    Public Shared Function Extract(stream As Stream, colorCount As Integer, Optional extractType As ExtractType = ExtractType.Octree) As List(Of ColorInfo)
        Dim extractor = ExtractorFactory(extractType)(colorCount)
        Using img = Image.Load(stream)
            Return extractor.Extract(img, colorCount)
        End Using
    End Function
    ''' <summary>
    ''' 提取图像的主要颜色
    ''' </summary>
    ''' <param name="filePath">要被处理的文件路径</param>
    ''' <param name="colorCount">颜色数量</param>
    ''' <param name="extractType">(可选)提取方式,默认为<seealso cref="Octree"/></param>
    Public Shared Function Extract(filePath As String, colorCount As Integer, Optional extractType As ExtractType = ExtractType.Octree) As List(Of ColorInfo)
        Dim extractor = ExtractorFactory(extractType)(colorCount)
        Using img = Image.Load(filePath)
            Return extractor.Extract(img, colorCount)
        End Using
    End Function
    ''' <summary>
    ''' 注册一个自己的提取算法
    ''' </summary>
    ''' <param name="type">提取类型名称</param>
    ''' <param name="factory">工厂</param>
    Public Shared Sub Register(type As ExtractType, factory As Func(Of Integer, IColorExtractor))
        ExtractorFactory(type) = factory
    End Sub
    Private Shared ReadOnly ExtractorFactory As New Dictionary(Of ExtractType, Func(Of Integer, IColorExtractor)) From {
    {ExtractType.KMeans, Function(count) New KMeans()},
    {ExtractType.MedianCut, Function(count) New MedianCut()},
    {ExtractType.Octree, Function(count) New Octree()}
}
End Class