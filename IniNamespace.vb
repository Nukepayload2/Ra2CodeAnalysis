Imports Nukepayload2.Ra2CodeAnalysis.Imaging
''' <summary>
''' inins功能支持。像xmlns那样。这样可以提供类型检查和IME支持。
''' </summary>
Public Class IniNamespace
    Public Items As New List(Of IniNamespaceItem)
    Public CurrentINITree As IEnumerable(Of MainKeyTreeNode)
    Protected Sub Load()
        Dim hd As New HelpDataProvider
        For Each mk In CurrentINITree
            For Each kv In mk.KeyValues
                Dim tp = hd.TempAnalizeUsage(kv.Value.Text)
            Next
        Next
    End Sub
    Sub New(INITree As IEnumerable(Of MainKeyTreeNode))
        CurrentINITree = INITree
        Load()
    End Sub
End Class
