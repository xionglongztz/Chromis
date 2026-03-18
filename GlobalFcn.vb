Imports SixLabors.ImageSharp
Imports SixLabors.ImageSharp.PixelFormats
Public Class GlobalFcn
    ''' <summary>
    ''' 对图像进行采样
    ''' </summary>
    ''' <param name="image">要被处理的图像</param>
    ''' <param name="stepCount">(可选)图像处理步长, 默认为 5</param>
    ''' <returns>采样的颜色点集合</returns>
    Public Shared Function GetPixelsFromImage(image As Image, Optional stepCount As Integer = 5) As List(Of Rgba64)
        Dim pixels As New List(Of Rgba64)()
        For x = 0 To image.Width - 1 Step stepCount '步长5加快处理速度
            For y = 0 To image.Height - 1 Step stepCount
                pixels.Add(image.GetPixel(x, y))
            Next
        Next
        Return pixels
    End Function
End Class
