﻿using System.Windows;
using Hacking;

namespace Inkognitor
{
    public partial class App : Application
    {
        private Personality personality = new Personality();
        private MainWindow window;
        private HackingMode hackingMode = new HackingMode();

        protected override void OnStartup(StartupEventArgs e)
        {
            window = new MainWindow();
            window.Show();

            window.TextEntered += HandleUserInput;
        }

        private void HandleUserInput(object sender, MainWindow.TextEnteredEventArgs args)
        {
            if (args.Text == "hack")
            {
                hackingMode.start();
            }
            else
            {
                personality.Respond(args.Text);
            }
        }
    }
}