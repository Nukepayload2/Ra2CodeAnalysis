Imports System.Text

Public Class IndentStringBuilder
    Dim sb As New StringBuilder
    Dim indent As New StrongBox(Of Integer)
    Public Property Length As Integer
        Get
            Return sb.Length
        End Get
        Set(value As Integer)
            sb.Length = value
        End Set
    End Property
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
    Public Function AppendLine() As IndentStringBuilder
        sb.AppendLine()
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
    Public Overrides Function ToString() As String
        Return sb.ToString
    End Function
    Public Function Remove(start As Integer, len As Integer) As IndentStringBuilder
        sb.Remove(start, len)
        Return Me
    End Function
End Class
