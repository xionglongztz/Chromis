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
Imports SixLabors.ImageSharp
Imports SixLabors.ImageSharp.Advanced
Imports SixLabors.ImageSharp.PixelFormats
Public Class GlobalFcn
    ''' <summary>
    ''' 包含一套颜色与占比的结果结构体
    ''' </summary>
    Public Structure ColorInfo
        ''' <summary>
        ''' 颜色
        ''' </summary>
        Public R As Byte
        Public G As Byte
        Public B As Byte
        ''' <summary>
        ''' 该颜色对应的比率, 范围为 0 到 1
        ''' </summary>
        Public Ratio As Single
        ''' <summary>
        ''' 转换成十六进制
        ''' </summary>
        ''' <returns>获得十六进制格式(如#7CBDFF)</returns>
        Public Function ToHex() As String
            Return $"#{R:X2}{G:X2}{B:X2}"
        End Function
        ''' <summary>
        ''' 获得三元组格式
        ''' </summary>
        Public Function ToTuple() As (Byte, Byte, Byte)
            Return (R, G, B)
        End Function
    End Structure
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
    ''' <summary>
    ''' 颜色提取器接口
    ''' </summary>
    Public Interface IColorExtractor
        Function Extract(image As Image, colorCount As Integer) As IReadOnlyList(Of ColorInfo)
    End Interface
End Class