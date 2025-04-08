using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace TicTacToe.Game;

public sealed class Drawer
{
    private readonly int _sizeInCells;
    private readonly int _fieldSize;
    private readonly int _cellSize;

    private readonly Canvas _canvas;

    public Drawer (Canvas canvas, int sizeInCells, int fieldSize)
    {
        _canvas = canvas;

        _sizeInCells = sizeInCells;
        _fieldSize = fieldSize;
        _cellSize = fieldSize / sizeInCells;
    }

    public void ClearCanvas()
    {
        _canvas.Children.Clear();
    }

    public void DrawGrid()
    {
        for (int i = 1; i < _sizeInCells; i++) {
            var verticalLine = new Border {
                Width = 1,
                Height = _fieldSize,
                Background = Brushes.DarkGray,
            };

            var horizontalLine = new Border {
                Height = 1,
                Width = _fieldSize,
                Background = Brushes.DarkGray,
            };

            Canvas.SetLeft(verticalLine, i * _cellSize);
            Canvas.SetTop(horizontalLine, i * _cellSize);

            _canvas.Children.Add(horizontalLine);
            _canvas.Children.Add(verticalLine);
        }
    }

    public void DrawEndgameLine(Point a, Point b)
    {
        var halfsize = _cellSize / 2;

        var line = new Line {
            X1 = a.X * _cellSize + halfsize,
            X2 = b.X * _cellSize + halfsize,
            Y1 = a.Y * _cellSize + halfsize,
            Y2 = b.Y * _cellSize + halfsize,
            Stroke = Brushes.Red,
            StrokeThickness = 9
        };

        _canvas.Children.Add(line);
    }

    public void DrawFigure(Point point, Figure figure)
    {
        switch (figure) {
            case Figure.Cross:
                DrawCross(point);
            break;

            case Figure.Circle:
                DrawCircle(point);
            break;
        }
    }

    private void DrawCircle(Point point)
    {
        var halfsize = _cellSize / 2;
        var ellipse = new Ellipse {
            Width = halfsize,
            Height = halfsize,
            Fill = Brushes.LightGreen
        };

        var middle = new Ellipse {
            Width = _cellSize / 4,
            Height = _cellSize / 4,
            Fill = (Brush)new BrushConverter().ConvertFrom("#252628")!
        };

        Canvas.SetLeft(ellipse, point.X * _cellSize + halfsize / 2);
        Canvas.SetTop(ellipse, point.Y * _cellSize + halfsize / 2);

        _canvas.Children.Add(ellipse);

        Canvas.SetLeft(middle, point.X * _cellSize + halfsize - middle.Width / 2);
        Canvas.SetTop(middle, point.Y * _cellSize + halfsize - middle.Height / 2);

        _canvas.Children.Add(middle);
    }

    private void DrawCross(Point point)
    {
        var offset = _cellSize / 6;
        var line1 = new Line {
            Stroke = Brushes.IndianRed,
            StrokeThickness = 3,
            X1 = point.X * _cellSize + offset,
            X2 = (point.X + 1) * _cellSize - offset,
            Y1 = point.Y * _cellSize + offset,
            Y2 = (point.Y + 1) * _cellSize - offset,
        };

        var line2 = new Line {
            Stroke = Brushes.IndianRed,
            StrokeThickness = 3,
            X1 = point.X * _cellSize + offset,
            X2 = (point.X + 1) * _cellSize - offset,
            Y1 = (point.Y + 1) * _cellSize - offset,
            Y2 = point.Y * _cellSize + offset,
        };

        _canvas.Children.Add(line1);
        _canvas.Children.Add(line2);
    }
}