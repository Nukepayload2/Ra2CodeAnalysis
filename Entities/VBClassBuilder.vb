Imports Nukepayload2.Ra2CodeAnalysis

Public Class VBClassBuilder
    Inherits VBTypeBuilder

    Public Sub New(nsBuilder As VBNamespaceBuilder, name As String, incident As StrongBox(Of Integer))
        MyBase.New(nsBuilder, VBTypeCategory.ClassType, name, incident)
    End Sub

    Public Property Properties As New List(Of VBPropertyDeclaration)

    Protected Overrides Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                ' 将缓存的 Properties 写入
                Flush()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    Private Sub Flush()
        Dim sb = nsBuilder.sb
        For Each prop In Properties
            sb.Append(" "c, Incident.Value).AppendLine(prop.ToString)
        Next
    End Sub
End Class
