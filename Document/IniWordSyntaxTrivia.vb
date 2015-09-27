Namespace Document
    Public Class IniWordSyntaxTrivia
        Inherits IniSyntaxTrivia
        Public Overrides ReadOnly Property ColorARGB As Integer = CodeColors.Black
        Sub New(Text As String, StartIndex%)
            MyBase.New(Text, StartIndex)
        End Sub
    End Class
End Namespace
