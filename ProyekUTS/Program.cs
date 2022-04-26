using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;

namespace ProyekUTS
{
	class Program
	{
		static void Main(string[] args)
		{
			var ourWindow = new NativeWindowSettings()
			{
				Size = new Vector2i(1200, 760),
				Title = "Proyek UTS"
			};

			using (var window = new Window(GameWindowSettings.Default, ourWindow))
			{
				window.Run();
			}
		}
	}
}
