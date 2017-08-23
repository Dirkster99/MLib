namespace MDemo.Demos.ViewModels
{
    using MDemo.ViewModels.Base;
    using MWindowInterfacesLib.Interfaces;
    using System.Collections.Generic;
    using System.Windows.Input;
    using System.Linq;
    using System;

    /// <summary>
    /// Maps the return value of the dialog to a certain button.
    /// This class associates each caption string with an integer value.
    /// 
    /// A lst of these associations is used to populate an unlimited list of buttons
    /// in the view - this can be used to implement a dialog with any number of buttons
    /// that might be required for a given user interaction.
    /// </summary>
    public class MapButtonReturnValue : ViewModelBase
    {
        private string _Caption = null;

        public MapButtonReturnValue(int returnValue, string caption)
        {
            Caption = caption;
            ReturnValue = returnValue;
        }

        public string Caption
        {
            get
            {
                return _Caption;
            }

            set
            {
                if (_Caption != value)
                {
                    _Caption = value;
                    RaisePropertyChanged(() => this.Caption);
                }
            }
        }

        /// <summary>
        /// Gets the value that depends on the button a user clicked
        /// or whazever gesture was performed to close the dialog
        /// (if any gesture caused it to close).
        /// </summary>
        public int ReturnValue { get; private set; }
    }

    /// <summary>
    /// Lists IDs that can be used to create a lsit of buttons in an associated view. 
    /// 
    /// Extend or edit this list to add or remove buttons in the associated view.
    /// </summary>
    public enum ButtonList
    {
        AffirmativeButtonValue = DialogIntResults.OK
       ,NegativButtonValue = DialogIntResults.CANCEL
       ,FirstAuxilaryButtonValue = 4
       ,SecondAuxilaryButtonValue = 5
    };

    public class MessageDialogViewModel : MsgDemoViewModel
    {
        #region fields
        public List<MapButtonReturnValue> _MapReturnValues = null;
        private ICommand _ButtonClickedCommand = null;

        private double _MaximumBodyHeight = double.NaN;
        #endregion fields

        public MessageDialogViewModel()
        {
            _MapReturnValues = new List<MapButtonReturnValue>();

            foreach (var item in Enum.GetValues(typeof(ButtonList)))
            {
                _MapReturnValues.Add(new MapButtonReturnValue((int)item, string.Empty));
            }
        }

        #region properties
        /// <summary>
        /// Gets a command that is invoked when any of the buttons in the dialog is clicked.
        /// (The command parameters hints the actual button that has been clicked).
        /// </summary>
        public ICommand ButtonClickedCommand
        {
            get
            {
                if (this._ButtonClickedCommand == null)
                {
                    this._ButtonClickedCommand = new RelayCommand<object>((p) =>
                    {
                        var map = p as MapButtonReturnValue;

                        if (map != null)
                        {
                            base.Result = map.ReturnValue;
                            base.SendDialogStateChangedEvent();
                            base.DialogCloseResult = true;
                        }
                    });
                }

                return this._ButtonClickedCommand;
            }
        }

        /// <summary>
        /// Gets a list of configured buttons
        /// (or rather, a list of their captions and id values).
        /// </summary>
        public List<MapButtonReturnValue> MapReturnValues
        {
            get
            {
                return this._MapReturnValues;
            }
        }

        /// <summary>
        /// Gets/sets the maximum height. (Default is unlimited height
        /// <a href="http://msdn.microsoft.com/de-de/library/system.double.nan">Double.NaN</a>
        /// </summary>
        public double MaximumBodyHeight
        {
            get
            {
                return _MaximumBodyHeight;
            }

            set
            {
                if (_MaximumBodyHeight != value)
                {
                    _MaximumBodyHeight = value;
                    RaisePropertyChanged(() => this.MaximumBodyHeight);
                }
            }
        }
        #endregion properties

        #region methods
        /// <summary>
        /// Method converts a dialog integer result into a human-readable string
        /// for output in demo of the <seealso cref="MessageView"/> dialog,
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public string ConvertResultToString(int result)
        {
            var map = GetMap(result);
            if (map != null)
                return map.Caption;

            return string.Format("(Uknown result: '{0}')", result);
        }

        /// <summary>
        /// Sets a caption for a known button or returns false
        /// if the int id value is not a known configured button.
        /// </summary>
        /// <param name="result"></param>
        /// <param name="caption"></param>
        /// <returns></returns>
        public bool SetCaption(int result, string caption)
        {
            var map = GetMap(result);
            if (map != null)
            {
                map.Caption = caption;
                return true;
            }

            return false;
        }

        private MapButtonReturnValue GetMap(int value)
        {
            var retVal = this._MapReturnValues.First(item => item.ReturnValue == value);

            return retVal;
        }
        #endregion methods
    }
}
