using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PropertyTrace : MonoBehaviour
{
    [TooltipAttribute("Temp, then pressure")]
    public LineRenderer[] lr; //temp, then pressure
    public PropertyDataSO properties;
    public Vector3 orign;
    public float pressure = 75.431f;
    public float temp = 390.69f;
    Vector3 offset = new Vector3(0, 180, 0);
    List<Vector3[]> truePos = new List<Vector3[]>();
    // Start is called before the first frame update
    void Awake()
    {
        //TODO: fix this its dumb
        properties.loadProp();
        ShowIsotherm(temp);
        ShowIsobar(pressure);
        //initRotate(0,0,0);
        var tempPos = new Vector3[lr[0].positionCount];
        lr[0].GetPositions(tempPos);
        truePos.Add(tempPos);
        var tempPos1 = new Vector3[lr[1].positionCount];
        lr[1].GetPositions(tempPos1);
        truePos.Add(tempPos1);
        //        Debug.Log(properties.GetDataByPressure(.01f)[0]);
        Rotate(0, 0, 0);
    }

    private void Start()
    {

    }

    // Update is called once per frame
    void initRotate(float angleX, float angleY, float angleZ)
    {

        for (int i = 0; i < lr.Length; i++)
        {
            Vector3[] rotPos = new Vector3[lr[i].positionCount];
            Vector3[] newPos = new Vector3[lr[i].positionCount];
            lr[i].GetPositions(rotPos);
            for (int r = 0; r < rotPos.Length; r++)
            {
                newPos[r] = Quaternion.Euler(offset)*Quaternion.Euler(angleX, angleY, angleZ) * rotPos[r];
                //Debug.Log(newPos[r]);
            }
            Debug.Log(i);
            lr[i].SetPositions(newPos);
        }
       
    }

    public void Rotate(float angleX, float angleY, float angleZ)
    {

        for (int i = 0; i < lr.Length; i++)
        {
            Vector3[] newPos = new Vector3[lr[i].positionCount];
            for (int r = 0; r < truePos[i].Length; r++)
            {
                newPos[r] = Quaternion.Euler(offset) * Quaternion.Euler(angleX, angleY, angleZ) * truePos[i][r];
                //Debug.Log(newPos[r]);
            }
          //  Debug.Log(i);
            lr[i].SetPositions(newPos);
        }

    }
    void Translate(ref Vector3[] props, Vector3 factor) {

        for (int i = 0; i < props.Length; i++)
        {
            props[i] += factor;
        }
    }

    //TODO: Getting the data is a pain
    public void ShowIsotherm(float T)
    {
        Vector3[] props = properties.GetDataByTemp(T);
        Translate(ref props,orign);
        lr[0].positionCount = props.Length;
        lr[0].SetPositions(props);
    }

    public void ShowIsobar(float P)
    {
        Vector3[] props = properties.GetDataByPressure(P);
        Translate(ref props, orign);
        lr[1].positionCount = props.Length;
        lr[1].SetPositions(props);
    }

    public void HideIsotherm()
    {
        lr[0].positionCount = 0;
    }

    public void HideIsobar()
    {
        lr[1].positionCount = 0;
    }
}
