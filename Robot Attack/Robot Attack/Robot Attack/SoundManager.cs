using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace Robot_Attack
{
    public static class SoundManager
    {
        #region Declarations
        private static List<SoundEffect> explosions =
            new List<SoundEffect>();

        private static int explosionCount = 4;

        private static SoundEffect normalShot;
        private static SoundEffect tripleShot;
        private static SoundEffect rocketShot;
        private static SoundEffect largeExplosion;

        private static Song BGM;

        private static Random rand = new Random();
        #endregion

        #region Initialization
        public static void Initialize(ContentManager content)
        {
            try
            {
                normalShot = content.Load<SoundEffect>(@"Sounds\NormalShot");
                tripleShot = content.Load<SoundEffect>(@"Sounds\TripleShot");
                rocketShot = content.Load<SoundEffect>(@"Sounds\RocketShot");
                largeExplosion = content.Load<SoundEffect>(@"Sounds\LargeExplosion");

                for (int x = 1; x <= explosionCount; x++)
                {
                    explosions.Add(
                        content.Load<SoundEffect>(@"Sounds\Explosion" +
                        x.ToString()));
                }

                BGM = content.Load<Song>(@"Sounds\BanditSlaughter");
                MediaPlayer.IsRepeating = true;
            }
            catch
            {
                Debug.Write("SoundManager Initialization Failed");
            }
        }
        #endregion

        #region Play Sounds
        public static void PlayBGM()
        {
            try
            {
                MediaPlayer.Play(BGM);
            }
            catch
            {
                Debug.Write("PlayBGM Failed");
            }
        }


        public static void PlayExplosion()
        {
            try
            {
                explosions[rand.Next(0, explosionCount)].Play(0.2f, 0.0f, 0.0f);
            }
            catch
            {
                Debug.Write("PlayExplosion Failed");
            }
        }

        public static void PlayLargeExplosion()
        {
            try
            {
                largeExplosion.Play(0.05f, 0.0f, 0.0f);
            }
            catch
            {
                Debug.Write("PlayLargeExplosion Failed");
            }
        }

        public static void PlayNormalShot()
        {
            try
            {
                normalShot.Play(0.2f, 0.0f, 0.0f);
            }
            catch
            {
                Debug.Write("PlayNormalShot Failed");
            }
        }

        public static void PlayTripleShot()
        {
            try
            {
                tripleShot.Play(0.2f, 0.0f, 0.0f);
            }
            catch
            {
                Debug.Write("PlayTripleShot Failed");
            }
        }

        public static void PlayRocketShot()
        {
            try
            {
                rocketShot.Play(0.2f, 0.0f, 0.0f);
            }
            catch
            {
                Debug.Write("PlayRocketShot Failed");
            }
        }
        #endregion
    }
}
