﻿using System.Drawing;
using System.IO;
using SdlDotNet.Core;
using SdlDotNet.Graphics;
using SdlDotNet.Graphics.Sprites;

namespace Hacking
{
    class InformationArea : Sprite
    {
        private static readonly string BackgroundImageFile = "information_area.png";

        private Surface background;

        public InformationArea(Rectangle rectangle)
        {
            Surface = new Surface(rectangle);
            background = new Surface(Path.Combine(HackingMode.ResourceDirectory, BackgroundImageFile));
            background = background.CreateResizedSurface(Surface.Size);
        }

        override public void Update(TickEventArgs args)
        {
            Surface.Blit(background);
        }
    }
}
