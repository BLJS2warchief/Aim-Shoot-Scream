using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{

    public enum SpawnState {SPAWNING, WAITING, COUNTING};

    [System.Serializable]
    public class Wave
    {
        public string name;
        public Transform enemy;
        public int count;
        public float rate;
    }

    public Wave[] waves;
    private int nextWave = 0;
    public int _NextWave
    {
        get { return nextWave; }
    }

    public float timeBetweenWaves = 5f;
    public float waveCountdown;
    public float _WaveCountdown
    {
        get { return waveCountdown;}
    }

    private float SearchCountDown = 1f;

    private SpawnState state = SpawnState.COUNTING;

    public SpawnState _State
    {
        get { return state; }
    }

    void Start()
    {
        waveCountdown = timeBetweenWaves;
    }

    void Update()
    {

        if(state == SpawnState.WAITING)
        {
            if (!EnemyIsAlive())
            {
                WaveCompleted();
            }
            else
            {
                return;
            }
        }

        if(waveCountdown <= 0)
        {
            if(state != SpawnState.SPAWNING)
            {
                StartCoroutine( SpawnWave(waves[nextWave]) );
            }
        }
        else
        {
            waveCountdown -= Time.deltaTime;
        }
    }

    void WaveCompleted()
    {
        Debug.Log("Wave Completed");

        state = SpawnState.COUNTING;
        waveCountdown = timeBetweenWaves;
        if(nextWave + 1 == waves.Length)
        {
            nextWave = -1;
            Debug.Log("All Waves completed, Ready For a Remach......?");
        }
        nextWave++;
    }

    bool EnemyIsAlive()
    {
        SearchCountDown -= Time.deltaTime;
        if (SearchCountDown <= 0f)
        {
            SearchCountDown = 1f;
            if (GameObject.FindGameObjectWithTag("Enemy") == null)
            {
                return false;
            }
        }
        return true;
    }

    IEnumerator SpawnWave(Wave _wave)
    {
        Debug.Log("Spawning Wave" + _wave.name);
        state = SpawnState.SPAWNING;

        for(int i = 0; i < _wave.count; i++)
        {
            SpawnEnemy(_wave.enemy);
            yield return new WaitForSeconds(1f / _wave.rate);
        }

        state = SpawnState.WAITING;
        yield break;
    }
    void SpawnEnemy(Transform _enemy)
    {
        Debug.Log("Spawning Enemy" + _enemy.name);
        Instantiate(_enemy, new Vector3(transform.position.x + Random.Range(-25, 25), transform.position.y + Random.Range(15, 25), transform.position.z), Quaternion.identity);
    }

}