using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Snake
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer;
        Random random;
        Point snake;
        Point food;
        int stepX, stepY, width, height;
        int size = 20;
        int total = 100;
        int length = 1;
        int points = 0;

        public MainWindow()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            width = (int)main.ActualWidth;
            height = (int)main.ActualHeight;
            random = new Random();
            timer = new DispatcherTimer();
            timer.Tick += GameRun;
            timer.Interval = new TimeSpan(10000);
            timer.IsEnabled = true;

            addFood();

            snake = new Point(width / 2, height / 2);
            stepX = 0;
            stepY = 0;
            MoveSnake();
        }

        private void MoveSnake()
        {
            snake.X += stepX * 3;
            snake.Y += stepY * 3;
            Ellipse ellipse = createFoodOrSnake(snake, Brushes.Red);
            if(main.Children.Count > length)
            {
                main.Children.RemoveAt(length);
            }
            main.Children.Insert(1, ellipse);
        }
        private void GameRun(object sender, EventArgs e)
        {
            MoveSnake();
            if (outOfScreen(snake))
            {
                MessageBox.Show("Game over :(", "Loser");
                Close();
            }
            else if (isEaten(snake, food))
            {
                if (++points == total)
                {
                    main.Children.RemoveAt(0);
                    MessageBox.Show("You win", "Winner");
                    Close();
                }
                else
                {
                    addFood();
                }
            }
        }
        private void keyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                    {
                        stepX = 0;
                        stepY = -1;
                        break;
                    }
                case Key.Down:
                    {
                        stepX = 0;
                        stepY = +1;
                        break;
                    }
                case Key.Left:
                    {
                        stepX = -1;
                        stepY = 0;
                        break;
                    }
                case Key.Right:
                    {
                        stepX = +1;
                        stepY = 0;
                        break;
                    }
            }
        }
        private Ellipse createFoodOrSnake(Point point, Brush brush)
        {
            Ellipse ellipse = new Ellipse();
            ellipse.Width = size;
            ellipse.Height = size;
            ellipse.Fill = brush;
            Canvas.SetLeft(ellipse, point.X);
            Canvas.SetTop(ellipse, point.Y);
            return ellipse;
        }
        private void addFood()
        {
            food = new Point(random.Next(width - size), random.Next(height - size));
            Ellipse ellipse = createFoodOrSnake(food, Brushes.White);
            if (main.Children.Count > 0)
            {
                main.Children.RemoveAt(0);
            }
            main.Children.Insert(0, ellipse);
            length++;
        }
        private bool isEaten(Point a, Point b)
        {
            return (Math.Abs(a.X - b.X)) < size && (Math.Abs(a.Y - b.Y)) < size;
        }
        private bool outOfScreen(Point point)
        {
            return point.X < 0 || point.Y < 0 || point.X > width - size || point.Y > height - size;
        }
    }
}
