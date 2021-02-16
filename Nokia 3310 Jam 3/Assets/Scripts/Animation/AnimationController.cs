using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public static event Action AnimationDone;

    [SerializeField] private PlayerDestination playerDestination = null;

    [SerializeField] private GameObject playerGO = null;
    private Animator objectAnimator = null;

    private bool isAnimating = false;
    private float runningTime;
    private float animationEndTime;

    private void Start()
    {
        PlayerMovement.ReadyToAnimate += Animate;
        objectAnimator = GetComponent<Animator>();
    }

    private void OnDestroy()
    {
        PlayerMovement.ReadyToAnimate -= Animate;
    }

    private void Update()
    {
        if (!isAnimating) { return; }

        runningTime += Time.deltaTime;

        if (runningTime >= animationEndTime)
        {
            EndAnimate();
        }
    }

    private void Animate(string destinationName)
    {
        if (gameObject.name != destinationName) { return; }

        string actionCode = playerDestination.GetPlayerAction().ToString();
        int code = playerDestination.GetPlayerAction();

        //Hide the player's game object.
        playerGO.SetActive(false);

        //Do the animation.
        objectAnimator.SetBool(actionCode, true);

        SetTimers(code);
    }

    private void EndAnimate()
    {
        string actionCode = playerDestination.GetPlayerAction().ToString();

        //Remove the animation.
        objectAnimator.SetBool(actionCode, false);

        if (!objectAnimator.GetCurrentAnimatorStateInfo(0).IsName("Default")) { return; }

        //Show the player's game object.
        playerGO.SetActive(true);

        ClearTimers();
        AnimationDone?.Invoke();
    }

    private void SetTimers(int actionCode)
    {        
        isAnimating = true;
        runningTime = Time.time;
        animationEndTime = runningTime + CheckAnimationTime(actionCode);
    }

    private float CheckAnimationTime(int actionCode)
    {
        switch (actionCode)
        {
            case 0:
                return 7;
            case 1:
            case 5:
                return 4;
            case 2:
                return 3;
            case 3:
                return 1;
            case 4:
                return 8;
            case 6:
            case 7:
            case 8:
            case 9:
                return 5;                
            default:
                return 0;
        }
    }

    private void ClearTimers()
    {
        isAnimating = false;
        runningTime = 0f;
        animationEndTime = 0f;
    }
}
