using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Buff : IBuffs
{
    private FunctionType _functionType;
    private BuffType _buffType;
    private BuffModificator _buffModificator;
    private ValueChangeType _valueChangeType;
    private ValueType _valueType;
    private StackType _stackType;
    private string _buffId;
    private float _buffTime;
    private float _value;
    private Sprite _sprite;

    public bool IsFinish { get; set; }
    public bool IsStack { get; set; }
    public bool IsHide { get; set; }

    public float GetValue()
    {
        return _value;
    }

    public StackType GetStackType()
    {
        return _stackType;
    }

    public void CreateBuff(FunctionType functionType, BuffType buffType, BuffModificator buffModificator, ValueChangeType valueChangeType, ValueType valueType, StackType stackType, bool isStack, float buffTime,
        string buffId, float value, Sprite sprite, bool isHide)
    {
        _functionType = functionType;
        _buffId = buffId;
        _buffModificator = buffModificator;
        _buffType = buffType;
        _buffTime = buffTime;
        _value = value;
        _valueChangeType = valueChangeType;
        _sprite = sprite;
        _valueType = valueType;
        _stackType = stackType;
        IsStack = isStack;
        IsHide = isHide;
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

    public ValueType GetValueType()
    {
        return _valueType;
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
        else
        {
            switch (_buffModificator)
            {
                case BuffModificator.HitPoint:
                {
                    if (_valueChangeType == ValueChangeType.PerSecond)
                    {
                        EventManager.Instance.OnPlayerGetHP(_value);
                    }
                    break;
                }
                case BuffModificator.Mass:
                    if (_valueChangeType == ValueChangeType.PerSecond)
                    {
                       
                    }
                    break;
                    break;
                case BuffModificator.Armor:
                    break;
                case BuffModificator.Speed:
                    break;
                case BuffModificator.CraftSpeed:
                    break;
                case BuffModificator.Damage:
                    break;
                case BuffModificator.Exp:
                    break;
                case BuffModificator.Food:  
                {
                    if (_valueChangeType == ValueChangeType.PerSecond)
                    {
                        EventManager.Instance.OnPlayerGetFood(_value);
                    }
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
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
