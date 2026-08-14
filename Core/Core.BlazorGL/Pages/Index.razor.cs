using Microsoft.JSInterop;
using Microsoft.Xna.Framework;
using System;
using System.Threading.Tasks;
using Core.Scenes;

namespace Core.Pages
{
    public partial class Index
    {
        Game _game;
        bool started = false;

        async Task StartGame()
        {
            started = true;
            Engine.Runtime.Paused = false;
        }
        
        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);
        
            if (firstRender)
            {
                Engine.Runtime.Paused = true;
                JsRuntime.InvokeAsync<object>("initRenderJS", DotNetObjectReference.Create(this));
            }
        }

        [JSInvokable]
        public void TickDotNet()
        {
            // init game
            if (_game == null)
            {
                _game = new CoreGame();
                _game.Run();
            }

            // run gameloop
            _game.Tick();
        }
    }
}
