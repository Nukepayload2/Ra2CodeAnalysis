Public Class IniFileItem
    Sub New(filePath As String, fileName As String)
        Me.FilePath = filePath
        Me.FileName = fileName
    End Sub

    Public Property FilePath As String
    Public Property FileName As String
End Class
