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
    class Level {
        public int id;
        public Mushroom[,] mushrooms;
        LinkedList<Centipede> centipedes;
        Random rng;
        public Color backgroundColor;

        public Level() {
            initialize();
        }

        public Level(int previousId) {
            initialize();

            // update id based on previous level
            id = previousId + 1;
        }

        void initialize() {
            id = 0;
            mushrooms = new Mushroom[30, 30];
            rng = new Random();

            for (int x = 0; x < mushrooms.GetLength(0); x++) {
                for (int y = 0; y < mushrooms.GetLength(0); y++) {
                    mushrooms[x, y] = new Mushroom(new Rectangle(x * 20, y * 20 + 40, 20, 20));
                }
            }

            backgroundColor = Globals.backgroundColors[id % Globals.backgroundColors.Length];
        }

        void mushroomInit() {
            for (int a = 0; a < mushrooms.GetLength(0); a++) {
                for (int b = 0; b < mushrooms.GetLength(0); b++) {
                    mushrooms[a, b].visible = false;
                }
            }
            while (checkArray(mushrooms) < id * 10) {
                bool check = false;
                int x = rng.Next(30);
                int y = rng.Next(30);
                if (x - 1 >= 0) {
                    if (y - 1 >= 0)
                        if (mushrooms[x - 1, y - 1].visible == true)
                            check = true;
                    if (y + 1 < mushrooms.GetLength(0))
                        if (mushrooms[x - 1, y + 1].visible == true)
                            check = true;
                }

                if (x + 1 < mushrooms.GetLength(0)) {
                    if (y - 1 >= 0)
                        if (mushrooms[x + 1, y - 1].visible == true)
                            check = true;
                    if (y + 1 < mushrooms.GetLength(0))
                        if (mushrooms[x + 1, y + 1].visible == true)
                            check = true;
                }
                if (check == false)
                    mushrooms[x, y].generate();
            }
        }
    }
}
