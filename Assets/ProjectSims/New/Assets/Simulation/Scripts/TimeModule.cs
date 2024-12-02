using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeModule 
{
    private const int SecondsToMiliSeconds = 1000; 
    private const int MinutesToSeconds = 60; 
    private const int HoursToMinutes = 60;   
    private const int DayToHours = 24;       
    private const int DayToSecond = 86400;   
    
    private long _miliSeconds;
    public long TotalSeconds => _miliSeconds / SecondsToMiliSeconds;
    public long TotalMinutes => TotalSeconds / MinutesToSeconds;
    public long TotalHours => TotalMinutes / HoursToMinutes;
    public long TotalDays => TotalHours / DayToHours;
    
    public long SecondsNow => TotalSeconds - TotalMinutes * MinutesToSeconds;
    public long MinutesNow => TotalMinutes - TotalHours * HoursToMinutes;
    public long HoursNow => TotalHours - TotalDays * DayToHours;
    public long TotalSecondsOneDay => HoursNow * HoursToMinutes * MinutesToSeconds + MinutesNow * MinutesToSeconds + SecondsNow;
    public string Meridiem
    {
        get
        {
            if (HoursNow > 12)
                return "PM";
            else
                return "AM";
        }
    }
    public float DayProgression => (float)TotalSecondsOneDay / (float)DayToSecond;

    public int Speed;
    
    public TimeModule(int miliSeconds)
    {
        _miliSeconds = miliSeconds;
    }
    
    public void Update(float dt) //dt in mili sec
    {
        int idt = (int)(dt * 1000);
        _miliSeconds += idt * Speed;
    }
    
    public new string ToString()
    {
        return $"Day: {TotalDays},{HoursNow.ToString("00")}:{MinutesNow.ToString("00")}:{SecondsNow.ToString("00")}{Meridiem}";
    }
}
