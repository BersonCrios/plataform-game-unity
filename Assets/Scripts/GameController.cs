using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Transform playerTransform;

    public Transform camLeftLimit, camRigthLimit, camTopLimit, camBottomLimit;

    private Camera cam;

    public float speedCam;

    [Header("Audio")]
    public AudioSource sfxSource;
    public AudioSource musicSource;

    public AudioClip sfxJump;
    public AudioClip sfxAtack;
    public AudioClip sfxCoin;
    public AudioClip sfxEnemyDeath;
    public AudioClip sfxPlayerDamage;
    public AudioClip[] sfxStep;


    void Start(){
        cam = Camera.main;    
    }

    void Update(){
        
    }

    void LateUpdate() {
        camController();
    }

    void camController() 
    {
        float posCamX = playerTransform.position.x;
        float posCamY = playerTransform.position.y;

        if (cam.transform.position.x < camLeftLimit.position.x && playerTransform.position.x < camLeftLimit.position.x)
        {
            posCamX = camLeftLimit.position.x;
        }
        else if (cam.transform.position.x > camRigthLimit.position.x && playerTransform.position.x > camRigthLimit.position.x)
        {
            posCamX = camRigthLimit.position.x;
        }


        if (cam.transform.position.y < camBottomLimit.position.y && playerTransform.position.y < camBottomLimit.position.y)
        {
            posCamY = camBottomLimit.position.y;
        }
        else if (cam.transform.position.y > camTopLimit.position.y && playerTransform.position.y > camTopLimit.position.y)
        {
            posCamY = camTopLimit.position.y;
        }

        Vector3 posCam = new Vector3(posCamX, posCamY, cam.transform.position.z);
        cam.transform.position = Vector3.Lerp(cam.transform.position, posCam, speedCam * Time.deltaTime);
    }

    public void playSFX(AudioClip sfxClip, float volume)
    {
      sfxSource.PlayOneShot(sfxClip, volume);
    }
}
