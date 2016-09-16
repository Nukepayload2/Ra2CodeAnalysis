﻿Public Class VBPropertyDeclarationSilm
    Sub New(name As String, typeName As String)
        Me.Name = name
        Me.TypeName = typeName
    End Sub

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
    ''' 它是否有多组指向实体的值 
    ''' </summary>
    Public Property IsQueryable As Boolean
    ''' <summary>
    ''' 实际生成的属性类型
    ''' </summary>
    Public ReadOnly Property RuntimeTypeName$
        Get
            Return If(TypeNameOverride Is Nothing, TypeName, If(IsQueryable, "IQueryable(Of " + TypeNameOverride.Name + ")", TypeNameOverride.Name))
        End Get
    End Property
    ''' <summary>
    ''' 将声明转换为 VB 代码, 可在接口中使用。
    ''' </summary>
    Public Overrides Function ToString() As String
        Return $"Property {Name} As {RuntimeTypeName}"
    End Function
    ''' <summary>
    ''' 写入初始化表达式（赋值语句）
    ''' </summary>
    ''' <param name="sb">要写入的目标</param>
    Public Sub WriteInitializeExpression(sb As IndentStringBuilder, InitialValue As String, writeIfNothing As Boolean)
        Dim isForeignKey = TypeNameOverride IsNot Nothing
        Dim className = RuntimeTypeName
        If isForeignKey Then
            If IsQueryable Then
                If InitialValue Is Nothing Then
                    sb.Append($" = Nothing")
                Else
                    sb.Append(InitialValue.Replace(" ", "").Replace("{", " = {New ").Replace(",", ", New "))
                End If
            Else
                If TypeOf TypeNameOverride Is VBInterfaceBuilder Then
                    If InitialValue Is Nothing Then
                        sb.Append(" = Nothing")
                    Else
                        sb.Append($" = New {InitialValue}")
                    End If
                Else
                    sb.Append(" = New ").Append(className)
                End If
            End If
        ElseIf Not String.IsNullOrEmpty(InitialValue) Then
            sb.Append(" = ").Append(InitialValue)
        Else
            If writeIfNothing Then sb.Append(" = Nothing")
        End If
    End Sub
End Class