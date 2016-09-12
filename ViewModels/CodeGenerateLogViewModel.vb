Imports System.Collections.ObjectModel
Public Class CodeGenerateLogViewModel
    Inherits SingleInstance(Of CodeGenerateLogViewModel)
    Public Property IniDiagnostics As New ObservableCollection(Of WidenIniAnalysisInfo)
    Public Property GenerateLog As New ObservableCollection(Of String)
End Class
