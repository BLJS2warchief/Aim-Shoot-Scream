using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public static GameMaster gm;

    private static int remainingLives = 3;
    public static int _RemainingLives
    {
        get { return remainingLives; }
    }

    void Awake(){
        remainingLives = 3;
        if(gm == null){
            gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        }
    }

    public Transform playerPrefab;
    public Transform SpawnPoint;
    public int SpawnDelay = 2;
    public Transform spawnParticles;
    public string spawnSoundName;

    public CameraShake cameraShake;

    [SerializeField]
    public GameObject gameOverUI;

    public string GameOverSound = "GameOver";

    public Transform enemyDeathParticles;

    private AudioManager audioManager;

    void Start()
    {

        if(cameraShake == null)
        {
            Debug.LogError("No camera shake referenced in gamemaster");
        }
        audioManager = AudioManager.instance;
        if(audioManager == null)
        {
            Debug.LogError("No audio manager found");
        }
    }

    public void EndGame()
    {
        Debug.Log("GAME OVER");
        audioManager.PlaySound(GameOverSound);
        gameOverUI.SetActive(true);
    }

    public IEnumerator RespawnPlayer(){
        audioManager.PlaySound(spawnSoundName);
        yield return new WaitForSeconds(SpawnDelay);

        Instantiate (playerPrefab, SpawnPoint.position, SpawnPoint.rotation);
        Transform spawnParticleClone = (Transform) Instantiate (spawnParticles, SpawnPoint.position, SpawnPoint.rotation);
        Destroy(spawnParticleClone.transform.gameObject, 3f);
    }

    public static void KillPlayer(Player player){
        Destroy(player.gameObject);
        remainingLives--;
        if(remainingLives <= 0)
        {
            gm.EndGame();
        }
        else
        {
            gm.StartCoroutine(gm.RespawnPlayer());
        }
    }

    public static void KillEnemy(Enemy enemy){
        gm._killEnemy(enemy);
    }

    public void _killEnemy(Enemy _enemy)
    {
        audioManager.PlaySound(_enemy.deathSoundName);

        Transform _clone = Instantiate(_enemy.DeathParticles, _enemy.transform.position, Quaternion.identity) as Transform;
        Destroy(_clone.gameObject, 5f);

        cameraShake.Shake(_enemy.shakeAmt, _enemy.shakeLength);
        Destroy(_enemy.gameObject);
    }
}
