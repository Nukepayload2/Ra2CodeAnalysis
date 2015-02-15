
Public Class AIHelpProvider
    Implements IHelpProvider

    Public Function GetHelpText(code As String) As String Implements IHelpProvider.GetHelpText
        Return New HelpDataProvider().GetHelpTextAI(code)
    End Function
End Class