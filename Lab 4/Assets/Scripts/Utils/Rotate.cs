using UnityEngine;

public class Rotate : MonoBehaviour
{
   public float speed = 10f;

    // Update is called once per frame
    private void Update()
    {
        transform.Rotate(Vector3.forward, speed * Time.deltaTime);
    }
}
