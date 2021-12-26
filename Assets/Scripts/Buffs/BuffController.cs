using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffController
{
    private List<IBuffs> HitPointBuffs = new List<IBuffs>();
    private List<IBuffs> FoodBuffs = new List<IBuffs>();
    private List<IBuffs> MassBuffs = new List<IBuffs>();
    private List<IBuffs> EXPBuffs = new List<IBuffs>();

    [SerializeField] private List<string> workingBuffs = new List<string>();
    
    public void Init()
    {
        EventManager.Instance.OnTimerSecondAction += UpdateController;
    }

    public Buff CreateNewBuff(string buffId)
    {
        foreach (var b in workingBuffs)
        {
            if (b == buffId)
            {
                Debug.LogError($@"Buff: {buffId} is already in Player!");
                return null;
            }
        }
        
        Buff buff = new Buff();
        BuffsDataSO data = DatabaseManager.Instance.BuffsDatabase.GetBuff(buffId);
        buff.CreateBuff(data.FunctionType,data.BuffType,data.BuffModificator,data.ValueChangeType, data.ValueType, data.StackType, data.IsStack, data.BuffTime, data.BuffId, data.Value, data.BuffSprite, data.IsHide);
        
        switch (buff.GetModificator())
        {
            case BuffModificator.HitPoint:
                HitPointBuffs.Add(buff);
                break;
            case BuffModificator.Mass:
                MassBuffs.Add(buff);
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
                EXPBuffs.Add(buff);
                break;
            case BuffModificator.Food:
                FoodBuffs.Add(buff);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        workingBuffs.Add(buffId);
        return buff;
    }
    
    public void RemoveBuff(string buffId)
    {
        Buff removed = null;

        foreach (var hp in HitPointBuffs)
        {
            if (hp.GetId() == buffId)
            {
                removed = (Buff)hp;
                break;
            }
        }
        HitPointBuffs.Remove(removed);
        
        foreach (var hp in FoodBuffs)
        {
            if (hp.GetId() == buffId)
            {
                removed = (Buff)hp;
                break;
            }
        }

        FoodBuffs.Remove(removed);
        
        foreach (var ms in MassBuffs)
        {
            if (ms.GetId() == buffId)
            {
                removed = (Buff)ms;
                break;
            }
        }

        MassBuffs.Remove(removed);
        
        foreach (var exp in EXPBuffs)
        {
            if (exp.GetId() == buffId)
            {
                removed = (Buff)exp;
                break;
            }
        }

        EXPBuffs.Remove(removed);
    }

    private void UpdateController()
    {
        List<IBuffs> toremove = new List<IBuffs>();
        
        foreach (var hitpoints in HitPointBuffs)
        {
            if(hitpoints.GetType() == BuffType.Perk)
                continue;
            hitpoints.Tick();

            if (hitpoints.IsFinish)
            {
                workingBuffs.RemoveAll(item => item == hitpoints.GetId());
                toremove.Add(hitpoints);
            }
        }
        
        foreach (var foods in FoodBuffs)
        {
            if(foods.GetType() == BuffType.Perk)
                continue;
            foods.Tick();

            if (foods.IsFinish)
            {
                workingBuffs.RemoveAll(item => item == foods.GetId());
                toremove.Add(foods);
            }
        }
        
        foreach (var mass in MassBuffs)
        {
            if(mass.GetType() == BuffType.Perk)
                continue;
            mass.Tick();

            if (mass.IsFinish)
            {
                workingBuffs.RemoveAll(item => item == mass.GetId());
                toremove.Add(mass);
            }
        }
        
        foreach (var exp in EXPBuffs)
        {
            if(exp.GetType() == BuffType.Perk)
                continue;
            exp.Tick();

            if (exp.IsFinish)
            {
                workingBuffs.RemoveAll(item => item == exp.GetId());
                toremove.Add(exp);
            }
        }

        foreach (var remHp in toremove)
        {
            HitPointBuffs.Remove(remHp);
        }
        foreach (var remfood in toremove)
        {
            FoodBuffs.Remove(remfood);
        }
        foreach (var mass in toremove)
        {
            MassBuffs.Remove(mass);
        }
        foreach (var exp in toremove)
        {
            MassBuffs.Remove(exp);
        }
    }

    public float GetBuffedExpValue(float expValue)
    {
        var val = expValue;

        if (EXPBuffs.Count <= 0) return val;
        
        foreach (var buff in EXPBuffs)
        {
            switch (buff.GetFunctionType())
            {
                case FunctionType.Add:
                    val += buff.GetValue();
                    break;
                case FunctionType.Mull:
                    val *= buff.GetValue();
                    break;
                case FunctionType.Set:
                    val = buff.GetValue();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        return val;
    }
    
    public float GetMaxHitPoint(float maxHp)
    {
        var val = maxHp;

        if (HitPointBuffs.Count <= 0) return val;
        
        foreach (var buff in HitPointBuffs)
        {
            if(buff.GetValueType() == ValueType.Current) continue;
            
            switch (buff.GetFunctionType())
            {
                case FunctionType.Add:
                    val += buff.GetValue();
                    break;
                case FunctionType.Mull:
                    val *= buff.GetValue();
                    break;
                case FunctionType.Set:
                    val = buff.GetValue();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        return val;
    }
    
    public float GetMaxMass(float maxMass)
    {
        var val = maxMass;

        if (MassBuffs.Count <= 0) return val;
        
        foreach (var buff in MassBuffs)
        {
            if(buff.GetValueType() == ValueType.Current) continue;
            
            switch (buff.GetFunctionType())
            {
                case FunctionType.Add:
                    val += buff.GetValue();
                    break;
                case FunctionType.Mull:
                    val *= buff.GetValue();
                    break;
                case FunctionType.Set:
                    val = buff.GetValue();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        return val;
    }
    
    public float GetMaxFood(float maxFood)
    {
        var val = maxFood;

        if (FoodBuffs.Count <= 0) return val;
        
        foreach (var buff in FoodBuffs)
        {
            if(buff.GetValueType() == ValueType.Current) continue;
            
            switch (buff.GetFunctionType())
            {
                case FunctionType.Add:
                    val += buff.GetValue();
                    break;
                case FunctionType.Mull:
                    val *= buff.GetValue();
                    break;
                case FunctionType.Set:
                    val = buff.GetValue();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        return val;
    }

    public void Destroy()
    {
        EventManager.Instance.OnTimerSecondAction -= UpdateController;
    }


}
