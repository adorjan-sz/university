using Godot;
using System;
using System.Collections.Generic;

[Serializable]
public class LightSave
{
    public List<int> LightPositionsX { get; set; } = new();
    public List<int> LightPositionsY { get; set; } = new();
    public double CurrentTime { get; set; }
    public string CurrentDayState { get; set; }
    public bool LightsOn { get; set; }

    public LightSave()
    {
    }

    public LightSave(LightManager lightManager)
    {
        var temp = lightManager.GetLightPos();
        foreach (var light in temp)
        {
            LightPositionsX.Add((int)light.X);
            LightPositionsY.Add((int)light.Y);
        }
        CurrentTime = lightManager.GetCurrentTime;
        CurrentDayState = lightManager.GetDayLightState.ToString();
        LightsOn = lightManager.GetLightsON;
    }
}
