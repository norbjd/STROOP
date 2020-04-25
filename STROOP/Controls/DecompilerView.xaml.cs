﻿using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml;

namespace STROOP.Controls
{
    /// <summary>
    /// Interaction logic for DecompilerView.xaml
    /// </summary>
    public partial class DecompilerView : UserControl
    {
        static readonly IHighlightingDefinition _pythonSyntax;

        static DecompilerView()
        {
            // Load python syntax
            using (MemoryStream stream = new MemoryStream(Properties.Resources.python_xshd))
            {
                _pythonSyntax = HighlightingLoader.Load(new XmlTextReader(stream),
                HighlightingManager.Instance);
            }
        }

        public event EventHandler<string> OnFunctionClicked;

        public DecompilerView()
        {
            InitializeComponent();
        }

        public string Text
        {
            set
            {
                textEditor.Text = value;
            }
        }

        private void textEditor_Initialized(object sender, EventArgs e)
        {
            textEditor.SyntaxHighlighting = _pythonSyntax;

            var generator = new VisualRegexLinkGenerator(new Regex("fn[0-9a-fA-F]{8}"));
            generator.LinkClick += (s, link) => OnFunctionClicked?.Invoke(this, link);
            textEditor.TextArea.TextView.ElementGenerators.Add(generator);
        }
    }
}
