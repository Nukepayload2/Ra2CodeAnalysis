Imports System.Text

Public Class VBPropertyDeclaration
    ''' <summary>
    ''' 名称和类型名称
    ''' </summary>
    Public Property BasicInformation As VBPropertyDeclarationSilm
    ''' <summary>
    ''' 如果不声明为 Foreign Key, 使用这部分内容初始化这个属性
    ''' </summary>
    Public Property InitialValue As String
    ''' <summary>
    ''' 添加 Implements 语句
    ''' </summary>
    Public Property ImplementsInterface As VBInterfaceBuilder
    ''' <summary>
    ''' 是否添加 KeyAttribute
    ''' </summary>
    Public Property IsPrimaryKey As Boolean
    ''' <summary>
    ''' 将声明转换为 VB 代码
    ''' </summary>
    Public Overrides Function ToString() As String
        Dim sb As New StringBuilder(100)
        If IsPrimaryKey Then
            sb.Append("<Key>")
        End If
        Dim isForeignKey = BasicInformation.TypeNameOverride IsNot Nothing
        sb.Append($"Public {If(isForeignKey, "Overridable ", "")}Property {BasicInformation.Name} As ")
        Dim className = BasicInformation.RuntimeTypeName
        Dim declTypeName = className
        If ImplementsInterface IsNot Nothing Then
            Dim foundProp = Aggregate p In ImplementsInterface.Properties Where p.Name = BasicInformation.Name Into FirstOrDefault
            If foundProp IsNot Nothing Then
                declTypeName = foundProp.RuntimeTypeName
            End If
        End If
        sb.Append(declTypeName)
        If isForeignKey Then
            sb.Append(" = New ").Append(className)
        Else
            If Not String.IsNullOrEmpty(InitialValue) Then
                sb.Append(" = ").Append(InitialValue)
            End If
        End If
        If ImplementsInterface IsNot Nothing Then
            sb.Append(" Implements ").Append(ImplementsInterface.Name).Append("."c).Append(BasicInformation.Name)
        End If
        Return sb.ToString
    End Function
End Class