// <copyright file="Logger.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Assembler
{
    using System;
    using System.IO;
    using System.Threading.Tasks;

    public class Logger : IDisposable
    {
        public static readonly Logger Error = new Logger(
            Console.Out,
            verbosityLimit: Int32.MaxValue,
            dispose: false);

        public static readonly Logger FatalError = new Logger(
            Console.Out,
            verbosityLimit: Int32.MaxValue,
            dispose: false);

        public static readonly Logger Standard = new Logger(
            Console.Out,
            verbosityLimit: 2,
            dispose: false);

        public static readonly Logger Warning = new Logger(
            Console.Out,
            verbosityLimit: 3,
            dispose: false);

        public Logger(
            TextWriter output,
            int verbosityLimit = 0,
            bool dispose = false)
        {
            Output = output
                ?? throw new ArgumentNullException(nameof(output));

            VerbosityLimit = verbosityLimit;
        }

        public TextWriter Output
        {
            get;
            private set;
        }

        public int VerbosityLimit
        {
            get;
            set;
        }

        private bool DisposeTextWriter
        {
            get;
        }

        public static void Log(
            int messageVerbosity,
            string message = null)
        {
            Standard.WriteLine(messageVerbosity, message);
        }

        public static Task LogAsync(
            int messageVerbosity,
            string message = null)
        {
            return Standard.WriteLineAsync(messageVerbosity, message);
        }

        public static void LogError(
            int messageVerbosity,
            string message = null)
        {
            Error.WriteLine(messageVerbosity, message);
        }

        public static Task LogErrorAsync(
            int messageVerbosity,
            string message = null)
        {
            return Error.WriteLineAsync(messageVerbosity, message);
        }

        public static void LogFatalError(
            int messageVerbosity,
            string message = null)
        {
            FatalError.WriteLine(messageVerbosity, message);
        }

        public static Task LogFatalErrorAsync(
            int messageVerbosity,
            string message = null)
        {
            return FatalError.WriteLineAsync(messageVerbosity, message);
        }

        public static void LogWarning(
            int messageVerbosity,
            string message = null)
        {
            Standard.WriteLine(messageVerbosity, message);
        }

        public static Task LogWarningAsync(
            int messageVerbosity,
            string message = null)
        {
            return Warning.WriteLineAsync(messageVerbosity, message);
        }

        public static void RedirectErrorOutput(TextWriter output)
        {
            Error.Output = output
                ?? throw new ArgumentNullException(nameof(output));
        }

        public static void RedirectFatalErrorOutput(TextWriter output)
        {
            FatalError.Output = output
                ?? throw new ArgumentNullException(nameof(output));
        }

        public static void RedirectStandardOutput(TextWriter output)
        {
            Standard.Output = output
                ?? throw new ArgumentNullException(nameof(output));
        }

        public static void RedirectWarningOutput(TextWriter output)
        {
            Warning.Output = output
                ?? throw new ArgumentNullException(nameof(output));
        }

        public void Dispose()
        {
            Dispose(true);
        }

        public void WriteLine(int messageVerbosity, string message = null)
        {
            if (messageVerbosity <= VerbosityLimit)
            {
                Output.WriteLine(message);
            }
        }

        public Task WriteLineAsync(int messageVerbosity, string message = null)
        {
            return messageVerbosity <= VerbosityLimit
                ? Output.WriteLineAsync(message)
                : Task.CompletedTask;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (DisposeTextWriter && disposing)
            {
                Output.Dispose();
            }
        }
    }
}
