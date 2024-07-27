using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum property
{
    pressure,
    temp,
    entropy
}


public class CycleControl : MonoBehaviour
{
    List<string> blank = new List<string>() { "Select " };
    //   public TMP_Dropdown State1P, State1T, State1S,
    //State2P, State2T, State2S,
    //State3P, State3T, State3S,
    //State4P, State4T, State4S;
    public Vector3 origin;
    public GameObject errorMessage;
    Vector3[] truepos;
    Vector3 offset = new Vector3(0, 180, 0);
    public TMP_Dropdown P1, P2, T1, T2;

    List<TMP_Dropdown[]> dropdowns = new List<TMP_Dropdown[]>();

    public LineRenderer lr;
    DomeControl dc;
    public PropertyDataSO properties;
    List<float> pressures, temps, tempOps;

    Vector3[] statePos = new Vector3[4];
    List<Vector3> linePos = new List<Vector3> ();
    //bool[] stateset = { false, false, false, false };
    bool[] stateset = { false, false};

    // Start is called before the first frame update
    void Awake()
    {
        lr = FindObjectOfType<LineRenderer>();
        //TODO: fix this its dumb
        properties.loadProp();
        pressures = properties.GrabPressures();
        tempOps = new List<float>();
        //ses = properties.GrabEntropies();
        truepos = new Vector3[0];
        dc = GetComponent<DomeControl>();
    }

    void LoadTemperatures()
    {
        TextAsset dataprop = Resources.Load<TextAsset>("Temps");
        string[] data = dataprop.text.Split("\n");
        for (int i = 1; i < data.Length; i++)
        {
            tempOps.Add(float.Parse(data[i]));
        }
    }
    //TODO: this is atrocious
    private void Start()
    {

        LoadTemperatures();
        //TMP_Dropdown[] state0 = new TMP_Dropdown[3]
        //{
        //    State1P, State1T, State1S
        //};
        //TMP_Dropdown[] state1 = new TMP_Dropdown[3]
        //{
        //    State2P, State2T, State2S
        //};
        //TMP_Dropdown[] state2 = new TMP_Dropdown[3]
        //{
        //    State3P, State3T, State3S
        //};
        //TMP_Dropdown[] state3 = new TMP_Dropdown[3]
        //{
        //    State4P, State4T, State4S
        //};

        TMP_Dropdown[] state0 = new TMP_Dropdown[2]
        {
            P1, T1
        }; 

        TMP_Dropdown[] state1 = new TMP_Dropdown[2]
        {
            P2, T2
        };
        dropdowns.Add(state0);
        dropdowns.Add(state1);
        //dropdowns.Add(state2);
        //dropdowns.Add(state3);

        SetDropdown(dropdowns[1][0], pressures, "Pressure");
        //SetDropdown(dropdowns[2][0], pressures, "Pressure");
        //SetDropdown(dropdowns[3][0], pressures, "Pressure");
        SetDropdown(dropdowns[0][0], pressures, "Pressure");


    }


    // Update is called once per frame
    public void ClearState(int state)
    {
        dropdowns[state][0].value = 0;
        for (int i = 1; i < 3; i++)
        {
            dropdowns[state][i].ClearOptions();
        }
    }

    void ShowErrorMessage(bool show)
    {
        errorMessage.SetActive(show);
    }

