using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class Game : MonoBehaviour
{
    public UIDocument UIDocument;
    public Menu Menu;
    public UIDocument End;

    [SerializeField] 
    private AssetProvider _assetProvider;

    [SerializeField] 
    private ParticleSystem _particleSystem;
    
    [SerializeField] 
    private HeartsViewUpdater _heartsView;
    
    [SerializeField]
    private SoundManager _soundManager;
    
    private LevelData _levelData;
    private int _indexCurrentQuestion = 0;
    private float _testScore = 0f;
    private bool _isPause;
    private int _levelIndex;

    private void OnEnable()
    {
        var menuButton = UIDocument.rootVisualElement.Q<Button>("settings_button");
        menuButton.clicked += OnMenuButtonClicked;
                
        _heartsView.Updater();
    }

    private void Start()
    {
        Show(0);
    }

    private void Update()
    {
        if (_isPause)
            return;
        
        if (_testScore > 0)
            _testScore -= Time.deltaTime * 2;
        else if (_testScore < 0)
            _testScore = 0;
        
        var scoreLabel = UIDocument.rootVisualElement.Q<Label>("score_label");
        scoreLabel.text = $"{Mathf.CeilToInt(_testScore)}";
    }

    public void Show(int index)
    {
        gameObject.SetActive(true);
        _isPause = false;

        _levelIndex = index;
        _levelData = _assetProvider.LevelsData[index];
        _indexCurrentQuestion = 0;
        _testScore = 100;
        
        QuestionViewUpdate();
    }

    public void Next()
    {
        _indexCurrentQuestion++;
        QuestionViewUpdate();
    }

    private void QuestionViewUpdate()
    {
        var currentQuestion = _levelData.Questions[_indexCurrentQuestion];
        
        var questionLabel = UIDocument.rootVisualElement.Q<Label>("question_label");
        questionLabel.text = currentQuestion.Title;
        
        var questionButton1 = UIDocument.rootVisualElement.Q<Button>("question_button1");
        questionButton1.text = currentQuestion.Questions[0];
        
        var questionButton2 = UIDocument.rootVisualElement.Q<Button>("question_button2");
        questionButton2.text = currentQuestion.Questions[1];
        
        var questionButton3 = UIDocument.rootVisualElement.Q<Button>("question_button3");
        questionButton3.text = currentQuestion.Questions[2];
        
        var questionButton4 = UIDocument.rootVisualElement.Q<Button>("question_button4");
        questionButton4.text = currentQuestion.Questions[3];
        
        var questionImage = UIDocument.rootVisualElement.Q<VisualElement>("question_image");
        var back = questionImage.style.backgroundImage.value;
        back.sprite = currentQuestion.Image;
        questionImage.style.backgroundImage = back;
        
        var progressLabel = UIDocument.rootVisualElement.Q<Label>("progress_label");
        progressLabel.text = $"{_indexCurrentQuestion + 1} ИЗ {_levelData.Questions.Length}";

        var testScoreLabel = UIDocument.rootVisualElement.Q<Label>("test_score_label");
        var testScore = PlayerPrefs.GetInt($"Test{_levelIndex}", -1);

        if (testScore == -1)
            testScore = 0;
        
        testScoreLabel.text = testScore.ToString();
        
        for (int i = 0; i < 4; i++)
        {
            var button = UIDocument.rootVisualElement.Q<Button>($"question_button{i+1}");
            
            button.clicked -= OnRightButtonClicked;
            button.clicked -= OnWrongButtonClicked;
            
            if (i == (int)_levelData.Questions[_indexCurrentQuestion].Correct)
                button.clicked += OnRightButtonClicked;
            else
                button.clicked += OnWrongButtonClicked;
        }
    }

    private void OnMenuButtonClicked()
    {
        _soundManager.ClickPlay();
        gameObject.SetActive(false);
        Menu.gameObject.SetActive(true);
    }

    private void OnRightButtonClicked() => 
        StartCoroutine(nameof(RightProcess));

    private void OnWrongButtonClicked() => 
        StartCoroutine(nameof(WrongProcess));

    private IEnumerator WrongProcess()
    {
        _soundManager.ClickPlay();
        End.gameObject.SetActive(true);
        _isPause = true;
        
        yield return null;
        
        var background = End.rootVisualElement.Q<VisualElement>("Background");
        background.AddToClassList("wrongOpen");
        
        yield return new WaitForSeconds(1f);
        
        _soundManager.WrongPlay();
        background.RemoveFromClassList("wrongOpen");
        
        GameData.Hearts--;
        _heartsView.Updater();
        
        yield return new WaitForSeconds(1f);
        End.gameObject.SetActive(false);

        if (GameData.Hearts > 0)
        {
            
        }
        else
        {
            gameObject.SetActive(false);
            Menu.gameObject.SetActive(true);
        }
    }
    
    private IEnumerator RightProcess()
    {
        _soundManager.ClickPlay();
        _particleSystem.Play();
        End.gameObject.SetActive(true);
        _isPause = true;
        
        yield return null;
        
        var background = End.rootVisualElement.Q<VisualElement>("Background");
        background.AddToClassList("winOpen");
        
        yield return new WaitForSeconds(1.0f);
        
        _soundManager.RightPlay();
        background.RemoveFromClassList("winOpen");
        
        yield return new WaitForSeconds(0.5f);
        
        End.gameObject.SetActive(false);

        if (_levelData.Questions.Length - 1 > _indexCurrentQuestion)
        {
            Next();
            _isPause = false;
        }
        else
        {
            PlayerPrefs.SetInt($"Test{_levelIndex}", Mathf.CeilToInt(_testScore));
            
            gameObject.SetActive(false);
            Menu.gameObject.SetActive(true);
        }
    }

    [ContextMenu("Ответить правильно")]
    private void TestWin() => StartCoroutine(nameof(RightProcess));
    
    [ContextMenu("Ошибиться")]
    private void TestWrong() => StartCoroutine(nameof(WrongProcess));
}