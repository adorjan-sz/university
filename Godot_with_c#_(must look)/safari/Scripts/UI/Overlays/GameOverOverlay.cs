using Godot;
using System;

public partial class GameOverOverlay : Control
{
    [Export] public Label ResultLabel;
    [Export] public Button YesButton;
    [Export] public Button NoButton;
    [Export] public NodePath EntityManagerPath;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        EntityManager entityManager = GetNode<EntityManager>(EntityManagerPath);
        entityManager.GameOver += OnGameOver;
        YesButton.Pressed += OnYesButton;
        NoButton.Pressed += OnNoButton;
        this.Visible = false;
    }

    private void OnGameOver(bool won)
    {
        this.Visible = true;
        if (won)
            ResultLabel.Text = "You won!";
        else
            ResultLabel.Text = "You lost!";
    }

    private void OnNoButton()
    {
        GetTree().Quit();
    }

    private void OnYesButton()
    {
        GetTree().Root.ProcessMode = ProcessModeEnum.Always;
        SceneTree tree = GetTree();
        tree.Root.PrintTreePretty();
        Control mainMenu = tree.Root.FindChild("MainMenu", true, false) as Control;
        mainMenu.Visible = true;
        tree.CurrentScene = mainMenu;
        tree.Root.FindChild("MainModel", true, false).QueueFree();
    }
}
