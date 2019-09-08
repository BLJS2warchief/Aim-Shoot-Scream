﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [System.Serializable]
    public class EnemyStats {
        public int maxHealth = 100;

        private int _curHealth;
        public int curHealth
        {
            get { return _curHealth; }
            set { _curHealth = Mathf.Clamp(value, 0, maxHealth); }
        }

        public int damage = 40;

        public void Init()
        {
            curHealth = maxHealth;
        }
    }
    public EnemyStats Stats = new EnemyStats();

    public Transform DeathParticles;

    public float shakeAmt = 1f;
    public float shakeLength = 0.5f;

    public string deathSoundName = "Explosion";

    [Header("Optional : ")]
    [SerializeField]
    private StatusIndicator statusIndicator;

    void Start()
    {
        Stats.Init();
        if(statusIndicator != null)
        {
            statusIndicator.SetHealth(Stats.curHealth, Stats.maxHealth);
        }
    }

    public void DamageEnemy(int damage){
        Stats.curHealth -= damage;
        if(Stats.curHealth <= 0){
            Debug.Log("Enemy Killed");
            GameMaster.KillEnemy(this);
        }

        if (statusIndicator != null)
        {
            statusIndicator.SetHealth(Stats.curHealth, Stats.maxHealth);
        }
        if (DeathParticles == null)
        {
            Debug.Log("No Death PArticles referenced");
        }
    }

    void OnCollisionEnter2D(Collision2D _colInfo)
    {
        Player _player = _colInfo.collider.GetComponent<Player>();
        if(_player != null)
        {
            _player.DamagePlayer(Stats.damage);
            DamageEnemy(999999);
        }

    }
}
