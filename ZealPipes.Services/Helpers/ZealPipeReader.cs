using System.Collections.Generic;
using System.IO.Pipes;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System;
using ZealPipes.Services.Models;
using System.Text.Json;
using ZealPipes.Common;

namespace ZealPipes.Services.Helpers
{
    public class ZealPipeReader
    {
        internal class PipeMessageReceivedEventArgs : EventArgs
        {
            internal PipeMessage Message { get; private set; }
            internal int ProcessId { get; private set; }

            internal PipeMessageReceivedEventArgs(int processId, PipeMessage message)
            {
                Message = message;
                ProcessId = processId;
            }
        }
        private readonly ZealSettings _zealSettings;
        private bool _keepReading;
        private bool _processMessages;
        private HashSet<int> _connectedProcesses = new HashSet<int>();
        internal event EventHandler<PipeMessageReceivedEventArgs> OnPipeMessageReceived;

        public ZealPipeReader(ZealSettings zealSettings)
        {
            _zealSettings = zealSettings;
        }
        internal void StopReading(int processId)
        {
            _keepReading = false;
        }
        internal async Task StartReading(int processId)
        {
            if (_connectedProcesses.Contains(processId))
            {
                Console.WriteLine($"Already reading Zeal pipe for ProcessId {processId}.");
                return;
            }
            _connectedProcesses.Add(processId);
            _keepReading = true;
            string pipeName = $"{_zealSettings.PipePrefix}_{processId}";

            try
            {
                using (NamedPipeClientStream pipeClient = new NamedPipeClientStream(".", pipeName, PipeDirection.In))
                {
                    pipeClient.Connect();

                    Console.WriteLine($"Connected to the named pipe for process {processId}.");
                    JsonSplitter splitter = new JsonSplitter();
                    while (_keepReading)
                    {
                        if (pipeClient.IsConnected)
                        {
                            byte[] buffer = new byte[_zealSettings.BufferSize];
                            int bytesRead = await pipeClient.ReadAsync(buffer, 0, buffer.Length);

                            if (bytesRead > 0)
                            {
                                string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                                foreach (string json in splitter.SplitJson(message))
                                {
                                    try
                                    {
                                        PipeMessage pm = JsonSerializer.Deserialize<PipeMessage>(json);
                                        //Console.WriteLine($"Received from process {processId} {pd.Character}: {pd.Data}");
                                        OnPipeMessageReceived?.Invoke(this, new PipeMessageReceivedEventArgs(processId, pm));
                                    }
                                    catch (JsonException ex)
                                    {
                                        // Handle JSON parsing error
                                        Console.WriteLine("Error parsing JSON: " + ex.Message);
                                    }
                                }

                            }
                            else
                            {
                                await Task.Delay(10); // Wait for data if none available
                            }
                        }
                        else
                        {
                            // Pipe is disconnected, remove the disconnected process from the active connections list
                            Console.WriteLine($"Process {processId} disconnected.");
                            _connectedProcesses.Remove(processId);
                            break;
                        }
                    }
                }
            }
            catch (IOException ex) when (ex.InnerException is ObjectDisposedException)
            {
                // Handle disconnection: Remove the disconnected process from the active connections list
                Console.WriteLine($"Process {processId} disconnected.");
                _connectedProcesses.Remove(processId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception for process {processId}: {ex.Message}");
            }
            finally
            {
                // Remove the process ID from the connected processes set when the connection ends
                _connectedProcesses.Remove(processId);
            }
        }

    }


}
