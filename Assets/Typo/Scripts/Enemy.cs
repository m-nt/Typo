using UnityEngine;
using RTLTMPro;
using System.Collections;
using TMPro;

public enum TextColor
{
    red,
    green,
    blue,
    yellow,
    undefined,
}
public struct ColorTag
{
    public string Tag { get; }
    public TextColor TextColor { get; set; }
    public string Seperator { get; set; }
    public int Height { get; set; }

    public ColorTag(TextColor textColor, string seperator = "&SPT&")
    {
        TextColor = textColor != TextColor.undefined ? textColor : TextColor.red;
        Seperator = seperator;
        Tag = "<color=" + TextColor + ">" + seperator + "</color>";
        Height = Tag.Length - seperator.Length + 1;
    }
}


public class Enemy : MonoBehaviour, IKeyboard
{
    # region Public Variables
    public float speed;
    public string Name { get; set; }
    public TextColor TagColor;
    public Color Primary, Secondary;

    #endregion
    # region Private Variables
    private RTLTextMeshPro3D text;

    private float Duration;
    private Vector3 Destination;
    private IEnumerator TransformCoroutine;
    private ColorTag Tag;
    private int index;
    #endregion

    #region MonoBehaviour
    // Start is called before the first frame update
    void Start()
    {
        if (KeyboardCapture.self != null)
            KeyboardCapture.self.registeredKeys.AddListener(OnKey);
        if (KeyboardCaptureV2.self != null)
            KeyboardCaptureV2.self.registeredKeys.AddListener(OnKeyBuiltIn);
        // Test, remove later
        // Inintialize("hello", 0.5f, new Vector3(0, -3, -5));

    }

    #endregion

    #region Private Properties
    private IEnumerator SmoothTransformCoroutine()
    {
        Vector3 startPosition = transform.position;
        float startTime = Time.time;

        while (Time.time - startTime < Duration)
        {
            float t = (Time.time - startTime) / Duration;
            transform.position = Vector3.Lerp(startPosition, Destination, t);
            yield return null;
        }

        transform.position = Destination;
        TransformCoroutine = null;
    }
    #endregion

    #region Public Properties
    public void OnKey(KeyboardType keyType)
    {
        KeyboardEventHandler(keyType.Key);
    }
    public void OnKeyBuiltIn(string keyType)
    {
        KeyboardEventHandler(keyType);
    }
    void KeyboardEventHandler(string keyType)
    {
        // Check if the characters of the enemy is filled, prevent overflow error
        if (index >= Name.Length) return;
        // Check if the key clicked/touched is the corresponding character of the enemy
        if (Name[index].ToString().ToUpper() == keyType)
        {
            if (index <= 0)
            {
                // if selected enemy in enemy manager is null asign this gameObject to the selected enemy
                EnemyManager.self.SelectedEnemy = EnemyManager.self.SelectedEnemy == null ? gameObject : EnemyManager.self.SelectedEnemy;
            }
            // Check is this enemy is selected or not
            if (EnemyManager.self.SelectedEnemy != gameObject) return;
            this.GetComponentInChildren<SpriteRenderer>().color = Secondary;
            // add the tag to the corresponding character
            text.add_tag(Tag.Tag, Tag.Seperator, index * Tag.Height);
            index++;
            if (index >= Name.Length)
            {
                // what gonna happens to the Enemy
                Debug.Log("enemy: " + Name + " is destroyed !");
                Destroy(gameObject);
                // TODO: add destroy affect
            }
        }
    }
    public void Inintialize(string name, float speed, Vector3 target, TMP_FontAsset TMP_Font = null, TextColor tagColor = TextColor.undefined)
    {
        Name = name;
        text = transform.GetComponent<RTLTextMeshPro3D>();
        text.text = Name.ToLower();
        text.font = TMP_Font == null ? text.font : TMP_Font;
        Destination = target;
        this.speed = speed;
        Duration = Vector3.Distance(transform.position, target) / speed;
        TagColor = tagColor;
        Tag = new ColorTag(tagColor);
        SmoothlyTransform();
    }
    public void SmoothlyTransform()
    {
        if (TransformCoroutine != null)
        {
            StopCoroutine(TransformCoroutine);
        }

        TransformCoroutine = SmoothTransformCoroutine();
        StartCoroutine(TransformCoroutine);
    }
    #endregion

    #region Static Properties

    #endregion
}
