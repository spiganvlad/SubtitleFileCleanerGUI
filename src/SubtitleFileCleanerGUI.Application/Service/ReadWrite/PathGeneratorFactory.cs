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
    public class PathGeneratorFactory : IPathGeneratorFactory
    {
        private readonly IHost host;

        public PathGeneratorFactory(IHost host)
        {
            this.host = host;
        }

        public IPathGenerator CreatePathGenerator(ReadWriteType type)
        {
            return type switch
            {
                ReadWriteType.FileSystem => host.Services.GetRequiredService<IEnumerable<IPathGenerator>>()
                    .First(pg => pg.GetType() == typeof(FileSystemPathGenerator)),

                _ => throw new NotImplementedException($"Creating a path generator is not implemented for type: {type}.")
            };
        }
    }
}
