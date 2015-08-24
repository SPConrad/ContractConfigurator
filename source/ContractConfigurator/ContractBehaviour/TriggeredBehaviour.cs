﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using KSP;
using Contracts;
using ContractConfigurator;
using ContractConfigurator.Parameters;
using ContractConfigurator.ExpressionParser;

namespace ContractConfigurator.Behaviour
{
    /// <summary>
    /// Abstract behaviour with a trigger state.
    /// </summary>
    public abstract class TriggeredBehaviour : ContractBehaviour
    {
        public enum State
        {
            ContractAccepted,
            ContractCompletedFailure,
            ContractCompletedSuccess,
            ParameterCompleted,
        }

        private State onState;
        private List<string> parameter;

        public TriggeredBehaviour()
        {
        }

        public TriggeredBehaviour(State onState, List<string> parameter)
        {
            this.onState = onState;
            this.parameter = parameter;
        }

        protected override void OnAccepted()
        {
            if (onState == State.ContractAccepted)
            {
                TriggerAction();
            }
        }

        protected override void OnCancelled()
        {
            if (onState == State.ContractCompletedFailure)
            {
                TriggerAction();
            }
        }

        protected override void OnDeadlineExpired()
        {
            if (onState == State.ContractCompletedFailure)
            {
                TriggerAction();
            }
        }

        protected override void OnFailed()
        {
            if (onState == State.ContractCompletedFailure)
            {
                TriggerAction();
            }
        }

        protected override void OnCompleted()
        {
            if (onState == State.ContractCompletedSuccess)
            {
                TriggerAction();
            }
        }

        protected override void OnParameterStateChange(ContractParameter param)
        {
            if (onState == State.ParameterCompleted && param.State == ParameterState.Complete && parameter.Contains(param.ID))
            {
                TriggerAction();
            }
        }

        protected override void OnLoad(ConfigNode configNode)
        {
            onState = ConfigNodeUtil.ParseValue<State>(configNode, "onState");
            parameter = ConfigNodeUtil.ParseValue<List<string>>(configNode, "parameter", new List<string>());
        }

        protected override void OnSave(ConfigNode configNode)
        {
            configNode.AddValue("onState", onState);
            foreach (string p in parameter)
            {
                configNode.AddValue("parameter", p);
            }
        }

        /// <summary>
        /// Called when the action needs to be triggered.
        /// </summary>
        protected abstract void TriggerAction();
    }
}