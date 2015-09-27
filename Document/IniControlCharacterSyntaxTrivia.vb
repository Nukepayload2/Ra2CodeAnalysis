Namespace Document
    Public Class IniControlCharacterSyntaxTrivia
        Inherits IniSyntaxTrivia

        Public Overrides ReadOnly Property ColorARGB As Integer = CodeColors.DarkRed
        Sub New(Text As String, StartIndex%)
            MyBase.New(Text, StartIndex)
        End Sub
    End Class
End Namespace
