## 目录结构：
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

其他详细参数见[buff说明文档](../../../Docs/Buff说明文档.md)
