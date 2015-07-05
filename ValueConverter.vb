Public Module ValueConverter
    <Extension>
    Public Function ToYesNo(Value As Boolean) As String
        Return If(Value, "yes", "no")
    End Function
End Module
