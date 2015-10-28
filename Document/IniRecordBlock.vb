Imports System.Text

Namespace Document
    Public Class IniRecordBlock
        Inherits IniBlock

        Sub New(Text As String, StartIndex%)
            MyBase.New(Text, StartIndex)
        End Sub

        Public Overrides Property Text As String
            Get
                If CurrentLoadOption = DocumentLoadOptions.SeparateBlocksOnly Then
                    Return _Text
                Else
                    Return Children.JoinText(Function(s) s.Text)
                End If
            End Get
            Set
                If CurrentLoadOption = DocumentLoadOptions.SeparateBlocksOnly Then
                    _Text = Value
                    Children.Clear()
                Else
                    Dim Children As New List(Of IniSyntax)
                    Dim NewBlocks = Value.Split({vbCrLf}, StringSplitOptions.None)
                    Dim Position% = 0
                    If NewBlocks.Count > 0 Then
                        Dim FirstBlk = NewBlocks(0)
                        Dim MainKeyPart = FirstBlk.ExceptComments
                        Dim TrimedMKPart = MainKeyPart.Trim
                        If NewBlocks.Count > 1 Then
                            FirstBlk &= vbCrLf
                        End If
                        If TrimedMKPart.StartsWith("[") AndAlso TrimedMKPart.EndsWith("]") Then
                            Children.Add(New IniMainKeySyntax(FirstBlk, 0))
                        Else
                            Children.Add(New IniWrongSyntax(FirstBlk, 0, "主键没有']'"))
                        End If
                        Position += FirstBlk.Length
                        For i = 1 To NewBlocks.Length - 1
                            Dim CurBlk = NewBlocks(i)
                            Dim MainPart = CurBlk.ExceptComments
                            If i < NewBlocks.Length - 1 Then
                                CurBlk &= vbCrLf
                            End If
                            If MainPart.Contains("=") Then
                                Children.Add(New IniKeyValuePairSyntax(CurBlk, Position))
                            ElseIf MainPart.TrimStart.StartsWith(";")
                                Children.Add(New IniCommentSyntax(CurBlk, Position))
                            Else
                                Children.Add(New IniWrongSyntax(CurBlk, Position, "注释没有;"))
                            End If
                            Position += CurBlk.Length
                        Next
                    End If
                    Me.Children.ReloadContent(Children, Function(s) s.Text)
                End If
            End Set
        End Property
    End Class
End Namespace
