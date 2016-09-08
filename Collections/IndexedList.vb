''' <summary>
''' 对列表某一成员建立唯一索引的列表。它与<see cref="Dictionary(Of TKey, TValue)"/>的区别是：不通过 TKey 查找 TValue。
''' </summary>
Public Class IndexedList(Of TIndex, TValue)
    ''' <summary>
    ''' 对列表某一成员建立唯一索引的列表
    ''' </summary>
    ''' <param name="getIndex">获取需要索引的成员</param>
    ''' <param name="clustered">索引是否是聚簇的</param>
    Sub New(getIndex As Func(Of TValue, TIndex), clustered As Boolean)
        Me.getIndex = getIndex
        Index = If(clustered, DirectCast(New SortedSet(Of TIndex), ISet(Of TIndex)), New HashSet(Of TIndex))
    End Sub

    Dim getIndex As Func(Of TValue, TIndex)
    Public ReadOnly Property Index As ISet(Of TIndex)
    Public ReadOnly Property Values As New List(Of TValue)
    ''' <summary>
    ''' 添加项目
    ''' </summary>
    Public Sub Add(item As TValue)
        Values.Add(item)
        Index.Add(getIndex(item))
    End Sub
    ''' <summary>
    ''' 删除项目
    ''' </summary>
    Public Sub Remove(item As TValue)
        Values.Remove(item)
        Index.Remove(getIndex(item))
    End Sub
    ''' <summary>
    ''' 确定被索引的成员是否包含指定项目。对于聚簇索引，使用折半查找。对于非聚簇索引，使用哈希查找。
    ''' </summary>
    Public Function Contains(key As TIndex) As Boolean
        Return Index.Contains(key)
    End Function
End Class