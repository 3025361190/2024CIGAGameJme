/*
文件名：jsonLoader.cs
编辑人：fortunate瑞
文件描述：用于加载static_json文件，并返回一个对象
*/

using UnityEngine;
using System.IO;
using System;

public class JsonLoader
{
    /// <summary>
    /// 从Resources文件夹加载JSON文件并转换为指定类型的对象
    /// </summary>
    /// <typeparam name="T">要转换成的目标类型</typeparam>
    /// <param name="fileName">JSON文件名称（不需要.json后缀）</param>
    /// <returns>转换后的对象</returns>
    public static T LoadJsonFromResources<T>(string fileName)
    {
        try
        {
            // 从Resources文件夹加载文本文件
            TextAsset jsonFile = Resources.Load<TextAsset>(fileName);
            
            if (jsonFile == null)
            {
                Debug.LogError($"无法找到JSON文件: {fileName}");
                return default(T);
            }

            // 将JSON文本转换为对象
            T result = JsonUtility.FromJson<T>(jsonFile.text);
            
            if (result == null)
            {
                Debug.LogError($"JSON转换失败: {fileName}");
                return default(T);
            }

            return result;
        }
        catch (Exception e)
        {
            Debug.LogError($"加载JSON文件时发生错误: {fileName}\n{e.Message}");
            return default(T);
        }
    }
}

