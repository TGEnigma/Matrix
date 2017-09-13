using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Matrix
{
    internal class Program
    {
        private static Random Random = new Random();

        public static float FPS { get; set; } = 60;

        public static int ParticleEmitFrequency { get; set; } = 1;

        public static int ParticleEmitCount { get; set; } = 1;

        public static int ParticleMinTrailLength { get; set; } = 2;

        public static int ParticleMaxTrailLength { get; set; } = 10;

        private static void Main(string[] args)
        {
            // update params
            int updateFrequency = ( int )( ( 1 / FPS ) * 1000 );
            var stopwatch = new Stopwatch();

            // render params
            Console.BufferHeight = 30;

            // ye good ol particles
            var particles = new List<MatrixParticle>();
            //particles.Add( new MatrixParticle( Random.Next( 0, Console.BufferWidth ) ), 5 );       

            // 'game' loop
            int frameCount = 0;
            while (true)
            {
                stopwatch.Reset();

                // Non blocking input
                if ( Console.KeyAvailable )
                {
                    var key = Console.ReadKey();
                    if ( key.Key == ConsoleKey.UpArrow )
                    {
                        ++ParticleEmitCount;
                    }
                    else if ( key.Key == ConsoleKey.DownArrow )
                    {
                        if ( ParticleEmitCount > 0 )
                            --ParticleEmitCount;
                    }
                }

                // make some particles, maybe
                if ( (frameCount % ParticleEmitFrequency) == 0 )
                {
                    for ( int i = 0; i < ParticleEmitCount; i++ )
                        particles.Add( CreateRandomParticle() );
                }                   

                // render & update particles
                int removedParticleCount = 0;
                for ( int i = ( 0 - removedParticleCount ); i < ( particles.Count - removedParticleCount ); i++ )
                {
                    var particle = particles[i];

                    // Remove particles on the bottom of the screen
                    if ( particle.Y == 0 )
                    {
                        particles.Remove( particle );
                        ++removedParticleCount;
                        continue;
                    }

                    particle.Update();
                    particle.Draw();
                }
                
                // try to maintain target frame rate
                // using a variable time step would be better but that's a lot of effort
                long timeSpent = stopwatch.ElapsedMilliseconds;
                int sleepTime = ( int )( updateFrequency - timeSpent );
                if ( sleepTime > 0 )
                {
                    Thread.Sleep( sleepTime );
                }

                ++frameCount;
            }
        }

        private static MatrixParticle CreateRandomParticle()
        {
            return new MatrixParticle( Random.Next( 0, Console.BufferWidth ), Random.Next( ParticleMaxTrailLength, ParticleMaxTrailLength + 1 ) );
        }
    }
}
