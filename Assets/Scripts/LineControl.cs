using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using System;

public class LineControl : MonoBehaviour
{
    DomeDraw2D dd;
    LineRenderer lr;
    Vector3 origin, scale, interpOffset;
    public float steepness;
    public int resolution = 10, temp;
    float xlabel = -.5f;
    float length = 2f, ratio = .5f; //increase in length/increase in height
    List<Vector3> points = new List<Vector3>();
    TextMeshPro label;
    public RectTransform tgl;
    float pressure;
    // Start is called before the first frame update
    private void Awake()
    {
        dd = FindObjectOfType<DomeDraw2D>();
        scale = dd.scale;
        origin = dd.origin;
        lr = GetComponent < LineRenderer > ();
        interpOffset = new Vector3(-steepness, -steepness);
        label = GetComponentInChildren<TextMeshPro>();

    }

    Transform GetChildbyTag(Transform t, string tag)
    {
        for (int i = 0; i < t.childCount; i++)
        {
            if (t.GetChild(i).CompareTag(tag))
                return t.GetChild(i);
        }
        return null;
    }
    void Start()

    {
        List<Vector3> dpoints = GetDomeVal(temp);
        Vector3 start = new Vector3(dpoints[0].x - length * ratio, dpoints[0].y + length);
        tgl.position = Camera.main.WorldToScreenPoint(start);
        points.Add(start);
        Vector3 interpoint = (start + dpoints[0]) / 2 + interpOffset;
        for (int i = 0; i < resolution; i++)
        {
            points.Add(quadInterp(start, interpoint, dpoints[0], (float)i / resolution));
        }
        points.AddRange(dpoints);
        Vector3 end = new Vector3(dpoints[1].x + length , dpoints[1].y - (length * ratio + .5f));
        interpoint = (dpoints[1] + end) / 2 + interpOffset;
        for (int i = 0; i < resolution; i++)
        {
            points.Add(quadInterp(dpoints[1], interpoint, end, (float)i / resolution));
        }
        lr.positionCount = points.Count();
        lr.SetPositions(points.ToArray());
        label.transform.position = new Vector3(xlabel,temp*scale.y+origin.y);
        label.text = "P = " + Math.Round(GetPressAtTemp(temp),3) + " MPa";
        gameObject.SetActive(false);
    }

    List<Vector3> GetDomeVal(float temp)
    {
        var pnts = dd.points.Where(x => x.y == temp * scale.y+ origin.y).ToList();
        if (pnts.Count() > 2) Debug.LogWarning("Ooops");
        return pnts;
    }

    float GetPressAtTemp(int temp)
    {
        return dd.pressures.ElementAt(temp);
    }

    Vector3 quadInterp(Vector3 start, Vector3 mid, Vector3 end, float index)
    {
        Vector3 first = Vector3.Lerp(start, mid, index);
        Vector3 sec = Vector3.Lerp(mid, end, index);
        Debug.Log("index: " + index + " " + Vector3.Lerp(first, sec, index));
        return Vector3.Lerp(first, sec, index);
    }

}
