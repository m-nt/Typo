using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AlanyticsTypes
{
    wpm,
    cps,
    acc,
    ttc,
    mis
}

public class AlanyticsUI : MonoBehaviour
{
    public Transform Center;
    public LineRenderer Line;
    public LineRendererFill LineFill;
    public AnalyticsItem[] Items;

    // Start is called before the first frame update
    void OnEnable()
    {
        int index = 0;
        foreach (AnalyticsItem item in Items)
        {
            item.Init();
            Vector3 _value = Center.position - (item.offset * item.percentage) + (item.transform.position - Center.position) * item.percentage;
            RTLTMPro.RTLTextMeshPro3D childText = item.Text.transform.GetChild(0).GetComponent<RTLTMPro.RTLTextMeshPro3D>();
            childText.transform.position = _value + (item.offset * 0.2f);
            childText.text = item.value.ToString();
            Debug.Log(item.Name + " : " + _value);
            Line.SetPosition(index, _value);

            index++;
        }
        LineFill.GeneratePentagonShape();
    }

    // Update is called once per frame
    void Update()
    {

    }
}

[System.Serializable]
public class AnalyticsItem
{
    public string Name;
    public Transform transform;
    public Vector3 offset;
    public float max, min;
    public AlanyticsTypes types;
    public RTLTMPro.RTLTextMeshPro3D Text;
    public float percentage, value;

    public void Init()
    {
        percentage = types switch
        {
            AlanyticsTypes.acc => Percentage(CharPerSecond.ACCURACY),
            AlanyticsTypes.cps => Percentage(CharPerSecond.CPS),
            AlanyticsTypes.wpm => Percentage(WordPerMinute.WPM),
            AlanyticsTypes.ttc => Percentage(CharPerSecond.TOTAL_CHAR),
            AlanyticsTypes.mis => Percentage(CharPerSecond.MISS_HITS),
            _ => 0
        };
    }
    private float Percentage(int value)
    {
        this.value = (float)System.Math.Round((double)value, 2);
        float convertedNumber = (value - min) / (max - min);
        return Mathf.Clamp01(convertedNumber);
    }
    private float Percentage(float value)
    {
        this.value = (float)System.Math.Round((double)value, 2);
        float convertedNumber = (value - min) / (max - min);
        return Mathf.Clamp01(convertedNumber);
    }
}