Imports System.Text
Imports Nukepayload2.Ra2CodeAnalysis.AnalysisHelper

Public Class EntityInferContext
    Sub New(analyzer As NamedIniAnalyzer, helpProvider As HelpProvider)
        Me.NamedAnalyzer = analyzer
        Me.HelpProvider = helpProvider
        NamespaceBuilder = New VBNamespaceBuilder(Text, analyzer.FileNameWithoutExt, Indent)
        ProcessData()
    End Sub

    Protected Sub ProcessData()
        FillDataHead()
        InferPossibleBaseClass()
        CleanInterface()
    End Sub
    ''' <summary>
    ''' 删除接口中冗余的部分
    ''' </summary>
    Private Sub CleanInterface()
        For Each itf In InterfaceData
            Dim curItf = itf.Item1
            For Each curImpl In InterfaceImplementationIndex(curItf)
                For Each pro In From prop In curItf.Properties Where Not curImpl.PropertyNameIndex.Contains(prop.Name)
                    curItf.Properties.Remove(pro)
                    curItf.PropertyNameIndex.Remove(pro.Name)
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
            curBase.PropertyNameIndex.Add(pkSilm.Name)
            curBase.Properties.Add(pk)
            '查阅接口记录注册了哪些类
            For Each clsName In From l In itf.Item2 Select l.Value.Item1
                If ClassNameIndex.Contains(clsName) Then
                    Dim curCls = ClassIndex(clsName)
                    If Not Aggregate i In curCls.ImplementInterfaces Where i.Name = curItf.Name Into Any Then
                        '让数据类实现接口
                        curCls.ImplementInterfaces.Add(curItf)
                        implList.Add(curCls)
                        '从数据类提取信息
                        Dim clsData = NamedAnalyzer.Analyzer.Values(clsName)
                        For Each line In clsData
                            Dim key = line.Key
                            If Not curBase.PropertyNameIndex.Contains(key) Then
                                Dim declSilm = New VBPropertyDeclarationSilm(key, HelpProvider.TempAnalizeUsage(key))
                                Dim helpText = HelpProvider.GetHelpText(key)
                                '向数据类增加初始值
                                curCls.ExtraPropertyInitialization.Add(New VBPropertyAssignmentDeclaration(declSilm, If(declSilm.TypeName = "String", """" + line.Value.Item1 + """", line.Value.Item1)))
                                '向接口添加冗余的临时属性
                                curItf.PropertyNameIndex.Add(key)
                                curItf.Properties.Add(declSilm)
                                '向可能的基类添加属性
                                Dim decl As New VBPropertyDeclaration(Indent, helpText, helpText.Contains("<已"), declSilm, Nothing, False)
                                curBase.PropertyNameIndex.Add(key)
                                curBase.Properties.Add(decl)
                            End If
                        Next
                    End If
                End If
            Next
        Next
    End Sub
    ''' <summary>
    ''' 将ini数据整理, 仅填充头部数据和索引, 不填充属性
    ''' </summary>
    Private Sub FillDataHead()
        For Each k In NamedAnalyzer.Analyzer.Values
            If k.Value.All(Function(rec) rec.Key.IsInteger) Then
                Dim itfb As VBInterfaceBuilder = New VBInterfaceBuilder(NamespaceBuilder, k.Key, Indent)
                InterfaceData.Add(New Tuple(Of VBInterfaceBuilder, Dictionary(Of String, Tuple(Of String, Integer)))(itfb, k.Value))
                InterfaceIndex.Add(k.Key, itfb)
                InterfaceNameIndex.Add(k.Key)
                InterfaceImplementationIndex.Add(itfb, New List(Of VBClassBuilder))
            Else
                Dim clsb = New VBClassBuilder(NamespaceBuilder, k.Key, Indent)
                ClassData.Add(New Tuple(Of VBClassBuilder, Dictionary(Of String, Tuple(Of String, Integer)))(clsb, k.Value))
                ClassIndex.Add(k.Key, clsb)
                ClassNameIndex.Add(k.Key)
            End If
        Next
    End Sub
    Public ReadOnly Property HelpProvider As HelpProvider
    Public ReadOnly Property NamespaceBuilder As VBNamespaceBuilder
    Public ReadOnly Property Text As New StringBuilder
    Public ReadOnly Property Indent As New StrongBox(Of Integer)(0)
    Public ReadOnly Property NamedAnalyzer As NamedIniAnalyzer
    Public ReadOnly Property InterfaceData As New List(Of Tuple(Of VBInterfaceBuilder, Dictionary(Of String, Tuple(Of String, Integer))))
    Public ReadOnly Property InterfaceNameIndex As New HashSet(Of String)
    Public ReadOnly Property InterfaceIndex As New Dictionary(Of String, VBInterfaceBuilder)
    Public ReadOnly Property InterfaceImplementationIndex As New Dictionary(Of VBInterfaceBuilder, List(Of VBClassBuilder))
    Public ReadOnly Property ClassData As New List(Of Tuple(Of VBClassBuilder, Dictionary(Of String, Tuple(Of String, Integer))))
    Public ReadOnly Property ClassNameIndex As New HashSet(Of String)
    Public ReadOnly Property ClassIndex As New Dictionary(Of String, VBClassBuilder)
    ''' <summary>
    ''' 从另一个实体信息推断上下文完善推断
    ''' </summary>
    Public Sub Infer(another As EntityInferContext)
        Throw New NotImplementedException
    End Sub
End Class