    //TODO: temp and entropy go blank when selecting pressure DONE
    //TODO: that's a bad idea but its easier
    //TODO: entropy dropdown is greyed out unless multiple options
    //TODO: add a button to clear everything
    public void FilterData(float filter, property changed, int state)
    {
        ShowErrorMessage(false);
        switch (changed)
        {
            case (property.pressure):
                //dropdowns[state][2].ClearOptions();
                //dropdowns[state][1].ClearOptions();
                //stateset[state] = false;
                //temps = properties.GrabTemps(properties.GetDataByPressure(filter));
                //temps = temps.Where(x => tempOps.Any(y => y == x)).ToList();
                //SetDropdown(dropdowns[state][1], temps, "Temperature");

                dropdowns[state][1].ClearOptions();
                stateset[state] = false;
                temps = properties.GrabTemps(properties.GetDataByPressure(filter));
                //Debug.Log(temps);
                temps = temps.Where(x => tempOps.Any(y => y == x)).ToList();
                //Debug.Log(temps.Count);
                SetDropdown(dropdowns[state][1], temps, "Temperature");
                break;
            case (property.temp):
                //dropdowns[state][2].ClearOptions();
                //stateset[state] = false;
                //var val = dropdowns[state][0].captionText.text;
                //ses = properties.GrabEntropies(filter, float.Parse(val));
                //SetDropdown(dropdowns[state][2], ses, "Entropy");

                stateset[state] = true;


                if (stateset[0] && stateset[1])
                {

                    statePos[0].x = float.Parse(dropdowns[0][0].captionText.text) / 100;
                    statePos[0].y = float.Parse(dropdowns[0][1].captionText.text) / 100;
                    statePos[1].x = float.Parse(dropdowns[0][0].captionText.text) / 100;
                    statePos[1].y = float.Parse(dropdowns[1][1].captionText.text) / 100;
                    statePos[2].x = float.Parse(dropdowns[1][0].captionText.text) / 100;
                    statePos[2].y = float.Parse(dropdowns[1][1].captionText.text) / 100;
                    statePos[3].x = float.Parse(dropdowns[1][0].captionText.text) / 100;
                    statePos[3].y = float.Parse(dropdowns[0][1].captionText.text) / 100;

                    for (int i = 0; i < 4; i++)
                    {
                        //TODO: be less lazy
                        if (properties.GrabEntropies(statePos[i].y * 100, statePos[i].x * 100).Count() < 1)
                            ShowErrorMessage(true);
                        else
                            statePos[i].z = properties.GrabEntropies(statePos[i].y * 100, statePos[i].x * 100)[0];
                    }



                    //Debug.Log(properties.GrabEntropies(statePos[i].y*100, statePos[i].x*100).Count);
                    //bro you should check theres only 1 value....
                
                SetPos(statePos);
                }
                

                break;
            case (property.entropy):
                //stateset[state] = true;
                //statePos[state] = new Vector3(
                //    float.Parse(dropdowns[state][0].captionText.text)/100,
                //    float.Parse(dropdowns[state][1].captionText.text)/100,
                //    float.Parse(dropdowns[state][2].captionText.text));
                //if (stateset[0] && stateset[1] && stateset[2] && stateset[3])
                //    SetPos(statePos);
                Debug.LogWarning("oops2");
                break;
            default:
                break;
        }
    }

