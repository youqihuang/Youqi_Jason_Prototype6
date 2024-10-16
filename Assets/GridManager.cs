using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class GridManager : MonoBehaviour
{

    public List<Button> selectedButtons = new List<Button>(); // To keep track of the selected buttons
    public List<Button> allButtons = new List<Button>();
    public HashSet<string> validWords; // Store a dictionary or list of valid words
    public HashSet<string> usedWords;
    public Dictionary<string, Color> colors = new Dictionary<string, Color>();

    // These fields will be used to expose color pickers for specific colors in the Inspector
    [ColorUsage(true, true)] public Color validColor = Color.green;
    [ColorUsage(true, true)] public Color invalidColor = Color.red;
    [ColorUsage(true, true)] public Color selectColor = Color.yellow;

    public SoundManager soundManager;

    public float gameTime;
    private float timer;
    private TMP_Text timeText;

    public int baseScore = 100;
    private int score = 0;
    private TMP_Text scoreText;

    public GameObject gameOver;
    private TextMeshProUGUI gameOverScore;

    private bool isDragging = false;

    void Start()
    {
        timer = gameTime;

        TextAsset wordFile = Resources.Load<TextAsset>("dictionary"); 
        if (wordFile != null)
        {
            string[] words = wordFile.text.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
            validWords = new HashSet<string>(words);
        }
        else
        {
            Debug.LogError("Dictionary words file not found!");
        }
        usedWords = new HashSet<string>();
        // Add event listeners for all buttons
        allButtons.AddRange(gameObject.GetComponentsInChildren<Button>());
        colors["valid"] = validColor;
        colors["invalid"] = invalidColor;
        colors["select"] = selectColor;

        timeText = GameObject.FindWithTag("timer").GetComponent<TMP_Text>();
        scoreText = GameObject.FindWithTag("score").GetComponent<TMP_Text>();

        //gameOver = GameObject.Find("GameOver");
        gameOverScore = GameObject.FindWithTag("gameOverScore").GetComponent<TextMeshProUGUI>();
        gameOver.SetActive(false);
    }

    void Update()
    {
        // Check if the "R" key is pressed
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartGame();
        }

        timer -= Time.deltaTime;

        if (timer <= 0){
            // game over
            Time.timeScale = 0;
            gameOver.SetActive(true);
            gameOverScore.text = "Score : " + score.ToString();
            timer = 0;

        }
        else
        {
            // Handle mouse drag selection
            if (Input.GetMouseButton(0))
            {
                isDragging = true;
                Vector2 mousePos = Input.mousePosition;
                CheckButtonUnderMouse(mousePos);
            }

            // Handle when user releases the mouse or touch
            if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
                StartCoroutine(Check());

            }
        }

        int minutes = (int)(timer / 60);
        int seconds = (int)(timer % 60);
        timeText.text = string.Format("Time: {0}:{1:00}", minutes, seconds);


    }




    // Check which button is currently under the mouse and add it to the selection
    void CheckButtonUnderMouse(Vector2 mousePos)
    {
        foreach (Button btn in allButtons)
        {
            RectTransform rectTransform = btn.GetComponent<RectTransform>();
            if (RectTransformUtility.RectangleContainsScreenPoint(rectTransform, mousePos))
            {
                if (!selectedButtons.Contains(btn))
                {
                    selectedButtons.Add(btn);
                    HighlightButton(btn);
                    soundManager.PlaySoundByName("drop");
                }
            }
        }
    }

    // Highlight button by changing its color to green
    void HighlightButton(Button btn)
    {
        //Debug.Log("Clicked "+ GetComponentInChildren<TextMeshProUGUI>().text);
        ColorBlock cb = btn.colors;
        btn.GetComponent<Image>().color = colors["select"];
    }

    // Check if the formed string is a valid word
    void CheckWordFormed()
    {
        string formedWord = "";
        foreach (Button btn in selectedButtons)
        {
            formedWord += btn.GetComponentInChildren<TextMeshProUGUI>().text;
        }

        Color c;
        if (validWords.Contains(formedWord) && !usedWords.Contains(formedWord) && formedWord.Length >= 3)
        {
            Debug.Log("Valid word: " + formedWord);
            c = colors["valid"];
            usedWords.Add(formedWord);
            soundManager.PlaySoundByName("pump");
            score += (int)(baseScore * Mathf.Pow(2, (formedWord.Length - 3))) ;
            scoreText.text = "Score: " + score.ToString();
        }
        else
        {
            Debug.Log("Invalid word: " + formedWord);
            c = colors["invalid"];
            soundManager.PlaySoundByName("downed");
        }
        foreach (Button btn in selectedButtons)
        {
            btn.GetComponent<Image>().color = c;
        }
    }



    // Clear the current selection and reset button colors
    void ClearSelection()
    {
        
        //Debug.Log("clearing");
        foreach (Button btn in selectedButtons)
        {
            btn.GetComponent<Image>().color = Color.white;
        }
        selectedButtons.Clear();
        
    }

    IEnumerator Check()
    {
        CheckWordFormed();
        yield return new WaitForSeconds(0.5f); // Pause for half a second
        ClearSelection();
    }

    void RestartGame()
    {
        // Get the current scene
        Scene currentScene = SceneManager.GetActiveScene();
        // Reload the current scene
        SceneManager.LoadScene(currentScene.name);
    }
}
