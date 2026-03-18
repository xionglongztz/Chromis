Imports SixLabors.ImageSharp
Public Class GlobalFcn
    ''' <summary>
    ''' 对图像进行采样
    ''' </summary>
    ''' <param name="image">要被处理的图像</param>
    ''' <param name="stepCount">(可选)图像处理步长, 默认为 5</param>
    ''' <returns>采样的颜色点集合</returns>
    Public Shared Function GetPixelsFromImage(image As Image, Optional stepCount As Integer = 5) As List(Of Color)
        Dim pixels As New List(Of Color)()
        Using bmp = New Bitmap(image)
            For x = 0 To bmp.Width - 1 Step stepCount '步长5加快处理速度
                For y = 0 To bmp.Height - 1 Step stepCount
                    pixels.Add(bmp.GetPixel(x, y))
                Next
            Next
        End Using
        Return pixels
    End Function
End Class
