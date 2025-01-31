﻿using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Input;
using HaruhiChokuretsuLib.Util;
using SerialLoops.Assets;
using SerialLoops.Lib.Items;
using SerialLoops.Models;
using SerialLoops.Utility;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;

namespace SerialLoops.ViewModels.Panels
{
    public abstract class ItemListPanel : ViewModelBase
    {
        public double Width { get; set; }
        public double Height { get; set; }

        private List<ItemDescription> _items;
        public List<ItemDescription> Items
        {
            protected get { return _items; }
            set
            {
                _items = value;
                Source = new ObservableCollection<ITreeItem>(GetSections());
            }
        }
        public ObservableCollection<ITreeItem> Source { get; set; }

        protected ILogger _log;
        protected bool ExpandItems { get; set; }

        public void InitializeItems(List<ItemDescription> items, bool expandItems, ILogger log)
        {
            Items = items;
            ExpandItems = expandItems;
            _log = log;
        }

        private ObservableCollection<ITreeItem> GetSections()
        {
            return new ObservableCollection<ITreeItem>(Items.GroupBy(i => i.Type).OrderBy(g => LocalizeItemTypes(g.Key))
                .Select(g => new SectionTreeItem(LocalizeItemTypes(g.Key), g.Select(i => new ItemDescriptionTreeItem(i)), ControlGenerator.GetIcon(g.Key.ToString(), _log))));
        }

        private static string LocalizeItemTypes(ItemDescription.ItemType type)
        {
            return type switch
            {
                ItemDescription.ItemType.Background => Strings.Backgrounds,
                ItemDescription.ItemType.BGM => Strings.BGMs,
                ItemDescription.ItemType.Character => Strings.Characters,
                ItemDescription.ItemType.Character_Sprite => Strings.Character_Sprites,
                ItemDescription.ItemType.Chess_Puzzle => Strings.Chess_Puzzles,
                ItemDescription.ItemType.Chibi => Strings.Chibis,
                ItemDescription.ItemType.Group_Selection => Strings.Group_Selections,
                ItemDescription.ItemType.Item => Strings.Items,
                ItemDescription.ItemType.Layout => Strings.Layouts,
                ItemDescription.ItemType.Map => Strings.Maps,
                ItemDescription.ItemType.Place => Strings.Places,
                ItemDescription.ItemType.Puzzle => Strings.Puzzles,
                ItemDescription.ItemType.Scenario => Strings.Scenario,
                ItemDescription.ItemType.Script => Strings.Scripts,
                ItemDescription.ItemType.SFX => Strings.SFXs,
                ItemDescription.ItemType.System_Texture => Strings.System_Textures,
                ItemDescription.ItemType.Topic => Strings.Topics,
                ItemDescription.ItemType.Transition => Strings.Transitions,
                ItemDescription.ItemType.Voice => Strings.Voices,
                _ => "UNKNOWN TYPE",
            };
        }

        public void SetupViewer(TreeView viewer)
        {
            viewer.ItemTemplate = new FuncTreeDataTemplate<ITreeItem>((item, namescope) => item.GetDisplay(), item => item.Children);
            viewer.ItemsSource = Source;
            viewer.DoubleTapped += ItemList_ItemDoubleClicked;
        }

        public abstract void ItemList_ItemDoubleClicked(object sender, TappedEventArgs args);
    }
}
