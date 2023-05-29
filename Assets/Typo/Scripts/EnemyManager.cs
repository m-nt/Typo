using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager self;
    public Transform[] SpawnPoints;
    public Transform[] Targets;
    public DictionaryWords Enemies;
    public GameObject EnemyPrefab;

    public Transform SpawnParent;

    public GameObject SelectedEnemy;
    public int EnemyLeft;
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


    void Awake()
    {
        if (self != null) throw new UnityException("Too many EnemyManager");
        self = this;
    }

    public void CreateEnemy(string name)
    {
        int spawn_index = Random.Range(0, SpawnPoints.Length);
        GameObject obj = Instantiate(EnemyPrefab, SpawnPoints[spawn_index].position, Quaternion.identity, SpawnParent);
        obj.name = name;
        int target_index = Random.Range(0, Targets.Length);
        Enemy enemy = obj.GetComponent<Enemy>();
        enemy.Inintialize(name, Random.Range(0.1f, 0.5f), Targets[target_index].position, null, enemy.TagColor);
    }
    public void Get_random_name()
    {
        // Enemies?.
    }
}
