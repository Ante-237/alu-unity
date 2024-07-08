using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireManager : MonoBehaviour
{
    public List<GameObject> sprites = new List<GameObject>();
    public SettingSO settings;

    private void Start()
    {
        updateAmmo();
    }

    public void updateAmmo()
    {
        for(int i = 0; i < settings.ammo; i++)
        {
            sprites[i].SetActive(true);
        }
    }
}
