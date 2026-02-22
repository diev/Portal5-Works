#region License
/*
Copyright 2025 Dmitrii Evdokimov
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

using System.Net;

namespace Diev.Extensions.Http.Mock;

/// <summary>
/// Custom HttpContent for streaming data in chunks, simulating a slow network.
/// </summary>
public class StreamingContent : HttpContent
{
    private readonly byte[] _data;
    private readonly int _chunkSize;

    /// <summary>
    /// Initializes a new instance of the StreamingContent class with specified data and chunk size.
    /// </summary>
    /// <param name="data">The data to stream.</param>
    /// <param name="chunkSize">The size of each data chunk.</param>
    public StreamingContent(byte[] data, int chunkSize)
    {
        _data = data ?? throw new ArgumentNullException(nameof(data));
        _chunkSize = chunkSize;
    }

    protected override async Task SerializeToStreamAsync(Stream stream, TransportContext? context)
    {
        for (int i = 0; i < _data.Length; i += _chunkSize)
        {
            int remaining = _data.Length - i;
            int count = Math.Min(remaining, _chunkSize);

            await stream.WriteAsync(_data, i, count);
            await Task.Delay(50); // Simulate slow network or "inject" issues
        }
    }

    protected override bool TryComputeLength(out long length)
    {
        length = _data.Length;
        return true;
    }
}
