Imports Nukepayload2.Ra2CodeAnalysis.AnalysisHelper

Namespace Imaging

    Public Class IniImagingAnalizer
        Inherits INIAnalizer
        Sub New(IniText As String)
            MyBase.New(IniText)
        End Sub
        Protected TreeRoot As  List(Of MainKeyTreeNode)
        Public ReadOnly Property Root As IEnumerable(Of MainKeyTreeNode) = TreeRoot
        Protected Overrides Sub Load(IniText As String)
            MyBase.Load(IniText)
            TreeRoot = New List(Of MainKeyTreeNode)
            For Each mk In Values.Keys
                Dim CurBranch As New MainKeyTreeNode(mk)
                TreeRoot.Add(CurBranch)
                For Each kv In Values(mk)
                    Dim Rec As New KeyValuePair(Of KeyTreeNode, ValueTreeNode)(New KeyTreeNode(kv.Key), New ValueTreeNode(kv.Value.Item1))
                    CurBranch.KeyValues.Add(Rec)
                    If Rec.Key.IsRegisterKey Then
                        For Each v In Rec.Value.Values
                            CurBranch.Register(v)
                        Next
                    End If
                Next
            Next
            For Each r In TreeRoot
                For i As Integer = r.RegisteredValues.Count - 1 To 0 Step -1
                    Dim v = r.RegisteredValues(i)
                    For Each r1 In TreeRoot
                        If r1.Text = v.Text Then
                            r.Register(r1)
                            r.UnRegister(v)
                            Exit For
                        End If
                    Next
                Next
            Next
        End Sub

    End Class

End Namespace
