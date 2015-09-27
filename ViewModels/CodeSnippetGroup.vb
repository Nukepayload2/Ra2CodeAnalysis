Public Class CodeSnippetGroup
    Public ReadOnly Property Name$
    Public ReadOnly Property Group As IList(Of CodeSnippet)
    Sub New(Name$, Group As IList(Of CodeSnippet))
        Me.Name = Name
        Me.Group = Group
    End Sub
End Class
