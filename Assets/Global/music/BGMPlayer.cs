using UnityEngine;
using System.Collections;

public class BGMPlayer {

    //親クラスのStateを作る
    class State
    {
        protected BGMPlayer bgmPlayer;
        public State(BGMPlayer bgmplayers) { this.bgmPlayer = bgmplayers; }
        //子クラスのコンストラクタを宣言する
        public virtual void bgmPlayMode(){}
        public virtual void bgmPauseMode(){}
        public virtual void bgmStopMode(){}
        public virtual void update(){}
    }
    //子クラスを作る

    //
    class Wait : State
    {
        public Wait(BGMPlayer bgmPlayer) : base(bgmPlayer){}
        public override void bgmPlayMode()
        {
            if (bgmPlayer.fadeInTime > 0.0f)
            {
                bgmPlayer.state= new FadeIn(bgmPlayer);
            }
            else
            {
                bgmPlayer.state = new Playing(bgmPlayer);
            }
        }

    }
    //フェードインステート
    class FadeIn : State
    {
        float t = 0.0f;

        public FadeIn(BGMPlayer bgmPlayer) : base(bgmPlayer)
        {
            bgmPlayer.endFadeOut = false;
            bgmPlayer.source.Play();
            bgmPlayer.source.volume = 0.0f;
        }
        public override void bgmPauseMode()
        {
           bgmPlayer.state = new Pause(bgmPlayer, this); 
        }
        public override void bgmStopMode()
        {
           bgmPlayer.state = new FadeOut(bgmPlayer);
        }
        public override void update()
        {
            t += Time.deltaTime;
            if (bgmPlayer.volBalancer)
            {
                bgmPlayer.source.volume = (((t / bgmPlayer.fadeInTime)* 0.5f)* bgmPlayer.Control);
                if (bgmPlayer.source.volume >=(bgmPlayer.Control * 0.5f))
                {
                    bgmPlayer.source.volume = bgmPlayer.Control* 0.5f;
                    bgmPlayer.state = new Playing(bgmPlayer);
                }

            }
            else
            {
                bgmPlayer.source.volume = (t / bgmPlayer.fadeInTime)* bgmPlayer.Control;
                if (t >= bgmPlayer.fadeInTime)
                {
                    bgmPlayer.source.volume = bgmPlayer.Control;
                    bgmPlayer.state = new Playing(bgmPlayer);
                }
            }
        }

    }
    class Playing : State
    {
        public Playing(BGMPlayer bgmPlayer):base (bgmPlayer)
        {
            if(bgmPlayer.source.isPlaying == false)
            {
                if (bgmPlayer.volBalancer)
                {
                    bgmPlayer.source.volume = 0.5f;
                    bgmPlayer.source.Play();
                }
                else
                {
                    bgmPlayer.source.volume = 1.0f;
                    bgmPlayer.source.Play();
                }
            }
        }
        public override void bgmPauseMode()
        {
            bgmPlayer.state = new Pause(bgmPlayer,this);
        }
        public override void bgmStopMode()
        {
            bgmPlayer.state = new FadeOut(bgmPlayer);
        }
        public override void update()
        {
            if (bgmPlayer.changeVol != 0)
            {
                bgmPlayer.source.volume = bgmPlayer.changeVol;
            }
        }
    }

    class Pause : State
    {
        State preState;

        public Pause(BGMPlayer bgmPlayer,State preState) : base(bgmPlayer)
        {
            this.preState = preState;
            bgmPlayer.source.Pause();
        }

        public override void bgmStopMode()
        {
            bgmPlayer.source.Stop();
            bgmPlayer.state = new Wait(bgmPlayer);
        }
        public override void bgmPlayMode()
        {
            bgmPlayer.state = preState;
            bgmPlayer.source.Play();
        }
    }
   
    class FadeOut: State
   {
       float initVolume;
       float t= 0.0f;
        public FadeOut(BGMPlayer bgmPlayer) : base(bgmPlayer)
       {
           initVolume = bgmPlayer.source.volume;
       }
        public override void bgmPauseMode()
        {
            bgmPlayer.state = new Pause(bgmPlayer, this);
        }

        public override void update()
        {
            t += Time.deltaTime;
            bgmPlayer.source.volume = initVolume * (1.0f - t / bgmPlayer.fadeOutTime);
            if(t>= bgmPlayer.fadeOutTime)
            {
                bgmPlayer.source.volume = 0.0f;
                bgmPlayer.source.Stop();
                bgmPlayer.endFadeOut = true;
                bgmPlayer.state = new Wait(bgmPlayer);
            }
        }

   }

    GameObject obj;
    AudioSource source;
    State state;
    //フェードの時間
    float fadeInTime = 0.0f;
    float fadeOutTime = 0.0f;
    bool volBalancer = false;
    bool endFadeOut = false;
    float Control;
    float changeVol;

    public BGMPlayer(){}

    public BGMPlayer(string bgmfilename)
    {
        AudioClip clip = (AudioClip)Resources.Load(bgmfilename);
        if(clip != null)
        {
            obj = new GameObject("BGMPlayer");
            source = obj.AddComponent<AudioSource>();
            source.clip = clip;
            //ステート管理
            state = new Wait(this);
            //テスト段階
            source.loop = true;
        }
        else
        {
            Debug.Log("BGM" + bgmfilename + "notFound");
        }
    }
    //オブジェクト削除
    public void destory() { if (source != null) GameObject.Destroy(obj); }
    //フェードなし再生
    public void playBGM() { if (source != null) state.bgmPlayMode(); }
    //フェードあり再生
    public void playBGM(float fadeTime, bool half,float ControlVol) 
    {
        if(source  != null)
        {
            volBalancer = half;
            Control = ControlVol;
            this.fadeInTime = fadeTime;
            state.bgmPlayMode();
        }
    }
    public void pauseBGM() { if (source != null)state.bgmPauseMode(); }
    //フェードなし停止
    public void stopBGM(){if(source!= null)state.bgmStopMode();}
    //フェードあり停止
    public void stopBGM(float fadeTime)
    {
        if(source != null)
        {
            this.fadeOutTime = fadeTime;
            state.bgmStopMode();
        }
    }
    public void update() { if (source != null) state.update(); }

    public bool hadFadeOut()
    {
        if(endFadeOut)
        {
            return true;
        }
        return false;
    }

    public void contollVOL(float vol)
    {
        changeVol = vol;
    }
}
