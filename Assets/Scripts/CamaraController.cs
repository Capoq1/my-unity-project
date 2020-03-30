using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraController : MonoBehaviour
{

    public GameObject player;
    // Start is called before the first frame update

    public Vector3 offset;
    void Start()
    {
        offset = transform.position - player.transform.position;
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
          transform.position = player.transform.position + offset;
    }
}
