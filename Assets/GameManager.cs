using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public GameObject npc;
    public GameObject questionBox;
    public bool questionAsked = false;
    public QuestionList questionList;
     public UIDocument uiDocument;
     public UIDocument scoreUI;
    private Label title;
    private int questionIndex = 0;
    private Label question;
    private Label greeting;
    private Button button1;
    private Button button2;
    private Button button3;
    private Button button4;
    private VisualElement avatarElement;
    private int score = 0;
    public int nextCharacter = 0;
    float spawnInterval = 10f;
    private float _spawnTimer = 0f;
    public bool firstSpawn = true;
    public bool timeEnded = false;
    public string highScoreSceneName = "HighScore";
    private bool _highScoreLoaded = false;
    public static int totalQuestionsAsked = 0;
    public static int totalQuestionsCorrect = 0;
    public static int totalQuestionsIncorrect = 0;
    public GameObject pausedText; 
    public GameObject player;
    private Behaviour playerController;
    
    
    
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("questions");
        questionList = JsonUtility.FromJson<QuestionList>(jsonFile.text);
        ShuffleQuestions();
        
        questionBox = GameObject.Find("UIDocument");
        questionBox.SetActive(false);
        pausedText = GameObject.Find("PausedText");
        pausedText.SetActive(false);
        player = GameObject.Find("PlayerArmature");
        if (player != null)
        {
            // Try to get the controller by class name (works for StarterAssets.ThirdPersonController)
            playerController = player.GetComponent("ThirdPersonController") as Behaviour;
        }
        questionAsked = false;
        for (int i = 0; i < 20; i++)
        {
            Instantiate(npc, new Vector3((-4+(i*5)), 0.45f, Random.Range(0, 24)), Quaternion.Euler(0, 180, 0));
        }  

        var scoreRoot = scoreUI.rootVisualElement;
        scoreRoot.Q<Label>("Score").text = score.ToString();
    }

    private void ShuffleQuestions()
    {
        if (questionList == null || questionList.questions == null) return;
        var arr = questionList.questions;
        for (int i = arr.Length - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            var tmp = arr[i];
            arr[i] = arr[j];
            arr[j] = tmp;
        }
    }

    // Update is called once per frame.
    void Update()
    {
        // spawn an NPC every spawnInterval seconds
        if (firstSpawn) {
            spawnInterval = 3f;
            firstSpawn = false;
        } else {
            spawnInterval = 7f;
        }
        _spawnTimer += Time.deltaTime;

        if (_spawnTimer >= spawnInterval)
        {
            for (int i = 0; i < 5; i++)
            {
                Instantiate(npc, new Vector3((-4+(i*5)), 0.45f, Random.Range(28f, 24f)), Quaternion.Euler(0, 180, 0));
            }   
            _spawnTimer = 0f;

        }

        if (timeEnded && !_highScoreLoaded) {
            _highScoreLoaded = true;
            // Persist values for the high score scene
            SceneManager.LoadScene(highScoreSceneName);
        }
    }

    public void QuestionTrigger(string avatar)
    {   
                questionAsked = true;
                if (questionAsked == true){
                questionBox.SetActive(true);
                var root = uiDocument.rootVisualElement;
                question = root.Q<Label>("Question");
                greeting = root.Q<Label>("Greeting");
                button1 = root.Q<Button>("Option1");
                button2 = root.Q<Button>("Option2");
                button3 = root.Q<Button>("Option3");
                button4 = root.Q<Button>("Option4");
                greeting.text = "Hi, I'm " + avatar + "! Can you help me with this?";

                avatarElement = root.Q<VisualElement>(avatar);
                avatarElement.style.display = DisplayStyle.Flex;

                question.text = questionList.questions[questionIndex].question;
                button1.text  = questionList.questions[questionIndex].options[0];
                button2.text  = questionList.questions[questionIndex].options[1];
                button3.text  = questionList.questions[questionIndex].options[2];
                button4.text  = questionList.questions[questionIndex].options[3];
                button1.clicked += () => AnswerQuestion(0);
                button2.clicked += () => AnswerQuestion(1);
                button3.clicked += () => AnswerQuestion(2);
                button4.clicked += () => AnswerQuestion(3); 


        } else {
            questionBox.SetActive(false);
        }

        
    }

    private System.Collections.IEnumerator HideQuestionBoxAfterDelay()
    {
        yield return new WaitForSeconds(0.2f);
        questionBox.SetActive(false);
    }


    private void AnswerQuestion(int answerIndex)
    {
        if (answerIndex == questionList.questions[questionIndex].answer)
        {
            totalQuestionsCorrect++;
            switch (answerIndex)
            {
                case 0:
                    button1.AddToClassList("correct");
                    button1.RemoveFromClassList("answer-btn");
                    
                    break;
                case 1:
                    button2.AddToClassList("correct");
                    button2.RemoveFromClassList("answer-btn");
                    break;
                case 2:
                    button3.AddToClassList("correct");
                    button3.RemoveFromClassList("answer-btn");
                    break;
                case 3:
                    button4.AddToClassList("correct");
                    button4.RemoveFromClassList("answer-btn");
                    break;
            }
            score+=10;
            var scoreRoot = scoreUI.rootVisualElement;
            scoreRoot.Q<Label>("Score").text = score.ToString();
        } else {
            totalQuestionsIncorrect++;
            switch (answerIndex) {
                case 0:
                    button1.AddToClassList("incorrect");
                    button1.RemoveFromClassList("answer-btn");
                    break;
                case 1:
                    button2.AddToClassList("incorrect");
                    button2.RemoveFromClassList("answer-btn");
                    break;
                case 2:
                    button3.AddToClassList("incorrect");
                    button3.RemoveFromClassList("answer-btn");
                    break;
                case 3:
                    button4.AddToClassList("incorrect");
                    button4.RemoveFromClassList("answer-btn");
                    break;
            }
        }

        totalQuestionsAsked++;
        questionIndex++;
        
        // Start coroutine to hide question box after 2 seconds
        StartCoroutine(HideQuestionBoxAfterDelay());
        
    }

    private bool _isPaused = false;

    public void PauseGame()
    {
        if (questionBox.activeSelf) {
            questionBox.SetActive(false);
        }
        if (_isPaused) {
            ResumeGame();
            return;
        }
        Time.timeScale = 0f;
        AudioListener.pause = true;
        _isPaused = true;
        pausedText.SetActive(true);
        // Disable player controller if present
        if (playerController != null)
        {
            playerController.enabled = false;
        }
    }

    public void ResumeGame()
    {
        if (questionBox.activeSelf) {
            questionBox.SetActive(false);
        }
        if (!_isPaused) {
            PauseGame();
            return;
        }
        Time.timeScale = 1f;
        AudioListener.pause = false;
        _isPaused = false;
        pausedText.SetActive(false);
        // Re-enable player controller if present
        if (playerController != null)
        {
            playerController.enabled = true;
        }
    }

    public void TogglePause()
    {
        if (_isPaused) ResumeGame(); else PauseGame();
    }
}