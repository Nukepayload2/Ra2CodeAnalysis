Public Class VBInterfaceBuilder
    Inherits VBTypeBuilder
    ''' <param name="nsBuilder">命名空间构建器</param>
    ''' <param name="name">接口名，但是不带前缀I</param>
    ''' <param name="indent">缩进</param>
    Public Sub New(nsBuilder As VBNamespaceBuilder, name As String, indent As StrongBox(Of Integer))
        MyBase.New(nsBuilder, VBTypeCategory.InterfaceType, "I" + name, indent)
        PossibleBaseClass = New VBClassBuilder(nsBuilder, name + "Base", indent)
    End Sub

    Public ReadOnly Property PropertyNameIndex As New HashSet(Of String)
    Public ReadOnly Property Properties As New List(Of VBPropertyDeclarationSilm)
    Public Property PossibleBaseClass As VBClassBuilder
    Public Overrides Sub EndBlock()
        Dim sb = nsBuilder.sb
        For Each prop In Properties
            sb.IndentAppendLine(prop.ToString)
        Next
        MyBase.EndBlock()
    End Sub

End Class
