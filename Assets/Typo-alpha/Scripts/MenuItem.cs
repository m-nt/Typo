using System.Collections;
using System.Collections.Generic;
using RTLTMPro;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public enum ActionType
{
    OPENSCENE,
    OPENMENU,
}
[RequireComponent(typeof(RTLTextMeshPro3D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class MenuItem : MonoBehaviour, IKeyboard
{
    public string EnglishName, FarsiName;
    public TMP_FontAsset EnglishFont, FarsiFont;
    public float accelerationTreshold;
    public float forceMagnitude; // The magnitude of the force to apply
    public Color Primary, Secondary;
    public UnityEvent actions;
    private string Name;
    private RTLTextMeshPro3D text;
    private ColorTag Tag;
    private Rigidbody2D rb;
    private Vector2 forceDirection; // The direction of the force to apply
    private int index;

    // Start is called before the first frame update
    void Start()
    {
        if (KeyboardCaptureV3.self != null)
            KeyboardCaptureV3.self.registeredKeys.AddListener(OnKeyBuiltIn);


        rb = GetComponent<Rigidbody2D>();
        Tag = new ColorTag("#" + ColorUtility.ToHtmlStringRGB(Secondary));
        text = GetComponent<RTLTextMeshPro3D>();
        Name = GlobalValues.Language switch
        {
            Language.EN => EnglishName,
            Language.FA => FarsiName,
            _ => FarsiName,
        };
        text.text = Name;
        text.font = GlobalValues.Language switch
        {
            Language.EN => EnglishFont,
            Language.FA => FarsiFont,
            _ => FarsiFont,
        };
        gameObject.name = Name;
    }
    void OnEnable()
    {
        Start();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        ContactPoint2D[] contacts = new ContactPoint2D[collision.contactCount];
        collision.GetContacts(contacts);

        foreach (ContactPoint2D contact in contacts)
        {
            Vector2 normal = contact.normal;
            forceDirection = new Vector2(Mathf.Abs(forceDirection.x) * normal.x, Mathf.Abs(forceDirection.y) * normal.y);
        }
    }
    public void AddForce(Vector2 direction)
    {
        forceDirection = direction.normalized;
        Debug.LogWarning(forceDirection);
        rb.AddForce(forceDirection * forceMagnitude, ForceMode2D.Force);
    }
    // Update is called once per frame
    void FixedUpdate()
    {

        if (Input.acceleration.sqrMagnitude > accelerationTreshold)
        {
            AddForce(Input.acceleration);
        }

    }
    public void OnKey(KeyboardType key)
    {
        KeyboardEventHandler(key.Key);
    }
    public void OnKeyBuiltIn(string key)
    {
        KeyboardEventHandler(key);
    }
    void KeyboardEventHandler(string keyType)
    {
        if (!gameObject.activeInHierarchy) return;

        // Check if the characters of the enemy is filled, prevent overflow error
        if (index >= Name.Length) { ResetObject(); return; }
        // Check if the key is BackSpace then reset the state
        if (keyType.ToUpper() == "BACKSPACE" && index > 0) { ResetObject(); return; }
        // Check if the key clicked/touched is the corresponding character of the enemy
        if (Name[index].ToString().ToUpper() != keyType.ToUpper()) return;
        if (index <= 0)
        {
            // if selected enemy in enemy manager is null asign this gameObject to the selected enemy
            MenuManager.self.SelectedItem = MenuManager.self.SelectedItem == null ? gameObject : MenuManager.self.SelectedItem;
        }
        // Check is this enemy is selected or not
        if (MenuManager.self.SelectedItem != gameObject) return;
        // add the tag to the corresponding character
        text.add_tag(Tag.Tag, Tag.Seperator, index * Tag.Height);
        index++;
        if (index < Name.Length) return;

        // what gonna happens when the object is selected
        MenuManager.self.SelectedItem = null;
        actions.Invoke();
    }
    void ResetObject()
    {
        Debug.Log(Name);
        MenuManager.self.SelectedItem = null;
        index = 0;
        text.text = Name;
    }
}
