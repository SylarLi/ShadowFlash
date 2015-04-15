using System;
using System.Collections.Generic;

/// <summary>
/// 可持久化
/// T只能为int, long, float, string, enum
/// </summary>
public interface IPersistable
{
    int GetInt(string key);

    void SetInt(string key, int value);

    long GetLong(string key);

    void SetLong(string key, long value);

    float GetFloat(string key);

    void SetFloat(string key, float value);

    string GetString(string key);

    void SetString(string key, string value);

    T GetEnum<T>(string key) where T : struct, IConvertible;

    void SetEnum<T>(string key, T value) where T : struct, IConvertible;
}
