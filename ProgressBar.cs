﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace console_progress_with_task
{
        class ProgressBar
    {
        private static readonly object ConsoleWriterLock = new object();
        private int rowid = -1;
        private string title;

        private int titleLength;
        private int progressLength;
        private int progressMax;
        private int progressCurrent = 0;

        private const string animation = @"|/-\";
        private int animationIndex = 0;

        public ProgressBar(string title, int progressMax, int progressLength = 50, int titleLength = 35)
        {
            lock (ConsoleWriterLock)
            {
                this.rowid = Console.CursorTop;
                Console.SetCursorPosition(Console.CursorLeft, this.rowid + 1);
            }

            this.title = title;
            this.progressLength = progressLength;
            this.titleLength = titleLength;
            this.progressMax = progressMax;
        }

        public void step()
        {
            this.setProgress(++this.progressCurrent);
        }

        public void setTitle(string title)
        {
            lock (ConsoleWriterLock)
            {
                int left = Console.CursorLeft;
                int top = Console.CursorTop;
                this.title = title;
                Console.SetCursorPosition(0, this.rowid);
                Console.Write(title.Length >= titleLength ? title.Substring(0, titleLength) : title + new string(' ', titleLength - title.Length));
                Console.SetCursorPosition(left, top);
            }
        }

        public void setProgress(int progress)
        {
            if (!Console.IsOutputRedirected && progress <= progressMax)
            {
                lock (ConsoleWriterLock)
                {
                    try
                    {
                        int left = Console.CursorLeft;
                        int top = Console.CursorTop;
                        int percent = (int)progress * 100 / progressMax;
                        int charCount = (int)progressLength * percent / 100;

                        Console.SetCursorPosition(0, this.rowid);
                        Console.Write("{0} [{1,3}%] [{2}] [{3}] {4}/{5}",
                            title.Length >= titleLength ? title.Substring(0, titleLength) : title + new string(' ', titleLength - title.Length),
                            percent,
                            new string('#', charCount) + new string('-', progressLength - charCount),
                            animation[animationIndex++ % animation.Length],
                            progress,
                            progressMax);
                        Console.SetCursorPosition(left, top);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
        }
    }
}
