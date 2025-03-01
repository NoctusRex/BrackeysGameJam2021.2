﻿using Bliss.Component.Sprites.Ui;
using Bliss.Manager;
using Bliss.Models;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using Color = Microsoft.Xna.Framework.Color;

namespace Bliss.Component.Sprites.Office
{
    public class Phone : Clickable
    {
        public event EventHandler OnImportantCallFinished;
        public event EventHandler OnVoiceLineStart;

        private Dictionary<string, Animation> Animations { get; set; }

        private SoundEffectInstance RingingSoundEffect { get; set; }
        private SoundEffectInstance CallOverSoundEffect { get; set; }
        private double Timer { get; set; }

        private PhoneCall PhoneCall { get; set; }
        private int CurrentVoiceLine { get; set; }
        public TextBox TextBox { get; set; }

        private PlayerStats PlayerStats { get; set; }

        public float SecondsBeforeMissedCall { get; set; } = 1;
        public bool IsRinging { get; private set; }
        public bool IsTalking { get; private set; }
        public bool IsCallOver { get; private set; }

        public bool IsInUse { get; private set; }

        public Phone(PlayerStats playerStats)
        {
            HoverColor = Color.Yellow;
            PlayerStats = playerStats;

            Texture = ContentManager.PhoneTexture;
            AnimationManager.Parent = this;
            AnimationManager.Scale = (SizeManager.JamGame.HeightScaleFactor) / 4;

            Animations = new Dictionary<string, Animation>()
            {
                {
                    "idle", new Animation(ContentManager.PhoneIdleAnimation, 1)
                },
                {
                    "ringing", new Animation(ContentManager.PhoneRingingAnimation, 2) { FrameSpeed = 0.5f }
                },
                {
                    "talking", new Animation(ContentManager.PhonePickedUpAnimation, 1)
                },
                {
                    "callOver", new Animation(ContentManager.PhoneCallOverAnimation, 1)
                },
            };

            AnimationManager.Play(Animations["idle"]);

            CallOverSoundEffect = AudioManager.PlayEffect(ContentManager.PhoneCallOverSoundEffect, isLooped: true);
            CallOverSoundEffect.Stop();
            RingingSoundEffect = AudioManager.PlayEffect(ContentManager.PhoneRingingSoundEffect, isLooped: true);
            RingingSoundEffect.Stop();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            HoverColor = !IsRinging && !IsTalking ? Color : Color.Yellow;

            if (IsRinging)
            {
                Timer += gameTime.ElapsedGameTime.TotalSeconds;

                if (Timer >= SecondsBeforeMissedCall)
                {
                    PlayerStats.MissedCalls++;
                    IsRinging = false;
                    IsInUse = false;
                    if (PhoneCall.IsImportant) OnImportantCallFinished?.Invoke(PhoneCall, new EventArgs());
                    AnimationManager.Play(Animations["idle"]);
                    RingingSoundEffect.Stop();
                    Timer = 0;
                }
            }

            if (IsCallOver && IsTalking)
            {
                Timer += gameTime.ElapsedGameTime.TotalSeconds;

                if (Timer >= 5)
                {
                    EndCall();
                    Timer = 0;
                }
            }

            if (IsTalking)
            {
                PlayPhoneCall();
            }

            base.Update(gameTime);
        }

        public void Ring(PhoneCall phoneCall)
        {
            if (phoneCall is null) return;

            AnimationManager.Play(Animations["ringing"]);
            RingingSoundEffect.Play();
            SecondsBeforeMissedCall = new Random().Next(3, 8);
            Timer = 0;
            IsRinging = true;
            IsInUse = true;
            PhoneCall = phoneCall;
        }

        protected override void Click()
        {
            if (!IsRinging && !IsTalking) return;

            if (IsRinging)
            {
                IsRinging = false;
                IsTalking = true;
                AnimationManager.Play(Animations["talking"]);
                RingingSoundEffect.Stop();
                AudioManager.PlayEffect(ContentManager.PhonePickUpSoundEffect);
                CurrentVoiceLine = -1;
                IsCallOver = false;
                TextBox.Visible = true;
            }
            else if (IsTalking)
            {
                EndCall();
            }

            base.Click();
        }

        private void EndCall()
        {
            IsTalking = false;
            TextBox.Visible = false;
            TextBox.Text = "";
            AnimationManager.Play(Animations["idle"]);
            AudioManager.PlayEffect(ContentManager.PhoneHangUpSoundEffect);

            CallOverSoundEffect.Stop();
            if (CurrentVoiceLine != PhoneCall.VoiceLines.Count) PhoneCall.VoiceLines[CurrentVoiceLine].Voice.Stop();
            if (PhoneCall.IsImportant)
            {
                OnImportantCallFinished?.Invoke(PhoneCall, new EventArgs());
            }
            PhoneCall = null;

            if (!IsCallOver)
            {
                PlayerStats.WronglyEndedCalls++;
            }
            IsInUse = false;
        }

        private void PlayPhoneCall()
        {
            if (CurrentVoiceLine == PhoneCall.VoiceLines.Count)
            {
                if (CallOverSoundEffect.State != SoundState.Playing)
                {
                    CallOverSoundEffect.Play();
                    AnimationManager.Play(Animations["callOver"]);
                }
                IsCallOver = true;
                TextBox.Visible = false;
                return;
            }

            if (CurrentVoiceLine > -1 && PhoneCall.VoiceLines[CurrentVoiceLine].Voice.State == SoundState.Playing) return;
            CurrentVoiceLine++;

            if (CurrentVoiceLine == PhoneCall.VoiceLines.Count) return;

            PhoneCall.VoiceLines[CurrentVoiceLine].Voice.Volume = AudioManager.GlobalVolume;
            PhoneCall.VoiceLines[CurrentVoiceLine].Voice.Play();
            TextBox.Text = PhoneCall.VoiceLines[CurrentVoiceLine].Text;
            OnVoiceLineStart?.Invoke(PhoneCall.VoiceLines[CurrentVoiceLine], new EventArgs());
        }

    }
}
