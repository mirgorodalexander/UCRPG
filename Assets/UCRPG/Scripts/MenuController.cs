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
    [Title("Events")]
    public GlobalEvent UpdateUiEvent;
    
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
    public CanvasGroup DieWindow;
    
    [Title("Elements Value")]
    public TextMeshProUGUI CoinsValue;
    public TextMeshProUGUI CurrentLevel;
    public TextMeshProUGUI CurrentLevelInGame;
    public TextMeshProUGUI NextLevel;
    
    [Title("Sliders")]
    public Slider PlayerExperience;
    public Slider PlayerHealth;

    [Title("Controllers")]
    public WeaponController WeaponController;
    public LocationController LocationController;

    public ItemController ItemController;

    [Title("Menu element Prefab")]
    public GameObject ItemElementPrefab;
    public GameObject WeaponElementPrefab;
    public GameObject LocationElementPrefab;
    public GameObject PowerIcon;

    public GameObject Breakline;

    private List<GameObject> elements;

    private void UpdateUI()
    {
        PlayerExperience.value = (1f / ExperienceDatabase.Items[Player.LVL]) * Player.EXP;
        ShowMenuButton.interactable = Player.Status != Player._Status.Fighting;
        CoinsValue.text = Coins.Value.ToString();
        CurrentLevel.text = Player.LVL.ToString();
        CurrentLevelInGame.text = Player.LVL.ToString();
        NextLevel.text = (Player.LVL+1).ToString();
        this.Draw();
    }

    [Title("Buttons")]
    [Button("ShowLevelUp", ButtonSizes.Large), GUIColor(1, 1, 1)]
    public void ShowLevelUp()
    {
        LevelUpWindow.gameObject.SetActive(true);
        ModalProvider modalProvider = LevelUpWindow.GetComponent<ModalProvider>();
        String description = modalProvider.Description.text.Replace("{LEVEL}", $"<incr f=6>{Player.LVL.ToString()}");
        modalProvider.Description.gameObject.GetComponent<TextAnimatorPlayer>().ShowText(description);
        DOVirtual.DelayedCall(0.1f, () =>
        {
        });
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
        // foreach (var child in WeaponDatabase.Items)
        // {
        //     var element = Instantiate(WeaponElementPrefab, ItemElementPrefab.transform.position, ItemElementPrefab.transform.rotation) as GameObject;
        //     Element Element = element.GetComponent<Element>();
        //     Element.gameObject.name = $"Weapon ({index})";
        //     if(Element.Prefab != null){
        //         var prefabIcon = Instantiate(child.Prefab, child.Prefab.transform.position, Quaternion.Euler(0, 90, 0)) as GameObject;
        //         prefabIcon.transform.SetParent(Element.Prefab.transform);
        //         prefabIcon.transform.localScale = new Vector3(200,200,200);
        //         prefabIcon.transform.position = new Vector3(0, 0, 0);
        //         prefabIcon.transform.localPosition = new Vector3(0, 0, 0);
        //     }
        //     if(Element.Title != null){
        //         Element.Title.text = child.Name;
        //     }
        //     if(Element.Description != null){
        //         Element.Description.text = child.Description;
        //     }
        //     Element.Locked.transform.GetChild(0).transform.Find("Content").transform.Find("Text").GetComponent<TextMeshProUGUI>().text =
        //         WeaponDatabase.LockedText.Replace("{Level}", child.Level.ToString());
        //     Element.Opened.transform.GetChild(0).transform.Find("Content").transform.Find("Text").GetComponent<TextMeshProUGUI>().text =
        //         WeaponDatabase.OpenedText.Replace("{Price}", child.Price.ToString());
        //     Element.Owned.transform.GetChild(0).transform.Find("Content").transform.Find("Text").GetComponent<TextMeshProUGUI>().text =
        //         WeaponDatabase.OwnedText;
        //     
        //     Button ButtonLocked = Element.Locked.transform.Find("Button").GetComponent<Button>();
        //     Button ButtonOpened = Element.Opened.transform.Find("Button").GetComponent<Button>();
        //     Button ButtonOwned = Element.Owned.transform.Find("Button").GetComponent<Button>();
        //
        //     ButtonOpened.onClick.AddListener(() =>
        //     {
        //         if(Coins.Value >= child.Price){
        //             Coins.Value -= child.Price;
        //             child.Status = WeaponDatabase.ItemSetupClass._Status.Owned;
        //             UpdateUiEvent.Publish();
        //         }
        //     });
        //     ButtonOwned.onClick.AddListener(() => WeaponController.Equip(child.ID));
        //
        //     Element.Locked.SetActive(false);
        //     Element.Opened.SetActive(false);
        //     Element.Owned.SetActive(false);
        //
        //     if (child.Status == WeaponDatabase.ItemSetupClass._Status.Locked)
        //     {
        //         Element.Locked.SetActive(true);
        //     }
        //
        //     if (child.Status == WeaponDatabase.ItemSetupClass._Status.Opened)
        //     {
        //         Element.Opened.SetActive(true);
        //     }
        //
        //     if (child.Status == WeaponDatabase.ItemSetupClass._Status.Owned)
        //     {
        //         Element.Owned.SetActive(true);
        //     }
        //
        //     Element.transform.SetParent(Weapons.transform);
        //     Element.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
        //
        //     if (index != WeaponsMenuElemets.Elements.Count - 1)
        //     {
        //         var breakline =
        //             Instantiate(Breakline, ItemElementPrefab.transform.position, ItemElementPrefab.transform.rotation) as GameObject;
        //         breakline.transform.SetParent(Weapons.transform);
        //         breakline.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
        //         elements.Add(breakline);
        //     }
        //
        //     elements.Add(element);
        //
        //     index++;
        // }
        
        
        //
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
            
            Element.LockedText.text =  WeaponsMenuElemets.LockedText.Replace("{Level}", child.Level.ToString());
            Element.OpenedText.text = WeaponsMenuElemets.OpenedText.Replace("{Price}", child.Price.ToString());
            Element.OwnedText.text = WeaponsMenuElemets.OwnedText;
            
            Button ButtonLocked = Element.Locked.transform.Find("Button").GetComponent<Button>();
            Button ButtonOpened = Element.Opened.transform.Find("Button").GetComponent<Button>();
            Button ButtonOwned = Element.Owned.transform.Find("Button").GetComponent<Button>();
        
            ButtonOpened.onClick.AddListener(() =>
            {
                if(Coins.Value >= child.Price){
                    Coins.Value -= child.Price;
                    child.Status = MenuElement.ElementSetupClass._Status.Owned;
                    UpdateUiEvent.Publish();
                }
            });
            ButtonOwned.onClick.AddListener(() => WeaponController.Equip(child.REFID));
        
            Element.Locked.SetActive(false);
            Element.Opened.SetActive(false);
            Element.Owned.SetActive(false);
        
            if (child.Status == MenuElement.ElementSetupClass._Status.Locked)
            {
                Element.Locked.SetActive(true);
            }
        
            if (child.Status == MenuElement.ElementSetupClass._Status.Opened)
            {
                Element.Opened.SetActive(true);
            }
        
            if (child.Status == MenuElement.ElementSetupClass._Status.Owned)
            {
                Element.Owned.SetActive(true);
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
        foreach (var child in ItemsMenuElemets.Elements)
        {
            var element = Instantiate(ItemElementPrefab, ItemElementPrefab.transform.position, ItemElementPrefab.transform.rotation) as GameObject;
            Element Element = element.GetComponent<Element>();
            Element.gameObject.name = $"Item ({index})";
            Element.Icon.sprite = child.Icon;
            Element.Title.text = child.Title;
            Element.Description.text = child.Description;

            if (Element.Description.text.Contains("{P}"))
            {
                Element.Description.text = Element.Description.text.Replace("{P}", "");
                if (Element.Description.text[0] == ' ')
                {
                    Element.Description.text = Element.Description.text.Substring(1);
                }
                var powerIcon = Instantiate(PowerIcon, PowerIcon.transform.position, PowerIcon.transform.rotation) as GameObject;
                powerIcon.transform.SetParent(Element.Description.gameObject.transform);
            }
            
            Element.LockedText.text =  ItemsMenuElemets.LockedText.Replace("{Level}", child.Level.ToString());
            Element.OpenedText.text = ItemsMenuElemets.OpenedText.Replace("{Price}", child.Price.ToString());
            Element.OwnedText.text = ItemsMenuElemets.OwnedText;
            
            Button ButtonLocked = Element.Locked.transform.Find("Button").GetComponent<Button>();
            Button ButtonOpened = Element.Opened.transform.Find("Button").GetComponent<Button>();
            Button ButtonOwned = Element.Owned.transform.Find("Button").GetComponent<Button>();

            ButtonOpened.onClick.AddListener(() =>
            {
                if(Coins.Value >= child.Price){
                    Coins.Value -= child.Price;
                    if(child.Status == MenuElement.ElementSetupClass._Status.Rechargeable){
                        child.Status = MenuElement.ElementSetupClass._Status.Rechargeable;
                        UpdateUiEvent.Publish();
                    }
                }
            });
            //ButtonOwned.onClick.AddListener(() => LocationController.Spawn(child.REFID));

            Element.Locked.SetActive(false);
            Element.Opened.SetActive(false);
            Element.Owned.SetActive(false);

            if (child.Status == MenuElement.ElementSetupClass._Status.Locked)
            {
                Element.Locked.SetActive(true);
            }

            if (child.Status == MenuElement.ElementSetupClass._Status.Opened)
            {
                Element.Opened.SetActive(true);
            }

            if (child.Status == MenuElement.ElementSetupClass._Status.Owned)
            {
                Element.Owned.SetActive(true);
            }

            if (child.Status == MenuElement.ElementSetupClass._Status.Rechargeable)
            {
                Element.Opened.SetActive(true);
            }

            Element.transform.SetParent(Items.transform);
            Element.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);

            if (index != ItemsMenuElemets.Elements.Count - 1)
            {
                var breakline =
                    Instantiate(Breakline, ItemElementPrefab.transform.position, ItemElementPrefab.transform.rotation) as GameObject;
                breakline.transform.SetParent(Items.transform);
                breakline.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
                elements.Add(breakline);
            }

            elements.Add(element);

            index++;
        }

        index = 0;
        foreach (var child in LocationsMenuElemets.Elements)
        {
            var element = Instantiate(LocationElementPrefab, ItemElementPrefab.transform.position, ItemElementPrefab.transform.rotation) as GameObject;
            Element Element = element.GetComponent<Element>();
            Element.gameObject.name = $"Location ({index})";
            Element.Icon.sprite = child.Icon;
            Element.Title.text = child.Title;
            Element.Description.text = child.Description;
            
            Element.LockedText.text =  LocationsMenuElemets.LockedText.Replace("{Level}", child.Level.ToString());
            Element.OpenedText.text = LocationsMenuElemets.OpenedText.Replace("{Price}", child.Price.ToString());
            Element.OwnedText.text = LocationsMenuElemets.OwnedText;

            Button ButtonLocked = Element.Locked.transform.Find("Button").GetComponent<Button>();
            Button ButtonOpened = Element.Opened.transform.Find("Button").GetComponent<Button>();
            Button ButtonOwned = Element.Owned.transform.Find("Button").GetComponent<Button>();

            ButtonOpened.onClick.AddListener(() =>
            {
                Debug.Log(child.Price);
                Debug.Log(Coins.Value);
                if(Coins.Value >= child.Price){
                    Coins.Value -= child.Price;
                    child.Status = MenuElement.ElementSetupClass._Status.Owned;
                    UpdateUiEvent.Publish();
                }
            });
            ButtonOwned.onClick.AddListener(() => LocationController.Spawn(child.REFID));

            Element.Locked.SetActive(false);
            Element.Opened.SetActive(false);
            Element.Owned.SetActive(false);

            if (child.Status == MenuElement.ElementSetupClass._Status.Locked)
            {
                Element.Locked.SetActive(true);
            }

            if (child.Status == MenuElement.ElementSetupClass._Status.Opened)
            {
                Element.Opened.SetActive(true);
            }

            if (child.Status == MenuElement.ElementSetupClass._Status.Owned)
            {
                Element.Owned.SetActive(true);
            }

            Element.transform.SetParent(Locations.transform);
            Element.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);

            if (index != LocationsMenuElemets.Elements.Count - 1)
            {
                var breakline =
                    Instantiate(Breakline, ItemElementPrefab.transform.position, ItemElementPrefab.transform.rotation) as GameObject;
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
        this.Show();
        UpdateUiEvent.Publish();
    }

    void Update()
    {
        if(UpdateUiEvent.isPublished()){
            Debug.Log("UI was updated.");
            this.UpdateUI();
        }
    }
}