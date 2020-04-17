using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using DG.Tweening;
using Febucci.UI;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.UI.ProceduralImage;

public class MenuController : MonoBehaviour
{
    [Title("Database")]
    public WeaponDatabase WeaponDatabase;
    public ExperienceDatabase ExperienceDatabase;
    
    [Title("Configurations")]
    public Player Player;

    public GlobalVariableInt Coins;

    public GameObject Weapons;
    public MenuElement WeaponsMenuElemets;
    public GameObject Items;
    public MenuElement ItemsMenuElemets;
    public GameObject Locations;
    public MenuElement LocationsMenuElemets;

    [Title("Elements UI")]
    public CanvasGroup Menu;
    public CanvasGroup TapToPlayButton;
    public CanvasGroup ShowMenuButton;
    public CanvasGroup LevelUpWindow;
    public CanvasGroup DefeatedWindow;
    
    [Title("Elements Value")]
    public TextMeshProUGUI CoinsValue;
    public TextMeshProUGUI CoinsValueInGame;
    public TextMeshProUGUI CurrentLevel;
    public TextMeshProUGUI CurrentLevelInGame;
    public TextMeshProUGUI NextLevel;
    
    [Title("Sliders")]
    public Slider PlayerExperience;
    public Slider PlayerHealth;

    [Title("Controllers")]
    public LocationController LocationController;
    public WeaponController WeaponController;
    public RenderController RenderController;
    public HealthController HealthController;

    public ItemController ItemController;

    [Title("Menu element Prefab")]
    public GameObject ItemElementPrefab;
    public GameObject WeaponElementPrefab;
    public GameObject LocationElementPrefab;
    public GameObject PowerIcon;

    public GameObject Breakline;

    public List<GameObject> elements;

    public void UpdateUI()
    {
        PlayerExperience.value = (1f / ExperienceDatabase.Items[Player.LVL]) * Player.EXP;
        PlayerHealth.value = (1f / HealthController.playerHealthDefault) * Player.HP;
        HealthController.PlayerHealth.gameObject.transform.Find("Viewport").gameObject.transform.Find("Value").GetComponent<TextMeshProUGUI>().text =
            $"{Player.HP} / {HealthController.playerHealthDefault}";
        
        ShowMenuButton.interactable = Player.Status != Player._Status.Fighting;
        
        CoinsValue.text = Coins.Value.ToString();
        CoinsValueInGame.text = Coins.Value.ToString();
        
        CurrentLevel.text = Player.LVL.ToString();
        CurrentLevelInGame.text = Player.LVL.ToString();
        
        NextLevel.text = (Player.LVL+1).ToString();
        
        this.Draw();
    }

    public void UpdateInGameUI()
    {
        PlayerExperience.value = (1f / ExperienceDatabase.Items[Player.LVL]) * Player.EXP;
        PlayerHealth.value = (1f / HealthController.playerHealthDefault) * Player.HP;
        HealthController.PlayerHealth.gameObject.transform.Find("Viewport").gameObject.transform.Find("Value").GetComponent<TextMeshProUGUI>().text =
            $"{Player.HP} / {HealthController.playerHealthDefault}";
        
        ShowMenuButton.interactable = Player.Status != Player._Status.Fighting;
        
        CoinsValue.text = Coins.Value.ToString();
        CoinsValueInGame.text = Coins.Value.ToString();
        
        CurrentLevel.text = Player.LVL.ToString();
        CurrentLevelInGame.text = Player.LVL.ToString();
        
        NextLevel.text = (Player.LVL+1).ToString();
    }

