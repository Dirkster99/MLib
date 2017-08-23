namespace PDFBinderLib
{
    /// <summary>
    /// Implements a factory class that exposes an interface to a set
    /// of classes that can be used for binding multiple PDF files.
    /// </summary>
    public class BinderFactory
    {
        public static BinderFactory Instance
        {
            get
            {
                return new BinderFactory();
            }
        }

        public IBinder BindPDF()
        {
            return new Combiner();
        }
    }
}
