using Core;
using ProtoBuf;
using System;
using System.Collections;
using System.Collections.Generic;

[ProtoContract]
public class Role : EventDispatcher, IRole
{
    public const string id = "id";
    public const string type = "type";
    public const string name = "name";
    public const string entityId = "entityId";

    public const int defaultInt = 0;
    public const long defaultLong = 0;
    public const float defaultFloat = 0;
    public const string defaultString = "";

    [ProtoMember(1)]
    private Dictionary<string, int> _ints;

    [ProtoMember(2)]
    private Dictionary<string, long> _longs;

    [ProtoMember(3)]
    private Dictionary<string, float> _floats;

    [ProtoMember(4)]
    private Dictionary<string, string> _strings;

    [ProtoMember(5)]
    private Dictionary<string, IConvertible> _enums;

    public Role()
    {
        _ints = new Dictionary<string, int>();
        _longs = new Dictionary<string, long>();
        _floats = new Dictionary<string, float>();
        _strings = new Dictionary<string, string>();
        _enums = new Dictionary<string, IConvertible>();

        _longs[id] = defaultInt;
        _enums[type] = RoleType.None;
        _strings[name] = defaultString;
        _ints[entityId] = defaultInt;
    }

    public int GetInt(string key)
    {
        return _ints[key];
    }

    public void SetInt(string key, int value)
    {
        if (_ints[key] != value)
        {
            _ints[key] = value;
            Notify(key);
        }
    }

    public long GetLong(string key)
    {
        return _longs[key];
    }

    public void SetLong(string key, long value)
    {
        if (_longs[key] != value)
        {
            _longs[key] = value;
            Notify(key);
        }
    }

    public float GetFloat(string key)
    {
        return _floats[key];
    }

    public void SetFloat(string key, float value)
    {
        if (_floats[key] != value)
        {
            _floats[key] = value;
            Notify(key);
        }
    }

    public string GetString(string key)
    {
        return _strings[key];
    }

    public void SetString(string key, string value)
    {
        if (_strings[key] != value)
        {
            _strings[key] = value;
            Notify(key);
        }
    }

    public T GetEnum<T>(string key) where T : struct, IConvertible
    {
        return (T)_enums[key];
    }

    public void SetEnum<T>(string key, T value) where T : struct, IConvertible
    {
        if (!_enums[key].Equals(value))
        {
            _enums[key] = value;
            Notify(key);
        }
    }

    private void Notify(string key)
    {
        RoleEvent evt = null;
        switch (key)
        {
            case name:
                {
                    evt = new RoleEvent(RoleEvent.NameChange);
                    break;
                }
            case entityId:
                {
                    evt = new RoleEvent(RoleEvent.EntityIdChange);
                    break;
                }
        }
        if (evt != null)
        {
            DispatchEvent(evt);
        }
    }
}
