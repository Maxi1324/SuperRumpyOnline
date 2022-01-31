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
            Debug.Log(b.key+":jasd");
            ButtonsD.Add(b.key,BC1);
        });
    }

    private void Update()
    {
        foreach(KeyValuePair<string, ButtonCool> kvp in ButtonsD)
        {
            kvp.Value.Update();
        }

        Debug.Log($"Button: {ButtonsD["Jump"].Button} ButtonDown: {ButtonsD["Jump"].ButtonDown} ButtonUp: {ButtonsD["Jump"].ButtonUp}");
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
        EventTrigger trigger = null;
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback.AddListener((data) =>{
            Down();
        });

        EventTrigger.Entry entry1 = new EventTrigger.Entry();
        entry1.eventID = EventTriggerType.PointerDown;
        entry1.callback.AddListener((data) => {
            Up();
        });
        trigger.triggers.Add(entry);
    }

    public void Up()
    {
        ButtonUp = true;
        Button = true;
    }

    public void Down()
    {
        ButtonDown = true;
        Button = false;
    }

    public void Update()
    {
        ButtonDown = false;
        ButtonUp = false;
    }
}
