using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GridGenerator : MonoBehaviour
{
    public GameObject letterPrefab;  // Reference to the letter prefab
    public Transform gridParent;     // Parent object with Grid Layout Group
    private List<GameObject> letterTiles = new List<GameObject>();  // Store all the letter tile GameObjects
    private char[] letters = "AAAAAAAAABBBCCDDDDEEEEEEEEEEEEFFGGGHHIIIIIIIIIJKKLLLLMMNNNNNNOOOOOOOOPPQRRRRRRSSSSTTTTTTUUVVWWXYYZAE".ToCharArray();   // Array of letters
    [ColorUsage(true, true)] public Color changeColor = Color.blue;

    public SoundManager soundManager;

    void Start()
    {
        GenerateGrid();  // Generate the letter grid
        StartCoroutine(ChangeRandomLetterEvery5Seconds());  // Start a coroutine to change a random letter every 5 seconds
    }

    void GenerateGrid()
    {
        for (int i = 0; i < 16; i++)  // Generate a 4x4 grid, 16 cells in total
        {
            GameObject letterTile = Instantiate(letterPrefab, gridParent);  // Instantiate the prefab

            // Get the TextMeshPro component from the Button
            TMP_Text letterText = letterTile.GetComponentInChildren<TMP_Text>();

            if (letterText != null)
            {
                // Generate a random letter and display it
                letterText.text = letters[Random.Range(0, letters.Length)].ToString();
                letterTiles.Add(letterTile);  // Add the letter tile GameObject to the list
            }
            else
            {
                Debug.LogError("LetterPrefab does not have a TMP_Text component!");
            }
        }
    }

    // Coroutine: Change a random letter every 5 seconds
    IEnumerator ChangeRandomLetterEvery5Seconds()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);  // Execute every 5 seconds

            // Select a random letter tile
            int randomIndex = Random.Range(0, letterTiles.Count);
            GameObject randomLetterTile = letterTiles[randomIndex];

            // Get the TMP_Text component from the selected tile
            TMP_Text randomLetterText = randomLetterTile.GetComponentInChildren<TMP_Text>();

            yield return StartCoroutine(FlashLetter(randomLetterTile));

            if (randomLetterText != null)
            {
                // Change to a new random letter
                randomLetterText.text = letters[Random.Range(0, letters.Length)].ToString();
            }
            soundManager.PlaySoundByName("clink");
        }
    }

    IEnumerator FlashLetter(GameObject letterTile)
    {
        Image letterImage = letterTile.GetComponent<Image>();
        Color originalColor = Color.white;

        for (int i = 0; i < 3; i++)
        {
            // Change to flash color
            letterImage.color = changeColor;
            yield return new WaitForSeconds(0.3f); // Wait for 0.3 seconds

            // Revert to original color
            letterImage.color = originalColor;
            yield return new WaitForSeconds(0.3f); // Wait for 0.3 seconds
        }
    }
}
