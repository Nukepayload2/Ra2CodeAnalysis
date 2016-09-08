Public Class SingleInstance(Of T As SingleInstance(Of T))
    Public Shared ReadOnly Property Instance As T
    Sub New()
        _Instance = CType(Me, T)
    End Sub
End Class
