using UnityEngine;

public class weapon : MonoBehaviour
{
    public float fireRate = 0;
    public int damage = 10;
    public LayerMask whatToHit;

    public Transform BulletTrailPrefab;
    public Transform muzzleFlashPrefab;
    public Transform HitPrefab;
    float timeToSpawnEffect = 0;
    public float effectSpawnRate = 10;

    public float camShakeAmount = 0.05f;
    public float camShakeLength = 0.1f;
    CameraShake camShake;
    public string weaponShootSound = "DefaultShot";

    float timeToFire = 0;
    Transform firePoint;

    AudioManager audioManager;

    void Awake() {
        firePoint = transform.Find("firePoint");
        if(firePoint == null){
            Debug.LogError("NO FirePoint!!!!! WHAT????!!!!");
        }
    }

    void Start(){
        camShake =  GameMaster.gm.GetComponent<CameraShake>();
        if(camShake == null){
            Debug.Log("No camshake found");
        }
        audioManager = AudioManager.instance;
        if (audioManager == null)
        {
            Debug.LogError("No Audiomanager Found");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(fireRate == 0){
            if(Input.GetButtonDown ("Fire1")){
                shoot();
            }
        }
        else {
            if(Input.GetButton ("Fire1") && Time.time > timeToFire){
                timeToFire = Time.time + 1 / fireRate;
                shoot();
            }
        }
    }
    void shoot(){
        // Debug.Log("Shots Fired");
        Vector2 mousePosition = new Vector2 (Camera.main.ScreenToWorldPoint (Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        Vector2 firePointPosition = new Vector2 (firePoint.position.x, firePoint.position.y);
        RaycastHit2D hit = Physics2D.Raycast (firePointPosition, mousePosition - firePointPosition, 100, whatToHit);
        Debug.DrawLine(firePointPosition, (mousePosition - firePointPosition) * 200, Color.cyan);

        if(hit.collider != null){
            Debug.DrawLine(firePointPosition, hit.point, Color.red);
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            if(enemy != null){
                enemy.DamageEnemy(damage);
                //Debug.Log("We hit " + hit.collider + " and did " + damage + " damage ");
            }
        }

        if(Time.time >= timeToSpawnEffect){

            Vector3 hitPos;
            Vector3 hitNormal;

            if(hit.collider == null){
                Debug.Log("adfghjkl");
                hitPos = (mousePosition - firePointPosition) * 30;
                hitNormal = new Vector3(9999, 9999, 9999);
            }
            else{
                hitPos = hit.point;
                hitNormal = hit.normal;
            }
            TrailEffect(hitPos, hitNormal);

            timeToSpawnEffect = Time.time + 1 / effectSpawnRate;
        }
    }
    void TrailEffect(Vector3 hitPos, Vector3 hitNormal){
        Transform Trail = Instantiate(BulletTrailPrefab, firePoint.position, firePoint.rotation);
        LineRenderer lr = Trail.GetComponent<LineRenderer>();

        if(lr != null){
            lr.SetPosition(0, firePoint.position);
            lr.SetPosition(1, hitPos);
        }

        Destroy(Trail.gameObject, 0.02f);

        if(hitNormal != new Vector3(9999, 9999, 9999)){
            Transform hitParticles = (Transform) Instantiate(HitPrefab, hitPos, Quaternion.FromToRotation(Vector3.right, hitNormal));
            Destroy(hitParticles.gameObject, 1f);
        }

        Transform clone = (Transform) Instantiate(muzzleFlashPrefab, firePoint.position, firePoint.rotation);
        clone.parent = firePoint;
        float size = Random.Range(0.7f, 0.9f);
        clone.localScale = new Vector3 (size, 2 * size / 3, size);
        Destroy (clone.gameObject, 0.02f);

        camShake.Shake(camShakeAmount, camShakeLength);
        audioManager.PlaySound(weaponShootSound);
    }
}
