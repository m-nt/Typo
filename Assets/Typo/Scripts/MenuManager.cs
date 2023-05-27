using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Language
{
    EN,
    FA
}

public class MenuManager : MonoBehaviour
{
    public static MenuManager self;
    public CameraCollider _camera;
    public GameObject SelectedItem;
    public GameObject Analitics, MainMenu;


    void Awake()
    {
        if (self != null) throw new UnityException("Too many MenuManager instances");
        self = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        _camera.SetColliders();
    }
    IEnumerator InitiationEnumerator()
    {
        yield return new WaitForSeconds(1);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OpenNewWaveGame()
    {
        SceneManager.LoadScene("Enemy V2" + (GlobalValues.Language == Language.EN ? "EN" : "FA"));
    }
    public void OpenAnalytics(GameObject from)
    {
        Analitics.SetActive(true);
        from.SetActive(false);
    }
    public void OpenMainMenu(GameObject from)
    {
        MainMenu.SetActive(true);
        from.SetActive(false);
    }
}

[System.Serializable]
public class CameraCollider
{
    public BoxCollider2D top, down, right, left;

    public Camera camera;

    public void SetColliders()
    {
        float screenAspect = (float)Screen.width / (float)Screen.height;
        if (!camera) camera = Camera.main;
        float cameraHeight = camera.orthographicSize * 2;
        float cameraWidth = cameraHeight * screenAspect;

        if (!top) top = MenuManager.self.gameObject.AddComponent<BoxCollider2D>();
        if (!down) down = MenuManager.self.gameObject.AddComponent<BoxCollider2D>();
        if (!right) right = MenuManager.self.gameObject.AddComponent<BoxCollider2D>();
        if (!left) left = MenuManager.self.gameObject.AddComponent<BoxCollider2D>();

        top.size = new Vector2(cameraWidth, cameraHeight / 6);
        down.size = new Vector2(cameraWidth, cameraHeight / 6);
        right.size = new Vector2(cameraWidth / 6, cameraHeight);
        left.size = new Vector2(cameraWidth / 6, cameraHeight);

        top.offset = new Vector2(0, cameraHeight / 2 + top.size.y / 2);
        down.offset = new Vector2(0, -cameraHeight / 2 - down.size.y / 2);
        left.offset = new Vector2(-cameraWidth / 2 - left.size.x / 2, 0);
        right.offset = new Vector2(cameraWidth / 2 + right.size.x / 2, 0);

    }

}