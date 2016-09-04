Imports System.Text

Public Module DocumentExtensions
    ''' <summary>
    ''' 去除Ini注释;后面的的内容
    ''' </summary>
    <Extension>
    Public Function ExceptComments(Source$) As String
        Return If(Source.Contains(";"), Source.Substring(0, Source.IndexOf(";"c)), Source)
    End Function
    ''' <summary>
    ''' 把每个内容按换行符分割存入字符串
    ''' </summary>
    <Extension>
    Public Function JoinLine(Of T)(Source As IEnumerable(Of T), GetText As Func(Of T, String)) As String
        Dim sb As New StringBuilder
        For Each blk In Source
            sb.AppendLine(GetText(blk))
        Next
        Return sb.ToString
    End Function
    ''' <summary>
    ''' 把每个内容无分割存入字符串
    ''' </summary>
    <Extension>
    Public Function JoinText(Of T)(Source As IEnumerable(Of T), GetText As Func(Of T, String)) As String
        Dim sb As New StringBuilder
        For Each blk In Source
            sb.Append(GetText(blk))
        Next
        Return sb.ToString
    End Function
    ''' <summary>
    ''' 如果数目一样则改写,不一样就删掉重新添加。
    ''' </summary>
    ''' <typeparam name="T"></typeparam>
    ''' <param name="Dest"></param>
    ''' <param name="Source"></param>
    ''' <param name="GetText"></param>
    <Extension>
    Public Sub ReloadContent(Of T)(Dest As IList(Of T), Source As IList(Of T), GetText As Func(Of T, String))
        If Source.Count = Dest.Count Then
            For i = 0 To Source.Count - 1
                If GetText(Dest(i)) <> GetText(Source(i)) Then
                    Dest(i) = Source(i)
                End If
            Next
        Else
            Dest.Clear()
            For Each b In Source
                Dest.Add(b)
            Next
        End If
    End Sub
End Module
