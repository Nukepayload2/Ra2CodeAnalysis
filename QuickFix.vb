Imports Nukepayload2.Ra2CodeAnalysis.AnalysisHelper
Imports Nukepayload2.Ra2CodeAnalysis.Imaging
''' <summary>
''' 修正ini的常见错误
''' </summary>
Public Class QuickFix

    Public Sub RegisterExistingItem(Root As IEnumerable(Of MainKeyTreeNode), NotRegistered As IRegisterable, Target As RegisterTreeNode)
        Target.RegisterAndModify(NotRegistered)
    End Sub

    Public Sub NewMainKey(Root As List(Of MainKeyTreeNode), MKName As String)
        Root.Add(New MainKeyTreeNode(MKName))
    End Sub

    Public Sub AddToKey(root As IEnumerable(Of MainKeyTreeNode), MainKeyName As String, KeyName As String, ValueText As String)
        Dim m = root.FindOne(MainKeyName)
        If m Is Nothing Then
            Throw New ArgumentException("没有相应的主键", NameOf(root))
        End If
        Dim k = m.KeyValues.Keys.FindOne(KeyName)
        If m Is Nothing Then
            Throw New ArgumentException("没有相应的键", NameOf(root))
        End If
        m.KeyValues(k).AddValue(New SubValueTreeNode(ValueText))
    End Sub
End Class
