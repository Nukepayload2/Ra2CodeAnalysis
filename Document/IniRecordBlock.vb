Imports System.Text

Namespace Document
    Public Class IniRecordBlock
        Inherits IniBlock

        Sub New(Text As String, StartIndex%)
            MyBase.New(Text, StartIndex)
        End Sub

        Public Overrides Property Text As String
            Get
                Return Children.JoinText(Function(s) s.Text)
            End Get
            Set
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
                        Else
                            Children.Add(New IniCommentSyntax(CurBlk, Position))
                        End If
                        Position += CurBlk.Length
                    Next
                End If
                Me.Children.ReloadContent(Children, Function(s) s.Text)
            End Set
        End Property
    End Class
End Namespace