    [Title("Buttons")]
    [Button("ShowDefeated", ButtonSizes.Large), GUIColor(1, 1, 1)]
    public void ShowDefeated(int explost)
    {
        Player.Status = Player._Status.Menu;
        if (!RenderController.ThirdPersonView)
        {
            WeaponController.WeaponParentFirstPerson.SetActive(false);
        }
        DefeatedWindow.gameObject.SetActive(true);
        ModalProvider modalProvider = DefeatedWindow.GetComponent<ModalProvider>();
        String description = modalProvider.Description.text.Replace("{EXPLOST}", $"<incr f=6>{explost.ToString()}</incr>");
        modalProvider.Description.gameObject.GetComponent<TextAnimatorPlayer>().ShowText(description);
    }
    [Title("Buttons")]
    [Button("ShowLevelUp", ButtonSizes.Large), GUIColor(1, 1, 1)]
    public void ShowLevelUp()
    {
        LevelUpWindow.gameObject.SetActive(true);
        Player.Status = Player._Status.Menu;
        ModalProvider modalProvider = LevelUpWindow.GetComponent<ModalProvider>();
        String description = modalProvider.Description.text.Replace("{LEVEL}", $"<incr f=6>{Player.LVL.ToString()}");
        modalProvider.Description.gameObject.GetComponent<TextAnimatorPlayer>().ShowText(description);
    }
    [Button("Hide", ButtonSizes.Large), GUIColor(1, 1, 1)]
    public void Hide()
    {
        if (Player.Status != Player._Status.Fighting)
        {
            Player.Status = Player._Status.Waiting;

            TapToPlayButton.gameObject.SetActive(false);

            this.Draw();
            
            Menu.gameObject.SetActive(false);
        }
    }

    [Button("Show", ButtonSizes.Large), GUIColor(1, 1, 1)]
    public void Show()
    {
        if (Player.Status != Player._Status.Fighting)
        {
            Player.Status = Player._Status.Menu;

            TapToPlayButton.gameObject.SetActive(true);

            this.Draw();
            
            Menu.gameObject.SetActive(true);
        }
    }

