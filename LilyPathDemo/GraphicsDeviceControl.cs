using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Xna.Framework.Graphics;
using OpenTK;
using Microsoft.Xna.Framework;
using OpenTK.Graphics.OpenGL;

namespace LilyPathDemo
{
    public abstract class GraphicsDeviceControl : GLControl
    {
        #region Fields

        private bool _designMode;

        Form _mainForm;

        GraphicsDeviceService _deviceService;

        ServiceContainer _services = new ServiceContainer();

        #endregion

        #region Properties

        public Form MainForm
        {
            get { return _mainForm; }
            internal set { _mainForm = value; }
        }

        public GraphicsDevice GraphicsDevice
        {
            get { return _deviceService.GraphicsDevice; }
        }

        public GraphicsDeviceService GraphicsDeviceService
        {
            get { return _deviceService; }
        }

        public ServiceContainer Services
        {
            get { return _services; }
        }

        #endregion

        #region Events

        public event EventHandler<EventArgs> ControlInitialized;
        public event EventHandler<EventArgs> ControlInitializing;

        #endregion

        #region Initialization

        protected GraphicsDeviceControl ()
        {
            _designMode = DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime;

            Load += _tkControl_Load;
        }

        private void _tkControl_Load (object sender, EventArgs e)
        {
            if (!DesignMode) {
                _deviceService = GraphicsDeviceService.AddRef(Handle, ClientSize.Width, ClientSize.Height);

                _services.AddService<IGraphicsDeviceService>(_deviceService);

                if (ControlInitializing != null) {
                    ControlInitializing(this, EventArgs.Empty);
                }

                Initialize();

                if (ControlInitialized != null) {
                    ControlInitialized(this, EventArgs.Empty);
                }
            }
        }

        protected override void Dispose (bool disposing)
        {
            if (_deviceService != null) {
                try {
                    _deviceService.Release();
                }
                catch { }

                _deviceService = null;
            }

            base.Dispose(disposing);
        }

        protected new bool DesignMode
        {
            get { return _designMode; }
        }

        #endregion

        #region Paint

        protected override void OnPaint (PaintEventArgs e)
        {
            string beginDrawError = BeginDraw();

            if (string.IsNullOrEmpty(beginDrawError)) {
                Draw();
                EndDraw();
            }
            else {
                PaintUsingSystemDrawing(e.Graphics, beginDrawError);
            }
        }

        private string BeginDraw ()
        {
            if (_deviceService == null) {
                return Text + "\n\n" + GetType();
            }

            string deviceResetError = HandleDeviceReset();

            if (!string.IsNullOrEmpty(deviceResetError)) {
                return deviceResetError;
            }

            GLControl control = GLControl.FromHandle(_deviceService.GraphicsDevice.PresentationParameters.DeviceWindowHandle) as GLControl;
            if (control != null) {
                control.Context.MakeCurrent(WindowInfo);
                _deviceService.GraphicsDevice.PresentationParameters.BackBufferHeight = ClientSize.Height;
                _deviceService.GraphicsDevice.PresentationParameters.BackBufferWidth = ClientSize.Width;
            }

            Viewport viewport = new Viewport();

            viewport.X = 0;
            viewport.Y = 0;

            viewport.Width = ClientSize.Width;
            viewport.Height = ClientSize.Height;

            viewport.MinDepth = 0;
            viewport.MaxDepth = 1;

            if (GraphicsDevice.Viewport.Bounds != viewport.Bounds)
                GraphicsDevice.Viewport = viewport;

            return null;
        }

        private static Random rand = new Random();

        private void EndDraw ()
        {
            try {
                SwapBuffers();
            }
            catch {
            }
        }

        private string HandleDeviceReset ()
        {
            bool needsReset = false;

            switch (GraphicsDevice.GraphicsDeviceStatus) {
                case GraphicsDeviceStatus.Lost:
                    return "Graphics device lost";

                case GraphicsDeviceStatus.NotReset:
                    needsReset = true;
                    break;

                default:
                    PresentationParameters pp = GraphicsDevice.PresentationParameters;
                    needsReset = (ClientSize.Width > pp.BackBufferWidth) || (ClientSize.Height > pp.BackBufferHeight);
                    break;
            }

            if (needsReset) {
                try {
                    _deviceService.ResetDevice(ClientSize.Width, ClientSize.Height);
                }
                catch (Exception e) {
                    return "Graphics device reset failed\n\n" + e;
                }
            }

            return null;
        }

        protected virtual void PaintUsingSystemDrawing (Graphics graphics, string text)
        {
            graphics.Clear(System.Drawing.Color.Black);

            using (Brush brush = new SolidBrush(System.Drawing.Color.White)) {
                using (StringFormat format = new StringFormat()) {
                    format.Alignment = StringAlignment.Center;
                    format.LineAlignment = StringAlignment.Center;

                    graphics.DrawString(text, Font, brush, ClientRectangle, format);
                }
            }
        }

        protected override void OnPaintBackground (PaintEventArgs pevent)
        {
        }

        #endregion

        #region Abstract Methods

        protected abstract void Initialize ();
        protected abstract void Draw ();

        #endregion
    }
}
