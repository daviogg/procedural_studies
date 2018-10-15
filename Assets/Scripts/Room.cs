using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;

public class Room : MonoBehaviour
{

    public Texture2D tex;
    private Sprite mySprite;
    private SpriteRenderer sr;

    void Awake()
    {
        sr = gameObject.AddComponent<SpriteRenderer>() as SpriteRenderer;
        sr.color = new Color(1, 1, 1, 1);
    }

    void Start()
    {
        mySprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, 70, 50), new Vector2(0.5f, 0.5f));
    }

    void OnGUI()
    {
        sr.sprite = mySprite;
    }
}
