using BierBuddy.Core;
using Material.Icons.WPF;
using Material.Icons;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BierBuddy.UILib
{
    public class ProfilePageRenderer : IPageRenderer
    {
        private Grid _ProfilePanel;
        private Size _MainWindowSize;
        private double _NavBarWidth;
        private Visitor _Visitor;
        private bool _ReadOnly;
        private ProfilePage _ProfilePage;

        private int BigFontSize = 28;
        private int GeneralFontSize = 16;

        private Border _Drinks;
        private Border _Interests;
        private Border _Activities;

        public ProfilePageRenderer(ProfilePage profilePage)
        {
            _ProfilePanel = new ();
            _Visitor = new(0, "temp", "temp", 0);
            _Drinks = new ProfileContentBorder(UIUtils.BabyPoeder);
            _Interests = new ProfileContentBorder(UIUtils.BabyPoeder);
            _Activities = new ProfileContentBorder(UIUtils.BabyPoeder);
            _ProfilePage = profilePage;
        }

        public Grid GetProfilePage(Visitor visitor, bool readOnly)
        {
            _ProfilePanel = new();
            _ProfilePanel.Margin = new Thickness(20);
            _ProfilePanel.Width = _MainWindowSize.Width - _NavBarWidth;
            _ProfilePanel.Height = _MainWindowSize.Height - 130;
            //kolommen voor automatische layout en resizen
            _ProfilePanel.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            _ProfilePanel.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            _Visitor = visitor;
            _ReadOnly = readOnly;
            _Drinks = new ProfileContentBorder(UIUtils.BabyPoeder);
            _Interests = new ProfileContentBorder(UIUtils.BabyPoeder);
            _Activities = new ProfileContentBorder(UIUtils.BabyPoeder);

            StackPanel profile = GetProfile();
            profile.Margin = new Thickness(0, 0, 20, 0);
            Grid.SetColumn(profile, 0);
            _ProfilePanel.Children.Add(profile);

            Border photos = GetPhotos(_Visitor.Photos);
            photos.Margin = new Thickness(0, 0, 60, 0);
            Grid.SetColumn(photos, 1);
            _ProfilePanel.Children.Add(photos);

            return _ProfilePanel;
        }

        private StackPanel GetProfile()
        {
            StackPanel stackPanel = new StackPanel();
            TextBlock pageTitle = new TextBlock { Text = "ACCOUNT", FontSize = BigFontSize, Margin = new Thickness(10, 0, 0, 20), Foreground = UIUtils.BabyPoeder, FontWeight = FontWeights.Bold };
            stackPanel.Children.Add(pageTitle);

            //naam en leeftijd
            Grid nameAndAge = GetNameAndAgeLabels();
            stackPanel.Children.Add(nameAndAge);

            //bio
            Border bio = GetBioLabel();
            stackPanel.Children.Add(bio);

            //drank
            SetDrinks();
            stackPanel.Children.Add(_Drinks);

            //activiteit
            SetActivities();
            stackPanel.Children.Add(_Activities);

            //interesses
            SetInterests();
            stackPanel.Children.Add(_Interests);

            return stackPanel;
        }

        private Grid GetNameAndAgeLabels()
        {
            Grid grid = new ();
            //kolommen en rijen voor layout en automatisch resizen
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(3, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            ProfileContentBorder name = new ProfileContentBorder(_Visitor.Name, UIUtils.BabyPoeder, GeneralFontSize);
            name.ProfileContentLabel.Foreground = Brushes.Black;
            name.Margin = new Thickness(0, 0, 20, 0);
            name.ProfileContentLabel.HorizontalAlignment = HorizontalAlignment.Left;
            name.ProfileContentLabel.Padding = new Thickness(20, 10, 0, 10);
            name.VerticalAlignment = VerticalAlignment.Center;

            ProfileContentBorder age = new ProfileContentBorder(_Visitor.Age.ToString(), new SolidColorBrush(Color.FromRgb(190, 194, 188)), GeneralFontSize);
            age.ProfileContentLabel.Foreground = Brushes.Black;
            age.ProfileContentLabel.Padding = new Thickness(0, 10, 0, 10);
            age.VerticalAlignment = VerticalAlignment.Center;

            Grid.SetColumn(name, 0);
            Grid.SetColumn(age, 1);
            grid.Children.Add(name);
            grid.Children.Add(age);

            grid.Margin = new Thickness(10, 0, 0, 10);

            return grid;
        }

        private ProfileContentBorder GetBioLabel()
        {
            ProfileContentBorder bio = new(UIUtils.BabyPoeder);
            bio.Height = _MainWindowSize.Height / 6;
            bio.Child = new TextBlock { Text = _Visitor.Bio, TextWrapping = TextWrapping.Wrap, FontSize = GeneralFontSize, Foreground = Brushes.Black, VerticalAlignment = VerticalAlignment.Top, HorizontalAlignment = HorizontalAlignment.Left, Padding = new Thickness(20, 10, 0, 10) };

            return bio;
        }

        private void SetDrinks()
        {
            if (_ReadOnly)
            {
                _Drinks = GetItemList(_Visitor.DrinkPreference.ToArray());
            }
            else
            {
                _Drinks = GetCustomItemList(_Drinks, _Visitor.DrinkPreference.ToArray(), _ProfilePage.GetPossiblePreferences("Drinks", _Visitor.DrinkPreference), "Drinks");
            }
        }

        private void SetActivities()
        {
            if (_ReadOnly)
            {
                _Activities = GetItemList(_Visitor.ActivityPreference.ToArray());
            }
            else
            {
                _Activities = GetCustomItemList(_Activities, _Visitor.ActivityPreference.ToArray(), _ProfilePage.GetPossiblePreferences("Activities", _Visitor.ActivityPreference), "Activities");
            }
        }

        private void SetInterests()
        {
            if (_ReadOnly)
            {
                _Interests = GetItemList(_Visitor.Interests.ToArray());
            }
            else
            {
                _Interests = GetCustomItemList(_Interests, _Visitor.Interests.ToArray(), _ProfilePage.GetPossiblePreferences("Interests", _Visitor.Interests), "Interests");
            }
        }

        private Border GetItemList(string[] items)
        {
            ProfileContentBorder border = new ProfileContentBorder(UIUtils.BabyPoeder);
            WrapPanel wrapPanel = new ();
            wrapPanel.Orientation = Orientation.Horizontal;
            wrapPanel.VerticalAlignment = VerticalAlignment.Center;

            //voeg alle items toe aan de wrapPanel
            foreach (string item in items)
            {
                ProfileContentBorder profileContentBorder = new ProfileContentBorder(item, new SolidColorBrush(Color.FromRgb(108, 114, 109)), GeneralFontSize);
                profileContentBorder.VerticalAlignment = VerticalAlignment.Center;
                profileContentBorder.ProfileContentLabel.Padding = new Thickness(20, 10, 20, 10);
                wrapPanel.Children.Add(profileContentBorder);
            }

            border.Child = wrapPanel;
            return border;
        }

        private Border GetCustomItemList(Border border, string[] items, List<string> options, string type)
        {
            WrapPanel? wrapPanel = border.Child as WrapPanel;  // Cast to WrapPanel

            if (wrapPanel == null)
            {
                // If wrapPanel is null (initial case), create a new one
                wrapPanel = new WrapPanel();
                wrapPanel.Orientation = Orientation.Horizontal;
                wrapPanel.VerticalAlignment = VerticalAlignment.Center;
                border.Child = wrapPanel; // Set the child of the border only if it was null
            }
            else
            {
                // Clear existing items if the wrapPanel already exists
                wrapPanel.Children.Clear();
            }

            //voeg alle items toe aan de wrapPanel
            foreach (string item in items)
            {
                ProfileContentBorder profileContentBorder = new ProfileContentBorder(item, new SolidColorBrush(Color.FromRgb(108, 114, 109)), GeneralFontSize);
                profileContentBorder.VerticalAlignment = VerticalAlignment.Center;
                profileContentBorder.ProfileContentLabel.Padding = new Thickness(20, 10, 20, 10);
                StackPanel stackPanel = new StackPanel();
                stackPanel.Tag = type;
                stackPanel.Orientation = Orientation.Horizontal;
                profileContentBorder.Child = stackPanel;
                stackPanel.Children.Add(profileContentBorder.ProfileContentLabel);
                if (items.Length > 1)
                {
                    Button delete = GetDeleteButton(20, 20);
                    stackPanel.Children.Add(delete);
                }
                wrapPanel.Children.Add(profileContentBorder);
            }

            if (items.Length < 4)
            {
                ProfileContentBorder profileContentBorder = new ProfileContentBorder(UIUtils.Transparent);
                profileContentBorder.Child = GetAddButton(20, 20, options);
                profileContentBorder.Tag = type;
                wrapPanel.Children.Add(profileContentBorder);
            }

            border.Child = wrapPanel;
            return border;
        }

        private Border GetPhotos(List<string> photos)
        {
            Border BGborder = new ProfileContentBorder(UIUtils.Outer_Space);
            Grid grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            for (int i = 0; i < photos.Count; i++) {
                string photo = photos[i];
                if (i < 2) // 2 foto's per rij
                {
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                }
                else if (i == 2) // 2 kolommen
                {
                    grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
                }
                if (!photo.Equals("Geen URL gevonden")) // als er echt een foto is
                {
                    Border border = GetPhoto(photo);
                    Grid.SetColumn(border, i % 2);
                    Grid.SetRow(border, i / 2);
                    grid.Children.Add(border);
                }
            }

            BGborder.Child = grid;

            return BGborder;
        }

        private Border GetPhoto(string photo)
        {
            ProfileContentBorder profileContentBorder = new ProfileContentBorder(UIUtils.Onyx70);
            Image image = new Image();
            image.Source = new BitmapImage(new Uri(photo));
            //afbeelding centreren in de border
            image.HorizontalAlignment = HorizontalAlignment.Center;
            image.VerticalAlignment = VerticalAlignment.Center;

            //afbeelding clippen zodat het in de border past, zelfs na het resizen
            profileContentBorder.SizeChanged += (s, e) =>
            {
                double borderWidth = profileContentBorder.ActualWidth;
                double borderHeight = profileContentBorder.ActualHeight;

                profileContentBorder.Clip = new RectangleGeometry(
                    new Rect(0, 0, borderWidth, borderHeight),
                    profileContentBorder.CornerRadius.TopLeft,
                    profileContentBorder.CornerRadius.TopLeft
                );
            };

            profileContentBorder.Child = image;
            return profileContentBorder;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if(sender is Button button)
            {
                if(button.Parent is StackPanel stackPanel)
                {
                    if (stackPanel.Children[0] is Label label)
                    {
                        if (label.Content is string String)
                        {
                            if (stackPanel.Tag.Equals("Drinks"))
                            {
                                _Visitor.RemoveFromDrinkPreference(String);
                                SetDrinks();
                            }
                            else if (stackPanel.Tag.Equals("Interests"))
                            {
                                _Visitor.RemoveFromInterests(String);
                                SetInterests();
                            }
                            else if (stackPanel.Tag.Equals("Activities"))
                            {
                                _Visitor.RemoveFromActivityPreference(String);
                                SetActivities();
                            }
                            _ProfilePage.UpdateProfile(_Visitor);
                        }
                    }
                }
            }
        }

        private void AddButton_Click(object sender, List<string> options)
        {
            if (sender is Button button)
            {
                if (button.Parent is ProfileContentBorder border)
                {
                    border.Background = UIUtils.BabyPoeder;
                    CustomComboBox customComboBox = new CustomComboBox();
                    customComboBox.ItemsSource = options;
                    customComboBox.IsTextSearchEnabled = true;
                    customComboBox.VerticalAlignment = VerticalAlignment.Center;
                    customComboBox.HorizontalAlignment = HorizontalAlignment.Center;
                    if (border.Tag.Equals("Drinks"))
                    {
                        customComboBox.SelectionChanged += (s, e) =>
                        {
                            if (customComboBox.SelectedItem is string selected)
                            {
                                _Visitor.AddToDrinkPreference(selected);
                                SetDrinks();
                                _ProfilePage.UpdateProfile(_Visitor);
                            }
                        };
                    }
                    else if (border.Tag.Equals("Interests"))
                    {
                        customComboBox.SelectionChanged += (s, e) =>
                        {
                            if (customComboBox.SelectedItem is string selected)
                            {
                                _Visitor.AddToInterests(selected);
                                SetInterests();
                                _ProfilePage.UpdateProfile(_Visitor);
                            }
                        };
                    }
                    else if (border.Tag.Equals("Activities"))
                    {
                        customComboBox.SelectionChanged += (s, e) =>
                        {
                            if (customComboBox.SelectedItem is string selected)
                            {
                                _Visitor.AddToActivityPreference(selected);
                                SetActivities();
                                _ProfilePage.UpdateProfile(_Visitor);
                            }
                        };
                    }
                    border.Child = customComboBox;
                }
            }
        }

        private Button GetDeleteButton(double width, double height)
        {
            Button deleteButton = new();
            deleteButton.Template = GetIconButtonTemplate(UIUtils.DeclineRed);
            deleteButton.Click += DeleteButton_Click;

            return GenerateIconButton(deleteButton, width, height, MaterialIconKind.Close, "delete");
        }

        private Button GetAddButton(double width, double height, List<string> options)
        {
            Button addButton = new();
            addButton.Template = GetIconButtonTemplate(UIUtils.AcceptGreen);
            addButton.Click += (s, e) => AddButton_Click(s, options);

            return GenerateIconButton(addButton, width, height, MaterialIconKind.Add, "add");

        }

        /// <summary>
        /// zet alle standaard waardes voor de accept en decline buttons
        /// </summary>
        private Button GenerateIconButton(Button button, double width, double height, MaterialIconKind iconKind, string name)
        {
            MaterialIcon icon = new MaterialIcon();
            icon.Kind = iconKind;
            icon.Foreground = UIUtils.BabyPoeder;
            button.Width = width;
            button.Height = height;
            button.Content = icon;
            button.VerticalAlignment = VerticalAlignment.Center;
            button.HorizontalAlignment = HorizontalAlignment.Center;
            button.Background = UIUtils.Transparent;
            button.Margin = new Thickness(0, 0, 5, 0);
            button.Name = name;

            return button;
        }

        private ControlTemplate GetIconButtonTemplate(Brush brush)
        {
            ControlTemplate template = new ControlTemplate(typeof(Button));
            FrameworkElementFactory gridFactory = new FrameworkElementFactory(typeof(Grid));

            //achtergrond
            FrameworkElementFactory borderFactory = new FrameworkElementFactory(typeof(Border));
            borderFactory.SetValue(Border.BackgroundProperty, brush);
            borderFactory.SetValue(Border.CornerRadiusProperty, new CornerRadius(90));
            gridFactory.AppendChild(borderFactory);

            //voorgrond
            FrameworkElementFactory contentPresenterFactory = new FrameworkElementFactory(typeof(ContentPresenter));
            contentPresenterFactory.SetValue(ContentPresenter.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            contentPresenterFactory.SetValue(ContentPresenter.VerticalAlignmentProperty, VerticalAlignment.Center);
            gridFactory.AppendChild(contentPresenterFactory);

            template.VisualTree = gridFactory;
            return template;
        }

        public void UpdatePageSize(double newNavBarWidth, Size newScreenSize)
        {
            _NavBarWidth = newNavBarWidth;
            _MainWindowSize = newScreenSize;
            _ProfilePanel.Width = _MainWindowSize.Width - _NavBarWidth;
            _ProfilePanel.Height = _MainWindowSize.Height - 80;
        }
    }
}
