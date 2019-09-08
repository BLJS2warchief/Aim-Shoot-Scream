using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(SpriteRenderer))]

public class tiling : MonoBehaviour
{
    public int offsetX = 2;

    public bool hasARightBuddy = false;
    public bool hasALeftBuddy = false;

    public bool reverseScale = true;
    private float spriteWidth = 0f;
    private Camera cam;
    private Transform myTransform;

    void Awake(){
        cam = Camera.main;
        myTransform = transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer sRenderer = GetComponent <SpriteRenderer> ();
        spriteWidth = sRenderer.sprite.bounds.size.x;
    }

    // Update is called once per frame
    void Update()
    {
        if(hasALeftBuddy == false || hasARightBuddy == false){
            // calculate camera's extend (half of width) of what the cam can see in world coordinates
            float camHorizExtend = cam.orthographicSize * Screen.width / Screen.height;

            // Calculate the x-position where the cam can see the edge of the sprite
            float edgeVisiblePosRight = (myTransform.position.x + spriteWidth / 2)  - camHorizExtend;
            float edgeVisiblePosLeft = (myTransform.position.x - spriteWidth / 2)  + camHorizExtend;

            if(cam.transform.position.x >= edgeVisiblePosRight - offsetX && hasARightBuddy == false){
                makeNewBuddy(1);
                hasARightBuddy = true;
            }
            else if(cam.transform.position.x <= edgeVisiblePosLeft + offsetX && hasALeftBuddy == false){
                makeNewBuddy(-1);
                hasALeftBuddy = true;
            }
        }
    }

    void makeNewBuddy(int rightOrLeft){
        // Calculating position for new buddy
        Vector3 newPosition = new Vector3(myTransform.position.x + spriteWidth * rightOrLeft, myTransform.position.y, myTransform.position.z);
        // Instantiating our new body and storing him in a variable
        Transform newBuddy = Instantiate(myTransform, newPosition, myTransform.rotation) as Transform ;
        if(reverseScale == true){
            newBuddy.localScale = new Vector3 (newBuddy.localScale.x * -1, newBuddy.localScale.y, newBuddy.localScale.z);
        }

        newBuddy.parent = myTransform.parent;
        if(rightOrLeft > 0){
            newBuddy.GetComponent<tiling>().hasALeftBuddy = true;
        }
        else {
            newBuddy.GetComponent<tiling>().hasARightBuddy = true;
        }
    }
}
