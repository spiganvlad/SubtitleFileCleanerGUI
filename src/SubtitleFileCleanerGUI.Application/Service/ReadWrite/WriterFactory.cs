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
    public class WriterFactory : IAsyncWriterFactory
    {
        private readonly IHost host;

        public WriterFactory(IHost host)
        {
            this.host = host;
        }

        public IAsyncWriter CreateAsyncWriter(ReadWriteType type)
        {
            return type switch
            {
                ReadWriteType.FileSystem => host.Services.GetRequiredService<IEnumerable<IAsyncWriter>>()
                    .First(aw => aw.GetType() == typeof(FileSystemAsyncWriter)),

                _ => throw new NotImplementedException($"Creating a writer is not implemented for type: {type}.")
            };
        }
    }
}
