Imports Nukepayload2.Ra2CodeAnalysis.AnalysisHelper
Public Class AIAnalizer
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
                Dim rulesunitscache As New List(Of String)
                rulesunitscache.AddRange(From i In rules.Values("infantrytypes") Select i.Value.Item1)
                rulesunitscache.AddRange(From v In rules.Values("vehicletypes") Select v.Value.Item1)
                rulesunitscache.AddRange(From a In rules.Values("aircrafttypes") Select a.Value.Item1)

                For Each record In Values
                    If record.Key = "aitriggertypes" Then
                        For Each r In record.Value
                            Dim vals = r.Value.Item1.Split(","c)
                            If vals.Count <> 18 Then
                                AdvResult.Fault.Add(New INIAnalizeInfo(r.Value.Item2, "ai触发参数数量错误,可导致运行时AccessViolation异常", r.Value.Item1, "aitriggertypes"))
                            Else
                                ValueRegistryCheck("teamtypes", "ai触发使用了未注册的作战小队1,可导致运行时AccessViolation异常", "aitriggertypes", vals(1), r.Value.Item2, AdvResult.Fault, Me)
                                If Not vals(2).Trim.StartsWith("<") Then
                                    ValueRegistryCheck("countries", "ai触发使用了未注册的国家,可导致运行时AccessViolation异常", "aitriggertypes", vals(2), r.Value.Item2, AdvResult.Fault, rules)
                                End If
                                TypeCheck(vals, {3, 4, 6, 10, 11, 12, 13, 15, 16, 17}, AddressOf IsUInteger, "ai触发参数第{0}个的类型错误,应为正整数,可导致ai行为异常", AdvResult.Message, r.Value.Item2, "aitriggertypes")
                                If Not vals(5).Trim.StartsWith("<") Then
                                    If SPBuildings.Contains(vals(5)) Then Continue For
                                    If Not rulesunitscache.Contains(vals(5)) Then
                                        ValueRegistryCheck("buildingtypes", "ai触发使用了未注册的建筑/单位,可导致运行时AccessViolation异常", "aitriggertypes", vals(5), r.Value.Item2, AdvResult.Fault, rules)
                                    End If
                                End If
                                TypeCheck(vals, {7, 8, 9}, AddressOf IsUFraction, "ai触发参数第{0}个的类型错误,应为展开写的正小数,可导致ai行为异常", AdvResult.Message, r.Value.Item2, "aitriggertypes")
                                If Not vals(14).Trim.StartsWith("<") Then
                                    ValueRegistryCheck("teamtypes", "ai触发使用了未注册的作战小队2,可导致运行时AccessViolation异常", "aitriggertypes", vals(14), r.Value.Item2, AdvResult.Fault, Me)
                                End If
                            End If
                        Next
                    ElseIf record.Key = "taskforces"
                        For Each r In record.Value
                            If Values.ContainsKey(r.Value.Item1) Then
                                For Each vs In Values(r.Value.Item1)
                                    If vs.Key.IsUInteger Then
                                        Try
                                            Dim tmp = CUInt(vs.Key)
                                        Catch ex As OverflowException
                                            AdvResult.Fault.Add(New INIAnalizeInfo(r.Value.Item2, "特遣小队成员id严重超出规定范围，可能导致ai行为混乱或崩溃", r.Value.Item1, record.Key))
                                            Continue For
                                        End Try
                                        If CUInt(vs.Key) > 6 Then
                                            AdvResult.Warning.Add(New INIAnalizeInfo(r.Value.Item2, "特遣小队成员id大于6，此成员会被忽略", r.Value.Item1, record.Key))
                                        Else
                                            Dim team = vs.Value.Item1.Split(","c)
                                            If team.Count <> 2 Then
                                                AdvResult.Fault.Add(New INIAnalizeInfo(r.Value.Item2, "特遣小队成员格式无效，应为数量,单位，可能导致运行时AccessViolation异常", r.Value.Item1, record.Key))
                                            ElseIf team(0).IsUInteger Then
                                                If Not rulesunitscache.Contains(team(1)) Then
                                                    AdvResult.Fault.Add(New INIAnalizeInfo(r.Value.Item2, "特遣小队成员单位未注册，可能导致运行时AccessViolation异常", r.Value.Item1, record.Key))
                                                End If
                                            Else
                                                AdvResult.Fault.Add(New INIAnalizeInfo(r.Value.Item2, "特遣小队成员数量格式无效，应为正整数，可能导致运行时AccessViolation异常", r.Value.Item1, record.Key))
                                            End If
                                        End If
                                    End If
                                Next
                            Else
                                AdvResult.Warning.Add(New INIAnalizeInfo(r.Value.Item2, "特遣注册了，但是没有定义，此注册会被忽略", r.Value.Item1, record.Key))
                            End If
                        Next
                    ElseIf record.Key = "scripttypes"
                        For Each r In record.Value
                            If Values.ContainsKey(r.Value.Item1) Then
                                Dim has490 As Boolean = False
                                For Each vs In Values(r.Value.Item1)
                                    If vs.Key.IsUInteger Then
                                        Dim s = vs.Value.Item1.Split(","c)
                                        If s.Count = 2 Then
                                            If s(0).Trim = "49" AndAlso s(1).Trim = "0" Then
                                                has490 = True
                                            ElseIf s(0).Trim = "16"
                                                AdvResult.Warning.Add(New INIAnalizeInfo(r.Value.Item2, "ai.ini的脚本中使用指令16(巡逻路径点)，可能导致ai行为异常", r.Value.Item1, "scripttypes"))
                                            End If
                                        Else
                                            AdvResult.Fault.Add(New INIAnalizeInfo(r.Value.Item2, "脚本内容的两个值格式错误，可能导致运行时AccessViolation异常", r.Value.Item1, "scripttypes"))
                                        End If
                                    End If
                                Next
                                If Not has490 Then
                                    AdvResult.Warning.Add(New INIAnalizeInfo(r.Value.Item2, "脚本没有49,0，可能导致ai行为异常", r.Value.Item1, "scripttypes"))
                                End If
                            Else
                                AdvResult.Warning.Add(New INIAnalizeInfo(r.Value.Item2, "脚本注册了，但是没有定义，此注册会被忽略", r.Value.Item1, "scripttypes"))
                            End If
                        Next
                    Else
                        For Each r In record.Value
                            Select Case r.Key
                                Case "taskforce"
                                    ValueRegistryCheck("taskforces", "特遣小队未注册", record.Key, r, AdvResult.Fault)
                                Case "script"
                                    MainKeyRegistryCheck("teamtypes", "作战小队未注册", record.Key, r, AdvResult.Fault)
                                    ValueRegistryCheck("scripttypes", "脚本未注册", record.Key, r, AdvResult.Fault)
                            End Select
                        Next
                    End If
                Next
            Else
                Dim RulesUnitsCache As New List(Of String)
                RulesUnitsCache.AddRange(From i In rules.Values("InfantryTypes") Select i.Value.Item1)
                RulesUnitsCache.AddRange(From v In rules.Values("VehicleTypes") Select v.Value.Item1)
                RulesUnitsCache.AddRange(From a In rules.Values("AircraftTypes") Select a.Value.Item1)

                For Each Record In Values
                    If Record.Key = "AITriggerTypes" Then
                        For Each r In Record.Value
                            Dim vals = r.Value.Item1.Split(","c)
                            If vals.Count <> 18 Then
                                AdvResult.Fault.Add(New INIAnalizeInfo(r.Value.Item2, "AI触发参数数量错误,可导致运行时AccessViolation异常", r.Value.Item1, "AITriggerTypes"))
                            Else
                                ValueRegistryCheck("TeamTypes", "AI触发使用了未注册的作战小队1,可导致运行时AccessViolation异常", "AITriggerTypes", vals(1), r.Value.Item2, AdvResult.Fault, Me)
                                If Not vals(2).Trim.StartsWith("<") Then
                                    ValueRegistryCheck("Countries", "AI触发使用了未注册的国家,可导致运行时AccessViolation异常", "AITriggerTypes", vals(2), r.Value.Item2, AdvResult.Fault, rules)
                                End If
                                TypeCheck(vals, {3, 4, 6, 10, 11, 12, 13, 15, 16, 17}, AddressOf IsUInteger, "AI触发参数第{0}个的类型错误,应为正整数,可导致AI行为异常", AdvResult.Message, r.Value.Item2, "AITriggerTypes")
                                If Not vals(5).Trim.StartsWith("<") Then
                                    If SPBuildings.Contains(vals(5)) Then Continue For
                                    If Not RulesUnitsCache.Contains(vals(5)) Then
                                        ValueRegistryCheck("BuildingTypes", "AI触发使用了未注册的建筑/单位,可导致运行时AccessViolation异常", "AITriggerTypes", vals(5), r.Value.Item2, AdvResult.Fault, rules)
                                    End If
                                End If
                                TypeCheck(vals, {7, 8, 9}, AddressOf IsUFraction, "AI触发参数第{0}个的类型错误,应为展开写的正小数,可导致AI行为异常", AdvResult.Message, r.Value.Item2, "AITriggerTypes")
                                If Not vals(14).Trim.StartsWith("<") Then
                                    ValueRegistryCheck("TeamTypes", "AI触发使用了未注册的作战小队2,可导致运行时AccessViolation异常", "AITriggerTypes", vals(14), r.Value.Item2, AdvResult.Fault, Me)
                                End If
                            End If
                        Next
                    ElseIf Record.Key = "TaskForces"
                        For Each r In Record.Value
                            If Values.ContainsKey(r.Value.Item1) Then
                                For Each vs In Values(r.Value.Item1)
                                    If vs.Key.IsUInteger Then
                                        Try
                                            Dim tmp = CUInt(vs.Key)
                                        Catch ex As OverflowException
                                            AdvResult.Fault.Add(New INIAnalizeInfo(r.Value.Item2, "特遣小队成员ID严重超出规定范围，可能导致AI行为混乱或崩溃", r.Value.Item1, Record.Key))
                                            Continue For
                                        End Try
                                        If CUInt(vs.Key) > 6 Then
                                            AdvResult.Warning.Add(New INIAnalizeInfo(r.Value.Item2, "特遣小队成员ID大于6，此成员会被忽略", r.Value.Item1, Record.Key))
                                        Else
                                            Dim team = vs.Value.Item1.Split(","c)
                                            If team.Count <> 2 Then
                                                AdvResult.Fault.Add(New INIAnalizeInfo(r.Value.Item2, "特遣小队成员格式无效，应为数量,单位，可能导致运行时AccessViolation异常", r.Value.Item1, Record.Key))
                                            ElseIf team(0).IsUInteger Then
                                                If Not RulesUnitsCache.Contains(team(1)) Then
                                                    AdvResult.Fault.Add(New INIAnalizeInfo(r.Value.Item2, "特遣小队成员单位未注册，可能导致运行时AccessViolation异常", r.Value.Item1, Record.Key))
                                                End If
                                            Else
                                                AdvResult.Fault.Add(New INIAnalizeInfo(r.Value.Item2, "特遣小队成员数量格式无效，应为正整数，可能导致运行时AccessViolation异常", r.Value.Item1, Record.Key))
                                            End If
                                        End If
                                    End If
                                Next
                            Else
                                AdvResult.Warning.Add(New INIAnalizeInfo(r.Value.Item2, "特遣注册了，但是没有定义，此注册会被忽略", r.Value.Item1, Record.Key))
                            End If
                        Next
                    ElseIf Record.Key = "ScriptTypes"
                        For Each r In Record.Value
                            If Values.ContainsKey(r.Value.Item1) Then
                                Dim Has490 As Boolean = False
                                For Each vs In Values(r.Value.Item1)
                                    If vs.Key.IsUInteger Then
                                        Dim s = vs.Value.Item1.Split(","c)
                                        If s.Count = 2 Then
                                            If s(0).Trim = "49" AndAlso s(1).Trim = "0" Then
                                                Has490 = True
                                            ElseIf s(0).Trim = "16"
                                                AdvResult.Warning.Add(New INIAnalizeInfo(r.Value.Item2, "AI.ini的脚本中使用指令16(巡逻路径点)，可能导致AI行为异常", r.Value.Item1, "ScriptTypes"))
                                            End If
                                        Else
                                            AdvResult.Fault.Add(New INIAnalizeInfo(r.Value.Item2, "脚本内容的两个值格式错误，可能导致运行时AccessViolation异常", r.Value.Item1, "ScriptTypes"))
                                        End If
                                    End If
                                Next
                                If Not Has490 Then
                                    AdvResult.Warning.Add(New INIAnalizeInfo(r.Value.Item2, "脚本没有49,0，可能导致AI行为异常", r.Value.Item1, "ScriptTypes"))
                                End If
                            Else
                                AdvResult.Warning.Add(New INIAnalizeInfo(r.Value.Item2, "脚本注册了，但是没有定义，此注册会被忽略", r.Value.Item1, "ScriptTypes"))
                            End If
                        Next
                    Else
                        For Each r In Record.Value
                            Select Case r.Key
                                Case "TaskForce"
                                    ValueRegistryCheck("TaskForces", "特遣小队未注册", Record.Key, r, AdvResult.Fault)
                                Case "Script"
                                    MainKeyRegistryCheck("TeamTypes", "作战小队未注册", Record.Key, r, AdvResult.Fault)
                                    ValueRegistryCheck("ScriptTypes", "脚本未注册", Record.Key, r, AdvResult.Fault)
                            End Select
                        Next
                    End If
                Next
            End If
        Catch ex As KeyNotFoundException
            AdvResult.Fault.Add(New INIAnalizeInfo(0, "关键的主键或键未找到,可能在运行时发生AccessViolation异常", "(未收集)", "(未收集)"))
        End Try

        Return AdvResult
    End Function
End Class
