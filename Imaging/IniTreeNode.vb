Imports System.Text
Namespace Imaging
    ''' <summary>
    ''' INI树节点
    ''' </summary>
    Public MustInherit Class IniTreeNode
        ReadOnly Property Text As String
        Sub New(Text As String)
            Me.Text = Text
        End Sub
        Friend Sub RenameInternal(Name As String)
            _Text = Name
        End Sub
        Public Sub Rename(Root As IEnumerable(Of MainKeyTreeNode), NewName As String)
            For Each MK In Root
                For Each kv In MK.KeyValues.Keys
                    Dim Vals As New StringBuilder
                    Dim vs = MK.KeyValues(kv).Values
                    Dim co = vs.Count
                    For i As Integer = 0 To co - 2
                        Dim v = vs(i)
                        Dim nam2 = If(Text = v.Text, NewName, v.Text)
                        v.RenameInternal(nam2)
                        Vals.Append(nam2)
                        Vals.Append(","c)
                    Next
                    Dim Last = vs(co - 1).Text
                    Dim nam = If(Text = Last, NewName, Last)
                    vs.Last.RenameInternal(nam)
                    Vals.Append(nam)
                    MK.KeyValues(kv).RenameInternal(Vals.ToString)
                Next
            Next
            RenameInternal(NewName)
        End Sub
    End Class
End Namespace