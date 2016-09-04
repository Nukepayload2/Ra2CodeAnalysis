Imports System.Text

Namespace Linq
    Public Module StringAggregation
        <Extension>
        Public Function Join(source As IEnumerable(Of String), space As Func(Of String, Integer)) As String
            Dim sb As New StringBuilder
            For Each blk In source
                sb.Append(" "c, space(blk)).AppendLine(blk)
            Next
            sb.Remove(sb.Length - 2, 2)
            Return sb.ToString
        End Function
    End Module
End Namespace
