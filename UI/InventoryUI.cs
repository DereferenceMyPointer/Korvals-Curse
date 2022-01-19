using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Character controller;
    public GameObject itemBox;
    private List<Image> itemImages;
    public GameObject pickupNotif;
    public float notifDuration;
    public float notifFadeInTime;
    public TextMeshProUGUI label;
    public TextMeshProUGUI descriptionBox;

    private void Start()
    {
        itemImages = new List<Image>();
        foreach(Transform t in itemBox.transform)
        {
            itemImages.Add(t.GetComponent<Image>());
        }
        DrawItems();
        controller.OnItemPickup += OnItemPickup;
    }

    public void DrawItems()
    {
        List<Item> items = controller.inventory.GetAllItems();
        for(int i = 0; i < items.Count; i++)
        {
            itemImages[i].sprite = items[i].icon;
            itemImages[i].gameObject.SetActive(true);
            itemImages[i].GetComponentInChildren<TextMeshProUGUI>().text = "" + items[i].count;
        }
    }

    public void OnItemPickup(Item i)
    {
        Image image = pickupNotif.transform.GetChild(0).GetComponent<Image>();
        image.sprite = i.icon;
        TextMeshProUGUI text = pickupNotif.GetComponentInChildren<TextMeshProUGUI>();
        text.text = "Received a " + i.name;
        StartCoroutine(FadeInItemNotif(image, pickupNotif.GetComponent<Image>(), text));
        DrawItems();
    }

    private IEnumerator FadeInItemNotif(Image i, Image obj, TextMeshProUGUI text)
    {
        pickupNotif.SetActive(true);
        float t = 0;
        Color c = Color.white;
        c.a = 0;
        i.color = c;
        text.color = c;
        obj.color = c;
        while (t < notifFadeInTime)
        {
            yield return new WaitForEndOfFrame();
            t += Time.deltaTime;
            c.a += Time.deltaTime / notifFadeInTime;
            i.color = c;
            text.color = c;
            obj.color = c;
        }
        yield return new WaitForSeconds(notifDuration);
        pickupNotif.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        label.alpha = 0.5f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        label.alpha = 0;
    }
}
