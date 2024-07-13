using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DomeControl : MonoBehaviour
{
    public Slider xval, yval, zval, zoom;
    public Vector4 TsDiagram, PsDiagram, TPDiagram;
    public float TurnSpeed = .5f, msp = .1f;
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
        //StartCoroutine(Rotate(Quaternion.Euler(PsDiagram.x, PsDiagram.y, PsDiagram.z), PsDiagram));
        Rotate();
        Zoom();
    }

    public void TsRotation () => StartCoroutine(Rotate(Quaternion.Euler(TsDiagram.x, TsDiagram.y, TsDiagram.z), TsDiagram));
    public void PsRotation () => StartCoroutine(Rotate(Quaternion.Euler(PsDiagram.x, PsDiagram.y, PsDiagram.z), PsDiagram));
    public void TPRotation() => StartCoroutine(Rotate(Quaternion.Euler(TPDiagram.x, TPDiagram.y, TPDiagram.z), TPDiagram));

    public void Rotate()
    {
        //float x = offset.x + zoom.value * Mathf.Sin(xval.value) * Mathf.Cos(yval.value);
        //Debug.Log(x);
        //float y = offset.y + zoom.value * Mathf.Sin(xval.value) * Mathf.Sin(yval.value);
        //float z = offset.z + zoom.value * Mathf.Cos(xval.value);
        //cam.position = new Vector3(x, y, z);
        //cam.rotation = Quaternion.LookRotation(target - cam.position, cam.up);

        transform.rotation = Quaternion.Euler(xval.value, yval.value, zval.value);

//        pt?.Rotate(-xval.value, yval.value, -zval.value);
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

    void SetPosition(Vector4 pos)
    {
        xval.value = pos.x;
        yval.value = pos.y;
        zval.value = pos.z;
        zoom.value = pos.w;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
        {

            transform.Rotate(new Vector3(Input.GetAxis("Mouse Y"), -Input.GetAxis("Mouse X"),0)/Time.deltaTime * msp, Space.World);
            SetPosition(new Vector4(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z, zoom.value));
        }
    }

    IEnumerator Rotate(Quaternion pos, Vector4 posvec)
    {
        float time = 0;
        Quaternion initial = transform.rotation;
        while (time < TurnSpeed)
        {
            transform.rotation = Quaternion.Lerp(initial, pos, time / TurnSpeed);
            time += Time.deltaTime;
            Debug.Log(time);
            yield return null;
        }
        transform.rotation = pos;
        SetPosition(posvec);
    }
}
