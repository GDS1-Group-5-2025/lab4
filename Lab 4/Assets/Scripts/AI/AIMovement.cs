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
    }

    // Update is called once per frame
    void Update()
    {
        if(Mathf.Abs(this.transform.position.y - destY) <= 0.05f){
            this.transform.position = new Vector2(startPos.x, destY);
            destY = Random.Range(lowerBound, upperBound);
        }
        this.transform.position = Vector2.MoveTowards(this.transform.position, new Vector2(startPos.x, destY), speed*Time.deltaTime);
    }
}
