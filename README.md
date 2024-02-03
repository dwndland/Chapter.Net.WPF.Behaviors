# Chapter.Net.WPF.Behaviors Library

## Overview
Brings additional behaviors to existing WPF controls.

## Features
- **ColumnSortBehavior:** Enables sorting by click on the header on a ListView.
- **ColumnVisibilityBehavior:** Enables dynamically display and hide of columns of a ListView.
- **ColumnWidthBehavior:** Allows defining column sizes of a ListView.
- **ComboBoxBehavior:** Brings the definition of max text length of the combobox if it is editable.
- **CutTooltipBehavior:** Brings the functionality to the TextBlock and Label to show the text in the tooltip automatically when its cut.
- **DoubleClickBehavior:** Brings the feature to be able to double click any UI element.
- **DragMoveBehavior:** Enables that a window can be moved by the mouse when drop down on a content.
- **DynamicResourceBehavior:** Brings the possibility to create a dynamic resource out of a binding value.
- **FocusBehavior:** Brings the feature to set the focus to a specific element or on window launch.
- **FrameworkBehavior:** Brings commands for actions on FrameworkElements.
- **KeyBlockBehavior:** Disabled specific keys on a control.
- **ListBehavior:** Gives you some commands when clicking in an ItemsControl or its items.
- **ScrollBehavior:** Brings the feature to modify the scroll position of an items control.
- **TextBoxBehavior:** Brings the features to text boxes to define its selection or bound the selection part.
- **WindowBehavior:** Brings the feature to a Window to bind loading and closing action or easy close with dialog result.
- **WindowTitleBarBehavior:** Brings the feature to the Window to disable or hide elements in the title bar.

## Getting Started

1. **Installation:**
    - Install the Chapter.Net.WPF.Behaviors library via NuGet Package Manager:
    ```bash
    dotnet add package Chapter.Net.WPF.Behaviors
    ```

2. **ColumnSortBehavior:**
    - Usage
    ```xaml
    <DataTemplate x:Key="HeaderArrowUp">
        <Grid>
            <TextBlock Text="{Binding}" VerticalAlignment="Center" HorizontalAlignment="Center" />
            <Path StrokeThickness="0" SnapsToDevicePixels="True" Data="M 0,4 L 4,0 L 8,4 L 0,4"
                  Margin="0,-2,0,0" VerticalAlignment="Top" HorizontalAlignment="Center">
                <Path.Fill>
                    <LinearGradientBrush StartPoint="0,0" EndPoint="1,1">
                        <GradientStop Color="#FF3C5E72" Offset="0"/>
                        <GradientStop Color="#FFC3E4F5" Offset="1"/>
                    </LinearGradientBrush>
                </Path.Fill>
            </Path>
        </Grid>
    </DataTemplate>
    <DataTemplate x:Key="HeaderArrowDown">
        <Grid>
            <TextBlock Text="{Binding}" VerticalAlignment="Center" HorizontalAlignment="Center" />
            <Path StrokeThickness="0" SnapsToDevicePixels="True" Data="M 0,0 L 4,4 L 8,0 L 0,0"
                  Margin="0,-2,0,0" VerticalAlignment="Top" HorizontalAlignment="Center">
                <Path.Fill>
                    <LinearGradientBrush StartPoint="0,0" EndPoint="1,1">
                        <GradientStop Color="#FF3C5E72" Offset="0"/>
                        <GradientStop Color="#FFC4E3F4" Offset="1"/>
                    </LinearGradientBrush>
                </Path.Fill>
            </Path>
        </Grid>
    </DataTemplate>
    <DataTemplate x:Key="HeaderTransparent">
        <Grid>
            <TextBlock Text="{Binding}" VerticalAlignment="Center" HorizontalAlignment="Center" />
        </Grid>
    </DataTemplate>

    <ListView controls:ColumnSortBehavior.AllowColumnSortings="True"
              controls:ColumnSortBehavior.AscendingSortHeaderTemplate="{StaticResource HeaderArrowUp}"
              controls:ColumnSortBehavior.DescendingSortHeaderTemplate="{StaticResource HeaderArrowDown}"
              controls:ColumnSortBehavior.NeutralHeaderTemplate="{StaticResource HeaderTransparent}">
        <ListView.View>
            <GridView>
                <GridViewColumn Header="Name" controls:ColumnSortBehavior.IsDefaultSortColumn="True" controls:ColumnSortBehavior.SortPropertyName="Name" />
                <GridViewColumn Header="Size" controls:ColumnSortBehavior.SortPropertyName="Size" />
                <GridViewColumn Header="Date" controls:ColumnSortBehavior.SortPropertyName="Date" />
            </GridView>
        </ListView.View>
    </ListView>
    ```

