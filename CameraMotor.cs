using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotor : MonoBehaviour
{
    public Transform lookAt; //our player
    public Vector3 offset = new Vector3(0, 0.0f, 0.0f);

    public Vector3 rotation = new Vector3(35, 0, 0);

    public bool IsMoving { set;  get; }
    

    private void LateUpdate()
    {
        if (!IsMoving)
            return;

        Vector3 desiredPosition = lookAt.position + offset;
        //desiredPosition.x = 0;
        transform.position = Vector3.Lerp(transform.position, desiredPosition,Time.deltaTime * 8);
        transform.rotation = Quaternion.Lerp(transform.rotation,Quaternion.Euler(rotation),1.0f);
    }
}
