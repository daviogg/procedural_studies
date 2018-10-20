using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;

public class Room : MonoBehaviour
{   
    public Texture2D tex;
    public int MinWidth = 60;
    public int MaxWidth = 100;
    public Color SpriteColor;
    private Sprite mySprite;
    private SpriteRenderer sr;

 

    void Awake()
    {
        sr = gameObject.AddComponent<SpriteRenderer>() as SpriteRenderer;
        sr.color = SpriteColor;
    }

    void Start()
    {
        mySprite = Sprite.Create(tex, new Rect(0, 0,Random.Range(MinWidth, MaxWidth), Random.Range(MinWidth, MaxWidth)), new Vector2(0.5f, 0.5f));
    }

    void OnGUI()
    {
        sr.sprite = mySprite;
    }
}
