using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class gameController : MonoBehaviour
{

    [Header("Camera")]
    public Transform        playerTransform;
    public Transform        limitCamLeft, limitCamRight, limitCamUp, limitCamDown;
    public float            speedCam;
    private Camera          cam;

    [Header("Audio")]
    public AudioSource sfxSource;
    public AudioSource musicSource;

    public AudioClip sfxJump;
    public AudioClip sfxAttack;
    public AudioClip sfxCoin;
    public AudioClip sfxEnemyDeath;
    public AudioClip[] sfxStep;
    public AudioClip sfxGetHit;
    public GameObject painelGameOver;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        painelGameOver.SetActive(false);
    }

    void Update()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        camController();
    }

    void camController()
    {
        float posCamX = playerTransform.position.x;
        float posCamY = playerTransform.position.y;

        if(cam.transform.position.x < limitCamLeft.position.x && playerTransform.position.x < limitCamLeft.position.x)
        {
            posCamX = limitCamLeft.position.x;
        }
        else if(cam.transform.position.x > limitCamRight.position.x && playerTransform.position.x > limitCamRight.position.x)
        {
            posCamX = limitCamRight.position.x;
        }

        if(cam.transform.position.y < limitCamDown.position.y && playerTransform.position.y < limitCamDown.position.y)
        {
            posCamY = limitCamDown.position.y;
        }
        else if(cam.transform.position.y > limitCamUp.position.y && playerTransform.position.y > limitCamUp.position.y)
        {
            posCamY = limitCamUp.position.y;
        }

        Vector3 posCam = new Vector3(posCamX, posCamY, cam.transform.position.z);

        cam.transform.position = Vector3.Lerp(cam.transform.position, posCam, speedCam * Time.deltaTime);
    }

    public void playSFX(AudioClip sfxClip, float volume)
    {
        sfxSource.PlayOneShot(sfxClip, volume);
    }

    public void getHit()
    {

    }
}
