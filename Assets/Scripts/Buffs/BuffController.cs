using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffController
{
    private List<IBuffs> HitPointBuffs = new List<IBuffs>();
    private List<IBuffs> FoodBuffs = new List<IBuffs>();

    public void Init()
    {
        EventManager.Instance.OnTimerSecondAction += UpdateController;
    }

    public void CreateNewBuff(string buffId)
    {
        Buff buff = new Buff();
        BuffsDataSO data = DatabaseManager.Instance.BuffsDatabase.GetBuff(buffId);
        buff.CreateBuff(data.FunctionType,data.BuffType,data.BuffModificator,data.BuffTime, data.BuffId, data.Value);
        
        switch (buff.GetModificator())
        {
            case BuffModificator.HitPoint:
                HitPointBuffs.Add(buff);
                break;
            case BuffModificator.Mass:
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
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
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
                toremove.Add(hitpoints);
            }
        }

        foreach (var remHp in toremove)
        {
            HitPointBuffs.Remove(remHp);
        }
    }
    
    public float GetMaxHitPoint(float maxHp)
    {
        var val = maxHp;

        if (HitPointBuffs.Count <= 0) return val;
        
        foreach (var buff in HitPointBuffs)
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

    public void Destroy()
    {
        EventManager.Instance.OnTimerSecondAction -= UpdateController;
    }


}
