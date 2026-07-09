using System;
using UnityEngine;

public class ClockDelay
{
    public float TimeStamp { get; set; } //t1
    public float CurrentTime { set; get; }

    public float WaitTime { set; get; }
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public ClockDelay(float startTime)
    {
        //for milliseconds
        TimeStamp = startTime;
    }


    public bool HasExceededTime()
    {
        return CurrentTime - TimeStamp > WaitTime;
    }

    public void UpdateTick(float tick)
    {
        CurrentTime += tick;
        //  Debug.Log($"Bool: {HasExceededTime()}, boolValue{CurrentTime - TimeStamp}, WaitTime: {WaitTime} , time: {CurrentTime}, timeStamp: {TimeStamp}");
    }

    public void UpdateTimeStamp()
    {
        TimeStamp = CurrentTime;
    }
}