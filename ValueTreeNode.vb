Namespace Imaging

    ''' <summary>
    ''' 值
    ''' </summary>
    Public Class ValueTreeNode
        Inherits IniTreeNode
        Protected Vals As New List(Of SubValueTreeNode)
        ReadOnly Property Values As IEnumerable(Of SubValueTreeNode) = Vals
        Public Sub AddValue(Item As SubValueTreeNode)
            RenameInternal(Text & "," & Item.Text)
            Vals.Add(Item)
        End Sub
        Sub New(Text As String)
            MyBase.New(Text)
            For Each txs In Text.Split(","c)
                Vals.Add(New SubValueTreeNode(txs.Trim))
            Next
        End Sub
    End Class

End Namespace