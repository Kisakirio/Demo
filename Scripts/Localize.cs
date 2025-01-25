using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
	public class Localize
	{

		public class SystemText
		{
			public string keyword;

			public string text;
		}

		private static List<SystemText> jsonListText;

		private static Dictionary<string,SystemText> SystemTextDictionary;

		public static string GetLocalizeTextByKeywor(string key)
		{

			if (jsonListText == null)
			{
				jsonListText = (List<SystemText>)JsonConvert.DeserializeObject(LocalizeManager.instance.SystemTextAsset.text, typeof(List<SystemText>));
				SystemTextDictionary = new Dictionary<string, SystemText>();
				foreach (var item in jsonListText)
				{
					if (!SystemTextDictionary.ContainsKey(item.keyword))
					{
						SystemTextDictionary.Add(item.keyword,item);
					}
				}
			}

			SystemText value = null;
			bool flag = false;
			flag = SystemTextDictionary.TryGetValue(key,out value);
			if (flag)
			{
				return value.text;
			}

			return key;
		}

		public static string ReplaceButton(string text, string search,string target=null)
		{
			if (!text.Contains(search))
			{
				return text;
			}
			if (target != null)
			{
				text = text.Replace(search, target);

			}

			return text;

		}

		public static string ReplaceButton(string s)
		{
			s = ReplaceButton(s, "{LEFT}","{A}");
			s = ReplaceButton(s, "{Space}");
			s = ReplaceButton(s, "{RIGHT}","{D}");
			s = ReplaceButton(s, "D");
			s = ReplaceButton(s, "{CONFIRM}","{ENTER}");

			s= s.Replace("{", "<color=#07AAF7>[");
			s = s.Replace("}", "]</color>");

			return s;


		}
	}

