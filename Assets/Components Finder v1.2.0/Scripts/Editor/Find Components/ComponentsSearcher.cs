/*Script created by Pierre Stempin*/

using UnityEngine;
using UnityEditor;
using System.Reflection;

namespace ComponentsFinder
{
	/// <summary>
	/// Does the main feature of the Components Finder : 
	/// the search by transforming a click on a specific button to a string search in the hierarchy window
	/// </summary>
	public class ComponentsSearcher
	{
		public const int filterMode_All = 0;
		public const int filterMode_Name = 1;
		public const int filterMode_Type = 2;

		static SearchableEditorWindow hierarchy;

		public static void SearchFilter (string componentToSearch) 
		{
			SearchFilter (componentToSearch, 0);
		}

		public static void SearchFilter (string filter, int filterMode) 
		{
			SearchFilter (filter, filterMode, ComponentsFinderStrings.SearchByType);
		}

		public static void SearchFilter (string filter, int filterMode, string prefix)
		{
			SearchableEditorWindow [] windows = (SearchableEditorWindow []) Resources.FindObjectsOfTypeAll (typeof (SearchableEditorWindow));

			foreach (SearchableEditorWindow window in windows) 
			{
				string currentName = window.GetType ().ToString ();
				string nameToCheck = ComponentsFinderStrings.UnityEditorSceneHierarchyWindow;

				if (currentName == nameToCheck) 
				{
					hierarchy = window;
					break;
				}
			}

			if (hierarchy != null)
			{
				string searchMethod = ComponentsFinderStrings.SetSearchFilter;
				BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.Instance;
				
				MethodInfo setSearchType = typeof (SearchableEditorWindow).GetMethod (searchMethod, bindingFlags); 
				filter = prefix + filter;
				object [] parameters = new object [] {filter, filterMode, false};
				
				setSearchType.Invoke (hierarchy, parameters);
			}
		}
	}
}
