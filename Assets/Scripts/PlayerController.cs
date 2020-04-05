using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public static int loadScene;

    private static bool isInit = false;
    private static Sprite spriteBedOn;
    private static GameObject bar;
    private static GameObject blood;
    private static AudioClip audioBed;
    private static AudioClip audioObstacle;

    private Camera cam;
    private float minCam, maxCam;
    private SpriteRenderer renderBed;
    private AudioSource audio;

    private string[] animationName = { "Stand", "Move", "Dead" };
    private int state = 0, drt = 1;
    private Animator animator;
    private bool isBlowing = false;
    private Rigidbody2D rb;
    private bool isPlaying = true;

    void Start()
    {
        if (!isInit)
        {
            isInit = true;
            spriteBedOn = Resources.Load<Sprite>("Sprites/Bed_On");
            bar = Resources.Load<GameObject>("Prefabs/Bar");
            blood = Resources.Load<GameObject>("Prefabs/Blood");
            audioBed = Resources.Load<AudioClip>("Sounds/Bed");
            audioObstacle = Resources.Load<AudioClip>("Sounds/Obstacle");
        }

        loadScene = SceneManager.GetActiveScene().buildIndex;

        renderBed = GameObject.Find("Bed").GetComponent<SpriteRenderer>();
        GameObject mainCamera = GameObject.Find("MainCamera");
        cam = mainCamera.GetComponent<Camera>();
        audio = mainCamera.GetComponent<AudioSource>();

        float camLeft = -cam.pixelWidth / 200f;
        float camRight = -camLeft;
        GameObject bg = GameObject.Find("Background");
        float bgLeft = bg.transform.position.x - bg.transform.localScale.x / 2;
        float bgRight = bg.transform.position.x + bg.transform.localScale.x / 2;
        minCam = bgLeft - camLeft;
        maxCam = bgRight - camRight;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    public bool IsPlaying()
    {
        return isPlaying;
    }

    public void Blow()
    {
        if (isBlowing)
        {
            rb.AddForce(Vector2.up * 0.8f);
        }
    }

    public void Move(int key)
    {
        if (key == 1)
        {
            rb.AddForce(Vector2.left * 3f);
            drt = 0;
            if (state != 1)
            {
                audio.Play();
                state = 1;
                animator.Play(animationName[state]);
            }
        }
        else if (key == 2)
        {
            rb.AddForce(Vector2.right * 3f);
            drt = 1;
            if (state != 1)
            {
                audio.Play();
                state = 1;
                animator.Play(animationName[state]);
            }
        }
        else
        {
            audio.Stop();
            if (state == 1)
            {
                state = 0;
                animator.Play(animationName[state]);
            }
        }
        float camPos = Mathf.Clamp(transform.position.x, minCam, maxCam);
        cam.transform.position = new Vector3(camPos, 0f, -10f);
        transform.localScale = new Vector3(2 * drt - 1, 1f, 1f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Vent")
        {
            isBlowing = true;
        }
        else if (collision.gameObject.tag == "Goal" && isPlaying)
        {
            isPlaying = false;
            Destroy(gameObject);
            renderBed.sprite = spriteBedOn;
            Debug.Log(cam.transform.position.x);
            Instantiate(bar, cam.transform);
            int nextLevel = SceneManager.GetActiveScene().buildIndex + 1;
            if (nextLevel < SceneManager.sceneCountInBuildSettings)
                loadScene = nextLevel;
            else
                loadScene = 0;
            audio.Stop();
            audio.PlayOneShot(audioBed);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Vent")
        {
            isBlowing = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            isPlaying = false;
            state = 2;
            animator.Play(animationName[state]);
            ParticleSystem particle = transform.GetComponentInChildren<ParticleSystem>();
            Destroy(particle);
            BoxCollider2D box = GetComponent<BoxCollider2D>();
            Destroy(box);
            rb.velocity = new Vector2(0f, 0f);
            Instantiate(blood, gameObject.transform);
            Instantiate(bar, cam.transform);
            audio.Stop();
            audio.PlayOneShot(audioObstacle);
        }
    }
}
