Namespace Document
    Public Class IniCommentSyntax
        Inherits IniSyntax
        Sub New(Text As String, StartIndex%)
            MyBase.New(Text, StartIndex)
        End Sub

        Public Overrides Property Text As String
            Get
                Return Children.JoinText(Function(s) s.Text)
            End Get
            Set
                Select Case Children.Count
                    Case 0
                        Children.Add(New IniWrongSyntaxTrivia(Value, 0))
                    Case 1
                        If Value.EndsWith(vbCrLf) Then
                            Value = Value.Substring(0, Value.Length - 2)
                            Children(0).Text = Value
                            Children.Add(New IniNewlineSyntaxTrivia(Value.Length))
                        Else
                            Children(0).Text = Value
                        End If
                    Case Else
                        If Value.EndsWith(vbCrLf) Then
                            Value = Value.Substring(0, Value.Length - 2)
                            Children(0).Text = Value
                        Else
                            Children.RemoveAt(1)
                            Children(0).Text = Value
                        End If
                End Select
            End Set
        End Property
    End Class
End Namespace
