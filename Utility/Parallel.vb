Public Module AsyncParallel
    Public Async Function ForEachAsync(Of T)(DataSource As IEnumerable(Of T), Proc As Action(Of T)) As Task
        Await Task.WhenAll(From d In DataSource Select Task.Run(Sub() Proc(d)))
    End Function
    Public Async Function ForEachAsync(Of T, TResult)(DataSource As IEnumerable(Of T), Proc As Func(Of T, TResult)) As Task(Of TResult())
        Return Await Task.WhenAll(From d In DataSource Select Task.Run(Function() Proc(d)))
    End Function
End Module
