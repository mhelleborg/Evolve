﻿using System;
using System.Runtime.InteropServices;
using Docker.DotNet;
using Docker.DotNet.Models;

namespace Evolve.Test.Utilities
{
    internal class DockerContainer : IDockerContainer
    {
        private readonly DockerClient _client;
        private readonly bool _rm;

        public DockerContainer(string id, bool rm = true)
        {
            Id = id;
            _rm = rm;

            _client = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                ? new DockerClientConfiguration(new Uri("npipe://./pipe/docker_engine")).CreateClient()
                : new DockerClientConfiguration(new Uri("unix:///var/run/docker.sock")).CreateClient();
        }

        public string Id { get; }

        public bool Start() => _client.Containers.StartContainerAsync(Id, null).Result;
        public bool Stop() => _client.Containers.StopContainerAsync(Id, new ContainerStopParameters()).Result;
        public void Remove() => _client.Containers.RemoveContainerAsync(Id, new ContainerRemoveParameters()).Wait();

        public void Dispose()
        {
            Stop();

            if(_rm)
            {
                Remove();
            }

            _client.Dispose();
        }
    }
}
