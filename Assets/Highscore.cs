using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class Highscore : MonoBehaviour
{
    public UIDocument uiDocument;
    public string gameSceneName = "Game"; // set this to your gameplay scene name in the Inspector

    void Start()
    {
        // Get UI root
        var root = uiDocument != null ? uiDocument.rootVisualElement : GetComponent<UIDocument>()?.rootVisualElement;
        if (root == null)
        {
            Debug.LogWarning("Highscore: UIDocument or rootVisualElement not found.");
            return;
        }

        // Read values
        int totalAsked = GameManager.totalQuestionsAsked;
        int totalCorrect = GameManager.totalQuestionsCorrect;

        // Find score label inside the 'Score' container
        var scoreContainer = root.Q<VisualElement>("Score");
        var scoreLabel = scoreContainer?.Q<Label>();
        Debug.Log("Score Label: " + scoreLabel);
        Debug.Log("Total Correct: " + totalCorrect);
        Debug.Log("Total Asked: " + totalAsked);
            if (totalAsked == 0) {
                scoreLabel.text = "0%";
            } else {
                float scorefraction = (float)totalCorrect/totalAsked;
                int percent = Mathf.RoundToInt(scorefraction * 100);
                scoreLabel.text = percent.ToString() + "%";
            }
        
    



        // Wire Play Again button (assumes the first Button is Play Again?)
        var playAgainButton = root.Q<Button>();
        if (playAgainButton != null)
        {
            playAgainButton.clicked += () =>
            {
                SceneManager.LoadScene(gameSceneName);
            };
        }
        else
        {
            Debug.LogWarning("Highscore: Play Again button not found.");
        }

        // Wire Quit button by name
        var quitButton = root.Q<Button>("Quit");
        if (quitButton != null)
        {
            quitButton.clicked += () =>
            {
                #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
                #else
                Application.Quit();
                #endif
            };
        }
        else
        {
            Debug.LogWarning("Highscore: Quit button not found.");
        }
    }
}
