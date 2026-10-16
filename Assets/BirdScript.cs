using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class BirdScript : MonoBehaviour
{
    private Rigidbody2D myRigidbody;
    private LogicScript logic;
    private bool gameIsOver;
    private AudioSource flapAudioSource;
    private AudioClip flapSound;
    private Collider2D birdCollider;
    public float flapStrength = 5f;

    private void Awake()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        birdCollider = GetComponent<Collider2D>();

        // Keep the oversized source sprite in proportion on phone screens.
        if (Application.isMobilePlatform)
        {
            transform.localScale *= 0.75f;

            Camera mainCamera = Camera.main;
            if (mainCamera != null && mainCamera.orthographic)
            {
                mainCamera.orthographicSize = Mathf.Max(mainCamera.orthographicSize, 11.5f);
            }
        }

        logic = FindFirstObjectByType<LogicScript>();
        flapAudioSource = GetComponent<AudioSource>();
        if (flapAudioSource == null)
        {
            flapAudioSource = gameObject.AddComponent<AudioSource>();
        }

        flapAudioSource.playOnAwake = false;
        flapAudioSource.spatialBlend = 0f;
        flapSound = Resources.Load<AudioClip>("Audio/Flap");
    }

    private void Update()
    {
        CheckScreenBounds();

        bool flapPressed = (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
            || (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
            || (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame);

        if (!gameIsOver && flapPressed)
        {
            myRigidbody.linearVelocity = Vector2.up * flapStrength;
            if (flapSound != null)
            {
                flapAudioSource.PlayOneShot(flapSound);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        EndGame();
    }

    private void CheckScreenBounds()
    {
        if (gameIsOver)
        {
            return;
        }

        Camera mainCamera = Camera.main;
        if (mainCamera == null)
        {
            return;
        }

        float cameraDepth = transform.position.z - mainCamera.transform.position.z;
        float bottomEdge = mainCamera.ViewportToWorldPoint(new Vector3(0f, 0f, cameraDepth)).y;
        float topEdge = mainCamera.ViewportToWorldPoint(new Vector3(0f, 1f, cameraDepth)).y;
        Bounds birdBounds = birdCollider != null
            ? birdCollider.bounds
            : new Bounds(transform.position, Vector3.zero);

        if (birdBounds.min.y <= bottomEdge || birdBounds.max.y >= topEdge)
        {
            EndGame();
        }
    }

    private void EndGame()
    {
        if (gameIsOver)
        {
            return;
        }

        gameIsOver = true;
        if (logic == null)
        {
            logic = FindFirstObjectByType<LogicScript>();
        }

        if (logic != null)
        {
            logic.GameOver();
        }
    }
}
