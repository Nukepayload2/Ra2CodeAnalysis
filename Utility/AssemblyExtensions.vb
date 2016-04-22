Imports System.Reflection

Module AssemblyExtensions
    <Extension>
    Function GetCustomAttribute(Of T As Attribute)(asm As Assembly) As T
        Return TryCast(asm.GetCustomAttributes(GetType(T), False).FirstOrDefault, T)
    End Function
    <Extension>
    Function GetCustomAttribute(Of T As Attribute)(asm As MemberInfo) As T
        Return TryCast(asm.GetCustomAttributes(GetType(T), False).FirstOrDefault, T)
    End Function
End Module
