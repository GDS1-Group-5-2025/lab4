using UnityEngine;

public class BiDirectionalDestruction : MonoBehaviour
{
    [SerializeField] private SpriteRenderer[] sR;
    [SerializeField] private BoxCollider2D bC;
    [SerializeField] private AudioSource aS;
    [SerializeField] private int lBound, uBound;

    private void Start()
    {
        lBound = 0;
        uBound = sR.Length-1;
        foreach(var i in sR){
            if(!i.enabled){ i.enabled = true; }
        }
    }

    private void OnCollisionEnter2D(Collision2D col) {
        if(col.gameObject.CompareTag("Bullet")){
            aS.Play();
            if(col.transform.position.x > transform.position.x){   //From the right
                sR[uBound].enabled = false;
                uBound -= 1;
                bC.size = new Vector2(bC.size.x-0.2f, bC.size.y);
                bC.offset = new Vector2(bC.offset.x-0.1f, bC.offset.y);
                if(uBound == 0){ DestroyObj(); }
            }
            else{                                                       //From the left
                sR[lBound].enabled = false;
                lBound += 1;
                bC.size = new Vector2(bC.size.x-0.2f, bC.size.y);
                bC.offset = new Vector2(bC.offset.x+0.1f, bC.offset.y);
                if(lBound == sR.Length-1){ DestroyObj(); }
            }
            Destroy(col.gameObject);
            if(lBound > uBound){ DestroyObj(); }
        }
    }

    private void DestroyObj(){
        Destroy(gameObject);
    }
}
