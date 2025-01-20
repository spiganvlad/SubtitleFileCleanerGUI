using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SubtitleFileCleanerGUI.Application.Abstractions.Enums;
using SubtitleFileCleanerGUI.Application.Abstractions.Service.ReadWrite;
using SubtitleFileCleanerGUI.Application.Service.ReadWrite.FileSystem;

namespace SubtitleFileCleanerGUI.Application.Service.ReadWrite
{
    public class ReaderFactory : IAsyncReaderFactory
    {
        private readonly IHost host;

        public ReaderFactory(IHost host)
        {
            this.host = host;
        }

        public IAsyncReader CreateAsyncReader(ReadWriteType type)
        {
            return type switch
            {
                ReadWriteType.FileSystem => host.Services.GetRequiredService<IEnumerable<IAsyncReader>>()
                    .First(ar => ar.GetType() == typeof(FileSystemAsyncReader)),

                _ => throw new NotImplementedException($"Creating a reader is not implemented for type: {type}.")
            };
        }
    }
}
