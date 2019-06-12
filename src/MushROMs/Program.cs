// <copyright file="Program.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.MushROMs
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Windows.Forms;
    using Maseya.MushROMs.Properties;

    public static class Program
    {
        internal static readonly Settings Settings = Settings.Default;

        public static void About()
        {
            About(null);
        }

        public static void About(IWin32Window owner)
        {
            using (var dialog = new AboutDialog())
            {
                dialog.ShowDialog(owner);
            }
        }

        internal static void InitializeSettings()
        {
            Settings.RecentFiles = new StringCollection();
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (Settings.FirstTime)
            {
                InitializeSettings();

                Settings.FirstTime = false;
                Settings.Save();
            }

            using (var form = new MainForm())
            {
                for (var i = Settings.RecentFiles.Count; --i >= 0;)
                {
                    form.RecentFiles.Add(Settings.RecentFiles[i]);
                }

                form.FormClosed += FormClosed;

                Application.Run(form);
            }
        }

        private static void FormClosed(object sender, FormClosedEventArgs e)
        {
            Settings.RecentFiles.Clear();
            Settings.RecentFiles.AddRange(
                new List<string>((sender as MainForm).RecentFiles).ToArray());

            Settings.Save();
        }
    }
}
