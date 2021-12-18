using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : Entity, IDamagable
{
    [SerializeField] private int count;
    [SerializeField] private float hitPoint;
    [SerializeField] private Item lootItem;

    private float  _maxHitPoint;

    public float HitPoint => hitPoint;
    public float MaxHitPoint => _maxHitPoint;

    private void Start()
    {
        _maxHitPoint = hitPoint;
    }

    public float GetHitPoint()
    {
        return hitPoint;
    }

    public  int GetCount()
    {
        return count;
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
