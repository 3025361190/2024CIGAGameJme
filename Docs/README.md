## `Resources/StaticData`目录结构：
- `levels_config.json` ：用于管理每个关卡的难度，该文件以静态方式管理，只读不可写，打包后不暴露
- `global_config.json` ：用于管理全局初始化配置，该文件以静态方式管理，只读不可写，打包后不暴露
- `bullet_config.json` ：用于管理子弹配置，该文件以静态方式管理，只读不可写，打包后不暴露
- `mode_config.json` ：用于管理分裂模式配置，该文件以静态方式管理，只读不可写，打包后不暴露
- `rage_config.json` ：用于管理狂暴模式配置，该文件以静态方式管理，只读不可写，打包后不暴露
- `buffs_config.json` ：用于管理每个buff的属性，该文件以静态方式管理，只读不可写，打包后不暴露

## 详细说明：
### 1. `levels_config.json` ：
  | 参数 | 类型 | 说明 |
  | --- | --- | --- |
  | `levelId` | int | 关卡ID |
  | `isBoss` | int | 是否为BOSS关卡 |
  | `bossHealthPoint` | int | BOSS血量 |
  | `monsNum` | int | 小怪数量 |
  | `bossTime` | int | BOSS出现时间 |
  | `buffId` | int | 可能出现的buff ID |
  | `time` | int | 关卡时间 |
  | `enemySpawnInterval` | float | 敌人出现时间间隔 |
  | `enemyMoveSpeed` | float | 敌人移动速度 |
  | `enemyDamage` | int | 敌人伤害 |
  | `bossMoveSpeed` | float | BOSS移动速度 |
  | `bossDamage` | int | BOSS伤害 |
  

### 2. `global_config.json` ：
  | 参数 | 类型 | 说明 |
  | --- | --- | --- |
  | `initialBulletCount` | int | 初始子弹数量 |
  | `initialHealth` | int | 玩家初始生命值 |
  | `maxMoveSpeed` | float | 玩家最大移动速度 |
  | `flashDuration` | float | 玩家受伤闪烁持续时间 |
  | `flashInterval` | float | 玩家受伤闪烁间隔时间 |
  | `bloodRaturnValue` | float | 玩家回血参数（每60/bloodRaturnValue秒回复1点血量） |

### 3. `buffs_config.json` ：
  | 属性 | 类型 | 说明 |
| --- | --- | --- |
| `buffId` | int | Buff 唯一标识符 |
| `buffName` | string | Buff 名称 |
| `buffDescription` | string | Buff 描述文本 |
| `buffIcon` | string | Buff 图标在Resources/BuffIcon目录下的路径，不包含后缀 |
| `buffDuration` | float | Buff 持续时间（-1表示永久，0表示一次性buff） |
| `buffStackable` | bool | 是否可叠加 |
| `stackType` | int | 计算方式（"0"表示加法叠加，"1"表示乘法叠加） |
| `isNormalBuff` | bool | 是否为普通buff（普通buff只在非boss关卡出现） |

  其他详细参数见[buff说明文档](./Buff说明文档.md)

### 4. `mode_config.json` ：
  | 参数 | 类型 | 说明 |
  | --- | --- | --- |
  | `splitModeCD` | int | 分裂模式（进入白汤）的技能CD（白汤进红汤不需要CD） |
  | `splitModeDuration` | int | 分裂模式最长持续时间 |


### 5. `rage_config.json` ：
  | 参数 | 类型 | 说明 |
  | --- | --- | --- |
  | `rageFiringRate` | int | 狂暴模式下子弹的射速 |
  | `rageThreshold` | int | 进入狂暴模式的阈值（清汤结束时收回的子弹数量与清汤持续时发射的子弹数量的比值） |
  | `rageMinValue` | int | 进入狂暴模式最少需要回收的子弹数量 |
  | `rageActivateTime` | float | 尝试进入狂暴状态的时间（等待子弹收回的时间） |
  | `rageDuration` | float | 狂暴模式持续的时间 |


