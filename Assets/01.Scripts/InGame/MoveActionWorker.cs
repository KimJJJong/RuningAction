using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class MoveActionWorker
{
    List<MoveAction> action_list = new List<MoveAction>();

    public MoveActionWorker AddAction(MoveAction item)
    {
        if (action_list.Count > 0)
        {
            MoveAction lastItem = action_list.Last();
            lastItem.listener.OnFinish(() =>
            {
                item.Play();
            });
        }
        else
        {
            item.Play();
        }

        action_list.Add(item);
        /* item.Pause().SetAutoKill(true).OnStart(() => { });
        
        if (action_list.Count > 0)
        {
            item.OnKill(() =>
            {
                item.Play();
            });
        }
        else
        {
            item.Play();
        } */
        //action_list.Add(item);
        return this;
    }

    public MoveActionWorker AddAction(ActionItemBuilder actionItemBuilder)
    {
        MoveAction item = actionItemBuilder.Build();

        return AddAction(item);
    }

    public MoveActionWorker Clear()
    {
        int loop = action_list.Count;
        int idx = 0;
        while (loop > 0)
        {
            var action = action_list[idx];

            if (action.Kill())
            {
                action_list.Remove(action);
            }
            else
            {
                idx++;
            }

            loop--;
        }

        return this;
    }

    public delegate void ActionCallback();

    public static ActionItemBuilder ActionBuilder()
    {
        return new ActionItemBuilder();
    }

    public class ActionItemBuilder
    {
        Tweener[] tweeners;
        List<ActionCallback> onStartCallback_list = new List<ActionCallback>();
        List<ActionCallback> onCompleteCallback_list = new List<ActionCallback>();
        List<ActionCallback> onFinishCallback_list = new List<ActionCallback>();

        public ActionItemBuilder SetTweener(params Tweener[] tweeners)
        {
            for (int i = 0; i < tweeners.Length; i++)
            {
                bool isFirst = i == 0;
                bool isLast = i == tweeners.Length - 1;

                tweeners[i].Pause().SetAutoKill(true);
            }

            this.tweeners = tweeners;

            return this;
        }

        public ActionItemBuilder AddStartCallBack(ActionCallback callback)
        {
            onStartCallback_list.Add(callback);
            return this;
        }

        public ActionItemBuilder AddCompleteCallback(ActionCallback callback)
        {
            onCompleteCallback_list.Add(callback);
            return this;
        }

        public ActionItemBuilder AddFinishCallBack(ActionCallback callback)
        {
            onFinishCallback_list.Add(callback);
            return this;
        }

        public MoveAction Build()
        {
            MoveAction output = new MoveAction(tweeners);

            foreach (ActionCallback callback in onStartCallback_list)
            {
                output.listener.OnStart(callback);
            }

            foreach (ActionCallback callback in onCompleteCallback_list)
            {
                output.listener.OnComplete(callback);
            }

            foreach (ActionCallback callback in onFinishCallback_list)
            {
                output.listener.OnFinish(callback);
            }

            output.SetTweenerCallback();

            return output;
        }
    }

    public class MoveAction
    {
        Tweener[] tweeners;

        //bool killable = true;
        KillableSetter isKillable = () => true;

        public MoveActionListener listener = new MoveActionListener();

        public MoveAction(params Tweener[] tweener)
        {
            this.tweeners = tweener;
            //this.killable = true;
        }

        public MoveAction(bool killable, params Tweener[] tweener)
        {
            this.tweeners = tweener;
            this.isKillable = () => killable;
        }

        public MoveAction(KillableSetter killableSetter, params Tweener[] tweener)
        {
            this.tweeners = tweener;
            this.isKillable = killableSetter;
        }

        public void SetTweenerCallback()
        {
            for (int i = 0; i < tweeners.Length; i++)
            {
                bool isFirst = i == 0;
                bool isLast = i == tweeners.Length - 1;

                Tweener tweener = tweeners[i];

                if (isFirst)
                {
                    TweenCallback tweenCallback = tweener.onPlay;

                    //tweener.onComplete
                    //tweener.OnPl
                    tweener.OnPlay(() =>
                    {
                        listener.Start();
                        tweenCallback?.Invoke();
                    });
                }

                if (isLast)
                {
                    TweenCallback tweenOnCompleteCallback = tweener.onComplete;
                    TweenCallback tweenOnKillCallback = tweener.onKill;
                    tweener
                        .OnComplete(() =>
                        {
                            tweenOnCompleteCallback?.Invoke();
                            listener.Complete();
                        })
                        .OnKill(() =>
                        {
                            tweenOnKillCallback?.Invoke();
                            listener.Finish();
                        });
                }
                else
                {
                    Tweener nextTweener = tweeners[i + 1];
                    TweenCallback tweenCallback = tweener.onKill;
                    tweener.OnKill(() =>
                    {
                        tweenCallback?.Invoke();
                        nextTweener.Play();
                    });
                }
            }
        }

        public void Play()
        {
            if (tweeners.Length == 0)
            {
                listener.Finish();
                return;
            }

            tweeners[0].Play();
        }

        public bool Kill()
        {
            bool result = isKillable();
            if (result)
            {
                foreach (Tweener tweener in tweeners)
                    tweener.Kill();
            }
            /* if (result)
                tweener.Kill(); */

            return result;
        }

        public Tweener GetTask()
        {
            return tweeners[0];
        }

        public delegate bool KillableSetter();
    }

    public class MoveActionListener
    {
        protected event ActionCallback onStart;
        protected event ActionCallback onComplete;
        protected event ActionCallback onFinish;

        public void Start()
        {
            onStart?.Invoke();
        }

        public void Complete()
        {
            onComplete?.Invoke();
        }

        public void Finish()
        {
            onFinish?.Invoke();
        }

        public MoveActionListener OnStart(ActionCallback callback)
        {
            onStart += callback;
            return this;
        }

        public MoveActionListener OnComplete(ActionCallback callback)
        {
            onComplete += callback;
            return this;
        }

        public MoveActionListener OnFinish(ActionCallback callback)
        {
            onFinish += callback;
            return this;
        }
    }
}
