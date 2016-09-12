Imports System.Text
Public Enum ErrorFilters
    None
    Message
    Warning
    MessageWarning
    Fault
    MessageFault
    WarningFault
    All
End Enum
Public Enum SeverityLevels
    Message
    Warning
    [Error]
End Enum

Public Class WidenIniAnalysisInfo
    Public Enum FileNames
        Rules
        Art
        AI
        Ra2
    End Enum
    Public Shared Function GetWidenIniAnalysisInfo(Result As INIAnalyzeResult, FileName As String, Optional AggOpt As Integer = ErrorFilters.All) As IEnumerable(Of WidenIniAnalysisInfo)
        Dim tmp As New List(Of WidenIniAnalysisInfo)
        If CBool(AggOpt And ErrorFilters.Message) Then
            For Each tp In Result.Message
                tmp.Add(New WidenIniAnalysisInfo(SeverityLevels.Message, tp.LineNumber, tp.Description, tp.LineText, tp.MainKey, FileName))
            Next
        End If
        If CBool(AggOpt And ErrorFilters.Warning) Then
            For Each tp In Result.Warning
                tmp.Add(New WidenIniAnalysisInfo(SeverityLevels.Warning, tp.LineNumber, tp.Description, tp.LineText, tp.MainKey, FileName))
            Next
        End If
        If CBool(AggOpt And ErrorFilters.Fault) Then
            For Each tp In Result.Fault
                tmp.Add(New WidenIniAnalysisInfo(SeverityLevels.Error, tp.LineNumber, tp.Description, tp.LineText, tp.MainKey, FileName))
            Next
        End If
        Return tmp
    End Function
    <DataGridDisplayName("类型")>
    Public ReadOnly Property InfoType As SeverityLevels
    <DataGridDisplayName("行号")>
    Public ReadOnly Property LineNumber As Integer
    <DataGridDisplayName("描述")>
    Public ReadOnly Property Description As String
    <DataGridDisplayName("文本")>
    Public ReadOnly Property Text As String
    <DataGridDisplayName("主键")>
    Public ReadOnly Property MainKey As String
    <DataGridDisplayName("文件名")>
    Public ReadOnly Property FileName As String
    Sub New(InfoType As SeverityLevels, LineNumber As Integer, Description As String, Text As String, MainKey As String, FileName As String)
        Me.InfoType = InfoType
        Me.LineNumber = LineNumber
        Me.Description = Description
        Me.Text = Text
        Me.MainKey = MainKey
        Me.FileName = FileName
    End Sub
End Class
