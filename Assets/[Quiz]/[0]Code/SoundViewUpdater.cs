using System;
using UnityEngine;
using UnityEngine.UIElements;

public class SoundViewUpdater : MonoBehaviour
{
    [SerializeField]
    private UIDocument _uIDocument;
    
    [SerializeField]
    private AssetProvider _assetProvider;
    
    [SerializeField]
    private SoundManager _soundManager;
    
    private void OnEnable()
    {
        var soundButton = _uIDocument.rootVisualElement.Q<Button>("sound_button");
        soundButton.clicked += OnSoundButtonClicked;
        
        SoundViewUpdate( PlayerPrefs.GetInt("Sound", 1) == 0);
    }

    private void OnSoundButtonClicked()
    {
        _soundManager.ClickPlay();
        
        var mute = PlayerPrefs.GetInt("Sound", 1) == 0;
        mute = !mute;
        PlayerPrefs.SetInt("Sound", mute ? 0 : 1);
        AudioListener.pause = mute;
        SoundViewUpdate(mute);
    }
    
    private void SoundViewUpdate(bool isMute)
    {
        var soundButton = _uIDocument.rootVisualElement.Q<Button>("sound_button");
        var back = soundButton.style.backgroundImage.value;
        back.sprite = isMute ? _assetProvider.SoundOff : _assetProvider.SoundOn;
        soundButton.style.backgroundImage = back;
    }
}