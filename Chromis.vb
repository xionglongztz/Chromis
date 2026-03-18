Imports SixLabors.ImageSharp
Public Class Chromis
#Region "快捷方法"
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
    Public Shared Sub Extract(image As Image, colorCount As Integer, Optional extractType As ExtractType = ExtractType.Octree)
        Select Case extractType
            Case ExtractType.KMeans
                KMeans.Extract(image, colorCount)
            Case ExtractType.MedianCut
                MedianCut.Extract(image, colorCount)
            Case ExtractType.Octree
                Octree.Extract(image, colorCount)
        End Select
    End Sub
#End Region
End Class
