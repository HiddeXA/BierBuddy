using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Xml;
using BierBuddy.Core;
using Material.Icons;
using Material.Icons.WPF;

//TODO:
//buttons like dislike
//show picture
//opmaak profile bio and all interests 
//dubbelle code weg halen

namespace BierBuddy.UILib
{
    public class FindBuddiesPageRenderer : IPageRenderer
    {
        private Canvas profilePanel;
        private Size MainWindowSize;
        private double NavBarWidth;
        private Visitor _Visitor { get; set; }

        public FindBuddiesPageRenderer()
        {
            profilePanel = new Canvas();
            _Visitor = new(0, "temp", "temp", 0);
        }
        public WrapPanel GetFindBuddiesPage(Visitor visitor, double navBarWidth, double screenWidth, double screenHeight)
        {
            profilePanel = new();
            profilePanel.Margin = new Thickness(20);
            profilePanel.VerticalAlignment = VerticalAlignment.Center;
            _Visitor = visitor;

            WrapPanel FindBuddiesPanel = new();
            double panelWidth = screenWidth - navBarWidth;

            FindBuddiesPanel.Children.Add(GetDislikeButton(panelWidth / 4 , screenHeight));
            FindBuddiesPanel.Children.Add(GetProfileBorder());
            SetProfilePanel(panelWidth / 2, screenHeight);
            FindBuddiesPanel.Children.Add(GetlikeButton(panelWidth / 4 , screenHeight));

            FindBuddiesPanel.VerticalAlignment = VerticalAlignment.Center;
            FindBuddiesPanel.HorizontalAlignment = HorizontalAlignment.Center;

            return FindBuddiesPanel;
        }

        private UIElement GetDislikeButton(double width, double height) 
        {
            DockPanel dislikeButtonPanel = new();
            dislikeButtonPanel.Width = width;
            dislikeButtonPanel.Height = height;

            Button dislikeButton = new();
            dislikeButton.Template = GetLikeButtonTemplate(new SolidColorBrush(Color.FromArgb(0xFF, 0xBE, 0x37, 0x32)));
            MaterialIcon icon = new MaterialIcon();
            icon.Kind = MaterialIconKind.GlassMugOff;
            icon.Foreground = UIUtils.BabyPoeder;
            dislikeButton.Content = icon;

            dislikeButton.Width = width / 3;
            dislikeButton.VerticalAlignment = VerticalAlignment.Center;
            dislikeButton.HorizontalAlignment = HorizontalAlignment.Center;
            dislikeButton.Background = UIUtils.Transparent;
            dislikeButton.Click += DislikeButton_Click;

            dislikeButtonPanel.Children.Add(dislikeButton);

            return dislikeButtonPanel;
        }

        private void DislikeButton_Click(object sender, RoutedEventArgs e)
        {
            //TODO dislike stuff
        }

        private UIElement GetlikeButton(double width, double height)
        {
            DockPanel likeButtonPanel = new();
            likeButtonPanel.Width = width;
            likeButtonPanel.Height = height;

            Button likeButton = new();
            likeButton.Template = GetLikeButtonTemplate(new SolidColorBrush(Color.FromArgb(0xFF, 0x7E, 0xA1, 0x72)));
           
            MaterialIcon icon = new MaterialIcon();
            icon.Kind = MaterialIconKind.GlassMug;
            icon.Foreground = UIUtils.BabyPoeder;
            likeButton.Content = icon;
            likeButton.Width = width / 3;
            likeButton.VerticalAlignment = VerticalAlignment.Center;
            likeButton.HorizontalAlignment = HorizontalAlignment.Center;
            likeButton.Background = UIUtils.Transparent;
            likeButton.Click += LikeButton_Click;

            likeButtonPanel.Children.Add(likeButton);
            return likeButtonPanel;
        }

        private void LikeButton_Click(object sender, RoutedEventArgs e)
        {
            //TODO like stuff
        }

