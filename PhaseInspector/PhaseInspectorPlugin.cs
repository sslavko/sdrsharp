using System;
using System.Collections.Generic;
using SDRSharp.Common;
using SDRSharp.Radio;

namespace SDRSharp.PhaseInspector
{
    public unsafe class PhaseInspectorPlugin : ISharpPlugin, IIQProcessor
    {
        ISharpControl _control;
        PhaseInspectorPanel _gui;
        PhaseInspectorDrawing _drawing;
        double _sampleRate;
        
        public string DisplayName
        {
            get
            {
                return "Phase Inspector";
            }
        }

        public bool Enabled { get; set; }

        public System.Windows.Forms.UserControl Gui
        {
            get
            {
                return _gui;
            }
        }

        public double SampleRate
        {
            set
            {
                _sampleRate = value;
            }
        }

        public void Close()
        {
            
        }

        public void Initialize(ISharpControl control)
        {
            _control = control;
            _gui = new PhaseInspectorPanel();
            _drawing = new PhaseInspectorDrawing();

            control.RegisterStreamHook(this, ProcessorType.RawIQ);
            control.RegisterFrontControl(_drawing, PluginPosition.Bottom);
        }

        public void Process(Complex* buffer, int length)
        {
            
        }
    }
}
