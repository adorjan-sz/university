using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "Scriptable Objects/CardData")]
public class CardData : ScriptableObject
{
    public string title;
    public string description;
    public Sprite perkImg;
    public PlayerPerks perk; 
    
}
