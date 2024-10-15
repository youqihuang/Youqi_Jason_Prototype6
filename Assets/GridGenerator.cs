using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class GridGenerator : MonoBehaviour
{
    public GameObject letterPrefab;  // Reference to the letter prefab
    public Transform gridParent;     // Parent object with Grid Layout Group
    private List<TMP_Text> letterTexts = new List<TMP_Text>();  // Store all the TextMeshPro components of the buttons
    private char[] letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();  // Array of letters

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
                letterTexts.Add(letterText);  // Add this Text component to the list
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

            // Select a random letter
            int randomIndex = Random.Range(0, letterTexts.Count);
            TMP_Text randomLetterText = letterTexts[randomIndex];

            // Change to a new random letter
            randomLetterText.text = letters[Random.Range(0, letters.Length)].ToString();
            
            Debug.Log($"Changed letter at position {randomIndex + 1} to: {randomLetterText.text}");
        }
    }
}



