using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MobileControls : MonoBehaviour
{
    public DynamicJoystick Joystick;

    public List<BC> Buttons = new List<BC>();

    public Dictionary<string, ButtonCool> ButtonsD = new Dictionary<string, ButtonCool>();

    private void Start()
    {
        Buttons.ForEach(b =>
        {
            ButtonCool BC1 = new ButtonCool(b.ETrigger);
            ButtonsD.Add(b.key,BC1);
        });
    }

    private void LateUpdate()
    {
        //Debug.Log($"Button: {ButtonsD["Jump"].Button} ButtonDown: {ButtonsD["Jump"].ButtonDown} ButtonUp: {ButtonsD["Jump"].ButtonUp}");

        foreach (KeyValuePair<string, ButtonCool> kvp in ButtonsD)
        {
            kvp.Value.Update();
        }
    }
}
[Serializable]
public struct BC
{
    public string key;
    public EventTrigger ETrigger;
}

[Serializable]
public class ButtonCool
{
    public bool ButtonUp { get; set; }    
    public bool ButtonDown { get; set; }    
    public bool Button { get; set; }

    public EventTrigger ET { get; set; }

    public ButtonCool(EventTrigger ET)
    {
        this.ET = ET;
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerUp;
        entry.callback.AddListener(Up);
        ET.triggers.Add(entry);

        EventTrigger.Entry entry1 = new EventTrigger.Entry();
        entry1.eventID = EventTriggerType.PointerDown;
        entry1.callback.AddListener(Down);
        ET.triggers.Add(entry1);
    }

    public void Up(BaseEventData data)
    {
        ButtonUp = true;
        Button = false;
    }

    public void Down(BaseEventData data)
    {
        ButtonDown = true;
        Button = true;
    }

    public void Update()
    {
        ButtonDown = false;
        ButtonUp = false;
    }
}
