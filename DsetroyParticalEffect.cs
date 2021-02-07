using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DsetroyParticalEffect : MonoBehaviour
{
    public float lifeTime;

    // Update is called once per frame
    private void Update()
    {
            Destroy(gameObject, lifeTime);
        
               
    }
}
