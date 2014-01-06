using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace dimigo_meal
{
    public class NarrationPlayer
    {
        private static string BASE_SOUND_PATH = Path.GetFullPath("song/");

        private static SoundPlayer _soundPlayer = new SoundPlayer();

        public void Play(string message)
        {
            Task.Factory.StartNew(() =>
            {
                _play(message, default(TimeSpan));
            });
        }

        public void Play(string message, TimeSpan timeSpan)
        {
            Task.Factory.StartNew(() =>
            {
                _play(message, timeSpan);
            });
        }

        public void PlayAsync(string message)
        {
            _play(message, default(TimeSpan));
        }

        public void PlayAsync(string message, TimeSpan timeSpan)
        {
            _play(message, timeSpan);
        }

        private void _play(string message, TimeSpan timeSpan)
        {
            if (timeSpan != default(TimeSpan))
            {
                if (numToSound(timeSpan.Hours))
                {
                    _soundPlayer.SoundLocation = string.Concat(BASE_SOUND_PATH, "시간.wav");
                    _soundPlayer.PlaySync();
                    System.Threading.Thread.Sleep(100);
                }

                if (numToSound(timeSpan.Minutes))
                {
                    _soundPlayer.SoundLocation = string.Concat(BASE_SOUND_PATH, "분.wav");
                    _soundPlayer.PlaySync();
                    System.Threading.Thread.Sleep(100);
                }
            }

            _soundPlayer.SoundLocation = string.Concat(BASE_SOUND_PATH, message, ".wav");
            _soundPlayer.Play();
        }

        private bool numToSound(int num)
        {
            _soundPlayer.Stop();

            bool result = false;
            int tmp = num / 10;
            if (tmp > 0)
            {
                result = true;
                switch (tmp)
                {
                    case 1:
                        _soundPlayer.SoundLocation = string.Concat(BASE_SOUND_PATH, "십.wav");
                        _soundPlayer.PlaySync();
                        break;
                    case 2:
                        _soundPlayer.SoundLocation = string.Concat(BASE_SOUND_PATH, "이십.wav");
                        _soundPlayer.PlaySync();
                        break;
                    case 3:
                        _soundPlayer.SoundLocation = string.Concat(BASE_SOUND_PATH, "삼십.wav");
                        _soundPlayer.PlaySync();
                        break;
                    case 4:
                        _soundPlayer.SoundLocation = string.Concat(BASE_SOUND_PATH, "사십.wav");
                        _soundPlayer.PlaySync();
                        break;
                    case 5:
                        _soundPlayer.SoundLocation = string.Concat(BASE_SOUND_PATH, "오십.wav");
                        _soundPlayer.PlaySync();
                        break;
                    case 6:
                        _soundPlayer.SoundLocation = string.Concat(BASE_SOUND_PATH, "육십.wav");
                        _soundPlayer.PlaySync();
                        break;
                    case 7:
                        _soundPlayer.SoundLocation = string.Concat(BASE_SOUND_PATH, "칠.wav");
                        _soundPlayer.PlaySync();
                        _soundPlayer.SoundLocation = string.Concat(BASE_SOUND_PATH, "십.wav");
                        _soundPlayer.PlaySync();
                        break;
                    case 8:
                        _soundPlayer.SoundLocation = string.Concat(BASE_SOUND_PATH, "팔.wav");
                        _soundPlayer.PlaySync();
                        _soundPlayer.SoundLocation = string.Concat(BASE_SOUND_PATH, "십.wav");
                        _soundPlayer.PlaySync();
                        break;
                    case 9:
                        _soundPlayer.SoundLocation = string.Concat(BASE_SOUND_PATH, "구.wav");
                        _soundPlayer.PlaySync();
                        _soundPlayer.SoundLocation = string.Concat(BASE_SOUND_PATH, "십.wav");
                        _soundPlayer.PlaySync();
                        break;
                }
            }

            tmp = num % 10;
            if (tmp > 0)
            {
                result = true;
                switch (num % 10)
                {
                    case 1:
                        _soundPlayer.SoundLocation = string.Concat(BASE_SOUND_PATH, "일.wav");
                        _soundPlayer.PlaySync();
                        break;
                    case 2:
                        _soundPlayer.SoundLocation = string.Concat(BASE_SOUND_PATH, "이.wav");
                        _soundPlayer.PlaySync();
                        break;
                    case 3:
                        _soundPlayer.SoundLocation = string.Concat(BASE_SOUND_PATH, "삼.wav");
                        _soundPlayer.PlaySync();
                        break;
                    case 4:
                        _soundPlayer.SoundLocation = string.Concat(BASE_SOUND_PATH, "사.wav");
                        _soundPlayer.PlaySync();
                        break;
                    case 5:
                        _soundPlayer.SoundLocation = string.Concat(BASE_SOUND_PATH, "오.wav");
                        _soundPlayer.PlaySync();
                        break;
                    case 6:
                        _soundPlayer.SoundLocation = string.Concat(BASE_SOUND_PATH, "육.wav");
                        _soundPlayer.PlaySync();
                        break;
                    case 7:
                        _soundPlayer.SoundLocation = string.Concat(BASE_SOUND_PATH, "칠.wav");
                        _soundPlayer.PlaySync();
                        break;
                    case 8:
                        _soundPlayer.SoundLocation = string.Concat(BASE_SOUND_PATH, "팔.wav");
                        _soundPlayer.PlaySync();
                        break;
                    case 9:
                        _soundPlayer.SoundLocation = string.Concat(BASE_SOUND_PATH, "구.wav");
                        _soundPlayer.PlaySync();
                        break;
                }
            }
            return result;
        }
    }
}