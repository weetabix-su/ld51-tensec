using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TMPPlatformSpecificText : MonoBehaviour
{
    [Header("Text Parameters")]
    public string textPC;
    public string textTouch;

    TextMeshProUGUI textDisplay;

    private void Awake()
    {
        textDisplay = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
#if UNITY_ANDROID || UNITY_IOS
        textDisplay.text = textTouch;
#else
        textDisplay.text = textPC;
#endif
    }
}
