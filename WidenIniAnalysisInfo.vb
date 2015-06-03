Imports System.Text

Public Class WidenIniAnalysisInfo
    Public Enum InfoTypes
        Message
        Warning
        [Error]
    End Enum
    Public Enum FileNames
        Rules
        Art
        AI
        Ra2
    End Enum
    Public Enum AggragateOptions
        None
        Message
        Warning
        MessageWarning
        Fault
        MessageFault
        WarningFault
        All
    End Enum
    Public Shared Function GetWidenIniAnalysisInfo(Result As INIAnalizeResult, FileName As String, Optional AggOpt As Integer = AggragateOptions.All) As IEnumerable(Of WidenIniAnalysisInfo)
        Dim tmp As New List(Of WidenIniAnalysisInfo)
        Dim curfn As FileNames
        FileName = FileName.ToLower
        If FileName.Contains("rules") Then
            curfn = FileNames.Rules
        ElseIf FileName.Contains("art")
            curfn = FileNames.Art
        ElseIf FileName.Contains("ai")
            curfn = FileNames.AI
        ElseIf FileName.Contains("ra")
            curfn = FileNames.Ra2
        Else
            Throw New ArgumentException("文件名不是受支持的")
        End If
        If CBool(AggOpt And AggragateOptions.Message) Then
            For Each tp In Result.Message
                tmp.Add(New WidenIniAnalysisInfo(InfoTypes.Message, tp.LineNumber, tp.Description, tp.LineText, tp.MainKey, curfn))
            Next
        End If
        If CBool(AggOpt And AggragateOptions.Warning) Then
            For Each tp In Result.Warning
                tmp.Add(New WidenIniAnalysisInfo(InfoTypes.Warning, tp.LineNumber, tp.Description, tp.LineText, tp.MainKey, curfn))
            Next
        End If
        If CBool(AggOpt And AggragateOptions.Fault) Then
            For Each tp In Result.Fault
                tmp.Add(New WidenIniAnalysisInfo(InfoTypes.Error, tp.LineNumber, tp.Description, tp.LineText, tp.MainKey, curfn))
            Next
        End If
        Return tmp
    End Function
    <DataGridDisplayName("类型")>
    Public ReadOnly Property InfoType As InfoTypes
    <DataGridDisplayName("行号")>
    Public ReadOnly Property LineNumber As Integer
    <DataGridDisplayName("描述")>
    Public ReadOnly Property Description As String
    <DataGridDisplayName("文本")>
    Public ReadOnly Property Text As String
    <DataGridDisplayName("主键")>
    Public ReadOnly Property MainKey As String
    <DataGridDisplayName("文件名")>
    Public ReadOnly Property FileName As FileNames
    Sub New(InfoType As InfoTypes, LineNumber As Integer, Description As String, Text As String, MainKey As String, FileName As FileNames)
        Me.InfoType = InfoType
        Me.LineNumber = LineNumber
        Me.Description = Description
        Me.Text = Text
        Me.MainKey = MainKey
        Me.FileName = FileName
    End Sub
End Class
