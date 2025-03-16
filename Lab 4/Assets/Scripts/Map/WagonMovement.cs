using UnityEngine;

public class WagonMovement : MonoBehaviour
{
    [SerializeField] private Vector2 endPos;
    [SerializeField] private float vertMotion, speed, animationChangeRateConst;
    [SerializeField] private int currSprite;
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private bool entering = true;

    private Vector2 _startPos;
    private float _animationChangeRate;

    private SpriteRenderer _spriteRenderer;

    private void Start(){
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        _startPos = transform.position;
        _animationChangeRate = animationChangeRateConst;
    }
    private void Update()
    {
        _animationChangeRate -= Time.deltaTime;
        if(_animationChangeRate <= 0){
            currSprite += 1;
            if(currSprite == sprites.Length){ currSprite = 0;}
            _spriteRenderer.sprite = sprites[currSprite];
            _animationChangeRate = animationChangeRateConst;
        }

        if(Mathf.Approximately(vertMotion, 1)){
            if(transform.position.y > endPos.y){ transform.position = _startPos; }
        }
        else if(Mathf.Approximately(vertMotion, -1)){
            if(transform.position.y < endPos.y){ transform.position = _startPos; }
        }
        transform.position = new Vector2(transform.position.x, transform.position.y+vertMotion*speed*Time.deltaTime);
    }
}
