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
    public class Level {
        public int id;
        public Mushroom[,] mushrooms;
        LinkedList<Centipede> centipedes;
        public Color backgroundColor;

        public Level(int previousId = 0) {
            initialize(previousId);
        }

        public void initialize(int previousId = 0) {
            id = previousId + 1;
            mushrooms = new Mushroom[30, 30];
           


            backgroundColor = Globals.backgroundColors[id % Globals.backgroundColors.Length];
            mushroomInit();
        }

        public void mushroomInit() {
            for (int x = 0; x < mushrooms.GetLength(0); x++) {
                for (int y = 0; y < mushrooms.GetLength(0); y++) {
                    mushrooms[x, y] = new Mushroom(new Rectangle(x * 20, y * 20 + 40, 20, 20),backgroundColor);
                }
            }

            while (checkArray() < id * 10) {
                bool check = false;
                int x = Globals.rng.Next(30);
                int y = Globals.rng.Next(30);
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

        public int checkArray() {
            int total = 0;
            for (int x = 0; x < mushrooms.GetLength(0); x++) {
                for (int y = 0; y < mushrooms.GetLength(0); y++) {
                    if (mushrooms[x, y].visible == true)
                        total += 1;
                }

            }
            return total;
        }
    }
}
