using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class Timeline : MonoBehaviour
{
    [SerializeField] private List<TimelineEvent> timelineEvents = new List<TimelineEvent>();
    [SerializeField] private List<TimelineEvent> removeevents = new List<TimelineEvent>();
    [SerializeField] private GameObject timelineEventPrefab;

    public List<TimelineEvent> TimelineEvents { get => timelineEvents; set => timelineEvents = value; }

    public void AddNewTimelineEvent(string ID, double time, EventAtionType eventAtionType)
    {
        GameObject eventObj = Instantiate(timelineEventPrefab);
        eventObj.transform.SetParent(gameObject.transform);
        TimelineEvent ev = eventObj.GetComponent<TimelineEvent>();
        ev.EventID = ID;
        ev.Seconds = (float)time;
        ev.EventEndDate = DateTime.UtcNow.AddSeconds(time);
        ev.ActionType = eventAtionType;
        TimelineEvents.Add(ev);
    }

    public void AddOldTimelineEvent(string ID, float seconds, string time, EventAtionType eventAtionType)
    {
        GameObject eventObj = Instantiate(timelineEventPrefab);
        eventObj.transform.SetParent(gameObject.transform);
        TimelineEvent ev = eventObj.GetComponent<TimelineEvent>();
        ev.EventID = ID;
        ev.Seconds = (float)seconds;
        ev.ActionType = eventAtionType;
        string inp = time;
        string format = "yyyy-MM-dd HH:mm:ssZ";
        DateTime dt;

        if (!DateTime.TryParseExact(inp, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
        {
            Console.WriteLine("Nope!");
        }
 
        ev.EventEndDate = dt.ToUniversalTime();

        TimelineEvents.Add(ev);
    }
    
    

    [ContextMenu("AddTestTimeline")]
    public void AddTestTimeline()
    {
        AddNewTimelineEvent(DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ssZ"), 20, EventAtionType.AddResources);
    }

    private void Start()
    {
        StartCoroutine(UpdateTimeline());
    }

    public void DropTimeline()
    {
        foreach(var v in timelineEvents)
        {
            v.Drop();
        }

        timelineEvents.Clear();
    }

    private IEnumerator UpdateTimeline()
    {
        timelineEvents = Support.RemoveNull(timelineEvents);
        
        yield return new WaitForSeconds(1f);

        if (TimelineEvents.Count > 0)
        {
            foreach(var v in TimelineEvents)
            {
                EventChecker(v);
            }
        }

        foreach (var rem in removeevents)
        {
            TimelineEvents.Remove(rem);
        }
        removeevents.Clear();

        StartCoroutine(UpdateTimeline());
    }

    private void EventChecker(TimelineEvent timelineEvent)
    {
        if(!timelineEvent)
        {
            return;
        }

        if(timelineEvent.isActive)
        {
            if (timelineEvent.EventEndDate <= DateTime.UtcNow)
            {
                timelineEvent.CompliteEvent();
                removeevents.Add(timelineEvent);
            }
        }
    }
}