Namespace Document
    Public Class IniCommentSyntaxTrivia
        Inherits IniSyntaxTrivia
        Public Overrides ReadOnly Property ColorARGB As Integer = CodeColors.ForestGreen
        Sub New(Text As String, StartIndex%)
            MyBase.New(Text, StartIndex)
        End Sub
    End Class
End Namespace
