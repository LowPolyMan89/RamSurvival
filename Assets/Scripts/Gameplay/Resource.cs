using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using Random = UnityEngine.Random;

public class Resource : Entity, IDamagable
{
    [SerializeField] private Vector2 countMinMax;
    [SerializeField] private float hitPoint;
    [SerializeField] private Item lootItem;
    [SerializeField] private int Exp;
    [SerializeField] private bool isCollect;
    [SerializeField] private float RestoreTime;
    [SerializeField] private GameObject brokenObj;
    [SerializeField] private GameObject normalObj;
    private float currRestoreTime;

    private float  _maxHitPoint;

    public float HitPoint => hitPoint;
    public float MaxHitPoint => _maxHitPoint;

    public bool IsCollect => isCollect;

    private void Start()
    {
        _maxHitPoint = hitPoint;
        EventManager.Instance.OnTimerSecondAction += TimerUpdate;
    }

    private void TimerUpdate()
    {
        if (IsCollect == true)
        {
            currRestoreTime += 1;
        }

        if (currRestoreTime >= RestoreTime)
        {
            isCollect = false;
            Restore();
        }
        
    }

    private void Restore()
    {
        string m = LayerMask.LayerToName(7);
        gameObject.layer = LayerMask.NameToLayer(m);
        brokenObj.SetActive(false);
        normalObj.SetActive(true);
        hitPoint = _maxHitPoint;
        isCollect = false;
        currRestoreTime = 0;
    }

    public float GetHitPoint()
    {
        return hitPoint;
    }

    public  Vector2 GetCount()
    {
        return countMinMax;
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
        item.Count = Random.Range((int)countMinMax.x, (int)countMinMax.y + 1);
        EventManager.Instance.OnPlayerGetXP(Exp);
        isCollect = true;
        
        string m = LayerMask.LayerToName(2);
        gameObject.layer = LayerMask.NameToLayer(m);
        brokenObj.SetActive(true);
        normalObj.SetActive(false);
    }
}
