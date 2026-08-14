using System;
using System.IO;
using Engine;
using Engine.Debugging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Core.Scripts
{
    public class ChipScript : Script
    {
        private Chip8 chip8;
        private AudioGenerator audio;
        
        private int cyclesPerFrame = 10;
        
        private Texture2D texture;

        public override void Initialize()
        {
            base.Initialize();
            
            chip8 = new Chip8();
            audio = new AudioGenerator(chip8.frequency, 0.2f);
            texture = new Texture2D(Runtime.GraphicsDevice, 64, 32, false, SurfaceFormat.Color);
        }

        public void Setup(string romToLoad = "games/Breakout (Brix hack) [David Winter, 1997].ch8")
        {
            base.Start();
            
            string path = "Content/roms/" + romToLoad;
            
            byte[] rom;
            
            using (var stream = TitleContainer.OpenStream(path))
            using (var ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                rom = ms.ToArray();
            }

            chip8.LoadProgram(rom);
        }
        
        public void Setup(byte[] rom)
        {
            base.Start();
            chip8.LoadProgram(rom);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            
            UpdateKeyboard();
            
            try
            {
                if (!chip8.WaitingForKeypress)
                {
                    for (int i = 0; i < cyclesPerFrame; i++)
                    {
                        chip8.Step();
                    }

                }

            }
            catch (Exception e)
            {
                //Console.WriteLine($"{e}");
                //throw;
            }
            
            if(chip8.SoundTimer > 0) audio.Update();
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            
            texture.SetData(chip8.Render());
            Runtime.SpriteBatch.Draw(texture, new Rectangle(0, 0, 64, 32), Color.White);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            audio.Destroy();
        }

        private void UpdateKeyboard()
        {
            var keyboard = Runtime.Input.Keyboard;

            if (keyboard.IsKeyDown(Keys.D1)) KeyPress(1);
            if (keyboard.IsKeyDown(Keys.D2)) KeyPress(2);
            if (keyboard.IsKeyDown(Keys.D3)) KeyPress(3);
            if (keyboard.IsKeyDown(Keys.D4)) KeyPress(0);

            if (keyboard.IsKeyDown(Keys.Q)) KeyPress(4);
            if (keyboard.IsKeyDown(Keys.W)) KeyPress(5);
            if (keyboard.IsKeyDown(Keys.E)) KeyPress(6);
            if (keyboard.IsKeyDown(Keys.R)) KeyPress(7);

            if (keyboard.IsKeyDown(Keys.A)) KeyPress(8);
            if (keyboard.IsKeyDown(Keys.S)) KeyPress(9);
            if (keyboard.IsKeyDown(Keys.D)) KeyPress(10);
            if (keyboard.IsKeyDown(Keys.F)) KeyPress(11);

            if (keyboard.IsKeyDown(Keys.Z)) KeyPress(12);
            if (keyboard.IsKeyDown(Keys.X)) KeyPress(13);
            if (keyboard.IsKeyDown(Keys.C)) KeyPress(14);
            if (keyboard.IsKeyDown(Keys.V)) KeyPress(15);

            ushort key = 0;
            if (keyboard.IsKeyUp(Keys.D1)) key |= 1 << 1;
            if (keyboard.IsKeyUp(Keys.D2)) key |= 1 << 2;
            if (keyboard.IsKeyUp(Keys.D3)) key |= 1 << 3;
            if (keyboard.IsKeyUp(Keys.D4)) key |= 1 << 0;

            if (keyboard.IsKeyUp(Keys.Q)) key |= 1 << 4;
            if (keyboard.IsKeyUp(Keys.W)) key |= 1 << 5;
            if (keyboard.IsKeyUp(Keys.E)) key |= 1 << 6;
            if (keyboard.IsKeyUp(Keys.R)) key |= 1 << 7;

            if (keyboard.IsKeyUp(Keys.A)) key |= 1 << 8;
            if (keyboard.IsKeyUp(Keys.S)) key |= 1 << 9;
            if (keyboard.IsKeyUp(Keys.D)) key |= 1 << 10;
            if (keyboard.IsKeyUp(Keys.F)) key |= 1 << 11;

            if (keyboard.IsKeyUp(Keys.Z)) key |= 1 << 12;
            if (keyboard.IsKeyUp(Keys.X)) key |= 1 << 13;
            if (keyboard.IsKeyUp(Keys.C)) key |= 1 << 14;
            if (keyboard.IsKeyUp(Keys.V)) key |= 1 << 15;

            if (key != 0) chip8.Keyboard &= (ushort)~key;
        }

        private void KeyPress(byte key)
        {
            chip8.Keyboard |= (ushort)(1 << key);
            if (chip8.WaitingForKeypress) chip8.KeyPressed(key);
        }
    }
}