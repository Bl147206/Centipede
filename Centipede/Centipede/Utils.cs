using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Centipede {
    public static class Globals {
        // Ensure that whenever we add new background colors that in the future we also add tints to
        // the sprites for these colors.
        //
        // This list is executed in order from 0 -> last element. Once last element has been displayed the
        // list is restarted starting from 0.
        public static readonly Color[] backgroundColors = { Color.Black };
    }
}
