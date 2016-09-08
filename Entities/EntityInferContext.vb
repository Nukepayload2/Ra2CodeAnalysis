Imports System.Text
Imports Nukepayload2.Ra2CodeAnalysis.AnalysisHelper

Public Class EntityInferContext
    Sub New(analyzer As NamedIniAnalyzer, helpProvider As HelpProvider)
        NamedAnalyzer = analyzer
        Me.HelpProvider = helpProvider
        NamespaceBuilder = New VBNamespaceBuilder(Text, analyzer.FileNameWithoutExt, Indent)
        ProcessData()
    End Sub

    Protected Sub ProcessData()
        FillDataHead()
        InferNewInterface()
        InferPossibleBaseClass()
        CleanInterface()
        MergePossibleBaseClass()
    End Sub

    Private Sub MergePossibleBaseClass()
        For Each curCls In ClassIndex.Values
            Dim implementInterfaces = curCls.ImplementInterfaces
            If implementInterfaces.Count > 0 Then
                Dim firstInterface = implementInterfaces.First
                Dim firstClass = firstInterface.PossibleBaseClass
                If implementInterfaces.Count > 1 Then
                    For Each impl In implementInterfaces.Skip(1)
                        Dim mergeClass = impl.PossibleBaseClass
                        Dim firstProperties = firstClass.Properties
                        For Each newProp In From p In mergeClass.Properties Where Not firstProperties.ContainsKey(p.Key)
                            Dim prop = newProp.Value
                            prop.ImplementsInterface = firstInterface
                            firstProperties.Add(newProp.Key, prop)
                        Next
                        impl.PossibleBaseClass = firstClass
                    Next
                End If
                curCls.InheritsClass = firstClass
            End If
        Next
    End Sub

    Private Sub InferNewInterface()
        For Each cls In ClassData
            Dim cdata = cls.Item2
            For Each kv In cdata
                Dim key = kv.Key
                Dim values = kv.Value.Item1
                Dim itf As VBInterfaceBuilder = Nothing
                Dim valueArray = values.Split(","c)
                For Each value In valueArray
                    value = value.Trim
                    If ClassIndex.ContainsKey(value) Then
                        Dim refCls = ClassIndex(value)
                        If InterfaceIndex.ContainsKey(key) Then
                            itf = InterfaceIndex(key)
                        Else
                            itf = New VBInterfaceBuilder(NamespaceBuilder, value, Indent)
                            AddInterface(itf)
                            If Not refCls.ImplementInterfaces.Contains(itf) Then
                                ImplementInterface(refCls, itf)
                            End If
                        End If
                        FillInterfaceImplInformation(refCls, itf)
                    End If
                Next
                Dim helptext As String = HelpProvider.GetHelpText(key)
                cls.Item1.Properties.Add(key, New VBPropertyDeclaration(Indent, helptext, helptext.Contains("<已"), New VBPropertyDeclarationSilm(key, HelpProvider.TempAnalizeUsage(values)) With {.TypeNameOverride = itf, .IsQueryable = valueArray.Length > 1}, Nothing, False))
            Next
        Next
    End Sub

    Private Sub ImplementInterface(curCls As VBClassBuilder, itf As VBInterfaceBuilder)
        curCls.ImplementInterfaces.Add(itf)
        InterfaceImplementationIndex(itf).Add(curCls)
    End Sub

    ''' <summary>
    ''' 删除接口中冗余的部分
    ''' </summary>
    Private Sub CleanInterface()
        For Each curItf In InterfaceIndex.Values
            For Each curImpl In InterfaceImplementationIndex(curItf)
                For Each pro In From prop In curItf.Properties.Values Where Not curImpl.Properties.ContainsKey(prop.Name)
                    curItf.Properties.Remove(pro.Name)
                    curItf.PossibleBaseClass.Properties(pro.Name).ImplementsInterface = Nothing
                Next
            Next
        Next
    End Sub

    ''' <summary>
    ''' 在类添加实现的接口, 向接口和可能的基类填充初步推断了类型的数据, 向数据类填充数据。
    ''' </summary>
    Private Sub InferPossibleBaseClass()
        For Each itf In InterfaceData
            Dim curItf = itf.Item1
            Dim implList = InterfaceImplementationIndex(curItf)
            Dim curBase = curItf.PossibleBaseClass
            '可能的基类的主键
            Dim itfName = curItf.Name
            Dim pkSilm As New VBPropertyDeclarationSilm(itfName + "Id", "String")
            Dim pk As New VBPropertyDeclaration(Indent, $"用于在Ini中索引{itfName}数据", False, pkSilm, Nothing, True)
            curBase.Properties.Add(pkSilm.Name, pk)
            If itf.Item2 Is Nothing Then Continue For
            '查阅接口记录注册了哪些类
            For Each clsName In From l In itf.Item2 Select l.Value.Item1
                If ClassIndex.ContainsKey(clsName) Then
                    Dim curCls = ClassIndex(clsName)
                    If Not Aggregate i In curCls.ImplementInterfaces Where i.Name = curItf.Name Into Any Then
                        '让数据类实现接口
                        ImplementInterface(curCls, curItf)
                        '从数据类提取信息
                        FillInterfaceImplInformation(curCls, curItf)
                    End If
                End If
            Next
        Next
    End Sub

    Private Sub FillInterfaceImplInformation(registeredClass As VBClassBuilder, curItf As VBInterfaceBuilder)
        Dim curBase = curItf.PossibleBaseClass
        For Each line In NamedAnalyzer.Analyzer.Values(registeredClass.Name)
            Dim key = line.Key
            If Not curBase.Properties.ContainsKey(key) Then
                Dim declSilm = New VBPropertyDeclarationSilm(key, HelpProvider.TempAnalizeUsage(key))
                Dim helpText = HelpProvider.GetHelpText(key)
                '向数据类增加初始值
                registeredClass.BasePropertyInitialization.Add(New VBPropertyAssignmentDeclaration(declSilm, If(declSilm.TypeName = "String", """" + line.Value.Item1 + """", line.Value.Item1)))
                '向接口添加冗余的临时属性
                curItf.Properties.Add(key, declSilm)
                '向可能的基类添加属性
                Dim decl As New VBPropertyDeclaration(Indent, helpText, helpText.Contains("<已"), declSilm, Nothing, False)
                curBase.Properties.Add(key, decl)
            End If
        Next
    End Sub

    ''' <summary>
    ''' 将ini数据整理, 仅填充头部数据和索引, 不填充属性
    ''' </summary>
    Private Sub FillDataHead()
        For Each k In NamedAnalyzer.Analyzer.Values
            If k.Value.All(Function(rec) rec.Key.IsInteger) Then
                Dim itfb As New VBInterfaceBuilder(NamespaceBuilder, k.Key, Indent)
                AddInterface(itfb, k)
            Else
                Dim clsb = New VBClassBuilder(NamespaceBuilder, k.Key, Indent)
                ClassData.Add(New Tuple(Of VBClassBuilder, Dictionary(Of String, Tuple(Of String, Integer)))(clsb, k.Value))
                ClassIndex.Add(k.Key, clsb)
            End If
        Next
    End Sub

    Private Sub AddInterface(itfb As VBInterfaceBuilder, Optional data As KeyValuePair(Of String, Dictionary(Of String, Tuple(Of String, Integer))) = Nothing)
        InterfaceData.Add(New Tuple(Of VBInterfaceBuilder, Dictionary(Of String, Tuple(Of String, Integer)))(itfb, data.Value))
        InterfaceIndex.Add(itfb.Name, itfb)
        InterfaceImplementationIndex.Add(itfb, New List(Of VBClassBuilder))
    End Sub

    Public ReadOnly Property HelpProvider As HelpProvider
    Public ReadOnly Property NamespaceBuilder As VBNamespaceBuilder
    Public ReadOnly Property Text As New StringBuilder
    Public ReadOnly Property Indent As New StrongBox(Of Integer)(0)
    Public ReadOnly Property NamedAnalyzer As NamedIniAnalyzer
    Public ReadOnly Property InterfaceData As New List(Of Tuple(Of VBInterfaceBuilder, Dictionary(Of String, Tuple(Of String, Integer))))
    Public ReadOnly Property InterfaceIndex As New Dictionary(Of String, VBInterfaceBuilder)
    Public ReadOnly Property InterfaceImplementationIndex As New Dictionary(Of VBInterfaceBuilder, List(Of VBClassBuilder))
    Public ReadOnly Property ClassData As New List(Of Tuple(Of VBClassBuilder, Dictionary(Of String, Tuple(Of String, Integer))))
    Public ReadOnly Property ClassIndex As New Dictionary(Of String, VBClassBuilder)
    ''' <summary>
    ''' 从另一个实体信息推断上下文完善推断
    ''' </summary>
    Public Sub Infer(another As EntityInferContext)
        Throw New NotImplementedException
    End Sub
End Class