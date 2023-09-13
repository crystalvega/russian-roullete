using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Russian_Roullete
{
	public class People
	{
		string name = "";
		int shoots = 0;

		public People(string name)
		{
			this.name = name;
		}
		public void ShootPlus()
		{
			shoots++;
			Console.WriteLine($"Игрок {name} стрельнул и выжил");
		}
		public void Death()
		{
			Console.WriteLine($"Игрок {name} стрельнул и умер.");
		}
		public void ShootsDo()
		{
			Console.WriteLine($"Игрок {name} сделал {shoots} выстрелов за игру.");
		}
	}
	public class Roullete
	{
		int bullet = 0;
		int currentBullet = 0;
		Random rnd;
		public People first;
		public People second;
		People current;
		bool reroll;
		public Roullete(People first, People second, bool reroll)
		{
			this.first = first;
			this.second = second;
			this.reroll = reroll;
			current = first;
			rnd = new Random();
			Task.Delay(500);
			Roll();
		}
		public bool Shoot()
		{
			if(bullet == currentBullet) 
			{
				current.Death();
				return true;
			}
			else
			{
				current.ShootPlus();
				if (currentBullet == 6)
					currentBullet = 0;
				else
					currentBullet++;
				if(current == first)
					current = second;
				else current = first;
				if (reroll)
				{
					Roll();
				}
				return false;
			}
		}
		public void Roll()
		{
			bullet = rnd.Next(0,6);
			Task.Delay(50);
			currentBullet = rnd.Next(0, 6);
		}
	}
	internal class Program
	{
		static void Main(string[] args)
		{
			string fileName = "config.ini";

			var ini = new IniFile();
			ini["Roullete"]["First Player"] = "Первый";
			ini["Roullete"]["Second Player"] = "Второй";
			ini["Roullete"]["Reroll"] = false;
			if(File.Exists(fileName))
				ini.Load(fileName);
			ini.Save(fileName);
			string firstPlayerName = ini["Roullete"]["First Player"].GetString();
			string secondPlayerName = ini["Roullete"]["Second Player"].GetString();
			bool reroll = ini["Roullete"]["Reroll"].ToBool();

			bool endgame = false;
			People first = new People(firstPlayerName);
			People second = new People(secondPlayerName);
			Roullete roul = new Roullete(first, second, reroll);
			while(!endgame)
			{
				endgame = roul.Shoot();
				Task.Delay(1000);
			}
			Console.WriteLine("Общий счёт по игре:");
			roul.first.ShootsDo();
			roul.second.ShootsDo();
			Console.ReadKey();
		}
	}
}
