using UnityEngine;
using RTLTMPro;
using System.Collections;
using TMPro;


[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(RTLTextMeshPro3D))]
[RequireComponent(typeof(Animator))]
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
    public float DestroyDelay;

    #endregion
    # region Private Variables
    private RTLTextMeshPro3D text;
    private float forceMagnitude; // The magnitude of the force to apply

    private ColorTag Tag;
    private int index;
    private Animator _Animator;


    private Rigidbody2D rb;
    #endregion

    #region MonoBehaviour
    // Start is called before the first frame update
    void Start()
    {
        KeyboardCapture.self?.registeredKeys.AddListener(OnKeyboardEventHandler);
        // Test, remove later
        // Inintialize("hello", 0.5f, new Vector3(0, -3, -5));
        rb = GetComponent<Rigidbody2D>();
        _Animator = GetComponent<Animator>();
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
        StartCoroutine(FinishLine(other));
    }

    private IEnumerator FinishLine(Collider2D other)
    {
        yield return new WaitForSeconds(DestroyDelay);
        if (other.gameObject.tag == "Finish")
        {
            if (GlobalValues.Score > 0) GlobalValues.Score--;
            _Animator.SetBool("is_destroy", true);
        }
    }
    #endregion

    #region Public Properties
    public void DestroyThisGameObject()
    {
        Destroy(gameObject);
    }

    public void OnKeyboardEventHandler(string keyType)
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
        GetComponentInChildren<SpriteRenderer>().color = Secondary;
        // add the tag to the corresponding character
        text.add_tag(Tag.Tag, Tag.Seperator, index * Tag.Height);
        index++;
        // CharPerSecond.Count++;
        if (index < Name.Length) return;
        // what gonna happens to the Enemy
        GlobalValues.Score++;
        // WordPerMinute.Count++;
        _Animator.SetBool("is_destroy", true);
        // TODO: add destroy affect
    }

    public void Inintialize(string name, float speed, Vector3 target, TMP_FontAsset TMP_Font = null, TextColor tagColor = TextColor.undefined)
    {
        Name = name;
        text = transform.GetComponent<RTLTextMeshPro3D>();
        text.text = Name.ToLower();
        text.font = TMP_Font == null ? text.font : TMP_Font;
        forceDirection = (target - this.transform.position).normalized;
        forceMagnitude = Random.Range(min_force_magnitude, max_force_magnitude);
        this.speed = speed;
        TagColor = tagColor;
        Tag = new ColorTag(tagColor);
    }

    #endregion

    #region Static Properties

    #endregion
}
