Imports System.Text

Namespace Imaging
    ''' <summary>
    ''' 主键
    ''' </summary>
    Public Class MainKeyTreeNode
        Inherits RegisterTreeNode
        Implements IRegisterable
        ''' <summary>
        ''' 把当前主键的信息转换回ini文本
        ''' </summary>
        ''' <returns></returns>
        Public Overrides Function ToString() As String
            With New StringBuilder
                .Append("["c)
                .Append(Text)
                .AppendLine("]"c)
                For Each kv In KeyValues
                    .Append(kv.Key.Text)
                    .Append("="c)
                    .AppendLine(kv.Value.Text)
                Next
                Return .ToString
            End With
        End Function
        ReadOnly Property KeyValues As IDictionary(Of KeyTreeNode, ValueTreeNode) =
        New Dictionary(Of KeyTreeNode, ValueTreeNode)
        Protected RegIn As New List(Of RegisterTreeNode)
        Public ReadOnly Property RegisteredIn As IEnumerable(Of RegisterTreeNode) = RegIn Implements IRegisterable.RegisteredIn

        Private ReadOnly Property IRegisterable_Text As String Implements IRegisterable.Text
            Get
                Return Text
            End Get
        End Property

        Sub New(Text As String)
            MyBase.New(Text)
        End Sub
        <Obsolete>
        Friend Sub RegisterToInternal(Node As RegisterTreeNode) Implements IRegisterable.RegisterToInternal
            RegIn.Add(Node)
        End Sub
        <Obsolete>
        Friend Sub UnRegisterFromInternal(Node As RegisterTreeNode) Implements IRegisterable.UnRegisterFromInternal
            RegIn.Remove(Node)
        End Sub

        Public Overrides Sub RegisterAndModify(Item As IRegisterable)
            If Not IsRegistered(Item) Then
                Register(Item)
                Dim keys = From k In KeyValues Select k.Key.Text
                For i As Integer = 0 To Integer.MaxValue
                    If Not keys.Contains(i.ToString) Then
                        Dim v = New ValueTreeNode(Item.Text)
                        KeyValues.Add(New KeyTreeNode(i.ToString, v), v)
                        Return
                    End If
                Next
                Throw New InvalidOperationException("注册已满")
            End If
        End Sub
    End Class
End Namespace