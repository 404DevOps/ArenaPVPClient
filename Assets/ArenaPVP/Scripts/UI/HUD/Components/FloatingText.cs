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
    //public Transform StickToObject;

    private TextMeshPro _textDisplay;

    //position
    private float _startOffsetY = 100f;
    private float _endOffsetY = 200f;
    //timing
    public float _timer { get; private set; }
    private float _duration = 2f;
    private float _durationSolidText = 0.3f;
    private float _moveSpeed = 0.1f;
    private float _currentOffset;

    // Start is called before the first frame update
    public void OnEnable()
    {
        _textDisplay = GetComponent<TextMeshPro>();
        _textDisplay.color = Color.WithAlpha(1f);
        _textDisplay.text = Text;
        _timer = 0;
        //this.transform.position = new Vector3(transform.position.x, transform.position.x + _startOffsetY, transform.position.z);
        _currentOffset = _startOffsetY;
    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;
        _currentOffset = Mathf.Lerp(_currentOffset, _endOffsetY, _moveSpeed * Time.deltaTime);

        if (_timer > _durationSolidText) 
        {
            var percentage = 1f - (_timer - _durationSolidText / _duration - _durationSolidText);
            _textDisplay.color = Color.WithAlpha(percentage);
        }

        if (_timer >= _duration)
            Destroy(this.gameObject);

        this.transform.position = new Vector3(transform.position.x, transform.position.y + _currentOffset, transform.position.z);
    }
}
