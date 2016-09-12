Public Class EmptyHelpProvider
    Inherits HelpProvider

    Public Overrides Function GetHelpText(code As String) As String
        Return String.Empty
    End Function
End Class
