Imports System.Text

Public Class VBNamespaceBuilder
    Public Property Name As String
    Sub New(sb As IndentStringBuilder, name$)
        Me.sb = sb
        Me.Name = name
    End Sub

    Public Sub BeginBlock()
        sb.IndentAppend("Namespace ").AppendLine(Name).IncreaseIndent()
    End Sub

    Friend sb As IndentStringBuilder

    Public Property Types As New List(Of VBTypeBuilder)

    Public Sub EndBlock()
        sb.DecreaseIndent.IndentAppendLine("End Namespace")
    End Sub

End Class
