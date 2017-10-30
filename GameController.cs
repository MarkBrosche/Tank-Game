using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public GameObject tankCam, overheadCam, tank;
    public Transform turret;
    private int _cameraState = 1;
    public AudioClip lose;
    private AudioSource _lose;
    public Text loseText, startText;

	// Use this for initialization
	void Start () {
		tankCam.transform.SetParent(turret);
        loseText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        // Quit game
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Escape pressed");
            Application.Quit();
        }

        // Disappear the intro text
        if (Input.anyKey)
        {
            startText.text = "Clear the area and find the exit!";
            StartCoroutine(PleaseWait()); // DOES NOT WORK?!
            startText.text = "";

        }

        // Switch from Tank camera to overhead
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("F pressed");
            switch (_cameraState)
            {
                case 0:
                    Debug.Log("Tank Camera Selected");
                    overheadCam.SetActive(false);
                    tankCam.SetActive(true);
                    _cameraState = 1;
                    break;
                case 1:
                    Debug.Log("Overhead Camera Selected");
                    tankCam.SetActive(false);
                    overheadCam.SetActive(true);
                    _cameraState = 0;
                    break;
            }
        }

        //if lose game, press 'r' to replay or 'Esc' to quit
        if (!tank)
        {
            Debug.Log("Loss of Tank");
            loseText.text = "Try again (Press 'r') \n Give Up (Press 'Esc')";
           // _lose.PlayOneShot(lose, 1f);
            if (Input.GetKeyDown(KeyCode.R))
            {
                StartCoroutine(PleaseWait());
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }        
    }


    private void Awake()
    {
        _lose = GetComponent<AudioSource>();
    }

    private IEnumerator PleaseWait()
    {
        yield return new WaitForSeconds(2.0f);
    }
}
