Public Class FindSymbolResult
    Public ReadOnly Property FileName$
    Public ReadOnly Property MainKey$
    Public ReadOnly Property LineNumber%
    Public ReadOnly Property Text$
    Public ReadOnly Property Remark$
    Sub New(FileName$, MainKey$, LineNumber%, Text$, Remark$)
        Me.FileName = FileName
        Me.MainKey = MainKey
        Me.LineNumber = LineNumber
        Me.Text = Text
        Me.Remark = Remark
    End Sub
End Class
