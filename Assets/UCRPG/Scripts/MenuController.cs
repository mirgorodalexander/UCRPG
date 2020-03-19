using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.ProceduralImage;

public class MenuController : MonoBehaviour
{
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
    public TextMeshProUGUI CoinsValue;
    public TextMeshProUGUI CurrentLevel;
    public TextMeshProUGUI NextLevel;

    [Title("Controllers")]
    public LocationController LocationController;

    public ItemController ItemController;

    [Title("Menu element Prefab")]
    public GameObject Prefab;

    public GameObject Breakline;

    private List<GameObject> elements;

    private void UpdateUI()
    {
        ShowMenuButton.interactable = Player.Status != Player._Status.Fighting;
        CoinsValue.text = Coins.Value.ToString();
        CurrentLevel.text = Player.LVL.ToString();
        NextLevel.text = (Player.LVL+1).ToString();
    }

    [Title("Buttons")]
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
            var element = Instantiate(Prefab, Prefab.transform.position, Prefab.transform.rotation) as GameObject;
            Element Element = element.GetComponent<Element>();
            Element.gameObject.name = $"Weapon ({index})";
            Element.Icon.sprite = child.Icon;
            Element.Title.text = child.Title;
            Element.Description.text = child.Description;
            Element.Locked.transform.GetChild(0).transform.Find("Content").transform.Find("Text").GetComponent<TextMeshProUGUI>().text =
                child.LockedText.Replace("{Level}", child.Level.ToString());
            Element.Opened.transform.GetChild(0).transform.Find("Content").transform.Find("Text").GetComponent<TextMeshProUGUI>().text =
                child.OpenedText.Replace("{Price}", child.Price.ToString());
            Element.Owned.transform.GetChild(0).transform.Find("Content").transform.Find("Text").GetComponent<TextMeshProUGUI>().text =
                child.OwnedText;
            
            Button ButtonLocked = Element.Locked.transform.Find("Button").GetComponent<Button>();
            Button ButtonOpened = Element.Opened.transform.Find("Button").GetComponent<Button>();
            Button ButtonOwned = Element.Owned.transform.Find("Button").GetComponent<Button>();

            ButtonOpened.onClick.AddListener(() =>
            {
                if(Coins.Value >= child.Price){
                    Coins.Value -= child.Price;
                    child.Status = MenuElement.ElementSetupClass._Status.Owned;
                    this.Draw();
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

            Element.transform.SetParent(Weapons.transform);
            Element.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);

            if (index != WeaponsMenuElemets.Elements.Count - 1)
            {
                var breakline =
                    Instantiate(Breakline, Prefab.transform.position, Prefab.transform.rotation) as GameObject;
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
            var element = Instantiate(Prefab, Prefab.transform.position, Prefab.transform.rotation) as GameObject;
            Element Element = element.GetComponent<Element>();
            Element.gameObject.name = $"Item ({index})";
            Element.Icon.sprite = child.Icon;
            Element.Title.text = child.Title;
            Element.Description.text = child.Description;
            Element.Locked.transform.GetChild(0).transform.Find("Content").transform.Find("Text").GetComponent<TextMeshProUGUI>().text =
                child.LockedText.Replace("{Level}", child.Level.ToString());
            Element.Opened.transform.GetChild(0).transform.Find("Content").transform.Find("Text").GetComponent<TextMeshProUGUI>().text =
                child.OpenedText.Replace("{Price}", child.Price.ToString());
            Element.Owned.transform.GetChild(0).transform.Find("Content").transform.Find("Text").GetComponent<TextMeshProUGUI>().text =
                child.OwnedText;
            
            Button ButtonLocked = Element.Locked.transform.Find("Button").GetComponent<Button>();
            Button ButtonOpened = Element.Opened.transform.Find("Button").GetComponent<Button>();
            Button ButtonOwned = Element.Owned.transform.Find("Button").GetComponent<Button>();

            ButtonOpened.onClick.AddListener(() =>
            {
                if(Coins.Value >= child.Price){
                    Coins.Value -= child.Price;
                    if(child.Status == MenuElement.ElementSetupClass._Status.Rechargeable){
                        child.Status = MenuElement.ElementSetupClass._Status.Rechargeable;
                        this.Draw();
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
                    Instantiate(Breakline, Prefab.transform.position, Prefab.transform.rotation) as GameObject;
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
            var element = Instantiate(Prefab, Prefab.transform.position, Prefab.transform.rotation) as GameObject;
            Element Element = element.GetComponent<Element>();
            Element.gameObject.name = $"Location ({index})";
            Element.Icon.sprite = child.Icon;
            Element.Title.text = child.Title;
            Element.Description.text = child.Description;
            Element.Locked.transform.GetChild(0).transform.Find("Content").transform.Find("Text").GetComponent<TextMeshProUGUI>().text =
                child.LockedText.Replace("{Level}", child.Level.ToString());
            Element.Opened.transform.GetChild(0).transform.Find("Content").transform.Find("Text").GetComponent<TextMeshProUGUI>().text =
                child.OpenedText.Replace("{Price}", child.Price.ToString());
            Element.Owned.transform.GetChild(0).transform.Find("Content").transform.Find("Text").GetComponent<TextMeshProUGUI>().text =
                child.OwnedText;

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
                    this.Draw();
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
                    Instantiate(Breakline, Prefab.transform.position, Prefab.transform.rotation) as GameObject;
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
        this.Show();
    }

    void Update()
    {
        this.UpdateUI();
    }
}