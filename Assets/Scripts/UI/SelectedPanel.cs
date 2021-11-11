using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedPanel : UIElement
{
    public Text Name;
    public Text Count;
    public Image Progress;
    public Image Icon;
    public float fillAmont;
    public Entity entity;

    private Resource _resource;
    private Item _item;


    public void Init()
    {
        if (entity is Resource)
        {
           _resource = entity.GetComponent<Resource>();
        }
        if (entity is Item)
        {
           _item = entity.GetComponent<Item>();
        }
    }

    private void OnDisable()
    {
        _resource = null;
        _item = null;
        entity = null;
    }

    protected override void Update()
    {
        if(entity)
        {
            if(entity is Resource)
            {
                Progress.fillAmount = _resource.HitPoint / _resource.MaxHitPoint;
            }
        }
    }


}
