Public Class RulesHelpProvider
    Implements IHelpProvider

    Public Function GetHelpText(code As String) As String Implements IHelpProvider.GetHelpText
        Return New HelpDataProvider().GetHelpTextRules(code)
    End Function
End Class