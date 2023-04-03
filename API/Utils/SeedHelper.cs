using System.Text.Json;

namespace API.Utils
{
	public static class SeedHelper
	{
		public static List<TEntity>? SeedData<TEntity>(string fileName)
		{
			string currentDirectory = Directory.GetCurrentDirectory();
			string path = "SeedData";
			string fullPath = Path.Combine(currentDirectory, path, fileName);

			if (File.Exists(fullPath) == false) return null;

			using StreamReader sr = new(fullPath);
			string json = sr.ReadToEnd();

			return JsonSerializer.Deserialize<List<TEntity>>(json);
		}
	}
}
