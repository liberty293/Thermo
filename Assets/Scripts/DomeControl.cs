using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DomeControl : MonoBehaviour
{
    public Slider xval, yval, zval, zoom;
    PropertyTrace pt;
    CycleControl cc;
    Transform cam;

    Vector3 offset = new Vector3(5, 0, 5);
    Vector3 target;
    private void Awake()
    {
        target = transform.position + offset;
            cam = Camera.main.transform;
        pt = GetComponent < PropertyTrace > ();
        cc = GetComponent<CycleControl>();
    }
    private void Start()
    {
        Rotate();
        Zoom();
    }
    public void Rotate()
    {
        //float x = offset.x + zoom.value * Mathf.Sin(xval.value)*Mathf.Cos(yval.value);
        //Debug.Log(x);
        //float y = offset.y + zoom.value * Mathf.Sin(xval.value) * Mathf.Sin(yval.value);
        //float z = offset.z + zoom.value * Mathf.Cos(xval.value);
        //cam.position = new Vector3(x, y, z);
        //cam.rotation = Quaternion.LookRotation(target-cam.position,cam.up);
        transform.rotation = Quaternion.Euler(xval.value, yval.value, zval.value);

        pt?.Rotate(-xval.value, yval.value, -zval.value);
        cc?.Rotate(-xval.value, yval.value, -zval.value);
    }

    public void Zoom()
    {
        cam.position = new Vector3(cam.position.x, cam.position.y, zoom.value);
        //float newz = cam.rotation.eulerAngles.z - (zval.value - currentzval);
        //cam.rotation = Quaternion.Euler(new Vector3(cam.rotation.eulerAngles.x, cam.rotation.eulerAngles.y, newz));
        //currentzval = zval.value;
        //Debug.Log(zval.value);
    }
}
