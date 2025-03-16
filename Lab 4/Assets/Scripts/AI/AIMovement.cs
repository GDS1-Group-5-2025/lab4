using UnityEngine;

public class AIMovement : MonoBehaviour, IMovement
{
    [SerializeField] private Vector2 startPos;
    [SerializeField] private float speed;
    [SerializeField] private float upperBound, lowerBound;
    [SerializeField] private float destY;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
   private void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    private void Update()
    {
        if(Mathf.Abs(transform.position.y - destY) <= 0.05f){
            transform.position = new Vector2(startPos.x, destY);
            destY = Random.Range(lowerBound, upperBound);
        }
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(startPos.x, destY), speed*Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("Wall")){
            if(transform.position.y < other.transform.position.y){ destY = upperBound = transform.position.y; }
            else if(transform.position.y > other.transform.position.y){ destY = lowerBound = transform.position.y; }
        }
    }
}
