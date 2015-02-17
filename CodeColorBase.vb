Imports System.Text.RegularExpressions

Public MustInherit Class CodeColorBase(Of Brush)
    Dim KWLst As IEnumerable(Of String) = {"(?<=\s)As(?=\s)",
        "(?<=<)(bool|float|object|int|string)(?=>)",
        "(?<=(As|Of)\s+)(Boolean|Integer|Single|String|Object)",
        "(bool|float|const|struct|object|int|string)(?=\s)",
        "(Dim|Const|Structure|Of)(?=\s)",
        "(?<==\s*)(Nothing|null|True|true|False|false)"}
    Dim SecKWLst As IList(Of String) = {"^\w+(?=\(Of)", 'VB样式的泛型左侧
        "\w+(?=<)", 'c#样式的泛型左侧
        "(?<=<)\w+(?=>)", 'c#样式的泛型右侧
        "(?<=:)\w+(?=\s+[^=]+;)", 'c#样式的单变量声明
        "(?<=(As|Of)\s+)\w+", 'VB样式的单变量声明+泛型右
        "(?<=\w\.)\w+(?=(\s|;))" '最右侧的成员
        }
    Dim Comment As New Regex("^[^用].*$", RegexOptions.Multiline)
    Protected MustOverride Sub SetColor(Color As Brush, Index As Integer, Length As Integer)
    Protected MustOverride Sub ClearEffects()

    Protected Sub ColorInternal(Code As String, Instructions As Brush, Types As Brush, Comments As Brush)
        ClearEffects()
        Dim ProcessedIns As New List(Of Integer)
        For Each b In KWLst
            Dim reg As New Regex(b)
            For Each m As Match In reg.Matches(Code)
                ProcessedIns.Add(m.Index)
                SetColor(Instructions, m.Index, m.Length)
            Next
        Next
        For Each g In SecKWLst
            Dim reg As New Regex(g)
            For Each m As Match In reg.Matches(Code)
                If Not ProcessedIns.Contains(m.Index) Then
                    SetColor(Types, m.Index, m.Length)
                End If
            Next
        Next
        For Each m As Match In Comment.Matches(Code)
            SetColor(Comments, m.Index, m.Length)
        Next
    End Sub
End Class
