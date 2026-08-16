using System;
using Microsoft.Xna.Platform;
using Microsoft.Xna.Platform.Graphics;
using Microsoft.Xna.Platform.Input;

namespace Core
{
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            GameFactory.RegisterGameFactory(new ConcreteGameFactory());
            InputFactory.RegisterInputFactory(new ConcreteInputFactory());
            GraphicsFactory.RegisterGraphicsFactory(new ConcreteGraphicsFactory());
            TitleContainerFactory.RegisterTitleContainerFactory(new ConcreteTitleContainerFactory());
            
            using (var game = new CoreGame())
                game.Run();
        }
    }
}
