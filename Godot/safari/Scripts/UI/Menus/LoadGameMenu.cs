using Godot;
using System;
using System.IO;
using System.Collections.Generic;

public partial class LoadGameMenu : Control
{
	[Export] public NodePath VBoxPath;
	[Export] public PackedScene MainModelScene;

	private VBoxContainer _vbox;

	public override void _Ready()
	{
		_vbox = GetNode<VBoxContainer>(VBoxPath);

		LoadSaveButtons();
	}

	private void LoadSaveButtons()
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
			{
				// Create a Button for each save file
				var button = new Button();
				button.Text = filename.Replace(".json", ""); // show clean park name
				button.Pressed += () => OnSaveSelected(button.Text);
				_vbox.AddChild(button);
			}

			filename = dir.GetNext();
		}

		dir.ListDirEnd();
	}

	private void OnSaveSelected(string saveName)
	{
		GD.Print($"Loading save: {saveName}");

		if (MainModelScene != null)
		{
			var mainModel = (MainModel)MainModelScene.Instantiate();
			

			var tree = GetTree();
			tree.CurrentScene.QueueFree();
			tree.Root.AddChild(mainModel);
			tree.CurrentScene = mainModel;
			mainModel.LoadGame(saveName);
		}
		else
		{
			GD.PrintErr("MainModelScene is not assigned in the Inspector!");
		}
	}
}
