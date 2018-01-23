namespace MDemo.Demos.ViewModels
{
    using MDemo.ViewModels.Base;
    using MWindowInterfacesLib.Enums;
    using MWindowInterfacesLib.Interfaces;
    using MWindowInterfacesLib.MsgBox.Enums;
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    #region Helper Test Classes
    /// <summary>
    /// This class is uses to create a type safe list
    /// of enumeration members for selection in combobox.
    /// </summary>
    public class MessageImageCollection
    {
        public string Name { get; set; }
        public MsgBoxImage EnumKey { get; set; }
    }

    /// <summary>
    /// Test class to enumerate over message box buttons enumeration.
    /// </summary>
    public class MessageButtonCollection
    {
        public string Name { get; set; }
        public MsgBoxButtons EnumKey { get; set; }
    }

    /// <summary>
    /// Test class to enumerate over message box result enumeration.
    /// The <seealso cref="MsgBoxResult"/> enumeration is used to define
    /// a default button (if any).
    /// </summary>
    public class MessageResultCollection
    {
        public string Name { get; set; }
        public MsgBoxResult EnumKey { get; set; }
    }

    /// <summary>
    /// Test class to enumerate over languages (and their locale) that
    /// are supported with specific (non-English) button and tool tip strings.
    /// 
    /// The class definition is based on BCP 47 which in turn is used to
    /// set the UI and thread culture (which in turn selects the correct string resource in MsgBox assembly).
    /// </summary>
    public class LanguageCollection
    {
        public string Language { get; set; }
        public string Locale { get; set; }
        public string Name { get; set; }

        /// <summary>
        /// Get BCP47 language tag for this language
        /// See also http://en.wikipedia.org/wiki/IETF_language_tag
        /// </summary>
        public string BCP47
        {
            get
            {
                if (string.IsNullOrEmpty(this.Locale) == false)
                    return String.Format("{0}-{1}", this.Language, this.Locale);
                else
                    return String.Format("{0}", this.Language);
            }
        }

        /// <summary>
        /// Get BCP47 language tag for this language
        /// See also http://en.wikipedia.org/wiki/IETF_language_tag
        /// </summary>
        public string DisplayName
        {
            get
            {
                return String.Format("{0} {1}", this.Name, this.BCP47);
            }
        }
    }
    #endregion Helper Test Classes

    public class DemoViewModel : MDemo.ViewModels.Base.ViewModelBase
    {
        #region private fields
        private ICommand _ShowMessageDialogCommand;
        private ICommand _ShowCustomDialogCommand;
        private ICommand _ShowInputDialogCommand;
        private ICommand _ShowLoginDialogCommand;
        private ICommand _ShowMsgBoxAsyncCommand;
        private ICommand _ShowProgressDialogCommand;
        #endregion private fields

        #region messagebox test fields
        private MessageImageCollection mMessageImageSelected;
        private ObservableCollection<MessageImageCollection> mMessageImages = null;
        private ObservableCollection<LanguageCollection> mButtonLanguages;
        private LanguageCollection mButtonLanguageSelected;

        private MessageButtonCollection mMessageButtonSelected;
        private ObservableCollection<MessageButtonCollection> mMessageButtons = null;

        private MessageResultCollection mDefaultMessageButtonSelected;
        private ObservableCollection<MessageResultCollection> mDefaultMessageButtons = null;

        private string _MessageText, _CaptionText;
        private bool _ShowCopyButton;
        private string _Result;
        private string _TestMethod;

        private ICommand _TestSamplMsgBoxAsync;
        private ICommand _ShowMsgBoxCommand;
        private ICommand _TestSampleMsgBoxCommand;
        #endregion messagebox test fields

        #region internal Demo Objects
        internal CustomDialogDemos CustomDlgDemo = new CustomDialogDemos();

        internal InputDialogDemos InputDlgDemo = new InputDialogDemos();

        internal MessageDialogDemos MessageDlgDemo = new MessageDialogDemos();

        internal LoginDialogDemos LoginDlgDemo = new LoginDialogDemos();

        internal ProgressDialogDemos ProgressDlgDemo = new ProgressDialogDemos();
        #endregion internal Demo Objects

        #region constructors
        public DemoViewModel()
        {
            this.mMessageImages = new ObservableCollection<MessageImageCollection>();

            foreach (var item in Enum.GetNames(typeof(MsgBoxImage)))
            {
                MsgBoxImage enumItem;
                Enum.TryParse<MsgBoxImage>(item, out enumItem);

                this.mMessageImages.Add(new MessageImageCollection() { Name = item.ToString(), EnumKey = enumItem });
            }

            // XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
            // Test Localization
            //
            // Addd supported test languages for localization and attempt to select
            // language from current culture
            //
            // Add a MsgBox/Local string resource with the name String.<Language>-<Local>.resx
            // when adding entries here...
            this.mButtonLanguages = new ObservableCollection<LanguageCollection>();

            this.mButtonLanguages.Add(new LanguageCollection() { Language = "de", Locale = "DE", Name = "Deutsch (German)" });
            this.mButtonLanguages.Add(new LanguageCollection() { Language = "en", Locale = "US", Name = "English (English)" });
            this.mButtonLanguages.Add(new LanguageCollection() { Language = "es", Locale = "ES", Name = "Spanish (Español)" });
            this.mButtonLanguages.Add(new LanguageCollection() { Language = "fr", Locale = "FR", Name = "Français (French)" });
            this.mButtonLanguages.Add(new LanguageCollection() { Language = "hi", Locale = "", Name = "हिंदी (Hindi)" });
            this.mButtonLanguages.Add(new LanguageCollection() { Language = "it", Locale = "IT", Name = "Italiano (Italian)" });
            this.mButtonLanguages.Add(new LanguageCollection() { Language = "nl", Locale = "NL", Name = "Nederland (Dutch)" });
            this.mButtonLanguages.Add(new LanguageCollection() { Language = "zh", Locale = "Hans", Name = "中文 - 简体 (Chinese - Simplified)" });

            // Attempt to set selected language to current language as identified via BCP 47 code
            // http://en.wikipedia.org/wiki/IETF_language_tag
            string BCP47 = Thread.CurrentThread.CurrentCulture.ToString();

            try
            {
                this.mButtonLanguageSelected = this.mButtonLanguages.FirstOrDefault(lang => lang.BCP47 == BCP47);
            }
            catch
            {
            }

            this.mMessageButtons = new ObservableCollection<MessageButtonCollection>();

            foreach (var item in Enum.GetNames(typeof(MsgBoxButtons)))
            {
                MsgBoxButtons enumItem;
                Enum.TryParse<MsgBoxButtons>(item, out enumItem);

                this.mMessageButtons.Add(new MessageButtonCollection() { Name = item.ToString(), EnumKey = enumItem });
            }

            this.mDefaultMessageButtons = new ObservableCollection<MessageResultCollection>();

            foreach (var item in Enum.GetNames(typeof(MsgBoxResult)))
            {
                MsgBoxResult enumItem;
                Enum.TryParse<MsgBoxResult>(item, out enumItem);

                this.mDefaultMessageButtons.Add(new MessageResultCollection() { Name = item.ToString(), EnumKey = enumItem });
            }

            this._MessageText = "This is an important anouncement from your computer(!).";
            this._CaptionText = "An Important Anouncement";
            this._ShowCopyButton = false;
            this._Result = string.Empty;

        }
        #endregion constructors

        #region Properties
        /// <summary>
        /// Gets/sets whether static (non-async) message boxes are shown
        /// as (fixed, moveable) external message box or not.
        /// </summary>
        public StaticMsgBoxModes MsgBoxModes
        {
            get
            {
                return GetService<IContentDialogService>().DialogSettings.MsgBoxMode;
            }

            set
            {
                var service = GetService<IContentDialogService>();

                if (service.DialogSettings.MsgBoxMode != value)
                {
                    service.DialogSettings.MsgBoxMode = value;
                    RaisePropertyChanged(() => this.MsgBoxModes);
                }
            }
        }

        public ICommand ShowMessageDialogCommand
        {
            get
            {
                if (_ShowMessageDialogCommand == null)
                {
                    _ShowMessageDialogCommand = new RelayCommand<object>((p) =>
                    {
                        var pstring = p as string;

                        this.TestMethod = "Async";
                        this.MessageDlgDemo.PerformDialogCoordinatorAction(
                                                this.MessageDlgDemo.ShowDialogFromVM(this, pstring),
                                                pstring == "DISPATCHER_THREAD");
                    });
                }

                return _ShowMessageDialogCommand;
            }
        }

        public ICommand ShowProgressDialogCommand
        {
            get
            {
                if (_ShowProgressDialogCommand == null)
                {
                    _ShowProgressDialogCommand = new RelayCommand(() =>
                    {
                        this.TestMethod = "Async";
                        this.ProgressDlgDemo.ShowDialogFromVM(this, true);
                    },
                    () => { return true; });
                }

                return _ShowProgressDialogCommand;
            }
        }

        public ICommand ShowCustomDialogCommand
        {
            get
            {
                if (_ShowCustomDialogCommand == null)
                {
                    _ShowCustomDialogCommand = new RelayCommand(() =>
                    {
                        this.TestMethod = "Async";
                        CustomDlgDemo.RunCustomFromVm(this);
                    },
                    () => { return true; });
                }

                return _ShowCustomDialogCommand;
            }
        }
        #endregion Properties

        #region Commands
        public ICommand ShowInputDialogCommand
        {
            get
            {
                if (_ShowInputDialogCommand == null)
                {
                    _ShowInputDialogCommand = new RelayCommand<object>((p) =>
                    {
                        this.TestMethod = "Async";
                        this.InputDlgDemo.ShowDialogFromVM(this);
                    },
                    (p) => { return true; });
                }

                return _ShowInputDialogCommand;
            }
        }

        public ICommand ShowLoginDialogCommand
        {
            get
            {
                if (_ShowLoginDialogCommand == null)
                {
                    _ShowLoginDialogCommand = new RelayCommand(() =>
                    {
                        this.TestMethod = "Async";
                        this.LoginDlgDemo.ShowDialogFromVM(this);
                    });
                }

                return _ShowLoginDialogCommand;
            }
        }

        public ICommand ShowMsgBoxAsyncCommand
        {
            get
            {
                if (_ShowMsgBoxAsyncCommand == null)
                {
                    _ShowMsgBoxAsyncCommand = new RelayCommand<object>(async (p) =>
                    {
                        this.TestMethod = "Async";
                        var msgbox = GetService<IContentDialogService>().MsgBox;

                        var result = await msgbox.ShowAsync("This is a simple test message with a simple dialog.... also press Escape or Enter to verify the result...");
                        this.Result = result.ToString();

                        await msgbox.ShowAsync(string.Format("Result was: {0}", result));


                    },
                    (p) => { return true; });
                }

                return _ShowMsgBoxAsyncCommand;
            }
        }

        public ICommand ShowMsgBoxCommand
        {
            get
            {
                if (_ShowMsgBoxCommand == null)
                {
                    _ShowMsgBoxCommand = new RelayCommand<object>((p) =>
                    {
                        this.TestMethod = "Sync";
                        var msg = GetService<IContentDialogService>().MsgBox;

                        var result = msg.Show("This is a simple test message with a simple dialog.... also press Escape or Enter to verify the result...");

                        result = msg.Show(string.Format("Result was: {0}", result));

                        Result = result.ToString();
                    },
                    (p) => { return true; });
                }

                return _ShowMsgBoxCommand;
            }
        }

        /// <summary>
        /// Implements a test command to test the fixed 1-17... sample message box
        /// displays with different pre-defined parameters.
        /// </summary>
        public ICommand TestSampleMsgBoxAsyncCommand
        {
            get
            {
                if (this._TestSamplMsgBoxAsync == null)
                    this._TestSamplMsgBoxAsync =
                        new RelayCommand<object>((p) =>
                        {
                            this.TestMethod = "Async";

                            this.TestSamplMsgBox_ExecutedAsync(p);
                        });

                return this._TestSamplMsgBoxAsync;
            }
        }

        /// <summary>
        /// Implements a test command to test the fixed 1-17... sample message box
        /// displays with different pre-defined parameters.
        /// </summary>
        public ICommand TestSampleMsgBoxCommand
        {
            get
            {
                if (this._TestSampleMsgBoxCommand == null)
                    this._TestSampleMsgBoxCommand =
                        new RelayCommand<object>((p) =>
                        {
                            this.TestMethod = "Sync";
                            this.TestSamplMsgBox_Executed(p);
                        });

                return this._TestSampleMsgBoxCommand;
            }
        }
        #endregion Commands

        /// <summary>
        /// Select an item from the collection of MessageImages listed below.
        /// </summary>
        public MessageImageCollection MessageImageSelected
        {
            get
            {
                if (this.mMessageImageSelected == null)
                {
                    this.mMessageImageSelected =
                      this.mMessageImages.FirstOrDefault(cat => cat.EnumKey == MsgBoxImage.Default);
                }

                return this.mMessageImageSelected;
            }

            set
            {
                if (this.mMessageImageSelected != value)
                {
                    this.mMessageImageSelected = value;
                    this.RaisePropertyChanged(() => this.MessageImageSelected);
                }
            }
        }

        #region text and caption
        /// <summary>
        /// Text displayed in test message box.
        /// </summary>
        public string MessageText
        {
            get
            {
                return this._MessageText;
            }

            set
            {
                if (this._MessageText != value)
                {
                    this._MessageText = value;
                    this.RaisePropertyChanged(() => this.MessageText);
                }
            }
        }

        /// <summary>
        /// Caption displayed in test message box.
        /// </summary>
        public string CaptionText
        {
            get
            {
                return this._CaptionText;
            }

            set
            {
                if (this._CaptionText != value)
                {
                    this._CaptionText = value;
                    this.RaisePropertyChanged(() => this.CaptionText);
                }
            }
        }
        #endregion text and caption

        /// <summary>
        /// Get/set property to determine whether the message box sports a copy button or not.
        /// This button can be used to copy the dsipaly content of the message box into the windows clipboard.
        /// </summary>
        public bool ShowCopyButton
        {
            get
            {
                return this._ShowCopyButton;
            }

            set
            {
                if (this._ShowCopyButton != value)
                {
                    this._ShowCopyButton = value;
                    this.RaisePropertyChanged(() => this.ShowCopyButton);
                }
            }
        }

        /// <summary>
        /// Get/set result of message box display.
        /// </summary>
        public string Result
        {
            get
            {
                return this._Result;
            }

            set
            {
                if (this._Result != value)
                {
                    this._Result = value;
                    this.RaisePropertyChanged(() => this.Result);
                }
            }
        }

        /// <summary>
        /// Gets/sets a string property to advertise the currently used test method.
        /// </summary>
        public string TestMethod
        {
            get
            {
                return this._TestMethod;
            }

            set
            {
                if (this._TestMethod != value)
                {
                    this._TestMethod = value;
                    this.RaisePropertyChanged(() => this.TestMethod);
                }
            }
        }

        #region Buttons
        /// <summary>
        /// Select an item from the collection of MessageImages listed below.
        /// </summary>
        public MessageButtonCollection MessageButtonSelected
        {
            get
            {
                if (this.mMessageButtonSelected == null)
                {
                    this.mMessageButtonSelected =
                      this.mMessageButtons.FirstOrDefault(cat => cat.EnumKey == MsgBoxButtons.OK);
                }

                return this.mMessageButtonSelected;
            }

            set
            {
                if (this.mMessageButtonSelected != value)
                {
                    this.mMessageButtonSelected = value;
                    this.RaisePropertyChanged(() => this.MessageButtonSelected);
                }
            }
        }

        /// <summary>
        /// Provide a collection of MessageImages to select one item from (see previous property).
        /// </summary>
        public ObservableCollection<MessageButtonCollection> MessageButtons
        {
            get
            {
                return this.mMessageButtons;
            }
        }
        #endregion Buttons

        #region LanguageButtons
        /// <summary>
        /// Get/set language of message box buttons for display in localized form.
        /// </summary>
        public LanguageCollection ButtonLanguageSelected
        {
            get
            {
                return this.mButtonLanguageSelected;
            }

            set
            {
                if (this.mButtonLanguageSelected != value)
                {
                    this.mButtonLanguageSelected = value;
                    this.RaisePropertyChanged(() => this.ButtonLanguageSelected);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<LanguageCollection> ButtonLanguages
        {
            get
            {
                return this.mButtonLanguages;
            }
        }
        #endregion LanguageButtons

        #region DefaultButtons
        /// <summary>
        /// Select an item from the collection of MessageImages listed below.
        /// </summary>
        public MessageResultCollection DefaultMessageButtonSelected
        {
            get
            {
                if (this.mDefaultMessageButtonSelected == null)
                {
                    this.mDefaultMessageButtonSelected =
                      this.mDefaultMessageButtons.FirstOrDefault(cat => cat.EnumKey == MsgBoxResult.OK);
                }

                return this.mDefaultMessageButtonSelected;
            }

            set
            {
                if (this.mDefaultMessageButtonSelected != value)
                {
                    this.mDefaultMessageButtonSelected = value;
                    this.RaisePropertyChanged(() => this.DefaultMessageButtonSelected);
                }
            }
        }

        /// <summary>
        /// Provide a collection of MessageImages to select one item from (see previous property).
        /// </summary>
        public ObservableCollection<MessageResultCollection> DefaultMessageButtons
        {
            get
            {
                return this.mDefaultMessageButtons;
            }
        }
        #endregion DefaultButtons

        /// <summary>
        /// Method is executed when TestMsgBoxParameters command is invoked.
        /// </summary>
        private async void TestMsgBoxParametersAsync_Executed()
        {
            MsgBoxImage image;
            image = this.MessageImageSelected.EnumKey;

            // Set the current culture and UI culture to the currently selected languages
            Thread.CurrentThread.CurrentCulture = new CultureInfo(this.ButtonLanguageSelected.BCP47);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(this.ButtonLanguageSelected.BCP47);

            var msg = GetService<IContentDialogService>().MsgBox;

            MsgBoxResult result = await msg.ShowAsync(this.MessageText, this.CaptionText,
                                           this.mMessageButtonSelected.EnumKey,
                                           image,
                                           this.DefaultMessageButtonSelected.EnumKey,
                                           null,
                                           "", "", null,
                                           this.ShowCopyButton);

            this.Result = result.ToString();
        }

        #region 1_17 Sample Tests
        /// <summary>
        /// Convert command parameters and call backend samples method
        /// with actual parameters.
        /// </summary>
        /// <param name="p"></param>
        private void TestSamplMsgBox_ExecutedAsync(object p)
        {
            object[] param = p as object[];

            if (param != null)
            {
                if (param.Length == 2)
                {
                    Button b = param[0] as Button;
                    Window w = param[1] as Window;

                    this.ShowTestSampleMsgBoxAsync(b.Content.ToString(), w);
                }
            }
        }

        private void TestSamplMsgBox_Executed(object p)
        {
            object[] param = p as object[];

            if (param != null)
            {
                if (param.Length == 2)
                {
                    Button b = param[0] as Button;
                    Window w = param[1] as Window;

                    this.ShowTestSampleMsgBox(b.Content.ToString(), w);
                }
            }
        }

        async private void ShowTestSampleMsgBoxAsync(string sampleID, Window sampleWindow)
        {
            MsgBoxResult result = MsgBoxResult.None;
            var msg = GetService<IContentDialogService>().MsgBox;

            switch (sampleID)
            {
                case "Sample 1":
                    result = await msg.ShowAsync("This options displays a message box with only message." +
                                      "\nThis is the message box with minimal options (just an OK button and no caption).");
                    break;

                case "Sample 2":
                    result = await msg.ShowAsync("This options displays a message box with both title and message.\nA default image and OK button are displayed.",
                                      "WPF MessageBox");
                    break;

                case "Sample 3":
                    result = await msg.ShowAsync("This options displays a message box with YES, NO, CANCEL option.",
                              "WPF MessageBox",
                              MsgBoxButtons.YesNoCancel, MsgBoxImage.Question);
                    break;

                case "Sample 4":
                    {
                        Exception exp = this.CreateDemoException();

                        result = await msg.ShowAsync(exp.Message, "Unexpected Error",
                                  exp.ToString(), MsgBoxButtons.OK, MsgBoxImage.Error, MsgBoxResult.NoDefaultButton,
                                  "http://www.codeproject.com/script/Articles/MemberArticles.aspx?amid=7799028",
                                  "http://www.codeproject.com/script/Articles/MemberArticles.aspx?amid=7799028",
                                  "Click the link to report this problem:");
                    }
                    break;

                case "Sample 5":
                    result = await msg.ShowAsync("This options displays a message box with YES, NO buttons.",
                              "WPF MessageBox",
                              MsgBoxButtons.YesNo, MsgBoxImage.Question);

                    break;

                case "Sample 6":
                    result = await msg.ShowAsync("This options displays a message box with Yes, No (No as default) options.",
                             "WPF MessageBox",
                             MsgBoxButtons.YesNo, MsgBoxImage.Question, MsgBoxResult.No);
                    break;

                case "Sample 7":
                    result = await msg.ShowAsync("Are you sure? Click the hyperlink to review the get more details.",
                              "WPF MessageBox with Hyperlink",
                              MsgBoxButtons.YesNo, MsgBoxImage.Question, MsgBoxResult.Yes,
                              "http://www.codeproject.com/script/Articles/MemberArticles.aspx?amid=7799028",
                              "Code Project Articles by Dirkster99");
                    break;

                case "Sample 8":
                    result = await msg.ShowAsync("Are you sure? Click the hyperlink to review the get more details.",
                              "WPF MessageBox with Custom Hyperlink Navigation",
                              MsgBoxButtons.YesNo, MsgBoxImage.Question, MsgBoxResult.Yes,
                              "http://www.codeproject.com/script/Articles/MemberArticles.aspx?amid=7799028",
                              "Code Project Articles by Dirkster99", "Help Topic:", this.MyCustomHyperlinkNaviMethod);
                    break;

                case "Sample 9":
                    result = await msg.ShowAsync("WPF MessageBox without Copy Button (OK and Cancel [default])",
                              "Are you sure this right?",
                              MsgBoxButtons.OKCancel, MsgBoxImage.Question, MsgBoxResult.Cancel,
                              null, string.Empty, string.Empty, null, false);
                    break;

                case "Sample 10":
                    result = await msg.ShowAsync("Are you sure? Click the hyperlink to review the get more details.",
                              "WPF MessageBox without Default Button",
                              MsgBoxButtons.YesNo, MsgBoxImage.Question, MsgBoxResult.NoDefaultButton,
                              null, string.Empty, string.Empty, null, false);
                    break;

                case "Sample 11":
                    result = await msg.ShowAsync("...display a messageBox with a close button and TakeNote icon.",
                             "WPF MessageBox with a close button",
                             MsgBoxButtons.Close, MsgBoxImage.Warning);
                    break;

                case "Sample 12":
                    {
                        Exception exp = this.CreateDemoException();

                        result = await msg.ShowAsync(exp, "Unexpected Error",
                                  MsgBoxButtons.OK, MsgBoxImage.Error, MsgBoxResult.NoDefaultButton,
                                  "http://www.codeproject.com/script/Articles/MemberArticles.aspx?amid=7799028",
                                  "http://www.codeproject.com/script/Articles/MemberArticles.aspx?amid=7799028",
                                  "Please click on the link to check if this is a known problem (and report it if not):", null, true);
                    }
                    break;

                case "Sample 13":
                    {
                        Exception exp = this.CreateDemoException();

                        result = await msg.ShowAsync(exp, "Reading file 'x' was not succesful.", "Unexpected Error",
                                  MsgBoxButtons.OK, MsgBoxImage.Error, MsgBoxResult.NoDefaultButton,
                                  "http://www.codeproject.com/script/Articles/MemberArticles.aspx?amid=7799028",
                                  "http://www.codeproject.com/script/Articles/MemberArticles.aspx?amid=7799028",
                                  "Please click on the link to check if this is a known problem (and report it if not):", null, true);
                    }
                    break;

                case "Sample 14":
                    {
                        Exception exp = this.CreateDemoException();

                        result = await msg.ShowAsync(sampleWindow, "...display a messageBox with an explicit reference to the owning window.",
                                         "MessageBox with a owner reference",
                                         MsgBoxButtons.OK, MsgBoxImage.Default_OffLight );
                    }
                    break;

                case "Sample 15":
                    {
                        result = await msg.ShowAsync("...display a messageBox with a default image.",
                                 "MessageBox with default image",
                                 MsgBoxButtons.YesNoCancel, MsgBoxResult.No,
                                 "http://www.codeproject.com/script/Articles/MemberArticles.aspx?amid=7799028",
                                 "http://www.codeproject.com/script/Articles/MemberArticles.aspx?amid=7799028",
                                 "Please click on the link to check if this is a known problem (and report it if not):",
                                 null, true);
                    }
                    break;

                // Display a message box that will not close via
                // Esc, F4, or Window Close (x) button.
                case "Sample 16":
                    {
                        result = await msg.ShowAsync("...Display a message box that will not close via Esc, F4, or Window Close (x) button.",
                                 "MessageBox with default image",
                                 MsgBoxButtons.YesNoCancel,
                                 MsgBoxResult.None, false,
                                 MsgBoxResult.No,
                                 "http://www.codeproject.com/script/Articles/MemberArticles.aspx?amid=7799028",
                                 "http://www.codeproject.com/script/Articles/MemberArticles.aspx?amid=7799028",
                                 "Please click on the link to check if this is a known problem (and report it if not):",
                                 null, true);
                    }
                    break;

                //    Display a message box that WILL CLOSE
                //    via Esc, F4, or Window Close (x) button leaving with a No result.
                case "Sample 17":
                    {
                        result = await msg.ShowAsync("...Display a message box that will close via Esc, F4, or Window Close (x) butto resulting in a No Answer",
                                 "MessageBox with default image",
                                 MsgBoxButtons.YesNoCancel,
                                 MsgBoxResult.No, true,
                                 MsgBoxResult.No,
                                 "http://www.codeproject.com/script/Articles/MemberArticles.aspx?amid=7799028",
                                 "http://www.codeproject.com/script/Articles/MemberArticles.aspx?amid=7799028",
                                 "Please click on the link to check if this is a known problem (and report it if not):",
                                 null, true);
                    }
                    break;
            }

            this.Result = result.ToString();
        }

        private void ShowTestSampleMsgBox(string sampleID, Window sampleWindow)
        {
            MsgBoxResult result = MsgBoxResult.None;
            var msg = GetService<IContentDialogService>().MsgBox;

            switch (sampleID)
            {
                case "Sample 1":
                    result = msg.Show("This options displays a message box with only message." +
                                      "\nThis is the message box with minimal options (just an OK button and no caption).");
                    break;

                case "Sample 2":
                    result = msg.Show("This options displays a message box with both title and message.\nA default image and OK button are displayed.",
                                      "WPF MessageBox");
                    break;

                case "Sample 3":
                    result = msg.Show("This options displays a message box with YES, NO, CANCEL option.",
                              "WPF MessageBox",
                              MsgBoxButtons.YesNoCancel, MsgBoxImage.Question);
                    break;

                case "Sample 4":
                    {
                        Exception exp = this.CreateDemoException();

                        result = msg.Show(exp.Message, "Unexpected Error",
                                  exp.ToString(), MsgBoxButtons.OK, MsgBoxImage.Error, MsgBoxResult.NoDefaultButton,
                                  "http://www.codeproject.com/script/Articles/MemberArticles.aspx?amid=7799028",
                                  "http://www.codeproject.com/script/Articles/MemberArticles.aspx?amid=7799028",
                                  "Click the link to report this problem:");
                    }
                    break;

                case "Sample 5":
                    result = msg.Show("This options displays a message box with YES, NO buttons.",
                              "WPF MessageBox",
                              MsgBoxButtons.YesNo, MsgBoxImage.Question);

                    break;

                case "Sample 6":
                    result = msg.Show("This options displays a message box with Yes, No (No as default) options.",
                             "WPF MessageBox",
                             MsgBoxButtons.YesNo, MsgBoxImage.Question, MsgBoxResult.No);
                    break;

                case "Sample 7":
                    result = msg.Show("Are you sure? Click the hyperlink to review the get more details.",
                              "WPF MessageBox with Hyperlink",
                              MsgBoxButtons.YesNo, MsgBoxImage.Question, MsgBoxResult.Yes,
                              "http://www.codeproject.com/script/Articles/MemberArticles.aspx?amid=7799028",
                              "Code Project Articles by Dirkster99");
                    break;

                case "Sample 8":
                    result = msg.Show("Are you sure? Click the hyperlink to review the get more details.",
                              "WPF MessageBox with Custom Hyperlink Navigation",
                              MsgBoxButtons.YesNo, MsgBoxImage.Question, MsgBoxResult.Yes,
                              "http://www.codeproject.com/script/Articles/MemberArticles.aspx?amid=7799028",
                              "Code Project Articles by Dirkster99", "Help Topic:", this.MyCustomHyperlinkNaviMethod);
                    break;

                case "Sample 9":
                    result = msg.Show("WPF MessageBox without Copy Button (OK and Cancel [default])",
                              "Are you sure this right?",
                              MsgBoxButtons.OKCancel, MsgBoxImage.Question, MsgBoxResult.Cancel,
                              null, string.Empty, string.Empty, null, false);
                    break;

                case "Sample 10":
                    result = msg.Show("Are you sure? Click the hyperlink to review the get more details.",
                              "WPF MessageBox without Default Button",
                              MsgBoxButtons.YesNo, MsgBoxImage.Question, MsgBoxResult.NoDefaultButton,
                              null, string.Empty, string.Empty, null, false);
                    break;

                case "Sample 11":
                    result = msg.Show("...display a messageBox with a close button and TakeNote icon.",
                             "WPF MessageBox with a close button",
                             MsgBoxButtons.Close, MsgBoxImage.Warning);
                    break;

                case "Sample 12":
                    {
                        Exception exp = this.CreateDemoException();

                        result = msg.Show(exp, "Unexpected Error",
                                  MsgBoxButtons.OK, MsgBoxImage.Error, MsgBoxResult.NoDefaultButton,
                                  "http://www.codeproject.com/script/Articles/MemberArticles.aspx?amid=7799028",
                                  "http://www.codeproject.com/script/Articles/MemberArticles.aspx?amid=7799028",
                                  "Please click on the link to check if this is a known problem (and report it if not):", null, true);
                    }
                    break;

                case "Sample 13":
                    {
                        Exception exp = this.CreateDemoException();

                        result = msg.Show(exp, "Reading file 'x' was not succesful.", "Unexpected Error",
                                  MsgBoxButtons.OK, MsgBoxImage.Error, MsgBoxResult.NoDefaultButton,
                                  "http://www.codeproject.com/script/Articles/MemberArticles.aspx?amid=7799028",
                                  "http://www.codeproject.com/script/Articles/MemberArticles.aspx?amid=7799028",
                                  "Please click on the link to check if this is a known problem (and report it if not):", null, true);
                    }
                    break;

                case "Sample 14":
                    {
                        Exception exp = this.CreateDemoException();

                        result = msg.Show(sampleWindow, "...display a messageBox with an explicit reference to the owning window.",
                                         "MessageBox with a owner reference",
                                         MsgBoxButtons.OK, MsgBoxImage.Default_OffLight);
                    }
                    break;

                case "Sample 15":
                    {
                        result = msg.Show("...display a messageBox with a default image.",
                                 "MessageBox with default image",
                                 MsgBoxButtons.YesNoCancel, MsgBoxResult.No,
                                 "http://www.codeproject.com/script/Articles/MemberArticles.aspx?amid=7799028",
                                 "http://www.codeproject.com/script/Articles/MemberArticles.aspx?amid=7799028",
                                 "Please click on the link to check if this is a known problem (and report it if not):",
                                 null, true);
                    }
                    break;

                // Display a message box that will not close via
                // Esc, F4, or Window Close (x) button.
                case "Sample 16":
                    {
                        result = msg.Show("...Display a message box that will not close via Esc, F4, or Window Close (x) button.",
                                 "MessageBox with default image",
                                 MsgBoxButtons.YesNoCancel,
                                 MsgBoxResult.None, false,
                                 MsgBoxResult.No,
                                 "http://www.codeproject.com/script/Articles/MemberArticles.aspx?amid=7799028",
                                 "http://www.codeproject.com/script/Articles/MemberArticles.aspx?amid=7799028",
                                 "Please click on the link to check if this is a known problem (and report it if not):",
                                 null, true);
                    }
                    break;

                //    Display a message box that WILL CLOSE
                //    via Esc, F4, or Window Close (x) button leaving with a No result.
                case "Sample 17":
                    {
                        result = msg.Show("...Display a message box that will close via Esc, F4, or Window Close (x) butto resulting in a No Answer",
                                 "MessageBox with default image",
                                 MsgBoxButtons.YesNoCancel,
                                 MsgBoxResult.No, true,
                                 MsgBoxResult.No,
                                 "http://www.codeproject.com/script/Articles/MemberArticles.aspx?amid=7799028",
                                 "http://www.codeproject.com/script/Articles/MemberArticles.aspx?amid=7799028",
                                 "Please click on the link to check if this is a known problem (and report it if not):",
                                 null, true);
                    }
                    break;
            }

            this.Result = result.ToString();
        }

        /// <summary>
        /// This is just a mockup test method to test whether custom
        /// hyperlink navigation will work when using custom hyperlink
        /// navigation methods.
        /// </summary>
        /// <param name="uriTarget"></param>
        /// <returns></returns>
        private bool MyCustomHyperlinkNaviMethod(object uriTarget)
        {
            MessageBox.Show("Starting Navigation to: " + uriTarget.ToString(), "Mockup Test Info");

            string uriTargetString = uriTarget as string;

            try
            {
                if (uriTargetString != null)
                {
                    Process.Start(new ProcessStartInfo(uriTargetString));
                    return true;
                }
            }
            catch
            {
            }

            return false;
        }
        #endregion 1_17 Sample Tests

        #region DemoException
        private Exception CreateDemoException()
        {
            try
            {
                this.CreateDemoException1();
            }
            catch (Exception exp)
            {
                return exp;
            }

            return null;
        }

        private void CreateDemoException1()
        {
            try
            {
                this.CreateDemoException2();
            }
            catch (Exception exp)
            {
                throw new Exception("A sub-system failure occured.", exp);
            }
        }

        private void CreateDemoException2()
        {
            int i = 0;

            try
            {
                int x = 1 / i;
            }
            catch (Exception exp)
            {
                throw new Exception("A division by zero occured.", exp);
            }
        }
        #endregion DemoException
    }
}
