using System.Windows;
using System.Windows.Input;
using TicTacToe.Game;

namespace TicTacToe;

public partial class GameWindow : Window
{
    private const int SIZE = 10;
    private readonly int _cellSize;
    private readonly int _fieldSize;

    private (int cross, int circle) _scores = (0, 0);

    private readonly Drawer _drawer;
    private readonly Game.TicTacToe _game;

    public GameWindow ()
    {
        _fieldSize = 400;
        _cellSize = 40;

        InitializeComponent();
        _drawer = new(field, SIZE, _fieldSize);
        _game = new(SIZE, _drawer);
    }

    protected override void OnContentRendered (EventArgs e)
    {
        _drawer.DrawGrid();
    }

    private void FieldLeftMouseDown (object sender, MouseButtonEventArgs e)
    {
        if (!_game.CanContinueGame) {
            return;
        }

        // getting mouse position relative to the game field
        var mousePos = Mouse.GetPosition(field);
        if (MouseOutOfBounds(mousePos)) { return; }

        // converting mouse pos into grid point (aka cell)
        Point point = new((int)mousePos.X / _cellSize, (int)mousePos.Y / _cellSize);

        var result = _game.MakeMove(point);

        if (result != GameResult.None) {
            FinishGame(result);
        }
    }

    private void FinishGame (GameResult result)
    {
        endgameText.Text = "Победа!";
        endgameText.Visibility = Visibility.Visible;

        switch (result) {
            case GameResult.CrossWin:
                _scores.cross++;
                crossScoresText.Text = _scores.cross.ToString();
                break;

            case GameResult.CircleWin:
                _scores.circle++;
                circleScoresText.Text = _scores.circle.ToString();
                break;

            case GameResult.Draw:
                endgameText.Text = "Ничья!";
                break;
        }
    }

    private bool MouseOutOfBounds (Point mousePosition)
    {
        if (mousePosition.X < 0 || mousePosition.Y < 0) { return true; }

        if (mousePosition.X > _fieldSize || mousePosition.Y > _fieldSize) { return true; }

        return false;
    }

    private void TitlebarMouseDown (object sender, MouseButtonEventArgs e)
    {
        DragMove();
    }

    private void CloseApp (object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }

    private void ResetScore (object sender, RoutedEventArgs e)
    {
        _scores = (0, 0);

        circleScoresText.Text = _scores.circle.ToString();
        crossScoresText.Text = _scores.cross.ToString();

        StartNewGame(null!, null!);
    }

    private void StartNewGame (object sender, RoutedEventArgs e)
    {
        endgameText.Visibility = Visibility.Collapsed;
        _game.StartNewGame();
    }
}