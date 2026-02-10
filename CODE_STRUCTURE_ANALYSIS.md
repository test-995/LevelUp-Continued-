# LevelUp-Continued 代码结构与含义分析

## 1. 项目定位
这是一个 RimWorld 模组（C# 类库），核心机制是给 Pawn 添加 `HealthLevel` 状态，并依据受伤造成的伤害累计经验，从而提升生命值倍率。

## 2. 目录与文件职责
- `LevelUp.sln`：解决方案入口。
- `LevelUp/LevelUp.csproj`：类库工程定义，目标框架 `net472`，并引用 RimWorld / Harmony / Unity 相关程序集。
- `LevelUp/LevelUpMod.cs`：模组入口和设置界面绘制。
- `LevelUp/LevelUpModSettings.cs`：模组参数持久化（升级速率、血量倍率、是否仅玩家等）。
- `LevelUp/HealthLevelling.cs`：核心升级逻辑（受伤后加经验、生成初始等级状态）。
- `LevelUp/HarmonyPatches.cs`：Harmony 补丁，改写 `Pawn.HealthScale` 与部位血量计算，支持浮点血量。
- `LevelUp/LevelUpHealthHediff.cs`：状态显示文本，展示“等级 + 当前等级进度”。
- `LevelUp/LevellingHediffDefOf.cs`、`LevelUp/LevellingSoundDefOf.cs`：DefOf 静态引用（状态定义、升级音效）。
- `LevelUp/SettingsHelper.cs`：设置界面扩展工具（单选组、数值输入布局等）。

## 3. 关键运行流程
1. 游戏加载后，Harmony 静态构造器注册补丁。
2. Pawn 生成时（`PostSpawnSetup`），根据 `playerOnly` 与阵营逻辑决定是否附加 `HealthLevel`。
3. Pawn 受伤时（`PostPostApplyDamage`），以本次伤害量换算经验并累加到 `HealthLevel.Severity`。
4. `Pawn.HealthScale` Getter 被补丁接管后，按等级对生命倍率进行复利/线性放大。
5. UI 中用 Hediff 文本显示等级整数部分与升级进度小数部分。

## 4. 设计要点与语义
- `Severity` 被复用为“等级值”：
  - `floor(severity)` = 当前等级；
  - 小数部分 = 距离下一等级的进度。
- 两种经验曲线：
  - `Compound Levelling`：升级需求按幂增长；
  - `Simple Levelling`：升级需求按线性增长。
- 两种血量成长：
  - `Compounding Health`：血量倍率按幂增长；
  - `Simple Health`：血量倍率按线性增长。
- README 提到本分支重点修复：
  - “只对玩家生效”逻辑问题；
  - 身体部件血量改为 `float` 计算，避免整数化误差。

## 5. 注意事项
- `.csproj` 中 `HintPath` 指向本地 Windows Steam 路径，当前仓库更像“源码镜像/反编译工程”，在其它环境不一定可直接编译。
- 部分代码存在反编译风格变量命名（`flagX`），可读性一般，但不影响功能理解。
