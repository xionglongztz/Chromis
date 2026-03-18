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
Public Class Octree
    Implements IColorExtractor
    Public Sub New()

    End Sub
    ''' <summary>
    ''' 八叉树算法
    ''' </summary>
    ''' <param name="pixels">要被处理的像素点</param>
    ''' <param name="colorCount">颜色点数</param>
    ''' <returns>包含具体颜色和比值的结构体</returns>
    Public Function Extract(pixels As List(Of RGBColor), colorCount As Integer) As IReadOnlyList(Of ColorInfo) Implements IColorExtractor.Extract
        Dim octree = New Octree()
        '构建八叉树
        For Each pixel In pixels
            octree.AddColor(pixel)
        Next
        '减少到指定的颜色数量
        While octree.LeafCount > colorCount
            octree.Reduce()
        End While
        '获取颜色和它们的出现次数
        Dim colorNodes = octree.GetLeafNodes()
        Dim totalPixels = pixels.Count
        Dim result = New List(Of ColorInfo)()
        For Each node In colorNodes
            result.Add(New ColorInfo With {
                .Color = node.Color,
                .Ratio = node.PixelCount / totalPixels
            })
        Next
        '按百分比降序排序
        Return result.OrderByDescending(Function(c) c.Ratio).ToList()
    End Function
    ''' <summary>
    ''' 八叉树类
    ''' </summary>
    Private Class OctreeNode
        Public PixelCount As Integer
        Public Red As Integer
        Public Green As Integer
        Public Blue As Integer
        Public Children As OctreeNode() = New OctreeNode(7) {}
        Public IsLeaf As Boolean = False
        Public NextReducible As OctreeNode
        Public ReadOnly Property Color As RGBColor
            Get
                Return RGBColor.FromRGB(Red \ PixelCount, Green \ PixelCount, Blue \ PixelCount)
            End Get
        End Property
    End Class
    Private Class Octree
        Private Root As OctreeNode
        Private _leafCount As Integer
        Private ReducibleNodes As OctreeNode() = New OctreeNode(7) {}
        Public Sub New()
            Root = New OctreeNode()
            _leafCount = 0
            For i = 0 To 7
                ReducibleNodes(i) = Nothing
            Next
        End Sub
        Public ReadOnly Property LeafCount As Integer
            Get
                Return _leafCount
            End Get
        End Property
        Public Sub AddColor(color As RGBColor)
            AddColor(Root, color, 0)
        End Sub
        Private Sub AddColor(node As OctreeNode, color As RGBColor, level As Integer)
            If node.IsLeaf Then
                node.PixelCount += 1
                node.Red += color.R
                node.Green += color.G
                node.Blue += color.B
            Else
                Dim index = GetColorIndex(color, level)

                If node.Children(index) Is Nothing Then
                    node.Children(index) = New OctreeNode()
                    If level = 7 Then
                        node.Children(index).IsLeaf = True
                        node.Children(index).PixelCount = 1
                        node.Children(index).Red = color.R
                        node.Children(index).Green = color.G
                        node.Children(index).Blue = color.B
                        _leafCount += 1
                    Else
                        node.Children(index).NextReducible = ReducibleNodes(level)
                        ReducibleNodes(level) = node.Children(index)
                        AddColor(node.Children(index), color, level + 1)
                    End If
                Else
                    AddColor(node.Children(index), color, level + 1)
                End If
            End If
        End Sub
        Public Sub Reduce()
            Dim level = 6
            While level >= 0 AndAlso ReducibleNodes(level) Is Nothing
                level -= 1
            End While
            If level < 0 Then Return
            Dim node = ReducibleNodes(level)
            If node Is Nothing Then Return '添加空检查防止崩溃
            ReducibleNodes(level) = node.NextReducible
            Dim r = 0, g = 0, b = 0, count = 0
            Dim childCount = 0 '统计实际合并的子节点数
            For i = 0 To 7
                If node.Children(i) IsNot Nothing Then
                    r += node.Children(i).Red
                    g += node.Children(i).Green
                    b += node.Children(i).Blue
                    count += node.Children(i).PixelCount
                    _leafCount -= 1
                    childCount += 1
                End If
            Next
            '只有当存在可合并的子节点时才更新当前节点
            If childCount > 0 Then
                node.IsLeaf = True
                node.PixelCount = count
                node.Red = r
                node.Green = g
                node.Blue = b
                _leafCount += 1
            End If
        End Sub
        Public Function GetLeafNodes() As List(Of OctreeNode)
            Dim nodes As New List(Of OctreeNode)()
            GetLeafNodes(Root, nodes)
            Return nodes
        End Function
        Private Sub GetLeafNodes(node As OctreeNode, nodes As List(Of OctreeNode))
            If node.IsLeaf Then
                nodes.Add(node)
            Else
                For i = 0 To 7
                    If node.Children(i) IsNot Nothing Then
                        GetLeafNodes(node.Children(i), nodes)
                    End If
                Next
            End If
        End Sub
        Private Function GetColorIndex(color As RGBColor, level As Integer) As Integer
            Dim shift = 7 - level '修正位偏移逻辑
            Dim rBit = (color.R >> shift) And 1
            Dim gBit = (color.G >> shift) And 1
            Dim bBit = (color.B >> shift) And 1
            Return (rBit << 2) Or (gBit << 1) Or bBit
        End Function
    End Class
End Class
