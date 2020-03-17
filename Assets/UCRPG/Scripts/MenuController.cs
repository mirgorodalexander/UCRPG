using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.ProceduralImage;

public class MenuController : MonoBehaviour
{
    [Title("Configurations")]
    public GameObject Weapons;
    public MenuElement WeaponsMenuElemets;
    public GameObject Items;
    public MenuElement ItemsMenuElemets;
    public GameObject Locations;
    public MenuElement LocationsMenuElemets;

    [Title("Controllers")]
    public LocationController LocationController;
    public ItemController ItemController;

    [Title("Menu element Prefab")]
    public GameObject Prefab;
    public GameObject Breakline;
    void Start()
    {
        int index = 0;
        foreach (var child in WeaponsMenuElemets.Elements)
        {
            var element = Instantiate(Prefab, Prefab.transform.position, Prefab.transform.rotation) as GameObject;
            Element Element = element.GetComponent<Element>();
            Element.gameObject.name = $"Weapon ({index})";
            Element.Icon.sprite = child.Icon;
            Element.Title.text = child.Title;
            Element.Description.text = child.Description;
            Element.Locked.transform.GetChild(0).transform.Find("Text").GetComponent<TextMeshProUGUI>().text = child.LockedText.Replace("{Level}", child.Level.ToString());
            Element.Opened.transform.GetChild(0).transform.Find("Text").GetComponent<TextMeshProUGUI>().text = child.OpenedText.Replace("{Price}", child.Price.ToString());
            Element.Owned.transform.GetChild(0).transform.Find("Text").GetComponent<TextMeshProUGUI>().text = child.OwnedText;
            
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
            Element.transform.parent = Weapons.transform;
            Element.gameObject.transform.localScale = new Vector3(1f,1f,1f);
            
            var breakline = Instantiate(Breakline, Prefab.transform.position, Prefab.transform.rotation) as GameObject;
            if(index != WeaponsMenuElemets.Elements.Count-1){
                breakline.transform.parent = Weapons.transform;
                breakline.gameObject.transform.localScale = new Vector3(1f,1f,1f);
            }
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
            Element.Locked.transform.GetChild(0).transform.Find("Text").GetComponent<TextMeshProUGUI>().text = child.LockedText.Replace("{Level}", child.Level.ToString());
            Element.Opened.transform.GetChild(0).transform.Find("Text").GetComponent<TextMeshProUGUI>().text = child.OpenedText.Replace("{Price}", child.Price.ToString());
            Element.Owned.transform.GetChild(0).transform.Find("Text").GetComponent<TextMeshProUGUI>().text = child.OwnedText;
            
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

            Element.transform.parent = Items.transform;
            Element.gameObject.transform.localScale = new Vector3(1f,1f,1f);
            
            var breakline = Instantiate(Breakline, Prefab.transform.position, Prefab.transform.rotation) as GameObject;
            if(index != ItemsMenuElemets.Elements.Count-1){
                breakline.transform.parent = Items.transform;
                breakline.gameObject.transform.localScale = new Vector3(1f,1f,1f);
            }
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
            Element.Locked.transform.GetChild(0).transform.Find("Text").GetComponent<TextMeshProUGUI>().text = child.LockedText.Replace("{Level}", child.Level.ToString());
            Element.Opened.transform.GetChild(0).transform.Find("Text").GetComponent<TextMeshProUGUI>().text = child.OpenedText.Replace("{Price}", child.Price.ToString());
            Element.Owned.transform.GetChild(0).transform.Find("Text").GetComponent<TextMeshProUGUI>().text = child.OwnedText;
            
            Button ButtonLocked = Element.Locked.transform.Find("Button").GetComponent<Button>();
            Button ButtonOpened = Element.Opened.transform.Find("Button").GetComponent<Button>();
            Button ButtonOwned = Element.Owned.transform.Find("Button").GetComponent<Button>();

            var localIndex = index;
            ButtonOwned.onClick.AddListener(() => LocationController.Spawn(localIndex));
            
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
            Element.transform.parent = Locations.transform;
            Element.gameObject.transform.localScale = new Vector3(1f,1f,1f);
            
            var breakline = Instantiate(Breakline, Prefab.transform.position, Prefab.transform.rotation) as GameObject;
            if(index != LocationsMenuElemets.Elements.Count-1){
                breakline.transform.parent = Locations.transform;
                breakline.gameObject.transform.localScale = new Vector3(1f,1f,1f);
            }
            index++;
        }
    }

    void Update()
    {
        
    }
}
