using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace console_progress_with_task
{
    class Program
    {
        static void Main(string[] args)
        {
            Random r=new Random();
            string message = null;
            List<Task> tasks = new List<Task>();

            for (int i = 1; i <= 25; i++)
			{
                int tableRowCount = r.Next(490)+10;
                string tasktitle = String.Format("{0}. dosya indiliyor", i);
                string taskmsg = String.Format(">>> {0}. dosya ilk tamamlandı!...", i);
                tasks.Add(Task.Run(async () =>
                {
                    int delay = 0;
                    var p = new ProgressBar(tasktitle, tableRowCount);
                    for (int ii = 0; ii <= tableRowCount; ii++)
                    {
                        p.setProgress(ii);
                        delay = r.Next(200-ii);
                        await Task.Delay(delay);
                    }
                    if (String.IsNullOrEmpty(message))
                        message = taskmsg;
                }));
			}

            Task.WaitAll(tasks.ToArray(), (10 * 60 * 1000));
            Console.WriteLine(message);

            Console.Read();
        }
    }
}
