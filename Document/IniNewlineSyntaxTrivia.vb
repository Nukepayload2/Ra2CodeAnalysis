Namespace Document
    Public Class IniNewlineSyntaxTrivia
        Inherits IniSyntaxTrivia

        Public Overrides ReadOnly Property ColorARGB As Integer = CodeColors.Transparent
        Sub New(Text As String, StartIndex%)
            MyBase.New(Text, StartIndex)
        End Sub
    End Class
End Namespace
