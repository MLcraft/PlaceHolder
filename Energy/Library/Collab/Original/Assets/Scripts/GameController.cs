using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public GameObject player, boss;
    public Canvas dialogue;
    public Image dialoguePortrait;
    public Text dialogueText;
    public Sprite playerPort;
    public Sprite bossPort;
    public Text victoryText;
    public Text gameoverText;
    private bool _runOnce;
    private string[] _lines;
    private int _count;
    private float _delay;
    private bool _cinematicMode;
    private int _part;      // Which part of the story
    private Sprite[] _portraits;

    // Use this for initialization
    void Start()
    {
        _runOnce = false;
        _lines = new string[]{
            "Sirbot: GAH, THIS BACKUP BATTERY WON'T LAST MUCH LONGER...",
            "*Sirbot spots nameless main character*",
            "Sirbot: YOU THERE! GIVE ME THAT JETPACK!",
            "Dude: wait what hell no"
        };

        _portraits = new Sprite[] {
                bossPort, bossPort, bossPort, playerPort
            };

        _part = 0;
        victoryText.gameObject.SetActive(false);
        gameoverText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        if (!_runOnce)
        {
            player.GetComponent<PlayerController>().enabled = false;
            boss.GetComponent<FirstBossAI>().enabled = false;
            dialogue.enabled = true;
            _cinematicMode = true;
            _runOnce = true;
        }

        if (_cinematicMode)
        {
            if (Input.GetAxis("Shoot") != 0 && _delay < 0)
            {
                // move to the next dialogue
                _delay = 0.25f;
                _count++;
                if (_count >= _lines.Length)
                {
                    player.GetComponent<PlayerController>().enabled = true;
                    boss.GetComponent<FirstBossAI>().enabled = true;
                    _cinematicMode = false;
                    dialogue.enabled = false;

                    if (_part == 1)
                    {
                        victoryText.gameObject.SetActive(true);
                        DeleteAll();
                    }
                }
            }
            else
            {
                _delay -= Time.deltaTime;
                displayDialogue();
            }
        }

        if (boss.GetComponent<FirstBossAI>().health <= 0 && _part == 0)
        {
            _runOnce = false;
            _count = 0;
            _part = 1;
            _lines = new string[] {
                "Dude: So why'd you need my jetpack anyway?",
                "Sirbot: POWER LEVELS... INSUFFICIENT... NEED ELECTRICITY TO SURVIVE...",
                "Dude: You do realize this jetpack isn't electricity powered, right?",
                "Sirbot: ...",
                "Dude: Also, I have a power bank, like, right in my pocket. You could have just asked for help instead.",
                "Sirbot: ...WELL, THIS ENTIRE FIGHT WAS MILDLY DUMB THEN, WASN'T IT?",
                "Dude: ...pretty much."
            };

            _portraits = new Sprite[] {
                playerPort, bossPort, playerPort, bossPort, playerPort, bossPort, playerPort
            };
        }
        if (player.GetComponent<PlayerController>().health <= 0)
        {
            SceneManager.LoadScene("Gameover", LoadSceneMode.Single);
        }
    }


    void displayDialogue()
    {
        if (_part == 0)
        {
            dialogueText.text = _lines[_count];
            dialoguePortrait.sprite = _portraits[_count];
        }
        else if (_part == 1)
        {
            dialogueText.text = _lines[_count];
            dialoguePortrait.sprite = _portraits[_count];
        }
        
    }

    void DeleteAll()
    {
        foreach (GameObject o in Object.FindObjectsOfType<GameObject>())
        {
            if (o.tag != "MainCamera")
                Destroy(o);
        }
    }
}
