namespace PDF_Binder.ViewModels.VMManagement
{
    public class VMItem
    {
        #region constructors
        public VMItem(VMItem copySource
            ) : this()
        {
            if (copySource == null)
                return;

            ItemKey = copySource.ItemKey;
            Name = copySource.Name;
            Instance = copySource.Instance;
        }

        public VMItem(int itemkey
                    , string name
            , object instance = null
            ) : this()
        {
            ItemKey = itemkey;
            Name = name;

            Instance = instance;
        }

        protected VMItem()
        {
            ItemKey = -1;
            Name = null;
            Instance = null;
        }
        #endregion constructors

        #region properties
        public int ItemKey { get; private set; }

        public string Name { get; private set; }

        public object Instance { get; internal set; }
        #endregion properties
    }
}
