
Namespace Imaging
    ''' <summary>
    ''' 值列表
    ''' </summary>
    Public Class SubValueTreeNode
        Inherits IniTreeNode
        Implements IRegisterable
        Protected RegIn As New List(Of RegisterTreeNode)
        Public ReadOnly Property RegisteredIn As IEnumerable(Of RegisterTreeNode) = RegIn Implements IRegisterable.RegisteredIn

        Private ReadOnly Property IRegisterable_Text As String Implements IRegisterable.Text
            Get
                Return Text
            End Get
        End Property

        <Obsolete>
        Friend Sub RegisterToInternal(Node As RegisterTreeNode) Implements IRegisterable.RegisterToInternal
            RegIn.Add(Node)
        End Sub
        Sub New(Text As String)
            MyBase.New(Text)
        End Sub
        <Obsolete>
        Friend Sub UnRegisterFromInternal(Node As RegisterTreeNode) Implements IRegisterable.UnRegisterFromInternal
            RegIn.Remove(Node)
        End Sub
    End Class
End Namespace