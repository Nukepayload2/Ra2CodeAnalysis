Imports System.Text

Namespace Document
    Public Class IniWrongSyntaxTrivia
        Inherits IniSyntaxTrivia
        Sub New(Text As String, StartIndex%)
            MyBase.New(Text, StartIndex)
        End Sub
        Public Overrides ReadOnly Property ColorARGB As Integer = CodeColors.Red
    End Class
End Namespace

