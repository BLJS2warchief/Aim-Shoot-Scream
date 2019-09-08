using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parallaxing : MonoBehaviour
{
    public Transform[] backgrounds;             // Array of all the back and fore grounds to be parallaxed;
    private float[] parallaxScales;             // The proportion of the camera's movement to move the backgrounds by
    public float smoothing = 1f;                // How smooth the parallax is going to be, set above 0

    private Transform cam;                      // Reference to the main camera's transform
    private Vector3 previousCamPos;             // Position of the camera in the previous frame

    // Is called before Start(), Great for references
    void Awake() {
        //Set up the cam for reference
        cam = Camera.main.transform;


    }

    // Start is called before the first frame update
    void Start()
    {
        //The previous frame had the current frame's position
        previousCamPos = cam.position;

        //Assigning corresponding parallax scales
        parallaxScales = new float[backgrounds.Length];

        for(int i = 0; i < backgrounds.Length; i++){
            parallaxScales[i] = backgrounds[i].position.z*-1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < backgrounds.Length; i++){
            // Movement of camera multiplied by parallaxScale
            float parallax = (previousCamPos.x - cam.position.x) * parallaxScales[i];

            // Set a target X position i.e current x plus parallax
            float backgroundTargetPosX = backgrounds[i].position.x + parallax;

            // Create a target position which is the background's current position with its target x position
            Vector3 backgroundTargetPos = new Vector3 (backgroundTargetPosX, backgrounds[i].position.y, backgrounds[i].position.z);

            // Fade  between current position and target position with Lerp
            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);
        }
        //Set the previousCamPos to camera pos at end of the frame
        previousCamPos = cam.position;
        
    }
}
