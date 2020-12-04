using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceInvaders
{
    class LineSchematic
    {

        int lives;
        int count;
        Enemy scheme;

        public LineSchematic(Enemy enemy, int count, int lives)
        {
            this.lives = lives;
            this.scheme = enemy;
            this.count = count;
        }

        public void generateScheme(Game instance, int sizeLine, Vecteur2D start)
        {
            int offset = (sizeLine - count * scheme.GetSprite().Draw().Width) / (count +1);
            start += new Vecteur2D(offset, 0);
            for (int i = 0; i < count; i++)
            {
                instance.AddNewGameObject(new Enemy(scheme, start, lives));
                start += new Vecteur2D(offset + scheme.GetSprite().Draw().Width, 0);
            }
        }
    }
}
