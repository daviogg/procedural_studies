using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility{

    const int size = 2;

    public static float RoundM(float n)
    {
        return Mathf.Floor(((n + size - 1) / size)) * size;
    }
}
