using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class npcEvent : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Color32 _normalColor = Color.red;
    [SerializeField] private Color32 _highlightColor = Color.blue;
    [SerializeField] private Color32 _activeColor = Color.green;

    private SpriteRenderer _renderer; // All renderers inherit from Renderer. 

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _renderer.color = _normalColor;
    }

    private void OnMouseEnter()
    {
        _renderer.color = _highlightColor;
    }

    private void OnMouseDown()
    {
        //shoot people
        _renderer.color = _activeColor;
    }

    private void OnMouseExit()
    {
        _renderer.color = _normalColor;
    }

    private void OnMouseUpAsButton()
    {
        _renderer.color = _highlightColor;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
