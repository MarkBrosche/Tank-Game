using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour {

    public Transform player, smoke, gun;
    public GameObject bulletPrefab, explosion, enemy, smoking, muzFlashParticle;

    // Adjustable variables for setting the valid target area and rate of fire.
    public float range, anlgeRange = 90, speed, interval;

    // These variables govern the life and sound effects of the enemy
    private int _life = 3;
    public AudioClip crack, collapse, gunShot;
    private AudioSource _crack, _collapse;

    // Use this for initialization
    void Start () {
        //gun.transform.rotation = Quaternion.identity;
    }
	
	// Update is called once per frame
	void Update () {
        // Scan the area:
        if (Input.GetKey(KeyCode.G))//!RaycastHit.Equals(Player, enemy))
        {
            Debug.Log("G pressed");
            gun.transform.rotation = Quaternion.AngleAxis(Mathf.PingPong(Time.time * 90, anlgeRange), Vector3.up);            
        }
        //Target player
        //gun.LookAt(player);
        //Shoot at player
        if (Input.GetKeyDown(KeyCode.O))
        {
            Fire();
        }
  	}

    void OnTriggerEnter(Collider other)
    {
        // Enemy Takes Damage
        if (other.gameObject.CompareTag("Shell"))
        {            
            _crack.PlayOneShot(crack, 0.5f);
            _life--;
            Kill(other.gameObject);
            Destroy(other.gameObject);
        }
    }

    //
    void Fire()
    {
        var bullet = (GameObject)Instantiate(bulletPrefab, gun.transform.position, gun.transform.rotation);
        bullet.GetComponent<Rigidbody>().velocity = -bullet.transform.up * speed;
        Instantiate(muzFlashParticle, gun.transform.position, gun.transform.rotation);
    }

    // Kill plays the associated death events like explosions, sounds, smoke, and destroying the game object.
    private void Kill(GameObject other)
    {
        if (_life <= 0)
        {
            Instantiate(explosion, other.transform.position, other.transform.rotation);
            _collapse.PlayOneShot(collapse, 0.5f);
            Instantiate(smoking, smoke.transform.position, smoke.transform.rotation);
            Destroy(enemy, 1f);
        }
    }
    private void Awake()
    {
        _crack = GetComponent<AudioSource>();
        _collapse = GetComponent<AudioSource>();
       // _gunShot = GetComponent<AudioSource>();
    }
}
