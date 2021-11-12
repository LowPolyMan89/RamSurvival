using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : Item, IDamagable
{
    [SerializeField] private int count;
    [SerializeField] private float hitPoint;
    [SerializeField] private Item lootItem;

    private float  _maxHitPoint;

    public float HitPoint => hitPoint;
    public float MaxHitPoint => _maxHitPoint;

    protected override void Start()
    {
        _maxHitPoint = hitPoint;
    }

    public float GetHitPoint()
    {
        return hitPoint;
    }

    public override int GetCount()
    {
        return count;
    }

    public override string GetName()
    {
        return ItemId;
    }

    public override int GetRare()
    {
        return ItemRare;
    }

    public void TakeDamage(float value)
    {
        hitPoint -= value;

        if(HitPoint <= 0)
        {
            CreateLootItem();
        }
    }

    [ContextMenu("CreateLootItem")]
    public void CreateLootItem()
    {
        var item = Instantiate(lootItem);
        item.transform.position = transform.position;
        item.Count = count;
        Destroy(this.gameObject);
    }
}
