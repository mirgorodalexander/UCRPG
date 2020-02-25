using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("Status", "LVL", "EXP", "EXPNEED", "HP", "MP", "ATK", "ATKD", "DEF")]
	public class ES3Type_Player : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3Type_Player() : base(typeof(Player))
		{
			Instance = this;
		}

		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (Player)obj;
			
			writer.WriteProperty("Status", instance.Status);
			writer.WriteProperty("LVL", instance.LVL, ES3Type_int.Instance);
			writer.WriteProperty("EXP", instance.EXP, ES3Type_int.Instance);
			writer.WriteProperty("EXPNEED", instance.EXPNEED, ES3Type_int.Instance);
			writer.WriteProperty("HP", instance.HP, ES3Type_int.Instance);
			writer.WriteProperty("MP", instance.MP, ES3Type_int.Instance);
			writer.WriteProperty("ATK", instance.ATK, ES3Type_int.Instance);
			writer.WriteProperty("ATKD", instance.ATKD, ES3Type_float.Instance);
			writer.WriteProperty("DEF", instance.DEF, ES3Type_int.Instance);
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (Player)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "Status":
						instance.Status = reader.Read<Player._Status>();
						break;
					case "LVL":
						instance.LVL = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "EXP":
						instance.EXP = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "EXPNEED":
						instance.EXPNEED = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "HP":
						instance.HP = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "MP":
						instance.MP = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "ATK":
						instance.ATK = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					case "ATKD":
						instance.ATKD = reader.Read<System.Single>(ES3Type_float.Instance);
						break;
					case "DEF":
						instance.DEF = reader.Read<System.Int32>(ES3Type_int.Instance);
						break;
					default:
						reader.Skip();
						break;
				}
			}
		}
	}

	public class ES3Type_PlayerArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3Type_PlayerArray() : base(typeof(Player[]), ES3Type_Player.Instance)
		{
			Instance = this;
		}
	}
}