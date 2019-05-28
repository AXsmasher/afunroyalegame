﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SyncName : NetworkBehaviour {

    public TMPro.TMP_Text text;

    [SyncVar]
    public string name;

    public GameObject parent;

    [SyncVar]
    public GameObject serverParent;

    public int wins;
    public HealthAI healthAI;

	// Use this for initialization
	IEnumerator Start () {
        if (GameObject.Find("LoadingPlayer"))
        {
            parent = GameObject.Find("LoadingPlayer");
            if (PlayerPrefs.HasKey("wins"))
            {
                wins = PlayerPrefs.GetInt("wins");
            }
            if (PlayerPrefs.HasKey("name"))
            {
                SyncData.name = PlayerPrefs.GetString("name");
                name = SyncData.name + " *" + wins.ToString() + "*";

                text.text = name;
            }
        }
        else
        {
            yield return new WaitForSeconds(1f);
            if (!hasAuthority)
            {
                parent = serverParent;
                healthAI = parent.GetComponent<HealthAI>();
                text.text = name;
            }
            text.color = parent.GetComponent<ColourSetterAI>().m_NewColor;
        }
    }

    // Update is called once per frame
    void LateUpdate() {
        if (parent)
        {
            if (healthAI)
            {
                if (healthAI.deaded)
                {
                    Destroy(gameObject);
                }
            }
            transform.position = parent.transform.position + new Vector3(0, 10, 10);
        }
	}

    public override void OnStartAuthority()
    {
        if (hasAuthority)
        {
            if (PlayerPrefs.HasKey("name"))
            {
                SyncData.name = PlayerPrefs.GetString("name");
            }
            if (PlayerPrefs.HasKey("wins"))
            {
                wins = PlayerPrefs.GetInt("wins");
            }
            else
            {
                wins = 0;
            }
            CmdUpdateName(SyncData.name, wins);
            UpdateName();
        }
    }

    public void UpdateName()
    {
        name = SyncData.name + " *" + wins.ToString() + "*";

        text.text = name;
    }

    [Command]
    public void CmdUpdateName(string nameS, int winsS)
    {
        name = nameS + " *" + winsS.ToString() + "*";
        RpcUpdateTxt();
        text.text = name;
    }

    [Command]
    public void CmdUpdateParent(GameObject parentObject)
    {
        parent = parentObject;
        healthAI = parent.GetComponent<HealthAI>();
        serverParent = parent;
        RpcUpdateParent(parent);
    }

    [ClientRpc]
    public void RpcUpdateParent(GameObject parentObject)
    {
        parent = parentObject;
        healthAI = parent.GetComponent<HealthAI>();
    }

    [ClientRpc]
    public void RpcUpdateTxt()
    {
        text.text = name;
    }
}
