/*
文件名：jsonLoader.cs
编辑人：fortunate瑞
文件描述：用于加载staticData文件夹下的json文件，并返回json数据 
*/

// 使用JsonLoader时，不需要using，直接使用JsonLoader.LoadJsonText("fileName")或JsonLoader.LoadJson<T>("fileName")即可

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class JsonLoader
{
    /// <summary>
    /// 从Resources/StaticData文件夹加载指定的JSON文件
    /// </summary>
    /// <typeparam name="T">要转换成的目标类型</typeparam>
    /// <param name="fileName">不带扩展名的文件名</param>
    /// <returns>解析后的对象</returns>
    public static T LoadJson<T>(string fileName)
    {
        string path = "StaticData/" + fileName;
        TextAsset textAsset = Resources.Load<TextAsset>(path);
        
        if (textAsset == null)
        {
            Debug.LogError($"无法加载JSON文件: {path}");
            return default(T);
        }
        
        return JsonUtility.FromJson<T>(textAsset.text);
    }
    
    /// <summary>
    /// 从Resources/StaticData文件夹加载指定的JSON文件并返回原始文本
    /// </summary>
    /// <param name="fileName">不带扩展名的文件名</param>
    /// <returns>JSON文本内容</returns>
    public static string LoadJsonText(string fileName)
    {
        string path = "StaticData/" + fileName;
        TextAsset textAsset = Resources.Load<TextAsset>(path);
        
        if (textAsset == null)
        {
            Debug.LogError($"无法加载JSON文件: {path}");
            return null;
        }
        
        return textAsset.text;
    }
}

