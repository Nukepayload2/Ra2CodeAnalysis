Public Class CrashAnalyzer
    Enum TSPlatformVersions
        TiberianSun203
        FireStrom
        Ra2Original
        Ra21006
        YuriOriginal
        Yuri1001
    End Enum
    Sub New(Version As TSPlatformVersions)
        If Version < TSPlatformVersions.YuriOriginal Then
            Throw New NotSupportedException($"不支持{Version}的游戏平台")
        End If
    End Sub
    Public Function FindEIP(Except As String) As String
        Dim reg As New Text.RegularExpressions.Regex("(?<=Eip:)[0-9A-F]{8}")
        If reg.IsMatch(Except) Then
            Return reg.Match(Except).Value
        Else
            Return Nothing
        End If
    End Function
    Public Function TryGetDetail(Except As String) As CrashItem
        Dim cureip = FindEIP(Except)
        If cureip IsNot Nothing Then
            Return TryGetHelpText(cureip)
        Else
            Return Nothing
        End If
    End Function
    Private Function TryGetHelpText(UpperEIPText As String) As CrashItem
        If EIPTable.ContainsKey(UpperEIPText) Then
            Dim s = EIPTable(UpperEIPText)
            Return New CrashItem(UpperEIPText, s(0), s(1))
        Else
            Return Nothing
        End If
    End Function

    Dim EIPTable As New Dictionary(Of String, String()) From {
{"004145BD", {"Art", "An AircraftType has a corrupted HVA.
Shows the file type as ""unknown"" in the XCC Mixer. Also an AircraftType has image tag missing or no artmd section.
This could also be caused from an aircraft with no name under AircraftTypes or the name is in the wrong section."}},
{"004157E3", {"Weapon", "An AircraftType used as spy plane has no Primary weapon."}},
{"00417D05", {"Weapon", "An AircraftType has fired a weapon which has Suicide=yes set."}},
{"004242DB", {"Animation", "Using a TrailerAnim on an Animation but not setting a TrailerSeperation (or setting TrailerSeperation=0). This is because the default TrailerSeperation is zero, and that number is used as a divisor."}},
{"00424A14", {"Animation", "An Animation with MakeInfantry=X set was played, where X was greater than the number of list entries in AnimToInfantry or less than 0."}},
{"0042E7AF", {"AI", "A Construction Yard does not have AIBuildThis=yes set and the owning side's AI was present in the game."}},
{"00441C28", {"Misc", "You have set [AudioVisual]→ShakeScreen= to zero."}},
{"004593BB", {"Misc", "See Tank Bunker / Sell Unit IEs"}},
{"0045EC90", {"Misc", "Multiple reasons, depending on the stack dump in the except:
 If 00506115 appears near the top of the stack dump - some country (present in the game at the moment, controlled by AI) cannot build anything from [General]→Shipyard=."}},
{"0045ED69", {"Misc", "The [General]→PadAircraft= list is empty."}},
{"0045ED71", {"Misc", "The first AircraftType in the [General]→PadAircraft= list doesn't have at least one BuildingType listed as its Dock."}},
{"0046650D", {"Weapon", "A unit's shrapnel weapon does not exist (see broken-reference causes, below)."}},
{"00471CA4", {"Weapon", "A unit's initial primary (Primary or Weapon1) weapon's Warhead does not have MindControl=yes set, but a weapon in some other weapon slot does. Triggered by one of the following events:
Unit was selected by the user and the user moused-over a potential target.
Unit was considering potential targets on its own (e.g. the unit was about to fire automatically at a nearby enemy unit).
An IFV or urban combat building gains a mind-control weapon via passenger entry or garrison."}},
{"00482096", {"Unit crates", "Your [AI]→BuildRefinery= is either invalid, missing or blank thereby the game can't find the harvesters for deciding the unit."}},
{"004895C7", {"Warhead", "You have a warhead with a CellSpread greater than 11."}},
{"004D5108", {"Weapon", "A unit's secondary weapon does not exist (see broken-reference causes, below), or the weapon's Warhead tag is missing or set to blank. Examples:
The offending weapon is the unit's Secondary weapon, and the unit just finished constructing.
The offending weapon is the unit's EliteSecondary weapon, and the unit just got promoted to Elite status."}},
{"004F8CCD", {"AI", "[AI]→BuildConst= lists less than 3 BuildingTypes and your last [listed] Construction Yard was destroyed or sold while you were on low power, or you went into low power without owning any Construction Yards."}},
{"004F65BF", {"Misc", "Some House cannot build anything from [General]→BaseUnit= ."}},
{"00505E41", {"Misc", "An AI-controlled House which, due to rules(md).ini configuration, is unable to construct a base, received a Construction Yard thus triggering the AI base planning routine. Triggered by the following events:
 If a player's MCV was mind-controlled by an enemy, that player is killed, and the MCV is then released from mind-control to the neutral side. The only workaround is to make MCVs immune to mind-control (this is done in the UMP).
 A neutral Engineer (there's an official multiplayer map that has neutral Psychic Sensors which, on rare occasions, can leave an Engineer as a survivor) captures a Construction Yard.
More detail: The AI base planning logic kicks in at the moment a player receives a Construction Yard and generates a plan of what buildings to build, in what order. However, the game makes an assumption that any country that can actually start base construction will be able to build at least 3 different BuildingTypes. When that assumption fails (a Construction Yard is received by the civilian house, who cannot build anything), everything goes haywire. For more info on how base planning logic works, refer to the AI Base Planning System article.
Interestingly, the civilian house acquiring a Construction Yard via relinquished mind-control (in the same way as for an MCV) does not cause an IE. Clearly this effect was not taken into consideration when mind-control was added to the engine and the AI base planning routine is only called when a Construction Yard is captured or is first created."}},
{"0050CD20", {"AI", "The AI is trying to pick a target for the Nuclear Missile or Weather Storm superweapon but is lacking target weighting values for a certain object that exists on the map. You need to uncomment/restore one of the [General]→AIIonCannonXValue= lines. - An InfantryType With Engineer=yes exists On the map. AIIonCannonEngineerValue needs restoring."}},
{"0050CD44", {"AI", "The AI is trying to pick a target for the Nuclear Missile or Weather Storm superweapon but is lacking target weighting values for a certain object that exists on the map. You need to uncomment/restore one of the [General]→AIIonCannonXValue= lines. - An InfantryType with VehicleThief=yes exists on the map. AIIonCannonThiefValue needs restoring."}},
{"0050CD79", {"AI", "The AI is trying to pick a target for the Nuclear Missile or Weather Storm superweapon but is lacking target weighting values for a certain object that exists on the map. You need to uncomment/restore one of the [General]→AIIonCannonXValue= lines. - A BuildingType With Factory=BuildingType exists On the map. AIIonCannonConYardValue needs restoring."}},
{"0050CDA2", {"AI", "The AI is trying to pick a target for the Nuclear Missile or Weather Storm superweapon but is lacking target weighting values for a certain object that exists on the map. You need to uncomment/restore one of the [General]→AIIonCannonXValue= lines. - A BuildingType with Factory=UnitType and Naval=no exists on the map. AIIonCannonWarFactoryValue needs restoring."}},
{"0050CDCC", {"AI", "The AI is trying to pick a target for the Nuclear Missile or Weather Storm superweapon but is lacking target weighting values for a certain object that exists on the map. You need to uncomment/restore one of the [General]→AIIonCannonXValue= lines. - A BuildingType With a positive Power value exists On the map. AIIonCannonPowerValue needs restoring."}},
{"0050CDF0", {"AI", "The AI is trying to pick a target for the Nuclear Missile or Weather Storm superweapon but is lacking target weighting values for a certain object that exists on the map. You need to uncomment/restore one of the [General]→AIIonCannonXValue= lines. - A BuildingType with IsBaseDefense=yes exists on the map. AIIonCannonBaseDefenseValue needs restoring."}},
{"0050CE14", {"AI", "The AI is trying to pick a target for the Nuclear Missile or Weather Storm superweapon but is lacking target weighting values for a certain object that exists on the map. You need to uncomment/restore one of the [General]→AIIonCannonXValue= lines. - A BuildingType With IsPlug=yes exists On the map. AIIonCannonPlugValue needs restoring."}},
{"0050CE38", {"AI", "The AI is trying to pick a target for the Nuclear Missile or Weather Storm superweapon but is lacking target weighting values for a certain object that exists on the map. You need to uncomment/restore one of the [General]→AIIonCannonXValue= lines. - A BuildingType with IsTemple=yes exists on the map. AIIonCannonTempleValue needs restoring."}},
{"0050CE5C", {"AI", "The AI is trying to pick a target for the Nuclear Missile or Weather Storm superweapon but is lacking target weighting values for a certain object that exists on the map. You need to uncomment/restore one of the [General]→AIIonCannonXValue= lines. - A BuildingType With HoverPad=yes exists On the map. AIIonCannonHelipadValue needs restoring."}},
{"0050CEA2", {"AI", "The AI is trying to pick a target for the Nuclear Missile or Weather Storm superweapon but is lacking target weighting values for a certain object that exists on the map. You need to uncomment/restore one of the [General]→AIIonCannonXValue= lines. - A BuildingType listed in [AI] → BuildTech exists on the map. AIIonCannonTechCenterValue needs restoring."}},
{"0050CECC", {"AI", "The AI is trying to pick a target for the Nuclear Missile or Weather Storm superweapon but is lacking target weighting values for a certain object that exists on the map. You need to uncomment/restore one of the [General]→AIIonCannonXValue= lines. - A VehicleType With Harvester=yes exists On the map. AIIonCannonHarvesterValue needs restoring."}},
{"0050CF15", {"AI", "The AI is trying to pick a target for the Nuclear Missile or Weather Storm superweapon but is lacking target weighting values for a certain object that exists on the map. You need to uncomment/restore one of the [General]→AIIonCannonXValue= lines. - A VehicleType with a positive Passengers value exists on the map. AIIonCannonAPCValue needs restoring."}},
{"0050CF2A", {"AI", "The AI is trying to pick a target for the Nuclear Missile or Weather Storm superweapon but is lacking target weighting values for a certain object that exists on the map. You need to uncomment/restore one of the [General]→AIIonCannonXValue= lines. - A VehicleType which DeploysInto a BuildingType listed in [AI] → BuildConst exists on the map. AIIonCannonMCVValue needs restoring."}},
{"00518369", {"Warhead", "An InfantryType Is taking damage from a non-existant warhead."}}, '{"0053A15B", {"Unknown", "map error"}},
{"0054AF0E", {"Object", "A unit's Secondary points to a non-existent weapon."}},
{"00567B43", {"Object", "An Object has a negative sight."}},
{"0056D388", {"Object", "An InfantryType or VehicleType does not have a valid movement zone."}},
{"005D7387", {"Misc", "Not having at least one valid InfantryType with AllowedToStartInMultiplayer=yes (default) for each house."}},
{"005DA453", {"Network", "A crash occurred with the network code around an object called FirewallHelper."}},
{"005F4F88", {"Animation", "An Animation has TiberiumSpawnType=SOME_OVERLAY and SOME_OVERLAY is one of the three last entries in [OverlayTypes].Link to original report"}},
{"005F5155", {"Misc", "You tried to construct a BuildingType with HasSpotlight=yes. Yuri's Revenge 1.001 doesn't support this, only patched versions do."}},
{"0062B662", {"Animation", "Having an animation with SpawnsParticle which does not point to a valid ParticleSystem (see broken-reference causes, below)."}},
{"0062DCD2", {"Misc", "An overlay type with Explodes=yes set has been destroyed, random dice roll determined that the particle specified in BarrelParticle should be displayed, but that flag is blank OR you're firing a weapon with UseSparkParticles=yes/UseFireParticles=yes/IsRailgun=yes without a valid AttachedParticleSystem= set."}},
{"0064003C", {"Misc", "If you have a buildable Construction Yard, start its construction, and then cancel it, an IE will occur. Construction Yards should not be buildable - they should only be deployed from vehicles."}},
{"006407A6", {"Misc", "You've made a MapShot ""Not ScreenShot"" that was saved as Map****.yrm and is in your Directory. Game is trying to load map during initial startup. Remove these MapShots from the directory to prevent this from happening."}},
{"0065B73F", {"Weapon", "You have a Weapon whose RadLevel is less than [Radiation]→RadLightDelay=. (Cause: Integer division is performed on those two values, which yields zero in this case, and then another value is divided by the result - division by zero.)"}},
{"0069ACC6", {"Map", "A PKT file's [MultiMaps] section declares a map which doesn't have its own section to define the parameters, or lacks the CD entry in the section."}},
{"00697F29", {"Misc", "The game can't find a valid gamemode. Your mpmodesmd.ini is corrupted."}},
{"006AEBB8", {"Misc", "Your ra2md.ini file lists a combination of game mode/map which the game cannot satisfy. This can happen if the range of valid combinations changes between one game session and the next (i.e. because you changed what maps were valid for which game modes in your mod, or you activated a different mod to the one that was active previously).
LaunchBase works around this by saving and restoring the game's configuration on a per-mod basis."}},
{"006B7D30", {"Object", "An object has a weapon with Spawner=yes, but it doesn't have Spawns=yes. The latter part is what tells the game to initialize the Spawn Manager for this unit when it's instantiated, the former part tells the game to try and access the Spawn Manager (and it doesn't check if it's been initialized properly)."}},
{"006B7718", {"Object", "A Spawned=yes attempted to fly over the map border instead of landing, thus crashing the game. A known reason for this issue is Selectable=no on AircraftTypes that use aircraft Locomotor."}},
{"006EA6AE", {"AI/Map", "You coerced the game into creating an instance of a TeamType that is not defined, either through Map Triggers or through AI Script Actions."}},
{"006F1FC6", {"AI programming", "A TeamType has been defined without assigning it a TaskForce, or a TeamType is being referenced without being defined at all."}},
{"006F352E", {"Weapon", "A unit has an ElitePrimary weapon specified which does not exist (see broken-reference causes, below), or the weapon's Warhead tag is missing or set to blank, and that unit just got promoted to Elite status."}},
{"006F72EF", {"Weapon", "A unit has an ElitePrimary weapon specified which does not exist (see broken-reference causes, below), or the weapon's Warhead tag is missing or set to blank, and that unit just got promoted to Elite status."}},
{"006F40A2", {"Weapon", "Started construction of a unit whose Primary weapon does not exist (see broken-reference causes, below), or the weapon's Warhead tag is missing or set to blank."}},
{"0070031E", {"Weapon", "A unit has a weapon specified which does not exist in the INI (see broken-reference causes, below), or the weapon's Warhead tag is missing or set to blank. (Common reason - that unit just got promoted to Elite status and one of the Elite weapons is misdefined.)"}},
{"00702330", {"Anim", "A building has missing or non-existing DebrisTypes. Like DebrisTypes=CRYSTAL1."}},
{"007120F7", {"Misc", "You have a BuildingType (which is click-repairable) with Strength=0 or Strength below [General]→RepairStep=."}},
{"0071AF4D", {"Warhead", "Detonating a Temporal=yes warhead under one of the following conditions:
Firing weapon was a shrapnel weapon.
Firing weapon was an urban combat weapon (fired from an occupied building).
Warhead has a non-zero CellSpread set."}},
{"0071B173", {"Warhead", "Firing a death weapon using a Temporal=yes warhead."}},
{"00442832", {"Warhead", "Using Sparky=yes warhead without three valid animations defined in [AudioVisual]→OnFire=. Crash happens when damaging a building into yellow or red health."}},
{"0071C661", {"Warhead", "Using Sparky=yes warhead without two valid animations defined in [AudioVisual]→TreeFire=. Crash happens when damaging a wooden terrain object."}},
{"0072652D", {"Map", "There is a trigger in the map that wants to change a house's non-existant object to another house."}},
{"00684E55", {"Map", "There is a trigger which requires a house, either because its events, actions or attached triggers require a house. The house on the trigger is not set."}},
{"007387EB", {"Art", "[AudioVisual]→ShakeScreen= is missing or set to zero."}},
{"0073B0C9", {"Misc", "The concept known as ""Infantry Linking"" can result in an IE, occuring when the linked infantry was modified in a subsequent game mode override file or a map and a human player scrolls their battlefield view to a place on the map where an AI-owned War Factory is located. Don't do Infantry Linking."}},
{"0073C762", {"Art", "The artmd.ini entry specified by a Voxel-using VehicleType's Image tag is missing - the game defaults to Voxel=no in this case and attempts to load and draw a non-existant SHP."}},
{"00756B2D", {"Art", "The specified ShadowIndex on a voxel VehicleType adresses a section that does not exist."}},
{"00772A98", {"Weapon", "A unit has an ElitePrimary weapon specified which does not exist (see broken-reference causes, below), or the weapon's Warhead tag is missing or set to blank, and that unit just got promoted to Elite status.
Also reported to be due to ""firing a weapon that has no projectile"".
This needs testing - missing projectile may be an alternative reason to all 'missing weapon' IEs."}},
{"007C9B92", {"Malformed input", "Multiple reasons, depending on the stack dump in the except:
 If 006DD5D2 or 006DD009 appear near the top of the stack dump - Map contains a malformed Map Action, while parsing it, a number was expected, something else or end-of-line was found instead. Map Actions expect well-formed input.
 If 00843EEC appears in the stack - RefinerySmokeOffsetThree is not set to a valid value."}},
{"007CAF66", {"Malformed input", "Multiple reasons, depending on the stack dump in the except:
 If 0075DE19 appears near the top of the stack dump - A Warhead whose Verses could not be processed. EBP value says how many values remained to be parsed when an error occurred - (11 - EBP) is the 0-based index of the problematic value."}},
{"007CFD30", {"Misc - memory management", "If line 20 of the stack dump includes 61108B, and line 23 includes 610CA0, you are likely trying to use RockPatch's ""Place Urban Areas"" feature without applying the necessary snowmd.ini fix. Please check the RockPatch Help for more info."}},
{"0051BB7D", {"Warhead", "A unit was being erased by a chrono weapon but the object that started the erase process no longer exists. When a unit is being erased, an instance of the TemporalClass is linked to it. This class references the object that is doing the erasing. If the object breaks the link under 'normal' circumstances (e.g. the firer is destroyed or moved) then the attack order is cancelled and the TemporalClass is removed. In some rare cases the link to the firer's TemporalClass is not removed and therefore points to garbage memory.
Examples of how the IE may occur:
Ore harvester with a chrono weapon started an attack but then transformed whilst unloading ore at a refinery before the target was destroyed. Don't give ore harvesters chrono weapons.
A vehicle was being erased but then got picked up by a carryall. This is very difficult to replicate - it has only been reported once.
The IE occurs when the unit would have been erased."}},
{"00520FC8", {"Warhead", "A building was being erased by a chrono weapon after infantry recieved the order to occupy it. When the building is erased before the infantry reaches it, an IE occurs. This is because the TemporalClass removes the building without marking it as dead, thus the occupying infantry is not informed its destination is gone. The IE occurs when the infantry unit updates its position. There is another TemporalClass related bug similar to this one – EIP address 00521BB6, which lies inside the same function, can be seen near the top on a stack of this exception."}},
{"90900004", {"Misc", "Generic exception, for example, raised when you are missing the snowmd.ini median fix."}},
{"FEEEFEEE", {"Misc", "Multiple reasons, depending on the stack dump in the except:
 If the stack dump starts with 006B771E then it's a variant of 006B7718."}}}

End Class
