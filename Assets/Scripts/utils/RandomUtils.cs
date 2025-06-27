/*
文件名：RandomUtils.cs
编辑人：Fortunate瑞
文件描述：随机数工具类
*/

using System;
using System.Collections.Generic;
using UnityEngine;

public static class RandomUtils
{
    /// <summary>
    /// 在 [min, max) 范围内生成 n 个不重复的随机整数。
    /// </summary>
    /// <param name="min">最小值（包含）</param>
    /// <param name="max">最大值（不包含）</param>
    /// <param name="n">随机数数量</param>
    /// <returns>一个包含 n 个不重复整数的列表</returns>
    public static List<int> GetUniqueRandomIntegers(int min, int max, int n)
    {
        if (max <= min)
            throw new ArgumentException("max 必须大于 min。");
        if (n > (max - min))
            throw new ArgumentException("范围不足以生成 " + n + " 个不重复随机数。");

        List<int> pool = new List<int>();
        for (int i = min; i < max; i++)
        {
            pool.Add(i);
        }

        // 洗牌（Fisher–Yates Shuffle）
        for (int i = pool.Count - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);
            (pool[i], pool[j]) = (pool[j], pool[i]);
        }

        return pool.GetRange(0, n);
    }
}
