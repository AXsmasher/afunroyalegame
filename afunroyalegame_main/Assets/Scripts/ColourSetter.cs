﻿using UnityEngine;
using Mirror;

public class ColourSetter : NetworkBehaviour
{
    public Color m_NewColor;
    public Material grapple;
    //These are the values that the Color Sliders return
    float m_Red, m_Blue, m_Green;

    void Start()
    {
        m_NewColor = new Color(
          Random.Range(0f, 1f),
          Random.Range(0f, 1f),
          Random.Range(0f, 1f)
        );
    }
}