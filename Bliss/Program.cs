﻿using System;
using Unity;
using Bliss.Manager;
using Bliss.States;
using Bliss.States.Game;
using Bliss.Factories;
using Bliss.States.GameOver;

namespace Bliss
{
    public static class Program
    {
        public static IUnityContainer UnityContainer = new UnityContainer();

        [STAThread]
        static void Main()
        {
            Register();

            using JamGame game = UnityContainer.Resolve<JamGame>();
            game.Run();
        }

        static void Register()
        {
            RegisterManager();
            RegisterStates();

            UnityContainer.RegisterSingleton<JamGame>();
        }

        static void RegisterManager()
        {
            UnityContainer.RegisterType<ContentManager>();
            UnityContainer.RegisterType<AudioManager>();
            UnityContainer.RegisterType<ParticleManager>();
            UnityContainer.RegisterSingleton<StateManager>();
            UnityContainer.RegisterType<AnimationManager>();
            UnityContainer.RegisterType<PhoneCallFactory>();
            UnityContainer.RegisterType<DocumentFactory>();
        }

        static void RegisterStates()
        {
            UnityContainer.RegisterType<DefaultState>(DefaultState.Name);
            UnityContainer.RegisterType<DefaultState>(GameState.Name);
            UnityContainer.RegisterType<GameOverState>(GameState.Name);
            UnityContainer.RegisterType<MenuState>(GameState.Name);
        }
    }
}
