﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UpdateValue : MonoBehaviour {

    public int volume = 100;
    public int volumeSFX = 100;
    public TMPro.TMP_Text text;
    public Vector2 res;
    public bool fullScreen = true;
    public bool isRes = false;
    public TMPro.TMP_Text textFullScreen;
    public int health = 400;
    public int worldSize = 13;
    [SerializeField]
    public Vector2[] resolutions;

    void Start()
    {
        volume = 100;
        volumeSFX = 100;
        health = 400;
        worldSize = 13;
        res = new Vector2(Screen.width, Screen.height);
        fullScreen = Screen.fullScreen;
        if (text.text == "400")
        {
            text.text = SyncData.health.ToString();
        }
        if (isRes)
        {
            text.text = res.x.ToString() + "X" + res.y.ToString();
            textFullScreen.text = fullScreen.ToString();
        }
    }

	public void VolumeDown()
    {
        volume -= 10;
        volume = Mathf.Clamp(volume, 0, 100);
        text.text = volume.ToString();
        EventSystem.current.SetSelectedGameObject(null);
    }
    public void VolumeUp()
    {
        volume += 10;
        volume = Mathf.Clamp(volume, 0, 100);
        text.text = volume.ToString();
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void VolumeDownSFX()
    {
        volumeSFX -= 10;
        volumeSFX = Mathf.Clamp(volumeSFX, 0, 100);
        text.text = volumeSFX.ToString();
        EventSystem.current.SetSelectedGameObject(null);
    }
    public void VolumeUpSFX()
    {
        volumeSFX += 10;
        volumeSFX = Mathf.Clamp(volumeSFX, 0, 100);
        text.text = volumeSFX.ToString();
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void ResUp()
    {
        for (int i = 0; i < resolutions.Length; i++)
        { 
            if (Screen.width == resolutions[i].x && Screen.height == resolutions[i].y && i <= resolutions.Length - 1)
            {
                res = new Vector2(resolutions[Mathf.Clamp(i + 1, 6, resolutions.Length)].x, resolutions[Mathf.Clamp(i + 1, 0, resolutions.Length)].y);
                Screen.SetResolution((int)res.x, (int)res.y, fullScreen);
                //text.text = res.x.ToString() + "X" + res.y.ToString();
                text.text = Screen.width + "X" + Screen.height;
                StartCoroutine("ResUpdate");
            }
        }
        text.text = Screen.width + "X" + Screen.height;
    }
    public void ResDown()
    {
        for (int i = 0; i < resolutions.Length; i++)
        {
            if (Screen.width == resolutions[i].x && Screen.height == resolutions[i].y && i >= 7)
            {
                res = new Vector2(resolutions[Mathf.Clamp(i - 1, 6, resolutions.Length)].x, resolutions[Mathf.Clamp(i - 1, 0, resolutions.Length)].y);
                Screen.SetResolution((int)res.x, (int)res.y, fullScreen);
                //text.text = res.x.ToString() + "X" + res.y.ToString();
                text.text = Screen.width + "X" + Screen.height;
                StartCoroutine("ResUpdate");
            }
        }
        text.text = Screen.width + "X" + Screen.height;
    }
    public void SwitchFullScreen()
    {
        fullScreen = !fullScreen;
        Screen.fullScreen = fullScreen;
        textFullScreen.text = fullScreen.ToString();
    }

    public void HealthDown()
    {
        health -= 100;
        health = Mathf.Clamp(health, 100, 1000);
        text.text = health.ToString();
        SyncData.health = health;
        EventSystem.current.SetSelectedGameObject(null);
    }
    public void HealthUp()
    {
        health += 100;
        health = Mathf.Clamp(health, 100, 1000);
        text.text = health.ToString();
        SyncData.health = health;
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void WorldDown()
    {
        worldSize -= 1;
        worldSize = Mathf.Clamp(worldSize, 2, 200);
        text.text = worldSize.ToString();
        SyncData.worldSize = worldSize;
        EventSystem.current.SetSelectedGameObject(null);
    }
    public void WorldUp()
    {
        worldSize += 1;
        worldSize = Mathf.Clamp(worldSize, 2, 200);
        text.text = worldSize.ToString();
        SyncData.worldSize = worldSize;
        EventSystem.current.SetSelectedGameObject(null);
    }

    IEnumerator ResUpdate()
    {
        yield return new WaitForSeconds(0.2f);
        text.text = Screen.width + "X" + Screen.height;
    }
}
