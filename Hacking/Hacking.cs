﻿using System;
using System.Drawing;
using SdlDotNet.Core;
using SdlDotNet.Graphics;
using SdlDotNet.Input;

namespace Hacking
{
    // TODO: Too much stuff in this class, split it up
    public class HackingMode
    {
        public static readonly string ResourceDirectory = "Resources";

        private static readonly Size WindowSize = new Size(800, 600);
        private static readonly string WindowName = "Inkognitor";
        private static readonly int CodeBlockColumns = 4;
        private static readonly int CodeBlockRows = 6;
        private static readonly int MaxErrorsPerCodeBlockRow = 3;
        private static readonly float ErrorCodeBlockProbability = 0.25f;
        private static readonly float InfoAreaHeight = 0.25f;
        private static readonly Rectangle CodeAreaRectangle = new Rectangle(
                new Point(0, (int)(WindowSize.Height * InfoAreaHeight)),
                new Size(WindowSize.Width, (int)(WindowSize.Height * (1.0f - InfoAreaHeight))));

        private Random random = new Random();

        private CodeBlockGrid codeBlocks;
        private Cursor cursor;

        public void start()
        {
            codeBlocks = new CodeBlockGrid(CodeBlockColumns, CodeBlockRows, CodeAreaRectangle.Size);
            cursor = new Cursor(codeBlocks.BlockPixelSize);
            InitializeCodeBlocks();

            Video.SetVideoMode(WindowSize.Width, WindowSize.Height);
            Video.WindowCaption = WindowName;

            Events.Tick += HandleTick;
            Events.KeyboardDown += HandleKeyboardDown;
            Events.Quit += HandleQuit;

            codeBlocks.Rotated += HandleCodeBlocksRotated;

            Events.Run();
        }

        private void HandleTick(object sender, TickEventArgs e)
        {
            codeBlocks.Update(e);
            cursor.Position = codeBlocks[cursor.GridX, cursor.GridY].Position;
            cursor.X += CodeAreaRectangle.X;
            cursor.Y += CodeAreaRectangle.Y;

            Video.Screen.Blit(codeBlocks, CodeAreaRectangle.Location);
            Video.Screen.Blit(cursor.Surface, cursor.Position, cursor.CalcClippingRectangle());

            Video.Screen.Update();
        }

        private void HandleKeyboardDown(object sender, KeyboardEventArgs e)
        {
            switch (e.Key)
            {
                case Key.LeftArrow:
                    cursor.GridX = cursor.GridX > 0 ? cursor.GridX - 1 : cursor.GridX;
                    break;
                case Key.RightArrow:
                    cursor.GridX = cursor.GridX < codeBlocks.Width - 1 ? cursor.GridX + 1 : cursor.GridX;
                    break;
                case Key.UpArrow:
                    cursor.GridY = cursor.GridY > 1 ? cursor.GridY - 1 : cursor.GridY;
                    break;
                case Key.DownArrow:
                    cursor.GridY = cursor.GridY < codeBlocks.Height - 2 ? cursor.GridY + 1 : cursor.GridY;
                    break;
            }
        }

        private void HandleQuit(object sender, QuitEventArgs e)
        {
            Events.QuitApplication();
        }

        private void HandleCodeBlocksRotated(object sender, EventArgs e)
        {
            if (cursor.GridY > 1)
            {
                cursor.GridY -= 1;
            }
        }

        private void InitializeCodeBlocks()
        {
            for (int i = 0; i < codeBlocks.Height; i++)
            {
                InitializeCodeBlockRow(i);
            }
        }

        private void InitializeCodeBlockRow(int row)
        {
            int errors = 0;

            for (int i = 0; i < codeBlocks.Width; i++)
            {
                CodeBlock block = codeBlocks[i, row];
            
                if (errors < MaxErrorsPerCodeBlockRow && random.NextDouble() < ErrorCodeBlockProbability)
                {
                    block.Personality = CodeBlock.PersonalityError;
                    errors += 1;
                }
                else
                {
                    block.Personality = random.Next(CodeBlock.Personalities);
                }
            }
        }
    }
}
