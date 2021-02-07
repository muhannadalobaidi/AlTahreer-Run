using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : MonoBehaviour
{
    public AudioClip collectSound;
    public GameObject pickUpEffect;



    void Update()
    {

        transform.Rotate(0, 5, 0 * Time.deltaTime); //rotates 50 degrees per second around z axis
    }



    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Player")
        {
            this.gameObject.SetActive(false);
            AudioSource.PlayClipAtPoint(collectSound, transform.position);
            Instantiate(pickUpEffect, transform.position, transform.rotation);
            StartCoroutine(Respawn(this.gameObject));
            GameManager.Instance.GetMoney();

        }
        IEnumerator Respawn(GameObject gObj)
        {
            yield return new WaitForSeconds(1);
            gObj.transform.position = Vector3.zero;
            gObj.SetActive(true);
        }
    }
}
