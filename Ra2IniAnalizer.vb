''' <summary>
''' 包含专门分析Ra2/Ts/Fs/Yr平台的Ini的函数
''' </summary>
Public MustInherit Class Ra2IniAnalizer
    Inherits INIAnalizer
    Public MustOverride Overrides Function Check() As INIAnalizeResult
    Sub New(INIText As StreamReader)
        MyBase.New(INIText)
    End Sub
    Sub New(INIText As String)
        MyBase.New(INIText)
    End Sub
    Protected Sub TypeCheck(Arr As IEnumerable(Of String), ids As IEnumerable(Of Integer), chk As Func(Of String, Boolean), errtxt As String, lst As IList(Of INIAnalizeInfo), Linenum As Integer, MKName As String)
        For Each n In ids
            If Not chk.Invoke(Arr(n)) Then
                lst.Add(New INIAnalizeInfo(Linenum, String.Format(errtxt, n), Arr(n), MKName))
            End If
        Next
    End Sub
    Protected Sub ValueRegistryCheck(RegistryMainKeyName As String, FailText As String, MainKeyOfCheck As String, Value As String, LineNumber As Integer, lst As IList(Of INIAnalizeInfo), ExternalValuesSource As INIAnalizer, Optional SecondaryMainKey As String = Nothing)
        If 0 = Aggregate c In From v In ExternalValuesSource.Values(RegistryMainKeyName) Where v.Value.Item1 = Value Into Count Then
            If String.IsNullOrEmpty(SecondaryMainKey) Then
                lst.Add(New INIAnalizeInfo(LineNumber, FailText, Value, MainKeyOfCheck))
            Else
                ValueRegistryCheck(SecondaryMainKey, FailText, MainKeyOfCheck, Value, LineNumber, lst, ExternalValuesSource)
            End If
        End If
    End Sub
    ''' <summary>
    ''' 值不包含在指定的主键的值则添加指定的列表
    ''' </summary>
    ''' <param name="MainKeyName">主键的名字</param>
    ''' <param name="FailText">如果要添加错误，指定错误的文本</param>
    ''' <param name="Record">正在扫描的记录</param>
    ''' <param name="lst">目标列表</param>
    Protected Sub ValueRegistryCheck(MainKeyName As String, FailText As String, MainKeyOfCheck As String, Record As KeyValuePair(Of String, Tuple(Of String, Integer)), lst As IList(Of INIAnalizeInfo), Optional SecondaryMainKey As String = Nothing)
        ValueRegistryCheck(MainKeyName, FailText, MainKeyOfCheck, Record.Value.Item1, Record.Value.Item2, lst, Me, SecondaryMainKey)
    End Sub
    ''' <summary>
    ''' 主键不包含在指定的主键的值则添加指定的列表
    ''' </summary>
    ''' <param name="MainKeyName">主键的名字</param>
    ''' <param name="FailText">如果要添加错误，指定错误的文本</param>
    ''' <param name="Record">正在扫描的记录</param>
    ''' <param name="lst">目标列表</param>
    Protected Sub MainKeyRegistryCheck(MainKeyName As String, FailText As String, MainKeyOfCheckingKV As String, Record As KeyValuePair(Of String, Tuple(Of String, Integer)), lst As IList(Of INIAnalizeInfo), Optional SecodaryMainKeyName As String = Nothing)
        Dim Value = Record.Value
        If 0 = Aggregate c In From v In Values(MainKeyName) Where v.Value.Item1 = MainKeyOfCheckingKV Into Count Then
            If String.IsNullOrEmpty(SecodaryMainKeyName) Then
                lst.Add(New INIAnalizeInfo(Value.Item2, FailText, Value.Item1, MainKeyOfCheckingKV))
            Else
                MainKeyRegistryCheck(SecodaryMainKeyName, FailText, MainKeyOfCheckingKV, Record, lst)
            End If
        End If
    End Sub
    ''' <summary>
    ''' 对用逗号分隔开的所有值进行注册检查
    ''' </summary>
    ''' <param name="r">要检查的键值对</param>
    ''' <param name="ls">要写入的记录</param>
    ''' <param name="MK">注册用的主键</param>
    ''' <param name="Tx">失败时的文本</param>
    ''' <param name="FindMKey">被检查的键值对的主键</param>
    ''' <param name="SecondaryMainKey">备用的注册用的主键</param>
    Protected Sub EachValueRegistryCheck(r As KeyValuePair(Of String, Tuple(Of String, Integer)), ls As List(Of INIAnalizeInfo), MK As String, Tx As String, FindMKey As String, Optional SecondaryMainKey As String = Nothing)
        For Each val In r.Value.Item1.Split(","c)
            If String.IsNullOrEmpty(val) Then
                ls.Add(New INIAnalizeInfo(r.Value.Item2, "语法错误：','前后应为数值", r.Value.Item1, MK))
            Else
                ValueRegistryCheck(FindMKey, Tx, MK, New KeyValuePair(Of String, Tuple(Of String, Integer))(r.Key, New Tuple(Of String, Integer)(val, r.Value.Item2)), ls, SecondaryMainKey)
            End If
        Next
    End Sub
    Protected Sub EachValueRegistryCheck(r As KeyValuePair(Of String, Tuple(Of String, Integer)), ls As List(Of INIAnalizeInfo), MK As String, Tx As String, FindMKey As String, ExternalValuesSource As INIAnalizer, Optional SecondaryMainKey As String = Nothing)
        For Each val In r.Value.Item1.Split(","c)
            If String.IsNullOrEmpty(val) Then
                ls.Add(New INIAnalizeInfo(r.Value.Item2, "语法错误：','前后应为数值", r.Value.Item1, MK))
            Else
                ValueRegistryCheck(FindMKey, Tx, MK, val, r.Value.Item2, ls, ExternalValuesSource, SecondaryMainKey)
            End If
        Next
    End Sub
    ''' <summary>
    ''' 异步获取分析结果
    ''' </summary>
    ''' <returns></returns>
    Public Function CheckAsync() As Task(Of INIAnalizeResult)
        Return New Task(Of INIAnalizeResult)(AddressOf Check)
    End Function

End Class
