using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager self;
    public GameObject SelectedEnemy;
    public Transform[] SpawnPoints;
    public Transform[] Targets;

    public Transform SpawnParent;
    // Test, Remove later
    public DictionaryWords Enemies;
    // Test, Remove later
    public GameObject EnemyPrefab;
    public float Delay = 3;
    public float WaveDelay = 3;
    public float DelaySteps = 0.01f;
    public float DelayTreshold = 0.5f;
    public int EnemyLeft = 0; // how many enemies in current wave left
    private int EnemyCount = 0;

    public int Wave;
    public string[] currentWave = new string[0];
    private Coroutine Spawner;

    void Awake()
    {
        if (self != null) throw new UnityException("Too many EnemyManager");
        self = this;
    }

    public int wave
    {
        set
        {
            Wave += value;
        }
        get
        {
            return Wave;
        }
    }
    public int deadEnemy
    {
        set
        {
            EnemyLeft -= value;
        }
        get
        {
            return EnemyLeft;
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
            yield return new WaitForSeconds(WaveDelay);
            if (EnemyLeft > 0) continue;
            EnemyCount += Random.Range(1, 3);
            EnemyLeft = EnemyCount;
            Enemies.words.RandomValues(EnemyCount, ref currentWave);
            wave = 1; // This means wave added by one and text updated as well
            for (int i = 0; i < currentWave.Length; i++)
            {
                int spawn_index = Random.Range(0, SpawnPoints.Length);
                GameObject obj = Instantiate(EnemyPrefab, SpawnPoints[spawn_index].position, Quaternion.identity, SpawnParent);
                int target_index = Random.Range(0, Targets.Length);
                Enemy enemy = obj.GetComponent<Enemy>();
                enemy.Inintialize(currentWave[i], Random.Range(0.1f, 0.5f), Targets[target_index].position, null, enemy.TagColor);
                if (Delay >= DelayTreshold) Delay -= DelaySteps;

                yield return new WaitForSeconds(Delay);
            }


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
