Namespace Synchronization
    Public Module Events
        Public Event ReloadAllDataFromRecord()
        Public Sub ReloadAllDataFromRecordRequest()
            RaiseEvent ReloadAllDataFromRecord()
        End Sub
    End Module
End Namespace


