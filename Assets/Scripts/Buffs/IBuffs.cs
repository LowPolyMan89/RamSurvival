
using UnityEngine;

public interface IBuffs
{
   public float GetTime();
   public string GetId();
   public BuffModificator GetModificator();
   public BuffType GetType();
   public FunctionType GetFunctionType();
   public void SetTime(float time);
   public void Tick();
   public void StartBuff();
   public void EndBuff();
   public float GetValue();
   public ValueType GetValueType();
   public StackType GetStackType();
   public void CreateBuff(FunctionType functionType, BuffType buffType, BuffModificator buffModificator, ValueChangeType valueChangeType, ValueType valueType, StackType stackType, bool isStack, float buffTime, string buffId, float value, Sprite sprite, bool isHide);
   bool IsFinish { get; set; }
   bool IsStack { get; set; }
   bool IsHide { get; set; }
}

public enum FunctionType
{
   Add, Mull, Set
}
public enum BuffType
{
   Buff, Perk
}
public enum BuffModificator
{
   HitPoint, Mass, Armor, Speed, CraftSpeed, Damage, Exp, Food
}

public enum ValueChangeType
{
   OneTime, PerSecond
}

public enum ValueType
{
   Maximum, Current
}

public enum StackType
{
   Set, Add
}