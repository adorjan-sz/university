
using Godot;
using System;
using System.Collections.Generic;

public enum DayState { DAY, NIGHT };

public partial class LightManager : DirectionalLight2D
{
	[Signal] public delegate void TimeChangedEventHandler(double time);
	[Signal] public delegate void DayPassedEventHandler();
	List<PointLight2D> lights = new List<PointLight2D>();
	public List<PointLight2D> GetLights => lights;
	bool lightsOn = false;
	Color DayColor = Colors.White; //const
	Color NightColor = new Color("#0e0821"); //const

	public const double DayStart = 8 * 60; //8:00
	public const double NightStart = 22 * 60; //22:00
	public const double TransitionRange = 2 * 60; //2:00

	MapManager mapManager;
	TileMapLayer layer;
	[Export] Texture2D lightTexture = new Texture2D();

	double currentTime = DayStart;
	DayState currentDaylightState;
	public bool GetLightsON
	{
		get => lightsOn;
		set
		{
			lightsOn = value;

		}
	}
	public DayState GetDayLightState
	{
		get => currentDaylightState;
		set
		{
			currentDaylightState = value;
		}
	}
	public double GetCurrentTime
	{
		get => currentTime;
		set
		{
			currentTime = value;
		}
	}


	public List<Vector2> GetLightPos()
	{
		List<Vector2> lightPos = new List<Vector2>();
		foreach (PointLight2D light in lights)
		{
			lightPos.Add(light.Offset);
		}
		return lightPos;
	}
	public override void _Ready()
	{
		BlendMode = BlendModeEnum.Mix;

		mapManager = (MapManager)GetParent();
		layer = mapManager.GetNode<TileMapLayer>("Layer1");

		mapManager.LightPlaced += OnLightPlaced;
		mapManager.LightSold += OnLightSold;
	}


	public override void _Process(double delta)
	{
		//Time update
		currentTime += delta * 10;
		if (currentTime >= 1440)
		{
			currentTime -= 1440;
		}
		EmitSignal(SignalName.TimeChanged, currentTime);

		//State update
		if (currentTime < DayStart || currentTime >= NightStart)
		{
			currentDaylightState = DayState.NIGHT;
		}
		else if (currentTime >= DayStart && currentTime < NightStart)
		{
			currentDaylightState = DayState.DAY;
		}

		//Ambient Light Update
		//Night -> Day
		if (currentTime >= DayStart - TransitionRange && currentTime < DayStart + TransitionRange)
		{
			Color = NightColor.Lerp(DayColor, (float)((currentTime - (DayStart - TransitionRange)) / (2 * TransitionRange)));
		}
		//Day -> Night
		else if (currentTime >= NightStart - TransitionRange && currentTime < NightStart + TransitionRange)
		{
			Color = DayColor.Lerp(NightColor, (float)((currentTime - (NightStart - TransitionRange)) / (2 * TransitionRange)));
		}

		//Placed Light Update
		if (lightsOn && currentDaylightState == DayState.DAY)
		{
			foreach (Light2D light in lights)
			{
				light.Enabled = false;
			}
			lightsOn = false;
		}
		else if (!lightsOn && currentDaylightState == DayState.NIGHT)
		{
			foreach (Light2D light in lights)
			{
				light.Enabled = true;
			}
			lightsOn = true;
			EmitSignal(nameof(DayPassed));
		}


	}

	private void OnLightPlaced(Vector2 pos)
	{

		PointLight2D newLight = new PointLight2D();

		newLight.Offset = pos;
		newLight.Color = DayColor;
		newLight.BlendMode = BlendModeEnum.Add;
		newLight.Enabled = lightsOn;
		newLight.Texture = lightTexture;
		newLight.Energy = 0.75f;
		newLight.Height = 100;
		newLight.RangeLayerMin = 0;

		AddChild(newLight);
		lights.Add(newLight);
	}

	private void OnLightSold(Vector2 pos)
	{
		foreach (PointLight2D light in lights)
		{
			if (light.Offset == pos)
			{
				lights.Remove(light);
				light.QueueFree();
				break;
			}
		}
	}
	//--------------------Load/Save--------------------
	public LightSave Save()
	{
		return new LightSave(this);
	}
	public void Load(LightSave save)
	{
		if (save == null)
		{
			GD.PrintErr("LightSave is null!");
			return;
		}

		// Clear existing lights first
		foreach (var light in lights)
		{
			light.QueueFree();
		}
		lights.Clear();

		// Rebuild lights
		for (int i = 0; i < save.LightPositionsX.Count; i++)
		{
			Vector2 pos = new Vector2(save.LightPositionsX[i], save.LightPositionsY[i]);
			PointLight2D newLight = new PointLight2D
			{
				Offset = pos,
				Color = DayColor,
				BlendMode = BlendModeEnum.Add,
				Enabled = save.LightsOn,
				Texture = lightTexture,
				Energy = 0.75f,
				Height = 100,
				RangeLayerMin = 0
			};

			AddChild(newLight);
			lights.Add(newLight);
		}

		// Restore other variables
		currentTime = save.CurrentTime;
		currentDaylightState = Enum.TryParse(save.CurrentDayState, out DayState parsedState) ? parsedState : DayState.DAY;
		lightsOn = save.LightsOn;

		// Update light enabling/disabling immediately
		foreach (var light in lights)
		{
			light.Enabled = lightsOn;
		}
		// Instantly update ambient Color after loading time
		if (currentTime >= DayStart - TransitionRange && currentTime < DayStart + TransitionRange)
		{
			Color = NightColor.Lerp(DayColor, (float)((currentTime - (DayStart - TransitionRange)) / (2 * TransitionRange)));
		}
		else if (currentTime >= NightStart - TransitionRange && currentTime < NightStart + TransitionRange)
		{
			Color = DayColor.Lerp(NightColor, (float)((currentTime - (NightStart - TransitionRange)) / (2 * TransitionRange)));
		}
		else if (currentTime >= DayStart && currentTime < NightStart)
		{
			Color = DayColor;
		}
		else
		{
			Color = NightColor;
		}


		GD.PrintErr("LightManager loaded successfully.");
	}
}
