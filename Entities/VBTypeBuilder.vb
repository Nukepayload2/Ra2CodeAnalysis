Public MustInherit Class VBTypeBuilder

    Friend nsBuilder As VBNamespaceBuilder
    Public Property Name$

    Dim category As String

    Public ReadOnly Property HasBegun As Boolean
    Public ReadOnly Property IsEnded As Boolean

    Public Sub New(nsBuilder As VBNamespaceBuilder, category As VBTypeCategory, name$, indent As StrongBox(Of Integer))
        Me.nsBuilder = nsBuilder
        Select Case category
            Case VBTypeCategory.ClassType
                Me.category = "Class"
            Case VBTypeCategory.EnumType
                Me.category = "Enum"
            Case Else
                Me.category = "Interface"
        End Select
        Me.Name = name
    End Sub

    Public Overridable Sub BeginBlock()
        If IsEnded Then
            Throw New InvalidOperationException("语句已经结束了")
        End If
        nsBuilder.sb.IndentAppendLine($"Public {category} {Name}").IncreaseIndent()
        _HasBegun = True
    End Sub

    Public Overridable Sub EndBlock()
        If Not HasBegun Then
            Throw New InvalidOperationException("语句块没有开始")
        End If
        nsBuilder.sb.DecreaseIndent.IndentAppendLine($"End {category}")
        _IsEnded = True
    End Sub

    Public Overrides Function GetHashCode() As Integer
        Return Name.GetHashCode()
    End Function
End Class
