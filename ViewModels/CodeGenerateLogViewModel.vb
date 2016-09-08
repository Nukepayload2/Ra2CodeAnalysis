Imports System.Collections.ObjectModel
Imports Nukepayload2.Ra2CodeAnalysis

Public Class CodeGenerateLogViewModel
    Public Property IniDiagnostics As New ObservableCollection(Of WidenIniAnalysisInfo)
    Public Property GenerateLog As New ObservableCollection(Of String)
End Class
