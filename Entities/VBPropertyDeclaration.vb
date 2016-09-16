﻿Imports System.Text

Public Class VBPropertyDeclaration
    Sub New(sb As IndentStringBuilder, helpText As String, isObsolete As Boolean, basicInformation As VBPropertyDeclarationSilm, initialValue As String, isPrimaryKey As Boolean)
        Me.sb = sb
        Me.HelpText = helpText
        Me.IsObsolete = isObsolete
        Me.BasicInformation = basicInformation
        Me.InitialValue = initialValue
        Me.IsPrimaryKey = isPrimaryKey
    End Sub

    Dim sb As IndentStringBuilder
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
    Public Property ImplementsInterface As New HashSet(Of VBInterfaceBuilder)
    ''' <summary>
    ''' 将声明转换为 VB 代码
    ''' </summary>
    Public Sub WriteCode()
        WriteXmlComment()
        WriteAttribute()
        WriteClassDecl()
        BasicInformation.WriteInitializeExpression(sb, InitialValue, False)
        WriteImplements()
        sb.AppendLine()
    End Sub

    Private Sub WriteImplements()
        If ImplementsInterface.Count > 0 Then
            sb.Append(" Implements ")
            For Each itf In ImplementsInterface
                sb.Append(itf.Name).Append("."c).Append(BasicInformation.Name).Append(", ")
            Next
            sb.Remove(sb.Length - 2, 2)
        End If
    End Sub

    Private Sub WriteClassDecl()
        Dim isForeignKey = BasicInformation.TypeNameOverride IsNot Nothing
        Dim className = BasicInformation.RuntimeTypeName
        sb.IndentAppend($"Public {If(isForeignKey, "Overridable ", "")}Property {BasicInformation.Name} As ")
        Dim declTypeName = className
        If ImplementsInterface.Count > 0 Then
            For Each itf In ImplementsInterface
                Dim foundProp = itf.Properties(BasicInformation.Name)
                If foundProp IsNot Nothing Then
                    declTypeName = foundProp.RuntimeTypeName
                    Exit For
                End If
            Next
        End If
        sb.Append(declTypeName)
    End Sub

    Private Sub WriteAttribute()
        If IsPrimaryKey Then
            sb.IndentAppendLine("<Key>")
        ElseIf IsObsolete Then
            sb.IndentAppendLine("<Obsolete>")
        End If
    End Sub

    Private Sub WriteXmlComment()
        If Not String.IsNullOrEmpty(HelpText) Then
            sb.IndentAppendLine("'''<summary>")
            sb.IndentAppend("'''").AppendLine(HelpText.Replace(vbCr, "").Replace(vbLf, "").Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;"))
            sb.IndentAppendLine("'''</summary>")
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