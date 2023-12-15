using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] 
    private AudioSource _clickSource, _wrongSource, _rightSource;

    public void ClickPlay() => _clickSource.Play();

    public void WrongPlay() => _wrongSource.Play();

    public void RightPlay() => _rightSource.Play();
}