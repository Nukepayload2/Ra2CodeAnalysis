''' <summary>
''' 用于传递属性和为值获取和设置提供通知
''' </summary>
''' <typeparam name="T"></typeparam>
Public Class ParameterizedProperty(Of T)
    Public ReadOnly Property Getter As Func(Of T)
    Public ReadOnly Property Setter As Action(Of T)
    Public ReadOnly Property IsReadOnly As Boolean
        Get
            Return Getter IsNot Nothing AndAlso Setter Is Nothing
        End Get
    End Property
    Public ReadOnly Property IsWriteOnly As Boolean
        Get
            Return Getter Is Nothing AndAlso Setter IsNot Nothing
        End Get
    End Property
    Public Property Value As T
        Get
            Return Getter().Invoke
        End Get
        Set
            Setter.Invoke(Value)
        End Set
    End Property
    Public Shared Narrowing Operator CType(Prop As ParameterizedProperty(Of T)) As T
        Return Prop.Value
    End Operator
    Sub New(Getter As Func(Of T), Setter As Action(Of T))
        Me.Getter = Getter
        Me.Setter = Setter
    End Sub
End Class
