Imports System.Text

Public Class VBPropertyDeclaration
    Sub New(indent As StrongBox(Of Integer), helpText As String, isObsolete As Boolean, basicInformation As VBPropertyDeclarationSilm, initialValue As String, isPrimaryKey As Boolean)
        Me.Indent = indent
        Me.HelpText = helpText
        Me.IsObsolete = isObsolete
        Me.BasicInformation = basicInformation
        Me.InitialValue = initialValue
        Me.IsPrimaryKey = isPrimaryKey
    End Sub

    Public Property Indent As StrongBox(Of Integer)
    ''' <summary>
    ''' 显示帮助文本（如果有的话）
    ''' </summary>
    Public Property HelpText As String
    ''' <summary>
    ''' 对于非主键属性，是否已经过时了
    ''' </summary>
    Public Property IsObsolete As Boolean
    ''' <summary>
    ''' 名称和类型名称
    ''' </summary>
    Public Property BasicInformation As VBPropertyDeclarationSilm
    ''' <summary>
    ''' 如果 <see cref="VBPropertyDeclarationSilm.TypeNameOverride"/> 为空, 在 Sub New 使用这部分内容初始化这个属性
    ''' </summary>
    Public Property InitialValue As String
    ''' <summary>
    ''' 是否添加 KeyAttribute
    ''' </summary>
    Public Property IsPrimaryKey As Boolean
    ''' <summary>
    ''' 添加 Implements 语句
    ''' </summary>
    Public Property ImplementsInterface As VBInterfaceBuilder
    ''' <summary>
    ''' 将声明转换为 VB 代码
    ''' </summary>
    Public Overrides Function ToString() As String
        Dim sb As New StringBuilder(100)
        WriteXmlComment(sb)
        WriteAttribute(sb)
        WriteClassDecl(sb)
        WriteImplements(sb)
        BasicInformation.WriteInitializeExpression(New IndentStringBuilder(sb, Indent), InitialValue)
        Return sb.ToString
    End Function

    Private Sub WriteImplements(sb As StringBuilder)
        If ImplementsInterface IsNot Nothing Then
            sb.Append(" Implements ").Append(ImplementsInterface.Name).Append("."c).Append(BasicInformation.Name)
        End If
    End Sub

    Private Sub WriteClassDecl(sb As StringBuilder)
        Dim isForeignKey = BasicInformation.TypeNameOverride IsNot Nothing
        Dim className = BasicInformation.RuntimeTypeName
        sb.Append($"Public {If(isForeignKey, "Overridable ", "")}Property {BasicInformation.Name} As ")
        Dim declTypeName = className
        If ImplementsInterface IsNot Nothing Then
            Dim foundProp = Aggregate p In ImplementsInterface.Properties Where p.Name = BasicInformation.Name Into FirstOrDefault
            If foundProp IsNot Nothing Then
                declTypeName = foundProp.RuntimeTypeName
            End If
        End If
        sb.Append(declTypeName)
    End Sub

    Private Sub WriteAttribute(sb As StringBuilder)
        If IsPrimaryKey Then
            sb.Append("<Key>")
        ElseIf IsObsolete Then
            sb.Append("<Obsolete>")
        End If
    End Sub

    Private Sub WriteXmlComment(sb As StringBuilder)
        If Not String.IsNullOrEmpty(HelpText) Then
            sb.AppendLine($"'''<summary>{HelpText.Replace(vbCr, "").Replace(vbLf, "").Replace("<", "&lt;").Replace(">", "&gt;").Replace("&", "&amp;")}</summary>").Append(" "c, Indent.Value)
        End If
    End Sub


    Public ReadOnly Property HasInitializeExpr As Boolean
        Get
            Dim isForeignKey = BasicInformation.TypeNameOverride IsNot Nothing
            Dim className = BasicInformation.RuntimeTypeName
            Return isForeignKey OrElse (Not isForeignKey AndAlso String.IsNullOrEmpty(InitialValue))
        End Get
    End Property
End Class