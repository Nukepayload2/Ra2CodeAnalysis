Namespace Document
    Public MustInherit Class IniSyntax
        Public Property StartIndex As Integer
        Public Overridable Property Text As String
        Public ReadOnly Property Children As New List(Of IniSyntaxTrivia)
        Sub New(Text$, StartIndex%)
            Me.Text = Text
            Me.StartIndex = StartIndex
        End Sub
    End Class
End Namespace
