using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class PrefDisplay : MonoBehaviour
{
    public string key;
    public string textToAdd;
    public TextMeshProUGUI text;

    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        if(PlayerPrefs.HasKey(key))
            text.text = textToAdd + PlayerPrefs.GetString(key);
    }
}
