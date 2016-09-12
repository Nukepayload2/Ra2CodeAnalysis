''' <summary>
''' 表示单精度的百分比
''' </summary>
Public Structure Percentage
    Implements IEquatable(Of Percentage), IComparable(Of Percentage), IFormattable

    Dim SingleValue As Single

    Sub New(singleValue As Single)
        Me.SingleValue = singleValue
    End Sub

    Public Shared Function Parse(text As String) As Percentage
        Dim perc As New Percentage
        If TryParse(text, perc) Then
            Return perc
        End If
        If String.IsNullOrEmpty(text) Then
            text = "空字符串"
        End If
        Throw New InvalidCastException($"无法将 {text} 转换为 {NameOf(Percentage)} 类型")
    End Function

    Public Shared Function TryParse(text As String, ByRef value As Percentage) As Boolean
        If text IsNot Nothing AndAlso text.Length > 1 AndAlso text.EndsWith("%") Then
            Dim numberPart = text.Substring(0, text.Length - 1)
            Dim sng = 0F
            If Single.TryParse(numberPart, sng) Then
                value.SingleValue = sng
                Return True
            End If
        End If
        Return False
    End Function
    Public Function CompareTo(other As Percentage) As Integer Implements IComparable(Of Percentage).CompareTo
        Return SingleValue.CompareTo(other.SingleValue)
    End Function

    Public Overloads Function Equals(other As Percentage) As Boolean Implements IEquatable(Of Percentage).Equals
        Return SingleValue = other.SingleValue
    End Function

    Public Overrides Function Equals(obj As Object) As Boolean
        If TypeOf obj Is Percentage Then
            Return SingleValue = DirectCast(obj, Percentage).SingleValue
        End If
        Return False
    End Function
    Public Overrides Function GetHashCode() As Integer
        Return SingleValue.GetHashCode()
    End Function
    Public Overrides Function ToString() As String
        Return (SingleValue * 100).ToString("#") & "%"
    End Function
    Public Overloads Function ToString(format As String, formatProvider As IFormatProvider) As String Implements IFormattable.ToString
        Return (SingleValue * 100).ToString(format, formatProvider) & "%"
    End Function

    Public Shared Operator =(left As Percentage, right As Percentage) As Boolean
        Return left.SingleValue = right.SingleValue
    End Operator

    Public Shared Operator <>(left As Percentage, right As Percentage) As Boolean
        Return left.SingleValue <> right.SingleValue
    End Operator

    Public Shared Operator >(left As Percentage, right As Percentage) As Boolean
        Return left.SingleValue > right.SingleValue
    End Operator

    Public Shared Operator <(left As Percentage, right As Percentage) As Boolean
        Return left.SingleValue < right.SingleValue
    End Operator

    Public Shared Operator >=(left As Percentage, right As Percentage) As Boolean
        Return left.SingleValue >= right.SingleValue
    End Operator

    Public Shared Operator <=(left As Percentage, right As Percentage) As Boolean
        Return left.SingleValue <= right.SingleValue
    End Operator

End Structure
Module Constants
    Public Const Yes As Boolean = True
    Public Const No As Boolean = True
End Module

Public Class IniEntities
    Public Shared Function Find(Of T)() As T
        ' TODO: 添加从某个ini文件读取一个实体的代码
        Throw New NotImplementedException
    End Function
    Public Shared Function Table(Of T)() As IQueryable(Of T)
        ' TODO: 添加从某个ini文件读取一组实体的代码
        Throw New NotImplementedException
    End Function
End Class