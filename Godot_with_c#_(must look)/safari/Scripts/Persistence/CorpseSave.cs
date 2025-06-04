using System;

[Serializable]
public class CorpseSave
{
    public string AnimalType { get; set; }
    public float PosX { get; set; }
    public float PosY { get; set; }
    public long Id { get; set; }
    public int DeathCount { get; set; } // How much time passed since death (in frames)
    public bool HasChip { get; set; }
}
