using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    //handle to text
    [SerializeField]
    private Text _scoreText;

    [SerializeField]
    private Image _LivesImg;

    //Ammo amount
    [SerializeField]
    private Text _ammoText;

    [SerializeField]
    private Text _noAmmoText;

    [SerializeField]
    private Text _gameOverText;

    [SerializeField]
    private Text _restartText;

    private bool _gameOver;

    private Game_Manager _gameManager;

    [SerializeField] 
    private Sprite[] _liveSprites;

    // Start is called before the first frame update
    void Start()
    {
        _gameOverText.gameObject.SetActive(false);
        _restartText.gameObject.SetActive(false);
        _noAmmoText.gameObject.SetActive(false);

        _gameManager = GameObject.Find("GameManager").GetComponent<Game_Manager>();

        if(_gameManager == null)
        {
            Debug.LogError("Game Manager is null!");
        }

        _scoreText.text = "Score: " + 0;
    }

    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore.ToString();
    }

    //Ammo update
    public void UpdateAmmo(int ammoAmount)
    {
        _ammoText.text = ammoAmount.ToString() + " /15";

        if(ammoAmount <= 0)
        {
            NoAmmoSequence();
        }
    }

    public void UpdateLives(int currentLives)
    {
        _LivesImg.sprite = _liveSprites[currentLives];

        if(currentLives == 0)
        {
            GameOverSequence();
        }
    }

    void NoAmmoSequence()
    {
        _noAmmoText.gameObject.SetActive(true);
        StartCoroutine(NoAmmoFlickerRoutine());
    }

    void GameOverSequence()
    {
        _gameManager.GameOver();
        _gameOverText.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true);
        StartCoroutine(GameOverFlickerRoutine());
    }

    IEnumerator GameOverFlickerRoutine()
    {
        while (true)
        {
            _gameOverText.text = "GAME OVER";
            yield return new WaitForSeconds(0.5f);
            _gameOverText.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator NoAmmoFlickerRoutine()
    {
        while (true)
        {
            _noAmmoText.text = "NO AMMO";
            yield return new WaitForSeconds(0.5f);
            _noAmmoText.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }

}
