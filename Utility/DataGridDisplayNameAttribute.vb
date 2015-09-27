Imports System.Reflection

<AttributeUsage(AttributeTargets.Property)>
Public NotInheritable Class DataGridDisplayNameAttribute
    Inherits Attribute
    Public ReadOnly Name As String
    Public Shared Function GetValueDicFromType(tp As Type) As Dictionary(Of String, String)
        Dim NameDic As New Dictionary(Of String, String)
        For Each m As MemberInfo In tp.GetProperties
            Dim Attrib = TryCast(GetCustomAttribute(m, GetType(DataGridDisplayNameAttribute)), DataGridDisplayNameAttribute)
            If Attrib IsNot Nothing Then
                NameDic.Add(m.Name, DirectCast(Attrib, DataGridDisplayNameAttribute).Name)
            Else
                NameDic.Add(m.Name, m.Name)
            End If
        Next
        Return NameDic
    End Function
    Sub New(Name As String)
        MyBase.New
        Me.Name = Name
    End Sub
End Class
