﻿Namespace Document
    Public Class IniNewlineSyntaxTrivia
        Inherits IniSyntaxTrivia

        Public Overrides ReadOnly Property ColorARGB As Integer = CodeColors.Black
        Sub New(StartIndex%)
            MyBase.New(vbCrLf, StartIndex)
        End Sub
    End Class
End Namespace
