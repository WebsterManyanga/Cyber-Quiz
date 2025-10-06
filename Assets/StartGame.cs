using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public UIDocument uiDocument;
    public string playgroundSceneName = "Playground"; // set to your Playground scene name

    void Start()
    {
        var root = uiDocument != null ? uiDocument.rootVisualElement : GetComponent<UIDocument>()?.rootVisualElement;
        if (root == null)
        {
            Debug.LogWarning("StartGame: UIDocument or rootVisualElement not found.");
            return;
        }

        var startButton = root.Q<Button>("StartButton");
        if (startButton == null)
        {
            Debug.LogWarning("StartGame: 'StartButton' not found in UI.");
            return;
        }

        startButton.clicked += () =>
        {
            SceneManager.LoadScene(playgroundSceneName);
        };
    }
}
