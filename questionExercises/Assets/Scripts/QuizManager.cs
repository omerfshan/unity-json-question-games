using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class QuizManager : MonoBehaviour
{
    [System.Serializable]
    public class Question
    {
        public string question;
        public string[] choices;
        public int correctAnswer;
    }

    [System.Serializable]
    public class QuestionList
    {
        public List<Question> questions;
    }

    public QuestionList questionList;

    // TextMeshPro ve Button referansları
    public TMP_Text questionText;
    public Button[] answerButtons;

    private int currentQuestionIndex = 0; // Şu anki sorunun indexi

    void Start()
    {
        LoadQuestions();
        DisplayQuestion(currentQuestionIndex); // İlk soruyu göster
    }

    void LoadQuestions()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("questions"); // JSON dosyasını yüklüyoruz
        questionList = JsonUtility.FromJson<QuestionList>(jsonFile.text);
    }

    // Soruyu ve şıkları UI'ye göster
    void DisplayQuestion(int index)
    {
        try
        {
            // Sorular bittiyse exception fırlatılacak
            Question currentQuestion = questionList.questions[index];
            questionText.text = currentQuestion.question; // Soruyu ekrana yazdır

            // Şıkları düğmelere yerleştir
            for (int i = 0; i < answerButtons.Length; i++)
            {
                answerButtons[i].GetComponentInChildren<TMP_Text>().text = currentQuestion.choices[i];
                int choiceIndex = i; // Lambda fonksiyonunda kullanılmak üzere local değişken
                answerButtons[i].onClick.RemoveAllListeners(); // Önceki eventleri kaldır
                answerButtons[i].onClick.AddListener(() => OnAnswerSelected(choiceIndex)); // Yeni event ekle
            }
        }
        catch (System.ArgumentOutOfRangeException)
        {
            Debug.Log("Sorular bitti!");
            questionText.text = "Tüm sorular tamamlandı!";
            // Şıkları devre dışı bırak
            foreach (var button in answerButtons)
            {
                button.interactable = false;
            }
        }
    }

    // Şık seçildiğinde bu fonksiyon çalışır
    void OnAnswerSelected(int choiceIndex)
    {
        Question currentQuestion = questionList.questions[currentQuestionIndex];

        // Cevabın doğru olup olmadığını kontrol et
        if (choiceIndex == currentQuestion.correctAnswer)
        {
            Debug.Log("Doğru");
            // Doğruysa bir sonraki soruya geç
            currentQuestionIndex++;
            DisplayQuestion(currentQuestionIndex);
        }
        else
        {
            Debug.Log("Yanlış");
            // Yanlışsa aynı soruyu göster
            DisplayQuestion(currentQuestionIndex);
        }
    }
}
