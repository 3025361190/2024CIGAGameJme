# Buff 说明文档

## Buff 基础设计

所有 Buff 都继承自基类 `BaseBuff`，包含以下基础属性：

| 属性 | 类型 | 说明 |
| --- | --- | --- |
| `buffId` | int | Buff 唯一标识符 |
| `buffName` | string | Buff 名称 |
| `buffDescription` | string | Buff 描述文本 |
| `buffIcon` | string | Buff 图标在Resources/BuffIcon目录下的路径 |
| `buffDuration` | float | Buff 持续时间（-1表示永久） |
| `buffStackable` | bool | 是否可叠加 |
| `stackType` | int | 计算方式（"0"表示加法叠加，"1"表示乘法叠加） |
| `isNormalBuff` | bool | 是否为普通buff（普通buff只在非boss关卡出现） |

## Buff详细配置

### 1. 强力射速（buffId:1）
- 说明：增加子弹发射速度

| 参数 | 类型 | 说明 |
| --- | --- | --- |
| `value` | float | 子弹发射速度倍率 |



### 11. 多多子弹（buffId:11）
- 说明：将当前子弹数乘指定值

| 参数 | 类型 | 说明 |
| --- | --- | --- |
| `value` | int | 子弹数倍率 |


