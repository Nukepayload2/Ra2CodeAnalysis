
Public Class INIAnalizeResult
    Public ReadOnly Property Message As New List(Of INIAnalyzeInfo)
    Public ReadOnly Property Warning As New List(Of INIAnalyzeInfo)
    Public ReadOnly Property Fault As New List(Of INIAnalyzeInfo)

    Sub New()

    End Sub
    Sub New(Msg As List(Of INIAnalyzeInfo), Warn As List(Of INIAnalyzeInfo), Fau As List(Of INIAnalyzeInfo))
        Message = Msg
        Warning = Warn
        Fault = Fau
    End Sub
    ''' <summary>
    ''' 返回被合并的两个结果
    ''' </summary> 
    Public Function Concat(AnotherResult As INIAnalizeResult) As INIAnalizeResult
        Dim tmp As New INIAnalizeResult(Message, Warning, Fault)
        tmp.Message.AddRange(AnotherResult.Message)
        tmp.Warning.AddRange(AnotherResult.Warning)
        tmp.Fault.AddRange(AnotherResult.Fault)
        Return tmp
    End Function
End Class