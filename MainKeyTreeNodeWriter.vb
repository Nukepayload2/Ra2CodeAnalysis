
Imports System.Text

Namespace Imaging
    Public Class MainKeyTreeNodeWriter
        Dim mkt As MainKeyTreeNode
        Protected Sub Load(ReadonlyTree As MainKeyTreeNode)
            mkt = ReadonlyTree
            Name = mkt.Text
            For Each itm In mkt.KeyValues
                KeyValues.Add(New WritableKeyValuePair（itm.Key.Text, itm.Value.Text）)
            Next
        End Sub
        Sub New(ReadonlyTree As MainKeyTreeNode)
            Load(ReadonlyTree)
        End Sub
        Property Name As String
            Get
                Return mkt.Text
            End Get
            Set(value As String)
                mkt.RenameInternal(value)
            End Set
        End Property
        Property KeyValues As New List(Of WritableKeyValuePair)
        Public Sub Rename(Root As IEnumerable(Of MainKeyTreeNode), NewName As String)
            mkt.Rename(Root, NewName)
        End Sub
        Public Sub SaveWithoutRename()
            mkt.KeyValues.Clear()
            For Each kv In KeyValues
                mkt.KeyValues.Add(New KeyValuePair(Of KeyTreeNode, ValueTreeNode)(New KeyTreeNode(kv.Key), New ValueTreeNode(kv.Value)))
            Next
        End Sub
    End Class
End Namespace
