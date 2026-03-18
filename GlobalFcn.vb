Imports SixLabors.ImageSharp
Imports SixLabors.ImageSharp.Advanced
Imports SixLabors.ImageSharp.PixelFormats
Public Class GlobalFcn

#Region "结构体定义"
    ''' <summary>
    ''' 包含一套颜色与占比的结果结构体
    ''' </summary>
    Public Structure ColorInfo
        ''' <summary>
        ''' 颜色
        ''' </summary>
        Public Color As Rgba64
        ''' <summary>
        ''' 该颜色对应的比率, 范围为 0 到 1
        ''' </summary>
        Public Ratio As Double
    End Structure
#End Region

#Region "图像预处理"
    ''' <summary>
    ''' 对图像进行采样
    ''' </summary>
    ''' <param name="image">要被处理的图像</param>
    ''' <param name="stepCount">(可选)图像处理步长, 默认为 5</param>
    ''' <returns>采样的颜色点集合</returns>
    ''' <exception cref="ArgumentException"></exception>
    Public Shared Function GetPixelsFromImage(image As Image, Optional stepCount As Integer = 5) As List(Of Rgba64)
        If stepCount <= 0 Then Throw New ArgumentException("stepCount must be positive")
        Dim pixels As New List(Of Rgba64)()
        '确保图像为Rgba64格式，如果不是则克隆转换
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
#End Region

#Region "颜色处理"
    ''' <summary>
    ''' 计算平均颜色
    ''' </summary>
    ''' <param name="colors">颜色列表</param>
    ''' <returns>平均颜色</returns>
    Public Shared Function CalculateAverageColor(colors As List(Of Rgba64)) As Rgba64
        Dim r = 0, g = 0, b = 0
        For Each color In colors
            r += color.R
            g += color.G
            b += color.B
        Next
        r \= colors.Count
        g \= colors.Count
        b \= colors.Count
        Return Color.FromRgb(r, g, b)
    End Function
#End Region

#Region "数学运算"
    ''' <summary>
    ''' 计算两个颜色的欧几里得距离
    ''' </summary>
    ''' <param name="AColor">颜色A</param>
    ''' <param name="BColor">颜色B</param>
    ''' <returns>两个颜色的欧几里得距离</returns>
    Public Shared Function CalcEuclideanDis(AColor As Rgba64, BColor As Rgba64) As Integer
        Dim deltaR As Double = CDbl(AColor.R) - CDbl(BColor.R)
        Dim deltaG As Double = CDbl(AColor.G) - CDbl(BColor.G)
        Dim deltaB As Double = CDbl(AColor.B) - CDbl(BColor.B)
        Return Math.Sqrt(deltaR * deltaR + deltaG * deltaG + deltaB * deltaB)
    End Function '计算两个颜色的欧氏距离
#End Region

End Class
