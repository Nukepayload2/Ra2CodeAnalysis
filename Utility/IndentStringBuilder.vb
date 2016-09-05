Imports System.Text

Public Class IndentStringBuilder
    Dim sb As New StringBuilder

    Sub New(sb As StringBuilder, indent As StrongBox(Of Integer))
        Me.sb = sb
        Me.indent = indent
    End Sub

    Dim indent As StrongBox(Of Integer)
    Public Function IncreaseIndent() As IndentStringBuilder
        indent.Value += 4
        Return Me
    End Function
    Public Function DecreaseIndent() As IndentStringBuilder
        indent.Value -= 4
        Return Me
    End Function
    Public Function Append(str As String) As IndentStringBuilder
        sb.Append(str)
        Return Me
    End Function
    Public Function IndentAppend(str As String) As IndentStringBuilder
        sb.Append(" "c, indent.Value).Append(str)
        Return Me
    End Function
    Public Function AppendLine(str As String) As IndentStringBuilder
        sb.AppendLine(str)
        Return Me
    End Function
    Public Function IndentAppendLine(str As String) As IndentStringBuilder
        sb.Append(" "c, indent.Value).AppendLine(str)
        Return Me
    End Function
    Public Function AppendLineIndent(str As String) As IndentStringBuilder
        sb.AppendLine(str).Append(" "c, indent.Value)
        Return Me
    End Function
End Class
