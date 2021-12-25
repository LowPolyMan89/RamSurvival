
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
   public void CreateBuff(FunctionType functionType, BuffType buffType, BuffModificator buffModificator, float buffTime, string buffId, float value);
   bool IsFinish { get; set; }
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
