using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class BulletTrail : MonoBehaviour
{
    private Renderer _r;
    private float _initialAlpha;
    public float duration = .7f;
    private float _elapsedTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        // Get the renderer component
        _r = GetComponent<Renderer>();

        // Get the initial alpha value
        _initialAlpha = _r.material.color.a;
    }

    // Update is called once per frame
    void Update()
    {
        // Increment elapsed time
        _elapsedTime += Time.deltaTime;

        // Calculate new alpha based on the percentage of time elapsed
        var alphaPercentage = 1f - (_elapsedTime / duration);
        var newAlpha = _initialAlpha * alphaPercentage;

        // Set the new alpha value
        var newColor = _r.material.color;
        newColor.a = newAlpha;
        _r.material.color = newColor;

        // Destroy the object after duration
        if (_elapsedTime >= duration)
        {
            Destroy(gameObject);
        }
    }
}
