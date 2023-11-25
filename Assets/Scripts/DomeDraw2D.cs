using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DomeDraw2D : MonoBehaviour
{
    LineRenderer lr;
    public string domecsv;
    public Vector2 origin, scale;

    [HideInInspector]
    public List<Vector3> points { get; private set; }
    [HideInInspector]
    public List<float> pressures { get; private set; }
    [HideInInspector]
    public int maxindex;
    // Start is called before the first frame update
    void Awake()
    {
        pressures = new List<float>();
        points = new List<Vector3>();

        lr = GetComponent<LineRenderer>();
   
        TextAsset dataprop = Resources.Load<TextAsset>(domecsv);
        string[] data = dataprop.text.Split("\n");
        maxindex = data.Length;
        for (int i = 0; i < data.Length; i++)
        {
            string[] row = data[i].Split(",");
            //   Debug.Log(data[i]);
            //   Debug.Log(float.Parse(row[0])/100);
            if (!float.TryParse(row[2], out _))
                continue;

            pressures.Add(float.Parse(row[1]));
            Vector3 point = new Vector3(float.Parse(row[2])*scale.x, float.Parse(row[0]) * scale.y, 0);
            points.Add(point + (Vector3)origin);

        }

        for (int i = data.Length - 2; i >= 0; i--)
        {
            string[] row = data[i].Split(",");
            //   Debug.Log(data[i]);
            //   Debug.Log(float.Parse(row[0])/100);
            if (!float.TryParse(row[2], out _))
                continue;


            Vector3 point = new Vector3(float.Parse(row[3])*scale.x, float.Parse(row[0]) * scale.y, 0);
            points.Add(point + (Vector3)origin);


        }
        lr.positionCount = points.Count();
        lr.SetPositions(points.ToArray());
    }

}
