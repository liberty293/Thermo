using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

//TODO: remove isotherm
//switch only between 2 views to see isobar straight vs curved
//on cycles already have the square set up

public class PropertyTrace : MonoBehaviour
{
    [TooltipAttribute("Temp, then pressure")]
    public LineRenderer[] lr; //temp, then pressure
    public PropertyDataSO properties;
    List<float> pressures, temps;
    public TMP_Dropdown P, T;
    public Vector3 orign;
    float pressure;
    float temp;
    float x, y, z;
    Vector3 offset = new Vector3(0, 180, 0);
    Vector3[] tempPs = new Vector3[0];
    Vector3[] presPs = new Vector3[0];
    //List<Vector3[]> truePos = new List<Vector3[]>();
    // Start is called before the first frame update
    void Awake()
    {
        //TODO: fix this its dumb
        properties.loadProp();
        //ShowIsobar(75.431f);
        //initRotate(0,0,0);
        //var tempPos = new Vector3[lr[0].positionCount];
        //lr[0].GetPositions(tempPos);
        //truePos.Add(tempPos);
        //var tempPos1 = new Vector3[lr[1].positionCount];
        //lr[1].GetPositions(tempPos1);
        //truePos.Add(tempPos1);
        //        Debug.Log(properties.GetDataByPressure(.01f)[0]);
        //Rotate(0, 0, 0);
    }

    void LoadTemperatures()
    {
        TextAsset dataprop = Resources.Load<TextAsset>("Temps");
        string[] data = dataprop.text.Split("\n");
        temps = new List<float>();
        for (int i = 1; i < data.Length; i++)
        {
            temps.Add(float.Parse(data[i]));
        }
    }

    private void Start()
    {
        pressures = properties.GrabPressures();
        LoadTemperatures();
        SetDropdown(P, pressures, "Pressure");
        SetDropdown(T, temps, "Temperature");
        P.onValueChanged.AddListener(e =>
        {
            
            if (float.TryParse(P.captionText.text, out pressure))
                ShowIsobar(pressure);
        });
        T.onValueChanged.AddListener(e =>
        {
            if (float.TryParse(T.captionText.text, out temp))
                ShowIsotherm(temp);
        });
    }

    void SetDropdown(TMP_Dropdown d2set, List<float> data, string selector)
    {
        d2set.AddOptions(new List<string>() { "Select " + selector });

        d2set.AddOptions(data.Select(x => x.ToString()).ToList());
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
        x = angleX; y = angleY; z = angleZ;
        //rotate pressure
        Vector3[] newPos = new Vector3[lr[1].positionCount];
        for (int r = 0; r < presPs.Length; r++)
        {
            newPos[r] = Quaternion.Euler(offset) * Quaternion.Euler(angleX, angleY, angleZ) * presPs[r];
            //Debug.Log(newPos[r]);
        }
        //  Debug.Log(i);
        lr[1].SetPositions(newPos);

        //rotate temp
        newPos = new Vector3[lr[0].positionCount];
        for (int r = 0; r < tempPs.Length; r++)
        {
            newPos[r] = Quaternion.Euler(offset) * Quaternion.Euler(angleX, angleY, angleZ) * tempPs[r];
            //Debug.Log(newPos[r]);
        }
        //  Debug.Log(i);
        lr[0].SetPositions(newPos);

    }
    void Translate(ref Vector3[] props, Vector3 factor) {

        for (int i = 0; i < props.Length; i++)
        {
            props[i] += factor;
        }
    }

    //TODO: Getting the data is a pain
     void ShowIsotherm(float T)
    {
        tempPs = properties.GetDataByTemp(T);
        //Debug.Log(props[5]);
        Translate(ref tempPs, orign);
        lr[0].positionCount = tempPs.Length;
        lr[0].SetPositions(tempPs);
        Rotate(x, y, z);
    }

     void ShowIsobar(float P)
    {
        presPs = properties.GetDataByPressure(P);
        Translate(ref presPs, orign);
        lr[1].positionCount = presPs.Length;
        lr[1].SetPositions(presPs);
        Rotate(x, y, z);
    }

    //public void HideShowP(bool show)
    //{
    //    if (show) ShowIsobar(pressure); else HideIsobar();
    //}

    //public void HideShowT(bool show)
    //{
    //    if (show) ShowIsotherm(temp); else HideIsotherm();
    //}

    //void HideIsotherm()
    //{
    //    lr[0].positionCount = 0;
    //}

    // void HideIsobar()
    //{
    //    lr[1].positionCount = 0;
    //}
}
