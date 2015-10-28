Namespace Document
    Public MustInherit Class IniBlock
        Public Property StartIndex As Integer
        Protected _Text$ = ""
        Public MustOverride Property Text As String
        Public ReadOnly Property Children As New List(Of IniSyntax)
        Sub New(Text$, StartIndex%)
            Me.Text = Text
            Me.StartIndex = StartIndex
        End Sub
    End Class
End Namespace