    void SetPos(Vector3[] Positions)
    {
        for (int i = 0; i < Positions.Length; i++)
        {
//            Debug.Log(Positions[i]);
        }
        linePos.Clear();
        linePos.Add(Positions[0]);
        for (int i = 1; i < 4; i++)
        {

            //check if an isobar
            if (Positions[i-1].x == Positions[i].x)
            {
///                Debug.Log("isobar");
                //grab everything at that pressure. Note this
                var potentials = properties.GetDataByPressure(Positions[i].x * 100);
                //Debug.Log(potentials.Length);
                //filter by temperature
                IEnumerable<Vector3> connectingData;
                if (Positions[i - 1].y > Positions[i].y)
                    connectingData = potentials.Where(x => x.y < Positions[i - 1].y)
                    .Where(x => x.y > Positions[i].y).Reverse();
                else
                    connectingData = potentials.Where(x => x.y > Positions[i - 1].y)
                    .Where(x => x.y < Positions[i].y);
                Debug.Log(connectingData.Count());
                //then add
                linePos.AddRange(connectingData);
            }
            //check if isotherm
            else if (Positions[i - 1].y == Positions[i].y)
            {
//                Debug.Log("isotherm");
                //grab everything at that temp
                var potentials = properties.GetDataByTemp(Positions[i].y*100); //this is scaled for the line
                //filter by pres
                //Debug.Log(potentials.Length);
                IEnumerable<Vector3> connectingData;
                if(Positions[i-1].x > Positions[i].x)
                    connectingData = potentials.Where(x => x.x < Positions[i - 1].x)
                    .Where(x => x.x > Positions[i].x).Reverse();
                else
                    connectingData = potentials.Where(x => x.x > Positions[i - 1].x)
                        .Where(x => x.x < Positions[i].x);
                //foreach (var c in connectingData)
                //{
                //    Debug.Log(c);
                //};
                //then add
                linePos.AddRange(connectingData);
            }
            //check if isoentropy
            //TODO: this only works bc you control the narrative...
            else if (Positions[i - 1].z == Positions[i].z)
            {
                Debug.Log("isoentr");
                //grab everything at that entropy
                var potentials = properties.GetDataByEntropy(Positions[i].z);
                //filter by pres
                var connectingData = potentials.Where(x => x.x > Positions[i - 1].x)
                    .Where(x => x.x < Positions[i].x);
                //then add
                linePos.AddRange(connectingData);
            }
            linePos.Add(Positions[i]);

        }

        //check if an isobar
        if (Positions[3].x == Positions[0].x)
        {
            Debug.Log("isobar");
            //grab everything at that pressure. Note this
            var potentials = properties.GetDataByPressure(Positions[3].x * 100);
            Debug.Log(potentials.Length);
            //filter by temperature
            IEnumerable<Vector3> connectingData;
            if (Positions[0].y > Positions[3].y)
                connectingData = potentials.Where(x => x.y < Positions[0].y)
                .Where(x => x.y > Positions[3].y).Reverse();
            else
                connectingData = potentials.Where(x => x.y > Positions[0].y)
                .Where(x => x.y < Positions[3].y);
            Debug.Log(connectingData.Count());
            //then add
            linePos.AddRange(connectingData);
        }
        //check if isotherm
        else if (Positions[0].y == Positions[3].y)
        {
            Debug.Log("isotherm");
            //grab everything at that temp
            var potentials = properties.GetDataByTemp(Positions[3].y * 100); //this is scaled for the line
            //filter by pres
            Debug.Log(potentials.Length);
            IEnumerable<Vector3> connectingData;
            if (Positions[0].x > Positions[3].x)
                connectingData = potentials.Where(x => x.x < Positions[0].x)
                .Where(x => x.x > Positions[3].x).Reverse();
            else
                connectingData = potentials.Where(x => x.x > Positions[0].x)
                    .Where(x => x.x < Positions[3].x);
            Debug.Log(connectingData.Count());
            //then add
            linePos.AddRange(connectingData);
        }
        //check if isoentropy
        //TODO: this only works bc you control the narrative...
        else if (Positions[0].z == Positions[3].z)
        {
            Debug.Log("isoentr");
            //grab everything at that entropy
            var potentials = properties.GetDataByEntropy(Positions[3].z);
            //filter by pres
            var connectingData = potentials.Where(x => x.x > Positions[0].x)
                .Where(x => x.x < Positions[3].x);
            //then add
            linePos.AddRange(connectingData);
        }
        linePos.Add(Positions[3]);

        truepos =  linePos.ToArray();
        Translate(ref truepos, origin);
        lr.positionCount = truepos.Length;
        lr.SetPositions(truepos);
        dc.Rotate();
        

    }

    void SetDropdown(TMP_Dropdown d2set, List<float> data, string selector)
    {
        blank[0] = "Select " + selector;
        d2set.AddOptions(blank);

        d2set.AddOptions(data.Select(x=>x.ToString()).ToList());
        
    }

    public void Rotate(float angleX, float angleY, float angleZ)
    {
            Vector3[] newPos = new Vector3[lr.positionCount];
            for (int r = 0; r < truepos.Length; r++)
            {
                newPos[r] = Quaternion.Euler(offset) * Quaternion.Euler(angleX, angleY, angleZ) * truepos[r];
                //Debug.Log(newPos[r]);
            }
            //  Debug.Log(i);
            lr.SetPositions(newPos);


    }
    void Translate(ref Vector3[] props, Vector3 factor)
    {

        for (int i = 0; i < props.Length; i++)
        {
            props[i] += factor;
        }
    }
}
