/*
文件名：LevelConfigList.cs
编辑人：fortunate瑞
文件描述：关卡配置列表类，用于从JSON文件加载关卡配置列表
*/

using System.Collections.Generic;
using UnityEngine;


// 因为只能将json转换为对象，所以需要一个封装类来将json列表转换为对象
// Unity 的 JsonUtility 只支持数组的反序列化，不支持 List<T>
[System.Serializable]
public class LevelConfigList
{
    public LevelConfig[] levels;
}
