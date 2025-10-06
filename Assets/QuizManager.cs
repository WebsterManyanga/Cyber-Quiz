using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

public class QuizManager : MonoBehaviour
{
    public UIDocument uiDocument;

    private Label title;

    void OnEnable()
    {
        Debug.Log("QuizManager.OnEnable");
        var root = uiDocument.rootVisualElement;

        // Link UI elements (make sure names match your UXML)
        title = root.Q<Label>("Title");

        // Example questions
        // questions = new List<Question>
        // {
        //     new Question { questionText = "What is 2 + 2?", answers = new string[]{"3","4","5","6"}, correctAnswerIndex = 1 },
        //     new Question { questionText = "What is the capital of France?", answers = new string[]{"Berlin","London","Paris","Rome"}, correctAnswerIndex = 2 }
        // };
        title.text = "Questions come here";

    }

    

}
