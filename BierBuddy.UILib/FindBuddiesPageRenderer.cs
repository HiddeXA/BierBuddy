﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Cache;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Imaging;
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
        private Canvas _profilePanel;
        private WrapPanel _FindBuddiesPanel;
        private Size _MainWindowSize;
        private double _NavBarWidth;
        private int _CurrentPhotoIndex = 0;
        private Image _ProfilePicture;
        private bool _ViewingPreferences = false;
        private Visitor? _Visitor { get; set; }
        private FindBuddies _FindBuddies;

        private int BigFontSize = 28;
        private int GeneralFontSize = 16;

        private readonly int _MinFontSizeBig = 20;
        private readonly int _MinFontSizeGeneral = 12;

        public FindBuddiesPageRenderer(FindBuddies findBuddies){
            _profilePanel = new Canvas();
            _FindBuddiesPanel = new();
            _ProfilePicture = new Image();
            _FindBuddies = findBuddies;
            _FindBuddies.Main.AccountSwitcher.OnClientProfileChanged += OnClientProfileChanged;
        }
        public WrapPanel GetFindBuddiesPage(Visitor? visitor)
        {
            _profilePanel.Margin = new Thickness(20);
            _profilePanel.VerticalAlignment = VerticalAlignment.Center;
            if (_Visitor == null || visitor == null || _Visitor.ID != visitor.ID)
            {
                _ViewingPreferences = false;
            }
            _Visitor = visitor;

            if (_ViewingPreferences)
            {
                SetFindBuddiesPanel();
                _profilePanel.Children.Clear();
                SetPreferencesPanel();
            }
            else
            {
                SetFindBuddiesPanel();
            }
            return _FindBuddiesPanel;
        }
        private void SetFindBuddiesPanel()
        {
            _profilePanel = new();
            double panelWidth = _MainWindowSize.Width - _NavBarWidth;
            if (_Visitor != null)
            {
                _FindBuddiesPanel.Children.Clear();
                _FindBuddiesPanel.Children.Add(GetDislikeButton(panelWidth / 4, _MainWindowSize.Height));
                _FindBuddiesPanel.Children.Add(GetProfileBorder());
                SetProfilePanel(panelWidth / 2, _MainWindowSize.Height);
                _FindBuddiesPanel.Children.Add(GetlikeButton(panelWidth / 4, _MainWindowSize.Height));
            }
            else
            {
                _FindBuddiesPanel.Children.Clear();
                _FindBuddiesPanel.Children.Add(GetFillerBlock(panelWidth / 4));
                _FindBuddiesPanel.Children.Add(GetProfileNotFound(panelWidth / 2 , _MainWindowSize.Height));
            }

            _FindBuddiesPanel.VerticalAlignment = VerticalAlignment.Center;
            _FindBuddiesPanel.HorizontalAlignment = HorizontalAlignment.Center;
        }

        private UIElement GetProfileNotFound(double width, double windowHight)
        {
            Border profileNotFoundBorder = new Border();

            profileNotFoundBorder.Background = UIUtils.Outer_Space;
            profileNotFoundBorder.CornerRadius = UIUtils.UniversalCornerRadius;
            profileNotFoundBorder.Margin = new Thickness(30);

            StackPanel profileNotFoundPanel = new StackPanel();
            profileNotFoundPanel.Width = width;
            profileNotFoundPanel.Height = windowHight - 90;
            profileNotFoundPanel.HorizontalAlignment = HorizontalAlignment.Center;

            MaterialIcon icon = new MaterialIcon();
            icon.Kind = MaterialIconKind.GlassMugOff;
            icon.Foreground = UIUtils.BeerYellow;
            icon.Width = width / 4;
            icon.HorizontalAlignment = HorizontalAlignment.Center;

            Label profileNotFoundLabel = new Label();
            profileNotFoundLabel.Content = "GEEN BIERBUDDIES MEER IN ZICHT.";
            profileNotFoundLabel.FontSize = BigFontSize;
            profileNotFoundLabel.Foreground = UIUtils.BabyPoeder;
            profileNotFoundLabel.HorizontalAlignment = HorizontalAlignment.Center;

            Label profileNotFoundSubText = new Label();
            profileNotFoundSubText.Content = "ER ZIJN GEEN BIERBUDDIES MEER DIE VOLDOEN AAN JOUW FILTERS";
            profileNotFoundSubText.FontSize = GeneralFontSize;
            profileNotFoundSubText.Foreground = UIUtils.BabyPoeder;
            profileNotFoundSubText.HorizontalAlignment = HorizontalAlignment.Center;

            Canvas buffer = new Canvas();
            buffer.Height = 200;

            profileNotFoundPanel.Children.Add(buffer);
            profileNotFoundPanel.Children.Add(icon);
            profileNotFoundPanel.Children.Add(profileNotFoundLabel);
            profileNotFoundPanel.Children.Add(profileNotFoundSubText);

            profileNotFoundBorder.Child = profileNotFoundPanel;


            return profileNotFoundBorder;
        }
        private UIElement GetFillerBlock(double width)
        {
            Canvas fillerCanvas = new Canvas();
            fillerCanvas.Width = width;
            return fillerCanvas;
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
            _FindBuddies.DislikeVisitor(_Visitor);
            RefreshPage();
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
            _FindBuddies.LikeVisitor(_Visitor);
            RefreshPage();
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
            profileBorder.Child = _profilePanel;
            return profileBorder;
        }
        private void SetProfilePanel(double width, double height)
        {
            _profilePanel.Children.Clear();
            _profilePanel.Width = width;
            _profilePanel.Height = height - 150;
            UIElement profilePicture = GetProfilePicture(width, height - 150);
            _profilePanel.Children.Add(profilePicture);
            Panel.SetZIndex(profilePicture, -1);

            UIElement profileContent = GetProfileContentPanel(width);
            Canvas.SetTop(profileContent, height - UIUtils.ProfileConentHeight - 150);
            _profilePanel.Children.Add(profileContent);

        }
        
        private UIElement GetProfilePicture(double width, double height)
        {
            Canvas canvas = new();
            if (_Visitor == null || _Visitor.Photos.Count == 0)
            {
                return canvas;
            }
            
            _ProfilePicture = new();
            _ProfilePicture.Source = UIUtils.ConvertByteArrayToImage(_Visitor.Photos[_CurrentPhotoIndex]);
            _ProfilePicture.Width = width;
            _ProfilePicture.Height = height;
            _ProfilePicture.Stretch = Stretch.UniformToFill;
            _ProfilePicture.HorizontalAlignment = HorizontalAlignment.Center;
            _ProfilePicture.VerticalAlignment = VerticalAlignment.Center;
            _ProfilePicture.Clip = new RectangleGeometry(new Rect(0, 0, width, height), UIUtils.UniversalCornerRadius.TopRight, UIUtils.UniversalCornerRadius.TopRight);

            canvas.Children.Add(_ProfilePicture);

            if (_Visitor.Photos.Count > 1)
            {
                UIElement previousPicture = GetPreviousPictureButton(100, 100);
                canvas.Children.Add(previousPicture);
                Canvas.SetLeft(previousPicture, 0 - 25);
                Canvas.SetTop(previousPicture, height / 2 - 25);
                UIElement nextPicture = GetNextPictureButton(100, 100);
                canvas.Children.Add(nextPicture);
                Canvas.SetLeft(nextPicture, width - 75);
                Canvas.SetTop(nextPicture, height / 2 - 25);
            }
            return canvas;
        }

        private UIElement GetPreviousPictureButton(double width, double height)
        {
            DockPanel previousButtonPanel = new();
            previousButtonPanel.Width = width;
            previousButtonPanel.Height = height;

            Button previousButton = new();
            previousButton.Template = GetChevronButtonTemplate(UIUtils.BabyPoeder);

            MaterialIcon icon = new MaterialIcon();
            icon.Kind = MaterialIconKind.ChevronLeft;
            icon.Foreground = UIUtils.Onyx;
            previousButton.Content = icon;
            previousButton.Width = width / 3;
            previousButton.VerticalAlignment = VerticalAlignment.Center;
            previousButton.HorizontalAlignment = HorizontalAlignment.Center;
            previousButton.Background = UIUtils.Transparent;
            previousButton.Click += PreviousButton_Click;

            previousButtonPanel.Children.Add(previousButton);
            return previousButtonPanel;
        }

        private UIElement GetNextPictureButton(double width, double height)
        {
            DockPanel nextButtonPanel = new();
            nextButtonPanel.Width = width;
            nextButtonPanel.Height = height;

            Button nextButton = new();
            nextButton.Template = GetChevronButtonTemplate(UIUtils.BabyPoeder);

            MaterialIcon icon = new MaterialIcon();
            icon.Kind = MaterialIconKind.ChevronRight;
            icon.Foreground = UIUtils.Onyx;
            nextButton.Content = icon;
            nextButton.Width = width / 3;
            nextButton.VerticalAlignment = VerticalAlignment.Center;
            nextButton.HorizontalAlignment = HorizontalAlignment.Center;
            nextButton.Background = UIUtils.Transparent;
            nextButton.Click += NextButton_Click;

            nextButtonPanel.Children.Add(nextButton);
            return nextButtonPanel;
        }

        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            if (_CurrentPhotoIndex > 0)
            {
                _CurrentPhotoIndex--;
            }
            else
            {
                _CurrentPhotoIndex = _Visitor.Photos.Count - 1;
            }
            _ProfilePicture.Source = UIUtils.ConvertByteArrayToImage(_Visitor.Photos[_CurrentPhotoIndex]);
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (_CurrentPhotoIndex < _Visitor.Photos.Count - 1)
            {
                _CurrentPhotoIndex++;
            }
            else
            {
                _CurrentPhotoIndex = 0;
            }
            _ProfilePicture.Source = UIUtils.ConvertByteArrayToImage(_Visitor.Photos[_CurrentPhotoIndex]);
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

            UIElement ProfileBanner = GetProfileBanner(UIUtils.Onyx70);
            Grid.SetColumnSpan(ProfileBanner, 3);
            Grid.SetRow(ProfileBanner, 1);

            UIElement drinkLabel;
            UIElement activityLabel;
            UIElement interestLabel;

            drinkLabel = GetProfileLabel(_Visitor.DrinkPreference[0]);
            Grid.SetColumn(drinkLabel, 0);
            Grid.SetRow(drinkLabel, 2);

            activityLabel = GetProfileLabel(_Visitor.ActivityPreference[0]);
            Grid.SetColumn(activityLabel, 1);
            Grid.SetRow(activityLabel, 2);

            interestLabel = GetProfileLabel(_Visitor.Interests[0]);
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
            profileContentBorder.CornerRadius = new CornerRadius(0, 0, 25, 25);

            profileContentBorder.Child = profileGrid;

            return profileContentBorder;
        }
        private UIElement GetProfileBanner(Brush backgroundColor)
        {
            WrapPanel bannerPanel = new WrapPanel();

            bannerPanel.Children.Add(GetNameLabel());
            bannerPanel.Children.Add(GetProfileBannerDot());
            bannerPanel.Children.Add(GetAgeLabel());

            bannerPanel.HorizontalAlignment = HorizontalAlignment.Center;
            bannerPanel.VerticalAlignment = VerticalAlignment.Center;

            ProfileContentBorder contentBannerBorder = new ProfileContentBorder(backgroundColor);
            contentBannerBorder.Child = bannerPanel;
            return contentBannerBorder;
        }
        private UIElement GetNameLabel()
        {
            ProfileContentLabel nameLabel = new ProfileContentLabel($"{_Visitor.Name}", BigFontSize);
            nameLabel.FontSize = BigFontSize;
            return nameLabel;
        }
        private UIElement GetAgeLabel()
        {
            ProfileContentLabel ageLabel = new ProfileContentLabel(_Visitor.Age.ToString(), BigFontSize);
            ageLabel.FontSize = BigFontSize;
            return ageLabel;
        }
        private UIElement GetProfileBannerDot()
        {
            MaterialIcon icon = new MaterialIcon();
            icon.Kind = MaterialIconKind.Dot;
            ProfileContentLabel nameLabel = new ProfileContentLabel(icon.ToString(), BigFontSize);
            nameLabel.Content = icon;
            nameLabel.FontSize = BigFontSize;
            return nameLabel;
        }
        private UIElement GetProfileLabel(string content)
        {
            ProfileContentBorder profileLabel = new ProfileContentBorder(content, GeneralFontSize);
            return profileLabel;
        }
        private UIElement GetBioButton()
        {
            Button bioButton = new Button();
            bioButton.Template = GetChevronButtonTemplate(UIUtils.Onyx70);

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
            _ViewingPreferences = true;
            _profilePanel.Children.Clear();
            SetPreferencesPanel();
            
        }
        private void SetPreferencesPanel()
        {
            Grid preferencesGrid = new Grid();
            preferencesGrid.Width = _profilePanel.Width;
            preferencesGrid.HorizontalAlignment = HorizontalAlignment.Left;
            preferencesGrid.VerticalAlignment = VerticalAlignment.Top;

            ColumnDefinition colDef1 = new ColumnDefinition();
            ColumnDefinition colDef2 = new ColumnDefinition();
            ColumnDefinition colDef3 = new ColumnDefinition();
            preferencesGrid.ColumnDefinitions.Add(colDef1);
            preferencesGrid.ColumnDefinitions.Add(colDef2);
            preferencesGrid.ColumnDefinitions.Add(colDef3);

            // Define the Rows
            RowDefinition rowDef1 = new RowDefinition();
            rowDef1.Height = new GridLength(_profilePanel.Height / 12);
            RowDefinition rowDef2 = new RowDefinition();
            rowDef2.Height = new GridLength(_profilePanel.Height / 9);
            RowDefinition rowDef3 = new RowDefinition();
            rowDef3.Height = new GridLength(_profilePanel.Height / 9 * 4.5);
            RowDefinition rowDef4 = new RowDefinition();
            rowDef4.Height = new GridLength(_profilePanel.Height / 9);
            RowDefinition rowDef5 = new RowDefinition();
            rowDef5.Height = new GridLength(_profilePanel.Height / 9 * 2);

            preferencesGrid.RowDefinitions.Add(rowDef1);
            preferencesGrid.RowDefinitions.Add(rowDef2);
            preferencesGrid.RowDefinitions.Add(rowDef3);
            preferencesGrid.RowDefinitions.Add(rowDef4);
            preferencesGrid.RowDefinitions.Add(rowDef5);

            UIElement backToProfuileButton = GetBioBackButton();
            Grid.SetColumn(backToProfuileButton, 1);
            Grid.SetRow(backToProfuileButton, 0);
            preferencesGrid.Children.Add(backToProfuileButton);

            UIElement profileBanner = GetProfileBanner(UIUtils.Onyx);
            Grid.SetColumnSpan(profileBanner, 3);
            Grid.SetRow(profileBanner, 1);
            preferencesGrid.Children.Add(profileBanner);

            UIElement drinkPreferences = GetDrinkPreferenceTable();
            Grid.SetColumn(drinkPreferences, 0);
            Grid.SetRow(drinkPreferences, 2);
            preferencesGrid.Children.Add(drinkPreferences);

            UIElement activityPreferences = GetActivityPreferenceTable();
            Grid.SetColumn(activityPreferences, 1);
            Grid.SetRow(activityPreferences, 2);
            preferencesGrid.Children.Add(activityPreferences);

            UIElement interests = GetinterestsTable();
            Grid.SetColumn(interests, 2);
            Grid.SetRow(interests, 2);
            preferencesGrid.Children.Add(interests);

            ProfileContentBorder bioBorder = new ProfileContentBorder("BIO", UIUtils.Onyx, BigFontSize);
            Grid.SetColumnSpan(bioBorder, 3);
            Grid.SetRow(bioBorder, 3);
            preferencesGrid.Children.Add(bioBorder);

            ProfileContentBorder bioText = new ProfileContentBorder(_Visitor.Bio, UIUtils.Onyx70, GeneralFontSize);
            bioText.Child = new TextBlock { TextWrapping = TextWrapping.Wrap, Text = _Visitor.Bio, FontSize = GeneralFontSize, Foreground = Brushes.White, Margin = new Thickness(10, 10, 10, 10) };
            Grid.SetColumnSpan(bioText, 3);
            Grid.SetRow(bioText, 4);
            preferencesGrid.Children.Add(bioText);

            Border preferencesBorder = new Border();
            preferencesBorder.Background = UIUtils.BabyPoeder;
            preferencesBorder.CornerRadius = UIUtils.SquirqilCornerRadius;
            preferencesBorder.Child = preferencesGrid;

            Canvas.SetTop(preferencesBorder, 0);
            _profilePanel.Children.Add(preferencesBorder);
        }
        private UIElement GetDrinkPreferenceTable()
        {
            StackPanel drinkPrefPanel = new();
            ProfileContentBorder drinkPrefContentBorder = new ProfileContentBorder("Drank", UIUtils.Onyx, BigFontSize);
            drinkPrefPanel.Children.Add(drinkPrefContentBorder);
            for (int i = 0; i < _Visitor.DrinkPreference.Count; i++)
            {
                ProfileContentBorder drinkPref = new(_Visitor.DrinkPreference[i], GeneralFontSize);
                drinkPrefPanel.Children.Add(drinkPref);
            }
            ProfileContentBorder profileContentBorder = new ProfileContentBorder(UIUtils.Onyx70);
            profileContentBorder.Child = drinkPrefPanel;
            return profileContentBorder;
        }
        private UIElement GetActivityPreferenceTable()
        {
            StackPanel activityPrefPanel = new();
            ProfileContentBorder activityPrefContentBorder = new ProfileContentBorder("Activiteiten", UIUtils.Onyx, GeneralFontSize);
            activityPrefContentBorder.ProfileContentLabel.FontSize = BigFontSize;
            activityPrefPanel.Children.Add(activityPrefContentBorder);
            for (int i = 0; i < _Visitor.ActivityPreference.Count; i++)
            {
                ProfileContentBorder drinkPref = new(_Visitor.ActivityPreference[i], GeneralFontSize);
                activityPrefPanel.Children.Add(drinkPref);
            }
            ProfileContentBorder profileContentBorder = new ProfileContentBorder(UIUtils.Onyx70);
            profileContentBorder.Child = activityPrefPanel;
            return profileContentBorder;
        }
        private UIElement GetinterestsTable()
        {
            StackPanel interestsPanel = new();
            ProfileContentBorder interestsContentBorder = new ProfileContentBorder("Interesses", UIUtils.Onyx, GeneralFontSize);
            interestsContentBorder.ProfileContentLabel.FontSize = BigFontSize;
            interestsPanel.Children.Add(interestsContentBorder);
            for (int i = 0; i < _Visitor.Interests.Count; i++)
            {
                ProfileContentBorder drinkPref = new(_Visitor.Interests[i], GeneralFontSize);
                interestsPanel.Children.Add(drinkPref);
            }
            ProfileContentBorder profileContentBorder = new ProfileContentBorder(UIUtils.Onyx70);
            profileContentBorder.Child = interestsPanel;
            return profileContentBorder;
        }


        private UIElement GetBioBackButton()
        {
            Button bioButton = new Button();
            bioButton.Template = GetChevronButtonTemplate(UIUtils.Onyx70);

            MaterialIcon icon = new MaterialIcon();
            icon.Kind = MaterialIconKind.ChevronUp;
            icon.Foreground = UIUtils.BabyPoeder;
            bioButton.Content = icon;
            bioButton.Click += BioBackButton_Click;

            bioButton.HorizontalAlignment = HorizontalAlignment.Center;
            return bioButton;
        }

        private ControlTemplate GetChevronButtonTemplate(Brush background)
        {
            ControlTemplate template = new ControlTemplate(typeof(Button));
            FrameworkElementFactory gridFactory = new FrameworkElementFactory(typeof(Grid));

            FrameworkElementFactory borderFactory = new FrameworkElementFactory(typeof(Border));
            //set background color
            borderFactory.SetValue(Border.BackgroundProperty, background);
            borderFactory.SetValue(Border.CornerRadiusProperty, new CornerRadius(90));
            gridFactory.AppendChild(borderFactory);

            FrameworkElementFactory contentPresenterFactory = new FrameworkElementFactory(typeof(ContentPresenter));
            contentPresenterFactory.SetValue(ContentPresenter.HorizontalAlignmentProperty, HorizontalAlignment.Center);
            contentPresenterFactory.SetValue(ContentPresenter.VerticalAlignmentProperty, VerticalAlignment.Center);
            gridFactory.AppendChild(contentPresenterFactory);

            template.VisualTree = gridFactory;
            return template;
        }
        private void BioBackButton_Click(object sender, RoutedEventArgs e)
        {
            _ViewingPreferences = false;
            _profilePanel.Children.Clear();
            SetProfilePanel((_MainWindowSize.Width - _NavBarWidth)/2, _MainWindowSize.Height);
        }

        public void RefreshPage()
        { 
            long oldID = _Visitor?.ID ?? -1;
            _Visitor = _FindBuddies.GetPotentialMatch();
            if (_Visitor == null || _Visitor.ID != oldID)
            {
                _ViewingPreferences = false;
            }
            GetFindBuddiesPage(_Visitor);
        }
        public void UpdatePageSize(double newNavBarWidth, Size newScreenSize)
        {
            _NavBarWidth = newNavBarWidth;
            _MainWindowSize = newScreenSize;

            // Fontsize aanpassen
            if (_MainWindowSize.Width < 1500)
            {
                BigFontSize = _MinFontSizeBig;
                GeneralFontSize = _MinFontSizeGeneral;
            }
            else
            {
                BigFontSize = 28;
                GeneralFontSize = 16;
            }
        }
        public void OnClientProfileChanged(object sender, ClientProfileChangedEventArgs args)
        {
            RefreshPage();
        }
    }
    

    public class ProfileContentBorder : Border
    {
        public int FatherFont;
        public Label ProfileContentLabel { get; set; }
        public ProfileContentBorder(string content, int fatherfont) : this(content, UIUtils.Onyx70, fatherfont) { }
        public ProfileContentBorder(string content, Brush backgroundColor, int fatherfont)
        {
            this.FatherFont = fatherfont;
            ProfileContentLabel = new ProfileContentLabel(content, FatherFont);
            this.Child = ProfileContentLabel;
            this.Background = backgroundColor;
            this.CornerRadius = UIUtils.UniversalCornerRadius;
            this.Margin = new Thickness(10);
        }
        public ProfileContentBorder(int fatherfont) : this(UIUtils.Onyx70) { }
        public ProfileContentBorder(Brush backgroundColor)
        {
            ProfileContentLabel = new Label();
            this.Background = backgroundColor;
            this.CornerRadius = UIUtils.UniversalCornerRadius;
            this.Margin = new Thickness(10);
        }

    }
    public class ProfileContentLabel : Label
    {


        public ProfileContentLabel(string content, int fontsize) : this(fontsize)
        {
            this.Content = content;
            this.FontSize = fontsize;
        }

        public TextAlignment TextAlignment { get; internal set; }


        public ProfileContentLabel(int fontsize)
        {
            this.Foreground = UIUtils.BabyPoeder;
            this.VerticalAlignment = VerticalAlignment.Center;
            this.HorizontalAlignment = HorizontalAlignment.Center;
            this.FontFamily = UIUtils.UniversalFontFamily;
            this.FontSize = fontsize;
        }

        public ProfileContentLabel(string name)
        {
            Name = name;
        }
    }
}
