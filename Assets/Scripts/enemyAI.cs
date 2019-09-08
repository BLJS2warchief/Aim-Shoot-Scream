using UnityEngine;
using System.Collections;
using Pathfinding;

[RequireComponent(typeof(Seeker))]
[RequireComponent(typeof(Rigidbody2D))]

public class enemyAI : MonoBehaviour
{
    public Transform target;
    public float updateRate = 2f;
    private Seeker seeker;
    private Rigidbody2D rb;

    // Calculated path
    public Path path;

    // AI's speed per second
    public float speed = 300f;
    public ForceMode2D fMode;

    [HideInInspector]
    public bool PAthIsEnded = false;
    public float nextWavePointDistance = 3;
    private int currentWavePoint;

    private bool searchingForPlayer;
    
    void Start(){
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        if(target == null){
            if(!searchingForPlayer){
                searchingForPlayer = true;
                StartCoroutine(searchForPlayer());
            }
            Debug.Log("This is a time to panic as there is no player");
            return;
        }
        seeker.StartPath(transform.position, target.position, OnPathComplete);

        StartCoroutine(UpdatePath());

    }

    IEnumerator searchForPlayer(){
        GameObject searchResult = GameObject.FindGameObjectWithTag("Player");
        if(searchResult == null){
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(searchForPlayer());
        }
        else
        {
            target = searchResult.transform;
            searchingForPlayer = false;
            StartCoroutine(UpdatePath());
            yield return false;
        }
    }

    IEnumerator UpdatePath(){
        if(target == null){
            if(!searchingForPlayer){
                searchingForPlayer = true;
                StartCoroutine(searchForPlayer());
            }
            Debug.Log("This is a time to panic as there is no player");
            yield return false;
        }
        seeker.StartPath(transform.position, target.position, OnPathComplete);

        yield return new WaitForSeconds(1 / updateRate); 
        StartCoroutine(UpdatePath());
    }
    public void OnPathComplete(Path p){
        Debug.Log("We got a path error: " + p.error);
        path = p;
        currentWavePoint = 0;

    }
    void FixedUpdate(){
        if(target == null){
            if(!searchingForPlayer){
                searchingForPlayer = true;
                StartCoroutine(searchForPlayer());
            }
            Debug.Log("This is a time to panic as there is no player");
            return;
        }

        if(path == null){
            Debug.Log("BAD NEWS AGAIN");
            return;
        }

        if(currentWavePoint >= path.vectorPath.Count){
            if(PAthIsEnded)
            {
                Debug.Log("BAd News");
                return;
            }

            Debug.Log("EndOfPathReached");
            PAthIsEnded = true;
            return;
        }

        PAthIsEnded = false;

        Vector3 dir = (path.vectorPath[currentWavePoint] - transform.position).normalized;
        dir *= speed * Time.fixedDeltaTime;

        rb.AddForce(dir, fMode);
        Debug.Log("waveForce");

        float dist = Vector3.Distance(transform.position, path.vectorPath[currentWavePoint]);

        if(dist < nextWavePointDistance){
            currentWavePoint++;
            return;
        }
    }




}
