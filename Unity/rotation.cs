using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotation : MonoBehaviour
{
    public List<Camera> camaras;
    bool rotateCameras = false;
    float rotationspeed = 25.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyUp(KeyCode.R)) rotateCameras = !rotateCameras;
        if (rotateCameras)
        {
            
           transform.Rotate(new Vector3(0, rotationspeed, 0) * Time.deltaTime);
            
        }
    }
}
