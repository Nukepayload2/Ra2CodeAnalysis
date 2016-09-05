Imports System.Text

Public Class VBNamespaceBuilder

    Sub New(sb As StringBuilder, name$, indent As StrongBox(Of Integer))
        Me.sb = New IndentStringBuilder(sb, indent)
    End Sub

    Public Sub BeginBlock(sb As IndentStringBuilder, name As String)
        sb.IndentAppendLine(name).IncreaseIndent()
    End Sub

    Friend sb As IndentStringBuilder

    Public Property Types As New List(Of VBTypeBuilder)

    Public Sub EndBlock()
        sb.DecreaseIndent.IndentAppendLine("End Namespace")
    End Sub

End Class