### 6. `bullet_config.json` ：
  | 参数 | 类型 | 说明 |
  | --- | --- | --- |
  | `maxBullets` | int | 最大子弹数量 |
  | `firingRate` | float | 射击频率 |
  | `bulletSpeed` | float | 子弹速度 |
  | `bulletCountInScreenMax` | int | 屏幕中子弹的最大存在数量 |
  | `speedAfterSplit` | int | 分裂后子弹的速度，一般初始等于子弹速度（bulletSpeed） |
  | `splitNum` | int | 初始子弹每次分裂的数量 |

### 7. `enemy_config.json` ：
  | 参数 | 类型 | 说明 |
  | --- | --- | --- |
  | `moveSpeed` | float | 敌人移动速度系数（已迭代为运行时参数，策划请勿修改，如需配置请在`level_config.json`中配置） |
  | `damage` | int | 敌人伤害系数（已迭代为运行时参数，策划请勿修改，如需配置请在`level_config.json`中配置） |
  | `randomRange` | float | 敌人随机移动范围 |
  | `changeDirectionInterval` | float | 敌人改变方向的间隔时间 |
  | `knockbackBaseForce` | float | 击退力基础值 |
  | `knockbackTime` | float | 击退持续时间 |
  | `bufferTime` | float | 击退后的缓冲时间 |
  | `spawnInterval` | float | 敌人生成的时间间隔系数（已迭代为运行时参数，策划请勿修改，如需配置请在`level_config.json`中配置） |
  | `attackCooldownTime` | float | 敌人伤害冷却时间 |
  | `chainEffectRadius` | float | 连锁爆炸范围半径 |
  | `chainExplosionDelay` | float | 连锁爆炸的传递延迟时间 |
  | `chainKnockbackForceMultiplier` | float | 连锁击退的力系数 |

### 8. `boss_config.json` ：
  | 参数 | 类型 | 说明 |
  | --- | --- | --- |
  | `moveSpeed` | float | BOSS移动速度系数（已迭代为运行时参数，策划请勿修改，如需配置请在`level_config.json`中配置） |
  | `damage` | int | BOSS伤害系数（已迭代为运行时参数，策划请勿修改，如需配置请在`level_config.json`中配置） |
  | `randomRange` | float | BOSS随机移动范围 |
  | `changeDirectionInterval` | float | BOSS改变方向的间隔时间 |
  | `knockbackBaseForce` | float | BOSS击退力基础值 |
  | `knockbackTime` | float | BOSS击退持续时间 |
  | `bufferTime` | float | BOSS击退后的缓冲时间 |
  | `attackCooldownTime` | float | BOSS伤害冷却时间 |
  | `chainKnockbackForceMultiplier` | float | BOSS连锁击退的力系数 |
  | `flashDuration` | float | BOSS闪烁持续时间 |
  | `flashInterval` | float | BOSS闪烁间隔时间 |



### 999. 关于玩家个人数据（如金币、角色等级、关卡进度）应该缓存在哪里？

推荐的本地缓存位置：Application.persistentDataPath

这是 Unity 官方推荐的跨平台本地存储路径，适合用于保存玩家个人数据。

示例用途：

玩家存档（JSON 格式、加密）

设置项（音量、画质等）

关卡通关记录

登录状态、token 等

针对 TapTap 小游戏平台的特别注意：

TapTap 小游戏使用的是浏览器 WebGL 构建 + 本地缓存存储（IndexedDB）。

Unity WebGL 下，Application.persistentDataPath 实际是：

使用 IndexedDB 存储数据（保存在浏览器缓存中）

受浏览器清理策略限制（比如浏览器清缓存后会丢失）

🔐 建议：

存档前加密（Base64、AES、签名都可）

定期上传数据到云端（如你支持账号登录）

