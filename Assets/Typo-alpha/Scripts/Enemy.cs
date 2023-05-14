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
    public string hex { get; set; }
    public string Seperator { get; set; }
    public int Height { get; set; }

    public ColorTag(TextColor textColor, string seperator = "&SPT&")
    {
        TextColor = textColor != TextColor.undefined ? textColor : TextColor.red;
        hex = "#ff0000";
        Seperator = seperator;
        Tag = "<color=" + TextColor + ">" + seperator + "</color>";
        Height = Tag.Length - seperator.Length + 1;
    }
    public ColorTag(string hex = "#ff0000", string seperator = "&SPT&")
    {
        TextColor = TextColor.undefined;
        this.hex = hex;
        Seperator = seperator;
        Tag = "<color=" + this.hex + ">" + seperator + "</color>";
        Height = Tag.Length - seperator.Length + 1;
    }
}

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class Enemy : MonoBehaviour, IKeyboard
{
    # region Public Variables
    public float speed;
    public string Name { get; set; }
    public TextColor TagColor;
    public Color Primary, Secondary;
    public Vector2 forceDirection; // The direction of the force to apply
    public float min_force_magnitude, max_force_magnitude; // The minimum

    #endregion
    # region Private Variables
    private RTLTextMeshPro3D text;
    private float forceMagnitude; // The magnitude of the force to apply

    private float Duration;
    private Vector3 Destination;
    private IEnumerator TransformCoroutine;
    private ColorTag Tag;
    private int index;


    private Rigidbody2D rb;
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
        rb = GetComponent<Rigidbody2D>();
    }

    #endregion

    #region Private Properties
    private void OnDestroy()
    {
        EnemyManager.self.deadEnemy = 1; // Remove 1 enemy from the EnemyManager
    }
    private void FixedUpdate()
    {
        Vector2 force = forceDirection.normalized * forceMagnitude;
        Vector2 velocity = rb.velocity;
        Vector2 velocityChange = force - velocity;
        rb.AddForce(velocityChange, ForceMode2D.Impulse);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        ContactPoint2D[] contacts = new ContactPoint2D[collision.contactCount];
        collision.GetContacts(contacts);

        foreach (ContactPoint2D contact in contacts)
        {
            Vector2 normal = contact.normal;
            forceDirection = new Vector2(Mathf.Abs(forceDirection.x) * normal.x, forceDirection.y);
            // rb.AddForce(normal * forceMagnitude * forceMagnitude, ForceMode2D.Impulse);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Finish")
        {
            if (EnemyManager.self.score > 0) EnemyManager.self.score = -1;
            Destroy(gameObject);
        }
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
        if (Name[index].ToString().ToUpper() != keyType.ToUpper()) return;

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
        if (index < Name.Length) return;
        // what gonna happens to the Enemy
        EnemyManager.self.score = 1;
        Destroy(gameObject);
        // TODO: add destroy affect
    }

    public void Inintialize(string name, float speed, Vector3 target, TMP_FontAsset TMP_Font = null, TextColor tagColor = TextColor.undefined)
    {
        Name = name;
        text = transform.GetComponent<RTLTextMeshPro3D>();
        text.text = Name.ToLower();
        text.font = TMP_Font == null ? text.font : TMP_Font;
        Destination = target;
        forceDirection = (target - this.transform.position).normalized;
        forceMagnitude = Random.Range(min_force_magnitude, max_force_magnitude);
        this.speed = speed;
        Duration = Vector3.Distance(transform.position, target) / speed;
        TagColor = tagColor;
        Tag = new ColorTag(tagColor);
        // SmoothlyTransform();
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

    #region Static Properties

    #endregion
}
