using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class Controls : MonoBehaviour
{
    //Editor Settings
    public float iFrameTime;
    public int maxHitpoints;
    public int platformSpacing;
    public float movementSpeed;
    public float jumpHeight;
    public float tapPadding;
    public float gameSpeedRate;
    public float fireEveryXSeconds;
    public Transform[] attackSpawnPositions;
    public GameObject[] attackPrefabs;
    public Camera mainCam;

    //Game Settings
    private int currentHP = 5;
    private int playerPlacement;
    private float iFrames;
    private float score;
    private float highScore;
    private float cameraPosition;
    private float targetPosition;
    private float heightTarget;
    private bool landed;
    public float projectileSpeed;
    private GameObject[] activeProjectiles;
    private GameObject[] activeCollectibles;
    private float timer;
    public AudioSource playerAudio;
    public AudioClip playerJump;
    public AudioClip enemyShoot;
    public AudioClip playerHit;
    public TextMeshProUGUI scoreUI;
    public TextMeshProUGUI healthUI;

    //GameManager Menus
    public GameObject menuPause;
    public GameObject menuTitle;
    public GameObject menuFail;
    public GameObject menuGame;
    public bool paused;
    private bool mainMenu;
    private bool gameOver;

    void Start()
    {
        Booleans(true, false, false);
        playerAudio = GetComponent<AudioSource>();
    }

    void Update()
    {
        //Show correct menus
        menuTitle.SetActive(mainMenu);
        menuPause.SetActive(paused && !gameOver);
        menuFail.SetActive(gameOver);
        menuGame.SetActive(!mainMenu && !paused && !gameOver);

        //Died
        if (currentHP == 0) {
            Booleans(false, false, true);
            Clear();
            if (score > highScore) highScore = score;
        }

        //Gameplay
        if (!paused && !mainMenu && !gameOver) {
            //Tap left of player to jump left, tap right of player to jump right
            //Will jump in place if in end lane
            cameraPosition = mainCam.WorldToScreenPoint(transform.position).x;
            if (landed) {
                if (Input.touchCount > 0) {
                    Touch press = Input.GetTouch(0);
                    Movement(press.position.x);
                } else if (Input.GetMouseButtonDown(0)) {
                    Movement(Input.mousePosition.x);
                }
            } else {
                //Raycast to detect ground
                if (Physics.Raycast(transform.position, Vector3.down, 1.01f) && transform.position.y < 0.05f) {
                    landed = true;
                }
            }

            //Fire projectiles
            if (timer / fireEveryXSeconds >= 1) {
                Instantiate(attackPrefabs[Random.Range(0, attackPrefabs.Length)], attackSpawnPositions[Random.Range(0, attackSpawnPositions.Length)].position, Quaternion.identity);
                playerAudio.PlayOneShot(enemyShoot, 1f);
                timer = 0;
            }

            //Move towards target
            float moveX = Mathf.MoveTowards(transform.position.x, targetPosition, movementSpeed * Time.deltaTime);
            float moveY = Mathf.MoveTowards(transform.position.y, heightTarget, (jumpHeight - transform.position.y) * Time.deltaTime * 4);
            transform.position = new Vector3(moveX, moveY, 0);
            if (jumpHeight - transform.position.y < 0.5f) { heightTarget = 0; }

            //Reduce the iframes every frame to 0 while playing
            iFrames = Mathf.Clamp(iFrames - Time.deltaTime, 0, iFrameTime);
            projectileSpeed += (gameSpeedRate * Time.deltaTime);
            score += Time.deltaTime;
            timer += Time.deltaTime;
        }
        scoreUI.text = "Score: " + score.ToString("0.0");
        healthUI.text = "Lives: " + currentHP.ToString();
    }

    //Set desired booleans
    private void Booleans(bool m, bool p, bool o)
    {
        mainMenu = m;
        paused = p;
        gameOver = o;
    }

    //Clear projectiles
    private void Clear()
    {
        activeProjectiles = GameObject.FindGameObjectsWithTag("Damage");
        activeCollectibles = GameObject.FindGameObjectsWithTag("Heal");
        foreach (GameObject projectile in activeProjectiles) Destroy(projectile);
        foreach (GameObject collect in activeCollectibles) Destroy(collect);
    }

    //Add upward force and assign target location to move to
    private void Forces()
    {
        playerAudio.PlayOneShot(playerJump, 1f);
        heightTarget = jumpHeight;
        targetPosition = (playerPlacement - 1) * platformSpacing;
    }

    //Move player based on where tap/click happened
    private void Movement(float coordX)
    {
        landed = false;
        if (coordX > cameraPosition + tapPadding) {
            playerPlacement = Mathf.Clamp(playerPlacement + 1, 0, 2);
        } else if (coordX < cameraPosition - tapPadding) {
            playerPlacement = Mathf.Clamp(playerPlacement - 1, 0, 2);
        }
        Forces();
    }

    //Collsion of enemy attacks
    void OnTriggerEnter(Collider hit)
    {
        //Damage/heal depending on tag of collider
        if (hit.CompareTag("Damage") && iFrames == 0) {
            currentHP = Mathf.Clamp(currentHP - 1, 0, maxHitpoints);
            iFrames = iFrameTime;
            playerAudio.PlayOneShot(playerHit, 0.5f);
            

        } else if (hit.CompareTag("Heal")) {
            currentHP = Mathf.Clamp(currentHP + 1, 0, maxHitpoints);
        }
    }

    //Reset game to begining
    private void Reset()
    {
        Clear();
        playerPlacement = 1;
        currentHP = maxHitpoints;
        score = 0;
        iFrames = 0;
        projectileSpeed = 0;
        Booleans(false, false, false);
    }

    //Button Functions
    //Start/Reset game
    public void Restart()
    {
        Reset();
    }

    //Return to the main menu
    public void TitleScreen()
    {
        Booleans(true, false, false);
        Clear();
    }

    //Pause game: True / Unpause Game: false
    public void PauseUnPause(bool onOff)
    {
        Booleans(false, onOff, false);
    }

    public void quitGame()
    {
        Application.Quit();
    }

}