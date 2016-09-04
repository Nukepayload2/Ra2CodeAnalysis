Public Class INIAnalyzeInfo
    <DataGridDisplayName("行号")>
    Public ReadOnly Property LineNumber As Integer
    <DataGridDisplayName("描述")>
    Public ReadOnly Property Description As String
    <DataGridDisplayName("文本")>
    Public ReadOnly Property LineText As String
    <DataGridDisplayName("主键")>
    Public ReadOnly Property MainKey As String
    Sub New(Ln As Integer, Desc As String, Text As String, MK As String)
        LineNumber = Ln
        Description = Desc
        LineText = Text
        MainKey = MK
    End Sub
End Class