using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveUI : MonoBehaviour
{

    [SerializeField]
    WaveSpawner spawner;

    [SerializeField]
    Animator WaveAnimator;

    [SerializeField]
    Text WaveCountdownText;

    [SerializeField]
    Text WaveCountText;

    private WaveSpawner.SpawnState previousState;

    // Start is called before the first frame update
    void Start()
    {
        if(spawner == null)
        {
            Debug.LogError("No spawner referenced");
            this.enabled = false;
        }
        if (WaveAnimator == null)
        {
            Debug.LogError("No WaveAnimator referenced");
            this.enabled = false;
        }

        if (WaveCountdownText == null)
        {
            Debug.LogError("No WaveCountdownText referenced");
            this.enabled = false;
        }

        if (WaveCountText == null)
        {
            Debug.LogError("No WaveCountText referenced");
            this.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (spawner._State)
        {
            case WaveSpawner.SpawnState.COUNTING:
                UpdateCountingUI();
                break;
            case WaveSpawner.SpawnState.SPAWNING:
                UpdateSpawningUI();
                break;
        }
        previousState = spawner._State;
    }

    void UpdateCountingUI()
    {
        if(previousState != WaveSpawner.SpawnState.COUNTING)
        {
            WaveAnimator.SetBool("WaveIncoming", false);
            WaveAnimator.SetBool("WaveCountdown", true);

            //Debug.Log("COUNTING");
        }
        WaveCountdownText.text = ((int) spawner.waveCountdown + 1).ToString();
    }
    void UpdateSpawningUI()
    {
        if (previousState != WaveSpawner.SpawnState.SPAWNING)
        {
            WaveAnimator.SetBool("WaveCountdown", false);
            WaveAnimator.SetBool("WaveIncoming", true);
            WaveCountText.text = (spawner._NextWave + 1).ToString();
            //Debug.Log("SPAWNING");
        }
    }
}
