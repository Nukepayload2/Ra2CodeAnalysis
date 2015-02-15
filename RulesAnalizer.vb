Imports Nukepayload2.Ra2CodeAnalysis.AnalysisHelper

''' <summary>
''' 分析Rules.ini或它的等效ini
''' </summary>
Public Class RulesAnalizer
    Inherits Ra2IniAnalizer
    ''' <summary>
    ''' 分析结果,包括基类提供的语法检查
    ''' </summary>
    ''' <returns></returns>
    Public Overrides Function Check() As INIAnalizeResult
        Return CheckInternal()
    End Function
    Sub New(INIText As StreamReader)
        MyBase.New(INIText)
    End Sub
    Sub New(INIText As String)
        MyBase.New(INIText)
    End Sub

    Protected Function CheckInternal() As INIAnalizeResult
        Dim AdvResult As New INIAnalizeResult
        SyncLock Result
            AdvResult = AdvResult.Concat(Result)
        End SyncLock
        Dim UsedWeapons As New List(Of String)
        Dim LoadWeapons As New List(Of Tuple(Of String, Tuple(Of String, Integer)))
        For Each k In {"SuperWeaponTypes", "Warheads", "Animations", "Countries", "AircraftTypes", "InfantryTypes", "VehicleTypes",
            "BuildingTypes", "OverlayTypes", "General", "AudioVisual"}
            If Not Values.ContainsKey(k) Then
                AdvResult.Fault.Add(New INIAnalizeInfo(0, "缺少必要的主键" & k & "，无法正常检查，可导致运行时AccessViolation异常", String.Empty, k))
            End If
        Next
        Dim DeadBodies = From d In Values("General")("DeadBodies").Item1.Split(","c) Select d.Trim
        Dim MetallicDebris = From d In Values("General")("MetallicDebris").Item1.Split(","c) Select d.Trim

        For Each Record In Values
            For Each r In Record.Value
                Select Case r.Key
                    Case "SuperWeapon", "SuperWeapon2" 'SW
                        ValueRegistryCheck("SuperWeaponTypes", "超级武器未注册或注册失败,可能导致运行时AccessViolation异常", Record.Key, r, AdvResult.Fault)
                    Case "Warhead" 'WH
                        ValueRegistryCheck("Warheads", "弹头未注册或注册失败,可能引发潜在Bug", Record.Key, r, AdvResult.Warning)
                    Case "DeadBodies"
                        If DeadBodies.ContainsEachTrim(r.Value.Item1.Split(","c)) Then
                            Exit Select
                        End If
                        EachValueRegistryCheck(r, AdvResult.Warning, Record.Key, "尸体动画未正确注册,可能导致尸体消失", "Animations", "VoxelAnims")
                    Case "DebrisAnim", "DebrisAnims"
                        If MetallicDebris.ContainsEachTrim(r.Value.Item1.Split(","c)) Then
                            Exit Select
                        End If
                        EachValueRegistryCheck(r, AdvResult.Warning, Record.Key, "碎片动画未正确注册,可能导致动画不起作用", "Animations", "VoxelAnims")
                    Case "AnimList", "Anim", "MetallicDebris", "ExpireAnim", "TrailerAnim", "Explosion"
                        EachValueRegistryCheck(r, AdvResult.Warning, Record.Key, "动画未正确注册,可能导致动画不起作用", "Animations", "VoxelAnims")
                    Case "Next"
                        ValueRegistryCheck("Animations", "关键动画未注册或注册失败,可导致运行时AccessViolation异常", Record.Key, New KeyValuePair(Of String, Tuple(Of String, Integer))(r.Key, New Tuple(Of String, Integer)(r.Value.Item1, r.Value.Item2)), AdvResult.Fault)
                    Case "Owner", "RequiredHouses", "FactoryOwners", "ForbiddenHouses"
                        EachValueRegistryCheck(r, AdvResult.Warning, Record.Key, "国家未正确注册,可能导致建造结果不是预期的", "Countries")
                    Case "Category" 'Unit 
                        Select Case r.Value.Item1
                            Case "AirLift", "AirPower", "AirSupport"
                                MainKeyRegistryCheck("AircraftTypes", "Category错误或战机未正确注册,可能导致无法正常建造或运行时AccessViolation异常", Record.Key, r, AdvResult.Fault, "VehicleTypes")
                            Case "VIP", "Soldier", "Civilian"
                                MainKeyRegistryCheck("InfantryTypes", "Category错误或步兵未正确注册,可能导致无法正常建造或运行时AccessViolation异常", Record.Key, r, AdvResult.Fault)
                            Case Else
                                MainKeyRegistryCheck("VehicleTypes", "Category错误或战车未正确注册,可能导致无法正常建造或运行时AccessViolation异常", Record.Key, r, AdvResult.Fault)
                        End Select
                    Case "Prerequisite", "Prerequisite2"
                        If {"BARRACKS", "RADAR", "TECH", "PROC"}.ContainsEachTrim(r.Value.Item1.Split(","c)) Then
                            Exit Select
                        End If
                        EachValueRegistryCheck(r, AdvResult.Warning, Record.Key, "建筑未正确注册,可能导致建造结果不是预期的", "BuildingTypes")
                    Case "BuildCat" 'Building 
                        MainKeyRegistryCheck("BuildingTypes", "建筑或覆盖物建筑未注册或注册失败,可能导致建筑无法建造或运行时AccessViolation异常", Record.Key, r, AdvResult.Fault, "OverlayTypes")
                    Case "TiberiumSpawnType" 'Overlay 
                        ValueRegistryCheck("OverlayTypes", "覆盖物未注册或注册失败,可能导致覆盖物无法产生或运行时AccessViolation异常", Record.Key, r, AdvResult.Fault)
                    Case "OccupyWeapon", "EliteOccupyWeapon", "Primary", "Secondary", "ElitePrimary", "EliteSecondary", "DeathWeapon"
                        If Not Values.ContainsKey(r.Value.Item1) Then
                            AdvResult.Warning.Add(New INIAnalizeInfo(r.Value.Item2, "武器未定义,可导致单位无法开火或失去一些死亡效果", r.Value.Item1, Record.Key))
                        End If
                        UsedWeapons.Add(r.Value.Item1)
                    Case "Verses"
                        Dim spl = r.Value.Item1.Split(","c)
                        If 0 = Aggregate c In From s In spl Where Not s.Contains("%") Into Count Then
                            If spl.Count <> 11 Then
                                AdvResult.Fault.Add(New INIAnalizeInfo(r.Value.Item2, "弹头百分比数量错误,可能导致运行时AccessViolation异常", r.Value.Item1, Record.Key))
                            End If
                        Else
                            AdvResult.Fault.Add(New INIAnalizeInfo(r.Value.Item2, "弹头百分比有语法错误,可能导致运行时AccessViolation异常", r.Value.Item1, Record.Key))
                        End If
                    Case "ShrapnelWeapon", "AirburstWeapon"
                        LoadWeapons.Add(New Tuple(Of String, Tuple(Of String, Integer))(Record.Key, r.Value))
                    Case "HoldsWhat"
                        MainKeyRegistryCheck("ParticleSystems", "粒子系统未注册或注册失败,可能导致运行时AccessViolation异常", Record.Key, r, AdvResult.Fault)
                        ValueRegistryCheck("Particles", "粒子未注册或注册失败,可能导致运行时AccessViolation异常", Record.Key, r, AdvResult.Fault)
                    Case Else
                        If r.Key.StartsWith("EliteWeapon") OrElse r.Key.StartsWith("Weapon") Then
                            UsedWeapons.Add(r.Value.Item1)
                        End If
                End Select
            Next
        Next
        Debug.WriteLine("Check 1 complete")
        Dim wp = From u In UsedWeapons Distinct
        Dim lo = From l In LoadWeapons Distinct
        For Each wea In lo
            If Not wp.Contains(wea.Item2.Item1) Then
                AdvResult.Fault.Add(New INIAnalizeInfo(wea.Item2.Item2, "武器未挂载,可能导致运行时AccessViolation异常", wea.Item2.Item1, wea.Item1))
            End If
        Next

        Debug.WriteLine("Mission Accomplished")
        Return AdvResult
    End Function

End Class
