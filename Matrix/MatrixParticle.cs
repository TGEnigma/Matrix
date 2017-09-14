using System;
using System.Diagnostics;

namespace Matrix
{
    public class MatrixParticle
    {
        public static Random Random = new Random();

        private int mTrailLength;

        public bool Visible { get; set; } = true;

        public int X { get; private set; }

        public int Y { get; private set; }

        public MatrixParticle( int startingX, int trailLength )
        {
            // Starting x value will be variable but we'll always start the top of the window
            X = startingX;
            Y = Console.BufferHeight - 1;

            mTrailLength = trailLength;
        }

        public void Update()
        {
            // Down we go
            Y--;

            if ( (Y + mTrailLength) < 0 )
            {
                Visible = false;
            }

            //Debug.WriteLine( $"[{X}, {Y}]" );
        }

        public void Draw()
        {
            // Hack wayy to clear the last character
            int lastCharY = Y + mTrailLength + 1;

            if (lastCharY < Console.BufferHeight)
            {
                WriteCharacter(X, lastCharY, ' ');
            }

            // Draw the trailing particles
            for (int i = 0; i < mTrailLength; i++)
            {
                int trailY = Y + (i + 1);

                if (trailY < Console.BufferHeight)
                {
                    WriteRandomASCIICharacter(X, trailY, Random.Next(0, 2) == 0 ? ConsoleColor.Green : ConsoleColor.DarkGreen);
                }
                else
                {
                    // If we're already at the top we can stop trying
                    break;
                }
            }

            WriteRandomASCIICharacter(X, Y, ConsoleColor.White);
        }

        public void Remove()
        {
            // Erase the particle
            for (int i = 0; i < mTrailLength + 1; i++)
            {
                int charY = i + 1;

                if (charY < Console.BufferHeight)
                    WriteCharacter(X, Y + (i + 1), ' ');
            }
        }

        private static void WriteCharacter( int x, int y, char character )
        {
            if (y < Console.BufferHeight && y > 0)
            {
                Console.SetCursorPosition(x, (Console.BufferHeight - 1) - y);
                Console.Write(character);
            }
        }

        private static void WriteRandomASCIICharacter( int x, int y, ConsoleColor color )
        {
            if ( y < Console.BufferHeight && y > 0 )
            {
                Console.SetCursorPosition(x, (Console.BufferHeight - 1) - y);
                Console.ForegroundColor = color;
                Console.Write((char)Random.Next(32, 127));
                //Console.Write( Random.Next( 0, 2 ) );
            }
        }
    }
}