using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;


//TODO: this should be able to handle all properties, but for now just doing entropy
[CreateAssetMenu(fileName = "Properties", menuName = "ScriptableObjects/PropertyData", order = 1)]
public class PropertyDataSO : ScriptableObject
{
    //TODO: this should be a dropdown
    [Tooltip("Don't include extention")]
    public string propertyfile;
    public List<Vector3> PTS  { get; private set; }

    public void loadProp()
    {
        PTS = new List<Vector3>();
        TextAsset dataprop = Resources.Load<TextAsset>(propertyfile); //file is PsT
        string[] data = dataprop.text.Split("\n");

        for (int i = 0; i < data.Length; i++)
        {
            string[] row = data[i].Split(",");
         //   Debug.Log(data[i]);
         //   Debug.Log(float.Parse(row[0])/100);
           if (!float.TryParse(row[2], out _))
                continue;
            if (float.Parse(row[2]) >= 551)
                continue;
            if (Single.IsNaN(float.Parse(row[2])) || Single.IsNaN(float.Parse(row[1])) )
                continue;
            if (float.Parse(row[1]) >= 10)
                continue;

            Vector3 point = new Vector3(float.Parse(row[0])/100, float.Parse(row[2])/100, float.Parse(row[1]));
            PTS.Add(point);
           // Debug.Log("point added");
        }

    }
    public Vector3[] GetDataByPressure(float pressure)
    {

        return PTS.Where(x => x.x == pressure/100).ToArray();
    }

    public Vector3[] GetDataByTemp(float temp)
    {
        return PTS.Where(x => x.y == temp/100).ToArray();
    }

    public Vector3[] GetDataByEntropy(float s)
    {
        return PTS.Where(x => x.z == s).ToArray();
    }

    public List<float> GrabPressures()
    {
        var pressures = PTS.Select(x => x.x*100);
        return pressures.Distinct().ToList();
    }

     List<float> GrabPressures(Vector3[] fromArray)
    {
        var pressures = fromArray.Select(x => x.x*100);
        return pressures.Distinct().ToList();
    }
    public List<float> GrabTemps()
    {
        var pressures = PTS.Select(x => x.y*100);
        return pressures.Distinct().ToList();
    }

    public List<float> GrabTemps(Vector3[] fromArray)
    {
        var pressures = fromArray.Select(x => x.y*100);
        return pressures.Distinct().ToList();
    }
    public List<float> GrabEntropies(float temp, float pressure)
    {
        var s = PTS.Where(x => x.x == pressure/100).Where(x => x.y==temp/100).Select(x => x.z);
        return s.Distinct().ToList();
    }
}
