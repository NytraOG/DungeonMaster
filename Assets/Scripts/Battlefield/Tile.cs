using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Entities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class Tile : MonoBehaviour
{
    [SerializeField] public  BaseUnit   unit;
    [SerializeField] private GameObject highlight;
    private                  bool       IsOccupied => unit != null;
    
    private void OnMouseEnter()
    {
        if(!IsOccupied)
            highlight.SetActive(true);
    }

    private void OnMouseExit()
    {
        highlight.SetActive(false);
    }
}
