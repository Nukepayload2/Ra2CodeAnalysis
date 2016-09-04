Public Class NamedIniAnalyzer
    Sub New(fileNameWithoutExt As String, analyzer As INIAnalizer)
        Me.FileNameWithoutExt = fileNameWithoutExt
        Me.Analyzer = analyzer
    End Sub

    Public Property FileNameWithoutExt As String
    Public Property Analyzer As INIAnalizer
End Class
