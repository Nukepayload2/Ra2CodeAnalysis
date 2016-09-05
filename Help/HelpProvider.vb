Public MustInherit Class HelpProvider
    Inherits HelpDataProvider
    Implements IHelpProvider

    Public MustOverride Function GetHelpText(code As String) As String Implements IHelpProvider.GetHelpText
End Class
