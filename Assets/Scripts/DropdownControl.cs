using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DropdownControl : MonoBehaviour
{
    TMP_Dropdown dropdown;
    CycleControl cc;
    public int state;
    public property property;
    // Start is called before the first frame update
    void Awake()
    {
        dropdown = GetComponent<TMP_Dropdown>();
        cc = FindObjectOfType<CycleControl>();
    }


    private void Start()
    {
        var comps = GetComponentsInChildren<TextMeshProUGUI>();
        for (int i = 0; i < comps.Length; i++)
        {
            comps[i].enableAutoSizing = true;
            comps[i].fontSizeMin = 9;
        }

        dropdown.onValueChanged.AddListener(e => {
            float val;
            if (float.TryParse(dropdown.captionText.text, out val))
                cc.FilterData(val, property, state);
        });
    }



}
