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

    private TextMeshPro _textDisplay;

    //position
    private float _startOffsetY = 3f;
    private float _endOffsetY = 5f;
    //timing
    private float _timer;
    private float _duration = 2f;
    private float _durationSolidText = 0.3f;
    private float _moveSpeed = 0.5f;

   

    // Start is called before the first frame update
    public void OnEnable()
    {
        _textDisplay = GetComponent<TextMeshPro>();
        _textDisplay.color = Color.WithAlpha(1f);
        _textDisplay.text = Text;
        _timer = 0;
        this.transform.position = new Vector3(transform.parent.position.x, transform.parent.position.x + _startOffsetY, transform.parent.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;
        transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward, Camera.main.transform.up);

        if (_timer > _durationSolidText) 
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, transform.position.y + _endOffsetY, transform.position.z), _moveSpeed * Time.deltaTime);

            var percentage = 1f - (_timer - _durationSolidText / _duration - _durationSolidText);
            _textDisplay.color = Color.WithAlpha(percentage);
        }

        if (_timer >= _duration)
            Destroy(this.gameObject);
    }
}
