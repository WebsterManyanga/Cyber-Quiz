[System.Serializable]
public class Question
{
    public string name;
    public string question;
    public string[] options;
    public int answer;
}

[System.Serializable]
public class QuestionList {
    public Question[] questions;

}