using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Keyboard_Trenager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<char> symbols = null;
        DispatcherTimer dt = new();
        int chars = 0, fails = 0, chcount = 0;
        bool shift = false;
        Dictionary<Key, string> dict = null;
        public MainWindow()
        {
            InitializeComponent();
            diff_value.Content = slider.Value.ToString("0");
            dt.Interval = new TimeSpan(0, 1, 0);
            dt.Tick += stop_ev;

            dict = new Dictionary<Key, string>
            {
                { Key.Oem6, "]"},
                { Key.Oem4, "["},
                { Key.Oem3, "`"},
                { Key.OemMinus, "-"},
                { Key.OemPlus, "="},
                { Key.Oem5, "\\"},
                { Key.Oem1, ";"},
                { Key.OemQuotes, "'"},
                { Key.OemComma, ","},
                { Key.OemPeriod, "."},
                { Key.OemQuestion, "/"}
            };
        }

        private void stop_ev(object? sender, EventArgs e)
        {
            try
            {
                if (dt.IsEnabled)
                    dt.Stop();
                stop_button.IsEnabled = false;
                start_button.IsEnabled = true;
                speed_label.Content = chars.ToString();
                chars = 0;
                fails = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void slider_ev(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            diff_value.Content = slider.Value.ToString("0");
        }

        private async void check_key(object sender, KeyEventArgs e)
        {
            try
            {
                if (start_button.IsEnabled == false && e.Key != Key.LeftShift && e.Key != Key.RightShift && e.Key != Key.CapsLock)
                {
                    chcount++;
                    if (Keyboard.IsKeyToggled(Key.CapsLock) || shift)
                    {
                        var key = e.Key;
                        if (e.Key <= Key.D0 || e.Key >= Key.D9)
                        {
                            chars++;
                            input_label.Content += key.ToString();
                        }
                    }
                    else
                    {
                        var key = e.Key;
                        chars++;
                        if (e.Key >= Key.D0 && e.Key <= Key.D9)
                        {
                            string digit = e.Key.ToString();
                            char num = digit.Last();
                            input_label.Content += num.ToString();
                        }
                        else
                        {
                            if (dict.ContainsKey(e.Key))
                                input_label.Content += dict[e.Key];
                            else
                                input_label.Content += key.ToString().ToLower();
                        }
                    }
                    if (chcount.ToString() == diff_value.Content.ToString())
                    {
                        chcount = 0;
                        if (input_label.Content.ToString() != target_label.Content.ToString())
                        {
                            fails++;
                            fails_label.Content = fails.ToString();
                        }
                        await Task.Delay(1000);
                        SelectNextChar();
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void shift_checker(object sender, KeyEventArgs e)
        {
            if (start_button.IsEnabled == false && (e.Key == Key.LeftShift || e.Key == Key.RightShift))
            {
                shift = true;
                foreach(UIElement el in main_grid.Children)
                {
                    if (el is Border border)
                    {
                        if (border.Child is Button el2)
                        {
                            Color grey = (Color)ColorConverter.ConvertFromString("#B5B5B5");
                            var color = border.Background as SolidColorBrush;
                            string content = el2.Content.ToString();
                            if (color != null && color.Color != grey && char.IsLetter(content[0]))
                            {
                                content = content.ToUpper();
                                el2.Content = content;
                            }
                        }
                    }
                }

                foreach (UIElement el in main_grid2.Children)
                {
                    if (el is Border border)
                    {
                        if (border.Child is Button el2)
                        {
                            Color grey = (Color)ColorConverter.ConvertFromString("#B5B5B5");
                            var color = border.Background as SolidColorBrush;
                            string content = el2.Content.ToString();
                            if (color != null && color.Color != grey && char.IsLetter(content[0]))
                            {
                                content = content.ToUpper();
                                el2.Content = content;
                            }
                        }
                    }
                }

                foreach (UIElement el in main_grid3.Children)
                {
                    if (el is Border border)
                    {
                        if (border.Child is Button el2)
                        {
                            Color grey = (Color)ColorConverter.ConvertFromString("#B5B5B5");
                            var color = border.Background as SolidColorBrush;
                            string content = el2.Content.ToString();
                            if (color != null && color.Color != grey && char.IsLetter(content[0]))
                            {
                                content = content.ToUpper();
                                el2.Content = content;
                            }
                        }
                    }
                }
            }
        }

        private void SelectNextChar()
        {
            target_label.Content = "";
            input_label.Content = "";
            if (dt.IsEnabled || !stop_button.IsPressed)
            {
                int x = 0;
                while (x.ToString() != diff_value.Content.ToString())
                {
                    int rand = new Random().Next(0, symbols.Count() - 1);
                    if (symbols[rand] == ' ')
                        target_label.Content += "space";
                    else
                        target_label.Content += symbols[rand].ToString();
                    x++;
                }
            }
        }

        private void shift_up(object sender, KeyEventArgs e)
        {
            if (start_button.IsEnabled == false)
            {
                Thread.Sleep(10);
                if (Keyboard.IsKeyUp(Key.LeftShift) && Keyboard.IsKeyUp(Key.RightShift))
                {
                    shift = false;
                    foreach (UIElement el in main_grid.Children)
                    {
                        if (el is Border border)
                        {
                            if (border.Child is Button el2)
                            {
                                Color grey = (Color)ColorConverter.ConvertFromString("#B5B5B5");
                                var color = border.Background as SolidColorBrush;
                                string content = el2.Content.ToString();
                                if (color != null && color.Color != grey && char.IsLetter(content[0]))
                                {
                                    content = content.ToLower();
                                    el2.Content = content;
                                }
                            }
                        }
                    }

                    foreach (UIElement el in main_grid2.Children)
                    {
                        if (el is Border border)
                        {
                            if (border.Child is Button el2)
                            {
                                Color grey = (Color)ColorConverter.ConvertFromString("#B5B5B5");
                                var color = border.Background as SolidColorBrush;
                                string content = el2.Content.ToString();
                                if (color != null && color.Color != grey && char.IsLetter(content[0]))
                                {
                                    content = content.ToLower();
                                    el2.Content = content;
                                }
                            }
                        }
                    }

                    foreach (UIElement el in main_grid3.Children)
                    {
                        if (el is Border border)
                        {
                            if (border.Child is Button el2)
                            {
                                Color grey = (Color)ColorConverter.ConvertFromString("#B5B5B5");
                                var color = border.Background as SolidColorBrush;
                                string content = el2.Content.ToString();
                                if (color != null && color.Color != grey && char.IsLetter(content[0]))
                                {
                                    content = content.ToLower();
                                    el2.Content = content;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void stop_ev(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dt.IsEnabled)
                    dt.Stop();
                stop_button.IsEnabled = false;
                start_button.IsEnabled = true;
                speed_label.Content = chars.ToString();
                chars = 0;
                fails = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void start_ev(object sender, RoutedEventArgs e)
        {
            try
            {
                if (case_sens.IsChecked == false)
                {
                    if (File.Exists("symbols.txt"))
                    {
                        symbols = new();
                        using (StreamReader sr = new("symbols.txt"))
                        {
                            while (!sr.EndOfStream)
                            {
                                char el = (char)sr.Read();
                                symbols.Add(el);
                            }
                        }
                    }
                }

                if (case_sens.IsChecked == true)
                {
                    if (File.Exists("symbolsCase.txt"))
                    {
                        symbols = new();
                        using (StreamReader sr = new("symbolsCase.txt"))
                        {
                            while (!sr.EndOfStream)
                            {
                                char el = (char)sr.Read();
                                symbols.Add(el);
                            }
                        }
                    }
                }


                dt.Start();
                if (speed_label.Content != "")
                {
                    speed_label.Content = "0";
                    chars = 0;
                }
                if (fails_label.Content != "")
                {
                    fails_label.Content = "0";
                    fails = 0;
                }
                start_button.IsEnabled = false;
                stop_button.IsEnabled = true;
                target_label.Content = "";
                input_label.Content = "";
                if (dt.IsEnabled || !stop_button.IsPressed)
                {
                    int x = 0;
                    while (x.ToString() != diff_value.Content.ToString())
                    {
                        int rand = new Random().Next(0, symbols.Count() - 1);
                        if (symbols[rand] == ' ')
                            target_label.Content += "space";
                        else
                            target_label.Content += symbols[rand].ToString();
                        x++;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        
    }
}