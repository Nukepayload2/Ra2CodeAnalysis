Imports System.Threading

Public Class ResetableDelayTask
    Dim DelayMilsec As Integer
    Public ReadOnly Property IsStopped As Boolean = False
    Dim CurrentDelay As Integer = 0
    Public Sub Reset()
        SyncLock New Object
            CurrentDelay = 0
        End SyncLock
    End Sub
    Public Async Function Run(ac As Action) As Task
        If IsStopped Then
            Reset()
            Return
        End If
        SyncLock New Object
            _IsStopped = True
        End SyncLock
        Do While CurrentDelay < DelayMilsec
            Await TaskEx.Delay(16)
            If Not IsStopped Then Return
            Interlocked.Add(CurrentDelay, 16)
        Loop
        Await TaskEx.Run(ac)
        SyncLock New Object
            _IsStopped = False
        End SyncLock
    End Function

    Sub New(DelayTime As TimeSpan)
        DelayMilsec = CInt(DelayTime.TotalMilliseconds)
    End Sub
End Class
