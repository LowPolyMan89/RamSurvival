using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    
    protected virtual void Start()
    {
        
    }

    
    protected virtual void Update()
    {
        
    }

    public virtual string GetName()
    {
        return "Is not init Name";
    }
    public virtual int GetRare()
    {
        return 0;
    }

    public virtual int GetCount()
    {
        return 0;
    }

    public virtual Sprite GetSprite()
    {
        return null;
    }
}
