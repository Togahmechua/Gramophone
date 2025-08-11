using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public static class ButtonExtensions
{
    public static void OnPointerDown(this Button button, Action action)
    {
        EventTrigger trigger = button.gameObject.GetComponent<EventTrigger>() ?? button.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerDown };
        entry.callback.AddListener((data) => { action(); });
        trigger.triggers.Add(entry);
    }

    public static void OnPointerUp(this Button button, Action action)
    {
        EventTrigger trigger = button.gameObject.GetComponent<EventTrigger>() ?? button.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry { eventID = EventTriggerType.PointerUp };
        entry.callback.AddListener((data) => { action(); });
        trigger.triggers.Add(entry);
    }
}