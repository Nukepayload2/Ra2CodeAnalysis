
Public Class INIAnalizeResult
    Public ReadOnly Message As New List(Of INIAnalizeInfo)
    Public ReadOnly Warning As New List(Of INIAnalizeInfo)
    Public ReadOnly Fault As New List(Of INIAnalizeInfo)

    Sub New()

    End Sub
    Sub New(Msg As List(Of INIAnalizeInfo), Warn As List(Of INIAnalizeInfo), Fau As List(Of INIAnalizeInfo))
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