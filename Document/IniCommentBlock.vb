Imports System.Text

Namespace Document
    Public Class IniCommentBlock
        Inherits IniBlock
        Sub New(Text As String, StartIndex%)
            MyBase.New(Text, StartIndex)
        End Sub

        Public Overrides Property Text As String
            Get
                Return Children.JoinText(Function(s) s.Text)
            End Get
            Set
                If Children.Count = 0 Then
                    Children.Add(New IniCommentSyntax(Value, 0))
                Else
                    Children.Item(0).Text = Value
                End If
            End Set
        End Property
    End Class

End Namespace
