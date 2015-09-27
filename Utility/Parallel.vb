Module Parallel
    Friend Async Function ForEachAsync(Of T)(DataSource As IEnumerable(Of T), Proc As Action(Of T)) As Task
        Await TaskEx.WhenAll(From d In DataSource Select TaskEx.Run(Sub() Proc(d)))
    End Function
    Friend Async Function ForEachAsync(Of T, TResult)(DataSource As IEnumerable(Of T), Proc As Func(Of T, TResult)) As Task(Of TResult())
        Return Await TaskEx.WhenAll(From d In DataSource Select TaskEx.Run(Function() Proc(d)))
    End Function
End Module
