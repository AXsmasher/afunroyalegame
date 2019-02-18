﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DamageDealer : MonoBehaviour {

    public float damage;
    public bool onServer = false;
    public bool parent = false;
    private GameObject destroyThis;
    public float speed = 5f;
    public Rigidbody weapon;
    public bool isAWeapon = false;
    public GameObject particle;
    public GameObject gunParticle;
    public Rigidbody rb;
    public bool particleDone = false;
    public bool punching = false;
    public float velocityX;
    public float velocityY;
    public bool weaponParticle = false;
    public Color ouchie = new Color(0.8f, 0.05f, 0.05f);
    Color oldColor;
    public GameObject hitMarker;
    GameObject hit;
    public bool hitable = false;
    public Transform local;

    public AudioClip splat;
    public AudioClip ping;
    public AudioSource audioSource;
    public bool hasKilled;

    public bool localFired;

    IEnumerator Start()
    {
        hasKilled = false;
        while (!GameObject.Find("LocalPlayer") && !GameObject.Find("LoadingPlayer"))
        {
            yield return null;
        }
        if (GameObject.Find("LocalPlayer"))
        {
            local = GameObject.Find("LocalPlayer").transform;
        }
        else if (GameObject.Find("LoadingPlayer"))
        {
            local = GameObject.Find("LoadingPlayer").transform;
        }
        rb = GetComponent<Rigidbody>();
        yield return new WaitForSeconds(0.1f);
        if (isAWeapon)
        {
            if (GetComponent<WeaponIndexHolder>().isServer)
            {
                weapon = GetComponent<Rigidbody>();
            }
            else
            {
                isAWeapon = false;
            }
        }
    }

    void FixedUpdate()
    {
        if (weapon && isAWeapon)
        {
            damage = (int)(weapon.velocity.magnitude * 0.3f);
        }
    }

    void OnCollisionEnter(Collision collisionInfo)
    {
        if (collisionInfo.gameObject.tag == "Breakable")
        {
            if (local)
            {
                audioSource.PlayOneShot(ping, SyncData.sfx * 1.3f * (Mathf.Clamp((200 - Vector3.Distance(transform.position, local.position)), 0, 200) / 200));
            }
            
            destroyThis = collisionInfo.gameObject;
            StartCoroutine("Destroyer");
        }
        if (collisionInfo.gameObject.layer == 24 && !particleDone && gameObject.tag != "WeaponItem" && gameObject.name != "LimbEnd")
        {
            if (local)
            {
                audioSource.PlayOneShot(splat, SyncData.sfx * 1.3f * (Mathf.Clamp((200 - Vector3.Distance(transform.position, local.position)), 0, 200) / 200));
            }
            StartCoroutine(SpawnParticle(collisionInfo));
            particleDone = true;
        }
        else if (collisionInfo.gameObject.layer == 24 && !particleDone && weaponParticle && gameObject.name != "LimbEnd")
        {
            if (local)
            {
                audioSource.PlayOneShot(splat, SyncData.sfx * 1.3f * (Mathf.Clamp((200 - Vector3.Distance(transform.position, local.position)), 0, 200) / 200));
            }
            StartCoroutine(SpawnParticle(collisionInfo));
            particleDone = true;
        }
        else if (collisionInfo.gameObject.layer == 24 && !particleDone && gameObject.tag != "WeaponItem" && punching)
        {
            if (local)
            {
                audioSource.PlayOneShot(splat, SyncData.sfx * 1.3f * (Mathf.Clamp((200 - Vector3.Distance(transform.position, local.position)), 0, 200) / 200));
            }
            StartCoroutine(SpawnParticle(collisionInfo));
            particleDone = true;
        }
        else if(gameObject.layer == 9 && !particleDone)
        {
            if (gameObject.tag != "WeaponItem")
            {
                if (audioSource.enabled && local)
                {
                    audioSource.PlayOneShot(ping, SyncData.sfx * 1.3f * (Mathf.Clamp((200 - Vector3.Distance(transform.position, local.position)), 0, 200) / 200));
                }
            }
            StartCoroutine(SpawnParticle(collisionInfo));
        }

        if (weapon)
        {
            if (collisionInfo.gameObject.layer == 24)
            {
                if (collisionInfo.gameObject.tag == "PosRelay")
                {
                    if (collisionInfo.gameObject.GetComponent<AISetup>().hasAuthority)
                    {
                        onServer = true;
                    }
                    else
                    {
                        onServer = false;
                    }
                    if (onServer)
                    {
                        if (collisionInfo.gameObject.GetComponent<HealthAI>().health - damage <= 0 && localFired)
                        {
                            if (!hasKilled)
                            {
                                SyncData.kills++;
                            }
                        }
                        hasKilled = true;

                        collisionInfo.gameObject.GetComponent<HealthAI>().CmdUpdateHealth(damage);
                    }
                    weapon = null;
                    return;
                }
                else if (collisionInfo.transform.parent.gameObject.tag == "PosRelay")
                {
                    if (collisionInfo.transform.parent.gameObject.GetComponent<AISetup>().hasAuthority)
                    {
                        onServer = true;
                    }
                    else
                    {
                        onServer = false;
                    }
                    if (onServer)
                    {
                        if (collisionInfo.transform.parent.gameObject.GetComponent<HealthAI>().health - damage <= 0 && localFired)
                        {
                            if (!hasKilled)
                            {
                                SyncData.kills++;
                            }
                        }
                        hasKilled = true;
                        collisionInfo.transform.parent.gameObject.GetComponent<HealthAI>().CmdUpdateHealth(damage);
                    }
                    weapon = null;
                    return;
                }
                else if (collisionInfo.transform.parent.parent.gameObject.tag == "PosRelay")
                {
                    if (collisionInfo.transform.parent.parent.gameObject.GetComponent<AISetup>().hasAuthority)
                    {
                        onServer = true;
                    }
                    else
                    {
                        onServer = false;
                    }
                    if (onServer)
                    {
                        if (collisionInfo.transform.parent.parent.gameObject.GetComponent<HealthAI>().health - damage <= 0 && localFired)
                        {
                            if (!hasKilled)
                            {
                                SyncData.kills++;
                            }
                        }
                        hasKilled = true;
                        collisionInfo.transform.parent.parent.gameObject.GetComponent<HealthAI>().CmdUpdateHealth(damage);
                    }
                    weapon = null;
                    return;
                }
                else if (collisionInfo.transform.parent.parent.parent.gameObject.tag == "PosRelay")
                {
                    if (collisionInfo.transform.parent.parent.parent.gameObject.GetComponent<AISetup>().hasAuthority)
                    {
                        onServer = true;
                    }
                    else
                    {
                        onServer = false;
                    }
                    if (onServer)
                    {
                        if (collisionInfo.transform.parent.parent.parent.gameObject.GetComponent<HealthAI>().health - damage <= 0 && localFired)
                        {
                            if (!hasKilled)
                            {
                                SyncData.kills++;
                            }
                        }
                        hasKilled = true;
                        collisionInfo.transform.parent.parent.parent.gameObject.GetComponent<HealthAI>().CmdUpdateHealth(damage);
                    }
                    weapon = null;
                    return;
                }
            }

        }
        else if(hitable)
        {
            if (collisionInfo.gameObject.layer == 24)
            {
                if (collisionInfo.gameObject.tag == "PosRelay")
                {
                    if (collisionInfo.gameObject.GetComponent<AISetup>().hasAuthority)
                    {
                        onServer = true;
                    }
                    else
                    {
                        onServer = false;
                    }
                    if (onServer)
                    {
                        if (collisionInfo.gameObject.GetComponent<HealthAI>().health - damage <= 0 && localFired)
                        {
                            if (!hasKilled)
                            {
                                SyncData.kills++;
                            }
                        }
                        hasKilled = true;
                        collisionInfo.gameObject.GetComponent<HealthAI>().CmdUpdateHealth(damage);
                    }
                    hitable = false;
                    return;
                }
                else if (collisionInfo.transform.parent.gameObject.tag == "PosRelay")
                {
                    if (collisionInfo.transform.parent.gameObject.GetComponent<AISetup>().hasAuthority)
                    {
                        onServer = true;
                    }
                    else
                    {
                        onServer = false;
                    }
                    if (onServer)
                    {
                        if (collisionInfo.transform.parent.gameObject.GetComponent<HealthAI>().health - damage <= 0 && localFired)
                        {
                            if (!hasKilled)
                            {
                                SyncData.kills++;
                            }
                        }
                        hasKilled = true;
                        collisionInfo.transform.parent.gameObject.GetComponent<HealthAI>().CmdUpdateHealth(damage);
                    }
                    hitable = false;
                    return;
                }
                else if (collisionInfo.transform.parent.parent.gameObject.tag == "PosRelay")
                {
                    if (collisionInfo.transform.parent.parent.gameObject.GetComponent<AISetup>().hasAuthority)
                    {
                        onServer = true;
                    }
                    else
                    {
                        onServer = false;
                    }
                    if (onServer)
                    {
                        if (collisionInfo.transform.parent.parent.gameObject.GetComponent<HealthAI>().health - damage <= 0 && localFired)
                        {
                            if (!hasKilled)
                            {
                                SyncData.kills++;
                            }
                        }
                        hasKilled = true;
                        collisionInfo.transform.parent.parent.gameObject.GetComponent<HealthAI>().CmdUpdateHealth(damage);
                    }
                    hitable = false;
                    return;
                }
                else if (collisionInfo.transform.parent.parent.parent.gameObject.tag == "PosRelay")
                {
                    if (collisionInfo.transform.parent.parent.parent.gameObject.GetComponent<AISetup>().hasAuthority)
                    {
                        onServer = true;
                    }
                    else
                    {
                        onServer = false;
                    }
                    if (onServer)
                    {
                        if (collisionInfo.transform.parent.parent.parent.gameObject.GetComponent<HealthAI>().health - damage <= 0 && localFired)
                        {
                            if (!hasKilled)
                            {
                                SyncData.kills++;
                            }
                        }
                        hasKilled = true;
                        collisionInfo.transform.parent.parent.parent.gameObject.GetComponent<HealthAI>().CmdUpdateHealth(damage);
                    }
                    hitable = false;
                    return;
                }
            }
        }
        isAWeapon = false;
        weaponParticle = false;
    }

    IEnumerator SpawnParticle(Collision info)
    {
        if (info.gameObject.layer == 24)
        {
            ParticleSystem.MainModule system = particle.GetComponent<ParticleSystem>().main;
            if (info.gameObject.tag == "PosRelay" || info.gameObject.name == "LimbEnd")
            {
                system.startColor = info.gameObject.GetComponent<SpriteRenderer>().color;
                GameObject ouchParticle = Instantiate(particle, info.contacts[0].point, Quaternion.identity);
                ouchParticle.transform.forward = new Vector3(-info.contacts[0].normal.x, -info.contacts[info.contacts.Length - 1].normal.y, info.contacts[info.contacts.Length - 1].normal.z);
                if (punching)
                {
                    hit = Instantiate(hitMarker, info.contacts[0].point, Quaternion.identity);
                    hit.GetComponent<SpriteRenderer>().color = oldColor;
                }

                oldColor = info.gameObject.GetComponent<ColouriserAI>().m_NewColor;

                info.gameObject.GetComponent<SpriteRenderer>().color = ouchie;
                
                yield return new WaitForSeconds(0.2f);
                info.gameObject.GetComponent<SpriteRenderer>().color = oldColor;
                if (punching)
                {
                    Destroy(hit);
                }
                yield return new WaitForEndOfFrame();
                info.gameObject.GetComponent<SpriteRenderer>().color = oldColor;
            }
            else
            {
                system.startColor = info.transform.GetChild(0).GetComponent<SpriteRenderer>().color;
                GameObject ouchParticle = Instantiate(particle, new Vector3(info.contacts[0].point.x, info.contacts[0].point.y, 1), Quaternion.identity);
                ouchParticle.transform.forward = new Vector3(-info.contacts[0].normal.x, -info.contacts[info.contacts.Length - 1].normal.y, info.contacts[info.contacts.Length - 1].normal.z);
                if (punching)
                {
                    hit = Instantiate(hitMarker, info.contacts[0].point, Quaternion.identity);
                    hit.GetComponent<SpriteRenderer>().color = oldColor;
                }

                oldColor = info.transform.GetChild(0).gameObject.GetComponent<ColouriserAI>().m_NewColor;

                info.transform.GetChild(0).GetComponent<SpriteRenderer>().color = ouchie;

                yield return new WaitForSeconds(0.1f);
                info.transform.GetChild(0).GetComponent<SpriteRenderer>().color = oldColor;
                if (punching)
                {
                    Destroy(hit);
                }
                yield return new WaitForEndOfFrame();
                info.transform.GetChild(0).GetComponent<SpriteRenderer>().color = oldColor;
            }
        }
        else if (gameObject.layer == 9)
        {
            GameObject particle = Instantiate(gunParticle, new Vector3(info.contacts[0].point.x, info.contacts[0].point.y, 1), Quaternion.identity);
            particle.transform.forward = info.contacts[0].normal;
        }
    }

    IEnumerator Destroyer()
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(destroyThis);
    }
}
