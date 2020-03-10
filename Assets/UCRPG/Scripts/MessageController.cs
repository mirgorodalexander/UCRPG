using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class MessageController : MonoBehaviour
{
    [Title("Configurations")]
    public GameObject ConsolePopupCanvas;
    public GameObject ExperiencePopupCanvas;
    public GameObject DamagePopupCanvas;

    [Title("Preferences")]
    public float ExperiencePopupLifetime;
    public float ExperiencePopupFadetime;
    public float ConsolePopupLifetime;
    public float ConsolePopupFadetime;
    [Title("FX")]
    public GameObject ConsolePopupPrefab;
    public GameObject PlayerExperiencePopupPrefab;
    public GameObject EnemyDamagePopupPrefab;
    public GameObject PlayerDamagePopupPrefab;
    
    
    [Button("Console Popup", ButtonSizes.Large), GUIColor(1, 1, 1)]
    public void ConsolePopup(string message)
    {
        GameObject prefab = ConsolePopupPrefab;
        GameObject Message = Instantiate(prefab, prefab.transform.position, prefab.transform.rotation) as GameObject;
        Message.transform.SetParent(ConsolePopupCanvas.transform);
        Message.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
        Message.transform.localScale = new Vector3(1f, 1f, 1f);
        Message.transform.SetSiblingIndex(0);
        Message.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = $"{message}";
        Message.name = $"Message";
        DOVirtual.DelayedCall(ConsolePopupLifetime, () =>
        {
            Message.GetComponent<CanvasGroup>().DOFade(0f, ConsolePopupFadetime).SetEase(Ease.Linear).OnComplete(() =>
            {
                Destroy(Message.gameObject);
            });
        });
    }
    
    [Button("Experience Popup", ButtonSizes.Large), GUIColor(1, 1, 1)]
    public void ExperiencePopup(int experience)
    {
        GameObject Experience = Instantiate(PlayerExperiencePopupPrefab, ExperiencePopupCanvas.transform.position, Quaternion.identity) as GameObject;
        Experience.transform.SetParent(ExperiencePopupCanvas.transform);
        Experience.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = $"+ {experience}";
        Experience.name = $"Experience + {experience}";
        Experience.transform.DOScale(new Vector3(0.7f, 0.7f, 0.7f), 0f).OnComplete(() =>
        {
            Experience.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), ExperiencePopupLifetime);
            Experience.transform.DOLocalMoveY(280, ExperiencePopupLifetime, false).OnComplete(() =>
            {
                Experience.gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().DOFade(0, ExperiencePopupFadetime);
                DOVirtual.DelayedCall(ExperiencePopupFadetime, () =>
                {
                    Experience.gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().DOFade(0, ExperiencePopupFadetime/2);
                    Experience.transform.DOScale(new Vector3(0.2f, 0.2f, 0.2f), ExperiencePopupFadetime/2).OnComplete(() =>
                        {
                            Destroy(Experience.gameObject);
                        });
                });
            });
        });
    }

    [Button("Enemy Damage Popup", ButtonSizes.Large), GUIColor(1, 1, 1)]
    public void EnemyDamagePopup(int damage)
    {
        GameObject Damage = Instantiate(EnemyDamagePopupPrefab, DamagePopupCanvas.transform.position, Quaternion.identity) as GameObject;
        Damage.transform.SetParent(DamagePopupCanvas.transform);
        Damage.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = $"{damage}";
        Damage.name = $"Damage - {damage}";
        Damage.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.3f);
        Damage.transform.DOLocalMove(new Vector3(160, 240, 0), 0.3f, false).OnComplete(() =>
        {
            Damage.gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().DOFade(0, 0.5f);
            Damage.transform.DOScale(new Vector3(0.2f, 0.2f, 0.2f), 0.3f);
            Damage.transform.DOLocalMove(new Vector3(340, -1000, 0), 1f, false).OnComplete(() =>
            {
                Destroy(Damage.gameObject);
            });
        });
    }

    [Button("Player Damage Popup", ButtonSizes.Large), GUIColor(1, 1, 1)]
    public void PlayerDamagePopup(int damage)
    {
        GameObject Damage = Instantiate(PlayerDamagePopupPrefab, DamagePopupCanvas.transform.position, Quaternion.identity) as GameObject;
        Damage.transform.SetParent(DamagePopupCanvas.transform);
        Damage.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = $"{damage}";
        Damage.name = $"Damage - {damage}";
        Damage.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.3f);
        Damage.transform.DOLocalMove(new Vector3(-160, 240, 0), 0.3f, false).OnComplete(() =>
        {
            Damage.gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().DOFade(0, 0.5f);
            Damage.transform.DOScale(new Vector3(0.2f, 0.2f, 0.2f), 0.3f);
            Damage.transform.DOLocalMove(new Vector3(-340, -1000, 0), 1f, false).OnComplete(() =>
            {
                Destroy(Damage.gameObject);
            });
        });
    }
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
