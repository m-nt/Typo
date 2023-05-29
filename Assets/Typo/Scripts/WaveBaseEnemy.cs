using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class WaveBaseEnemy : EnemyManager
{

    public float Delay = 3;
    public float WaveDelay = 3;
    public float DelaySteps = 0.01f;
    public float DelayTreshold = 0.5f;
    private int EnemyCount = 0;
    public int EnemyLength;

    public int Wave;
    public string[] currentWave = new string[0];
    private Coroutine Spawner;



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
            Enemies.words.RandomValues(EnemyCount, EnemyLength, ref currentWave);
            wave = 1; // This means wave added by one and text updated as well
            for (int i = 0; i < currentWave.Length; i++)
            {
                CreateEnemy(currentWave[i]);
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
