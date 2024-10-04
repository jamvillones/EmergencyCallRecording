using System;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

namespace Sound {
    public static class SoundInfo {
        [DllImport("winmm.dll")]
        private static extern uint mciSendString(
            string command,
            StringBuilder returnValue,
            int returnLength,
            IntPtr winHandle);

        public static int GetSoundLength(string fileName) {
            StringBuilder lengthBuf = new StringBuilder(32);

            mciSendString(string.Format("open \"{0}\" type waveaudio alias wave", fileName), null, 0, IntPtr.Zero);
            mciSendString("status wave length", lengthBuf, lengthBuf.Capacity, IntPtr.Zero);
            mciSendString("close wave", null, 0, IntPtr.Zero);


            _ = int.TryParse(lengthBuf.ToString(), out int length);

            return length;
        }
        //public static double GetMediaDuration(string MediaFilename) {
        //    double duration = 0.0;
        //    using (FileStream fs = File.OpenRead(MediaFilename)) {
        //        Mp3Frame frame = Mp3Frame.LoadFromStream(fs);
        //        if (frame != null) {
        //            _sampleFrequency = (uint)frame.SampleRate;
        //        }
        //        while (frame != null) {
        //            if (frame.ChannelMode == ChannelMode.Mono) {
        //                duration += (double)frame.SampleCount * 2.0 / (double)frame.SampleRate;
        //            }
        //            else {
        //                duration += (double)frame.SampleCount * 4.0 / (double)frame.SampleRate;
        //            }
        //            frame = Mp3Frame.LoadFromStream(fs);
        //        }
        //    }
        //    return duration;
        //}
    }
}