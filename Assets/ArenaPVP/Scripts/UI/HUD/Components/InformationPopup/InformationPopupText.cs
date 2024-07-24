using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class InformationPopupText : MonoBehaviour
{
    public string Text;

    [SerializeField] TextMeshProUGUI _text;
    [SerializeField] float DisplayDuration = 3f;
    private Color _color;
    public float Timer { get; private set; }

    void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
        _text.text = Text;
        _color = _text.color;

    }

    void Update()
    {
        Timer += Time.deltaTime;

        var percentage = 1 - (Timer / DisplayDuration);
        _text.color = _color.WithAlpha(percentage);

        if (Timer >= DisplayDuration)
        {
            Destroy(this.gameObject);
        }
    }
}
