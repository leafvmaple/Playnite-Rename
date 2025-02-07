using Playnite.SDK;
using Playnite.SDK.Events;
using Playnite.SDK.Models;
using Playnite.SDK.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

using System.IO;
using System.Windows;
using System.Collections.ObjectModel;

namespace Rename
{
    public class Rename : GenericPlugin
    {
        private static readonly ILogger logger = LogManager.GetLogger();
        private readonly IResourceProvider resources;

        private RenameSettingsViewModel settings { get; set; }

        public override Guid Id { get; } = Guid.Parse("89056061-1b70-40a1-8a17-07f01b24ea29");

        public Rename(IPlayniteAPI api) : base(api)
        {
            resources = api.Resources;
            settings = new RenameSettingsViewModel(this);
            Properties = new GenericPluginProperties
            {
                HasSettings = true
            };
        }

        public override void OnGameInstalled(OnGameInstalledEventArgs args)
        {
            // Add code to be executed when game is finished installing.
        }

        public override void OnGameStarted(OnGameStartedEventArgs args)
        {
            // Add code to be executed when game is started running.
        }

        public override void OnGameStarting(OnGameStartingEventArgs args)
        {
            // Add code to be executed when game is preparing to be started.
        }

        public override void OnGameStopped(OnGameStoppedEventArgs args)
        {
            // Add code to be executed when game is preparing to be started.
        }

        public override void OnGameUninstalled(OnGameUninstalledEventArgs args)
        {
            // Add code to be executed when game is uninstalled.
        }

        public override void OnApplicationStarted(OnApplicationStartedEventArgs args)
        {
            // Add code to be executed when Playnite is initialized.
        }

        public override void OnApplicationStopped(OnApplicationStoppedEventArgs args)
        {
            // Add code to be executed when Playnite is shutting down.
        }

        public override void OnLibraryUpdated(OnLibraryUpdatedEventArgs args)
        {
            // Add code to be executed when library is updated.
        }

        public override ISettings GetSettings(bool firstRunSettings)
        {
            return settings;
        }

        public override UserControl GetSettingsView(bool firstRunSettings)
        {
            return new RenameSettingsView();
        }

        public override IEnumerable<GameMenuItem> GetGameMenuItems(GetGameMenuItemsArgs args)
        {
            yield return new GameMenuItem
            {
                Description = resources.GetString("MenuItemDescription"),
                Action = (_) => OnMenuAction()
            };
        }

        // To add new main menu items override GetMainMenuItems
        public override IEnumerable<MainMenuItem> GetMainMenuItems(GetMainMenuItemsArgs args)
        {
            yield return new MainMenuItem
            {
                MenuSection = "@Rename",
                Description = resources.GetString("MenuItemDescription"),
                Action = (_) => OnMenuAction()
            };
        }

        private void OnMenuAction()
        {
            var selectedGames = PlayniteApi.MainView.SelectedGames;
            if (selectedGames == null || !selectedGames.Any())
            {
                PlayniteApi.Dialogs.ShowMessage("Select A Game");
                return;
            }

            //var filePath = PlayniteApi.Dialogs.SelectFile("Select A File");
            //if (string.IsNullOrEmpty(filePath))
            //{
            //    return;
            //}

            List<(string first, string second, Game third)> items = new List<(string first, string second, Game third)>();
            foreach (var game in selectedGames)
            {
                var installDirectory = game.InstallDirectory;
                var roms = game.Roms;
                if (roms == null || roms.Count == 0)
                    continue;
                var romPath = RenameFilesHelper.getAlteredRomPath(game);
                var oldPath = RenameFilesHelper.getFormatPath(installDirectory, roms[0].Path);
                var newPath = RenameFilesHelper.getFormatPath(installDirectory, romPath);
                // game.Roms[0].Path = romPath;
                items.Add((oldPath, newPath, game));
            }

            if (ShowDialog(items))
            {
                foreach (var (oldPath, newPath, game) in items)
                {
                    try
                    {
                        RenameFilesHelper.RenameFile(oldPath, newPath);
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex, "Rename Failed!");
                        PlayniteApi.Dialogs.ShowErrorMessage($"Rename Failed：{ex.Message}", "error");
                    }
                }
            }
        }

        private bool ShowDialog(List<(string, string, Game)> items)
        {
            var messageBuilder = new StringBuilder();
            messageBuilder.AppendLine($"{ resources.GetString("RenameFilesHeader") } \n");

            foreach (var (oldPath, newPath, _) in items)
            {
                var oldName = Path.GetFileName(oldPath);
                var newName = Path.GetFileName(newPath);
                messageBuilder.AppendLine($"{ oldName } { resources.GetString("RenameFilesSeparator") } { newName }");
            }

            messageBuilder.AppendLine($"\n{ string.Format(resources.GetString("RenameFilesFooter"), items.Count) }");

            var result = PlayniteApi.Dialogs.ShowMessage(
                messageBuilder.ToString(),
                resources.GetString("RenameFilesDialogTitle"),
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            return result == MessageBoxResult.Yes;
        }
    }
}