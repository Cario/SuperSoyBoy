using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    public GameObject playerDeathPrefab;
    public AudioClip deathClip;
    public Sprite hitSprite;
    private SpriteRenderer spriteRenderer;


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.transform.tag == "Player")
        {
            var audioSource = GetComponent<AudioSource>();

            if(audioSource != null && deathClip != null)
            {
                audioSource.PlayOneShot(deathClip);
            }
            Instantiate(playerDeathPrefab, coll.contacts[0].point, Quaternion.identity); //instantiate it as collision point 
            spriteRenderer.sprite = hitSprite; //Swap sawblade with hitSprite
            Destroy(coll.gameObject); //Destroy player

            GameManager.instance.RestartLevel(1.25f);
        }
    }
}
