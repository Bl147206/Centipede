using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Centipede {
    public static class Globals {
        // Ensure that whenever we add new background colors that in the future we also add tints to
        // the sprites for these colors.
        //
        // This list is executed in order from 0 -> last element. Once last element has been displayed the
        // list is restarted starting from 0.
        public static readonly Color[] backgroundColors = { Color.Green, Color.Pink, Color.Red, Color.Yellow, Color.Aqua, Color.Orange };

        // Global random state
        public static Random rng = new Random();

        // This is the initial full texture
        public static Texture2D mushroom0;

        // This is the first hit texture
        public static Texture2D mushroom1;

        // This is the second hit texture
        public static Texture2D mushroom2;

        //This is the max level reachable
        public static int levelCap = 30;
    }
}
