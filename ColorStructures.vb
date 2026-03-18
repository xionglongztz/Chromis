Imports System.Drawing
Public Class ColorStructures
    ''' <summary>
    ''' 包含一套颜色与占比的结果结构体
    ''' </summary>
    Public Structure ColorInfo
        ''' <summary>
        ''' 颜色
        ''' </summary>
        Public Color As Color
        ''' <summary>
        ''' 该颜色对应的比率, 取值为 0 到 1
        ''' </summary>
        Public Ratio As Double
    End Structure

    Public Structure ColorList

    End Structure
End Class