using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : IBuffs
{
    private FunctionType _functionType;
    private BuffType _buffType;
    private BuffModificator _buffModificator;
    private string _buffId;
    private float _buffTime;
    private float _value;
    public bool IsFinish { get; set; }

    public float GetValue()
    {
        return _value;
    }

    public void CreateBuff(FunctionType functionType, BuffType buffType, BuffModificator buffModificator, float buffTime,
        string buffId, float value)
    {
        _functionType = functionType;
        _buffId = buffId;
        _buffModificator = buffModificator;
        _buffType = buffType;
        _buffTime = buffTime;
        _value = value;
    }

    public float GetTime()
    {
        return _buffTime;
    }

    public string GetId()
    {
        return _buffId;
    }

    public BuffModificator GetModificator()
    {
        return _buffModificator;
    }

    public BuffType GetType()
    {
        return _buffType;
    }

    public FunctionType GetFunctionType()
    {
        return _functionType;
    }

    public void SetTime(float time)
    {
        _buffTime = time;
    }

    public void Tick()
    {
        _buffTime -= 1f;
        if (_buffTime <= 0)
        {
            EndBuff();
        }
    }

    public void StartBuff()
    {
        throw new System.NotImplementedException();
    }

    public void EndBuff()
    {
        IsFinish = true;
    }
}
