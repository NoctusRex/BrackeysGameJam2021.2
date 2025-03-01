﻿using Bliss.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Bliss.Component.Sprites.Ui
{
    enum TextBoxParts
    {
        TopLeft,
        TopCenter,
        TopRight,
        MiddleLeft,
        MiddleCenter,
        MiddleRight,
        BottomLeft,
        BottomCenter,
        BottomRight
    }

    public class TextBox : Sprite
    {
        private int TextBoxPartSize => 16;
        private List<List<TextBoxParts>> TextBoxParts { get; set; }
        private string text;
        public string Text
        {
            get => text; 
            set
            {
                text = value;
                SetText();
            }
        }

        public TextBox()
        {
            Texture = ContentManager.TextboxTexture;
        }

        private void SetText()
        {
            Vector2 stringSize = ContentManager.PatrickHandFont.MeasureString(Text);
            int xCount = Math.Max(3, (int)stringSize.X / TextBoxPartSize + 1);
            int yCount = Math.Max(3, (int)stringSize.Y / TextBoxPartSize + 1);

            TextBoxParts = new List<List<TextBoxParts>>();

            for (int i = 0; i < yCount; i++)
            {
                TextBoxParts.Add(new List<TextBoxParts>());

                for (int j = 0; j < xCount; j++)
                {
                    if (i == 0)
                    {
                        if (j == 0)
                        {
                            TextBoxParts[i].Add(Ui.TextBoxParts.TopLeft);
                        }
                        else if (j == xCount - 1)
                        {
                            TextBoxParts[i].Add(Ui.TextBoxParts.TopRight);
                        }
                        else
                        {
                            TextBoxParts[i].Add(Ui.TextBoxParts.TopCenter);
                        }

                        continue;
                    }

                    if (i == yCount - 1)
                    {
                        if (j == 0)
                        {
                            TextBoxParts[i].Add(Ui.TextBoxParts.BottomLeft);
                        }
                        else if (j == xCount - 1)
                        {
                            TextBoxParts[i].Add(Ui.TextBoxParts.BottomRight);
                        }
                        else
                        {
                            TextBoxParts[i].Add(Ui.TextBoxParts.BottomCenter);
                        }

                        continue;
                    }

                    if (j == 0)
                    {
                        TextBoxParts[i].Add(Ui.TextBoxParts.MiddleLeft);
                    }
                    else if (j == xCount - 1)
                    {
                        TextBoxParts[i].Add(Ui.TextBoxParts.MiddleRight);
                    }
                    else
                    {
                        TextBoxParts[i].Add(Ui.TextBoxParts.MiddleCenter);
                    }
                }
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (TextBoxParts is null) return;

            for (int y = 0; y < TextBoxParts.Count; y++)
            {
                for (int x = 0; x < TextBoxParts[y].Count; x++)
                {
                    spriteBatch.Draw(Texture, new Vector2(Position.X + x * TextBoxPartSize, Position.Y + y * TextBoxPartSize), new Rectangle((int)TextBoxParts[y][x] * TextBoxPartSize,
                                               0,
                                               TextBoxPartSize,
                                               TextBoxPartSize), Color.White, 0, Vector2.Zero, 3, SpriteEffects.None, 0);
                }
            }

            spriteBatch.DrawString(ContentManager.PatrickHandFont, Text, new Vector2(Position.X + 15, Position.Y + 15), Color.Black);
        }

    }
}
