using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CollisionAudio : MonoBehaviour
{
    private AudioSource audioSource;
    public float minimumImpactVelocity = 1.5f;

    void Start()
    {
        // Grabs the Audio Source attached to this specific object
        audioSource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision collision)
    {
        // Checks how hard the object hit the floor/table
        if (collision.relativeVelocity.magnitude > minimumImpactVelocity)
        {
            // Plays the thud sound, scaling volume slightly based on impact speed
            float impactVolume = Mathf.Clamp01(collision.relativeVelocity.magnitude / 5f);
            audioSource.PlayOneShot(audioSource.clip, impactVolume);
        }
    }
}