Imports System.Reflection
Imports System.Text
Imports Nukepayload2.Ra2CodeAnalysis.AnalysisHelper

Namespace Imaging
    ''' <summary>
    ''' 用于产生分析图的ini分析器。Values保存着撤销全部更改的结果。
    ''' </summary>
    Public Class IniImagingAnalizer
        Inherits INIAnalyzer
        Sub New(IniText As String)
            MyBase.New(IniText)
        End Sub
        Protected TreeRoot As List(Of MainKeyTreeNode)
        ''' <summary>
        ''' 注意：修改Root会导致分析结果无效
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Root As List(Of MainKeyTreeNode) = TreeRoot

        ''' <summary>
        ''' 将ini分析结果保存为String
        ''' </summary>
        ''' <returns></returns>
        Public Overrides Function ToString() As String
            Return Root.GetText(Me.GetType)
        End Function

        Protected Overrides Sub Load(IniText As String)
            MyBase.Load(IniText)
            TreeRoot = New List(Of MainKeyTreeNode)
            For Each mk In Values.Keys
                Dim CurBranch As New MainKeyTreeNode(mk)
                TreeRoot.Add(CurBranch)
                For Each kv In Values(mk)
                    Dim va = New ValueTreeNode(kv.Value.Item1)
                    Dim Rec As New KeyValuePair(Of KeyTreeNode, ValueTreeNode)(New KeyTreeNode(kv.Key, va), va)
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
                            Exit For
                        End If
                    Next
                Next
            Next
        End Sub
    End Class

End Namespace
