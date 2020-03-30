using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;

    float movHorizontal, movVertical;
    public float velocidad = 1.0f;
    public float altitud = 100.0f;
    public bool isJump = false;
    int stars; int lifes=3;
    public GameObject puntoInicio, panelGameOver,panelContratulations,uiMobile;
    float totalTime = 120f;
    bool pause = false;
    public MenuManager menuManager;
    AudioSource audioPlayer;
    public AudioClip sonidoPuntos,JumpSound,DeadSound;
    public bl_Joystick joystick;

      public Text lifesText, starsText, timeText,finalLifesText,finalStarsText;
    // Start is called before the first frame update
    void Start()
    {
      #if UNITY_ANDROID
        uiMobile.SetActive(true);
        #endif



     rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!pause){

        
        CountDown();
        }
        
        #if UNITY_ANDROID
        movVertical = joystick.Vertical * 0.12f;
        movHorizontal = joystick.Horizontal * 0.12f;
        #endif

        //movVertical = Input.GetAxis("Vertical");
        //movHorizontal = Input.GetAxis("Horizontal");
        //print(movVertical);

        Vector3 movimiento = new Vector3(movHorizontal, 0.0f, movVertical); 

        rb.AddForce(movimiento * velocidad);

        if(Input.GetKey(KeyCode.Space) && (!isJump)){

           Jump();
        }
    }
    void OnCollisionEnter(Collision collision){
        if(collision.gameObject.name == "Floor" || collision.gameObject.name == "Float"){
            isJump = false;
        }
    }
    void OnTriggerEnter(Collider collider){
        if(collider.gameObject.name == "Diamante"){
            Destroy(collider.gameObject);
                stars+=1;
            starsText.text = "0" +stars.ToString();
            GetComponent<AudioSource>().clip = sonidoPuntos;
            GetComponent<AudioSource>().Play();
                
        }

        if((collider.gameObject.name == "DeathZone")|| (collider.gameObject.name == "AXE")){

            transform.position = puntoInicio.transform.position;

            lifes -=1;
            lifesText.text= "0" + lifes.ToString();
            if(lifes == 0){
                GameOverGame();
            }
             GetComponent<AudioSource>().clip = DeadSound;
            GetComponent<AudioSource>().Play();
            
        }
        if(collider.gameObject.name == "Final"){
            FinishedGame();
        }
    }
    void CountDown(){
        totalTime -= Time.deltaTime;
        int minutes = Mathf.FloorToInt(totalTime / 60f);
        int seconds = Mathf.FloorToInt(totalTime - (minutes * 60));

        timeText.text = string.Format("{0:0}:{01:00}", minutes,seconds);
        if((minutes == 0)&&(seconds== 0)){
            GameOverGame();
        }

    }
    public void PauseGame(){
        pause = !pause;
        rb.isKinematic = pause;
    }
    void GameOverGame(){
        menuManager.GoToMenu(panelGameOver);
        PauseGame();
    }
    public void RestartGame(){
        totalTime = 120f;
        lifes = 3;
        stars = 0;
        lifesText.text = "03";
        starsText.text = "00";
        rb.isKinematic = false;
        pause = false;
    }
    void FinishedGame(){
        PauseGame();
        menuManager.GoToMenu(panelContratulations);
        finalLifesText.text = "0" + lifes.ToString();
        finalStarsText.text = "0" + stars.ToString();
    }
    public void Jump(){
         Vector3 salto = new Vector3(0,altitud,0);
            rb.AddForce(salto * velocidad);
            isJump = true;
             GetComponent<AudioSource>().clip = JumpSound;
            GetComponent<AudioSource>().Play();
    }
}
