﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class groundForceTriggerLLLAAI : MonoBehaviour
{
    public GameObject local;
    void OnCollisionEnter(Collision collsionInfo)
    {
        if (collsionInfo.collider.gameObject.layer == 12)
        {
            local.GetComponent<groundForce>().hitLLLA = true;
        }
        if (collsionInfo.collider.gameObject.layer == 16)
        {
            local.GetComponent<groundForce>().hitLLLAObject = true;
        }
    }
    void OnCollisionExit(Collision collsionInfo)
    {
        if (collsionInfo.collider.gameObject.layer == 12)
        {
            local.GetComponent<groundForce>().hitLLLA = false;
        }
        if (collsionInfo.collider.gameObject.layer == 16)
        {
            local.GetComponent<groundForce>().hitLLLAObject = false;
        }
    }
}
