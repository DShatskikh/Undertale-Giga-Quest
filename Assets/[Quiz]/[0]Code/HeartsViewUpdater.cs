using System;
using UnityEngine;
using UnityEngine.UIElements;

public class HeartsViewUpdater : MonoBehaviour
{
    [SerializeField]
    private UIDocument _uIDocument;
    
    [SerializeField]
    private AssetProvider _assetProvider;

    public void Updater()
    {
        for (int i = 1; i <= 3; i++)
        {
            var heart3 = _uIDocument.rootVisualElement.Q<VisualElement>($"heart{4 - i}");
            var back3 = heart3.style.backgroundImage.value;
            
            if (GameData.Hearts < i)
            {
                back3.sprite = _assetProvider.BrokenHeart;
            }
            else
            {
                back3.sprite = _assetProvider.NormalHeart;
            }
            
            heart3.style.backgroundImage = back3;
        }
    }
}