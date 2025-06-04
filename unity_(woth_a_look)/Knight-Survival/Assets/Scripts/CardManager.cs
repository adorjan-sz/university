using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class CardManager : MonoBehaviour
{

    public List<CardData> cardData;
    public List<GameObject> cards;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
 
    public void placeRandomPerks(List<PlayerPerks> perks) {
        int i;
        //Debug.Log("perkek száma" + perks.Count);
        foreach (PlayerPerks p in perks) {
            Debug.Log(p);
        }
        //Debug.Log("Kártyák száma" + cardData.Count);
        for (i = 0; i < 3 && perks.Any<PlayerPerks>(); ++i)
        {
            SelectARandomCard(perks, i);
        }
        while (i < cards.Count) {
            Debug.Log("Lefutok");
            cards[i].SetActive(false);
            i++;
        }
    }

    private void SelectARandomCard(List<PlayerPerks> perks, int i)
    {
        int idx = Random.Range(0, perks.Count);
        PlayerPerks choosen = perks[idx];
        Debug.Log("Sorsolt" + i + " . " + choosen);
        perks.RemoveAt(idx);
        foreach (CardData data in cardData)
        {
            if (data.perk == choosen)
            {
                CardDisplay display = cards[i].GetComponent<CardDisplay>();
                display.SwitchCardData(data);
                cards[i].SetActive(true);
            }
        }
    }
}
