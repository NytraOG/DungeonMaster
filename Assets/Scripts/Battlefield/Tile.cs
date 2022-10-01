using System;
using System.Collections;
using System.Collections.Generic;
using Entities;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public BaseUnit Unit;

    private void OnMouseUp()
    {
        Debug.Log("Hello: " + $"{Unit.gameObject.name}");
    }
}
