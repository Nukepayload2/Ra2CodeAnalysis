Imports System.Reflection
Imports System.Text
Imports Nukepayload2.CodeAnalysis
Imports Nukepayload2.CodeAnalysis.Linq

Public Module VBProjectWriterExtension

    <Extension>
    Public Function GenerateVBFileFromIni(d As NamedIniAnalyzer) As GeneratedCodeFile
        Dim hlp As HelpProvider = New EmptyHelpProvider
        Select Case d.FileNameWithoutExt.ToLower
            Case "rules", "rulesmd"
                hlp = New RulesHelpProvider
            Case "art", "artmd"
                hlp = New ArtHelpProvider
            Case "ai", "aimd"
                hlp = New AIHelpProvider
        End Select
        Dim sb As New IndentStringBuilder
        Dim entityContext As New EntityInferContext(d, hlp, sb)
        Dim ns As New VBNamespaceBuilder(sb, d.FileNameWithoutExt)
        ns.BeginBlock()
        For Each itf In entityContext.InterfaceIndex.Values
            itf.BeginBlock()
            itf.EndBlock()
        Next
        For Each cls In entityContext.ClassIndex.Values
            cls.BeginBlock()
            cls.EndBlock()
        Next
        ns.EndBlock()
        Return New GeneratedCodeFile(d.FileNameWithoutExt + ".vb", sb.ToString)
    End Function
End Module