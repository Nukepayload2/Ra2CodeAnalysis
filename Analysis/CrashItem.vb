Public Class CrashItem
    Public EIP, Category, Description As String
    Sub New(EIP As String, Category As String, Desc As String)
        Me.EIP = EIP
        Me.Category = Category
        Description = Desc
    End Sub
End Class