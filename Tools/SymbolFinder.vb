Imports Nukepayload2.Ra2CodeAnalysis.AnalysisHelper

Public Class SymbolFinder
    Public Async Function Find(MatchText As String, Rules As RulesAnalizer, Art As ArtAnalizer, AI As AIAnalizer) As Task(Of IEnumerable(Of FindSymbolResult))
        Dim Results = Await ForEachAsync(Of Ra2IniAnalizer, IEnumerable(Of FindSymbolResult))({Rules, Art, AI},
                 Function(ini)
                     Dim ls As New List(Of FindSymbolResult)
                     For Each mk In ini.Values
                         If MatchText = mk.Key Then
                             Dim ln = mk.Value.FirstOrDefault.Value.Item2
                             If ln = 0 Then ln = 1
                             ls.Add(New FindSymbolResult(ini.Name, mk.Key, ln - 1, mk.Key, "定义"))
                         End If
                         For Each kv In mk.Value
                             For Each v In kv.Value.Item1.Split(","c)
                                 If v.Trim = MatchText Then
                                     Dim Comment As String
                                     If kv.Key.IsNumeric() Then
                                         Comment = "注册"
                                     Else
                                         Comment = "引用"
                                     End If
                                     ls.Add(New FindSymbolResult(ini.Name, v, kv.Value.Item2, v, Comment))
                                 End If
                             Next
                         Next
                     Next
                     Return ls
                 End Function)
        Dim Heads = Results.First
        For i = 1 To 2
            Heads = Heads.Concat(Results(i))
        Next
        Return Heads
    End Function
End Class
