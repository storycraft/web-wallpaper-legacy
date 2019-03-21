using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebWallpaper.Taskbar
{
    public class TaskbarController : IDisposable
    {

        public WebWallpaper.Wallpaper.WebWallpaper WebWallpaper { get; }

        protected ToolStripMenuItem InfoLabelItem { get; private set; }
        protected ToolStripMenuItem MouseMoveEnabledItem { get; private set; }
        protected ToolStripMenuItem MouseInteractionEnabledItem { get; private set; }

        protected ToolStripMenuItem SetURLItem { get; private set; }

        protected ToolStripMenuItem RenderEnabledItem { get; private set; }

        protected ToolStripMenuItem ExitItem { get; private set; }

        public bool Visible
        {
            get => TaskbarIcon.Visible;
            set
            {
                TaskbarIcon.Visible = value;
            }
        }

        public NotifyIcon TaskbarIcon { get; private set; }

        public TaskbarController(WebWallpaper.Wallpaper.WebWallpaper webWallpaper)
        {
            WebWallpaper = webWallpaper;
        }

        public void Run()
        {
            TaskbarIcon = new NotifyIcon();

            InitIcon();

            Visible = true;

            Application.EnableVisualStyles();

            Application.Run();
        }

        protected virtual void InitIcon()
        {
            TaskbarIcon.Text = "WebWallpaper";
            TaskbarIcon.Icon = SystemIcons.Application;

            TaskbarIcon.ContextMenuStrip = BuildContextMenu();
            TaskbarIcon.ContextMenuStrip.ItemClicked += MenuItemClicked;
            TaskbarIcon.ContextMenuStrip.Opening += MenuOpening;
        }

        private void MenuOpening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MouseMoveEnabledItem.Checked = WebWallpaper.ConfigManager.CurrentConfig.HandleMovement.Value;
            MouseInteractionEnabledItem.Checked = WebWallpaper.ConfigManager.CurrentConfig.ClickEnabled.Value;
            RenderEnabledItem.Checked = WebWallpaper.ConfigManager.CurrentConfig.RenderEnabled.Value;
        }

        private void MenuItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem == ExitItem)
            {
                WebWallpaper.Stop();
            }
            else if (e.ClickedItem == MouseMoveEnabledItem)
            {
                WebWallpaper.ConfigManager.CurrentConfig.HandleMovement.Value = !WebWallpaper.ConfigManager.CurrentConfig.HandleMovement.Value;
            }
            else if (e.ClickedItem == MouseInteractionEnabledItem)
            {
                WebWallpaper.ConfigManager.CurrentConfig.ClickEnabled.Value = !WebWallpaper.ConfigManager.CurrentConfig.ClickEnabled.Value;
            }
            else if (e.ClickedItem == SetURLItem)
            {
                if (!WebWallpaper.BrowserManager.Ready)
                {
                    MessageBox.Show("Web process is still loading. Please wait.");
                    return;
                }

                string url = WebWallpaper.BrowserManager.Browser.CurrentURL;
                DialogResult result = InputUtil.InputBox("WebWallpaper", "Type Wallpaper url here", ref url);

                if (result.HasFlag(DialogResult.OK))
                {
                    WebWallpaper.BrowserManager.Browser.CurrentURL = url;
                }
            }
            else if (e.ClickedItem == RenderEnabledItem)
            {
                WebWallpaper.ConfigManager.CurrentConfig.RenderEnabled.Value = !WebWallpaper.ConfigManager.CurrentConfig.RenderEnabled.Value;
            }
        }

        protected virtual ContextMenuStrip BuildContextMenu()
        {
            ContextMenuStrip menu = new ContextMenuStrip();

            menu.Items.Add(InfoLabelItem = new ToolStripMenuItem()
            {
                Text = "WebWallpaper",
                Enabled = false
            });

            menu.Items.Add(new ToolStripSeparator());

            menu.Items.Add(MouseMoveEnabledItem = new ToolStripMenuItem()
            {
                Text = "Mouse Movement"
            });

            menu.Items.Add(MouseInteractionEnabledItem = new ToolStripMenuItem()
            {
                Text = "Mouse Interaction"
            });

            menu.Items.Add(SetURLItem = new ToolStripMenuItem()
            {
                Text = "Set URL"
            });

            menu.Items.Add(RenderEnabledItem = new ToolStripMenuItem()
            {
                Text = "Wallpaper Enabled"
            });

            menu.Items.Add(ExitItem = new ToolStripMenuItem()
            {
                Text = "Exit"
            });

            return menu;
        }

        public void Dispose()
        {
            if (TaskbarIcon != null)
            {
                TaskbarIcon.Dispose();
            }
        }
    }
}
