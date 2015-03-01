Imports Nukepayload2.Ra2CodeAnalysis.AnalysisHelper

Public Class ArtAnalizer
    Inherits Ra2IniAnalizer
    Dim rules As RulesAnalizer
    Sub New(INIText As StreamReader, rules As RulesAnalizer)
        MyBase.New(INIText)
        Me.rules = rules
    End Sub
    Sub New(INIText As String, rules As RulesAnalizer, Optional Option_Compare_Text As Boolean = False)
        MyBase.New(INIText, Option_Compare_Text)
        Me.rules = rules
    End Sub

    Public Overrides Function Check() As INIAnalizeResult
        Dim AdvResult As New INIAnalizeResult
        SyncLock Result
            AdvResult = AdvResult.Concat(Result)
        End SyncLock
        Try
            If TextCompare Then
                For Each record In Values
                    For Each r In record.Value
                        Select Case r.Key
                            Case "sequence"
                                If Not Values.ContainsKey(r.Value.Item1) Then
                                    AdvResult.Fault.Add(New INIAnalizeInfo(r.Value.Item2, "使用了不存在的动作序列，可导致运行时AccessViolation异常。", r.Value.Item1, record.Key))
                                End If
                            Case "trailer", "next"
                                ValueRegistryCheck("animations", "使用了未注册的动画，动画不会生效。", record.Key, r.Value.Item1, r.Value.Item2, AdvResult.Warning, rules, "voxelanims")
                            Case "expireanim", "spawns"
                                EachValueRegistryCheck(r, AdvResult.Warning, r.Key, "使用了未注册的动画，动画不会生效。", "animations", rules, "voxelanims")
                            Case "tooverlay"
                                ValueRegistryCheck("overlaytypes", "使用了未注册的覆盖物，可导致运行时AccessViolation异常。", record.Key, r.Value.Item1, r.Value.Item2, AdvResult.Fault, rules)
                            Case "tiberiumspawntype"
                                ValueRegistryCheck("overlaytypes", "使用了未注册的覆盖物，可导致运行时AccessViolation异常。", record.Key, r.Value.Item1, r.Value.Item2, AdvResult.Fault, rules)
                                If r.Value.Item1.Length <= 2 OrElse Not r.Value.Item1.Substring(r.Value.Item1.Length - 2).IsUInteger Then
                                    AdvResult.Fault.Add(New INIAnalizeInfo(r.Value.Item2, "产生的覆盖物名的结尾不是两位数字，可导致运行时AccessViolation异常。", r.Value.Item1, record.Key))
                                End If
                            Case "warhead"
                                If Not rules.Values.Keys.Contains(r.Value.Item1) Then
                                    AdvResult.Fault.Add(New INIAnalizeInfo(r.Value.Item2, "使用了不存在的弹头，可导致运行时AccessViolation异常。", r.Value.Item1, record.Key))
                                End If
                            Case "spawnsparticle"
                                ValueRegistryCheck("particles", "使用了未注册的粒子动画，可导致运行时AccessViolation异常。", record.Key, r.Value.Item1, r.Value.Item2, AdvResult.Fault, rules)
                        End Select
                    Next
                Next

            Else
                For Each Record In Values
                    For Each r In Record.Value
                        Select Case r.Key
                            Case "Sequence"
                                If Not Values.ContainsKey(r.Value.Item1) Then
                                    AdvResult.Fault.Add(New INIAnalizeInfo(r.Value.Item2, "使用了不存在的动作序列，可导致运行时AccessViolation异常。", r.Value.Item1, Record.Key))
                                End If
                            Case "Trailer", "Next"
                                ValueRegistryCheck("Animations", "使用了未注册的动画，动画不会生效。", Record.Key, r.Value.Item1, r.Value.Item2, AdvResult.Warning, rules, "VoxelAnims")
                            Case "ExpireAnim", "Spawns"
                                EachValueRegistryCheck(r, AdvResult.Warning, r.Key, "使用了未注册的动画，动画不会生效。", "Animations", rules, "VoxelAnims")
                            Case "ToOverlay"
                                ValueRegistryCheck("OverlayTypes", "使用了未注册的覆盖物，可导致运行时AccessViolation异常。", Record.Key, r.Value.Item1, r.Value.Item2, AdvResult.Fault, rules)
                            Case "TiberiumSpawnType"
                                ValueRegistryCheck("OverlayTypes", "使用了未注册的覆盖物，可导致运行时AccessViolation异常。", Record.Key, r.Value.Item1, r.Value.Item2, AdvResult.Fault, rules)
                                If r.Value.Item1.Length <= 2 OrElse Not r.Value.Item1.Substring(r.Value.Item1.Length - 2).IsUInteger Then
                                    AdvResult.Fault.Add(New INIAnalizeInfo(r.Value.Item2, "产生的覆盖物名的结尾不是两位数字，可导致运行时AccessViolation异常。", r.Value.Item1, Record.Key))
                                End If
                            Case "Warhead"
                                If Not rules.Values.Keys.Contains(r.Value.Item1) Then
                                    AdvResult.Fault.Add(New INIAnalizeInfo(r.Value.Item2, "使用了不存在的弹头，可导致运行时AccessViolation异常。", r.Value.Item1, Record.Key))
                                End If
                            Case "SpawnsParticle"
                                ValueRegistryCheck("Particles", "使用了未注册的粒子动画，可导致运行时AccessViolation异常。", Record.Key, r.Value.Item1, r.Value.Item2, AdvResult.Fault, rules)
                        End Select
                    Next
                Next
            End If
        Catch ex As KeyNotFoundException
            AdvResult.Fault.Add(New INIAnalizeInfo(0, "关键的主键或键未找到,可能在运行时发生AccessViolation异常", "(未收集)", "(未收集)"))
        End Try
        Return AdvResult
    End Function
End Class
