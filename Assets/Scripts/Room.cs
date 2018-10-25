using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;

public class Room : MonoBehaviour
{   
    public Texture2D tex;
    public int MinSize = 60;
    public int MaxSize = 100;
    public Color SpriteColor;

    private Sprite mySprite;
    private SpriteRenderer sr;

    [HideInInspector]
    public float Width;

    [HideInInspector]
    public float Height;

    void Awake()
    {
        sr = gameObject.AddComponent<SpriteRenderer>() as SpriteRenderer;
        sr.color = SpriteColor;  
    }

    void Start()
    {
       Width  = Utility.RoundM(Random.Range(MinSize, MaxSize));
       Height = Utility.RoundM(Random.Range(MinSize, MaxSize));
       mySprite     = Sprite.Create(tex, new Rect(0, 0, Width, Height), new Vector2(1f, 1f), 100);
    }

    void OnGUI()
    {
        sr.sprite = mySprite;
    }
}
