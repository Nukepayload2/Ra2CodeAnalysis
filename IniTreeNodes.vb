Imports System.Text
Imports Nukepayload2.Ra2CodeAnalysis.AnalysisHelper

Namespace Imaging
    ''' <summary>
    ''' 能被注册到主键或键
    ''' </summary>
    Public Interface IRegisterable
        ReadOnly Property Text As String
        ReadOnly Property RegisteredIn As IEnumerable(Of RegisterTreeNode)
        Sub RegisterToInternal(Node As RegisterTreeNode)
        Sub UnRegisterFromInternal(Node As RegisterTreeNode)
    End Interface
    ''' <summary>
    ''' INI树节点
    ''' </summary>
    Public MustInherit Class IniTreeNode
        ReadOnly Property Text As String
        Sub New(Text As String)
            Me.Text = Text
        End Sub
        Friend Sub RenameInternal(Name As String)
            _Text = Name
        End Sub
        Public Sub Rename(Root As IEnumerable(Of MainKeyTreeNode), NewName As String)
            For Each MK In Root
                For Each kv In MK.KeyValues.Keys
                    Dim Vals As New StringBuilder
                    Dim vs = MK.KeyValues(kv).Values
                    Dim co = vs.Count
                    For i As Integer = 0 To co - 2
                        Dim v = vs(i)
                        Dim nam2 = If(Text = v.Text, NewName, v.Text)
                        v.RenameInternal(nam2)
                        Vals.Append(nam2)
                        Vals.Append(","c)
                    Next
                    Dim Last = vs(co - 1).Text
                    Dim nam = If(Text = Last, NewName, Last)
                    vs.Last.RenameInternal(nam)
                    Vals.Append(nam)
                    MK.KeyValues(kv).RenameInternal(Vals.ToString)
                Next
            Next
            RenameInternal(NewName)
        End Sub
    End Class
    ''' <summary>
    ''' 主键和键
    ''' </summary>
    Public Class RegisterTreeNode
        Inherits IniTreeNode
        Protected RegValues As New List(Of IRegisterable)
        Public ReadOnly Property RegisteredValues As IEnumerable(Of IRegisterable) = RegValues
        Public Sub RegisterAndModify(Item As IRegisterable)
            Register(Item)

        End Sub
        Public Sub Register(Item As IRegisterable)
            RegValues.Add(Item)
            Item.RegisterToInternal(Me)
        End Sub
        Public Sub UnRegister(Item As IRegisterable)
            RegValues.Remove(Item)
            Item.UnRegisterFromInternal(Me)
        End Sub
        Sub New(Text As String)
            MyBase.New(Text)
        End Sub
    End Class
    ''' <summary>
    ''' 主键
    ''' </summary>
    Public Class MainKeyTreeNode
        Inherits RegisterTreeNode
        Implements IRegisterable
        ''' <summary>
        ''' 把当前主键的信息转换回ini文本
        ''' </summary>
        ''' <returns></returns>
        Public Overrides Function ToString() As String
            With New StringBuilder
                .Append("["c)
                .Append(Text)
                .Append("]"c)
                For Each kv In KeyValues
                    .Append(kv.Key)
                    .Append("="c)
                    .Append(kv.Value)
                Next
                Return .ToString
            End With
        End Function
        ReadOnly Property KeyValues As IDictionary(Of KeyTreeNode, ValueTreeNode) =
            New Dictionary(Of KeyTreeNode, ValueTreeNode)
        Protected RegIn As New List(Of RegisterTreeNode)
        Public ReadOnly Property RegisteredIn As IEnumerable(Of RegisterTreeNode) = RegIn Implements IRegisterable.RegisteredIn

        Private ReadOnly Property IRegisterable_Text As String Implements IRegisterable.Text
            Get
                Return Text
            End Get
        End Property

        Sub New(Text As String)
            MyBase.New(Text)
        End Sub
        <Obsolete>
        Friend Sub RegisterToInternal(Node As RegisterTreeNode) Implements IRegisterable.RegisterToInternal
            RegIn.Add(Node)
        End Sub
        <Obsolete>
        Friend Sub UnRegisterFromInternal(Node As RegisterTreeNode) Implements IRegisterable.UnRegisterFromInternal
            RegIn.Remove(Node)
        End Sub
    End Class
    ''' <summary>
    ''' 键
    ''' </summary>
    Public Class KeyTreeNode
        Inherits RegisterTreeNode
        Public ReadOnly Property IsRegisterKey As Boolean
            Get
                Return Text.IsUInteger
            End Get
        End Property
        Sub New(Text As String)
            MyBase.New(Text)
        End Sub
    End Class
    ''' <summary>
    ''' 值列表
    ''' </summary>
    Public Class SubValueTreeNode
        Inherits IniTreeNode
        Implements IRegisterable
        Protected RegIn As New List(Of RegisterTreeNode)
        Public ReadOnly Property RegisteredIn As IEnumerable(Of RegisterTreeNode) = RegIn Implements IRegisterable.RegisteredIn

        Private ReadOnly Property IRegisterable_Text As String Implements IRegisterable.Text
            Get
                Return Text
            End Get
        End Property

        <Obsolete>
        Friend Sub RegisterToInternal(Node As RegisterTreeNode) Implements IRegisterable.RegisterToInternal
            RegIn.Add(Node)
        End Sub
        Sub New(Text As String)
            MyBase.New(Text)
        End Sub
        <Obsolete>
        Friend Sub UnRegisterFromInternal(Node As RegisterTreeNode) Implements IRegisterable.UnRegisterFromInternal
            RegIn.Remove(Node)
        End Sub
    End Class
    ''' <summary>
    ''' 值
    ''' </summary>
    Public Class ValueTreeNode
        Inherits IniTreeNode
        Protected Vals As New List(Of SubValueTreeNode)
        ReadOnly Property Values As IEnumerable(Of SubValueTreeNode) = Vals
        Sub New(Text As String)
            MyBase.New(Text)
            For Each txs In Text.Split(","c)
                Vals.Add(New SubValueTreeNode(txs.Trim))
            Next
        End Sub
    End Class

End Namespace