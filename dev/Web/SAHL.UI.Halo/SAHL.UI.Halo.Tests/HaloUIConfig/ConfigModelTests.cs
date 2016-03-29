using NUnit.Framework;
using System.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using SAHL.UI.Halo.Tests.HaloUIConfigPredicates;
namespace SAHL.UI.Halo.Tests.HaloUIConfig
{
    [TestFixture]
    public class ConfigModelTests: HaloUIConfigTests
    {
        [Test]
        public void MatchModelsToGeneratedViews()
        {
            //---------------Set up test pack-------------------
            var configurations = new Dictionary<string, string>();
            var expected = String.Empty;
            var actual = String.Empty;
            var haloTileModels = base.haloTileModels.GetRegisteredTypes();
            var matchesFound = new List<string>();

            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            foreach (var haloTileModel in haloTileModels)
            {
                if (haloTileModel.BaseType == typeof(Object))
                {
                    expected = haloTileModel.Name.ToLower();
                    this.TraverseDir((file) =>
                    {
                        actual = file.Name.ToLower().Replace("html", String.Empty).Replace("tpl", String.Empty).Replace(".", String.Empty);
                        if (actual == expected)
                        {
                            matchesFound.Add(actual);
                        }
                    });

                    if (!matchesFound.Contains(expected))
                    {
                        actual = "could not find...";
                        configurations.Add(expected, actual);
                    }
                }
            }

            //---------------Test Result -----------------------
            AssertConfiguration(configurations);
        }
        
        private void TraverseDir(Action<FileInfo> currentFile)
        {
            var parentDir = Directory.GetParent(Directory.GetCurrentDirectory());
            var dirCount = parentDir.FullName.Split(new string[] { @"\" }, StringSplitOptions.RemoveEmptyEntries).Length;
            var count = 0;
            while (count < dirCount)
            {
                if (parentDir == null)
                {
                    break;
                }
                else
                {
                    parentDir = Directory.GetParent(parentDir.FullName);
                    if (parentDir.GetDirectories("SAHL.UI.Halo.Views").Count() > 0)
                    {
                        parentDir = new DirectoryInfo(Path.Combine(parentDir.FullName, "SAHL.UI.Halo.Views")).GetDirectories("lib").First();
                        foreach (var tpl in parentDir.GetFiles("*.tpl.html", SearchOption.AllDirectories))
                        {
                            currentFile.Invoke(tpl);
                        }
                    }
                }
                count++;
            }
        }
    }
}
