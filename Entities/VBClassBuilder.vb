Imports Nukepayload2.Ra2CodeAnalysis.Linq

Public Class VBClassBuilder
    Inherits VBTypeBuilder

    Public Sub New(nsBuilder As VBNamespaceBuilder, name As String, indent As StrongBox(Of Integer))
        MyBase.New(nsBuilder, VBTypeCategory.ClassType, name, indent)
    End Sub

    Public Property Properties As New Dictionary(Of String, VBPropertyDeclaration)
    Public Property BasePropertyInitialization As New HashSet(Of VBPropertyAssignmentDeclaration)(New VBPropertyAssignmentDeclarationNameComparer)
    Public Property ImplementInterfaces As New List(Of VBInterfaceBuilder)
    Public Property InheritsClass As VBClassBuilder

    Public Overrides Sub EndBlock()
        Dim sb = nsBuilder.sb
        If InheritsClass IsNot Nothing Then
            sb.IndentAppend("Inherits ").AppendLine(InheritsClass.Name)
        End If
        If ImplementInterfaces.Any Then
            sb.IndentAppend("Implements ").AppendLine(Aggregate imp In ImplementInterfaces Select imp.Name Into Join(","c))
        End If
        For Each prop In Properties.Values
            prop.WriteCode()
            sb.AppendLine()
        Next
        sb.IndentAppendLine("Sub New()").IncreaseIndent()
        For Each prop In BasePropertyInitialization
            sb.IndentAppend(prop.PropertyBasicInformation.Name)
            prop.PropertyBasicInformation.WriteInitializeExpression(sb, prop.InitialValue)
        Next
        sb.DecreaseIndent.IndentAppendLine("End Sub")
        MyBase.EndBlock()
    End Sub
End Class
