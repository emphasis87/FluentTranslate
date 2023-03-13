using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentTranslate.Tests.Providers
{
    public class FileProviderTests
    {
        private string WorkingDirectory { get; set; }

        [SetUp]
        public void Setup()
        {
            WorkingDirectory = Path.Combine(
                TestContext.CurrentContext.WorkDirectory,
                TestContext.CurrentContext.Random.GetString(10));
        }

        [TearDown]
        public void TearDown()
        {
            if (Directory.Exists(WorkingDirectory))
                Directory.Delete(WorkingDirectory, true);
        }


        [Test]
        public async Task Can_watch_folder()
        {
            var d1 = Path.Combine(Path.Combine(WorkingDirectory, "d1"));

            Directory.CreateDirectory(Path.Combine(Path.Combine(WorkingDirectory, "d1", "sub")));
            Directory.CreateDirectory(Path.Combine(Path.Combine(WorkingDirectory, "d2", "sub")));

            var watcher = new FileSystemWatcher();
            try
            {
                watcher.Path = Path.Combine(Path.Combine(WorkingDirectory, "d1"));
                watcher.IncludeSubdirectories = true;
                watcher.Created += Watcher_Created;
                watcher.Error += Watcher_Error;
            }
            catch { }
            watcher.EnableRaisingEvents = true;
            
            File.WriteAllText(Path.Combine(Path.Combine(WorkingDirectory, "d1"), "a.ftl"), "hello = 1");
            File.WriteAllText(Path.Combine(Path.Combine(WorkingDirectory, "d1"), "sub/a.ftl"), "hello = 2");
            File.WriteAllText(Path.Combine(Path.Combine(WorkingDirectory, "d2"), "a.ftl"), "hello = 3");
            File.WriteAllText(Path.Combine(Path.Combine(WorkingDirectory, "d2"), "sub/a.ftl"), "hello = 4");

            Directory.Delete(d1, true);

            await Task.Delay(10000);

            Directory.CreateDirectory(Path.Combine(Path.Combine(WorkingDirectory, "d1", "sub")));

            //watcher.Path = Path.Combine(Path.Combine(WorkingDirectory, "d1"));

            File.WriteAllText(Path.Combine(Path.Combine(WorkingDirectory, "d1"), "a.ftl"), "hello = 1");
            File.WriteAllText(Path.Combine(Path.Combine(WorkingDirectory, "d1"), "sub/a.ftl"), "hello = 2");
            File.WriteAllText(Path.Combine(Path.Combine(WorkingDirectory, "d2"), "a.ftl"), "hello = 3");
            File.WriteAllText(Path.Combine(Path.Combine(WorkingDirectory, "d2"), "sub/a.ftl"), "hello = 4");

            // UseActivePolling, UsePollingFileWatcher
            var provider1 = new PhysicalFileProvider(Path.Combine(WorkingDirectory, "d1"));
            var provider2 = new PhysicalFileProvider(Path.Combine(WorkingDirectory, "d2"));
            var provider = new CompositeFileProvider(
                provider1,
                provider2);

            foreach(var c in provider1.GetDirectoryContents(""))
            {
                Console.WriteLine(c);
            }

            foreach (var c in provider2.GetDirectoryContents(""))
            {
                Console.WriteLine(c);
            }

            foreach (var c in provider.GetDirectoryContents(""))
            {
                Console.WriteLine(c);
            }
        }

        private async void Watcher_Error(object sender, ErrorEventArgs e)
        {
            Console.WriteLine($"Error: {e.GetException().Message}");
            //if (!Directory.Exists(Path.Combine(Path.Combine(WorkingDirectory, "d1"))))
            //{
                Directory.CreateDirectory(Path.Combine(Path.Combine(WorkingDirectory, "d1", "sub")));
            await Task.Delay(1000);
                if (sender is FileSystemWatcher watcher)
                {
                    watcher.EnableRaisingEvents = false;
                    watcher.EnableRaisingEvents = true;
                }
            //}
        }

        private void Watcher_Created(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine(e.FullPath);
        }
    }
}
