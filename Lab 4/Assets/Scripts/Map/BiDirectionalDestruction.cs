using UnityEngine;

public class BiDirectionalDestruction : MonoBehaviour
{
    [SerializeField] private SpriteRenderer[] sR;
    [SerializeField] private int lBound, uBound;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lBound = 0;
        uBound = sR.Length-1;
        foreach(SpriteRenderer i in sR){
            if(!i.enabled){ i.enabled = true; }
        }
    }

    private void OnCollisionEnter2D(Collision2D col) {
        Debug.Log("1");
        if(col.gameObject.CompareTag("Bullet")){
            Debug.Log("2");
            if(col.transform.position.x > this.transform.position.x){   //From the right
                sR[uBound].enabled = false;
                uBound -= 1;
                Debug.Log("3a");
            }
            else{                                                       //From the left
                sR[lBound].enabled = false;
                lBound += 1;
                Debug.Log("3b");
            }
            if(lBound > uBound){ Debug.Log("4"); Destroy(this.gameObject); }
        }
    }
}
