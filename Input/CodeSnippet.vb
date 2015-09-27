Public Class CodeSnippet
    Public ReadOnly Property Shortcut As String
    Public ReadOnly Property Text As String
    Public ReadOnly Property Description As String
    Sub New(Shortcut As String, Text As String, Description As String)
        Me.Shortcut = Shortcut
        Me.Text = Text
        Me.Description = Description
    End Sub
End Class
