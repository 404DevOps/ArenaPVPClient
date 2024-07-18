using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LogEntry : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _text;

    public Color TextColor;
    public string Text;

    private void OnEnable()
    {
        _text.text = Text;
        _text.color = TextColor;
    }
}
