﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Bliss.Component.Sprites.Office.Documents
{
    public class Letter : BaseDocument
    {
        public bool HasStamp { get; set; }
        public string ReturnAdress { get; set; }
        public string Department { get; set; }

        public Letter(Vector2 spawnPoint, Microsoft.Xna.Framework.Rectangle tableArea) : base(spawnPoint, tableArea)
        {
            Texture = ContentManager.LetterTexture;
            Size = SizeManager.GetSize(200, 100);
            Load(spawnPoint, tableArea);
        }

        public override List<Component> GetDetailViewComponents()
        {
            Size size = SizeManager.GetSize(600, 300);

            Sprite sprite = new Sprite()
            {
                Size = size,
                Position = new Vector2(
                        (int)(SizeManager.ScaleForWidth(SizeManager.JamGame.BaseWidth) - size.Width) / 2,
                        (int)(SizeManager.ScaleForHeight(SizeManager.JamGame.BaseHeight) - size.Height) / 2
                    ),
                Texture = ContentManager.LetterTexture
            };

            return new List<Component>() { sprite };
        }
    }
}
