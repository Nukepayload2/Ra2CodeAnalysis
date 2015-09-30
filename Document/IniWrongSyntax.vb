Imports System.Text

Namespace Document
    Public Class IniWrongSyntax
        Inherits IniSyntax
        Public ReadOnly Property Description$

        Public Overrides Property Text As String
            Get
                Return Children.JoinText(Function(s) s.Text)
            End Get
            Set
                If Children.Count = 0 Then
                    Children.Add(New IniWrongSyntaxTrivia(Value, 0))
                Else
                    Children(0).Text = Value
                End If
            End Set
        End Property

        Sub New(Text As String, StartIndex%, Description$)
            MyBase.New(Text, StartIndex)
            Me.Description = Description
        End Sub
    End Class
End Namespace