3. **ColumnVisibilityBehavior:**
    - Usage
    ```csharp
    public class MainViewModel : ObservableObject
    {
        public MainViewModel()
        {
            VisibleColumnNames = new EnhancedObservableCollection<string> { "Name", "Size" };
        }

        public EnhancedObservableCollection<string> VisibleColumnNames { get; private set; }
    }
    ```
    ```xaml
    <ListView controls:ColumnVisibilityBehavior.VisibleColumns="{Binding VisibleColumnNames}">
        <ListView.View>
            <GridView>
                <GridViewColumn Header="Name" DisplayMemberBinding="{Binding Name}" controls:ColumnVisibilityBehavior.Name="Name" />
                <GridViewColumn Header="Size" DisplayMemberBinding="{Binding Size}" controls:ColumnVisibilityBehavior.Name="Size" />
                <GridViewColumn Header="Date" DisplayMemberBinding="{Binding Date}" controls:ColumnVisibilityBehavior.Name="Date" />
            </GridView>
        </ListView.View>
    </ListView>
    ```

4. **ColumnWidthBehavior:**
    - Usage
    ```xaml
    <ListView controls:ColumnWidthBehavior.AutoSize="Proportional">
        <ListView.View>
            <GridView>
                <GridViewColumn Header="Name" DisplayMemberBinding="{Binding Name}" controls:ColumnWidthBehavior.ProportionalWidth="60" />
                <GridViewColumn Header="Size" DisplayMemberBinding="{Binding Size}" controls:ColumnWidthBehavior.ProportionalWidth="30" />
                <GridViewColumn Header="Date" DisplayMemberBinding="{Binding Date}" controls:ColumnWidthBehavior.ProportionalWidth="10" />
            </GridView>
        </ListView.View>
    </ListView>
    ```

5. **ComboBoxBehavior:**
    - Usage
    ```xaml
    <ComboBox IsEditable="True" controls:ComboBoxBehavior.MaxLength="30" />
    ```

6. **CutTooltipBehavior:**
    - Usage
    ```xaml
    <TextBlock Text="{Binding AnyLongtext}" controls:CutTooltipBehavior.ShowTooltip="Width" />
    ```

7. **DoubleClickBehavior:**
    - Usage
    ```xaml
    <TextBlock Text="Doubleclick Me"
               controls:DoubleClickBehavior.Command="{Binding ItemDoubleClicked}"
               controls:DoubleClickBehavior.CommandParameter="Parameter" />
    ```

8. **DragMoveBehavior:**
    - Usage
    ```xaml
    <Window controls:DragMoveBehavior.Enabled="True">
    </Window>

    <Window>
        <DockPanel>
            <Border Height="22" Background="Gray" controls:DragMoveBehavior.Enabled="True">
                <TextBlock Text="{Binding Title}" />
            </Border>

            <Grid>
            </Grid>
        </DockPanel>
    </Window>
    ```

9. **DynamicResourceBehavior:**
    - Usage
    ```xaml
    <TextBlock controls:DynamicResourceBehavior.ResourceKey="{Binding MyTranslationKey}" />
    ```