    [Button("Draw", ButtonSizes.Large), GUIColor(1, 1, 1)]
    public void Draw()
    {
        if (elements != null)
        {
            if (elements.Count > 0)
            {
                foreach (var child in elements)
                {
                    if (child != null)
                    {
                        Destroy(child.gameObject);
                    }
                }
            }
        }

        elements = new List<GameObject> { };
        
        int index = 0;
        foreach (var child in WeaponsMenuElemets.Elements)
        {
            var element = Instantiate(WeaponElementPrefab, ItemElementPrefab.transform.position, ItemElementPrefab.transform.rotation) as GameObject;
            Element Element = element.GetComponent<Element>();
            Element.gameObject.name = $"Weapon ({index})";
            if(Element.Icon != null){
                Element.Icon.sprite = child.Icon;
            }
            if(Element.Title != null){
                Element.Title.text = child.Title;
            }
            if(Element.Description != null){
                Element.Description.text = child.Description;
            }

            if (Element.Description.text.Contains("{P}"))
            {
                Element.Description.text = Element.Description.text.Replace("{P}", "\U0000f6b2");
            }
            if (Element.Description.text.Contains("{S}"))
            {
                Element.Description.text = Element.Description.text.Replace("{S}", "\U0000f0e7");
            }
            
            Element.Locked.text =  WeaponsMenuElemets.LockedText.Replace("{Level}", child.Level.ToString());
            Element.Opened.text = WeaponsMenuElemets.OpenedText.Replace("{Price}", child.Price.ToString());
            Element.Owned.text = WeaponsMenuElemets.OwnedText;
            
            if (child.Status == MenuElement.ElementSetupClass._Status.Locked)
            {
                Element.Locked.gameObject.transform.parent.gameObject.SetActive(true);
                if (Player.LVL >= child.Level)
                {
                    child.Status = MenuElement.ElementSetupClass._Status.Opened;
                    Element.Locked.gameObject.transform.parent.gameObject.SetActive(false);
                }
            }
            if (child.Status == MenuElement.ElementSetupClass._Status.Opened)
            {
                Element.Opened.gameObject.transform.parent.gameObject.SetActive(true);
                
                Element.Button.onClick.AddListener(() =>
                {
                    if(Coins.Value >= child.Price){
                        Coins.Value -= child.Price;
                        child.Status = MenuElement.ElementSetupClass._Status.Owned;
                        WeaponController.Equip(child.REFID);
                        UpdateUI();
                    }
                });
            }
            if (child.Status == MenuElement.ElementSetupClass._Status.Owned)
            {
                Element.Owned.gameObject.transform.parent.gameObject.SetActive(true);
                Element.Button.onClick.AddListener(() => WeaponController.Equip(child.REFID));
            }
        
            Element.transform.SetParent(Weapons.transform);
            Element.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
        
            if (index != WeaponsMenuElemets.Elements.Count - 1)
            {
                var breakline =
                    Instantiate(Breakline, ItemElementPrefab.transform.position, ItemElementPrefab.transform.rotation) as GameObject;
                breakline.transform.SetParent(Weapons.transform);
                breakline.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
                elements.Add(breakline);
            }
        
            elements.Add(element);
        
            index++;
        }
        
        index = 0;
        
        foreach (var child in LocationsMenuElemets.Elements)
        {
            var element = Instantiate(LocationElementPrefab, LocationElementPrefab.transform.position, LocationElementPrefab.transform.rotation) as GameObject;
            Element Element = element.GetComponent<Element>();
            Element.gameObject.name = $"Location ({index})";
            if(Element.Icon != null){
                Element.Icon.sprite = child.Icon;
            }
            if(Element.Title != null){
                Element.Title.text = child.Title;
            }
            if(Element.Description != null){
                Element.Description.text = child.Description;
            }

            if (Element.Description.text.Contains("{P}"))
            {
                Element.Description.text = Element.Description.text.Replace("{P}", "\U0000f6b2");
            }
            if (Element.Description.text.Contains("{S}"))
            {
                Element.Description.text = Element.Description.text.Replace("{S}", "\U0000f0e7");
            }
            
            Element.Locked.text =  LocationsMenuElemets.LockedText.Replace("{Level}", child.Level.ToString());
            Element.Opened.text = LocationsMenuElemets.OpenedText.Replace("{Price}", child.Price.ToString());
            Element.Owned.text = LocationsMenuElemets.OwnedText;

            if (child.Status == MenuElement.ElementSetupClass._Status.Locked)
            {
                Element.Locked.gameObject.transform.parent.gameObject.SetActive(true);
                if (Player.LVL >= child.Level)
                {
                    child.Status = MenuElement.ElementSetupClass._Status.Opened;
                    Element.Locked.gameObject.transform.parent.gameObject.SetActive(false);
                }
            }
            if (child.Status == MenuElement.ElementSetupClass._Status.Opened)
            {
                Element.Opened.gameObject.transform.parent.gameObject.SetActive(true);
                // if (Player.LVL < child.Level)
                // {
                //     child.Status = MenuElement.ElementSetupClass._Status.Locked;
                //     Element.Opened.gameObject.SetActive(false);
                //     Element.Locked.gameObject.SetActive(true);
                // }
                
                Element.Button.onClick.AddListener(() =>
                {
                    if(Coins.Value >= child.Price){
                        Coins.Value -= child.Price;
                        child.Status = MenuElement.ElementSetupClass._Status.Owned;
                        LocationController.Spawn(child.REFID);
                        UpdateUI();
                    }
                });
            }
            if (child.Status == MenuElement.ElementSetupClass._Status.Owned)
            {
                Element.Owned.gameObject.transform.parent.gameObject.SetActive(true);
                Element.Button.onClick.AddListener(() => LocationController.Spawn(child.REFID));
            }
        
            Element.transform.SetParent(Locations.transform);
            Element.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
        
            if (index != LocationsMenuElemets.Elements.Count - 1)
            {
                var breakline =
                    Instantiate(Breakline, LocationElementPrefab.transform.position, LocationElementPrefab.transform.rotation) as GameObject;
                breakline.transform.SetParent(Locations.transform);
                breakline.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
                elements.Add(breakline);
            }
        
            elements.Add(element);
        
            index++;
        }
    }

    void Start()
    {
        LevelUpWindow.gameObject.SetActive(false);
        DefeatedWindow.gameObject.SetActive(false);
        UpdateInGameUI();
        this.Hide();
    }

    void Update()
    {
    }
}