using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.iOS;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI healthText;
    [SerializeField]
    private TextMeshProUGUI maxHealthText;
    [SerializeField]
    private TextMeshProUGUI timerText;
    [SerializeField]
    private Image experienceBar;
    private UITime timer;
    private Player player;

    public GameObject ShopPanel;
    public GameObject cardHolder;
    public GameObject WinPanel;
    public event Action<PlayerPerks> ChoosenPerk;
    /*
     * A helper class that defines the time setting operations.
     */
    class UITime {

        public UITime(int seconds, int minutes, int hours)
        {

            Seconds = seconds;
            Minutes = minutes;
            Hours = hours;
        }
        public int Seconds { get; private set; }
        public int Minutes { get; private set; }
        public int Hours { get; private set; }

        public void increment() {
            Seconds++;
            if (Seconds == 60) {
                Seconds = 0;
                Minutes++;
            }
            if (Minutes == 60)
            {
                Minutes = 0;
                Hours++;
            }
        }

        public void clear() {
            Hours = 0;
            Minutes = 0;
            Seconds = 0;

        }
        public override string ToString()
        {

            if (Hours == 0)
            {
                return string.Format("{0:0}:{1:00}", Minutes, Seconds);
            }
            return string.Format("{0:0}:{1:00}:{2:00}", Hours, Minutes, Seconds);
        }

    }

    void Awake()
    {
        EntityManager.Instance.ShowRemaining += ShowLevelUpPanel;
        ChoosenPerk += EntityManager.Instance.AddPerk;
    }

    void Start()
    {
        player = EntityManager.Instance.GetPlayer();
        player.Died += BackToMenu;

        timer = new UITime(0, 0, 0);
        startCounter();
    }

    void Update()
    {
        CheckForUpdates();
    }

    private void CheckForUpdates()
    {
        if (player.Health != System.Int32.Parse(healthText.text))
        {
            healthText.text = player.Health.ToString();
        }
        if (EntityManager.Instance.PlayerHealth != System.Int32.Parse(maxHealthText.text))
        {
            maxHealthText.text = (100 * EntityManager.Instance.PlayerHealth).ToString();
        }
        if (player.xp != experienceBar.fillAmount)
        {
            experienceBar.fillAmount = player.xp / player.xpToLevelUp;
        }
    }

    void startCounter() {
        StartCoroutine(countSeconds());

    }
    void stopCounter() {
        StopCoroutine(countSeconds());
    }

    IEnumerator countSeconds() {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            timer.increment();
            timerText.text = timer.ToString();
        }
    }

    void BackToMenu() {
        StartCoroutine(SwitchToMenu());
    
    }
    IEnumerator SwitchToMenu()
    {
        yield return new WaitForSecondsRealtime(2.5f);
        SceneManager.LoadScene(0);
    }
    void ShowLevelUpPanel(List<PlayerPerks> remaining)
    {
        ShopPanel.SetActive(true);
        CardManager cardManager = cardHolder.GetComponent<CardManager>();
        DetermineWinCondition(remaining, cardManager);
    }

    private void DetermineWinCondition(List<PlayerPerks> remaining, CardManager cardManager)
    {
        if (!remaining.Any<PlayerPerks>())
        {
            cardManager.placeRandomPerks(remaining);
            PlayerWon();
        }
        else cardManager.placeRandomPerks(remaining);
    }

    internal void SendPerk(PlayerPerks perk)
    {
        ShopPanel.SetActive(false);
        ChoosenPerk.Invoke(perk);
    }

    void PlayerWon() {
        WinPanel.SetActive(true);
    }
}
