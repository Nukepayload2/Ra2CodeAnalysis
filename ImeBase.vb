Imports System.Text
Imports Nukepayload2.Ra2CodeAnalysis.AnalysisHelper

Public MustInherit Class ImeBase
    Protected MustOverride ReadOnly Property SelectPosition As Integer
    Protected MustOverride ReadOnly Property ImeList As IList
    MustOverride Property ListVisible As Boolean
    Protected MustOverride Property TextInBox As String
    Protected MustOverride Sub SelectAndSetText(Start As Integer, Length As Integer, Text As String)

    Dim LeftVals As New List(Of String)
    Dim RightVals As New List(Of String)
    Dim MainKeys As New List(Of String)

    Public Sub AutoCompleteFast(CompleteText As String)
        Dim StartPos As Integer = 0
        Dim Selected = SelectText(False, StartPos).Replace("*", "")
        SelectAndSetText(StartPos, Selected.Length, CompleteText)
    End Sub
    Protected Event ItemAddRequired(Item As Object)
    Private Shared IsRunning As Boolean = False
    Protected Sub AddItem(Item As Object) Handles Me.ItemAddRequired
        ImeList.Add(Item)
    End Sub
    Public Sub ViewList()
        ImeList.Clear()
        Dim Left As Boolean = False
        Dim sea = GenerateSearch(Left)
        Dim ls = If(Left, LeftVals, RightVals)
        Dim Que = From i In ls Where i.StartsWith(sea)
        If Left Then
            For Each it In Que '使用Like触发测试版vbc的bug 
                RaiseEvent ItemAddRequired(it & "=")
            Next
        Else
            For Each it In Que '使用Like触发测试版vbc的bug 
                RaiseEvent ItemAddRequired(it)
            Next
        End If
        ListVisible = True
    End Sub
    Protected Function SelectText(ByRef IsLeft As Boolean, ByRef StartPos As Integer) As String
        StartPos = Me.SelectPosition
        IsLeft = True
        If String.IsNullOrEmpty(TextInBox) Then
            Return String.Empty
        End If
        If StartPos > TextInBox.Length Then
            Throw New ArgumentOutOfRangeException("Position", "位置位于字符串外侧")
        End If
        If StartPos = 0 Then
            Return String.Empty
        Else
            Dim wrd As New List(Of Char)
            Do Until StartPos = 0
                StartPos -= 1
                Dim ch = TextInBox.Chars(StartPos)
                If ch = "=" OrElse ch = "," Then IsLeft = False
                If ch.IsRegisterableChar Then
                    wrd.Add(ch)
                ElseIf ch <> " "
                    Exit Do
                End If
            Loop
            StartPos += 1
            wrd.Reverse()
            Return New String(wrd.ToArray)
        End If
    End Function
    Protected Function GenerateSearch(ByRef IsLeft As Boolean) As String
        Return SelectText(IsLeft, 0)
    End Function

    Public Sub Reload(Ini As INIAnalizer)
        MainKeys.Clear()
        LeftVals.Clear()
        RightVals.Clear()
        For Each mk In Ini.Values
            If Not MainKeys.Contains(mk.Key) Then
                MainKeys.Add(mk.Key)
            End If
            For Each kv In mk.Value
                If Not LeftVals.Contains(kv.Key) Then
                    LeftVals.Add(kv.Key)
                End If
                For Each v In kv.Value.Item1.Split(","c)
                    v = v.Trim
                    If Not RightVals.Contains(v) AndAlso Not String.IsNullOrWhiteSpace(v) Then
                        RightVals.Add(v)
                    End If
                Next
            Next
        Next
    End Sub
    Sub New(Ini As INIAnalizer)
        Reload(Ini)
    End Sub
End Class
