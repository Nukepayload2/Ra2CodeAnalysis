Public Class CodeColorBlock(Of TColor)
    Public StartIndex As Integer
    Public Length As Integer
    Public Color As TColor
    Sub New(Index As Integer, Length As Integer, Color As TColor)
        StartIndex = Index
        Me.Length = Length
        Me.Color = Color
    End Sub
End Class
