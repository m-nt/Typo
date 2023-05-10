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
    public string[] Enemies;
    // Test, Remove later
    public GameObject EnemyPrefab;

    void Awake()
    {
        if (self != null) throw new UnityException("Too many EnemyManager");
        self = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(spwnEnemies());
    }
    // Test, Remove later
    IEnumerator spwnEnemies()
    {
        for (int i = 0; i < Enemies.Length; i++)
        {
            yield return new WaitForSeconds(1);
            int spawn_index = Random.Range(0, SpawnPoints.Length);
            GameObject obj = Instantiate(EnemyPrefab, SpawnPoints[spawn_index].position, Quaternion.identity, SpawnParent);
            obj.GetComponent<Enemy>().Inintialize(Enemies[i], Random.Range(0.1f, 0.5f), Target.position);
        }

    }
    // Update is called once per frame
    void Update()
    {

    }
}
