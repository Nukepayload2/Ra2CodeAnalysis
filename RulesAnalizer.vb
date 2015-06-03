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
    Public Shared Function IsWeaponKey(Key As String) As Boolean
        Return {"OccupyWeapon", "EliteOccupyWeapon", "Primary", "Secondary", "ElitePrimary", "EliteSecondary", "DeathWeapon"}.Contains(Key) OrElse Key.StartsWith("Weapon") OrElse Key.StartsWith("EliteWeapon")
    End Function
    Protected Function CheckInternal() As INIAnalizeResult
        Dim AdvResult As New INIAnalizeResult
        SyncLock Result
            AdvResult = AdvResult.Concat(Result)
        End SyncLock
        Dim UsedWeapons As New List(Of String)
        Dim LoadWeapons As New List(Of Tuple(Of String, Tuple(Of String, Integer)))

        Try
            Dim DeadBodies, MetallicDebris As IEnumerable(Of String)

            DeadBodies = From d In Values("General")("DeadBodies").Item1.Split(","c) Select d.Trim
            MetallicDebris = From d In Values("General")("MetallicDebris").Item1.Split(","c) Select d.Trim
            For Each Record In Values
                For Each r In Record.Value
                    Dim wpchk As Action = Sub()
                                              If Values.ContainsKey(r.Value.Item1) Then
                                                  UsedWeapons.Add(r.Value.Item1)
                                              ElseIf r.Value.Item1.ToLower <> "none"
                                                  AdvResult.Fault.Add(New INIAnalizeInfo(r.Value.Item2, "武器未定义,可导致单位无法开火或建造时发生AccessViolation", r.Value.Item1, Record.Key))
                                              End If
                                          End Sub
                    Select Case r.Key
                        Case "SuperWeapon", "SuperWeapon2" 'SW
                            ValueRegistryCheck("SuperWeaponTypes", "超级武器未注册或注册失败,可能导致运行时AccessViolation异常", Record.Key, r, AdvResult.Fault)
                        Case "Warhead" 'WH
                            ValueRegistryCheck("Warheads", "弹头未注册或注册失败.", Record.Key, r, AdvResult.Message)
                        Case "DeadBodies"
                            If DeadBodies.ContainsEachTrim(r.Value.Item1.Split(","c)) Then
                                Exit Select
                            End If
                            EachValueRegistryCheck(r, AdvResult.Warning, Record.Key, "尸体动画未正确注册,可能导致尸体消失", "Animations", "VoxelAnims")
                        Case "DebrisAnim", "DebrisAnims"
                            If MetallicDebris.ContainsEachTrim(r.Value.Item1.Split(","c)) Then
                                Exit Select
                            End If
                            EachValueRegistryCheck(r, AdvResult.Message, Record.Key, "碎片动画未正确注册.", "Animations", "VoxelAnims")
                        Case "AnimList", "Anim", "MetallicDebris", "ExpireAnim", "TrailerAnim", "Explosion"
                            EachValueRegistryCheck(r, AdvResult.Message, Record.Key, "一般的动画未注册或注册失败.", "Animations", "VoxelAnims")
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
                            If SPBuildings.ContainsEachTrim(r.Value.Item1.Split(","c)) Then
                                Exit Select
                            End If
                            EachValueRegistryCheck(r, AdvResult.Warning, Record.Key, "建筑未正确注册,可能导致建造结果不是预期的", "BuildingTypes")
                        Case "BuildCat" 'Building 
                            MainKeyRegistryCheck("BuildingTypes", "建筑或覆盖物建筑未注册或注册失败,可能导致建筑无法建造或运行时AccessViolation异常", Record.Key, r, AdvResult.Fault, "OverlayTypes")
                        Case "TiberiumSpawnType" 'Overlay 
                            ValueRegistryCheck("OverlayTypes", "覆盖物未注册或注册失败,可能导致覆盖物无法产生或运行时AccessViolation异常", Record.Key, r, AdvResult.Fault)
                        Case "OccupyWeapon", "EliteOccupyWeapon", "Primary", "Secondary", "ElitePrimary", "EliteSecondary", "DeathWeapon"
                            wpchk()
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
                            If (r.Key.StartsWith("EliteWeapon") AndAlso r.Key.Length > 11 AndAlso r.Key.Substring(11).IsUInteger) OrElse (r.Key.StartsWith("Weapon") AndAlso r.Key.Length > 6 AndAlso r.Key.Substring(6).IsUInteger) Then
                                wpchk()
                            End If
                    End Select
                Next
            Next

            Debug.WriteLine("Generic Check complete.Weapon check begins")
            Dim wp = From u In UsedWeapons Distinct
            Dim lo = From l In LoadWeapons Distinct
            For Each wea In lo
                If Not wp.Contains(wea.Item2.Item1) Then
                    AdvResult.Fault.Add(New INIAnalizeInfo(wea.Item2.Item2, "武器未挂载,可能导致运行时AccessViolation异常", wea.Item2.Item1, wea.Item1))
                End If
            Next
            For Each w In wp
                If Values.ContainsKey(w) Then
                    Dim wpref = Values(w)
                    If wpref.Count > 0 Then
                        Dim fir = wpref.First
                        For Each BasicKey In {"Damage", "Projectile", "Warhead", "Report", "ROF", "Speed", "Range"}
                            Dim Rec = If({"Projectile", "Warhead"}.Contains(BasicKey), AdvResult.Fault, AdvResult.Warning)
                            If wpref.ContainsKey(BasicKey) Then
                                If String.IsNullOrWhiteSpace(wpref(BasicKey).Item1) Then
                                    AdvResult.Fault.Add(New INIAnalizeInfo(fir.Value.Item2, $"武器的{BasicKey}值为空,可能导致运行时AccessViolation异常", BasicKey, w))
                                End If
                                Select Case BasicKey
                                    Case "Warhead", "Projectile"
                                        If Not Values.ContainsKey(w) AndAlso w.ToLower <> "none" Then
                                            AdvResult.Fault.Add(New INIAnalizeInfo(fir.Value.Item2, $"武器的{BasicKey}值不存在于rules中,可能导致运行时AccessViolation异常", BasicKey, w))
                                        End If
                                End Select
                            Else
                                Rec.Add(New INIAnalizeInfo(fir.Value.Item2, $"武器没有{BasicKey},可能导致运行时AccessViolation异常", BasicKey, w))
                            End If
                        Next
                    Else
                        AdvResult.Fault.Add(New INIAnalizeInfo(0, $"武器{w}为空,可能导致运行时AccessViolation异常", w, w))
                    End If
                Else
                    AdvResult.Fault.Add(New INIAnalizeInfo(0, $"Internal Check Error: 没有武器{w},注册信息,但是当作已经注册的武器", w, w))
                End If
            Next
        Catch ex As KeyNotFoundException
            AdvResult.Fault.Add(New INIAnalizeInfo(0, "关键的键或主键没有找到,可导致运行时AccessViolation异常", "(未收集)", "(无)"))
        End Try

        Debug.WriteLine("Mission Accomplished")
        Return AdvResult
    End Function

End Class
