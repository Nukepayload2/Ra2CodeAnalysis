Imports Nukepayload2.Ra2CodeAnalysis.Linq

Public Class VBClassBuilder
    Inherits VBTypeBuilder

    Public Sub New(nsBuilder As VBNamespaceBuilder, name As String, indent As StrongBox(Of Integer))
        MyBase.New(nsBuilder, VBTypeCategory.ClassType, name, indent)
    End Sub

    Public Property PropertyNameIndex As New HashSet(Of String)
    Public Property Properties As New List(Of VBPropertyDeclaration)
    Public Property ExtraPropertyInitialization As New List(Of VBPropertyAssignmentDeclaration)
    Public Property ImplementInterfaces As New List(Of VBInterfaceBuilder)
    Public Property InheritsClass As VBClassBuilder

    Public Overrides Sub EndBlock()
        Dim sb = nsBuilder.sb
        If ImplementInterfaces.Any Then
            sb.IndentAppend("Implements ").Append(Aggregate imp In ImplementInterfaces Select imp.Name Into Join(","c))
        End If
        If InheritsClass IsNot Nothing Then
            sb.IndentAppend("Inherits ").Append(InheritsClass.Name)
        End If
        For Each prop In Properties
            sb.IndentAppendLine(prop.ToString)
        Next
        sb.IndentAppend("Sub New()").IncreaseIndent()
        For Each prop In ExtraPropertyInitialization
            sb.IndentAppend(prop.PropertyBasicInformation.Name)
            prop.PropertyBasicInformation.WriteInitializeExpression(sb, prop.InitialValue)
        Next
        sb.DecreaseIndent.IndentAppend("End Sub")
        MyBase.EndBlock()
    End Sub
End Class
