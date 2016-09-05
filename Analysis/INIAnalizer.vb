''' <summary>
''' 对未拓展的Ini进行分析
''' </summary>
Public Class INIAnalizer
    ''' <summary>
    ''' 不加载任何信息。只能在关于类型的情况使用
    ''' </summary>
    Sub New()

    End Sub
    Public Overridable Function Check() As INIAnalizeResult
        Return Result
    End Function
    Public Overridable ReadOnly Property Result As New INIAnalizeResult
    ''' <summary>
    ''' 主键，键和值。如果主键重复注册则合并，如果键重复注册则进入ConflictValues。
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property Values As New Dictionary(Of String, Dictionary(Of String, Tuple(Of String, Integer)))
    ''' <summary>
    ''' 重复注册的值的多余的部分
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property ConflictValues As New Dictionary(Of String, List(Of KeyValuePair(Of String, String)))
    Protected Sub LineProc(tx As String, ln As Integer, ByRef curMK As String)
        If tx.Contains(";") Then
            tx = tx.Substring(0, tx.IndexOf(";"c)).Trim
        End If
        If Not String.IsNullOrEmpty(tx) Then
            If tx.StartsWith("[") Then
                If tx.Length = 1 Then
                    Result.Fault.Add(New INIAnalyzeInfo(ln, "语法错误：空的主键", tx, curMK))
                    Return
                End If
                curMK = tx.Substring(1, tx.Length - 2).Trim
                If String.IsNullOrWhiteSpace(curMK) Then
                    Result.Fault.Add(New INIAnalyzeInfo(ln, "语法错误：空的主键", tx, curMK))
                End If
                If Not Values.ContainsKey(curMK) Then
                    Values.Add(curMK, New Dictionary(Of String, Tuple(Of String, Integer)))
                End If
            ElseIf tx.Contains("=")
                If Values.ContainsKey(curMK) Then
                    Dim spl As Integer = tx.IndexOf("="c)
                    Dim left = tx.Substring(0, spl).Trim
                    Dim rig = tx.Substring(spl + 1, tx.Length - 1 - spl).Trim
                    If String.IsNullOrEmpty(left) Then
                        Result.Warning.Add(New INIAnalyzeInfo(ln, "空的左值(键)，此记录会被忽略", tx, curMK))
                        'ElseIf String.IsNullOrEmpty(rig) Then
                        '    Result.Message.Add(New INIAnalizeInfo(ln, "空的右值(值)，此记录可能无效", tx, curMK))
                    Else
                        If Values(curMK).ContainsKey(left) Then
                            Result.Warning.Add(New INIAnalyzeInfo(ln, "重复注册，此记录会被忽略", tx, curMK))
                            If ConflictValues.ContainsKey(curMK) Then
                                ConflictValues(curMK).Add(New KeyValuePair(Of String, String)(left, rig))
                            Else
                                ConflictValues.Add(curMK, New List(Of KeyValuePair(Of String, String)))
                            End If
                        Else
                            Values(curMK).Add(left, New Tuple(Of String, Integer)(rig, ln))
                        End If
                    End If
                Else
                    Result.Warning.Add(New INIAnalyzeInfo(ln, "内容无效：记录应包括在主键内", tx, "(没有主键)"))
                End If
            Else
                Result.Warning.Add(New INIAnalyzeInfo(ln, "内容无效：注释应该以;开头", tx, curMK))
            End If
        End If
    End Sub
    Public Async Function ReloadAsync(IniText As String) As Task
        Await Task.Run(Sub() Reload(IniText))
    End Function
    Public Sub Reload(IniText As String)
        Values.Clear()
        ConflictValues.Clear()
        Result.Fault.Clear()
        Result.Message.Clear()
        Result.Warning.Clear()
        Load(IniText)
    End Sub
    Protected Overridable Sub Load(IniText As String)
        Dim curMK As String = String.Empty
        Dim txs = IniText.Split(CChar(vbLf))
        SyncLock New Object
            For ln As Integer = 0 To txs.Length - 1
                Dim tx = txs(ln).Trim
                LineProc(tx, ln, curMK)
            Next
        End SyncLock
    End Sub
    Sub New(INIText As String)
        INIText = If(INIText, "")
        Load(INIText)
        Debug.WriteLine(Me.GetType.Name & " Initialized.")
    End Sub
    Sub New(INIText As StreamReader)
        Dim curMK As String = String.Empty
        Dim ln As Integer = 0
        SyncLock New Object
            Do While Not INIText.EndOfStream
                Dim tx = INIText.ReadLine.Trim
                LineProc(tx, ln, curMK)
                ln += 1
            Loop
        End SyncLock
    End Sub
    ''' <summary>
    ''' 把分析的INI重新写出来,这样能去除重复注册,删除所有注释。
    ''' </summary>
    ''' <returns></returns>
    Public Overrides Function ToString() As String
        Dim sb As New Text.StringBuilder
        sb.AppendLine(";由Nukepayload2.Ra2CodeAnalysis生成")
        SyncLock Me  'Protect Values
            For Each ks In Values.Keys
                sb.AppendLine()
                sb.AppendLine("[" & ks & "]")
                For Each vs In Values(ks)
                    sb.AppendLine(vs.Key & "=" & vs.Value.Item1)
                Next
            Next
        End SyncLock
        Return sb.ToString
    End Function
End Class
