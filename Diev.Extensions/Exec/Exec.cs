#region License
/*
Copyright 2022-2024 Dmitrii Evdokimov
Open source software

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/
#endregion

using System.Diagnostics;
using System.Text;

namespace Diev.Extensions.Exec;

public static class Exec
{
    /// <summary>
    /// Init required
    /// </summary>
    private static bool _init = true;

    // Define static variables shared by class methods.
    //private static StreamWriter? _streamError = null;
    private static readonly StringBuilder _output = new();
    private static readonly StringBuilder _error = new();
    //private static bool _errorsWritten = false;

    //public static string ErrorFile { get; set; } = "crypto.log";
    public static bool ErrorRedirect { get; set; } = true;

    /// <summary>
    /// Запустить программу с параметрами и дождаться ее завершения.
    /// </summary>
    /// <param name="exe">Запускаемая программа.</param>
    /// <param name="cmdline">Параметры для запускаемой программы.</param>
    /// <exception cref="FileNotFoundException"></exception>
    /// <exception cref="Exception"></exception>
    public static void Start(string exe, string cmdline)
    {
        if (_init)
        {
            if (!File.Exists(exe))
            {
                throw new FileNotFoundException("File to exec not found.", exe);
            }

            _init = false;
        }

        ProcessStartInfo startInfo = new()
        {
            CreateNoWindow = false,
            WindowStyle = ProcessWindowStyle.Normal,
            UseShellExecute = true,
            FileName = exe,
            Arguments = cmdline
        };

        try
        {
            using Process? process = Process.Start(startInfo);

            if (process is null)
            {
                throw new Exception("Fail to get starting process.");
            }
            else
            {
                process.WaitForExit();
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Fail to start [\"{exe}\" {cmdline}]", ex);
        }
    }

    /// <summary>
    /// Запустить программу с параметрами и дождаться ее завершения.
    /// </summary>
    /// <param name="exe">Файл запускаемой программы.</param>
    /// <param name="cmdline">Параметры для запускаемой программы.</param>
    /// <param name="visible">Показывать ли окно запускаемой программы.</param>
    /// <returns>Код возврата из программы.</returns>
    /// <exception cref="FileNotFoundException"></exception>
    /// <exception cref="Exception"></exception>
    public static async Task<int> StartAsync(string exe, string cmdline, bool visible)
    {
        if (_init)
        {
            if (!File.Exists(exe))
            {
                throw new FileNotFoundException("File to exec not found.", exe);
            }

            _init = false;
        }

#if DEBUG
        string test = @$"""{exe}"" {cmdline}";
        Console.WriteLine(test);
#endif

        ProcessStartInfo startInfo = new()
        {
            CreateNoWindow = false,
            WindowStyle = visible ? ProcessWindowStyle.Normal : ProcessWindowStyle.Hidden,
            UseShellExecute = false,
            FileName = exe,
            Arguments = cmdline
        };

        try
        {
            using Process? process = Process.Start(startInfo);

            if (process is null)
            {
                throw new Exception("Fail to get starting process.");
            }
            else
            {
                await process.WaitForExitAsync();

                return process.ExitCode;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(@$"Fail to start [""{exe}"" {cmdline}]", ex);
        }
    }

    public static async Task<int> StartAsync(string exe, StringBuilder cmd, bool visible)
        => await StartAsync(exe, cmd.ToString(), visible);

    /// <summary>
    /// Запустить программу с параметрами и дождаться ее завершения.
    /// </summary>
    /// <param name="exe">Файл запускаемой программы.</param>
    /// <param name="cmdline">Параметры для запускаемой программы.</param>
    /// <param name="visible">Показывать ли окно запускаемой программы.</param>
    /// <returns>Текст вывода из программы.</returns>
    /// <exception cref="FileNotFoundException"></exception>
    /// <exception cref="Exception"></exception>
    public static async Task<(int ExitCode, string Output, string Error)> StartWithOutputAsync(string exe, string cmdline, bool visible)
    {
        if (_init)
        {
            if (!File.Exists(exe))
            {
                throw new FileNotFoundException("File to exec not found.", exe);
            }

            _init = false;
        }

#if DEBUG
        //string test = @$"""{exe}"" {cmdline}";
        //Console.WriteLine(test);
#endif

        // https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.process.beginerrorreadline?view=net-8.0

        int result = 1;
        _output.Clear();
        _error.Clear();

        try
        {
            // Initialize the process and its StartInfo properties.
            using Process process = new();
            process.StartInfo.FileName = exe;


            process.StartInfo.CreateNoWindow = false;
            process.StartInfo.WindowStyle = visible ? ProcessWindowStyle.Normal : ProcessWindowStyle.Hidden;

            // Build the net command argument list.
            process.StartInfo.Arguments = cmdline;

            // Set UseShellExecute to false for redirection.
            process.StartInfo.UseShellExecute = false;

            // Redirect the standard output of the command.
            // This stream is read asynchronously using an event handler.
            process.StartInfo.RedirectStandardOutput = true;
            process.OutputDataReceived += new DataReceivedEventHandler(OutputDataHandler);

            if (ErrorRedirect)
            {
                // Redirect the error output of the command.
                process.StartInfo.RedirectStandardError = true;
                process.ErrorDataReceived += new DataReceivedEventHandler(ErrorDataHandler);
            }
            else
            {
                // Do not redirect the error output.
                process.StartInfo.RedirectStandardError = false;
            }

            // Start the process.
            process.Start();

            // Start the asynchronous read of the standard output stream.
            process.BeginOutputReadLine();

            if (ErrorRedirect)
            {
                // Start the asynchronous read of the standard error stream.
                process.BeginErrorReadLine();
            }

            // Let the command run, collecting the output.
            await process.WaitForExitAsync();
            result = process.ExitCode;

            //if (_streamError != null)
            //{
            //    // Close the error file.
            //    _streamError.Close();
            //}
            //else
            //{
            //    // Set errorsWritten to false if the stream is not open.
            //    // Either there are no errors, or the error file could not be opened.
            //    _errorsWritten = false;
            //}

            //if (_output.Length > 0)
            //{
            //    // If the process wrote more than just white space,
            //    // write the output to the console.
            //    Console.WriteLine("\nOutput:\n{0}\n",
            //        _output);
            //}

            //if (_error.Length > 0)
            //{
            //    // If the process wrote more than just white space,
            //    // write the output to the console.
            //    Console.WriteLine("\nError:\n{0}\n",
            //        _error);

            //    //_output.AppendLine().AppendLine("Error:").Append(_error);
            //}

            //if (_errorsWritten)
            //{
            //    // Signal that the error file had something written to it.
            //    string[] errorOutput = await File.ReadAllLinesAsync(ErrorFile);

            //    if (errorOutput.Length > 0)
            //    {
            //        Console.WriteLine("\nThe following error output was appended to {0}.",
            //            ErrorFile);

            //        // File
            //        _output.AppendLine().AppendLine("Error:");

            //        foreach (string errLine in errorOutput)
            //        {
            //            Console.WriteLine("  " + errLine);

            //            // File
            //            _output.Append("  ").AppendLine(errLine);
            //        }
            //    }

            //    Console.WriteLine();
            //}

            process.Close();
        }
        catch (Exception ex)
        {
            throw new Exception(@$"Fail to start [""{exe}"" {cmdline}]", ex);
        }

        return (result, _output.ToString(), _error.ToString());
    }

    public static async Task<(int ExitCode, string Output, string Error)> StartWithOutputAsync(string exe, StringBuilder cmd, bool visible)
        => await StartWithOutputAsync(exe, cmd.ToString(), visible);

    private static void OutputDataHandler(object sendingProcess, DataReceivedEventArgs outLine)
    {
        // Collect the command output.
        if (!string.IsNullOrEmpty(outLine.Data))
        {
            // Add the text to the collected output.
            _output.Append("  ").AppendLine(outLine.Data);
        }
    }

    private static void ErrorDataHandler(object sendingProcess, DataReceivedEventArgs errLine)
    {
        // Collect the command output.
        if (!string.IsNullOrEmpty(errLine.Data))
        {
            // Add the text to the collected output.
            _error.Append("  ").AppendLine(errLine.Data);
        }

        //// Write the error text to the file
        //// if there is something to write and an error file has been specified.
        //if (!string.IsNullOrEmpty(errLine.Data))
        //{
        //    if (!_errorsWritten)
        //    {
        //        if (_streamError == null)
        //        {
        //            // Open the file.
        //            try
        //            {
        //                _streamError = new StreamWriter(ErrorFile, true);
        //            }
        //            catch (Exception e)
        //            {
        //                Console.WriteLine("Could not open error file!");
        //                Console.WriteLine(e.Message.ToString());
        //            }
        //        }

        //        if (_streamError != null)
        //        {
        //            // Write a header to the file
        //            // if this is the first call to the error output handler.
        //            _streamError.WriteLine();
        //            _streamError.WriteLine(DateTime.Now.ToString());
        //            _streamError.WriteLine("Error output:");
        //        }

        //        _errorsWritten = true;
        //    }

        //    if (_streamError != null)
        //    {
        //        // Write redirected errors to the file.
        //        _streamError.WriteLine(errLine.Data);
        //        _streamError.Flush();
        //    }
        //}
    }
}
