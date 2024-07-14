using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Logger = Assets.Scripts.Helpers.Logger;

public class FloatingText : MonoBehaviour
{
    public Color Color;
    public string Text;
    public Transform StickToObject;

    private TextMeshProUGUI _textDisplay;

    //position
    private float _startOffsetY = 100f;
    private float _endOffsetY = 200f;
    //timing
    private float _timer;
    private float _duration = 2f;
    private float _durationSolidText = 0.3f;
    private float _moveSpeed = 0.5f;

    private float _currentOffset;

   

    // Start is called before the first frame update
    public void OnEnable()
    {
        _textDisplay = GetComponent<TextMeshProUGUI>();
        _textDisplay.color = Color.WithAlpha(1f);
        _textDisplay.text = Text;
        _timer = 0;
        var worldToScreen = Camera.main.WorldToScreenPoint(StickToObject.position);
        this.transform.position = new Vector3(worldToScreen.x, worldToScreen.x + _startOffsetY, worldToScreen.z);
        _currentOffset = _startOffsetY;
    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;
        //transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward, Camera.main.transform.up);
        var worldToScreen = Camera.main.WorldToScreenPoint(StickToObject.position);

        if (_timer > _durationSolidText) 
        {
            _currentOffset = Mathf.Lerp(_currentOffset, _endOffsetY, _moveSpeed * Time.deltaTime);
            var percentage = 1f - (_timer - _durationSolidText / _duration - _durationSolidText);
            _textDisplay.color = Color.WithAlpha(percentage);
        }

        if (_timer >= _duration)
            Destroy(this.gameObject);

        this.transform.position = new Vector3(worldToScreen.x, worldToScreen.y + _currentOffset, worldToScreen.z);
    }
}
