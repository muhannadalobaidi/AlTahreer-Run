using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Coin : MonoBehaviour
{

    public AudioClip collectSound;
    public GameObject pickUpEffect;
    
    

    void Update()
    {
        
        transform.Rotate(0, 0, 150 * Time.deltaTime); //rotates 50 degrees per second around z axis
    }

    

        private void OnTriggerEnter(Collider other)
    {

        if(other.tag == "Player")
        {

            this.gameObject.SetActive(false);
            AudioSource.PlayClipAtPoint(collectSound, transform.position);
            GameManager.Instance.GetCoin();
            StartCoroutine(Respawn(this.gameObject));
            Instantiate(pickUpEffect, transform.position, transform.rotation);
        }
        IEnumerator Respawn(GameObject gObj)
        {
            yield return new WaitForSeconds(1);
            gObj.transform.position = Vector3.zero;
            gObj.SetActive(true);
        }
    }
}
