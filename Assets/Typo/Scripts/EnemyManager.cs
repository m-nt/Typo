using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager self;
    public GameObject SelectedEnemy;
    public Transform[] SpawnPoints;
    public Transform Target;
    public Transform SpawnParent;
    // Test, Remove later
    public DictionaryWords Enemies;
    // Test, Remove later
    public GameObject EnemyPrefab;
    public float Delay = 3;
    private Coroutine Spawner;

    void Awake()
    {
        if (self != null) throw new UnityException("Too many EnemyManager");
        self = this;
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
            yield return new WaitForSeconds(Delay);
            int spawn_index = Random.Range(0, SpawnPoints.Length);
            GameObject obj = Instantiate(EnemyPrefab, SpawnPoints[spawn_index].position, Quaternion.identity, SpawnParent);
            obj.GetComponent<Enemy>().Inintialize(Enemies.words.RandomValue, Random.Range(0.1f, 0.5f), Target.position);
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
