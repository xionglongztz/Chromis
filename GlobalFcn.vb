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
Public Class GlobalFcn
    ''' <summary>
    ''' 颜色结构体
    ''' </summary>
    Public Structure RGBColor
        Public R As Byte
        Public G As Byte
        Public B As Byte
        ''' <summary>
        ''' 根据RGB构造RGBColor
        ''' </summary>
        ''' <param name="r">红色</param>
        ''' <param name="g">绿色</param>
        ''' <param name="b">蓝色</param>
        ''' <returns><seealso cref="RGBColor"/>类型</returns>
        Public Shared Function FromRGB(r As Integer, g As Integer, b As Integer) As RGBColor
            Dim _color As New RGBColor With {
                .R = r,
                .G = g,
                .B = b
            }
            Return _color
        End Function
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
    ''' 包含一套颜色与占比的结果结构体
    ''' </summary>
    Public Structure ColorInfo
        ''' <summary>
        ''' 颜色
        ''' </summary>
        Public Color As RGBColor
        ''' <summary>
        ''' 该颜色对应的比率, 范围为 0 到 1
        ''' </summary>
        Public Ratio As Single
    End Structure
    ''' <summary>
    ''' 颜色提取器接口
    ''' </summary>
    Public Interface IColorExtractor
        Function Extract(pixels As List(Of RGBColor), colorCount As Integer) As IReadOnlyList(Of ColorInfo)
    End Interface
End Class