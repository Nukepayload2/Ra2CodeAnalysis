Imports Nukepayload2.Ra2CodeAnalysis.Imaging
''' <summary>
''' inins功能支持。像xmlns那样。这样可以提供类型检查和IME支持。
''' </summary>
Public Class IniNamespace
    Public Items As New List(Of IniNamespaceItem)
    Public Const Rules = "http://schemas.nukepayload2.com/ra2codeanalysis/rules"
    Public Const AI = "http://schemas.nukepayload2.com/ra2codeanalysis/ai"
    Public Const Art = "http://schemas.nukepayload2.com/ra2codeanalysis/art"

    Sub New(INITree As IEnumerable(Of MainKeyTreeNode), ini As INIAnalyzer, HelpProvider As IHelpProvider)
        Dim hd As New HelpDataProvider
        Dim KeyNames As New List(Of String)
        For Each mk In INITree
            For Each kv In mk.KeyValues
                Dim tp = hd.DeepAnalizeType(kv.Value.Text, ini)
                Items.Add(New IniNamespaceItem(kv.Key.Text, HelpProvider.GetHelpText(kv.Key.Text), tp))
                KeyNames.Add(kv.Key.Text)
            Next
        Next
    End Sub
End Class
