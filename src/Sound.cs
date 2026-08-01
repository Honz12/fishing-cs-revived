namespace fishing_cs_revived.src
{
    using System.Diagnostics;
    using System.Runtime.InteropServices;

    public static class Sound
    {
        public static void PlaySoundRawPath(string path)
        {
            if (!Program.audioEnabled)
                return;

            Process? process = Process.Start(
                new ProcessStartInfo
                {
                    FileName = "ffplay",
                    Arguments = $"-nodisp -autoexit -loglevel quiet \"{path}\"",
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            );

            //process?.WaitForExit();
        }

        public static void PlayAudioFile(string name)
        {
            PlaySoundRawPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "src", "audio", name));
        }
    }
}
