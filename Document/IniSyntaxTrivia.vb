Namespace Document
    Public MustInherit Class IniSyntaxTrivia
        Public Property StartIndex As Integer
        Public Property Text As String
        Public MustOverride ReadOnly Property ColorARGB As Integer
        Sub New(Text$, StartIndex%)
            Me.Text = Text
            Me.StartIndex = StartIndex
        End Sub
    End Class
End Namespace
