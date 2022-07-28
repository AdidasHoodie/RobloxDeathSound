using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;

namespace ChangeRobloxUfSound
{
    class Program
    {
        public static void DownloadOldDeathSound()
        {
            Console.WriteLine("Downloading Old Death Sound");
            string OldSoundUrl = "https://cdn.discordapp.com/attachments/739698239527321691/1001639867266109571/ouch.ogg";
            using (var client = new WebClient())
            {
                client.DownloadFile(OldSoundUrl, "old.ogg");
            }
        }

        public static void Update(string RobloxPath)
        {
            Console.WriteLine("Updating death sound");
            string old = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\old.ogg";
            if (File.Exists(old) && File.Exists(RobloxPath + "\\ouch.ogg"))
            {
                File.Delete(RobloxPath + "\\ouch.ogg");
                byte[] oldbt = File.ReadAllBytes(old);
                File.WriteAllBytes(RobloxPath + "\\ouch.ogg", oldbt);
                File.Delete(old);
            }
        }

        static void Main(string[] args)
        {
            Console.Title = "Roblox Making Fun Of Us";
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Created by Adidas Hoodie. Copying prohibited");
            Console.WriteLine("Fixing roblox death sound");
            Console.WriteLine();
            long oldunixtime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            Process[] robloxProcList = Process.GetProcessesByName("RobloxPlayerBeta");
            if (robloxProcList.Length == 0)
            {
                Console.WriteLine("Couldn't find roblox. Starting automatic progress");
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\roblox-player"))
                {
                    if (key != null)
                    {
                        Object o = key.GetValue("InstallLocation");
                        if (o != null)
                        {
                            string RobloxDirString = (string)o;
                            RobloxDirString += "content\\sounds";
                            Console.WriteLine("Roblox installation has been found");
                            DownloadOldDeathSound();
                            Update(RobloxDirString);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Roblox not installed. Exiting");
                        Thread.Sleep(3000);
                        return;
                    }
                }
            }
            else
            {
                Console.WriteLine("Roblox Found! Getting directory of current version");
                Process robloxProc = robloxProcList[0];
                string RobloxDirStringUnclear = robloxProc.MainModule.FileName;
                int index = RobloxDirStringUnclear.IndexOf("\\RobloxPlayerBeta.exe");
                string RobloxDirString = (index < 0)
                    ? RobloxDirStringUnclear
                    : RobloxDirStringUnclear.Remove(index, "\\RobloxPlayerBeta.exe".Length);
                RobloxDirString += "content\\sounds";
                Console.WriteLine("Roblox installation has been found");
                DownloadOldDeathSound();
                Update(RobloxDirString);
            }

            Console.WriteLine($"Finished in {DateTimeOffset.Now.ToUnixTimeMilliseconds() - oldunixtime}ms!");

            Thread.Sleep(7000);
        }
    }
}
