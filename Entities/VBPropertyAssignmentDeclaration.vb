Public Class VBPropertyAssignmentDeclaration
    Public Sub New(propertyBasicInformation As VBPropertyDeclarationSilm, initialValue As String)
        Me.PropertyBasicInformation = propertyBasicInformation
        Me.InitialValue = initialValue
    End Sub

    Public Property PropertyBasicInformation As VBPropertyDeclarationSilm
    Public Property InitialValue As String
End Class
