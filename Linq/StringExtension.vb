Imports System.Text

Namespace Linq
    Public Module StringAggregation
        ''' <summary>
        ''' 带缩进和 Windows 格式 换行的合并
        ''' </summary>
        <Extension>
        Public Function Join(source As IEnumerable(Of String), space As Func(Of String, Integer)) As String
            Dim sb As New StringBuilder
            For Each blk In source
                sb.Append(" "c, space(blk)).AppendLine(blk)
            Next
            sb.Remove(sb.Length - 2, 2)
            Return sb.ToString
        End Function
        ''' <summary>
        ''' 带单字符分隔符的合并
        ''' </summary>
        <Extension>
        Public Function Join(source As IEnumerable(Of String), separator As Func(Of String, String)) As String
            Dim sb As New StringBuilder
            For Each blk In source
                sb.Append(blk).Append(separator(blk))
            Next
            Dim len = separator(String.Empty).Length
            If len > 0 Then sb.Remove(sb.Length - len, len)
            Return sb.ToString
        End Function
    End Module
End Namespace
