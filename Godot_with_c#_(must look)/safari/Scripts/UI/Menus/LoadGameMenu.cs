using Godot;
using System;
using System.IO;
using System.Collections.Generic;

public partial class LoadGameMenu : Control
{
	[Export] public OptionButton FileDropdown;
	[Export] public Button LoadButton;
	[Export] public Button BackButton;
	[Export] public PackedScene MainModelScene;
	


	private VBoxContainer _vbox;

	public override void _Ready()
	{		
		LoadSaveOptions();

		LoadButton.Pressed += OnLoadPressed;
		BackButton.Pressed += OnBackPressed;
	}

	private void LoadSaveOptions()
	{
		string saveFolder = "user://"; // All your saves are in user://

		// Get all files in save folder
		var dir = DirAccess.Open(saveFolder);

		if (dir == null)
		{
			GD.PrintErr("Cannot open save folder!");
			return;
		}

		dir.ListDirBegin();
		string filename = dir.GetNext();

		while (filename != "")
		{
			if (!dir.CurrentIsDir() && filename.EndsWith(".json"))
				FileDropdown.AddItem(filename.Replace(".json", ""));

			filename = dir.GetNext();
		}

		dir.ListDirEnd();

		if (FileDropdown.ItemCount == 0)
		{
			FileDropdown.Disabled = true;
			LoadButton.Disabled = true;
		}
		else
			FileDropdown.Selected = FileDropdown.GetSelectableItem();
	}

	private void OnLoadPressed()
	{
		OnSaveSelected(FileDropdown.GetItemText(FileDropdown.GetSelectedId()));
	}

	private void OnBackPressed()
	{
		SceneTree tree = GetTree();
		tree.Root.PrintTreePretty();
		Control mainMenu = tree.Root.FindChild("MainMenu", true, false) as Control;
		mainMenu.Visible = true;
		tree.CurrentScene = mainMenu;
		tree.Root.FindChild("LoadGameMenu", true, false).QueueFree();

	}

	private void OnSaveSelected(string saveName)
	{
		GD.Print($"Loading save: {saveName}");

		if (MainModelScene != null)
		{
			var mainModel = (MainModel)MainModelScene.Instantiate();
			GameVariables.Instance.ParkName = saveName;

			var tree = GetTree();
			tree.CurrentScene.QueueFree();
			tree.Root.AddChild(mainModel);
			tree.CurrentScene = mainModel;
			mainModel.LoadGame(saveName);
			tree.Root.FindChild("LoadGameMenu", true, false).QueueFree();
		}
		else
		{
			GD.PrintErr("MainModelScene is not assigned in the Inspector!");
		}
	}
}
