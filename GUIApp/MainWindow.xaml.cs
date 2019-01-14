using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ViewModels;

namespace GUIApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private WalkerViewModel walkerViewModel;
        private Dictionary<string, Polygon> cells;

        public MainWindow()
        {
            InitializeComponent();

            InitData();
            honeycombPanel.Loaded += HoneycombPanel_Loaded;
        }

        private void HoneycombPanel_Loaded(object sender, RoutedEventArgs e)
        {
            DrawHoneycomb();
        }

        private void InitData()
        {
            cells = new Dictionary<string, Polygon>();
            walkerViewModel = new WalkerViewModel();
            DataContext = walkerViewModel;
            ListProgress.ItemsSource = walkerViewModel.ProgressMessages;
            walkerViewModel.ProgressMessages.Add("Application started");
        }

        private void DrawHoneycomb()
        {
            double width = honeycombPanel.ActualWidth;
            double height = honeycombPanel.ActualHeight;

            int cols = walkerViewModel.Honeycomb.Right - walkerViewModel.Honeycomb.Left + 1;
            int rows = walkerViewModel.Honeycomb.Top - walkerViewModel.Honeycomb.Bottom + 1;

            double cellSize = CellSize(width, height, rows, cols);

            walkerViewModel.Honeycomb.ForEachCell(cell =>
            {
                AddCell(cell, cols, rows, width, height, cellSize);
            });
        }

        private Polygon AddCell(Honeycomb.Cell<long>cell,int cols,int rows,double width,double height,double cellSize)
        {
            Point pt = ConvertCellCoords(cell, cols, rows, width, height, cellSize);
            var hexagon = CreateHexagon(pt, cellSize);
            honeycombPanel.Children.Add(hexagon);

            cells[cell.Key] = hexagon;
            return hexagon;
        }
        

        private Point ConvertCellCoords(Honeycomb.Cell<long> cell, int cols,int rows,double width,double height,double cellSize)
        {
            var pt = new Point();

            // re-origin to top-left
            int x = cell.Column - walkerViewModel.Honeycomb.Left;
            int y = walkerViewModel.Honeycomb.Top - cell.Row;

            // offsets from top and left
            double xOffset = cellSize*2;
            double yOffset = cellSize;

            // axis factors
            double xFactor = Math.Min((width - (2 * xOffset)) / cols, (height - (2 * yOffset)) / rows);
            double yFactor = xFactor;

            //
            pt.X = (x * xFactor * 1.5) + xOffset;
            pt.Y = (y * (yFactor/1.25)) + yOffset;

            return pt;
        }

        private double CellSize(double panelWidth,double panelHeight,int rows,int cols)
        {
            return Math.Min(panelWidth / rows, panelHeight / cols)/2;
        }

        private Polygon CreateHexagon(Point origin, double size)
        {
            var hexagon = new Polygon();

            double root3Div2 = Math.Sqrt(3)/2;
            double sizeDiv2 = size / 2;

            var A = new Point(origin.X - sizeDiv2, origin.Y - (root3Div2 * size));
            var B = new Point(origin.X + sizeDiv2, origin.Y - (root3Div2 * size));
            var C = new Point(origin.X + size, origin.Y);
            var D = new Point(origin.X + sizeDiv2, origin.Y + (root3Div2 * size));
            var E = new Point(origin.X - sizeDiv2, origin.Y + (root3Div2 * size));
            var F = new Point(origin.X - size, origin.Y);

            hexagon.Points.Add(A);
            hexagon.Points.Add(B);
            hexagon.Points.Add(C);
            hexagon.Points.Add(D);
            hexagon.Points.Add(E);
            hexagon.Points.Add(F);

            hexagon.Stroke = Brushes.Black;

            return hexagon;
        }

        private async void GoButton_Click(object sender, RoutedEventArgs e)
        {
            await (DataContext as WalkerViewModel).WalkAsync();

            string key = Honeycomb.Cell<long>.CellKey(walkerViewModel.Column, walkerViewModel.Row);
            Polygon mostLikely = cells[key];
            mostLikely.Fill = Brushes.Green;
        }
    }
}
