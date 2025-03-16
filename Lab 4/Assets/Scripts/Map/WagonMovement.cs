using UnityEngine;

public class WagonMovement : MonoBehaviour
{
    private Vector2 startPos;
    [SerializeField] private Vector2 endPos;
    [SerializeField] private float vertMotion, speed, animationChangeRateConst;
    private float animationChangeRate;
    [SerializeField] private int currSprite;
    [SerializeField] private Sprite[] sprites;
    private bool entering = true;

    void Start(){
        startPos = this.transform.position;
        animationChangeRate = animationChangeRateConst;
    }
    void Update()
    {
        animationChangeRate -= Time.deltaTime;
        if(animationChangeRate <= 0){
            currSprite += 1;
            if(currSprite == sprites.Length){ currSprite = 0;}
            this.gameObject.GetComponent<SpriteRenderer>().sprite = sprites[currSprite];
            animationChangeRate = animationChangeRateConst;
        }

        if(vertMotion == 1){
            if(this.transform.position.y > endPos.y){ this.transform.position = startPos; }
        }
        else if(vertMotion == -1){
            if(this.transform.position.y < endPos.y){ this.transform.position = startPos; }
        }
        this.transform.position = new Vector2(this.transform.position.x, this.transform.position.y+vertMotion*speed*Time.deltaTime);
    }
}
