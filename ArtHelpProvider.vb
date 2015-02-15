
Public Class ArtHelpProvider
    Implements IHelpProvider

    Public Function GetHelpText(code As String) As String Implements IHelpProvider.GetHelpText
        Return New HelpDataProvider().GetHelpTextArt(code)
    End Function
End Class