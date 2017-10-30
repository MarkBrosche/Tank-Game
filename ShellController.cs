using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellController : MonoBehaviour {

    // This script controls the properties of the shells fired from the tank
    // destroys on collision with a rigid body
    // create explosion and smoke on collision

    public float speed = 20f;
    public Transform shellSpawn;
    public GameObject shellPrefab, hitParticle, muzFlashParticle;
    public AudioClip shot;
    private AudioSource _shot;

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            fire();
        }        
    }

    // Fire instantiates a shell with trajectory, speed, and special effects
    void fire()
    {
        var shell = (GameObject)Instantiate(shellPrefab, shellSpawn.position, shellSpawn.rotation);
        shell.GetComponent<Rigidbody>().velocity = shell.transform.forward * speed;
        Instantiate(muzFlashParticle, shellSpawn.position, shellSpawn.rotation);
        _shot.PlayOneShot(shot);        
    }

    // On collisions between the parent object colliders and objects of defined tags, carry out the following:
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Shot Destroyer") || other.gameObject.CompareTag("Player"))
        {
            Instantiate(hitParticle, shellPrefab.transform.position, shellPrefab.transform.rotation);
            Destroy(shellPrefab, 0.2f);
        }
    }

    // Used to set audio
    private void Awake()
    {
        _shot = GetComponent<AudioSource>();
    }
}