        private ControlTemplate GetLikeButtonTemplate(Brush brush)
        {
            ControlTemplate template = new ControlTemplate(typeof(Button));
            FrameworkElementFactory gridFactory = new FrameworkElementFactory(typeof(Grid));

            FrameworkElementFactory borderFactory = new FrameworkElementFactory(typeof(Border));
            //set background color
            borderFactory.SetValue(Border.BackgroundProperty, brush);
            borderFactory.SetValue(Border.CornerRadiusProperty, new CornerRadius(90));
            gridFactory.AppendChild(borderFactory);

            FrameworkElementFactory contentPresenterFactory = new FrameworkElementFactory(typeof(ContentPresenter));
            contentPresenterFactory.SetValue(ContentPresenter.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            contentPresenterFactory.SetValue(ContentPresenter.VerticalAlignmentProperty, VerticalAlignment.Center);
            gridFactory.AppendChild(contentPresenterFactory);

            template.VisualTree = gridFactory;

            return template;
        }
        private UIElement GetProfileBorder()
        {
            Border profileBorder = new();
            profileBorder.Background = UIUtils.Outer_Space;
            profileBorder.CornerRadius = UIUtils.UniversalCornerRadius;
            profileBorder.Child = profilePanel;
            return profileBorder;
        }
        private void SetProfilePanel(double width, double height)
        {
            profilePanel.Width = width;
            profilePanel.Height = height - 150;
            UIElement profilePicture = GetProfilePicture(width);
            profilePanel.Children.Add(profilePicture);

            UIElement profileContent = GetProfileContentPanel(width);
            Canvas.SetTop(profileContent, height - UIUtils.ProfileConentHeight - 150);
            profilePanel.Children.Add(profileContent);
        }
        
        private UIElement GetProfilePicture(double width)
        {
            Label tempFoto = new Label();
            tempFoto.Content = "FOTO!";
            tempFoto.Foreground = UIUtils.testMarking;
            return  tempFoto;
        }

        private UIElement GetProfileContentPanel(double width)
        {
            // Create the Grid
            Grid profileGrid = new Grid();
            profileGrid.Width = width;
            profileGrid.Height = UIUtils.ProfileConentHeight;
            profileGrid.HorizontalAlignment = HorizontalAlignment.Left;
            profileGrid.VerticalAlignment = VerticalAlignment.Top;

            // Define the Columns
            ColumnDefinition colDef1 = new ColumnDefinition();
            ColumnDefinition colDef2 = new ColumnDefinition();
            ColumnDefinition colDef3 = new ColumnDefinition();
            profileGrid.ColumnDefinitions.Add(colDef1);
            profileGrid.ColumnDefinitions.Add(colDef2);
            profileGrid.ColumnDefinitions.Add(colDef3);

            // Define the Rows
            RowDefinition rowDef1 = new RowDefinition();
            RowDefinition rowDef2 = new RowDefinition();
            RowDefinition rowDef3 = new RowDefinition();
            RowDefinition rowDef4 = new RowDefinition();
            rowDef4.Height = new GridLength(UIUtils.ProfileConentHeight / 6);
            profileGrid.RowDefinitions.Add(rowDef1);
            profileGrid.RowDefinitions.Add(rowDef2);
            profileGrid.RowDefinitions.Add(rowDef3);
            profileGrid.RowDefinitions.Add(rowDef4);

            UIElement ProfileBanner = GetProfileBanner();
            Grid.SetColumnSpan(ProfileBanner, 3);
            Grid.SetRow(ProfileBanner, 1);

            UIElement drinkLabel;
            UIElement activityLabel;
            UIElement interestLabel;

            if (_Visitor.DrinkPreference.Count != 0)
            {
                drinkLabel = GetProfileLabel(_Visitor.DrinkPreference[0]);
            }
            else
            {
                drinkLabel = GetProfileLabel("Heb ik niet");
            }
            if (_Visitor.ActivityPreference.Count != 0)
            {
                activityLabel = GetProfileLabel(_Visitor.ActivityPreference[0]);
            }
            else
            {
                activityLabel = GetProfileLabel("Heb ik niet");
            }
            if (_Visitor.DrinkPreference.Count != 0)
            {
                interestLabel = GetProfileLabel(_Visitor.Interests[0]);
            }
            else
            {
                interestLabel = GetProfileLabel("Heb ik niet");
            };
            
            Grid.SetColumn(drinkLabel, 0);
            Grid.SetRow(drinkLabel, 2);

            Grid.SetColumn(activityLabel, 1);
            Grid.SetRow(activityLabel, 2);

            Grid.SetColumn(interestLabel, 2);
            Grid.SetRow(interestLabel, 2);

            UIElement bioButton = GetBioButton();
            Grid.SetColumn(bioButton, 1);
            Grid.SetRow(bioButton, 3);

            profileGrid.Children.Add(ProfileBanner);
            profileGrid.Children.Add(drinkLabel);
            profileGrid.Children.Add(activityLabel);
            profileGrid.Children.Add(interestLabel);
            profileGrid.Children.Add(bioButton);

            Border profileContentBorder = new Border();
            //gradient toevoegen
            LinearGradientBrush gradientBrush = new LinearGradientBrush
            {
                StartPoint = new Point(0, 0), // Bovenkant
                EndPoint = new Point(0, 1)   // Onderkant
            };
            gradientBrush.GradientStops.Add(new GradientStop(Colors.Transparent, 0));
            gradientBrush.GradientStops.Add(new GradientStop(Color.FromArgb(0xFF, 0xFC, 0xFF, 0xF7), 1));
            profileContentBorder.Background = gradientBrush;
            profileContentBorder.CornerRadius = new CornerRadius(0, 0, 40, 40);

            profileContentBorder.Child = profileGrid;

            return profileContentBorder;
        }
        private UIElement GetProfileBanner()
        {
            WrapPanel bannerPanel = new WrapPanel();

            bannerPanel.Children.Add(GetNameLabel());
            bannerPanel.Children.Add(GetProfileBannerDot());
            bannerPanel.Children.Add(GetAgeLabel());

            bannerPanel.HorizontalAlignment = HorizontalAlignment.Center;
            bannerPanel.VerticalAlignment = VerticalAlignment.Center;

            ProfileContentBorder contentBannerBorder = new ProfileContentBorder();
            contentBannerBorder.Child = bannerPanel;
            return contentBannerBorder;
        }
        private UIElement GetNameLabel()
        {
            ProfileContentLabel nameLabel = new ProfileContentLabel(_Visitor.Name);
            return nameLabel;
        }
        private UIElement GetAgeLabel()
        {
            ProfileContentLabel ageLabel = new ProfileContentLabel(_Visitor.Age.ToString());
            return ageLabel;
        }
        private UIElement GetProfileBannerDot()
        {
            MaterialIcon icon = new MaterialIcon();
            icon.Kind = MaterialIconKind.Dot;
            ProfileContentLabel nameLabel = new ProfileContentLabel();
            nameLabel.Content = icon;
            nameLabel.FontSize = 56;
            return nameLabel;
        }
        private UIElement GetProfileLabel(string content)
        {
            ProfileContentBorder profileLabel = new ProfileContentBorder(content);
            return profileLabel;
        }
        private UIElement GetBioButton()
        {
            Button bioButton = new Button();
            bioButton.Template = GetBioButtonTemplate();

            MaterialIcon icon = new MaterialIcon();
            icon.Kind = MaterialIconKind.ChevronDown;
            icon.Foreground = UIUtils.BabyPoeder;
            bioButton.Content = icon;
            bioButton.Click += BioButton_Click;

            bioButton.HorizontalAlignment = HorizontalAlignment.Center;
            return bioButton;
        }
        private void BioButton_Click(object sender, RoutedEventArgs e)
        {
            profilePanel.Children.Clear();
            SetPreferencesPanel();
            
        }
        private void SetPreferencesPanel()
        {
            profilePanel.Children.Add(GetPreferencesTable());
            profilePanel.Children.Add(GetBio());
        }
        private UIElement GetPreferencesTable()
        {
            Grid profileGrid = new Grid();
            profileGrid.Width = profilePanel.Width;
            profileGrid.Height = UIUtils.ProfileConentHeight;
            profileGrid.HorizontalAlignment = HorizontalAlignment.Left;
            profileGrid.VerticalAlignment = VerticalAlignment.Top;
            profileGrid.ShowGridLines = true;

            // Define the Columns
            ColumnDefinition colDef1 = new ColumnDefinition();
            ColumnDefinition colDef2 = new ColumnDefinition();
            ColumnDefinition colDef3 = new ColumnDefinition();
            profileGrid.ColumnDefinitions.Add(colDef1);
            profileGrid.ColumnDefinitions.Add(colDef2);
            profileGrid.ColumnDefinitions.Add(colDef3);

            // Define the Rows
            RowDefinition rowDef1 = new RowDefinition();
            RowDefinition rowDef2 = new RowDefinition();
            RowDefinition rowDef3 = new RowDefinition();
            RowDefinition rowDef4 = new RowDefinition();
            RowDefinition rowDef5 = new RowDefinition();
            profileGrid.RowDefinitions.Add(rowDef1);
            profileGrid.RowDefinitions.Add(rowDef2);
            profileGrid.RowDefinitions.Add(rowDef3);
            profileGrid.RowDefinitions.Add(rowDef4);
            profileGrid.RowDefinitions.Add(rowDef5);

            UIElement backButton = GetBioBackButton();
            Grid.SetColumn(backButton, 1);
            Grid.SetRow(backButton, 0);
            profileGrid.Children.Add(backButton);

            for (int i = 0; i < _Visitor.DrinkPreference.Count; i++)
            {
                ProfileContentBorder drinkPref = new(_Visitor.DrinkPreference[i]);
                Grid.SetColumn(drinkPref, 0);
                Grid.SetRow(drinkPref, i + 1);
                profileGrid.Children.Add(drinkPref);
            }
            for (int i = 0; i < _Visitor.ActivityPreference.Count; i++)
            {
                ProfileContentBorder activityPref = new(_Visitor.ActivityPreference[i]);
                Grid.SetColumn(activityPref, 1);
                Grid.SetRow(activityPref, i + 1);
                profileGrid.Children.Add(activityPref);
            }
            for (int i = 0; i < _Visitor.Interests.Count; i++)
            {
                ProfileContentBorder interests = new(_Visitor.Interests[i]);
                Grid.SetColumn(interests, 2);
                Grid.SetRow(interests, i + 1);
                profileGrid.Children.Add(interests);
            }

            return profileGrid;
        }
        private UIElement GetBioBackButton()
        {
            Button bioButton = new Button();
            bioButton.Template = GetBioButtonTemplate();

            MaterialIcon icon = new MaterialIcon();
            icon.Kind = MaterialIconKind.ChevronUp;
            icon.Foreground = UIUtils.BabyPoeder;
            bioButton.Content = icon;
            bioButton.Click += BioBackButton_Click;

            bioButton.HorizontalAlignment = HorizontalAlignment.Center;
            return bioButton;
        }
        private void BioBackButton_Click(object sender, RoutedEventArgs e)
        {
            profilePanel.Children.Clear();
            SetProfilePanel((MainWindowSize.Width - NavBarWidth)/2, MainWindowSize.Height);
        }
        private ControlTemplate GetBioButtonTemplate()
        {
            ControlTemplate template = new ControlTemplate(typeof(Button));
            FrameworkElementFactory gridFactory = new FrameworkElementFactory(typeof(Grid));

            FrameworkElementFactory borderFactory = new FrameworkElementFactory(typeof(Border));
            //set background color
            borderFactory.SetValue(Border.BackgroundProperty, UIUtils.Onyx70);
            borderFactory.SetValue(Border.CornerRadiusProperty, new CornerRadius(90));
            gridFactory.AppendChild(borderFactory);

            FrameworkElementFactory contentPresenterFactory = new FrameworkElementFactory(typeof(ContentPresenter));
            contentPresenterFactory.SetValue(ContentPresenter.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            contentPresenterFactory.SetValue(ContentPresenter.VerticalAlignmentProperty, VerticalAlignment.Center);
            gridFactory.AppendChild(contentPresenterFactory);

            template.VisualTree = gridFactory;
            return template;
        }
        private UIElement GetBio()
        {
            TextBlock bio = new();
            bio.Text = _Visitor.Bio;
            return bio;
        }
        public void UpdatePageSize(double newNavBarWidth, Size newScreenSize)
        {
            NavBarWidth = newNavBarWidth;
            MainWindowSize = newScreenSize;
        }
    }

    internal class ProfileContentBorder : Border
    {
        public Label? ProfileContentLabel { get; set; }

        public ProfileContentBorder(string content) : this()
        {
            ProfileContentLabel = new ProfileContentLabel(content);
            this.Child = ProfileContentLabel;
        }
        public ProfileContentBorder()
        {
            this.Background = UIUtils.Onyx70;
            this.CornerRadius = UIUtils.UniversalCornerRadius;
            this.Margin = new Thickness(10);
        }
    }
    internal class ProfileContentLabel : Label
    {
        public ProfileContentLabel(string content) : this()
        {
            this.Content = content;
        }
        public ProfileContentLabel()
        {
            this.Foreground = UIUtils.BabyPoeder;
            this.VerticalAlignment = VerticalAlignment.Center;
            this.HorizontalAlignment = HorizontalAlignment.Center;
            this.FontFamily = UIUtils.ÜniversalFontFamily;
            this.FontSize = 30;
        }
    }
}
