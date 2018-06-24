using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Orleans;
using Orleans.Providers;
using Orleans.Runtime;
using Orleans.Runtime.Configuration;
using Orleans.Serialization;
using Orleans.Storage;

namespace Persistence.Host.FileStorageProvider
{
	public class FileStorageProvider : IStorageProvider
	{
		private JsonSerializerSettings _jsonSettings;
		public string RootDirectory { get; set; }

		public Logger Log { get; set; }

		public string Name { get; set; }

		public Task Init(string name, IProviderRuntime providerRuntime, IProviderConfiguration config)
		{
			_jsonSettings = OrleansJsonSerializer.UpdateSerializerSettings(
								OrleansJsonSerializer.GetDefaultSerializerSettings(
															providerRuntime.ServiceProvider.GetRequiredService<SerializationManager>(), 
															providerRuntime.GrainFactory), 
								config);

			Name = name;
			if (string.IsNullOrWhiteSpace(config.Properties["RootDirectory"]))
				throw new ArgumentException("RootDirectory property not set");

			var directory = new DirectoryInfo(config.Properties["RootDirectory"]);

			if (!directory.Exists)
				directory.Create();

			RootDirectory = directory.FullName;

			return Task.CompletedTask;
		}

		public Task Close()
		{
			return Task.CompletedTask;
		}

		public async Task ReadStateAsync(string grainType, GrainReference grainRef, IGrainState grainState)
		{
			var fileInfo = GrainFile(grainRef, grainState);
			if (!fileInfo.Exists)
				return;

			using (var stream = fileInfo.OpenText())
			{
				var storedData = await stream.ReadToEndAsync();
				grainState.State = JsonConvert.DeserializeObject(storedData, grainState.State.GetType(), _jsonSettings);
			}
		}

		private FileInfo GrainFile(GrainReference grainRef, IGrainState grainState)
		{
			var collectionName = grainState.GetType().Name;
			var key = grainRef.ToKeyString();

			var fName = key + "." + collectionName;
			var path = Path.Combine(RootDirectory, fName);

			var fileInfo = new FileInfo(path);
			return fileInfo;
		}

		public async Task WriteStateAsync(string grainType, GrainReference grainRef, IGrainState grainState)
		{
			var storedData = JsonConvert.SerializeObject(grainState.State, _jsonSettings);

			var fileInfo = GrainFile(grainRef, grainState);

			using (var stream = new StreamWriter(fileInfo.Open(FileMode.Create, FileAccess.Write)))
			{
				await stream.WriteAsync(storedData);
			}
		}

		public Task ClearStateAsync(string grainType, GrainReference grainRef, IGrainState grainState)
		{
			var fileInfo = GrainFile(grainRef, grainState);
			fileInfo.Delete();
			return Task.CompletedTask;
		}

	}

	public static class ProviderConfigurationExtensions
	{

		public static void AddSimpleFileSystemStorageProvider(
			this ClusterConfiguration config,
			string providerName,
			string rootDirectory = "./Storage")
		{
			if (string.IsNullOrWhiteSpace(providerName))
				throw new ArgumentNullException(nameof(providerName));

			var properties = new Dictionary<string, string>
			{
				{"RootDirectory", rootDirectory}
			};

			config.Globals.RegisterStorageProvider<FileStorageProvider>(providerName, properties);
		}
	}
}
