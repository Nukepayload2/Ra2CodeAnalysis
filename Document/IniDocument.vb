Imports System.Text

Namespace Document
    ''' <summary>
    ''' 表示完整的Ini文档信息
    ''' </summary>
    Public Class IniDocument(Of TObservable As {IList(Of IniBlock), New})
        Public Overridable Property Text As String
            Get
                Dim sb As New StringBuilder
                For Each blk In Children
                    sb.AppendLine(blk.Text)
                Next
                Return sb.ToString
            End Get
            Set(value As String)
                ReloadBlocks(value)
            End Set
        End Property
        Protected Function GetNewBlocks(Value$) As TObservable
            Dim NewBlocks As New TObservable
            NewBlocks.Clear()
            Dim ln = Value.Split({vbCrLf}, StringSplitOptions.None)
            Dim StartIndexCounter = 0
            If ln.Length >= 1 Then
                Dim CurBlk As New StringBuilder
                Dim LastComment = Not ln(0).Trim.StartsWith("[")
                Dim BlockIsComment = LastComment
                CurBlk.AppendLine(ln(0))
                For i = 1 To ln.Length - 1
                    Dim lt = ln(i).Trim
                    Dim CurIsRecordStart = lt.StartsWith("[")
                    Dim CurComment = Not (CurIsRecordStart OrElse lt.Split(";"c)(0).Contains("="))
                    If BlockIsComment Then
                        If Not CurComment Then
                            If CurBlk.Length > 0 Then CurBlk.Remove(CurBlk.Length - 2, 2)
                            StartIndexCounter += CurBlk.Length
                            NewBlocks.Add(New IniCommentBlock(CurBlk.ToString, StartIndexCounter))
                            CurBlk.Clear()
                            BlockIsComment = False
                        End If
                        CurBlk.AppendLine(ln(i))
                    Else
                        If CurComment Then
                            Dim j%
                            For j = i + 1 To ln.Length - 1
                                Dim ltp = ln(j).TrimStart
                                Dim PeekIsRecordStart = ltp.StartsWith("[")
                                Dim PeekIsRecord = ltp.Split(";"c)(0).Contains("=")
                                If PeekIsRecordStart Then
                                    If CurBlk.Length > 0 Then CurBlk.Remove(CurBlk.Length - 2, 2)
                                    StartIndexCounter += CurBlk.Length
                                    NewBlocks.Add(New IniRecordBlock(CurBlk.ToString, StartIndexCounter))
                                    CurBlk.Clear()
                                    CurBlk.AppendLine(ln(i))
                                    BlockIsComment = True
                                    Exit For
                                ElseIf PeekIsRecord Then
                                    CurBlk.AppendLine(ln(i))
                                    Exit For
                                End If
                            Next
                            If j = ln.Length Then
                                CurBlk.AppendLine(ln(i))
                            End If
                        Else
                            CurBlk.AppendLine(ln(i))
                        End If
                    End If
                Next
                If CurBlk.Length > 0 Then
                    CurBlk.Remove(CurBlk.Length - 2, 2)
                    StartIndexCounter += CurBlk.Length
                    NewBlocks.Add(New IniRecordBlock(CurBlk.ToString, StartIndexCounter))
                End If
            End If
            Return NewBlocks
        End Function
        Public Sub ReloadBlocks(Value$)
            Dim Blk = GetNewBlocks(Value)
            If Blk.Count = Children.Count Then
                For i = 0 To Blk.Count - 1
                    If Children(i).Text <> Blk(i).Text Then
                        Children(i) = Blk(i)
                    End If
                Next
            Else
                Children.Clear()
                For Each b In Blk
                    Children.Add(b)
                Next
            End If
        End Sub
        Public Property Children As New TObservable
        Sub New()

        End Sub
        Sub New(Text$)
            Me.Text = Text
        End Sub
    End Class
End Namespace
