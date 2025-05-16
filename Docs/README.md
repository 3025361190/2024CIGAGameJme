## `Resources/StaticData`目录结构：
- `levels_config.json` ：用于管理每个关卡的难度，该文件以静态方式管理，只读不可写，打包后不暴露
- `global_config.json` ：用于管理全局配置，该文件以静态方式管理，只读不可写，打包后不暴露
- `buffs_config.json` ：用于管理每个buff的属性，该文件以静态方式管理，只读不可写，打包后不暴露

## 详细说明：
### 1. `levels_config.json` ：
  | 参数 | 说明 |
  | --- | --- |
  | `levelId` | 关卡ID |
  | `isBoss` | 是否为BOSS关卡 |
  | `bossNum` | BOSS数量 |
  | `monsNum` | 小怪数量 |
  | `bossTime` | BOSS出现时间 |
  | `buffId` | 可能出现的buff ID |
  | `time` | 关卡时间 |

### 2. `global_config.json` ：
  | 参数 | 说明 |
  | --- | --- |
  | `initialBulletCount` | 初始子弹数量 |
  | `firingRate` | 射击频率 |
  | `bulletSpeed` | 子弹速度 |
  | `splitModeCD` | 分裂模式（进入白汤）的技能CD（白汤进红汤不需要CD） |
  | `splitModeDuration` | 分裂模式最长持续时间 |

### 3. `buffs_config.json` ：
  | 属性 | 类型 | 说明 |
  | --- | --- | --- |
  | `buffId` | int | Buff 唯一标识符 |
  | `buffName` | string | Buff 名称 |
  | `buffDescription` | string | Buff 描述文本 |
  | `buffDuration` | float | Buff 持续时间（-1表示永久） |
  | `buffStackable` | bool | 是否可叠加 |
  | `maxStack` | int | 最大叠加层数 |
  | `buffIcon` | sprite | Buff 图标 |
  | `stackType` | int | 叠加方式（"0"表示加法叠加，"1"表示乘法叠加） |

其他详细参数见[buff说明文档](./Buff说明文档.md)


### 4. 关于玩家个人数据（如金币、角色等级、关卡进度）应该缓存在哪里？

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

