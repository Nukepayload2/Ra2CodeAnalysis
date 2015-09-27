
Imports Nukepayload2.Ra2CodeAnalysis.AnalysisHelper

Namespace Imaging
    ''' <summary>
    ''' 键
    ''' </summary>
    Public Class KeyTreeNode
        Inherits RegisterTreeNode
        Public Property ValueNode As ValueTreeNode
        Public ReadOnly Property IsRegisterKey As Boolean
            Get
                Return Text.IsUInteger
            End Get
        End Property
        Sub New(Text As String, ValueNode As ValueTreeNode)
            MyBase.New(Text)
            Me.ValueNode = ValueNode
        End Sub

        Public Overrides Sub RegisterAndModify(Item As IRegisterable)
            Register(Item)
            If Not (From v In ValueNode.Values Select v.Text).Contains(Item.Text) Then
                ValueNode.AddValue(New SubValueTreeNode(Item.Text))
            End If
        End Sub

        Public Overrides Sub UnRegisterAndModify(Item As IRegisterable)
            UnRegister(Item)
            If (From v In ValueNode.Values Select v.Text).Contains(Item.Text) Then
                ValueNode.RemoveValue(New SubValueTreeNode(Item.Text))
            End If
        End Sub
    End Class

End Namespace