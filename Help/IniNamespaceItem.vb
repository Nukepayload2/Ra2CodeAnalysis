Public Class IniNamespaceItem
    Public KeyName As String
    Public Usage As List(Of DescTypePair)
    Sub New(Key As String, Desc As String, TpName As String)
        KeyName = Key
        Usage = New List(Of DescTypePair) From {New DescTypePair(Desc, TpName)}
    End Sub
End Class
Public Class DescTypePair
    Public Description As String
    Public TypeName As String
    Sub New(Desc As String, TpName As String)
        Description = Desc
        TypeName = TpName
    End Sub
End Class