
Namespace Imaging
    ''' <summary>
    ''' 能被注册到主键或键
    ''' </summary>
    Public Interface IRegisterable
        ReadOnly Property Text As String
        ReadOnly Property RegisteredIn As IEnumerable(Of RegisterTreeNode)
        Sub RegisterToInternal(Node As RegisterTreeNode)
        Sub UnRegisterFromInternal(Node As RegisterTreeNode)
    End Interface

End Namespace