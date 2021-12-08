using System;
using System.IO;
using System.Timers;

namespace DocumentsReceiver
{
    public class DocumentsReceiver
    {
        private event ElapsedEventHandler TimeOut;
        private string _targetDirectory;
        Timer timer;
        FileSystemWatcher watcher;

        public void Start(string targetDirectory, TimeSpan waitingInterval)
        {
            _targetDirectory = targetDirectory;
            //Проверяем наличие директории и в случае отсутствия создаем ее
            if (!Directory.Exists(_targetDirectory))
            {
                Directory.CreateDirectory(_targetDirectory);
            }

            //Запускаем таймер
            timer = new Timer(waitingInterval.TotalMilliseconds);
            timer.Elapsed += TimeOut;
            timer.Enabled = true;

            watcher = new FileSystemWatcher(_targetDirectory)
            {
                EnableRaisingEvents = true
            };
            watcher.Changed += DocumentsReady; 
        }


        private void DocumentsReady(object sender, FileSystemEventArgs e)
        {
            if (File.Exists(_targetDirectory + "\\Паспорт.jpg")
                && File.Exists(_targetDirectory + "\\Заявление.txt")
                && File.Exists(_targetDirectory + "\\Фото.jpg"))
            {
                timer.Elapsed -= TimeOut;
                watcher.Changed -= DocumentsReady;
                throw new Exception("Документы готовы.");
            }
        }
    }
}
