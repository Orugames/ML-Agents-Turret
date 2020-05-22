using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;

public class Bullet : MonoBehaviour
{
    public TurretAgent turretAgent;
    public float seconds = 2;

    public void OnEnable()
    {
        StartCoroutine(DestructionInSeconds(seconds));
    }

    public void Update()
    {
        transform.Translate(-this.transform.forward * 4, Space.Self);
    }


    public IEnumerator DestructionInSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        this.gameObject.SetActive(false);
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.name == "Objective")
        {
            Debug.Log("toque objetivo");
            turretAgent.ObjectiveShot();
        }
            
    }
}
