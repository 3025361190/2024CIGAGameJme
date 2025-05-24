# Buff 说明文档

## Buff 基础设计

所有 Buff 都继承自基类 `BaseBuff`，包含以下基础属性：

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

## 数值型 Buff 详细参数

### 1. 增加子弹发射速度 (Buff ID: 1)
| 参数 | 类型 | 说明 |
| --- | --- | --- |
| `firingRateMultiplier` | float | 射速倍率 |
| `stackValue` | float | 叠加增幅 |

### 2. 减少怪移动速度 (Buff ID: 2)
| 参数 | 类型 | 说明 |
| --- | --- | --- |
| `enemySpeedMultiplier` | float | 敌人速度倍率 |
| `stackValue` | float | 叠加增幅 |

### 3. 子弹暴击 (Buff ID: 3)
| 参数 | 类型 | 说明 |
| --- | --- | --- |
| `criticalChance` | float | 暴击概率 |
| `stackValue` | float | 叠加增幅 |

### 4. 增加人物移动速度 (Buff ID: 4)
| 参数 | 类型 | 说明 |
| --- | --- | --- |
| `playerSpeedMultiplier` | float | 移动速度倍率 |
| `stackValue` | float | 叠加增幅 |

### 5. 连爆范围增大 (Buff ID: 5)
| 参数 | 类型 | 说明 |
| --- | --- | --- |
| `chainExplosionRange` | float | 连爆范围 |
| `stackValue` | float | 叠加增幅 |

### 6. 子弹分裂后速度增加 (Buff ID: 6)
| 参数 | 类型 | 说明 |
| --- | --- | --- |
| `splitBulletSpeedMultiplier` | float | 分裂后子弹速度倍率 |
| `stackValue` | float | 叠加增幅 |

### 7. 增加狂暴模式时间 (Buff ID: 7)
| 参数 | 类型 | 说明 |
| --- | --- | --- |
| `splitModeDurationBonus` | float | 增加的狂暴模式时间(秒) |
| `stackValue` | float | 叠加增幅 |

### 8. 人物血量增加 (Buff ID: 8)
| 参数 | 类型 | 说明 |
| --- | --- | --- |
| `healthBonus` | int | 增加的血量值 |
| `stackValue` | int | 叠加增幅 |

### 9. 再次翻牌 (Buff ID: 9)
| 参数 | 类型 | 说明 |
| --- | --- | --- |
none

### 10. 我nb，我不需要buff (Buff ID: 10)
| 参数 | 类型 | 说明 |
| --- | --- | --- |
| `requiredCount` | int | 可以获得奖励的次数 |

## 机制型 Buff 详细参数

### 11. 子弹散射 (Buff ID: 11)
| 参数 | 类型 | 说明 |
| --- | --- | --- |
| `extraBulletDirections` | int | 额外射击方向数 |
| `spreadAngle` | float | 散射角度(度) |
| `stackValue` | int | 叠加增幅 |

### 12. 储备子弹模式 (Buff ID: 12)
| 参数 | 类型 | 说明 |
| --- | --- | --- |
| `sameColorMultiplier` | float | 相同颜色子弹分裂倍率 |
| `stackValue` | float | 叠加增幅 |

### 13. 起死回生 (Buff ID: 13)
| 参数 | 类型 | 说明 |
| --- | --- | --- |
| `emergencyBulletCount` | int | 紧急恢复的子弹数量 |
| `triggerThreshold` | int | 触发阈值(子弹数) |
| `cooldown` | float | 技能冷却时间(秒) |

### 14. 赌狗 (Buff ID: 14)
| 参数 | 类型 | 说明 |
| --- | --- | --- |
| `reducedSplitModeDuration` | float | 减少后的分裂模式持续时间(秒) |
| `splitMultiplierBoost` | float | 分裂倍率提升 |

## Buff 获取与叠加规则

1. 普通关卡结束后，玩家可从3个随机 Buff 中选择1个
2. 相同 Buff 可叠加，但受到 `maxStack` 限制
3. 叠加效果根据 `stackType` 属性决定是加法(+)还是乘法(*)叠加，具体数值由 `stackValue` 决定
