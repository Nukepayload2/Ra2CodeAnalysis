﻿Imports System.Text

Namespace Document
    Public Class IniMainKeySyntax
        Inherits IniRecordSyntax
        Sub New(Text As String, StartIndex%)
            MyBase.New(Text, StartIndex)
        End Sub
    End Class
End Namespace
