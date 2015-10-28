Imports System.Text

Namespace Document
    Public Class IniRecordSyntax
        Inherits IniSyntax
        Sub New(Text As String, StartIndex%)
            MyBase.New(Text, StartIndex)
        End Sub

        Public Overrides Property Text As String
            Get
                Return Children.JoinText(Function(s) s.Text)
            End Get
            Set
                Dim IsWhiteSpace = False
                Dim sb As New StringBuilder
                Dim i%
                Dim NonComment = Value.ExceptComments
                Dim Comment = If(NonComment.Length = Value.Length, "", Value.Substring(NonComment.Length))
                For i = 0 To NonComment.Length - 2
                    Dim ch = NonComment(i)
                    If Char.IsSymbol(ch) OrElse Char.IsPunctuation(ch) Then
                        If sb.Length > 0 Then
                            If IsWhiteSpace Then
                                Children.Add(New IniWhitepaceSyntaxTrivia(sb.ToString, i - sb.Length))
                            Else
                                Children.Add(New IniWordSyntaxTrivia(sb.ToString, i - sb.Length))
                            End If
                            sb.Clear()
                        End If
                        Children.Add(New IniControlCharacterSyntaxTrivia(ch.ToString, i))
                    ElseIf ch = vbCr
                    ElseIf ch = vbLf
                        Children.Add(New IniNewlineSyntaxTrivia(i - 1))
                    ElseIf Char.IsWhiteSpace(ch)
                        If sb.Length > 0 AndAlso Not IsWhiteSpace Then
                            Children.Add(New IniWordSyntaxTrivia(sb.ToString, i - sb.Length))
                            sb.Clear()
                        End If
                        IsWhiteSpace = True
                        sb.Append(ch)
                    Else
                        If sb.Length > 0 AndAlso IsWhiteSpace Then
                            Children.Add(New IniWhitepaceSyntaxTrivia(sb.ToString, i - sb.Length))
                            sb.Clear()
                        End If
                        sb.Append(ch)
                        IsWhiteSpace = False
                    End If
                Next
                i = NonComment.Length - 1
                If Not String.IsNullOrWhiteSpace(NonComment(i)) Then
                    sb.Append(NonComment(i))
                End If
                If sb.Length > 0 Then
                    If IsWhiteSpace Then
                        Children.Add(New IniWhitepaceSyntaxTrivia(sb.ToString, i - sb.Length))
                        sb.Clear()
                    Else
                        Children.Add(New IniWordSyntaxTrivia(sb.ToString, i - sb.Length))
                        sb.Clear()
                    End If
                End If
                If i > 0 AndAlso Value(i) = vbLf Then
                    Children.Add(New IniNewlineSyntaxTrivia(i - 1))
                End If
                If Not String.IsNullOrEmpty(Comment) Then
                    Children.Add(New IniCommentSyntaxTrivia(Comment, NonComment.Length))
                End If
            End Set
        End Property
    End Class
End Namespace
