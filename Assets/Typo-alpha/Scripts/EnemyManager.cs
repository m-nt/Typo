using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager self;
    public GameObject SelectedEnemy;
    public Transform[] SpawnPoints;
    public Transform Target;
    public RTLTMPro.RTLTextMeshPro ScoreText;
    public string ScoreTextTemplate;
    private int Score;
    public Transform SpawnParent;
    // Test, Remove later
    public DictionaryWords Enemies;
    // Test, Remove later
    public GameObject EnemyPrefab;
    public float Delay = 3;
    public float DelaySteps = 0.01f;
    public float DelayTreshold = 0.5f;
    private Coroutine Spawner;

    void Awake()
    {
        if (self != null) throw new UnityException("Too many EnemyManager");
        self = this;
    }
    public int score
    {
        set
        {
            Score += value;
            ScoreText.text = ScoreTextTemplate + Score;
        }
        get
        {
            return Score;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        Spawner = StartCoroutine(spwnEnemies());
    }
    // Test, Remove later
    IEnumerator spwnEnemies()
    {
        while (true)
        {
            int spawn_index = Random.Range(0, SpawnPoints.Length);
            GameObject obj = Instantiate(EnemyPrefab, SpawnPoints[spawn_index].position, Quaternion.identity, SpawnParent);
            obj.GetComponent<Enemy>().Inintialize(Enemies.words.RandomValue, Random.Range(0.1f, 0.5f), Target.position);
            if (Delay >= DelayTreshold) Delay -= DelaySteps;
            yield return new WaitForSeconds(Delay);
        }

    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StopCoroutine(Spawner);
        }
    }
}
