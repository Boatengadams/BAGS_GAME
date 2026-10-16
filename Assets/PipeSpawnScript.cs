using UnityEngine;

public class PipeSpawnScript : MonoBehaviour
{
   public GameObject pipe;
   public float spawnRate=3.5f;
   private float timer=0f;
   public float heightOffset = 10;

    private void Awake()
    {
        if (pipe == null)
        {
            Debug.LogError("PipeSpawnScript needs a pipe prefab assigned.", this);
            enabled = false;
            return;
        }

        if (spawnRate <= 0f)
        {
            Debug.LogError("PipeSpawnScript spawnRate must be greater than zero.", this);
            enabled = false;
            return;
        }
    }

    private void Start()
    {
        spawnPipe();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnRate)
        {
            spawnPipe();
            timer -= spawnRate;
        }
    }

    private void spawnPipe()
    {
        float lowestPoint = transform.position.y - heightOffset;
        float highestPoint = transform.position.y + heightOffset;
        float spawnX = transform.position.x;
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            float cameraDistance = Mathf.Abs(mainCamera.transform.position.z - transform.position.z);
            float rightEdge = mainCamera.ViewportToWorldPoint(new Vector3(1f, 0.5f, cameraDistance)).x;
            SpriteRenderer pipeSprite = pipe.GetComponentInChildren<SpriteRenderer>();
            float pipeHalfWidth = pipeSprite != null ? pipeSprite.bounds.extents.x : 0f;
            spawnX = rightEdge + pipeHalfWidth + 0.1f;
        }

        Instantiate(pipe, new Vector3(spawnX, Random.Range(lowestPoint, highestPoint), transform.position.z), transform.rotation);
    }
}
