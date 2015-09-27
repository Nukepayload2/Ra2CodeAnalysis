Imports System.Text

Public Class CodeSnippetsViewModel
    Public Shared ScaleWidth%, ScaleHeight%

    Public ReadOnly Property CodeSnippets As IList(Of CodeSnippetGroup)
        Get
            Return New List(Of CodeSnippetGroup) From {New CodeSnippetGroup("Rules", DefaultRules),
                    New CodeSnippetGroup("Art", DefaultArt),
                    New CodeSnippetGroup("AI", DefaultAI),
                    New CodeSnippetGroup("Ra2", DefaultRa2),
                    New CodeSnippetGroup("公共", DefaultCommon)}
        End Get
    End Property
    Protected ReadOnly Property DefaultRa2 As New List(Of CodeSnippet) From {
        New CodeSnippet("vbufn", "VideoBackBuffer=no", "视频缓冲区关闭"),
        New CodeSnippet("hires", "AllowHiResModes=yes", "启用高分辨率模式"),
        New CodeSnippet("reso", $"ScreenWidth={ScaleWidth}
ScreenHeight={ScaleHeight}", "屏幕分辨率")
    }
    Protected ReadOnly Property DefaultCommon As New List(Of CodeSnippet) From {
        New CodeSnippet("summ", $";;;<summary>
;;;
;;;</summary>
;;;<copyright author="" "", date=""{Date.Now.ToString("yyyy-MM-dd")}"", culture=""zh-cn"", allRightsReserved=""true""/>", "xml格式的注释")}
    Protected ReadOnly Property DefaultAI As New List(Of CodeSnippet) From {
        New CodeSnippet("scri", "
[<Name>]
Name=<Name>
0=", "空白脚本"),
New CodeSnippet("tskf", "
[<Name>]
Name=<Name>
0=
Group=-1
", "空白特遣小队")}
    Protected ReadOnly Property DefaultArt As New List(Of CodeSnippet) From {
New CodeSnippet("unita", "
[<Name>]
Cameo=<Name>ico
AltCameo=<Name>uico
Sequence=<Name>Sequence
Crawls=yes
Remapable=yes
FireUp=3
PrimaryFireFLH=100,0,100
", "单位的Art声明"),
New CodeSnippet("unitas", "
;请使用其它软件生成动作序列，然后覆盖此序列
[<Name>Sequence]
Ready=,,
Guard=,,
Walk=,,
Idle=,,,S
Idle=,,,E
Crawl=,,
Prone=,,
Die=,,
Die=,,
FireUp=,,
FireProne=,,
Down=,,
Up=,,
Paradrop=,,
Cheer=,,,E
Tread=,,
Swim=,,
WetAttack=,,
WetIdle=,,,S
WetIdle=,,,E
WetDie=,,
WetDie=,,
Panic=,,
Die3=,,
Die4=,,
Die5=,,
", "单位的动作序列声明")
}
    Protected ReadOnly Property DefaultRules As New List(Of CodeSnippet) From {
        New CodeSnippet("infasov", "
;名称 <Name> 主武器 <Pri>
[<Name>]
UIName=Name:<Name>
Name=<Name>
Image=<Name>
Category=Soldier
Primary=<Pri>
OccupyWeapon=<Pri>Occupy
EliteOccupyWeapon=<Pri>OccupyE
Occupier=yes
Prerequisite=NAHAND
CrushSound=InfantrySquish
Strength=125
Armor=flak
TechLevel=1
Pip=white
OccupyPip=PersonRed
Sight=5
Speed=4
Owner=Russians,Confederation,Africans,Arabs
Cost=100
Soylent=50
Points=5
IsSelectableCombatant=yes
VoiceSelect=<Name>Select
VoiceMove=<Name>Move
VoiceAttack=<Name>AttackCommand
VoiceFeedback=<Name>Fear
VoiceSpecialAttack=<Name>Move
DieSound=<Name>Die
Locomotor={4A582744-9839-11d1-B709-00A024DDAFD1}
PhysicalSize=1
MovementZone=Infantry
ThreatPosed=5
VeteranAbilities=STRONGER,FIREPOWER,ROF,SIGHT,FASTER
EliteAbilities=SELF_HEAL,STRONGER,FIREPOWER,ROF
ImmuneToVeins=yes
Size=1
ElitePrimary=<Pri>E
IFVMode=2
", "苏联基础步兵"),
New CodeSnippet("infaall", "
;名称 <Name> 主武器 <Pri> 副武器 <Sec>
[<Name>]
UIName=Name:<Name>
Name=<Name>
Image=<Name>
Category=Soldier
Primary=<Pri>
Secondary=<Sec>
Occupier=yes
OccupyWeapon=<Pri>Occupy
EliteOccupyWeapon=<Sec>OccupyE
OpenTransportWeapon=1
Prerequisite=GAPILE
CrushSound=InfantrySquish
Strength=125
Pip=white
OccupyPip=PersonBlue
Armor=none
TechLevel=1
Sight=5
Speed=4
Owner=British,French,Germans,Americans,Alliance
Cost=200
Soylent=100
Points=10
IsSelectableCombatant=yes
VoiceSelect=<Name>Select
VoiceMove=<Name>Move
VoiceAttack=<Name>AttackCommand
VoiceFeedback=<Name>Fear
VoiceSpecialAttack=<Name>Move
DieSound=<Name>Die
Locomotor={4A582744-9839-11d1-B709-00A024DDAFD1}
PhysicalSize=1
MovementZone=Infantry
ThreatPosed=10
ImmuneToVeins=yes
ImmuneToPsionics=no
Bombable=yes
Deployer=yes
DeployFire=yes
VeteranAbilities=STRONGER,FIREPOWER,ROF,SIGHT,FASTER
EliteAbilities=SELF_HEAL,STRONGER,FIREPOWER,ROF
Size=1
Crushable=yes
DeploySound=<Name>Deploy
UndeploySound=<Name>Undeploy
ElitePrimary=<Pri>E
EliteSecondary=<Sec>E
IFVMode=2
", "盟军基础步兵"),
New CodeSnippet("infayur", "
;名称 <Name> 主武器 <Pri>
[<Name>]
UIName=Name:<Name>
Name=<Name>
Category=Soldier
Primary=<Pri>
Occupier=yes
OccupyWeapon=<Pri>Occupy
EliteOccupyWeapon=<Pri>OccupyE
OpenTransportWeapon=1
Prerequisite=YABRCK
CrushSound=InfantrySquish
Strength=125
Pip=white
OccupyPip=PersonBlue
Armor=none
TechLevel=1
Sight=5
Speed=4
Owner=British,French,Germans,Americans,Alliance,Russians,Confederation,Africans,Arabs,YuriCountry
Cost=200
Soylent=100
Points=10
IsSelectableCombatant=yes
VoiceSelect=<Name>Select
VoiceMove=<Name>Move
VoiceAttack=<Name>AttackCommand
VoiceFeedback=<Name>Fear
VoiceSpecialAttack=<Name>Move
DieSound=<Name>Die
Locomotor={4A582744-9839-11d1-B709-00A024DDAFD1}
PhysicalSize=1
MovementZone=Infantry
ThreatPosed=10
ImmuneToVeins=yes
ImmuneToPsionics=no
Bombable=yes
VeteranAbilities=STRONGER,FIREPOWER,ROF,SIGHT,FASTER
EliteAbilities=SELF_HEAL,STRONGER,FIREPOWER,ROF
Size=1
Crushable=yes
ElitePrimary=<Pri>E
IFVMode=2
", "尤里基础步兵"),
New CodeSnippet("infa", "
;名称 <Name> 主武器 <Pri>
[<Name>]
UIName=Name:<Name>
Name=<Name>
Category=Soldier
Primary=<Pri>
Occupier=yes
OccupyWeapon=<Pri>Occupy
EliteOccupyWeapon=<Pri>OccupyE
OpenTransportWeapon=1
Prerequisite=BARRACKS
CrushSound=InfantrySquish
Strength=125
Pip=white
OccupyPip=PersonBlue
Armor=none
TechLevel=1
Sight=5
Speed=4
Owner=British,French,Germans,Americans,Alliance,Russians,Confederation,Africans,Arabs,YuriCountry
Cost=200
Soylent=100
Points=10
IsSelectableCombatant=yes
VoiceSelect=<Name>Select
VoiceMove=<Name>Move
VoiceAttack=<Name>AttackCommand
VoiceFeedback=<Name>Fear
VoiceSpecialAttack=<Name>Move
DieSound=<Name>Die
Locomotor={4A582744-9839-11d1-B709-00A024DDAFD1}
PhysicalSize=1
MovementZone=Infantry
ThreatPosed=10
ImmuneToVeins=yes
ImmuneToPsionics=no
Bombable=yes
VeteranAbilities=STRONGER,FIREPOWER,ROF,SIGHT,FASTER
EliteAbilities=SELF_HEAL,STRONGER,FIREPOWER,ROF
Size=1
Crushable=yes
ElitePrimary=<Pri>E
IFVMode=2
", "三阵营普通步兵"),
New CodeSnippet("gunwp", "
[<Weapon>]
Damage=15
ROF=20
Range=4
Projectile=InvisibleLow
Speed=100
Warhead=SA
Report=GIAttack
Anim=<Weapon>-N,<Weapon>-NE,<Weapon>-E,<Weapon>-SE,<Weapon>-S,<Weapon>-SW,<Weapon>-W,<Weapon>-NW
OccupantAnim=UCFLASH
", "枪械类武器"),
New CodeSnippet("gunwh", "
[<Weapon>WH]
Verses=100%,80%,80%,50%,25%,25%,75%,50%,25%,100%,100%
InfDeath=1
AnimList=PIFFPIFF,PIFFPIFF
Bullets=yes
ProneDamage=70%
", "枪械类弹头"),
New CodeSnippet("gunall", (Function()
                               Dim ini As New StringBuilder
                               Dim i As Integer = 0
                               For Each wp In {"", "Occupy", "E", "OccupyE"}
                                   ini.AppendLine($"[<Weapon>{wp}]
Damage={15 + i * 2}
ROF={60 / (i + 3)}
Range={CInt(2 + i * 0.75)}
Projectile=<Weapon>Proj
Speed=100
Warhead=<Weapon>WH
Report=<Weapon>Attack
Anim=<Weapon>-N,<Weapon>-NE,<Weapon>-E,<Weapon>-SE,<Weapon>-S,<Weapon>-SW,<Weapon>-W,<Weapon>-NW
OccupantAnim=<Weapon>FLASH
")
                                   i += 1
                               Next
                               ini.AppendLine("
[<Weapon>Proj]
Inviso=yes
Image=none
SubjectToCliffs=yes
SubjectToElevation=yes
SubjectToWalls=yes

[<Weapon>WH]
Verses=100%,80%,80%,50%,45%,35%,75%,50%,25%,100%,100%
InfDeath=1
AnimList=PIFFPIFF,PIFFPIFF
Bullets=yes
ProneDamage=70%")
                               Return ini.ToString
                           End Function).Invoke, "枪械武器完整代码"),
New CodeSnippet("cannonwp", "
[<Weapon>]
Damage=90
ROF=65
Range=5.75
Projectile=Cannon
Speed=40
Warhead=AP
Report=RhinoTankAttack
Anim=GUNFIRE
Bright=yes
", "炮弹类武器"),
New CodeSnippet("laser1e", "
[<Weapon>]
Damage=100
ROF=100
Range=10
Projectile=<Weapon>P
Speed=40
Report=<Weapon>Attack
Warhead=<Weapon>WH
Bright=yes
IsHouseColor=true
LaserOuterSpread= 0,0,0
LaserDuration = 15
IsLaser=true

[<Weapon>E]
Damage=150
ROF=80
Range=12
Projectile=<Weapon>EP
Speed=40
Report=<Weapon>Attack
Warhead=<Weapon>WH
Bright=yes
IsHouseColor=true
LaserOuterSpread= 0,0,0
LaserDuration = 15
IsLaser=true

[<Weapon>Fragment]
Damage=30
ROF=120
Range=3
Projectile=<Weapon>P
Speed=10
Warhead=<Weapon>WH
Bright=yes
IsHouseColor=true
LaserOuterSpread= 0,0,0
LaserDuration = 15
IsLaser=true

[<Weapon>FragmentE]
Damage=40
ROF=120
Range=3
Projectile=<Weapon>EP
Speed=10
Warhead=<Weapon>WH
Bright=yes
IsHouseColor=true
LaserOuterSpread= 0,0,0
LaserDuration = 15
IsLaser=true

[<Weapon>P]
ShrapnelWeapon=<Weapon>Fragment
ShrapnelCount=5
Inviso=yes
Image=none
SubjectToCliffs=yes
SubjectToElevation=no
SubjectToWalls=no

[<Weapon>EP]
ShrapnelWeapon=<Weapon>FragmentE
ShrapnelCount=7
Inviso=yes
Image=none
SubjectToCliffs=yes
SubjectToElevation=no
SubjectToWalls=no

[<Weapon>WH]
Wall=no
Verses=100%,100%,100%,75%,50%,50%,200%,200%,200%,100%,100%
AnimList=XGRYSML1,XGRYSML2,EXPLOSML,XGRYMED1,XGRYMED2,EXPLOMED,EXPLOLRG,TWLT070

[<Weapon>Payload]
UIName=Name:<Weapon>Payload
Name=<Weapon>Payload
Category=Soldier
Primary=<Weapon>Fragment
Secondary=<Weapon>FragmentE
ElitePrimary=
EliteSecondary=
DeathWeapon=
TechLevel=-1
", "带有1次溅射和高级版本的激光类武器"),
New CodeSnippet("tesla1e", "
[<Weapon>]
Damage=100
ROF=100
Range=10
Projectile=<Weapon>P
Speed=40
Report=<Weapon>Attack
Warhead=<Weapon>WH
Bright=yes
IsElectricBolt=true
AssaultAnim=UCELEC

[<Weapon>E]
Damage=150
ROF=80
Range=12
Projectile=<Weapon>EP
Speed=40
Report=<Weapon>Attack
Warhead=<Weapon>WH
Bright=yes
IsElectricBolt=true
AssaultAnim=UCELEC

[<Weapon>Fragment]
Damage=30
ROF=120
Range=3
Projectile=<Weapon>P
Speed=10
Warhead=<Weapon>WH
Bright=yes
IsElectricBolt=true
AssaultAnim=UCELEC

[<Weapon>FragmentE]
Damage=40
ROF=120
Range=3
Projectile=<Weapon>EP
Speed=10
Warhead=<Weapon>WH
Bright=yes
IsElectricBolt=true
AssaultAnim=UCELEC

[<Weapon>P]
ShrapnelWeapon=<Weapon>Fragment
ShrapnelCount=3
Inviso=yes
Image=none
SubjectToCliffs=yes
SubjectToElevation=no
SubjectToWalls=no

[<Weapon>EP]
ShrapnelWeapon=<Weapon>FragmentE
ShrapnelCount=5
Inviso=yes
Image=none
SubjectToCliffs=yes
SubjectToElevation=no
SubjectToWalls=no

[<Weapon>WH]
Wall=no
Verses=500%,400%,300%,80%,75%,70%,70%,30%,50%,100%,100%
AnimList=XGRYSML1,XGRYSML2,EXPLOSML,XGRYMED1,XGRYMED2,EXPLOMED,EXPLOLRG,TWLT070

[<Weapon>Payload]
UIName=Name:<Weapon>Payload
Name=<Weapon>Payload
Category=Soldier
Primary=<Weapon>Fragment
Secondary=<Weapon>FragmentE
ElitePrimary=
EliteSecondary=
DeathWeapon=
TechLevel=-1
", "带有1次溅射和高级版本的磁暴类武器"),
    New CodeSnippet("airc", "
;名称<Name> 主武器<Pri>
[<Name>]
UIName=Name:<Name>
Name=<Name>
Prerequisite=RADAR
Primary=<Pri>
CanPassiveAquire=no ; Won't try to pick up own targets
CanRetaliate=no; Won't fire back when hit
Strength=200
Category=AirPower
Armor=light
TechLevel=3
Sight=8
RadarInvisible=no
Landable=yes
MoveToShroud=yes
Dock=GAAIRC,AMRADR
PipScale=Ammo
Speed=14
PitchSpeed=1.1
PitchAngle=0
OmniFire=yes
Owner=British,French,Germans,Americans,Alliance
Cost=1200
Points=20
ROT=3
Ammo=1
Crewed=yes
ConsideredAircraft=yes
AirportBound=yes ; If I ever need to land and there are no airports I crash because I can only land on them
GuardRange=30
Explosion=TWLT070,S_BANG48,S_BRNL58,S_CLSN58,S_TUMU60
MaxDebris=3
IsSelectableCombatant=yes
VoiceSelect=<Name>Select
VoiceMove=<Name>Move
VoiceAttack=<Name>AttackCommand
VoiceCrashing=<Name>VoiceDie
DieSound=
MoveSound=<Name>MoveLoop
CrashingSound=<Name>Die
ImpactLandSound=GenAircraftCrash
Locomotor={4A582746-9839-11d1-B709-00A024DDAFD1}
MovementZone=Fly
ThreatPosed=20	; This value MUST be 0 for all building addons
DamageParticleSystems=SparkSys,SmallGreySSys
VeteranAbilities=STRONGER,FIREPOWER,SIGHT,FASTER
EliteAbilities=STRONGER,FIREPOWER,ROF
Fighter=yes
AllowedToStartInMultiplayer=no
ImmuneToPsionics=yes
ElitePrimary=<Pri>E
PreventAttackMove=yes
", "盟军歼击机"),
New CodeSnippet("airs", "
;名称<Name> 主武器<Pri>
[<Name>]
UIName=Name:<Name>
Name=<Name> Airship
Prerequisite=NAWEAP,NATECH
Primary=<Pri>
Strength=2000
Category=AirPower
Armor=medium
TechLevel=10
Sight=8
RadarInvisible=no
MoveToShroud=yes
BalloonHover=yes
;OmniFire=yes
Speed=5
JumpjetSpeed=5 ;params not defined use defaults (old globals way up top called Jumpjet controls)
JumpjetClimb=6 ; SJM increased from 2 so <Name> can get out of factory before doors close
JumpjetCrash=12 ; Climb, but down
JumpJetAccel=10
JumpJetTurnRate=2
JumpjetHeight=750
;JumpjetWobbles=.01
;JumpjetDeviation=1
JumpjetNoWobbles=yes
Crashable=yes ; JJ plummets down like aircraft
PitchSpeed=.9
PitchAngle=0
Owner=Russians,Confederation,Africans,Arabs
Cost=2000
Soylent=2000
Points=100
ROT=10
SpeedType=Hover
Crewed=no
ConsideredAircraft=yes
Explosion=TWLT070,S_BANG48,S_BRNL58,S_CLSN58,S_TUMU60
MaxDebris=14
IsSelectableCombatant=yes
VoiceSelect=<Name>Select
VoiceMove=<Name>Move
VoiceAttack=<Name>AttackCommand
VoiceFeedback=
VoiceCrashing=<Name>VoiceDie
DieSound=
CreateSound=<Name>Created
CrashingSound=<Name>Die
ImpactLandSound=<Name>Crash
Locomotor={92612C46-F71F-11d1-AC9F-006008055BB5} ;Jumpjet
MovementZone=Fly
ThreatPosed=30	; This value MUST be 0 for all building addons
DamageParticleSystems=SparkSys,SmallGreySSys
AuxSound1=Dummy ;Taking off
AuxSound2=Dummy ;Landing
AllowedToStartInMultiplayer=no
VeteranAbilities=STRONGER,FIREPOWER,SIGHT,FASTER
EliteAbilities=SELF_HEAL,STRONGER,FIREPOWER,ROF
SelfHealing=Yes
MoveSound=<Name>MoveLoop
ElitePrimary=<Pri>E
Parasiteable=no
Size=50
Bunkerable=no; Units default to yes, others default to no
", "苏军空艇"),
New CodeSnippet("subm", "
;名称<Name> 主武器<Pri>
[<Name>]
UIName=Name:<Name>
Name=<Name>
Prerequisite=NAYARD
Primary=<Pri>
NavalTargeting=5
LandTargeting=1
FireAngle=64
Category=AFV
Strength=600
Naval=yes
Armor=heavy
TechLevel=2
Underwater=yes
Sight=4
Sensors=yes
SensorsSight=7
Speed=4
CrateGoodie=no
Owner=Russians,Confederation,Africans,Arabs
AllowedToStartInMultiplayer=no
Cost=1000
Soylent=1000
Turret=no
Points=30
ROT=2
Crusher=no;gs yes
Crewed=no
Weight=4
Explosion=TWLT070,S_BANG48,S_BRNL58,S_CLSN58,S_TUMU60
VoiceSelect=Typhoon<Name>Select
VoiceMove=Typhoon<Name>Move
VoiceAttack=Typhoon<Name>AttackCommand
VoiceFeedback=<Name>Fear
DieSound=GenSmallWaterDie
MoveSound=<Name>MoveStart
Locomotor={2BEA74E1-7CCA-11d3-BE14-00104B62A16C};{4A582741-9839-11d1-B709-00A024DDAFD1}
SpeedType=Float
MovementZone=Water
ThreatPosed=20	; This value MUST be 0 for all building addons
Accelerates=true
Cloakable=yes
CloakingSpeed=1
VeteranAbilities=STRONGER,FIREPOWER,ROF,SIGHT,FASTER
EliteAbilities=SELF_HEAL,STRONGER,FIREPOWER,ROF
TooBigToFitUnderBridge=true
ElitePrimary=<Pri>E
Size=20
", "苏军潜艇"),
New CodeSnippet("subm", "
;名称<Name> 主武器<Pri> 副武器<Sec>
[<Name>]
UIName=Name:<Name>
Name=Sea Scorpion
Prerequisite=NAYARD,NARADR
Primary=<Pri>
Secondary=<Sec>
ToProtect=yes
Category=AFV
Strength=400
Naval=yes
Armor=heavy
TechLevel=6
MovementRestrictedTo=Water
Sight=8
Speed=8
CrateGoodie=no
Owner=Russians,Confederation,Africans,Arabs
AllowedToStartInMultiplayer=no
Cost=600
Soylent=600
Points=20
ROT=6
Crusher=no
Crewed=no
IsSelectableCombatant=yes
Weight=2
Explosion=TWLT070,S_BANG48,S_BRNL58,S_CLSN58,S_TUMU60
VoiceSelect=<Name>Select
VoiceMove=<Name>Move
VoiceAttack=<Name>AttackCommand
VoiceFeedback=
DieSound=GenSmallWaterDie
MoveSound=<Name>MoveStart
Locomotor={2BEA74E1-7CCA-11d3-BE14-00104B62A16C};{4A582741-9839-11d1-B709-00A024DDAFD1}
;SpeedType=Amphibious ;gs Wha!?!
;MovementZone=Amphibious
SpeedType=Float
MovementZone=Water
ThreatPosed=25	; This value MUST be 0 for all building addons
DamageParticleSystems=SparkSys,SmallGreySSys
VeteranAbilities=STRONGER,FIREPOWER,ROF,SIGHT,FASTER
EliteAbilities=SELF_HEAL,STRONGER,FIREPOWER,ROF
ElitePrimary=<Pri>E
EliteSecondary=<Sec>E
Size=20
", "苏军船"),
New CodeSnippet("buil", "
;名称<Name>
[<Name>]
UIName=Name:<Name>
Name=<Name>
BuildCat=Combat
Prerequisite=NACNST
Strength=1000
Armor=concrete
TechLevel=5
Adjacent=2
Sight=4
Owner=British,French,Germans,Americans,Alliance,Russians,Confederation,Africans,Arabs,YuriCountry
AIBasePlanningSide=1 ;gs 0 for Good, 1 for Evil
Cost=1000
Points=10
Power=-20
Crewed=yes
Capturable=false
Explosion=TWLT070,S_BANG48,S_BRNL58,S_CLSN58,S_TUMU60
MaxDebris=15
MinDebris=5
DebrisAnim=Dbris3sm,Dbris4lg,Dbris4sm,Dbris6sm,Dbris7lg,Dbris7sm,Dbris8sm,Dbris9lg,Dbris10lg,Dbris10sm
ThreatPosed=0 ; This value MUST be 0 for all building addons
DamageParticleSystems=SparkSys,LGSparkSys
Nominal=yes
RevealToAll=yes
HasStupidGuardMode=false
Spyable=yes
AIBuildThis=yes
ProtectWithWall=yes
BuildLimit=1
DamageSelf=yes
", "苏军建筑"),
New CodeSnippet("npls", "
;名称<Name>
;不要忘记注册鼠标动画
;[MouseCursors]
;<Name>Action=<FrameStart>,<FrameCount>,<FrameDelay>,<ThumbnailStart>,<ThumbnailCount>,<CursorPosX>,<CursorPosY>
;注:CursorPos枚举 0左上,12345中,54321右下
[<Name>Special]
UIName=Name:<Name>Special
Name=<Name>Special
Powered=true
IsPowered=true
RechargeTime=25
Type=LightningStorm; 关键字段
Action=<Name>Action; 添加自定义鼠标动画，否则无效，详见28节
SidebarImage=<Name>ICON
ShowTimer=yes
DisableableFromShell=no
Range=7
LineMultiplier=2
AIDefendAgainst=yes;新参数
Warhead=<Name>WH;闪电风暴弹头
LStormPrintText=yes
LStormText=TXT_LIGHTNING_STORM
LStormText2=TXT_LIGHTNING_STORM_APPROACHING
LStormDeferment=250;攻击的延时
LStormDamage=250;杀伤力
LStormDuration=180;持续时间
LStormHitDelay=10; 攻击时间的间隔
LStormScatterDelay=5;攻击空间的间隔
LStormCellSpread=10;攻击有多远
LStormSeparation=3;间隔 云/闪电
LStormSound=WeatherIntro;起初声音
LStormSounds=WeatherStrike; 爆炸声音
LStormClouds=<Name>Cloud;云图像
LStormBolts=<Name>Lighting;闪电图像
LStormBoltExplosion=<Name>Explode;爆炸图像
LightRed=50;红
LightGreen=50;绿
LightBlue=100;蓝
LightAmbient=70
LightGround=0
LightLevel=3
Lighting=yes; 启动环境变色系统
", "np新闪电风暴"),
New CodeSnippet("npani", "
;名称<Name>
;不要忘记注册鼠标动画
;[MouseCursors]
;<Name>Action=<FrameStart>,<FrameCount>,<FrameDelay>,<ThumbnailStart>,<ThumbnailCount>,<CursorPosX>,<CursorPosY>
;注:CursorPos枚举 0左上,12345中,54321右下
[<Name>Special] 
UIName=Name:<Name>Special
Name=<Name>
RechargeTime=1
Type=Animation
Action=<Name>Action
SidebarImage=<Name>ICON
ShowTimer=yes
DisableableFromShell=no
Animation=<Name>Anim
AITargetingType=4
", "np动画超级武器"),
New CodeSnippet("nppa", "
;名称<Name> 飞机<Plane> 单位<Unit>
;不要忘记注册鼠标动画
;[MouseCursors]
;<Name>Action=<FrameStart>,<FrameCount>,<FrameDelay>,<ThumbnailStart>,<ThumbnailCount>,<CursorPosX>,<CursorPosY>
;注:CursorPos枚举 0左上,12345中,54321右下
[<Name>Special]
UIName=Name:<Name>
Name=<Name>
IsPowered=false
RechargeTime=5
Type=NewParaDrop
Action=<Name>Action
SidebarImage=<Name>ICON
ShowTimer=yes
DisableableFromShell=no
ParaDropPlane=<Plane>
DropInf=<Unit>
DropInfNum=8
", "np伞兵"),
New CodeSnippet("tank", "
;名称<Name> 主武器<Pri>
[<Name>]
UIName=Name:<Name>
Name=<Name>
Prerequisite=NAWEAP
Primary=<Pri>
Strength=450
Category=AFV
Armor=heavy
Turret=yes
IsTilter=yes
PipScale=Passengers
TargetLaser=no
TooBigToFitUnderBridge=true
TechLevel=2
Sight=8
Speed=5
CrateGoodie=no
Crusher=yes
Owner=Russians,Confederation,Africans,Arabs
Cost=900
Soylent=900
Points=25
ROT=5
IsSelectableCombatant=yes
Explosion=TWLT070,S_BANG48,S_BRNL58,S_CLSN58,S_TUMU60
VoiceSelect=<Name>Select
VoiceMove=<Name>Move
VoiceAttack=<Name>AttackCommand
VoiceFeedback=
DieSound=<Name>Die
MoveSound=<Name>MoveStart
CrushSound=<Name>Crush
Maxdebris=3
Locomotor={4A582741-9839-11d1-B709-00A024DDAFD1}
MovementZone=Destroyer
ThreatPosed=40
DamageParticleSystems=SparkSys,SmallGreySSys
DamageSmokeOffset=100, 100, 275
Weight=3.5
VeteranAbilities=STRONGER,FIREPOWER,SIGHT,FASTER
EliteAbilities=SELF_HEAL,STRONGER,FIREPOWER,ROF
Accelerates=false
ZFudgeColumn=8
ZFudgeTunnel=13
Size=3
OpportunityFire=yes
ElitePrimary=<Pri>E
", "苏联坦克"),
New CodeSnippet("alli", "British,French,Germans,Americans,Alliance", "默认的盟军国家"),
New CodeSnippet("sovi", "Russians,Confederation,Africans,Arabs", "默认的苏联国家"),
New CodeSnippet("yuri", "YuriCountry", "默认的尤里国家")
    }
End Class
