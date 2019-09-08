using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public Camera mainCam;
    float shakeAmt = 0;

    void Awake(){
        if(mainCam == null){
            mainCam = Camera.main;
        }
    }

    void Update() {
        if(Input.GetKeyDown(KeyCode.T)){
            Shake(0.1f, 0.2f);
        }    
    }

    public void Shake(float amt, float length){
        shakeAmt = amt;
        InvokeRepeating("BeginShake", 0, 0.01f);
        Invoke("StopShake", length);
    }

    void BeginShake(){
        if(shakeAmt > 0){
            Vector3 camPos = mainCam.transform.position;

            float shakeAmtX = Random.value * shakeAmt * 2 - shakeAmt;
            float shakeAmtY = Random.value * shakeAmt * 2 - shakeAmt;

            camPos.x += shakeAmtX / 2;
            camPos.y += shakeAmtY / 2;
            mainCam.transform.position = camPos;
        }
        
    }

    void StopShake(){
        Debug.Log("asdfghjkuytfdcvbnjuygvbnuhb");
        CancelInvoke("BeginShake");
        mainCam.transform.localPosition = Vector3.zero;
    }
}
