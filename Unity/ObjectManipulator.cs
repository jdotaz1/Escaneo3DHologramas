using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManipulator : MonoBehaviour
{
    public GameObject ManipulatedObject;
    public float ManipulationSensitivity = 0.25f;
    public float RotationSensitivity = 10.0f;
    public float ScaleSensitivity = 0.01f;

	void Start ()
    {
        ActionManager.Instance.ResetEvent += ResetTransform;
	}

	void Update ()
    {
        PerformManipulation();
        PerformRotation();
        PerformZoom();
    }

    void PerformManipulation()
    {
        return;
    }

    void PerformRotation()
    {
            return;

    }

    void PerformZoom()
    {
            return;
  

    }

    void ResetTransform()
    {
        if (ManipulatedObject == null)
            return;

        ManipulatedObject.transform.position = Vector3.zero;
        ManipulatedObject.transform.rotation = Quaternion.identity;
        ManipulatedObject.transform.localScale = Vector3.one;
    }
}
