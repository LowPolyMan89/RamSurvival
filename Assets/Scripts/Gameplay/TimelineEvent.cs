using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TimelineEvent : MonoBehaviour
{
    public string EventID;
    public DateTime EventEndDate;
    public float Seconds;
    public bool isActive = true;
    public bool isComplited = false;
    public EventAtionType ActionType;

    private void Start()
    {
        gameObject.name = EventID + " / " + EventEndDate.ToString("yyyy-MM-dd HH:mm:ssZ");
    }

    public virtual void CompliteEvent()
    {
        isComplited = true;
        print("Event " + EventID + " Complited");
    }

    public void Drop()
    {
        Destroy(this.gameObject, 0.1f);
    }

    [ContextMenu("EventAction")]
    public void EventAction()
    {
        switch(ActionType)
        {
            case EventAtionType.AddResources:
                AddResourceAction();
                break;
            case EventAtionType.UpgrageBuilding:
                break;
            default:
                break;
        }

        Drop();
    }


    private void AddResourceAction()
    {
        
    }

}

public enum EventAtionType
{
    AddResources = 0, UpgrageBuilding
}