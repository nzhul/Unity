using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace Assets.Scripts
{
    /// <summary>
    /// For more information: https://catlikecoding.com/unity/tutorials/tower-defense/animation/
    /// </summary>
    [System.Serializable]
    public struct EnemyAnimator
    {
        const float transitionSpeed = 5f;

        /// <summary>
        /// Controlling the animation state of an object is done via a playable graph, 
        /// which exists in native code and not in C#. We can control it via a PlayableGraph struct, 
        /// which contains a reference to the native data. 
        /// A graph is created via the static PlayableGraph.Create method. 
        /// All Playables are created in a similar way.
        /// </summary>
        PlayableGraph graph;

        /// <summary>
        /// To support multiple animations we have to add an animation mixer to EnemyAnimator. 
        /// Give it an AnimationMixerPlayable field to keep track of it.
        /// </summary>
        AnimationMixerPlayable mixer;

        public Clip CurrentClip { get; private set; }

        public bool IsDone => GetPlayable(CurrentClip).IsDone();

        Clip previousClip;

        float transitionProgress;

        public void Configure(Animator animator, EnemyAnimationConfig config)
        {
            graph = PlayableGraph.Create();
            graph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);
            mixer = AnimationMixerPlayable.Create(graph, 4);

            /// To play an animation clip we first have to create a playable representation of it, 
            /// via AnimationClipPlayable.Create. We have to provide the graph it belongs to and the animation clip as arguments.
            /// 
            /// The easiest way to add a clip to a mixer is by invoking ConnectInput on the mixer with the clip's 
            /// index and the playable clip as arguments. A third argument specifies the output index of the clip, which is always zero. 
            /// Do this for all three clips in Configure.
            var clip = AnimationClipPlayable.Create(graph, config.Move);
            clip.Pause();
            mixer.ConnectInput((int)Clip.Move, clip, 0);

            clip = AnimationClipPlayable.Create(graph, config.Intro);
            clip.SetDuration(config.Intro.length);
            mixer.ConnectInput((int)Clip.Intro, clip, 0);

            clip = AnimationClipPlayable.Create(graph, config.Outro);
            clip.SetDuration(config.Outro.length);
            clip.Pause();
            mixer.ConnectInput((int)Clip.Outro, clip, 0);

            clip = AnimationClipPlayable.Create(graph, config.Dying);
            clip.SetDuration(config.Dying.length);
            clip.Pause();
            mixer.ConnectInput((int)Clip.Dying, clip, 0);

            var output = AnimationPlayableOutput.Create(graph, "Enemy", animator);
            output.SetSourcePlayable(mixer);
        }

        public void PlayIntro()
        {
            SetWeight(Clip.Intro, 1f);
            CurrentClip = Clip.Intro;
            graph.Play();
            transitionProgress = -1f;
        }

        public void PlayMove(float speed)
        {
            //SetWeight(CurrentClip, 0f);
            //SetWeight(Clip.Move, 1f);
            //var clip = GetPlayable(Clip.Move);
            //clip.SetSpeed(speed);
            //clip.Play();
            //CurrentClip = Clip.Move;

            GetPlayable(Clip.Move).SetSpeed(speed);
            BeginTransition(Clip.Move);
        }

        public void PlayOutro()
        {
            //SetWeight(CurrentClip, 0f);
            //SetWeight(Clip.Outro, 1f);
            //GetPlayable(Clip.Outro).Play();
            //CurrentClip = Clip.Outro;

            BeginTransition(Clip.Outro);
        }

        public void PlayDying()
        {
            BeginTransition(Clip.Dying);
        }

        public void Stop()
        {
            graph.Stop();
        }

        public void Destroy()
        {
            graph.Destroy();
        }

        public void GameUpdate()
        {
            if (transitionProgress >= 0)
            {
                transitionProgress += Time.deltaTime * transitionSpeed;
                if (transitionProgress >= 1f)
                {
                    transitionProgress = -1f;
                    SetWeight(CurrentClip, 1f);
                    SetWeight(previousClip, 0f);
                    GetPlayable(previousClip).Pause();
                }
                else
                {
                    SetWeight(CurrentClip, transitionProgress);
                    SetWeight(previousClip, 1f - transitionProgress);
                }
            }
        }

        private void SetWeight(Clip clip, float weight)
        {
            mixer.SetInputWeight((int)clip, weight);
        }

        private Playable GetPlayable(Clip clip)
        {
            return mixer.GetInput((int)clip);
        }

        private void BeginTransition(Clip nextClip)
        {
            previousClip = CurrentClip;
            CurrentClip = nextClip;
            transitionProgress = 0f;
            GetPlayable(nextClip).Play();
        }

        public enum Clip
        {
            Move,
            Intro,
            Outro,
            Dying
        }
    }
}