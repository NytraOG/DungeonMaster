using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Entities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tile : MonoBehaviour
{
    public                   BaseUnit   Unit;
    [SerializeField] 
    private GameObject highlight;

    void OnMouseUp()
    {
        Debug.Log("Hello: " + $"{Unit.gameObject.name}");
    }

    private void OnMouseEnter()
    {
        highlight.SetActive(true);
    }

    private void OnMouseExit()
    {
        highlight.SetActive(false);
    }
}
