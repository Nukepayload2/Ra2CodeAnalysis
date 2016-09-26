Imports System.Text
Imports Nukepayload2.CodeAnalysis
Imports Nukepayload2.Ra2CodeAnalysis.AnalysisHelper

Public Class EntityInferContext
    Sub New(analyzer As NamedIniAnalyzer, helpProvider As HelpProvider, namespaceBuilder As IndentStringBuilder)
        NamedAnalyzer = analyzer
        Me.NamespaceBuilder = New VBNamespaceBuilder(namespaceBuilder, analyzer.FileNameWithoutExt)
        Me.HelpProvider = helpProvider
        ProcessData()
    End Sub

    Protected Sub ProcessData()
        FillDataHead()
        InferNewInterface()
        InferPossibleBaseClass()
        CleanInterface()
        MergePossibleBaseClass()
        CleanClass()
        CorrectMemberName()
    End Sub
    ''' <summary>
    ''' 清除与基类重复的声明，同时更新类型推断和数据
    ''' </summary>
    Private Sub CleanClass()
        For Each cls In ClassIndex.Values
            Dim baseClass = cls.InheritsClass
            Dim curProperties = cls.Properties
            If baseClass IsNot Nothing Then
                Dim baseProperties = baseClass.Properties
                If cls.ImplementInterfaces.Count > 0 Then
                    For Each impl In cls.ImplementInterfaces
                        baseClass.ImplementInterfaces.Add(impl)
                        For Each prop In impl.Properties
                            baseClass.Properties(prop.Key).ImplementsInterface.Add(impl)
                        Next
                    Next
                    cls.ImplementInterfaces.Clear()
                End If
                For Each prop In baseProperties
                    Dim curKey = prop.Key
                    If curProperties.ContainsKey(curKey) Then
                        Dim curProp = curProperties(curKey)
                        Dim basicInformation = curProp.BasicInformation
                        Dim typeNameOverride = basicInformation.TypeNameOverride
                        '纠正类型推断
                        If typeNameOverride IsNot Nothing Then
                            baseProperties(curKey).BasicInformation.TypeNameOverride = typeNameOverride
                            Dim initExpr = Aggregate tp In cls.BasePropertyInitialization
                                           Where tp.PropertyBasicInformation.Name = basicInformation.Name
                                           Into FirstOrDefault
                            If initExpr IsNot Nothing Then
                                Dim init = initExpr.InitialValue
                                If init.StartsWith("""") AndAlso init.EndsWith("""") Then
                                    initExpr.InitialValue = init.Substring(1, init.Length - 2)
                                    If initExpr.InitialValue.Contains(",") Then
                                        initExpr.PropertyBasicInformation.IsQueryable = True
                                    ElseIf ClassIndex.ContainsKey(initExpr.InitialValue) Then
                                        Dim refCls = ClassIndex(initExpr.InitialValue)
                                        If refCls Is cls Then
                                            initExpr.InitialValue = init
                                            initExpr.PropertyBasicInformation.TypeNameOverride = Nothing
                                        Else
                                            initExpr.PropertyBasicInformation.TypeNameOverride = refCls
                                        End If
                                    End If
                                Else
                                    initExpr.InitialValue = $"{NamespaceBuilder.Name}Context.{initExpr.PropertyBasicInformation.RuntimeTypeName}.Find({init})"
                                End If
                            End If
                        End If
                        '删除多余的属性定义
                        curProperties.Remove(curKey)
                    Else
                        If prop.Value.IsPrimaryKey Then
                            Dim baseProp = prop.Value
                            cls.BasePropertyInitialization.Add(New VBPropertyAssignmentDeclaration(baseProp.BasicInformation, SurroundInitExpr(cls.Name, "String")))
                        End If
                    End If
                Next
            Else
                Dim vals = NamedAnalyzer.Analyzer.Values
                If vals.ContainsKey(cls.Name) Then
                    Dim clsData = vals(cls.Name)
                    For Each curProp In curProperties
                        Dim data = clsData(curProp.Key)
                        curProp.Value.InitialValue = SurroundInitExpr(data.Item1, curProp.Value.BasicInformation.RuntimeTypeName)
                    Next
                End If
            End If
        Next
    End Sub

    Private Sub CorrectMemberName()
        For Each itf In InterfaceIndex.Values
            itf.Name = RenameForVBName(itf.Name)
            Dim newProps As New Dictionary(Of String, VBPropertyDeclarationSilm)
            For Each prop In itf.Properties.Values
                prop.Name = RenameForVBName(prop.Name)
                newProps.Add(prop.Name, prop)
            Next
            itf.Properties = newProps
        Next
        For Each cls In ClassIndex.Values
            cls.Name = RenameForVBName(cls.Name)
            Dim newProps As New Dictionary(Of String, VBPropertyDeclaration)
            For Each prop In cls.Properties.Values
                prop.BasicInformation.Name = RenameForVBName(prop.BasicInformation.Name)
                newProps.Add(prop.BasicInformation.Name, prop)
            Next
            cls.Properties = newProps
        Next

    End Sub

    Private Shared Function RenameForVBName(name As String) As String
        If Not String.IsNullOrEmpty(name) Then
            If Char.IsNumber(name(0)) Then
                name = "_" + name
            End If
            name = name.Replace("-"c, "_"c).Replace(" "c, "_")
        End If
        If VBKeyWordTranslator.KeywordTable.ContainsKey(name) Then
            name = "[" + name + "]"
        End If
        Return name
    End Function

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
                            prop.ImplementsInterface.Add(firstInterface)
                            firstProperties.Add(newProp.Key, prop)
                        Next
                        impl.PossibleBaseClass = firstClass
                    Next
                End If
                curCls.InheritsClass = firstClass
            End If
        Next
    End Sub
    ''' <summary>
    ''' 通过值提取接口
    ''' </summary>
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
                        If InterfaceIndex.ContainsKey("I" + key) Then
                            itf = InterfaceIndex("I" + key)
                        Else
                            itf = New VBInterfaceBuilder(NamespaceBuilder, key, Indent)
                            AddInterface(itf)
                            If Not refCls.ImplementInterfaces.Contains(itf) Then
                                ImplementInterface(refCls, itf)
                            End If
                        End If
                        FillInterfaceImplInformation(refCls, itf)
                    End If
                Next
                Dim helptext = TrimHelp(HelpProvider.GetHelpText(key))
                cls.Item1.Properties.Add(key, New VBPropertyDeclaration(NamespaceBuilder.sb, helptext, helptext.Contains("<已"), New VBPropertyDeclarationSilm(key, HelpProvider.TempAnalizeUsage(values)) With {.TypeNameOverride = itf, .IsQueryable = valueArray.Length > 1}, Nothing, False))
            Next
        Next
    End Sub

    Private Shared Function TrimHelp(helptext As String) As String
        If helptext IsNot Nothing AndAlso helptext.Contains("用法") Then helptext = helptext.Substring(0, helptext.IndexOf("用法"))
        Return helptext
    End Function

    Private Sub ImplementInterface(curCls As VBClassBuilder, itf As VBInterfaceBuilder)
        If Aggregate i In curCls.ImplementInterfaces Select i.Name = itf.Name Into Any Then
            Return
        End If
        curCls.ImplementInterfaces.Add(itf)
        InterfaceImplementationIndex(itf).Add(curCls)
    End Sub

    ''' <summary>
    ''' 删除接口中冗余的部分
    ''' </summary>
    Private Sub CleanInterface()
        For Each curItf In InterfaceIndex.Values
            For Each curImpl In InterfaceImplementationIndex(curItf)
                Dim dels = Aggregate prop In curItf.Properties.Values Where Not curImpl.Properties.ContainsKey(prop.Name) Into ToArray
                For i = dels.Count - 1 To 0 Step -1
                    Dim curDel = dels(i)
                    If curItf.Properties.ContainsKey(curDel.Name) Then
                        curItf.Properties.Remove(curDel.Name)
                    End If
                    curItf.PossibleBaseClass.Properties(curDel.Name).ImplementsInterface.Clear()
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
            ClassIndex.Add(curBase.Name, curBase)
            '可能的基类的主键
            Dim itfName = curItf.Name
            Dim pkSilm As New VBPropertyDeclarationSilm(itfName + "Id", "String")
            Dim pk As New VBPropertyDeclaration(NamespaceBuilder.sb, $"用于在Ini中索引{itfName}数据", False, pkSilm, Nothing, True)
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
                Dim declSilm = New VBPropertyDeclarationSilm(key, HelpProvider.TempAnalizeUsage(line.Value.Item1))
                Dim helpText = TrimHelp(HelpProvider.GetHelpText(key))
                '向数据类增加初始值
                Dim initValue = line.Value.Item1
                Dim typeName = declSilm.TypeName
                initValue = SurroundInitExpr(initValue, typeName)
                registeredClass.BasePropertyInitialization.Add(New VBPropertyAssignmentDeclaration(declSilm, initValue))
                '向接口添加冗余的临时属性
                curItf.Properties.Add(key, declSilm)
                '向可能的基类添加属性
                Dim decl As New VBPropertyDeclaration(NamespaceBuilder.sb, helpText, helpText.Contains("<已"), declSilm, Nothing, False)
                curBase.Properties.Add(key, decl)
            End If
        Next
    End Sub

    Private Shared Function SurroundInitExpr(initValue As String, typeName As String) As String
        Const enumerStr As String = "IEnumerable"
        If typeName.StartsWith(enumerStr) Then
            Const ofStr As String = "(Of "
            Dim ofIdx = typeName.IndexOf(ofStr)
            If ofIdx > 0 AndAlso typeName.EndsWith(")") Then
                Dim innerTypeName = typeName.Substring(ofIdx + ofStr.Length, typeName.Length - enumerStr.Length - ofStr.Length - 1)
                If innerTypeName.Length > 0 Then
                    Dim sb As New StringBuilder("{")
                    For Each value In initValue.Split(","c)
                        sb.Append(SurroundInitExpr(value, innerTypeName)).Append(", ")
                    Next
                    sb.Remove(sb.Length - 2, 2).Append("}")
                    Return sb.ToString
                End If
            End If
            Return $"{{{initValue}}}"
        End If
        Select Case typeName
            Case "String"
                Return """" + initValue + """"
            Case "Guid"
                Return $"New {typeName}(""{initValue}"")"
            Case "Percentage", "BigInteger"
                Return $"{typeName}.Parse(""{initValue}"")"
            Case "Single"
                Return initValue & "F"
            Case "Long"
                Return initValue & "L"
            Case "ULong"
                Return initValue & "UL"
            Case "Decimal"
                Return initValue & "D"
            Case Else
                Return initValue
        End Select
    End Function

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
        InterfaceIndex.Add(itfb.Name, itfb)
        InterfaceImplementationIndex.Add(itfb, New List(Of VBClassBuilder))
        InterfaceData.Add(New Tuple(Of VBInterfaceBuilder, Dictionary(Of String, Tuple(Of String, Integer)))(itfb, data.Value))
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