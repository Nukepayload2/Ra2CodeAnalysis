Imports Nukepayload2.Ra2CodeAnalysis

Public Class VBPropertyAssignmentDeclaration
    Public Sub New(propertyBasicInformation As VBPropertyDeclarationSilm, initialValue As String)
        Me.PropertyBasicInformation = propertyBasicInformation
        Me.InitialValue = initialValue
    End Sub

    Public Property PropertyBasicInformation As VBPropertyDeclarationSilm
    Public Property InitialValue As String
End Class

Public Class VBPropertyAssignmentDeclarationNameComparer
    Implements IEqualityComparer(Of VBPropertyAssignmentDeclaration)

    Public Overloads Function Equals(x As VBPropertyAssignmentDeclaration, y As VBPropertyAssignmentDeclaration) As Boolean Implements IEqualityComparer(Of VBPropertyAssignmentDeclaration).Equals
        Return x.PropertyBasicInformation.Name = y.PropertyBasicInformation.Name
    End Function

    Public Overloads Function GetHashCode(obj As VBPropertyAssignmentDeclaration) As Integer Implements IEqualityComparer(Of VBPropertyAssignmentDeclaration).GetHashCode
        Return obj.PropertyBasicInformation.Name.GetHashCode
    End Function
End Class