Imports System.Text

Public Class VBNamespaceBuilder
    Dim name As String
    Sub New(sb As IndentStringBuilder, name$)
        Me.sb = sb
        Me.name = name
    End Sub

    Public Sub BeginBlock()
        sb.IndentAppend("Namespace ").AppendLine(name).IncreaseIndent()
    End Sub

    Friend sb As IndentStringBuilder

    Public Property Types As New List(Of VBTypeBuilder)

    Public Sub EndBlock()
        sb.DecreaseIndent.IndentAppendLine("End Namespace")
    End Sub

End Class
