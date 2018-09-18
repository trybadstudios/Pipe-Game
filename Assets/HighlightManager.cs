using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightManager : MonoBehaviour {
    
    public void SetHighlight(Color highlightColor)
    {
        foreach (Transform child in transform)
        {
            child.GetComponent<MeshRenderer>().material.color = highlightColor;
        }
    }

    public void Start()
    {
    }
}
