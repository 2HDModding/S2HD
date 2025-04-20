// Decompiled with JetBrains decompiler
// Type: SonicOrca.FiniteStateMachine
// Assembly: S2HD, Version=2.0.1012.10521, Culture=neutral, PublicKeyToken=null
// MVID: 18631A0F-16CF-4E18-8563-1EC5E54750D6
// Assembly location: C:\Games\S2HD_2.0.1012-rc2\S2HD.exe

using System;

namespace SonicOrca
{

    internal class FiniteStateMachine
    {
      private FiniteStateMachine.State _currentState;
      private bool _reInitialised;

      public FiniteStateMachine.IState Begin()
      {
        this._reInitialised = true;
        return (FiniteStateMachine.IState) (this._currentState = new FiniteStateMachine.State(this));
      }

      public void Finish()
      {
        this._reInitialised = true;
        this._currentState = (FiniteStateMachine.State) null;
      }

      public bool Update()
      {
        if (this._currentState == null)
          return false;
        this._reInitialised = false;
        FiniteStateMachine.State state = this._currentState.Update();
        if (!this._reInitialised)
          this._currentState = state;
        return true;
      }

      public interface IState
      {
        FiniteStateMachine.IState Do(Action action);

        FiniteStateMachine.IState While(Func<bool> condition);

        FiniteStateMachine.IState While(Func<bool> condition, Action action);

        FiniteStateMachine.IState Wait(int ticks);
      }

      private class State : FiniteStateMachine.IState
      {
        private readonly FiniteStateMachine _finiteStateMachine;
        private readonly Action _action;
        private FiniteStateMachine.State _next;
        private bool _finished;

        public State(FiniteStateMachine finiteStateMachine)
        {
          this._finiteStateMachine = finiteStateMachine;
        }

        private State(FiniteStateMachine finiteStateMachine, Action action)
        {
          this._finiteStateMachine = finiteStateMachine;
          this._action = action;
        }

        public FiniteStateMachine.State Update()
        {
          if (this._finiteStateMachine._reInitialised)
            return (FiniteStateMachine.State) null;
          if (this._action == null)
            return this._next == null ? (FiniteStateMachine.State) null : this._next.Update();
          this._action();
          if (!this._finished)
            return this;
          return this._next != null ? this._next.Update() : (FiniteStateMachine.State) null;
        }

        public FiniteStateMachine.IState Do(Action action)
        {
          FiniteStateMachine.State state1 = new FiniteStateMachine.State(this._finiteStateMachine, action);
          state1._finished = true;
          FiniteStateMachine.State state2 = state1;
          this._next = state1;
          return (FiniteStateMachine.IState) state2;
        }

        public FiniteStateMachine.IState While(Func<bool> condition)
        {
          return (FiniteStateMachine.IState) (this._next = new FiniteStateMachine.State(this._finiteStateMachine, (Action) (() =>
          {
            if (condition())
              return;
            this._next._finished = true;
          })));
        }

        public FiniteStateMachine.IState While(Func<bool> condition, Action action)
        {
          return (FiniteStateMachine.IState) (this._next = new FiniteStateMachine.State(this._finiteStateMachine, (Action) (() =>
          {
            if (condition())
              action();
            else
              this._next._finished = true;
          })));
        }

        public FiniteStateMachine.IState Wait(int ticks)
        {
          return (FiniteStateMachine.IState) (this._next = new FiniteStateMachine.State(this._finiteStateMachine, (Action) (() =>
          {
            if (ticks-- > 0)
              return;
            this._next._finished = true;
          })));
        }
      }
    }
}
