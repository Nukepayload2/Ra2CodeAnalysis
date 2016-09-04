Public Class VBPropertyDeclarationSilm
    Public Property Name$
    ''' <summary>
    ''' 推断出的基础类型
    ''' </summary>
    Public Property TypeName$
    ''' <summary>
    ''' 如果属性这个不为空，则会取代 <see cref="TypeName"/> 
    ''' </summary>
    Public Property TypeNameOverride As VBTypeBuilder
    ''' <summary>
    ''' 实际生成的属性类型
    ''' </summary>
    Public ReadOnly Property RuntimeTypeName$
        Get
            Return If(TypeNameOverride Is Nothing, TypeName, TypeNameOverride.Name)
        End Get
    End Property
    ''' <summary>
    ''' 将声明转换为 VB 代码
    ''' </summary>
    Public Overrides Function ToString() As String
        Return $"Public Property {Name} As {RuntimeTypeName}"
    End Function
End Class