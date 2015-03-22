
Namespace Imaging
    ''' <summary>
    ''' 主键和键是能注册其它节点的
    ''' </summary>
    Public MustInherit Class RegisterTreeNode
        Inherits IniTreeNode
        Protected RegValues As New List(Of IRegisterable)
        Public ReadOnly Property RegisteredValues As IEnumerable(Of IRegisterable) = RegValues
        Public MustOverride Sub RegisterAndModify(Item As IRegisterable)
        Protected Function IsRegistered(Item As IRegisterable) As Boolean
            Return (From r In RegValues Select r.Text).Contains(Item.Text)
        End Function
        Public Sub Register(Item As IRegisterable)
            If Not IsRegistered(Item) Then
                RegValues.Add(Item)
                Item.RegisterToInternal(Me)
            End If
        End Sub
        Public Sub UnRegister(Item As IRegisterable)
            RegValues.Remove(Item)
            Item.UnRegisterFromInternal(Me)
        End Sub
        Sub New(Text As String)
            MyBase.New(Text)
        End Sub
    End Class
End Namespace