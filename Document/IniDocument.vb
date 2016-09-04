Imports System.Text

Namespace Document
    Friend Module LoadSetting
        Public CurrentLoadOption As DocumentLoadOptions
    End Module
    ''' <summary>
    ''' 表示完整的Ini文档信息, 以便使用 Linq 对它进行分析。
    ''' </summary>
    Public Class IniDocument(Of TObservable As {IList(Of IniBlock), New})
        Public Property Children As New TObservable
        Dim _Text$ = ""
        Sub New()
            CurrentLoadOption = DocumentLoadOptions.FullAnalysis
        End Sub
        Sub New(Text$, Optional LoadOption As DocumentLoadOptions = DocumentLoadOptions.FullAnalysis)
            Me.Text = Text
            CurrentLoadOption = LoadOption
        End Sub
        Public Overridable Property Text As String
            Get
                Select Case CurrentLoadOption
                    Case DocumentLoadOptions.Store
                        Return _Text
                    Case Else
                        Return Children.JoinLine(Function(s) s.Text)
                End Select
            End Get
            Set(value As String)
                Select Case CurrentLoadOption
                    Case DocumentLoadOptions.Store
                        _Text = value
                        Children.Clear()
                    Case Else
                        ReloadBlocks(value)
                End Select
            End Set
        End Property
        Protected Function GetNewBlocks(Value$) As TObservable
            Dim NewBlocks As New TObservable
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
                            NewBlocks.Add(New IniCommentBlock(CurBlk.ToString, StartIndexCounter))
                            StartIndexCounter += CurBlk.Length
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
                                    NewBlocks.Add(New IniRecordBlock(CurBlk.ToString, StartIndexCounter))
                                    StartIndexCounter += CurBlk.Length
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
            Children.ReloadContent(Blk, Function(s) s.Text)
        End Sub
    End Class
End Namespace
