using UnityEngine;

public class CloudBackgroundScript : MonoBehaviour
{
    public Sprite cloudSprite;
    public int cloudCount = 5;
    public float moveSpeed = 0.65f;
    public float minimumY = -5f;
    public float maximumY = 6f;
    public float cloudScale = 0.65f;

    private Camera gameCamera;
    private Transform[] clouds;
    private float cloudWidth;

    private void Start()
    {
        gameCamera = GetComponent<Camera>();
        if (gameCamera == null)
        {
            gameCamera = Camera.main;
        }

        if (gameCamera == null || cloudSprite == null)
        {
            Debug.LogError("CloudBackgroundScript needs a camera and cloud sprite.", this);
            enabled = false;
            return;
        }

        cloudCount = Mathf.Max(1, cloudCount);
        clouds = new Transform[cloudCount];
        cloudWidth = cloudSprite.bounds.size.x * cloudScale;
        float halfViewWidth = gameCamera.orthographicSize * gameCamera.aspect;

        for (int i = 0; i < clouds.Length; i++)
        {
            GameObject cloud = new GameObject("Cloud");
            SpriteRenderer renderer = cloud.AddComponent<SpriteRenderer>();
            renderer.sprite = cloudSprite;
            renderer.sortingOrder = -10;
            cloud.transform.localScale = Vector3.one * cloudScale;
            cloud.transform.position = new Vector3(
                Random.Range(-halfViewWidth, halfViewWidth),
                Random.Range(minimumY, maximumY),
                0f);
            clouds[i] = cloud.transform;
        }
    }

    private void Update()
    {
        if (clouds == null)
        {
            return;
        }

        float halfViewWidth = gameCamera.orthographicSize * gameCamera.aspect;
        foreach (Transform cloud in clouds)
        {
            cloud.position += Vector3.left * moveSpeed * Time.deltaTime;
            if (cloud.position.x < -halfViewWidth - cloudWidth * 0.5f)
            {
                cloud.position = new Vector3(
                    halfViewWidth + cloudWidth * 0.5f,
                    Random.Range(minimumY, maximumY),
                    cloud.position.z);
            }
        }
    }
}
