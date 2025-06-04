using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    public CardData card;
    public TextMeshProUGUI title;
    public TextMeshProUGUI description;
    [SerializeField]
    public Image cardArt;
    public GameObject ui;
    private Vector3 initialScale;


    void Start()
    {
        title.text = card.title;
        description.text = card.description;
        cardArt.sprite = card.perkImg;
    }


    public void Select() {
        Debug.Log("Katt");
        UIController controller = ui.GetComponent<UIController>();
        controller.SendPerk(card.perk);
    }

    public void SwitchCardData(CardData data) {
        card = data;
        title.text = data.title;
        description.text = data.description;
        cardArt.sprite = data.perkImg;
        //Debug.Log("A kártya perkje most már " + card.perk);
    }
}
