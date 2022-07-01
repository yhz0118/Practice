using System;
using System.IO;

class JPEG
{
	public byte[] read(string from)
	{
		byte[] bb;
		using (var fs = new FileStream(from, FileMode.Open, FileAccess.Read))
		{
			int len = (int)fs.Length;

			Console.WriteLine(len);

			bb = new byte[len];
			fs.Read(bb, 0, len);
		}
		return bb;
	}

	public byte[] find_thumbnail(byte[] image_bytes)
	{
		byte[] bb;
		int count = 1;
		int b_pos = 0;
		int e_pos = 0;

		for (int i = 0; i < image_bytes.Length; i++)
		{
			if (image_bytes[i] == '\xff' && image_bytes[i + 1] == '\xd8' && image_bytes[i + 2] == '\xff')
			{
				if (count == 2)
				{
					b_pos = i;
					for (int j = i; j < image_bytes.Length; j++)
					{
						if (image_bytes[j] == '\xff' && image_bytes[j + 1] == '\xd9')
						{
							e_pos = j;
						}
					}
				}
				count++;
			}
		}
		bb = new byte[e_pos - b_pos + 1];
		for (int i = 0; i < e_pos - b_pos + 1; i++)
		{
			bb[i] = image_bytes[b_pos + i];
		}



		return bb;
	}



	public bool save(byte[] thumbnail, string to)
	{
		try
		{
			using (var fs = new FileStream(to, FileMode.Create))
			{
				for (int i = 0; i < thumbnail.Length; i++)
				{
					fs.WriteByte(thumbnail[i]);
				}
			}
			return true;
		}
		catch (Exception ex)
		{
			return false;
		}
	}


	class Program
	{
		static void Main(string[] args)
		{
			var jpeg = new JPEG();

			var b1 = jpeg.read(from: "/Users/yunseochoi/Desktop/flower.jpeg");
			var b2 = jpeg.find_thumbnail(b1);
			var ok = jpeg.save(thumbnail: b2, to: "/Users/yunseochoi/Desktop/thumbnail.jpeg");

			var msg = "";
			if (ok) msg = "success";
			else msg = "fail";

			Console.WriteLine("Save thumbnail " + msg);
			Console.ReadKey();
		}
	}
}