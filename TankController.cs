using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class TankController : MonoBehaviour {

    public float tankSpeed = 10f;
    public GameObject explosion, tankCamera, tank, turret, leftFrontWheel, leftSteer, rightFrontWheel, rightSteer, leftBackWheel, rightBackWheel, cannon;
    
    private float _frontWheelRadius = 100f, _backWheelRadius = 150f, _steerY = 0f, _cannonX = 0f;
    private Vector3 _tankY;

    private int _life = 20;
    public AudioClip ding, boom;
    private AudioSource _ding, _boom;
    public Text lifeText, winText;

    // Use this for initialization
    void Start () {
        SetLifeText();
        winText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        // Turn Left
        if (Input.GetKey(KeyCode.A))
        {
            _steerY -= 3;
            _steerY = Mathf.Clamp(_steerY, -45, 45);
            rightSteer.transform.localEulerAngles = new Vector3(0, _steerY, 0);
            leftSteer.transform.localEulerAngles = new Vector3(0, _steerY, 0);
        }
        
        // Turn Right
        if (Input.GetKey(KeyCode.D))
        {
            _steerY += 3;
            _steerY = Mathf.Clamp(_steerY, -45, 45);
            rightSteer.transform.localEulerAngles = new Vector3(0, _steerY, 0);
            leftSteer.transform.localEulerAngles = new Vector3(0, _steerY, 0);
        }

        // Move Forward
        if (Input.GetKey(KeyCode.W))
        {
            tank.transform.Translate(Vector3.forward * tankSpeed * Time.deltaTime, Space.Self);
            leftFrontWheel.transform.Rotate(Vector3.right, RotSpeed(_frontWheelRadius));
            rightFrontWheel.transform.Rotate(Vector3.right, RotSpeed(_frontWheelRadius));
            leftBackWheel.transform.Rotate(Vector3.right, RotSpeed(_backWheelRadius));
            rightBackWheel.transform.Rotate(Vector3.right, RotSpeed(_backWheelRadius));
            if (_steerY != 0)
            {
                _tankY = new Vector3(0, _steerY * Time.deltaTime, 0); // Steer to turn ratio: 0.4478f
                tank.transform.Rotate(_tankY);
            }
        }

        // Move Backward
        if (Input.GetKey(KeyCode.S))
        {
            tank.transform.Translate(Vector3.back * tankSpeed * Time.deltaTime, Space.Self);
            leftFrontWheel.transform.Rotate(Vector3.left, RotSpeed(_frontWheelRadius));
            rightFrontWheel.transform.Rotate(Vector3.left, RotSpeed(_frontWheelRadius));
            leftBackWheel.transform.Rotate(Vector3.left, RotSpeed(_backWheelRadius));
            rightBackWheel.transform.Rotate(Vector3.left, RotSpeed(_backWheelRadius));
            if (_steerY != 0)
            {
                _tankY = new Vector3(0, -_steerY * Time.deltaTime, 0); // Steer to turn ratio: 0.4478f
                tank.transform.Rotate(_tankY);
            }
        }

        // Rotate Turret Left
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            turret.transform.Rotate(Vector3.down, 30 * Time.deltaTime);
        }

        // Rotate Turret Right
        if (Input.GetKey(KeyCode.RightArrow))
        {
            turret.transform.Rotate(Vector3.up, 30 * Time.deltaTime);
        }

        // Raise Cannon
        if (Input.GetKey(KeyCode.UpArrow))
        {
            _cannonX--;
            _cannonX = Mathf.Clamp(_cannonX, -90, 0);
            cannon.transform.localEulerAngles = new Vector3(_cannonX, 0, 0);
        }

        // Lower Cannon
        if (Input.GetKey(KeyCode.DownArrow))
        {
            _cannonX++;
            _cannonX = Mathf.Clamp(_cannonX, -90, 0);
            cannon.transform.localEulerAngles = new Vector3(_cannonX, 0, 0);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Tank Damage
        if (other.gameObject.CompareTag("Bullet") || other.gameObject.CompareTag("Shell"))
        {
            Destroy(other.gameObject);
            _ding.PlayOneShot(ding, 0.5f);
            _life--;
            SetLifeText();
        }

        // Win Level
        if (other.gameObject.CompareTag("Finish"))
        {
            winText.text = "LEVEL COMPLETED! GOOD JOB SOLDIER";
            StartCoroutine(PleaseWait());
            SceneManager.LoadScene("Level2");
        }
    }

    // SetLifeText holds and updates the UI with remaining Tank HP and destroys tank if <= 0.
    private void SetLifeText()
    {
        lifeText.text = "Health: " + _life.ToString();
        if (_life <= 0)
        {
            tankCamera.transform.SetParent(null);
            Instantiate(explosion, tank.transform.localPosition, tank.transform.localRotation);
            _boom.PlayOneShot(boom, 2.0f);
            Destroy(tank, 2f);
        }           
    }

    // Function used in setting up audio.
    private void Awake()
    {
        _ding = GetComponent<AudioSource>();
        _boom = GetComponent<AudioSource>();
    }

    // Function that converts tank speed to wheel rotation speed.
    float RotSpeed(float radius)
    {
        return tankSpeed / (radius * 3.14159f) * 180;
    }

    //  This is supposed to allow a delay... but it doesn't work.
    private IEnumerator PleaseWait()
    {
        yield return new WaitForSeconds(500.0f);
    }
}
