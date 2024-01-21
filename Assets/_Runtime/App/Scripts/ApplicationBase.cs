using System;
using System.Collections;
using System.Collections.Generic;
using AK.StateMachine;
using UnityEngine;
using Views;

public class ApplicationBase : MonoBehaviour
{
    [SerializeField] private FiniteStateMachine FiniteStateMachine;
    [SerializeField] private ServicesProvider   ServicesProvider;
    private                  Coroutine          _stateMachineRoutine = null;

    private void Start()
    {
        ServicesProvider.InitServices();
        _stateMachineRoutine = StartCoroutine(FiniteStateMachine.Tick());
    }
}