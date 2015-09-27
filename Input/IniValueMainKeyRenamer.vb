Imports System.Text

Public Class IniValueMainKeyRenamer
    Sub New()

    End Sub
    Public Async Function RenameAsync(MatchItem As String, Replacement As String, RenameOption As RenameOptions, DataSource As IEnumerable(Of ParameterizedProperty(Of String))) As Task
        If CBool(RenameOption And RenameOptions.MainKey) Then
            Await ForEachAsync(DataSource,
                Sub(prop)
                    Dim ini = prop.Value
                    If MatchItem.StartsWith("<") AndAlso MatchItem.EndsWith(">") Then
                        prop.Value = prop.Value.Replace(MatchItem, Replacement)
                        Return
                    End If
                    Dim txs = ini.Split({vbCrLf}, StringSplitOptions.None)
                    Dim sb As New StringBuilder
                    For Each tx In txs
                        Dim Original = tx
                        If tx.Contains(";") Then
                            tx = tx.Substring(0, tx.IndexOf(";"c)).Trim
                        End If
                        If tx.Contains("[") Then
                            sb.Append(Original.Substring(0, Original.IndexOf("["c) + 1))
                            If tx.Length > 1 Then
                                Dim MK = tx.Substring(1, tx.Length - 2).Trim
                                If MK = MatchItem Then
                                    MK = Replacement
                                End If
                                sb.Append(MK).AppendLine(Original.Substring(Original.IndexOf("]"c)))
                            End If
                        Else
                            sb.AppendLine(Original)
                        End If
                    Next
                    prop.Value = sb.ToString
                End Sub)
        End If
        If CBool(RenameOption And RenameOptions.Value) Then
            Await ForEachAsync(DataSource,
                Sub(prop)
                    Dim ini = prop.Value
                    Dim txs = ini.Split({vbCrLf}, StringSplitOptions.None)
                    Dim sb As New StringBuilder
                    For Each tx In txs
                        Dim Original = tx
                        If tx.Contains(";") Then
                            tx = tx.Substring(0, tx.IndexOf(";"c)).Trim
                        End If
                        If tx.Contains("=") Then
                            Dim txEq = tx.Split("="c)
                            sb.Append(txEq(0)).Append("=")
                            Dim txRights = txEq(1).Split(","c)
                            Dim newRights = Aggregate s In txRights Select If(s = MatchItem, Replacement, s) Into ToArray
                            For i = 0 To newRights.Length - 2
                                sb.Append(newRights(i)).Append(",")
                            Next
                            sb.AppendLine(newRights.Last)
                        Else
                            sb.AppendLine(Original)
                        End If
                    Next
                    prop.Value = sb.ToString
                End Sub)
        End If
    End Function
End Class