using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorRotation : MonoBehaviour
{
    [SerializeField]
    private GameObject doorHinge;
    public bool open;
    public void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Trigger" + " hitting " + other.gameObject.name);
        open = true;
    }
    
    
    
    private void Update()
    {
        if (open)
        {
            doorHinge.transform.rotation = Quaternion.Slerp(doorHinge.transform.rotation, Quaternion.Euler(0, 90, 0), 2 * Time.deltaTime);
        }
    }
}
    