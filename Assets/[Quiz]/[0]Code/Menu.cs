using UnityEngine;
using UnityEngine.UIElements;

public class Menu : MonoBehaviour
{
    public UIDocument UIDocument;
    public Game Game;
    
    [SerializeField]
    private AssetProvider _assetProvider;

    [SerializeField] 
    private HeartsViewUpdater _heartsView;
    
    [SerializeField]
    private SoundManager _soundManager;
    
    private void OnEnable()
    {
        var easyButton = UIDocument.rootVisualElement.Q<Button>("easy_start_button");
        easyButton.clicked += () => ShowGame(0);

        var middleButton = UIDocument.rootVisualElement.Q<Button>("middle_start_button");
        middleButton.clicked += () => ShowGame(1);
        
        var hardButton = UIDocument.rootVisualElement.Q<Button>("hard_start_button");
        hardButton.clicked += () => ShowGame(2);
        
        TestViewUpdate(1, out var score1);
        TestViewUpdate(2, out var score2);
        TestViewUpdate(3, out var score3);
        
        var allScoreLabel = UIDocument.rootVisualElement.Q<Label>("all_score_label");
        allScoreLabel.text = $"Всего очков: {score1 + score2 + score3}";

        if (GameData.Hearts < 1)
            GameData.Hearts = 1;
        
        _heartsView.Updater();
        
        var topButton = UIDocument.rootVisualElement.Q<Button>("top_button");
        topButton.clicked += OnTopButtonClicked;
        
        var adsButton = UIDocument.rootVisualElement.Q<Button>("ads_button");
        adsButton.clicked += OnAdsButtonClicked;
    }

    private void OnAdsButtonClicked()
    {
        _soundManager.ClickPlay();
        GameData.Hearts++;
        _heartsView.Updater();
    }

    private void ShowGame(int index)
    {
        _soundManager.ClickPlay();
        
        Game.Show(index);
        gameObject.SetActive(false);
    }

    private void TestViewUpdate(int index, out int score)
    {
        var easyCountQuestionsLabel = UIDocument.rootVisualElement.Q<Label>($"count_questions{index}_label");
        easyCountQuestionsLabel.text = $"{_assetProvider.LevelsData[index - 1].Questions.Length} Вопросов";
        
        var score1 = PlayerPrefs.GetInt($"Test{index - 1}", -1);
        
        if (score1 > -1)
        {
            var easyLabel = UIDocument.rootVisualElement.Q<Label>($"state{index}_label");
            easyLabel.text = "Пройдено";
            
            var easyScoreLabel = UIDocument.rootVisualElement.Q<Label>($"score{index}_label");
            easyScoreLabel.text = $"Очки: {score1}";
        }

        score = score1;

        if (score < 0)
            score = 0;
    }

    private void OnTopButtonClicked()
    {
        _soundManager.ClickPlay();
    }
}