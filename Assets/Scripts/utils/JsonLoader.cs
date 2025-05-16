/*
文件名：jsonLoader.cs
编辑人：fortunate瑞
文件描述：用于加载staticData文件夹下的json文件，并返回json对象
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class JsonLoader
{
    /// <summary>
    /// 加载JSON文件并返回JObject对象
    /// </summary>
    /// <param name="path">JSON文件的相对路径（相对于Resources文件夹），不需要.json后缀。</param>
    /// <returns>解析后的JObject对象</returns>
    public static Newtonsoft.Json.Linq.JObject LoadJsonAsJObject(string path)
    {
        TextAsset textAsset = Resources.Load<TextAsset>(path);
        if (textAsset == null)
        {
            Debug.LogError($"无法加载JSON文件: {path}");
            return null;
        }
        
        try
        {
            return Newtonsoft.Json.Linq.JObject.Parse(textAsset.text);
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"解析JSON文件时出错: {path}, 错误: {ex.Message}");
            return null;
        }
    }
    // 如何访问JObject中的数据？
    // 例如：JObject jsonObject = JsonLoader.LoadJsonAsJObject("levels_config");
    // 访问方式：string name = (string)jsonObject["levels"][0]["name"]
}

