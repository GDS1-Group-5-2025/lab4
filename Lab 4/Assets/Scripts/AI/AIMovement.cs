using UnityEngine;

public class AIMovement : MonoBehaviour
{
    [SerializeField] private Vector2 startPos;
    [SerializeField] private float speed;
    [SerializeField] private float upperBound, lowerBound;
    [SerializeField] private float destY;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPos = transform.position;
        destY = float.NaN;
    }

    // Update is called once per frame
    void Update()
    {
        if(float.IsNaN(destY)){ Debug.Log("True"); }
    }
}
