using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    private static AnimationManager _instance;

    public static AnimationManager Instance
    {
        get
        {
            if (_instance is null)
                Debug.LogWarning("AnimationManager is NULL");
            return _instance;
        }
    }


    public Animator fadeAnimator;

    private int _endOfTheDayHash;
    private int _exitSceneHash;
    private int _loseHash;

    public Animator dayAnimator;
    private int _pazienteInHash;
    private int _pazienteOutHash;

    private void Awake()
    {
        // -- Singleton
        if (_instance == null)
        {
            //First run, set the _instance
            _instance = this;
            DontDestroyOnLoad(gameObject);

        }
        else if (_instance != this)
        {
            //_instance is not the same as the one we have, destroy old one, and reset to newest one
            fadeAnimator = (fadeAnimator == null) ? _instance.fadeAnimator : fadeAnimator;

            Destroy(_instance.gameObject);
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        _endOfTheDayHash = Animator.StringToHash("EndOfTheDay");
        _exitSceneHash = Animator.StringToHash("ExitScene"); 
        _loseHash = Animator.StringToHash("Lose");
        _pazienteInHash = Animator.StringToHash("PazienteIn");
        _pazienteOutHash= Animator.StringToHash("PazienteOut");
        
    }



    public void AnimationForEndOfTheDay()
    {
        fadeAnimator.SetTrigger(_endOfTheDayHash);
    }

    public void AnimationForExitScene()
    {
        fadeAnimator.SetTrigger(_exitSceneHash);
    }

    public void AnimationLose() => fadeAnimator.SetTrigger(_loseHash);
    public void AnimationPazienteIn() => dayAnimator.SetTrigger(_pazienteInHash);

    public void AnimationPazienteOut() => dayAnimator?.SetTrigger(_pazienteOutHash);

    public void DisableDayAnimator () => dayAnimator.enabled = false;
}
