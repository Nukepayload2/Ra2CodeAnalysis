
Public Class AIHelpProvider
    Inherits HelpProvider

    Public Overrides Function GetHelpText(code As String) As String
        Return GetHelpTextWithUsage(code, AIHelp, AITypes)
    End Function

    Public Shared AIHelp As New Dictionary(Of String, String) From {{"VeteranLevel", "<否决的> 地图文件中指定此小队出现在地图时的兵种等级"},
{"MindControlDecision", "<不明确> 遭心灵控制后的动作，0自动，1加入控制者作战小队，2送入部队回收厂，3送入生化反应炉，4搜索敌人，5什么也不做"},
{"Loadable", "<不明确> 可装载"},
{"Full", "<不明确> <否决的> 创建小队成员的时候，如果小队中有运载工具，其他成员是否应该位于运载工具的内部"},
{"Annoyance", "<不明确> 烦恼效果，作用不明"},
{"GuardSlower", "<不明确> 降低反应速度，作用不明"},
{"Recruiter", "强制重组优先级较低的小队的成员，如果它们是可以重组的"},
{"Autocreate", "<不明确> 自动建造"},
{"Prebuild", "<不明确> 预先建造，作用不明"},
{"Reinforce", "<不明确> <否决的> 增援部队"},
{"Droppod", "<否决的> 地图文件中指定使用空降进入战区"},
{"UseTransportOrigin", "<不明确> 使用原始的运载工具"},
{"Whiner", "<不明确> 哀叫效果，作用不明"},
{"LooseRecruit", "<不明确> 解散新兵，作用不明"},
{"Aggressive", "侵略性的，电脑在防御达到一定数量之后再开始建造该小队"},
{"Suicide", "自毁式攻击，受到攻击不允许还击"},
{"Priority", "优先级"},
{"Max", "同一作战方至多允许同时存在的该类型小队个数"},
{"TechLevel", "<不明确> <否决的> 科技等级，疑似无效"},
{"OnTransOnly", "<不明确> 只能运输，作用不明"},
{"AvoidThreats", "规避威胁，小队成员在移动的过程中将会试图规避途中可能遭遇的敌方单位"},
{"IonImmune", "<已过时> 离子风暴发生时仍然建造"},
{"TransportsReturnOnUnload", "运载工具卸载后返回基地"},
{"AreTeamMembersRecruitable", "小队成员是否允许重组"},
{"IsBaseDefense", "基地防御小队"},
{"OnlyTargetHouseEnemy", "<不明确> 只攻击敌方单位，作用不明"},
{"Script", "指定脚本"}, {"House", "指定国家"}, {"TaskForce", "指定特遣小队"},
{"Group", "指定分组，对阵营使用的AI触发一般填-1"}, {"Name", "此主键的注释"}
}

    Private Shared AITypes As New Dictionary(Of String, String) From {{"Name", "String"},
{"Group", "Integer"},
{"VeteranLevel", "Boolean"},
{"MindControlDecision", "Integer"},
{"Loadable", "Boolean"},
{"Full", "Boolean"},
{"Annoyance", "Boolean"},
{"GuardSlower", "Boolean"},
{"House", "String"},
{"Recruiter", "Boolean"},
{"Autocreate", "Boolean"},
{"Prebuild", "Boolean"},
{"Reinforce", "Boolean"},
{"Droppod", "Boolean"},
{"UseTransportOrigin", "Boolean"},
{"Whiner", "Boolean"},
{"LooseRecruit", "Boolean"},
{"Aggressive", "Boolean"},
{"Suicide", "Boolean"},
{"Priority", "Integer"},
{"Max", "Integer"},
{"TechLevel", "Integer"},
{"OnTransOnly", "Boolean"},
{"AvoidThreats", "Boolean"},
{"IonImmune", "Boolean"},
{"TransportsReturnOnUnload", "Boolean"},
{"AreTeamMembersRecruitable", "Boolean"},
{"IsBaseDefense", "Boolean"},
{"OnlyTargetHouseEnemy", "Boolean"},
{"Script", "String"},
{"TaskForce", "String"}
}
End Class