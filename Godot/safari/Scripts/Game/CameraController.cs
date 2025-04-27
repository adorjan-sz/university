
using Godot;
using System;

public partial class CameraController : Camera2D
{
    [Signal] public delegate void CameraChangedEventHandler(Vector2 position, Vector2 zoom);

    [Export] public float MoveSpeed = 500f; // Camera movement speed
	[Export] public float ZoomSpeed = 0.1f; // Zoom speed
	[Export] public float MinZoom = 0.5f; // Minimum zoom level

	[Export] public float MaxZoom = 2.0f; // Maximum zoom level

    private BuyMenuOverlay buyMenuOverlay;
    private MiniMap minimap;

	private bool cameraDragEnabled;
    private bool isDragging;
    private Vector2 dragStartPos;

    private Vector2 _lastPosition;
    private Vector2 _lastZoom;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        buyMenuOverlay = GetNode<BuyMenuOverlay>("/root/MainModel/GameOverlay/Control/TabContainer");
		cameraDragEnabled = true;
        buyMenuOverlay.CameraDrag += OnCameraDrag;
    



        minimap = GetNode<MiniMap>("/root/MainModel/GameOverlay/Control/MiniMap");
        minimap.MinimapClicked += OnMinimapClicked;

        // Store the initial state
        _lastPosition = Vector2.Zero;
        _lastZoom = Vector2.Zero;
    }


	private void OnCameraDrag(bool enabled)
	{
		cameraDragEnabled = enabled;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
		Vector2 direction = Vector2.Zero;

		// Keyboard movement (WASD or arrow keys)
		if (Input.IsActionPressed("ui_right"))
			direction.X += 1;
		if (Input.IsActionPressed("ui_left"))
			direction.X -= 1;
		if (Input.IsActionPressed("ui_down"))
			direction.Y += 1;
		if (Input.IsActionPressed("ui_up"))
			direction.Y -= 1;

		// Movement
		Position += direction.Normalized() * MoveSpeed * (float)delta / (float)Engine.TimeScale;

		// Zoom adjustment using the mouse wheel
		if (Input.IsActionJustPressed("zoom_in"))
			Zoom *= (1f + ZoomSpeed);
		if (Input.IsActionJustPressed("zoom_out"))
			Zoom *= (1f - ZoomSpeed);

        // Limiting the zoom level


        Zoom = new Vector2(Mathf.Clamp(Zoom.X, MinZoom, MaxZoom), Mathf.Clamp(Zoom.Y, MinZoom, MaxZoom));

        // Check if the camera has moved or zoom changed
        if (Position != _lastPosition || Zoom != _lastZoom)
        {
            EmitSignal(nameof(CameraChanged), Position, Zoom);
            _lastPosition = Position;
            _lastZoom = Zoom;
        }
    }


    public override void _UnhandledInput(InputEvent inputEvent)
    {
        if (!cameraDragEnabled)
            return;

        if (inputEvent is InputEventMouseButton mouseEvent )
        {
            if (mouseEvent.ButtonIndex == MouseButton.Left)
            {
                if (mouseEvent.Pressed) 
					isDragging = true;
                else 
					isDragging = false;
            }
        }

        if (isDragging && inputEvent is InputEventMouseMotion mouseMotion)
            Position -= mouseMotion.Relative / Zoom;

    }

    public void OnMinimapClicked(Vector2 newCameraPosition)
    {
        Tween tween = GetTree().CreateTween();
        tween.TweenProperty(this, "position", newCameraPosition, 0.8f * Engine.TimeScale)
             .SetTrans(Tween.TransitionType.Sine)
             .SetEase(Tween.EaseType.Out);
    }
}

