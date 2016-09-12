Public Class IniFileItem
    Sub New(filePath As String)
        Me.FilePath = filePath
        FileName = Path.GetFileNameWithoutExtension(filePath)
    End Sub

    Public Property FilePath As String
    Public Property FileName As String
    Public Property CachedContent As String
End Class
