using UnityEngine;

public class QuestionTrigger : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject gameManager;
    public GameObject character1;
    public GameObject character2;
    public GameObject character3;
    public GameObject character4;
    public GameObject character5;
    public GameObject character6;
    public GameObject character7;
    public GameObject character8;
    public GameObject character9;
    public GameObject character10;
    public GameObject character11;
    public GameObject[] characters;
    public GameObject arrow;
    
    public bool stopMoving = false;
    int characterIndex;
    bool characterHasAQuestion = false;
    string avatar;

    void Start()
    {
        avatar = "";
        gameManager = GameObject.Find("Game Manager");
        characters = new GameObject[] { character1, character2, character3, character4, character5, character6, character7, character8, character9, character10, character11};
        if (gameManager.GetComponent<GameManager>().nextCharacter < characters.Length) 
        {gameManager.GetComponent<GameManager>().nextCharacter++;} 
        else 
        {gameManager.GetComponent<GameManager>().nextCharacter = 0;}
        characterIndex = gameManager.GetComponent<GameManager>().nextCharacter;
        characters[characterIndex].SetActive(true);
        characterHasAQuestion = Random.Range(0, 4) == 1;
        arrow.SetActive(characterHasAQuestion);
        
        switch (characterIndex) {
            case 0:
                avatar = "Fred";
                break;
            case 1:
                avatar = "Elvis";
                break;
            case 2:
                avatar = "Trevor";
                break;
            case 3:
                avatar = "Meemaw";
                break;
            case 4:
                avatar = "Riley";
                break;
            case 5:
                avatar = "Skipper";
                break;
            case 6:
                avatar = "Luna";
                break;
            case 7:
                avatar = "Lola";
                break;
            case 8:
                avatar = "Kelly";
                break;
            case 9:
                avatar = "Max";
                break;
            case 10:
                avatar = "Zombody";
                break;

        }
        
        if (!characterHasAQuestion) {
            gameObject.GetComponent<BoxCollider>().isTrigger = false;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!stopMoving) {     
        gameObject.transform.position += Vector3.back * Time.deltaTime;
        }

        if (gameObject.transform.position.z <= -20) {
            Destroy(gameObject);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (!characterHasAQuestion) {
            return;
        }

        if (other.tag == "Player") {
            gameManager.GetComponent<GameManager>().QuestionTrigger(avatar);
            // gameManager.GetComponent<GameManager>().questionAsked = true;\
            characters[characterIndex].GetComponent<Animator>().SetTrigger("Stop");
            stopMoving = true;
        }


    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player") {
            gameManager.GetComponent<GameManager>().questionAsked = false;
            Destroy(gameObject, 0.5f);
        }
    }
}
