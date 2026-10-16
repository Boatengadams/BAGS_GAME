using UnityEngine;

public class PipeMiddleScript : MonoBehaviour
{
    public LogicScript logic;
    private bool hasScored;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasScored || other.GetComponent<BirdScript>() == null)
        {
            return;
        }

        if (logic == null)
        {
            logic = FindFirstObjectByType<LogicScript>();
        }

        if (logic == null)
        {
            Debug.LogError("PipeMiddleScript could not find a LogicScript in the scene.", this);
            return;
        }

        hasScored = true;
        logic.AddScore();
    }
}
