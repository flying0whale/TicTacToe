using System.Windows;
using Cell = (System.Windows.Point point, TicTacToe.Game.Figure figure);

namespace TicTacToe.Game;

public sealed class TicTacToe
{
    private readonly int _fieldSize;
    private readonly Drawer _drawer;
    private Figure _currentFigure = Figure.Cross;

    private Cell[] _cells;

    private bool _canContinueGame = true;

    public bool CanContinueGame  => _canContinueGame;

    public TicTacToe (int fieldSize, Drawer drawer)
    {
        _fieldSize = fieldSize;
        _drawer = drawer;
        _cells = new Cell[fieldSize * fieldSize];
    }

    public void StartNewGame()
    {
        _drawer.ClearCanvas();
        _drawer.DrawGrid();

        _canContinueGame = true;

        _cells = new Cell[_fieldSize * _fieldSize];
    }

    public GameResult MakeMove(Point point)
    {
        if (!_canContinueGame) { return GameResult.None; }

        // if something is already placed into the cell -> return None
        if (!PointAvailable(point)) { return GameResult.None; }

        PlaceCurrentFigure(point);

        var result = CheckCompletion(point, _currentFigure);

        // switch figure to the opposite one for next move
        _currentFigure = 1 + (Figure)(2 - (int)_currentFigure);

        if (result != GameResult.None) {
            _canContinueGame = false;
        }

        return result;
    }

    private GameResult CheckCompletion (Point point, Figure figure)
    {
        (Point a, Point b) winPoints;

        var vertical = CheckVerticalCompletion(point, figure, out winPoints);
        var horizontal = CheckHorizontalCompletion(point, figure, out winPoints);
        var diagonal = CheckDiagonalCompletion(point, figure, out winPoints);

        if (vertical || horizontal || diagonal) {
            _drawer.DrawEndgameLine(winPoints.a, winPoints.b);

            return _currentFigure switch {
                Figure.Cross => GameResult.CrossWin,
                Figure.Circle => GameResult.CircleWin,
                _ => GameResult.None
            };
        }

        if (_cells.Count(f => f.figure != Figure.None) == _fieldSize * _fieldSize) {
            return GameResult.Draw;
        }

        return GameResult.None;
    }

    private bool CheckVerticalCompletion (Point point, Figure figure, out (Point a, Point b) winPoints)
    {
        var verticalFigures = _cells.Where(f => f.point.X == point.X && f.figure == figure).ToList();

        var win = verticalFigures.Count == _fieldSize;

        if (win) {
            verticalFigures = verticalFigures.OrderBy(f => f.point.X + f.point.Y).ToList();
            winPoints = (verticalFigures[0].point, verticalFigures[^1].point);
        }

        return win;
    }

    private bool CheckHorizontalCompletion (Point point, Figure figure, out (Point a, Point b) winPoints)
    {
        var horizontalFigures = _cells.Where(f => f.point.Y == point.Y && f.figure == figure).ToList();

        var win = horizontalFigures.Count == _fieldSize;

        if (win) {
            horizontalFigures = horizontalFigures.OrderBy(f => f.point.X + f.point.Y).ToList();
            winPoints = (horizontalFigures[0].point, horizontalFigures[^1].point);
        }

        return win;
    }

    private bool CheckDiagonalCompletion (Point point, Figure figure, out (Point a, Point b) winPoints)
    {
        var diagonalFigures = _cells.Where(f => f.point.X == f.point.Y &&
                                                  f.figure == figure);

        if (diagonalFigures.Count() == _fieldSize) {
            var points = diagonalFigures.OrderBy(f => f.point.X + f.point.Y).ToList();
            winPoints = (points[0].point, points[^1].point);
            return true;
        }

        diagonalFigures = _cells.Where(f => f.point.X == _fieldSize - f.point.Y - 1 &&
                                              f.figure == figure);

        if (diagonalFigures.Count() == _fieldSize) {
            var points = diagonalFigures.OrderBy(f => f.point.X + f.point.Y).ToList();
            winPoints = (points[0].point, points[^1].point);
            return true;
        }

        return false;
    }

    private void PlaceCurrentFigure(Point point)
    {
        _drawer.DrawFigure(point, _currentFigure);

        _cells[(int)(point.X + point.Y * _fieldSize)] = (point, _currentFigure);
    }

    private bool PointAvailable (Point point)
    {
        return !_cells.Any(cell => cell.figure != Figure.None && cell.point == point);
    }
}

public enum GameResult
{
    CrossWin,
    CircleWin,
    Draw,
    None
}

public enum Figure
{
    None,
    Cross,
    Circle
}