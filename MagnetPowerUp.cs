using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetPowerUp : MonoBehaviour
{
    public GameObject coinDetector;

     void Start()
    {
        coinDetector = GameObject.FindGameObjectWithTag("coinDetector");
        coinDetector.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            StartCoroutine(ActivateCoin());
            Destroy(gameObject);
        }
    }
    
    IEnumerator ActivateCoin()
    {
        coinDetector.SetActive(true);
        yield return new WaitForSeconds(10f);
        coinDetector.SetActive(false);
    }

}
