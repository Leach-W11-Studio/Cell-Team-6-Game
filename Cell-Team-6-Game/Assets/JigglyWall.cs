using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JigglyWall : MonoBehaviour
{
    public float jiggleFrequency;
    public float jiggleTime;
    public float amplitude;
    public LayerMask layerMask;

    private bool canJiggle = true;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Impact(float amplitude, Vector2 directionVector, float jiggleFrequency, float jiggleTime) {
        float oldFrequency = jiggleFrequency;
        float oldJiggleTime = jiggleTime;
        this.jiggleFrequency = jiggleFrequency;
        this.jiggleTime = oldJiggleTime;
        StartCoroutine(Jiggle(amplitude, directionVector));

        this.jiggleFrequency = oldFrequency;
        this.jiggleTime = oldJiggleTime;
    }

    public void Impact(float amplitude, Vector2 directionVector) {
        StartCoroutine(Jiggle(amplitude, directionVector));
    }

    private void OnCollisionEnter2D(Collision2D other) {
        Rigidbody2D otherRB = other.gameObject.GetComponent<Rigidbody2D>();
        if (otherRB) {
            if (other.gameObject.CompareTag("PlayerBullet") || other.gameObject.CompareTag("EnemyBullet")) {
                SimpleBullet bullet = other.gameObject.GetComponent<SimpleBullet>();
                Impact(Random.Range(amplitude/2, amplitude), bullet.velocity.normalized);
            }
        }
    }

    public IEnumerator Jiggle (float amplitude, Vector2 directionVector) {
        if (canJiggle) {
            canJiggle = false;
            Vector2 originalPosition = transform.position;

            for (float elapsedime = 0f; elapsedime < jiggleTime; elapsedime += Time.deltaTime) {
                float percent = (jiggleTime - elapsedime)/jiggleTime;
                float jiggleScalar = amplitude * percent * Mathf.Sin(elapsedime * jiggleFrequency);
                Vector3 jiggleVector = directionVector.normalized * jiggleScalar;
                transform.position = (Vector3) originalPosition + jiggleVector;

                yield return new WaitForEndOfFrame();
            }

            transform.position = originalPosition;
            canJiggle = true;

            yield return null;
        }
    }
}
