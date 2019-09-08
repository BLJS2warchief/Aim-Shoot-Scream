using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [System.Serializable]
    public class PlayerStats {
        public int maxHealth = 100;
        private int _curHealth;
        public int curHealth
        {
            get { return _curHealth; }
            set { _curHealth = Mathf.Clamp(value, 0, maxHealth); }
        }

        public void Init()
        {
            _curHealth = maxHealth;
        }
    }

    public int fallBoundary = -20;

    public string DeathScream = "DeathScream";
    public string grunt = "Grunt";

    private AudioManager audioManager;

    [SerializeField]
    private StatusIndicator statusIndicator;

    void Start()
    {
        stats.Init();
        if(statusIndicator == null)
        {
            Debug.Log("Player ststus indicator not found");
        }
        else
        {
            statusIndicator.SetHealth(stats.curHealth, stats.maxHealth);
        }
        audioManager = AudioManager.instance;
        if (audioManager == null)
        {
            Debug.LogError("No Audiomanager Found");
        }
    }

    public PlayerStats stats = new PlayerStats();

    void Update(){
        if(transform.position.y <= fallBoundary){
            DamagePlayer(999999);
        }
    }

    public void DamagePlayer(int damage){
        stats.curHealth -= damage;
        if(stats.curHealth <= 0){
            audioManager.PlaySound(DeathScream);
            Debug.Log("Player Killed");
            GameMaster.KillPlayer(this);
        }
        else
        {
            audioManager.PlaySound(grunt);
        }
        statusIndicator.SetHealth(stats.curHealth, stats.maxHealth);
    }
}
