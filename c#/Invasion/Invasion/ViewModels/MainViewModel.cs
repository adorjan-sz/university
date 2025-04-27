using CommunityToolkit.Mvvm.Input;
using System;
using Game.Model;
using System.Security.Cryptography;
using Game.Persistence;
using System.Reflection.Metadata.Ecma335;
namespace Invasion.ViewModels;

public class InvasionViewModel : ViewModelBase
{
    #region Fields

    private GameModel _model; // modell
    public int Size { get { return _model.Board.Size; } }

    #endregion

    #region Properties

    /// <summary>
    /// Új játék kezdése parancs lekérdezése.
    /// </summary>
    public RelayCommand NewGameCommand { get; private set; }

    /// <summary>
    /// Játék betöltése parancs lekérdezése.
    /// </summary>
    public RelayCommand LoadGameCommand { get; private set; }

    /// <summary>
    /// Játék mentése parancs lekérdezése.
    /// </summary>
    public RelayCommand SaveGameCommand { get; private set; }
    public RelayCommand PauseGameCommand { get; private set; }
    public System.Collections.ObjectModel.ObservableCollection<InvasionField> Fields { get; set; }

    #endregion
    public string HealthCount
    {
        get; set;
    }

    /// <summary>
    /// Fennmaradt játékidő lekérdezése.
    /// </summary>
    public String GameTime
    {
        get { return TimeSpan.FromSeconds(_model.GameTime).ToString("g"); }
    }
    /// <summary>
    /// Új játék eseménye.
    /// </summary>
    public event EventHandler? NewGame;

    /// <summary>
    /// Játék betöltésének eseménye.
    /// </summary>
    public event EventHandler? LoadGame;

    /// <summary>
    /// Játék mentésének eseménye.
    /// </summary>
    public event EventHandler? SaveGame;

    public event EventHandler? PauseGame;

    public InvasionViewModel(GameModel model)
    {
        // játék csatlakoztatása
        _model = model;
        _model.BoardChanged += new EventHandler<BoardEventArgs>(Model_BoardChanged);
        _model.DataChanged += new EventHandler<DataEventArgs>(Model_DataChanged);
        


        // parancsok kezelése
        NewGameCommand = new RelayCommand(OnNewGame);
        LoadGameCommand = new RelayCommand(OnLoadGame);
        SaveGameCommand = new RelayCommand(OnSaveGame);
        PauseGameCommand = new RelayCommand(OnPauseGame);

        Fields = new System.Collections.ObjectModel.ObservableCollection<InvasionField>();
        _model.NewGame();


    }
    private void Model_BoardChanged(object? sender, BoardEventArgs e)
    {
        Fields = new System.Collections.ObjectModel.ObservableCollection<InvasionField>();
        for (Int32 i = 0; i < e.Board.Size; i++) // inicializáljuk a mezőket
        {
            for (Int32 j = 0; j < e.Board.Size; j++)
            {

                Fields.Add(new InvasionField
                {
                    IsEnemy = e.Board[i, j].IsEnemy,
                    IsField = e.Board[i, j].IsField,
                    X = i,
                    Y = j,
                    StepCommand = new RelayCommand<Tuple<Int32, Int32>>(position =>
                    {
                        if (position != null)
                            _model.SetPlayer(position.Item1, position.Item2);
                    })
                    // ha egy mezőre léptek, akkor jelezzük a léptetést, változtatjuk a lépésszámot
                });
            }
        }
        OnPropertyChanged(nameof(Fields));
        OnPropertyChanged(nameof(GameTime));
    }
    private void Model_DataChanged(object? sender, DataEventArgs e)
    {
        HealthCount = e.Health.ToString();
        OnPropertyChanged(nameof(HealthCount));


    }
    private void OnNewGame()
    {
        NewGame?.Invoke(this, EventArgs.Empty);
    }
    private void OnLoadGame()
    {
        LoadGame?.Invoke(this, EventArgs.Empty);
    }
    private void OnSaveGame()
    {
        SaveGame?.Invoke(this, EventArgs.Empty);
    }
    private void OnPauseGame()
    {
        PauseGame?.Invoke(this, EventArgs.Empty);
    }
}


