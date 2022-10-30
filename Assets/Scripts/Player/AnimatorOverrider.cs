using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class AnimatorOverrider : MonoBehaviour
{
    private Animator anim;
    [SerializeField] AnimatorOverrideController defaultController;
    [SerializeField] AnimatorOverrideController ogreController;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void SetOverrideAnimator(Component sender, object data)
    {
        if (data is string)
        {
            switch(data)
            {
                case "default":
                    anim.runtimeAnimatorController = defaultController;
                    break;
                case "ogre":
                    anim.runtimeAnimatorController = ogreController;
                    break;
            }
        }

        anim.Play("Player_Idle_Down");
    }

}
