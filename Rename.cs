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

            foreach (var game in selectedGames)
            {
                var installDirectory = game.InstallDirectory;

                // 获取游戏启动文件
                var roms = game.Roms;
                var filePath = roms[0].Path.Replace("{InstallDir}\\", installDirectory);

                try
                {
                    RenameFilesHelper.RenameFile(filePath, game.Name);
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "Rename Failed!");
                    PlayniteApi.Dialogs.ShowErrorMessage($"Rename Failed：{ex.Message}", "error");
                }
            }
        }
    }
}