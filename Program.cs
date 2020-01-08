using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace httpListener
{
    class Program
    {
		private static int messageCounter = 0;
		static void Main(string[] args)
		{
			using (var listener = new HttpListener())
			{
				listener.Prefixes.Add("http://localhost:5000/");

				listener.Start();

				for (; ; )
				{

					Console.WriteLine("Listening...");

					HttpListenerContext context = listener.GetContext();
					HttpListenerRequest request = context.Request;
					Console.WriteLine($"Total Length:{request.ContentLength64}  {request.ToString()}  {request.HttpMethod}");
					string text;
					using (var reader = new StreamReader(request.InputStream,
														 request.ContentEncoding))
					{
						text = reader.ReadToEnd();
					}
					Console.WriteLine($"JSON:{text}");
					using (System.IO.StreamWriter jsonFile = new System.IO.StreamWriter($"C:\\Extract\\listen{messageCounter}.json"))
					{
						jsonFile.WriteLine(text);
					}
					// TODO: read and parse the JSON data from 'request.InputStream'

					using (HttpListenerResponse response = context.Response)
					{
						// returning some test results

						// TODO: return the results in JSON format

						string responseString = "<HTML><BODY>Hello, world!</BODY></HTML>";
						byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
						response.ContentLength64 = buffer.Length;
						using (var output = response.OutputStream)
						{
							output.Write(buffer, 0, buffer.Length);
						}
					}
				}
			}
		}
	}
}