10. **FocusBehavior:**
    - Usage
    ```xaml
    <Window controls:FocusBehavior.ApplicationGotFocusCommand="{Binding SwitchedToApplicationCommand}"
            controls:FocusBehavior.ApplicationLostFocusCommand="{Binding SwitchedOutFromApplicationCommand}">
    </Window>

    <Button controls:FocusBehavior.GotFocusCommand="{Binding ButtonGotFocusCommand}"
            controls:FocusBehavior.GotFocusCommandParameter="Example" />

    <Button controls:FocusBehavior.LostFocusCommand="{Binding ButtonGotFocusCommand}"
            controls:FocusBehavior.LostFocusCommandParameter="Example" />
 
    <Button controls:FocusBehavior.HasFocus="{Binding IsButtonFocused}" />
    ```

11. **FrameworkBehavior:**
    - Usage
    ```xaml
    <Button controls:FrameworkBehavior.LoadedCommand="{Binding ButtonLoadedCommand" />
    ```
    ```csharp
    public class MainViewModel : ObservableObject
    {
        public MainViewModel()
        {
            ButtonLoadedCommand = new DelegateCommand(ButtonLoaded);
        }
   
        public DelegateCommand ButtonLoadedCommand { get; private set; }
   
        private void ButtonLoaded()
        {
        }
    }
    ```

12. **KeyBlockBehavior:**
    - Usage
    ```xaml
    <Window controls:KeyBlockBehavior.BlockAll="True">
    </Window>
    ```

13. **ListBehavior:**
    - Usage
    ```xaml
    <ListBox controls:ListBehavior.ItemDoubleClickedCommand="{Binding ItemDoubleClickedCommand}"
         
             controls:ListBehavior.ItemClickedCommand="{Binding ItemClickedCommand}"
         
             controls:ListBehavior.EmptyAreaDoubleClickCommand="{Binding EmptyAreaDoubleClickCommand}"
             controls:ListBehavior.EmptyAreaDoubleClickCommandParameter="Parameter"
         
             controls:ListBehavior.EmptyAreaClickCommand="{Binding EmptyAreaClickCommand}"
             controls:ListBehavior.EmptyAreaClickCommandParameter="Parameter"
         
             controls:ListBehavior.AutoDeselect="True" />
    ```

14. **ScrollBehavior:**
    - Usage
    ```xaml
    <ListBox ItemsSource="{Binding LogEntries}"
             controls:ScrollBehavior.AutoScrollToLast="True" />

    <ListBox ItemsSource="{Binding LogEntries}"
             controls:ScrollBehavior.ScrollToItem="{Binding ImportantEntry}" />
    ```

15. **TextBoxBehavior:**
    - Usage
    ```xaml
    <TextBox Text="{Binding TheText}"
             controls:TextBoxBehavior.SelectAllOnFocus="True"
             controls:TextBoxBehavior.SelectedText="{Binding SelectedText}" />
    ```

16. **WindowBehavior:**
    - Usage
    ```xaml
    <Window controls:WindowBehavior.ClosingCommand="{Binding ClosingCommand}">

        <Button Content="Close" controls:WindowBehavior.DialogResult="True" />
    
        <Button Content="Try Close" controls:WindowBehavior.DialogResultCommand="{Binding TryCloseCommand}" />

    </Window>
    ```
    ```csharp
    public class MainViewModel : ObservableObject
    {
        public MainViewModel()
        {
            TryCloseCommand = new DelegateCommand<WindowClosingArgs>(TryClose);
        }
  
        public DelegateCommand<WindowClosingArgs> TryCloseCommand { get; private set; }
  
        private void TryClose(WindowClosingArgs e)
        {
            // Ask user if really close
            e.Cancel = true;
  
            //e.DialogResult = false;
        }
    }
    ```

17. **WindowTitleBarBehavior:**
    - Usage
    ```xaml
    <Window controls:WindowTitleBarBehavior.DisableMinimizeButton="True"
            controls:WindowTitleBarBehavior.DisableMaximizeButton="True"
            controls:WindowTitleBarBehavior.DisableSystemMenu="True">
    </Window>
    ```

## Links
* [NuGet](https://www.nuget.org/packages/Chapter.Net.WPF.Behaviors)
* [GitHub](https://github.com/dwndland/Chapter.Net.WPF.Behaviors)

## License
Copyright (c) David Wendland. All rights reserved.
Licensed under the MIT License. See LICENSE file in the project root for full license information.
