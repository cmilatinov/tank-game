using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class LoadingText : MonoBehaviour {

    [Header("Parameters")]
    [SerializeField] 
    private float loopTime = 3.0f;

    [SerializeField] 
    private int numDots = 3;
    
    [Header("References")]
    [SerializeField]
    private Text text;

    private float _lastTime;
    private int _count;
    private string _text;
    
    private void Reset() {
        text = GetComponent<Text>();
    }

    // Start is called before the first frame update
    void Start() {
        _text = text.text;
    }

    // Update is called once per frame
    void Update() {
        if (Time.time - _lastTime > loopTime / numDots) {
            _count = (_count + 1) % (numDots + 1);
            string textStr = _text;
            for (int i = 0; i < _count; i++)
                textStr += ".";
            text.text = textStr;
            _lastTime = Time.time;
        }
    }
    
}
