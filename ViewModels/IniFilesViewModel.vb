Imports System.Collections.ObjectModel
Imports Nukepayload2.Ra2CodeAnalysis

Public Class IniFilesViewModel
    Inherits SingleInstance(Of IniFilesViewModel)
    Public Property IniFileItems As New ObservableCollection(Of IniFileItem)
End Class
