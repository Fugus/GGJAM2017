﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ImportBumperBot : MonoBehaviour
{
#if UNITY_EDITOR
    public bool DoImport = false;

    [SerializeField]
    private RuntimeAnimatorController characterAnimatorController;

    private void Update()
    {
        if (DoImport)
        {
            DoImport = false;

            ThirdPersonCharacter characterController = GetComponent<ThirdPersonCharacter>();

            Transform bumperBotTransform = transform.FindChild("bumperBot");
            Transform reactorTransform = bumperBotTransform.FindChild("bumper_reactor");
            Transform platformTransform = bumperBotTransform.FindChild("bumper_platform");
            Transform botTransform = platformTransform.FindChild("bumperBot3_1");


            characterController.m_Animator = botTransform.GetComponent<Animator>();
			characterController.m_Animator.runtimeAnimatorController = characterAnimatorController;
			
            if (reactorTransform.GetComponent<OrientReactor>() == null)
            {
                reactorTransform.gameObject.AddComponent<OrientReactor>();
            }
        }

    }

#endif // UNITY_EDITOR
}