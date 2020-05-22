using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject bulletParent;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 100; i++)
        {
            GameObject newBullet = Instantiate(bulletPrefab, bulletParent.transform);
            
        }
    }

    public GameObject GetBullet()
    {
        // Search for any inactive cube in the list and return it
        for (int i = 0; i < 100; i++)
        {
            if (!bulletParent.transform.GetChild(i).gameObject.activeSelf)
            {
                bulletParent.transform.GetChild(i).gameObject.SetActive(true);
                return bulletParent.transform.GetChild(i).gameObject;
            }

        }

        return null;
    }
}
